﻿using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using STRINGS;

namespace KMod
{
	// Token: 0x02000D77 RID: 3447
	public class Steam : IDistributionPlatform, SteamUGCService.IClient
	{
		// Token: 0x06006B67 RID: 27495 RVA: 0x002A0770 File Offset: 0x0029E970
		private Mod MakeMod(SteamUGCService.Mod subscribed)
		{
			if (subscribed == null)
			{
				return null;
			}
			if ((SteamUGC.GetItemState(subscribed.fileId) & 4U) == 0U)
			{
				return null;
			}
			string steamModID = subscribed.fileId.m_PublishedFileId.ToString();
			Label label = new Label
			{
				id = steamModID,
				distribution_platform = Label.DistributionPlatform.Steam,
				version = (long)subscribed.lastUpdateTime,
				title = subscribed.title
			};
			ulong num;
			string filename;
			uint num2;
			if (!SteamUGC.GetItemInstallInfo(subscribed.fileId, out num, out filename, 1024U, out num2))
			{
				Global.Instance.modManager.events.Add(new Event
				{
					event_type = EventType.InstallInfoInaccessible,
					mod = label
				});
				return null;
			}
			ZipFile zipFile = new ZipFile(filename);
			KModHeader header = KModUtil.GetHeader(zipFile, label.defaultStaticID, subscribed.title, subscribed.description, false);
			label.title = header.title;
			return new Mod(label, header.staticID, header.description, zipFile, UI.FRONTEND.MODS.TOOLTIPS.MANAGE_STEAM_SUBSCRIPTION, delegate()
			{
				App.OpenWebURL("https://steamcommunity.com/sharedfiles/filedetails/?id=" + steamModID);
			});
		}

		// Token: 0x06006B68 RID: 27496 RVA: 0x002A089C File Offset: 0x0029EA9C
		public void UpdateMods(IEnumerable<PublishedFileId_t> added, IEnumerable<PublishedFileId_t> updated, IEnumerable<PublishedFileId_t> removed, IEnumerable<SteamUGCService.Mod> loaded_previews)
		{
			foreach (PublishedFileId_t publishedFileId_t in added)
			{
				SteamUGCService.Mod mod = SteamUGCService.Instance.FindMod(publishedFileId_t);
				if (mod == null)
				{
					string details = string.Format("Mod Steam PublishedFileId_t {0}", publishedFileId_t);
					KCrashReporter.ReportDevNotification(string.Format("SteamUGCService just told us ADDED id {0} was valid!", publishedFileId_t), Environment.StackTrace, details, false);
				}
				else
				{
					Mod mod2 = this.MakeMod(mod);
					if (mod2 != null)
					{
						Global.Instance.modManager.Subscribe(mod2, this);
					}
				}
			}
			foreach (PublishedFileId_t publishedFileId_t2 in updated)
			{
				SteamUGCService.Mod mod3 = SteamUGCService.Instance.FindMod(publishedFileId_t2);
				if (mod3 == null)
				{
					string details2 = string.Format("Mod Steam PublishedFileId_t {0}", publishedFileId_t2.m_PublishedFileId);
					KCrashReporter.ReportDevNotification("SteamUGCService just told us UPDATED id was valid!", Environment.StackTrace, details2, false);
				}
				else
				{
					Mod mod4 = this.MakeMod(mod3);
					if (mod4 != null)
					{
						Global.Instance.modManager.Update(mod4, this);
					}
				}
			}
			foreach (PublishedFileId_t publishedFileId_t3 in removed)
			{
				Manager modManager = Global.Instance.modManager;
				Label label = default(Label);
				ulong publishedFileId = publishedFileId_t3.m_PublishedFileId;
				label.id = publishedFileId.ToString();
				label.distribution_platform = Label.DistributionPlatform.Steam;
				modManager.Unsubscribe(label, this);
			}
			if (added.Count<PublishedFileId_t>() != 0)
			{
				Global.Instance.modManager.Sanitize(null);
				return;
			}
			Global.Instance.modManager.Report(null);
		}
	}
}
