using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Klei;
using Newtonsoft.Json;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000D7F RID: 3455
	[JsonObject(MemberSerialization.OptIn)]
	[DebuggerDisplay("{title}")]
	public class Mod
	{
		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06006B8B RID: 27531 RVA: 0x002A1211 File Offset: 0x0029F411
		// (set) Token: 0x06006B8C RID: 27532 RVA: 0x002A1219 File Offset: 0x0029F419
		public Content available_content { get; private set; }

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06006B8D RID: 27533 RVA: 0x002A1222 File Offset: 0x0029F422
		// (set) Token: 0x06006B8E RID: 27534 RVA: 0x002A122A File Offset: 0x0029F42A
		[JsonProperty]
		public string staticID { get; private set; }

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06006B8F RID: 27535 RVA: 0x002A1233 File Offset: 0x0029F433
		// (set) Token: 0x06006B90 RID: 27536 RVA: 0x002A123B File Offset: 0x0029F43B
		public LocString manage_tooltip { get; private set; }

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06006B91 RID: 27537 RVA: 0x002A1244 File Offset: 0x0029F444
		// (set) Token: 0x06006B92 RID: 27538 RVA: 0x002A124C File Offset: 0x0029F44C
		public System.Action on_managed { get; private set; }

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06006B93 RID: 27539 RVA: 0x002A1255 File Offset: 0x0029F455
		public bool is_managed
		{
			get
			{
				return this.manage_tooltip != null;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06006B94 RID: 27540 RVA: 0x002A1260 File Offset: 0x0029F460
		public string title
		{
			get
			{
				return this.label.title;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06006B95 RID: 27541 RVA: 0x002A126D File Offset: 0x0029F46D
		// (set) Token: 0x06006B96 RID: 27542 RVA: 0x002A1275 File Offset: 0x0029F475
		public string description { get; private set; }

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06006B97 RID: 27543 RVA: 0x002A127E File Offset: 0x0029F47E
		// (set) Token: 0x06006B98 RID: 27544 RVA: 0x002A1286 File Offset: 0x0029F486
		public Content loaded_content { get; private set; }

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06006B99 RID: 27545 RVA: 0x002A128F File Offset: 0x0029F48F
		// (set) Token: 0x06006B9A RID: 27546 RVA: 0x002A1297 File Offset: 0x0029F497
		public IFileSource file_source
		{
			get
			{
				return this._fileSource;
			}
			set
			{
				if (this._fileSource != null)
				{
					this._fileSource.Dispose();
				}
				this._fileSource = value;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06006B9B RID: 27547 RVA: 0x002A12B3 File Offset: 0x0029F4B3
		// (set) Token: 0x06006B9C RID: 27548 RVA: 0x002A12BB File Offset: 0x0029F4BB
		public bool DevModCrashTriggered { get; private set; }

		// Token: 0x06006B9D RID: 27549 RVA: 0x002A12C4 File Offset: 0x0029F4C4
		[JsonConstructor]
		public Mod()
		{
		}

		// Token: 0x06006B9E RID: 27550 RVA: 0x002A12D8 File Offset: 0x0029F4D8
		public void CopyPersistentDataTo(Mod other_mod)
		{
			other_mod.status = this.status;
			other_mod.enabledForDlc = ((this.enabledForDlc != null) ? new List<string>(this.enabledForDlc) : new List<string>());
			other_mod.crash_count = this.crash_count;
			other_mod.loaded_content = this.loaded_content;
			other_mod.loaded_mod_data = this.loaded_mod_data;
			other_mod.reinstall_path = this.reinstall_path;
		}

		// Token: 0x06006B9F RID: 27551 RVA: 0x002A1344 File Offset: 0x0029F544
		public Mod(Label label, string staticID, string description, IFileSource file_source, LocString manage_tooltip, System.Action on_managed)
		{
			this.label = label;
			this.status = Mod.Status.NotInstalled;
			this.staticID = staticID;
			this.description = description;
			this.file_source = file_source;
			this.manage_tooltip = manage_tooltip;
			this.on_managed = on_managed;
			this.loaded_content = (Content)0;
			this.available_content = (Content)0;
			this.ScanContent();
		}

		// Token: 0x06006BA0 RID: 27552 RVA: 0x002A13AA File Offset: 0x0029F5AA
		public bool IsEnabledForActiveDlc()
		{
			return this.IsEnabledForDlc(DlcManager.GetHighestActiveDlcId());
		}

		// Token: 0x06006BA1 RID: 27553 RVA: 0x002A13B7 File Offset: 0x0029F5B7
		public bool IsEnabledForDlc(string dlcId)
		{
			return this.enabledForDlc != null && this.enabledForDlc.Contains(dlcId);
		}

		// Token: 0x06006BA2 RID: 27554 RVA: 0x002A13CF File Offset: 0x0029F5CF
		public void SetEnabledForActiveDlc(bool enabled)
		{
			this.SetEnabledForDlc(DlcManager.GetHighestActiveDlcId(), enabled);
		}

		// Token: 0x06006BA3 RID: 27555 RVA: 0x002A13E0 File Offset: 0x0029F5E0
		public void SetEnabledForDlc(string dlcId, bool set_enabled)
		{
			if (this.enabledForDlc == null)
			{
				this.enabledForDlc = new List<string>();
			}
			bool flag = this.enabledForDlc.Contains(dlcId);
			if (set_enabled && !flag)
			{
				this.enabledForDlc.Add(dlcId);
				return;
			}
			if (!set_enabled && flag)
			{
				this.enabledForDlc.Remove(dlcId);
			}
		}

		// Token: 0x06006BA4 RID: 27556 RVA: 0x002A1438 File Offset: 0x0029F638
		public void ScanContent()
		{
			this.ModDevLog(string.Format("{0} ({1}): Setting up mod.", this.label, this.label.id));
			this.available_content = (Content)0;
			if (this.file_source == null)
			{
				if (this.label.id.EndsWith(".zip"))
				{
					DebugUtil.DevAssert(false, "Does this actually get used ever?", null);
					this.file_source = new ZipFile(this.label.install_path);
				}
				else
				{
					this.file_source = new Directory(this.label.install_path);
				}
			}
			if (!this.file_source.Exists())
			{
				global::Debug.LogWarning(string.Format("{0}: File source does not appear to be valid, skipping. ({1})", this.label, this.label.install_path));
				return;
			}
			KModHeader header = KModUtil.GetHeader(this.file_source, this.label.defaultStaticID, this.label.title, this.description, this.IsDev);
			if (this.label.title != header.title)
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"\t",
					this.label.title,
					" has a mod.yaml with the title `",
					header.title,
					"`, using that from now on."
				}));
			}
			if (this.label.defaultStaticID != header.staticID)
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"\t",
					this.label.title,
					" has a mod.yaml with a staticID `",
					header.staticID,
					"`, using that from now on."
				}));
			}
			this.label.title = header.title;
			this.staticID = header.staticID;
			this.description = header.description;
			Mod.ArchivedVersion mostSuitableArchive = this.GetMostSuitableArchive();
			if (mostSuitableArchive == null)
			{
				global::Debug.LogWarning(string.Format("{0}: No archive supports this game version, skipping content.", this.label));
				this.contentCompatability = ModContentCompatability.DoesntSupportDLCConfig;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			this.packagedModInfo = mostSuitableArchive.info;
			Content content;
			this.ScanContentFromSource(mostSuitableArchive.relativePath, out content);
			if (content == (Content)0)
			{
				global::Debug.LogWarning(string.Format("{0}: No supported content for mod, skipping content.", this.label));
				this.contentCompatability = ModContentCompatability.NoContent;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			bool flag = mostSuitableArchive.info.APIVersion == 2;
			if ((content & Content.DLL) != (Content)0 && !flag)
			{
				global::Debug.LogWarning(string.Format("{0}: DLLs found but not using the correct API version.", this.label));
				this.contentCompatability = ModContentCompatability.OldAPI;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			this.contentCompatability = ModContentCompatability.OK;
			this.available_content = content;
			this.relative_root = mostSuitableArchive.relativePath;
			global::Debug.Assert(this.content_source == null);
			this.content_source = new Directory(this.ContentPath);
			string arg = string.IsNullOrEmpty(this.relative_root) ? "root" : this.relative_root;
			global::Debug.Log(string.Format("{0}: Successfully loaded from path '{1}' with content '{2}'.", this.label, arg, this.available_content.ToString()));
		}

		// Token: 0x06006BA5 RID: 27557 RVA: 0x002A1768 File Offset: 0x0029F968
		private Mod.ArchivedVersion GetMostSuitableArchive()
		{
			Mod.PackagedModInfo packagedModInfo = this.GetModInfoForFolder("");
			if (packagedModInfo == null)
			{
				packagedModInfo = new Mod.PackagedModInfo
				{
					supportedContent = "vanilla_id",
					minimumSupportedBuild = 0
				};
				if (this.ScanContentFromSourceForTranslationsOnly(""))
				{
					this.ModDevLogWarning(string.Format("{0}: No mod_info.yaml found, but since it contains a translation, default its supported content to 'ALL'", this.label));
					packagedModInfo.supportedContent = "all";
				}
				else
				{
					this.ModDevLogWarning(string.Format("{0}: No mod_info.yaml found, default its supported content to 'VANILLA_ID'", this.label));
				}
			}
			Mod.ArchivedVersion archivedVersion = new Mod.ArchivedVersion
			{
				relativePath = "",
				info = packagedModInfo
			};
			if (!this.file_source.Exists("archived_versions"))
			{
				this.ModDevLog(string.Format("\t{0}: No archived_versions for this mod, using root version directly.", this.label));
				if (!this.DoesModSupportCurrentContent(packagedModInfo))
				{
					return null;
				}
				return archivedVersion;
			}
			else
			{
				List<FileSystemItem> list = new List<FileSystemItem>();
				this.file_source.GetTopLevelItems(list, "archived_versions");
				if (list.Count == 0)
				{
					this.ModDevLog(string.Format("\t{0}: No archived_versions for this mod, using root version directly.", this.label));
					if (!this.DoesModSupportCurrentContent(packagedModInfo))
					{
						return null;
					}
					return archivedVersion;
				}
				else
				{
					List<Mod.ArchivedVersion> list2 = new List<Mod.ArchivedVersion>();
					list2.Add(archivedVersion);
					foreach (FileSystemItem fileSystemItem in list)
					{
						string relativePath = Path.Combine("archived_versions", fileSystemItem.name);
						Mod.PackagedModInfo modInfoForFolder = this.GetModInfoForFolder(relativePath);
						if (modInfoForFolder != null)
						{
							list2.Add(new Mod.ArchivedVersion
							{
								relativePath = relativePath,
								info = modInfoForFolder
							});
						}
					}
					list2 = (from v in list2
					where this.DoesModSupportCurrentContent(v.info)
					select v).ToList<Mod.ArchivedVersion>();
					list2 = (from v in list2
					where v.info.APIVersion == 2 || v.info.APIVersion == 0
					select v).ToList<Mod.ArchivedVersion>();
					Mod.ArchivedVersion archivedVersion2 = (from v in list2
					where (long)v.info.minimumSupportedBuild <= 577063L
					orderby v.info.minimumSupportedBuild descending
					select v).FirstOrDefault<Mod.ArchivedVersion>();
					if (archivedVersion2 == null)
					{
						return null;
					}
					return archivedVersion2;
				}
			}
		}

		// Token: 0x06006BA6 RID: 27558 RVA: 0x002A19A8 File Offset: 0x0029FBA8
		private Mod.PackagedModInfo GetModInfoForFolder(string relative_root)
		{
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relative_root);
			bool flag = false;
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.File && fileSystemItem.name.ToLower() == "mod_info.yaml")
				{
					flag = true;
					break;
				}
			}
			string text = string.IsNullOrEmpty(relative_root) ? "root" : relative_root;
			if (!flag)
			{
				this.ModDevLogWarning(string.Concat(new string[]
				{
					"\t",
					this.title,
					": has no mod_info.yaml in folder '",
					text,
					"'"
				}));
				return null;
			}
			string text2 = this.file_source.Read(Path.Combine(relative_root, "mod_info.yaml"));
			if (string.IsNullOrEmpty(text2))
			{
				this.ModDevLogError(string.Format("\t{0}: Failed to read {1} in folder '{2}', skipping", this.label, "mod_info.yaml", text));
				return null;
			}
			YamlIO.ErrorHandler handle_error = delegate(YamlIO.Error e, bool force_warning)
			{
				YamlIO.LogError(e, !this.IsDev);
			};
			Mod.PackagedModInfo packagedModInfo = YamlIO.Parse<Mod.PackagedModInfo>(text2, default(FileHandle), handle_error, null);
			if (packagedModInfo == null)
			{
				this.ModDevLogError(string.Format("\t{0}: Failed to parse {1} in folder '{2}', text is {3}", new object[]
				{
					this.label,
					"mod_info.yaml",
					text,
					text2
				}));
				return null;
			}
			if (packagedModInfo.supportedContent == null)
			{
				this.ModDevLogError(string.Format("\t{0}: {1} in folder '{2}' does not specify supportedContent. Make sure you spelled it correctly in your mod_info!", this.label, "mod_info.yaml", text));
				return null;
			}
			if (packagedModInfo.lastWorkingBuild != 0)
			{
				this.ModDevLogError(string.Format("\t{0}: {1} in folder '{2}' is using `{3}`, please upgrade this to `{4}`", new object[]
				{
					this.label,
					"mod_info.yaml",
					text,
					"lastWorkingBuild",
					"minimumSupportedBuild"
				}));
				if (packagedModInfo.minimumSupportedBuild == 0)
				{
					packagedModInfo.minimumSupportedBuild = packagedModInfo.lastWorkingBuild;
				}
			}
			this.ModDevLog(string.Format("\t{0}: Found valid mod_info.yaml in folder '{1}': {2} at {3}", new object[]
			{
				this.label,
				text,
				packagedModInfo.supportedContent,
				packagedModInfo.minimumSupportedBuild
			}));
			return packagedModInfo;
		}

		// Token: 0x06006BA7 RID: 27559 RVA: 0x002A1BE8 File Offset: 0x0029FDE8
		private bool DoesModSupportCurrentContent(Mod.PackagedModInfo mod_info)
		{
			string text = DlcManager.GetHighestActiveDlcId();
			if (text == "")
			{
				text = "vanilla_id";
			}
			text = text.ToLower();
			string text2 = mod_info.supportedContent.ToLower();
			return text2.Contains(text) || text2.Contains("all");
		}

		// Token: 0x06006BA8 RID: 27560 RVA: 0x002A1C38 File Offset: 0x0029FE38
		private bool ScanContentFromSourceForTranslationsOnly(string relativeRoot)
		{
			this.available_content = (Content)0;
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relativeRoot);
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.File && fileSystemItem.name.ToLower().EndsWith(".po"))
				{
					this.available_content |= Content.Translation;
				}
			}
			return this.available_content > (Content)0;
		}

		// Token: 0x06006BA9 RID: 27561 RVA: 0x002A1CD0 File Offset: 0x0029FED0
		private bool ScanContentFromSource(string relativeRoot, out Content available)
		{
			available = (Content)0;
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relativeRoot);
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.Directory)
				{
					string directory = fileSystemItem.name.ToLower();
					available |= this.AddDirectory(directory);
				}
				else
				{
					string file = fileSystemItem.name.ToLower();
					available |= this.AddFile(file);
				}
			}
			return available > (Content)0;
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06006BAA RID: 27562 RVA: 0x002A1D70 File Offset: 0x0029FF70
		public string ContentPath
		{
			get
			{
				return Path.Combine(this.label.install_path, this.relative_root);
			}
		}

		// Token: 0x06006BAB RID: 27563 RVA: 0x002A1D88 File Offset: 0x0029FF88
		public bool IsEmpty()
		{
			return this.available_content == (Content)0;
		}

		// Token: 0x06006BAC RID: 27564 RVA: 0x002A1D94 File Offset: 0x0029FF94
		private Content AddDirectory(string directory)
		{
			Content content = (Content)0;
			string text = directory.TrimEnd(new char[]
			{
				'/'
			});
			if (text != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 1519694028U)
				{
					if (num != 948591336U)
					{
						if (num != 1318520008U)
						{
							if (num == 1519694028U)
							{
								if (text == "elements")
								{
									content |= Content.LayerableFiles;
								}
							}
						}
						else if (text == "buildingfacades")
						{
							content |= Content.Animation;
						}
					}
					else if (text == "templates")
					{
						content |= Content.LayerableFiles;
					}
				}
				else if (num <= 3037049615U)
				{
					if (num != 2960291089U)
					{
						if (num == 3037049615U)
						{
							if (text == "worldgen")
							{
								content |= Content.LayerableFiles;
							}
						}
					}
					else if (text == "strings")
					{
						content |= Content.Strings;
					}
				}
				else if (num != 3319670096U)
				{
					if (num == 3570262116U)
					{
						if (text == "codex")
						{
							content |= Content.LayerableFiles;
						}
					}
				}
				else if (text == "anim")
				{
					content |= Content.Animation;
				}
			}
			return content;
		}

		// Token: 0x06006BAD RID: 27565 RVA: 0x002A1EB4 File Offset: 0x002A00B4
		private Content AddFile(string file)
		{
			Content content = (Content)0;
			if (file.EndsWith(".dll"))
			{
				content |= Content.DLL;
			}
			if (file.EndsWith(".po"))
			{
				content |= Content.Translation;
			}
			return content;
		}

		// Token: 0x06006BAE RID: 27566 RVA: 0x002A1EE6 File Offset: 0x002A00E6
		private static void AccumulateExtensions(Content content, List<string> extensions)
		{
			if ((content & Content.DLL) != (Content)0)
			{
				extensions.Add(".dll");
			}
			if ((content & (Content.Strings | Content.Translation)) != (Content)0)
			{
				extensions.Add(".po");
			}
		}

		// Token: 0x06006BAF RID: 27567 RVA: 0x002A1F0C File Offset: 0x002A010C
		[Conditional("DEBUG")]
		private void Assert(bool condition, string failure_message)
		{
			if (string.IsNullOrEmpty(this.title))
			{
				DebugUtil.Assert(condition, string.Format("{2}\n\t{0}\n\t{1}", this.title, this.label.ToString(), failure_message));
				return;
			}
			DebugUtil.Assert(condition, string.Format("{1}\n\t{0}", this.label.ToString(), failure_message));
		}

		// Token: 0x06006BB0 RID: 27568 RVA: 0x002A1F74 File Offset: 0x002A0174
		public void Install()
		{
			if (this.IsLocal)
			{
				this.status = Mod.Status.Installed;
				return;
			}
			this.status = Mod.Status.ReinstallPending;
			if (this.file_source == null)
			{
				return;
			}
			if (!FileUtil.DeleteDirectory(this.label.install_path, 0))
			{
				return;
			}
			if (!FileUtil.CreateDirectory(this.label.install_path, 0))
			{
				return;
			}
			this.file_source.CopyTo(this.label.install_path, null);
			this.file_source = new Directory(this.label.install_path);
			this.status = Mod.Status.Installed;
		}

		// Token: 0x06006BB1 RID: 27569 RVA: 0x002A2004 File Offset: 0x002A0204
		public bool Uninstall()
		{
			this.SetEnabledForActiveDlc(false);
			if (this.loaded_content != (Content)0)
			{
				global::Debug.Log(string.Format("Can't uninstall {0}: still has loaded content: {1}", this.label.ToString(), this.loaded_content.ToString()));
				this.status = Mod.Status.UninstallPending;
				return false;
			}
			if (!this.IsLocal && !FileUtil.DeleteDirectory(this.label.install_path, 0))
			{
				global::Debug.Log(string.Format("Can't uninstall {0}: directory deletion failed", this.label.ToString()));
				this.status = Mod.Status.UninstallPending;
				return false;
			}
			this.status = Mod.Status.NotInstalled;
			return true;
		}

		// Token: 0x06006BB2 RID: 27570 RVA: 0x002A20AC File Offset: 0x002A02AC
		private bool LoadStrings()
		{
			string path = FileSystem.Normalize(Path.Combine(this.ContentPath, "strings"));
			if (!Directory.Exists(path))
			{
				return false;
			}
			int num = 0;
			foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
			{
				if (!(fileInfo.Extension.ToLower() != ".po"))
				{
					num++;
					Localization.OverloadStrings(Localization.LoadStringsFile(fileInfo.FullName, false));
				}
			}
			return true;
		}

		// Token: 0x06006BB3 RID: 27571 RVA: 0x002A2129 File Offset: 0x002A0329
		private bool LoadTranslations()
		{
			return false;
		}

		// Token: 0x06006BB4 RID: 27572 RVA: 0x002A212C File Offset: 0x002A032C
		private bool LoadAnimation()
		{
			string path = FileSystem.Normalize(Path.Combine(this.ContentPath, "anim"));
			if (!Directory.Exists(path))
			{
				return false;
			}
			int num = 0;
			DirectoryInfo[] directories = new DirectoryInfo(path).GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				foreach (DirectoryInfo directoryInfo in directories[i].GetDirectories())
				{
					KAnimFile.Mod mod = new KAnimFile.Mod();
					foreach (FileInfo fileInfo in directoryInfo.GetFiles())
					{
						if (fileInfo.Extension == ".png")
						{
							byte[] data = File.ReadAllBytes(fileInfo.FullName);
							Texture2D texture2D = new Texture2D(2, 2);
							texture2D.LoadImage(data);
							mod.textures.Add(texture2D);
						}
						else if (fileInfo.Extension == ".bytes")
						{
							string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
							byte[] array = File.ReadAllBytes(fileInfo.FullName);
							if (fileNameWithoutExtension.EndsWith("_anim"))
							{
								mod.anim = array;
							}
							else if (fileNameWithoutExtension.EndsWith("_build"))
							{
								mod.build = array;
							}
							else
							{
								DebugUtil.LogWarningArgs(new object[]
								{
									string.Format("Unhandled TextAsset ({0})...ignoring", fileInfo.FullName)
								});
							}
						}
						else
						{
							DebugUtil.LogWarningArgs(new object[]
							{
								string.Format("Unhandled asset ({0})...ignoring", fileInfo.FullName)
							});
						}
					}
					string name = directoryInfo.Name + "_kanim";
					if (mod.IsValid() && ModUtil.AddKAnimMod(name, mod))
					{
						num++;
					}
				}
			}
			return true;
		}

		// Token: 0x06006BB5 RID: 27573 RVA: 0x002A22F0 File Offset: 0x002A04F0
		public void Load(Content content)
		{
			content &= (this.available_content & ~this.loaded_content);
			if (content > (Content)0)
			{
				global::Debug.Log(string.Format("Loading mod content {2} [{0}:{1}] (provides {3})", new object[]
				{
					this.title,
					this.label.id,
					content.ToString(),
					this.available_content.ToString()
				}));
			}
			if ((content & Content.Strings) != (Content)0 && this.LoadStrings())
			{
				this.loaded_content |= Content.Strings;
			}
			if ((content & Content.Translation) != (Content)0 && this.LoadTranslations())
			{
				this.loaded_content |= Content.Translation;
			}
			if ((content & Content.DLL) != (Content)0)
			{
				this.loaded_mod_data = DLLLoader.LoadDLLs(this, this.staticID, this.ContentPath, this.IsDev);
				if (this.loaded_mod_data != null)
				{
					this.loaded_content |= Content.DLL;
				}
			}
			if ((content & Content.LayerableFiles) != (Content)0)
			{
				global::Debug.Assert(this.content_source != null, "Attempting to Load layerable files with content_source not initialized");
				FileSystem.file_sources.Insert(0, this.content_source.GetFileSystem());
				this.loaded_content |= Content.LayerableFiles;
			}
			if ((content & Content.Animation) != (Content)0 && this.LoadAnimation())
			{
				this.loaded_content |= Content.Animation;
			}
		}

		// Token: 0x06006BB6 RID: 27574 RVA: 0x002A242F File Offset: 0x002A062F
		public void PostLoad(IReadOnlyList<Mod> mods)
		{
			if ((this.loaded_content & Content.DLL) != (Content)0 && this.loaded_mod_data != null)
			{
				DLLLoader.PostLoadDLLs(this.staticID, this.loaded_mod_data, mods);
			}
		}

		// Token: 0x06006BB7 RID: 27575 RVA: 0x002A2455 File Offset: 0x002A0655
		public void Unload(Content content)
		{
			content &= this.loaded_content;
			if ((content & Content.LayerableFiles) != (Content)0)
			{
				FileSystem.file_sources.Remove(this.content_source.GetFileSystem());
				this.loaded_content &= ~Content.LayerableFiles;
			}
		}

		// Token: 0x06006BB8 RID: 27576 RVA: 0x002A248E File Offset: 0x002A068E
		private void SetCrashCount(int new_crash_count)
		{
			this.crash_count = MathUtil.Clamp(0, 3, new_crash_count);
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06006BB9 RID: 27577 RVA: 0x002A249E File Offset: 0x002A069E
		public bool IsDev
		{
			get
			{
				return this.label.distribution_platform == Label.DistributionPlatform.Dev;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06006BBA RID: 27578 RVA: 0x002A24AE File Offset: 0x002A06AE
		public bool IsLocal
		{
			get
			{
				return this.label.distribution_platform == Label.DistributionPlatform.Dev || this.label.distribution_platform == Label.DistributionPlatform.Local;
			}
		}

		// Token: 0x06006BBB RID: 27579 RVA: 0x002A24CE File Offset: 0x002A06CE
		public void SetCrashed()
		{
			this.SetCrashCount(this.crash_count + 1);
			if (!this.IsDev)
			{
				this.SetEnabledForActiveDlc(false);
			}
		}

		// Token: 0x06006BBC RID: 27580 RVA: 0x002A24ED File Offset: 0x002A06ED
		public void Uncrash()
		{
			this.SetCrashCount(this.IsDev ? (this.crash_count - 1) : 0);
		}

		// Token: 0x06006BBD RID: 27581 RVA: 0x002A2508 File Offset: 0x002A0708
		public bool IsActive()
		{
			return this.loaded_content > (Content)0;
		}

		// Token: 0x06006BBE RID: 27582 RVA: 0x002A2513 File Offset: 0x002A0713
		public bool AllActive(Content content)
		{
			return (this.loaded_content & content) == content;
		}

		// Token: 0x06006BBF RID: 27583 RVA: 0x002A2520 File Offset: 0x002A0720
		public bool AllActive()
		{
			return (this.loaded_content & this.available_content) == this.available_content;
		}

		// Token: 0x06006BC0 RID: 27584 RVA: 0x002A2537 File Offset: 0x002A0737
		public bool AnyActive(Content content)
		{
			return (this.loaded_content & content) > (Content)0;
		}

		// Token: 0x06006BC1 RID: 27585 RVA: 0x002A2544 File Offset: 0x002A0744
		public bool HasContent()
		{
			return this.available_content > (Content)0;
		}

		// Token: 0x06006BC2 RID: 27586 RVA: 0x002A254F File Offset: 0x002A074F
		public bool HasAnyContent(Content content)
		{
			return (this.available_content & content) > (Content)0;
		}

		// Token: 0x06006BC3 RID: 27587 RVA: 0x002A255C File Offset: 0x002A075C
		public bool HasOnlyTranslationContent()
		{
			return this.available_content == Content.Translation;
		}

		// Token: 0x06006BC4 RID: 27588 RVA: 0x002A2568 File Offset: 0x002A0768
		public Texture2D GetPreviewImage()
		{
			string text = null;
			foreach (string text2 in Mod.PREVIEW_FILENAMES)
			{
				if (Directory.Exists(this.ContentPath) && File.Exists(Path.Combine(this.ContentPath, text2)))
				{
					text = text2;
					break;
				}
			}
			if (text == null)
			{
				return null;
			}
			Texture2D result;
			try
			{
				byte[] data = File.ReadAllBytes(Path.Combine(this.ContentPath, text));
				Texture2D texture2D = new Texture2D(2, 2);
				texture2D.LoadImage(data);
				result = texture2D;
			}
			catch
			{
				global::Debug.LogWarning(string.Format("Mod {0} seems to have a preview.png but it didn't load correctly.", this.label));
				result = null;
			}
			return result;
		}

		// Token: 0x06006BC5 RID: 27589 RVA: 0x002A2634 File Offset: 0x002A0834
		public void ModDevLog(string msg)
		{
			if (this.IsDev)
			{
				global::Debug.Log(msg);
			}
		}

		// Token: 0x06006BC6 RID: 27590 RVA: 0x002A2644 File Offset: 0x002A0844
		public void ModDevLogWarning(string msg)
		{
			if (this.IsDev)
			{
				global::Debug.LogWarning(msg);
			}
		}

		// Token: 0x06006BC7 RID: 27591 RVA: 0x002A2654 File Offset: 0x002A0854
		public void ModDevLogError(string msg)
		{
			if (this.IsDev)
			{
				this.DevModCrashTriggered = true;
				global::Debug.LogError(msg);
			}
		}

		// Token: 0x04004EC4 RID: 20164
		public const int MOD_API_VERSION_NONE = 0;

		// Token: 0x04004EC5 RID: 20165
		public const int MOD_API_VERSION_HARMONY1 = 1;

		// Token: 0x04004EC6 RID: 20166
		public const int MOD_API_VERSION_HARMONY2 = 2;

		// Token: 0x04004EC7 RID: 20167
		public const int MOD_API_VERSION = 2;

		// Token: 0x04004EC8 RID: 20168
		[JsonProperty]
		public Label label;

		// Token: 0x04004EC9 RID: 20169
		[JsonProperty]
		public Mod.Status status;

		// Token: 0x04004ECA RID: 20170
		[JsonProperty]
		public bool enabled;

		// Token: 0x04004ECB RID: 20171
		[JsonProperty]
		public List<string> enabledForDlc;

		// Token: 0x04004ECD RID: 20173
		[JsonProperty]
		public int crash_count;

		// Token: 0x04004ECE RID: 20174
		[JsonProperty]
		public string reinstall_path;

		// Token: 0x04004ED0 RID: 20176
		public bool foundInStackTrace;

		// Token: 0x04004ED1 RID: 20177
		public string relative_root = "";

		// Token: 0x04004ED2 RID: 20178
		public Mod.PackagedModInfo packagedModInfo;

		// Token: 0x04004ED7 RID: 20183
		public LoadedModData loaded_mod_data;

		// Token: 0x04004ED8 RID: 20184
		private IFileSource _fileSource;

		// Token: 0x04004ED9 RID: 20185
		public IFileSource content_source;

		// Token: 0x04004EDA RID: 20186
		public bool is_subscribed;

		// Token: 0x04004EDC RID: 20188
		private const string VANILLA_ID = "vanilla_id";

		// Token: 0x04004EDD RID: 20189
		private const string ALL_ID = "all";

		// Token: 0x04004EDE RID: 20190
		private const string ARCHIVED_VERSIONS_FOLDER = "archived_versions";

		// Token: 0x04004EDF RID: 20191
		private const string MOD_INFO_FILENAME = "mod_info.yaml";

		// Token: 0x04004EE0 RID: 20192
		public ModContentCompatability contentCompatability;

		// Token: 0x04004EE1 RID: 20193
		public const int MAX_CRASH_COUNT = 3;

		// Token: 0x04004EE2 RID: 20194
		private static readonly List<string> PREVIEW_FILENAMES = new List<string>
		{
			"preview.png",
			"Preview.png",
			"PREVIEW.PNG"
		};

		// Token: 0x02001C50 RID: 7248
		public enum Status
		{
			// Token: 0x0400806E RID: 32878
			NotInstalled,
			// Token: 0x0400806F RID: 32879
			Installed,
			// Token: 0x04008070 RID: 32880
			UninstallPending,
			// Token: 0x04008071 RID: 32881
			ReinstallPending
		}

		// Token: 0x02001C51 RID: 7249
		public class ArchivedVersion
		{
			// Token: 0x04008072 RID: 32882
			public string relativePath;

			// Token: 0x04008073 RID: 32883
			public Mod.PackagedModInfo info;
		}

		// Token: 0x02001C52 RID: 7250
		public class PackagedModInfo
		{
			// Token: 0x17000A60 RID: 2656
			// (get) Token: 0x06009CE7 RID: 40167 RVA: 0x0034EF8E File Offset: 0x0034D18E
			// (set) Token: 0x06009CE8 RID: 40168 RVA: 0x0034EF96 File Offset: 0x0034D196
			public string supportedContent { get; set; }

			// Token: 0x17000A61 RID: 2657
			// (get) Token: 0x06009CE9 RID: 40169 RVA: 0x0034EF9F File Offset: 0x0034D19F
			// (set) Token: 0x06009CEA RID: 40170 RVA: 0x0034EFA7 File Offset: 0x0034D1A7
			[Obsolete("Use minimumSupportedBuild instead!")]
			public int lastWorkingBuild { get; set; }

			// Token: 0x17000A62 RID: 2658
			// (get) Token: 0x06009CEB RID: 40171 RVA: 0x0034EFB0 File Offset: 0x0034D1B0
			// (set) Token: 0x06009CEC RID: 40172 RVA: 0x0034EFB8 File Offset: 0x0034D1B8
			public int minimumSupportedBuild { get; set; }

			// Token: 0x17000A63 RID: 2659
			// (get) Token: 0x06009CED RID: 40173 RVA: 0x0034EFC1 File Offset: 0x0034D1C1
			// (set) Token: 0x06009CEE RID: 40174 RVA: 0x0034EFC9 File Offset: 0x0034D1C9
			public int APIVersion { get; set; }

			// Token: 0x17000A64 RID: 2660
			// (get) Token: 0x06009CEF RID: 40175 RVA: 0x0034EFD2 File Offset: 0x0034D1D2
			// (set) Token: 0x06009CF0 RID: 40176 RVA: 0x0034EFDA File Offset: 0x0034D1DA
			public string version { get; set; }
		}
	}
}
