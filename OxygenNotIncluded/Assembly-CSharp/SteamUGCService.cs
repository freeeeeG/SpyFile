﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zip;
using Steamworks;
using UnityEngine;

// Token: 0x02000A3C RID: 2620
public class SteamUGCService : MonoBehaviour
{
	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06004EE4 RID: 20196 RVA: 0x001BCC16 File Offset: 0x001BAE16
	public static SteamUGCService Instance
	{
		get
		{
			return SteamUGCService.instance;
		}
	}

	// Token: 0x06004EE5 RID: 20197 RVA: 0x001BCC20 File Offset: 0x001BAE20
	private SteamUGCService()
	{
		this.on_subscribed = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(new Callback<RemoteStoragePublishedFileSubscribed_t>.DispatchDelegate(this.OnItemSubscribed));
		this.on_unsubscribed = Callback<RemoteStoragePublishedFileUnsubscribed_t>.Create(new Callback<RemoteStoragePublishedFileUnsubscribed_t>.DispatchDelegate(this.OnItemUnsubscribed));
		this.on_updated = Callback<RemoteStoragePublishedFileUpdated_t>.Create(new Callback<RemoteStoragePublishedFileUpdated_t>.DispatchDelegate(this.OnItemUpdated));
		this.on_query_completed = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.OnSteamUGCQueryDetailsCompleted));
		this.mods = new List<SteamUGCService.Mod>();
	}

	// Token: 0x06004EE6 RID: 20198 RVA: 0x001BCD08 File Offset: 0x001BAF08
	public static void Initialize()
	{
		if (SteamUGCService.instance != null)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("/SteamManager");
		SteamUGCService.instance = gameObject.GetComponent<SteamUGCService>();
		if (SteamUGCService.instance == null)
		{
			SteamUGCService.instance = gameObject.AddComponent<SteamUGCService>();
		}
	}

	// Token: 0x06004EE7 RID: 20199 RVA: 0x001BCD54 File Offset: 0x001BAF54
	public void AddClient(SteamUGCService.IClient client)
	{
		this.clients.Add(client);
		ListPool<PublishedFileId_t, SteamUGCService>.PooledList pooledList = ListPool<PublishedFileId_t, SteamUGCService>.Allocate();
		foreach (SteamUGCService.Mod mod in this.mods)
		{
			pooledList.Add(mod.fileId);
		}
		client.UpdateMods(pooledList, Enumerable.Empty<PublishedFileId_t>(), Enumerable.Empty<PublishedFileId_t>(), Enumerable.Empty<SteamUGCService.Mod>());
		pooledList.Recycle();
	}

	// Token: 0x06004EE8 RID: 20200 RVA: 0x001BCDDC File Offset: 0x001BAFDC
	public void RemoveClient(SteamUGCService.IClient client)
	{
		this.clients.Remove(client);
	}

	// Token: 0x06004EE9 RID: 20201 RVA: 0x001BCDEC File Offset: 0x001BAFEC
	public void Awake()
	{
		global::Debug.Assert(SteamUGCService.instance == null);
		SteamUGCService.instance = this;
		uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
		if (numSubscribedItems != 0U)
		{
			PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
			SteamUGC.GetSubscribedItems(array, numSubscribedItems);
			this.downloads.UnionWith(array);
		}
	}

	// Token: 0x06004EEA RID: 20202 RVA: 0x001BCE34 File Offset: 0x001BB034
	public bool IsSubscribed(PublishedFileId_t item)
	{
		return this.downloads.Contains(item) || this.proxies.Contains(item) || this.queries.Contains(item) || this.publishes.Any((SteamUGCDetails_t candidate) => candidate.m_nPublishedFileId == item) || this.mods.Exists((SteamUGCService.Mod candidate) => candidate.fileId == item);
	}

	// Token: 0x06004EEB RID: 20203 RVA: 0x001BCEBC File Offset: 0x001BB0BC
	public SteamUGCService.Mod FindMod(PublishedFileId_t item)
	{
		return this.mods.Find((SteamUGCService.Mod candidate) => candidate.fileId == item);
	}

	// Token: 0x06004EEC RID: 20204 RVA: 0x001BCEED File Offset: 0x001BB0ED
	private void OnDestroy()
	{
		global::Debug.Assert(SteamUGCService.instance == this);
		SteamUGCService.instance = null;
	}

	// Token: 0x06004EED RID: 20205 RVA: 0x001BCF08 File Offset: 0x001BB108
	private Texture2D LoadPreviewImage(SteamUGCDetails_t details)
	{
		byte[] array = null;
		if (details.m_hPreviewFile != UGCHandle_t.Invalid)
		{
			SteamRemoteStorage.UGCDownload(details.m_hPreviewFile, 0U);
			array = new byte[details.m_nPreviewFileSize];
			if (SteamRemoteStorage.UGCRead(details.m_hPreviewFile, array, details.m_nPreviewFileSize, 0U, EUGCReadAction.k_EUGCRead_ContinueReadingUntilFinished) != details.m_nPreviewFileSize)
			{
				if (this.retry_counts[details.m_nPublishedFileId] % 100 == 0)
				{
					global::Debug.LogFormat("Steam: Preview image load failed", Array.Empty<object>());
				}
				array = null;
			}
		}
		if (array == null)
		{
			System.DateTime dateTime;
			array = SteamUGCService.GetBytesFromZip(details.m_nPublishedFileId, SteamUGCService.previewFileNames, out dateTime, false);
		}
		Texture2D texture2D = null;
		if (array != null)
		{
			texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(array);
		}
		else
		{
			Dictionary<PublishedFileId_t, int> dictionary = this.retry_counts;
			PublishedFileId_t nPublishedFileId = details.m_nPublishedFileId;
			dictionary[nPublishedFileId]++;
		}
		return texture2D;
	}

	// Token: 0x06004EEE RID: 20206 RVA: 0x001BCFD8 File Offset: 0x001BB1D8
	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (Game.Instance != null)
		{
			return;
		}
		this.downloads.ExceptWith(this.removals);
		this.publishes.RemoveWhere((SteamUGCDetails_t publish) => this.removals.Contains(publish.m_nPublishedFileId));
		this.previews.RemoveWhere((SteamUGCDetails_t publish) => this.removals.Contains(publish.m_nPublishedFileId));
		this.proxies.ExceptWith(this.removals);
		HashSetPool<SteamUGCService.Mod, SteamUGCService>.PooledHashSet loaded_previews = HashSetPool<SteamUGCService.Mod, SteamUGCService>.Allocate();
		HashSetPool<PublishedFileId_t, SteamUGCService>.PooledHashSet cancelled_previews = HashSetPool<PublishedFileId_t, SteamUGCService>.Allocate();
		SteamUGCService.Mod mod;
		foreach (SteamUGCDetails_t steamUGCDetails_t in this.previews)
		{
			mod = this.FindMod(steamUGCDetails_t.m_nPublishedFileId);
			DebugUtil.DevAssert(mod != null, "expect mod with pending preview to be published", null);
			mod.previewImage = this.LoadPreviewImage(steamUGCDetails_t);
			if (mod.previewImage != null)
			{
				loaded_previews.Add(mod);
			}
			else if (1000 < this.retry_counts[steamUGCDetails_t.m_nPublishedFileId])
			{
				cancelled_previews.Add(mod.fileId);
			}
		}
		this.previews.RemoveWhere((SteamUGCDetails_t publish) => loaded_previews.Any((SteamUGCService.Mod mod) => mod.fileId == publish.m_nPublishedFileId) || cancelled_previews.Contains(publish.m_nPublishedFileId));
		cancelled_previews.Recycle();
		ListPool<SteamUGCService.Mod, SteamUGCService>.PooledList pooledList = ListPool<SteamUGCService.Mod, SteamUGCService>.Allocate();
		HashSetPool<PublishedFileId_t, SteamUGCService>.PooledHashSet published = HashSetPool<PublishedFileId_t, SteamUGCService>.Allocate();
		foreach (SteamUGCDetails_t steamUGCDetails_t2 in this.publishes)
		{
			if ((SteamUGC.GetItemState(steamUGCDetails_t2.m_nPublishedFileId) & 48U) == 0U)
			{
				global::Debug.LogFormat("Steam: updating info for mod {0}", new object[]
				{
					steamUGCDetails_t2.m_rgchTitle
				});
				SteamUGCService.Mod mod2 = new SteamUGCService.Mod(steamUGCDetails_t2, this.LoadPreviewImage(steamUGCDetails_t2));
				pooledList.Add(mod2);
				if (steamUGCDetails_t2.m_hPreviewFile != UGCHandle_t.Invalid && mod2.previewImage == null)
				{
					this.previews.Add(steamUGCDetails_t2);
				}
				published.Add(steamUGCDetails_t2.m_nPublishedFileId);
			}
		}
		this.publishes.RemoveWhere((SteamUGCDetails_t publish) => published.Contains(publish.m_nPublishedFileId));
		published.Recycle();
		foreach (PublishedFileId_t publishedFileId_t in this.proxies)
		{
			global::Debug.LogFormat("Steam: proxy mod {0}", new object[]
			{
				publishedFileId_t
			});
			pooledList.Add(new SteamUGCService.Mod(publishedFileId_t));
		}
		this.proxies.Clear();
		ListPool<PublishedFileId_t, SteamUGCService>.PooledList pooledList2 = ListPool<PublishedFileId_t, SteamUGCService>.Allocate();
		ListPool<PublishedFileId_t, SteamUGCService>.PooledList pooledList3 = ListPool<PublishedFileId_t, SteamUGCService>.Allocate();
		using (List<SteamUGCService.Mod>.Enumerator enumerator3 = pooledList.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				SteamUGCService.Mod mod = enumerator3.Current;
				int num = this.mods.FindIndex((SteamUGCService.Mod candidate) => candidate.fileId == mod.fileId);
				if (num == -1)
				{
					this.mods.Add(mod);
					pooledList2.Add(mod.fileId);
				}
				else
				{
					this.mods[num] = mod;
					pooledList3.Add(mod.fileId);
				}
			}
		}
		pooledList.Recycle();
		bool flag = this.details_query == UGCQueryHandle_t.Invalid;
		if (pooledList2.Count != 0 || pooledList3.Count != 0 || (flag && this.removals.Count != 0) || loaded_previews.Count != 0)
		{
			foreach (SteamUGCService.IClient client in this.clients)
			{
				IEnumerable<PublishedFileId_t> added = pooledList2;
				IEnumerable<PublishedFileId_t> updated = pooledList3;
				IEnumerable<PublishedFileId_t> removed;
				if (!flag)
				{
					removed = Enumerable.Empty<PublishedFileId_t>();
				}
				else
				{
					IEnumerable<PublishedFileId_t> enumerable = this.removals;
					removed = enumerable;
				}
				client.UpdateMods(added, updated, removed, loaded_previews);
			}
		}
		pooledList2.Recycle();
		foreach (PublishedFileId_t publishedFileId_t2 in pooledList3)
		{
			if ((SteamUGC.GetItemState(publishedFileId_t2) & 4U) != 0U)
			{
				string details = string.Format("Mod Steam PublishedFileId_t {0}", publishedFileId_t2.m_PublishedFileId);
				KCrashReporter.ReportDevNotification("SteamUGCService updated Mod has not finished updating!", Environment.StackTrace, details, false);
			}
		}
		pooledList3.Recycle();
		loaded_previews.Recycle();
		if (flag)
		{
			using (HashSet<PublishedFileId_t>.Enumerator enumerator2 = this.removals.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					PublishedFileId_t removal = enumerator2.Current;
					this.mods.RemoveAll((SteamUGCService.Mod candidate) => candidate.fileId == removal);
				}
			}
			this.removals.Clear();
		}
		foreach (PublishedFileId_t nPublishedFileID in this.downloads)
		{
			EItemState itemState = (EItemState)SteamUGC.GetItemState(nPublishedFileID);
			if (((itemState & EItemState.k_EItemStateInstalled) == EItemState.k_EItemStateNone || (itemState & EItemState.k_EItemStateNeedsUpdate) != EItemState.k_EItemStateNone) && (itemState & (EItemState.k_EItemStateDownloading | EItemState.k_EItemStateDownloadPending)) == EItemState.k_EItemStateNone)
			{
				SteamUGC.DownloadItem(nPublishedFileID, false);
			}
		}
		if (this.details_query == UGCQueryHandle_t.Invalid)
		{
			this.queries.UnionWith(this.downloads);
			this.downloads.Clear();
			if (this.queries.Count != 0)
			{
				this.details_query = SteamUGC.CreateQueryUGCDetailsRequest(this.queries.ToArray<PublishedFileId_t>(), (uint)this.queries.Count);
				SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(this.details_query);
				this.on_query_completed.Set(hAPICall, null);
			}
		}
	}

	// Token: 0x06004EEF RID: 20207 RVA: 0x001BD600 File Offset: 0x001BB800
	private void OnSteamUGCQueryDetailsCompleted(SteamUGCQueryCompleted_t pCallback, bool bIOFailure)
	{
		EResult eResult = pCallback.m_eResult;
		if (eResult != EResult.k_EResultOK)
		{
			if (eResult != EResult.k_EResultBusy)
			{
				string[] array = new string[10];
				array[0] = "Steam: [OnSteamUGCQueryDetailsCompleted] - handle: ";
				int num = 1;
				UGCQueryHandle_t handle = pCallback.m_handle;
				array[num] = handle.ToString();
				array[2] = " -- Result: ";
				array[3] = pCallback.m_eResult.ToString();
				array[4] = " -- NUm results: ";
				array[5] = pCallback.m_unNumResultsReturned.ToString();
				array[6] = " --Total Matching: ";
				array[7] = pCallback.m_unTotalMatchingResults.ToString();
				array[8] = " -- cached: ";
				array[9] = pCallback.m_bCachedData.ToString();
				global::Debug.Log(string.Concat(array));
				HashSet<PublishedFileId_t> hashSet = this.proxies;
				this.proxies = this.queries;
				this.queries = hashSet;
			}
			else
			{
				string[] array2 = new string[5];
				array2[0] = "Steam: [OnSteamUGCQueryDetailsCompleted] - handle: ";
				int num2 = 1;
				UGCQueryHandle_t handle = pCallback.m_handle;
				array2[num2] = handle.ToString();
				array2[2] = " -- Result: ";
				array2[3] = pCallback.m_eResult.ToString();
				array2[4] = " Resending";
				global::Debug.Log(string.Concat(array2));
			}
		}
		else
		{
			for (uint num3 = 0U; num3 < pCallback.m_unNumResultsReturned; num3 += 1U)
			{
				SteamUGCDetails_t steamUGCDetails_t = default(SteamUGCDetails_t);
				if (SteamUGC.GetQueryUGCResult(this.details_query, num3, out steamUGCDetails_t))
				{
					if (!this.removals.Contains(steamUGCDetails_t.m_nPublishedFileId))
					{
						this.publishes.Add(steamUGCDetails_t);
						this.retry_counts[steamUGCDetails_t.m_nPublishedFileId] = 0;
					}
					this.queries.Remove(steamUGCDetails_t.m_nPublishedFileId);
				}
				else
				{
					KCrashReporter.ReportDevNotification("SteamUGCService.GetQueryUGCResult details_query is an invalid handle!", Environment.StackTrace, "", false);
				}
			}
		}
		SteamUGC.ReleaseQueryUGCRequest(this.details_query);
		this.details_query = UGCQueryHandle_t.Invalid;
	}

	// Token: 0x06004EF0 RID: 20208 RVA: 0x001BD7CD File Offset: 0x001BB9CD
	private void OnItemSubscribed(RemoteStoragePublishedFileSubscribed_t pCallback)
	{
		this.downloads.Add(pCallback.m_nPublishedFileId);
	}

	// Token: 0x06004EF1 RID: 20209 RVA: 0x001BD7E1 File Offset: 0x001BB9E1
	private void OnItemUpdated(RemoteStoragePublishedFileUpdated_t pCallback)
	{
		this.downloads.Add(pCallback.m_nPublishedFileId);
	}

	// Token: 0x06004EF2 RID: 20210 RVA: 0x001BD7F5 File Offset: 0x001BB9F5
	private void OnItemUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t pCallback)
	{
		this.removals.Add(pCallback.m_nPublishedFileId);
	}

	// Token: 0x06004EF3 RID: 20211 RVA: 0x001BD80C File Offset: 0x001BBA0C
	public static byte[] GetBytesFromZip(PublishedFileId_t item, string[] filesToExtract, out System.DateTime lastModified, bool getFirstMatch = false)
	{
		byte[] result = null;
		lastModified = System.DateTime.MinValue;
		ulong num;
		string text;
		uint num2;
		SteamUGC.GetItemInstallInfo(item, out num, out text, 1024U, out num2);
		try
		{
			lastModified = File.GetLastWriteTimeUtc(text);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ZipFile zipFile = ZipFile.Read(text))
				{
					ZipEntry zipEntry = null;
					foreach (string text2 in filesToExtract)
					{
						if (text2.Length > 4)
						{
							if (zipFile.ContainsEntry(text2))
							{
								zipEntry = zipFile[text2];
							}
						}
						else
						{
							foreach (ZipEntry zipEntry2 in zipFile.Entries)
							{
								if (zipEntry2.FileName.EndsWith(text2))
								{
									zipEntry = zipEntry2;
									break;
								}
							}
						}
						if (zipEntry != null)
						{
							break;
						}
					}
					if (zipEntry != null)
					{
						zipEntry.Extract(memoryStream);
						memoryStream.Flush();
						result = memoryStream.ToArray();
					}
				}
			}
		}
		catch (Exception)
		{
		}
		return result;
	}

	// Token: 0x0400334C RID: 13132
	private UGCQueryHandle_t details_query = UGCQueryHandle_t.Invalid;

	// Token: 0x0400334D RID: 13133
	private Callback<RemoteStoragePublishedFileSubscribed_t> on_subscribed;

	// Token: 0x0400334E RID: 13134
	private Callback<RemoteStoragePublishedFileUpdated_t> on_updated;

	// Token: 0x0400334F RID: 13135
	private Callback<RemoteStoragePublishedFileUnsubscribed_t> on_unsubscribed;

	// Token: 0x04003350 RID: 13136
	private CallResult<SteamUGCQueryCompleted_t> on_query_completed;

	// Token: 0x04003351 RID: 13137
	private HashSet<PublishedFileId_t> downloads = new HashSet<PublishedFileId_t>();

	// Token: 0x04003352 RID: 13138
	private HashSet<PublishedFileId_t> queries = new HashSet<PublishedFileId_t>();

	// Token: 0x04003353 RID: 13139
	private HashSet<PublishedFileId_t> proxies = new HashSet<PublishedFileId_t>();

	// Token: 0x04003354 RID: 13140
	private HashSet<SteamUGCDetails_t> publishes = new HashSet<SteamUGCDetails_t>();

	// Token: 0x04003355 RID: 13141
	private HashSet<PublishedFileId_t> removals = new HashSet<PublishedFileId_t>();

	// Token: 0x04003356 RID: 13142
	private HashSet<SteamUGCDetails_t> previews = new HashSet<SteamUGCDetails_t>();

	// Token: 0x04003357 RID: 13143
	private List<SteamUGCService.Mod> mods = new List<SteamUGCService.Mod>();

	// Token: 0x04003358 RID: 13144
	private Dictionary<PublishedFileId_t, int> retry_counts = new Dictionary<PublishedFileId_t, int>();

	// Token: 0x04003359 RID: 13145
	private static readonly string[] previewFileNames = new string[]
	{
		"preview.png",
		"Preview.png",
		"PREVIEW.png",
		".png",
		".jpg"
	};

	// Token: 0x0400335A RID: 13146
	private List<SteamUGCService.IClient> clients = new List<SteamUGCService.IClient>();

	// Token: 0x0400335B RID: 13147
	private static SteamUGCService instance;

	// Token: 0x0400335C RID: 13148
	private const EItemState DOWNLOADING_MASK = EItemState.k_EItemStateDownloading | EItemState.k_EItemStateDownloadPending;

	// Token: 0x0400335D RID: 13149
	private const int RETRY_THRESHOLD = 1000;

	// Token: 0x020018C8 RID: 6344
	public class Mod
	{
		// Token: 0x060092DE RID: 37598 RVA: 0x0032D534 File Offset: 0x0032B734
		public Mod(SteamUGCDetails_t item, Texture2D previewImage)
		{
			this.title = item.m_rgchTitle;
			this.description = item.m_rgchDescription;
			this.fileId = item.m_nPublishedFileId;
			this.lastUpdateTime = (ulong)item.m_rtimeUpdated;
			this.tags = new List<string>(item.m_rgchTags.Split(new char[]
			{
				','
			}));
			this.previewImage = previewImage;
		}

		// Token: 0x060092DF RID: 37599 RVA: 0x0032D5A3 File Offset: 0x0032B7A3
		public Mod(PublishedFileId_t id)
		{
			this.title = string.Empty;
			this.description = string.Empty;
			this.fileId = id;
			this.lastUpdateTime = 0UL;
			this.tags = new List<string>();
			this.previewImage = null;
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x060092E0 RID: 37600 RVA: 0x0032D5E2 File Offset: 0x0032B7E2
		// (set) Token: 0x060092E1 RID: 37601 RVA: 0x0032D5EA File Offset: 0x0032B7EA
		public string title { get; private set; }

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x060092E2 RID: 37602 RVA: 0x0032D5F3 File Offset: 0x0032B7F3
		// (set) Token: 0x060092E3 RID: 37603 RVA: 0x0032D5FB File Offset: 0x0032B7FB
		public string description { get; private set; }

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x060092E4 RID: 37604 RVA: 0x0032D604 File Offset: 0x0032B804
		// (set) Token: 0x060092E5 RID: 37605 RVA: 0x0032D60C File Offset: 0x0032B80C
		public PublishedFileId_t fileId { get; private set; }

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x060092E6 RID: 37606 RVA: 0x0032D615 File Offset: 0x0032B815
		// (set) Token: 0x060092E7 RID: 37607 RVA: 0x0032D61D File Offset: 0x0032B81D
		public ulong lastUpdateTime { get; private set; }

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x060092E8 RID: 37608 RVA: 0x0032D626 File Offset: 0x0032B826
		// (set) Token: 0x060092E9 RID: 37609 RVA: 0x0032D62E File Offset: 0x0032B82E
		public List<string> tags { get; private set; }

		// Token: 0x040072FC RID: 29436
		public Texture2D previewImage;
	}

	// Token: 0x020018C9 RID: 6345
	public interface IClient
	{
		// Token: 0x060092EA RID: 37610
		void UpdateMods(IEnumerable<PublishedFileId_t> added, IEnumerable<PublishedFileId_t> updated, IEnumerable<PublishedFileId_t> removed, IEnumerable<SteamUGCService.Mod> loaded_previews);
	}
}
