using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zlib;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using KMod;
using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02000950 RID: 2384
[AddComponentMenu("KMonoBehaviour/scripts/SaveLoader")]
public class SaveLoader : KMonoBehaviour
{
	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x0600457B RID: 17787 RVA: 0x00187E5C File Offset: 0x0018605C
	// (set) Token: 0x0600457C RID: 17788 RVA: 0x00187E64 File Offset: 0x00186064
	public bool loadedFromSave { get; private set; }

	// Token: 0x0600457D RID: 17789 RVA: 0x00187E6D File Offset: 0x0018606D
	public static void DestroyInstance()
	{
		SaveLoader.Instance = null;
	}

	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x0600457E RID: 17790 RVA: 0x00187E75 File Offset: 0x00186075
	// (set) Token: 0x0600457F RID: 17791 RVA: 0x00187E7C File Offset: 0x0018607C
	public static SaveLoader Instance { get; private set; }

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x06004580 RID: 17792 RVA: 0x00187E84 File Offset: 0x00186084
	// (set) Token: 0x06004581 RID: 17793 RVA: 0x00187E8C File Offset: 0x0018608C
	public Action<Cluster> OnWorldGenComplete { get; set; }

	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x06004582 RID: 17794 RVA: 0x00187E95 File Offset: 0x00186095
	public Cluster ClusterLayout
	{
		get
		{
			return this.m_clusterLayout;
		}
	}

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x06004583 RID: 17795 RVA: 0x00187E9D File Offset: 0x0018609D
	// (set) Token: 0x06004584 RID: 17796 RVA: 0x00187EA5 File Offset: 0x001860A5
	public SaveGame.GameInfo GameInfo { get; private set; }

	// Token: 0x06004585 RID: 17797 RVA: 0x00187EAE File Offset: 0x001860AE
	protected override void OnPrefabInit()
	{
		SaveLoader.Instance = this;
		this.saveManager = base.GetComponent<SaveManager>();
	}

	// Token: 0x06004586 RID: 17798 RVA: 0x00187EC2 File Offset: 0x001860C2
	private void MoveCorruptFile(string filename)
	{
	}

	// Token: 0x06004587 RID: 17799 RVA: 0x00187EC4 File Offset: 0x001860C4
	protected override void OnSpawn()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (WorldGen.CanLoad(activeSaveFilePath))
		{
			Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
			SimMessages.CreateSimElementsTable(ElementLoader.elements);
			SimMessages.CreateDiseaseTable(Db.Get().Diseases);
			this.loadedFromSave = true;
			this.loadedFromSave = this.Load(activeSaveFilePath);
			this.saveFileCorrupt = !this.loadedFromSave;
			if (!this.loadedFromSave)
			{
				SaveLoader.SetActiveSaveFilePath(null);
				if (this.mustRestartOnFail)
				{
					this.MoveCorruptFile(activeSaveFilePath);
					Sim.Shutdown();
					App.LoadScene("frontend");
					return;
				}
			}
		}
		if (!this.loadedFromSave)
		{
			Sim.Shutdown();
			if (!string.IsNullOrEmpty(activeSaveFilePath))
			{
				DebugUtil.LogArgs(new object[]
				{
					"Couldn't load [" + activeSaveFilePath + "]"
				});
			}
			if (this.saveFileCorrupt)
			{
				this.MoveCorruptFile(activeSaveFilePath);
			}
			int num = 0;
			bool flag = WorldGen.CanLoad(WorldGen.GetSIMSaveFilename(num));
			if (!flag || !this.LoadFromWorldGen())
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Couldn't start new game with current world gen, moving file"
				});
				if (flag)
				{
					KMonoBehaviour.isLoadingScene = true;
					while (FileSystem.FileExists(WorldGen.GetSIMSaveFilename(num)))
					{
						this.MoveCorruptFile(WorldGen.GetSIMSaveFilename(num));
						num++;
					}
				}
				App.LoadScene("frontend");
			}
		}
	}

	// Token: 0x06004588 RID: 17800 RVA: 0x00187FFC File Offset: 0x001861FC
	private static void CompressContents(BinaryWriter fileWriter, byte[] uncompressed, int length)
	{
		using (ZlibStream zlibStream = new ZlibStream(fileWriter.BaseStream, CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestSpeed))
		{
			zlibStream.Write(uncompressed, 0, length);
			zlibStream.Flush();
		}
	}

	// Token: 0x06004589 RID: 17801 RVA: 0x00188044 File Offset: 0x00186244
	private byte[] FloatToBytes(float[] floats)
	{
		byte[] array = new byte[floats.Length * 4];
		Buffer.BlockCopy(floats, 0, array, 0, array.Length);
		return array;
	}

	// Token: 0x0600458A RID: 17802 RVA: 0x00188069 File Offset: 0x00186269
	private static byte[] DecompressContents(byte[] compressed)
	{
		return ZlibStream.UncompressBuffer(compressed);
	}

	// Token: 0x0600458B RID: 17803 RVA: 0x00188074 File Offset: 0x00186274
	private float[] BytesToFloat(byte[] bytes)
	{
		float[] array = new float[bytes.Length / 4];
		Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
		return array;
	}

	// Token: 0x0600458C RID: 17804 RVA: 0x0018809C File Offset: 0x0018629C
	private SaveFileRoot PrepSaveFile()
	{
		SaveFileRoot saveFileRoot = new SaveFileRoot();
		saveFileRoot.WidthInCells = Grid.WidthInCells;
		saveFileRoot.HeightInCells = Grid.HeightInCells;
		saveFileRoot.streamed["GridVisible"] = Grid.Visible;
		saveFileRoot.streamed["GridSpawnable"] = Grid.Spawnable;
		saveFileRoot.streamed["GridDamage"] = this.FloatToBytes(Grid.Damage);
		Global.Instance.modManager.SendMetricsEvent();
		saveFileRoot.active_mods = new List<Label>();
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.IsEnabledForActiveDlc())
			{
				saveFileRoot.active_mods.Add(mod.label);
			}
		}
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				Camera.main.transform.parent.GetComponent<CameraController>().Save(binaryWriter);
			}
			saveFileRoot.streamed["Camera"] = memoryStream.ToArray();
		}
		return saveFileRoot;
	}

	// Token: 0x0600458D RID: 17805 RVA: 0x001881F8 File Offset: 0x001863F8
	private void Save(BinaryWriter writer)
	{
		writer.WriteKleiString("world");
		Serializer.Serialize(this.PrepSaveFile(), writer);
		Game.SaveSettings(writer);
		Sim.Save(writer, 0, 0);
		this.saveManager.Save(writer);
		Game.Instance.Save(writer);
	}

	// Token: 0x0600458E RID: 17806 RVA: 0x00188238 File Offset: 0x00186438
	private bool Load(IReader reader)
	{
		global::Debug.Assert(reader.ReadKleiString() == "world");
		Deserializer deserializer = new Deserializer(reader);
		SaveFileRoot saveFileRoot = new SaveFileRoot();
		deserializer.Deserialize(saveFileRoot);
		if ((this.GameInfo.saveMajorVersion == 7 || this.GameInfo.saveMinorVersion < 8) && saveFileRoot.requiredMods != null)
		{
			saveFileRoot.active_mods = new List<Label>();
			foreach (ModInfo modInfo in saveFileRoot.requiredMods)
			{
				saveFileRoot.active_mods.Add(new Label
				{
					id = modInfo.assetID,
					version = (long)modInfo.lastModifiedTime,
					distribution_platform = Label.DistributionPlatform.Steam,
					title = modInfo.description
				});
			}
			saveFileRoot.requiredMods.Clear();
		}
		KMod.Manager modManager = Global.Instance.modManager;
		modManager.Load(Content.LayerableFiles);
		if (!modManager.MatchFootprint(saveFileRoot.active_mods, Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Mod footprint of save file doesn't match current mod configuration"
			});
		}
		string text = string.Format("Mod Footprint ({0}):", saveFileRoot.active_mods.Count);
		foreach (Label label in saveFileRoot.active_mods)
		{
			text = text + "\n  - " + label.title;
		}
		global::Debug.Log(text);
		this.LogActiveMods();
		Global.Instance.modManager.SendMetricsEvent();
		WorldGen.LoadSettings(false);
		CustomGameSettings.Instance.LoadClusters();
		if (this.GameInfo.clusterId == null)
		{
			SaveGame.GameInfo gameInfo = this.GameInfo;
			if (!string.IsNullOrEmpty(saveFileRoot.clusterID))
			{
				gameInfo.clusterId = saveFileRoot.clusterID;
			}
			else
			{
				try
				{
					gameInfo.clusterId = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;
				}
				catch
				{
					gameInfo.clusterId = WorldGenSettings.ClusterDefaultName;
					CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, gameInfo.clusterId);
				}
			}
			this.GameInfo = gameInfo;
		}
		Game.clusterId = this.GameInfo.clusterId;
		Game.LoadSettings(deserializer);
		GridSettings.Reset(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells);
		if (Application.isPlaying)
		{
			Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
		}
		Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
		SimMessages.CreateSimElementsTable(ElementLoader.elements);
		Sim.AllocateCells(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells, false);
		SimMessages.CreateDiseaseTable(Db.Get().Diseases);
		Sim.HandleMessage(SimMessageHashes.ClearUnoccupiedCells, 0, null);
		IReader reader2;
		if (saveFileRoot.streamed.ContainsKey("Sim"))
		{
			reader2 = new FastReader(saveFileRoot.streamed["Sim"]);
		}
		else
		{
			reader2 = reader;
		}
		if (Sim.LoadWorld(reader2) != 0)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\nSimDLL found bad data\n"
			});
			Sim.Shutdown();
			return false;
		}
		Sim.Start();
		SceneInitializer.Instance.PostLoadPrefabs();
		this.mustRestartOnFail = true;
		if (!this.saveManager.Load(reader))
		{
			Sim.Shutdown();
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\n"
			});
			SaveLoader.SetActiveSaveFilePath(null);
			return false;
		}
		Grid.Visible = saveFileRoot.streamed["GridVisible"];
		if (saveFileRoot.streamed.ContainsKey("GridSpawnable"))
		{
			Grid.Spawnable = saveFileRoot.streamed["GridSpawnable"];
		}
		Grid.Damage = this.BytesToFloat(saveFileRoot.streamed["GridDamage"]);
		Game.Instance.Load(deserializer);
		CameraSaveData.Load(new FastReader(saveFileRoot.streamed["Camera"]));
		ClusterManager.Instance.InitializeWorldGrid();
		SimMessages.DefineWorldOffsets((from container in ClusterManager.Instance.WorldContainers
		select new SimMessages.WorldOffsetData
		{
			worldOffsetX = container.WorldOffset.x,
			worldOffsetY = container.WorldOffset.y,
			worldSizeX = container.WorldSize.x,
			worldSizeY = container.WorldSize.y
		}).ToList<SimMessages.WorldOffsetData>());
		return true;
	}

	// Token: 0x0600458F RID: 17807 RVA: 0x0018865C File Offset: 0x0018685C
	private void LogActiveMods()
	{
		string text = string.Format("Active Mods ({0}:", Global.Instance.modManager.mods.Count((Mod x) => x.IsEnabledForActiveDlc()));
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.IsEnabledForActiveDlc())
			{
				text = text + "\n  - " + mod.title;
			}
		}
		global::Debug.Log(text);
	}

	// Token: 0x06004590 RID: 17808 RVA: 0x00188714 File Offset: 0x00186914
	public static string GetSavePrefix()
	{
		return System.IO.Path.Combine(global::Util.RootFolder(), string.Format("{0}{1}", "save_files", System.IO.Path.DirectorySeparatorChar));
	}

	// Token: 0x06004591 RID: 17809 RVA: 0x0018873C File Offset: 0x0018693C
	public static string GetCloudSavePrefix()
	{
		string text = System.IO.Path.Combine(global::Util.RootFolder(), string.Format("{0}{1}", "cloud_save_files", System.IO.Path.DirectorySeparatorChar));
		string userID = SaveLoader.GetUserID();
		if (string.IsNullOrEmpty(userID))
		{
			return null;
		}
		text = System.IO.Path.Combine(text, userID);
		if (!System.IO.Directory.Exists(text))
		{
			System.IO.Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x06004592 RID: 17810 RVA: 0x00188798 File Offset: 0x00186998
	public static string GetSavePrefixAndCreateFolder()
	{
		string savePrefix = SaveLoader.GetSavePrefix();
		if (!System.IO.Directory.Exists(savePrefix))
		{
			System.IO.Directory.CreateDirectory(savePrefix);
		}
		return savePrefix;
	}

	// Token: 0x06004593 RID: 17811 RVA: 0x001887BC File Offset: 0x001869BC
	public static string GetUserID()
	{
		DistributionPlatform.User localUser = DistributionPlatform.Inst.LocalUser;
		if (localUser == null)
		{
			return null;
		}
		return localUser.Id.ToString();
	}

	// Token: 0x06004594 RID: 17812 RVA: 0x001887E4 File Offset: 0x001869E4
	public static string GetNextUsableSavePath(string filename)
	{
		int num = 0;
		string arg = System.IO.Path.ChangeExtension(filename, null);
		while (File.Exists(filename))
		{
			filename = SaveScreen.GetValidSaveFilename(string.Format("{0} ({1})", arg, num));
			num++;
		}
		return filename;
	}

	// Token: 0x06004595 RID: 17813 RVA: 0x00188822 File Offset: 0x00186A22
	public static string GetOriginalSaveFileName(string filename)
	{
		if (!filename.Contains("/") && !filename.Contains("\\"))
		{
			return filename;
		}
		filename.Replace('\\', '/');
		return System.IO.Path.GetFileName(filename);
	}

	// Token: 0x06004596 RID: 17814 RVA: 0x00188851 File Offset: 0x00186A51
	public static bool IsSaveAuto(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/auto_save/");
	}

	// Token: 0x06004597 RID: 17815 RVA: 0x0018886A File Offset: 0x00186A6A
	public static bool IsSaveLocal(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/save_files/");
	}

	// Token: 0x06004598 RID: 17816 RVA: 0x00188883 File Offset: 0x00186A83
	public static bool IsSaveCloud(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/cloud_save_files/");
	}

	// Token: 0x06004599 RID: 17817 RVA: 0x0018889C File Offset: 0x00186A9C
	public static string GetAutoSavePrefix()
	{
		string text = System.IO.Path.Combine(SaveLoader.GetSavePrefixAndCreateFolder(), string.Format("{0}{1}", "auto_save", System.IO.Path.DirectorySeparatorChar));
		if (!System.IO.Directory.Exists(text))
		{
			System.IO.Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x0600459A RID: 17818 RVA: 0x001888DD File Offset: 0x00186ADD
	public static void SetActiveSaveFilePath(string path)
	{
		KPlayerPrefs.SetString("SaveFilenameKey/", path);
	}

	// Token: 0x0600459B RID: 17819 RVA: 0x001888EA File Offset: 0x00186AEA
	public static string GetActiveSaveFilePath()
	{
		return KPlayerPrefs.GetString("SaveFilenameKey/");
	}

	// Token: 0x0600459C RID: 17820 RVA: 0x001888F8 File Offset: 0x00186AF8
	public static string GetActiveAutoSavePath()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (activeSaveFilePath == null)
		{
			return SaveLoader.GetAutoSavePrefix();
		}
		return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(activeSaveFilePath), "auto_save");
	}

	// Token: 0x0600459D RID: 17821 RVA: 0x00188924 File Offset: 0x00186B24
	public static string GetAutosaveFilePath()
	{
		return SaveLoader.GetAutoSavePrefix() + "AutoSave Cycle 1.sav";
	}

	// Token: 0x0600459E RID: 17822 RVA: 0x00188938 File Offset: 0x00186B38
	public static string GetActiveSaveColonyFolder()
	{
		string text = SaveLoader.GetActiveSaveFolder();
		if (text == null)
		{
			text = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), SaveLoader.Instance.GameInfo.baseName);
		}
		return text;
	}

	// Token: 0x0600459F RID: 17823 RVA: 0x0018896C File Offset: 0x00186B6C
	public static string GetActiveSaveFolder()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (!string.IsNullOrEmpty(activeSaveFilePath))
		{
			return System.IO.Path.GetDirectoryName(activeSaveFilePath);
		}
		return null;
	}

	// Token: 0x060045A0 RID: 17824 RVA: 0x00188990 File Offset: 0x00186B90
	public static List<SaveLoader.SaveFileEntry> GetSaveFiles(string save_dir, bool sort, SearchOption search = SearchOption.AllDirectories)
	{
		List<SaveLoader.SaveFileEntry> list = new List<SaveLoader.SaveFileEntry>();
		if (string.IsNullOrEmpty(save_dir))
		{
			return list;
		}
		try
		{
			if (!System.IO.Directory.Exists(save_dir))
			{
				System.IO.Directory.CreateDirectory(save_dir);
			}
			foreach (string text in System.IO.Directory.GetFiles(save_dir, "*.sav", search))
			{
				try
				{
					System.DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(text);
					SaveLoader.SaveFileEntry item = new SaveLoader.SaveFileEntry
					{
						path = text,
						timeStamp = lastWriteTimeUtc
					};
					list.Add(item);
				}
				catch (Exception ex)
				{
					global::Debug.LogWarning("Problem reading file: " + text + "\n" + ex.ToString());
				}
			}
			if (sort)
			{
				list.Sort((SaveLoader.SaveFileEntry x, SaveLoader.SaveFileEntry y) => y.timeStamp.CompareTo(x.timeStamp));
			}
		}
		catch (Exception ex2)
		{
			string text2 = null;
			if (ex2 is UnauthorizedAccessException)
			{
				text2 = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, save_dir);
			}
			else if (ex2 is IOException)
			{
				text2 = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, save_dir);
			}
			if (text2 == null)
			{
				throw ex2;
			}
			GameObject parent = (FrontEndManager.Instance == null) ? GameScreenManager.Instance.ssOverlayCanvas : FrontEndManager.Instance.gameObject;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(text2, null, null, null, null, null, null, null, null);
		}
		return list;
	}

	// Token: 0x060045A1 RID: 17825 RVA: 0x00188B10 File Offset: 0x00186D10
	public static List<SaveLoader.SaveFileEntry> GetAllFiles(bool sort, SaveLoader.SaveType type = SaveLoader.SaveType.both)
	{
		switch (type)
		{
		case SaveLoader.SaveType.local:
			return SaveLoader.GetSaveFiles(SaveLoader.GetSavePrefixAndCreateFolder(), sort, SearchOption.AllDirectories);
		case SaveLoader.SaveType.cloud:
			return SaveLoader.GetSaveFiles(SaveLoader.GetCloudSavePrefix(), sort, SearchOption.AllDirectories);
		case SaveLoader.SaveType.both:
		{
			List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(SaveLoader.GetSavePrefixAndCreateFolder(), false, SearchOption.AllDirectories);
			List<SaveLoader.SaveFileEntry> saveFiles2 = SaveLoader.GetSaveFiles(SaveLoader.GetCloudSavePrefix(), false, SearchOption.AllDirectories);
			saveFiles.AddRange(saveFiles2);
			if (sort)
			{
				saveFiles.Sort((SaveLoader.SaveFileEntry x, SaveLoader.SaveFileEntry y) => y.timeStamp.CompareTo(x.timeStamp));
			}
			return saveFiles;
		}
		default:
			return new List<SaveLoader.SaveFileEntry>();
		}
	}

	// Token: 0x060045A2 RID: 17826 RVA: 0x00188B9B File Offset: 0x00186D9B
	public static List<SaveLoader.SaveFileEntry> GetAllColonyFiles(bool sort, SearchOption search = SearchOption.TopDirectoryOnly)
	{
		return SaveLoader.GetSaveFiles(SaveLoader.GetActiveSaveColonyFolder(), sort, search);
	}

	// Token: 0x060045A3 RID: 17827 RVA: 0x00188BA9 File Offset: 0x00186DA9
	public static bool GetCloudSavesDefault()
	{
		return !(SaveLoader.GetCloudSavesDefaultPref() == "Disabled");
	}

	// Token: 0x060045A4 RID: 17828 RVA: 0x00188BC0 File Offset: 0x00186DC0
	public static string GetCloudSavesDefaultPref()
	{
		string text = KPlayerPrefs.GetString("SavesDefaultToCloud", "Enabled");
		if (text != "Enabled" && text != "Disabled")
		{
			text = "Enabled";
		}
		return text;
	}

	// Token: 0x060045A5 RID: 17829 RVA: 0x00188BFE File Offset: 0x00186DFE
	public static void SetCloudSavesDefault(bool value)
	{
		SaveLoader.SetCloudSavesDefaultPref(value ? "Enabled" : "Disabled");
	}

	// Token: 0x060045A6 RID: 17830 RVA: 0x00188C14 File Offset: 0x00186E14
	public static void SetCloudSavesDefaultPref(string pref)
	{
		if (pref != "Enabled" && pref != "Disabled")
		{
			global::Debug.LogWarning("Ignoring cloud saves default pref `" + pref + "` as it's not valid, expected `Enabled` or `Disabled`");
			return;
		}
		KPlayerPrefs.SetString("SavesDefaultToCloud", pref);
	}

	// Token: 0x060045A7 RID: 17831 RVA: 0x00188C51 File Offset: 0x00186E51
	public static bool GetCloudSavesAvailable()
	{
		return !string.IsNullOrEmpty(SaveLoader.GetUserID()) && SaveLoader.GetCloudSavePrefix() != null;
	}

	// Token: 0x060045A8 RID: 17832 RVA: 0x00188C6C File Offset: 0x00186E6C
	public static string GetLatestSaveFile()
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(true, SaveLoader.SaveType.both);
		if (allFiles.Count == 0)
		{
			return null;
		}
		return allFiles[0].path;
	}

	// Token: 0x060045A9 RID: 17833 RVA: 0x00188C98 File Offset: 0x00186E98
	public static string GetLatestSaveForCurrentDLC()
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(true, SaveLoader.SaveType.both);
		for (int i = 0; i < allFiles.Count; i++)
		{
			global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(allFiles[i].path);
			if (fileInfo != null)
			{
				SaveGame.Header first = fileInfo.first;
				SaveGame.GameInfo second = fileInfo.second;
				if (second.saveMajorVersion >= 7 && DlcManager.GetHighestActiveDlcId() == second.dlcId)
				{
					return allFiles[i].path;
				}
			}
		}
		return null;
	}

	// Token: 0x060045AA RID: 17834 RVA: 0x00188D10 File Offset: 0x00186F10
	public void InitialSave()
	{
		string text = SaveLoader.GetActiveSaveFilePath();
		if (string.IsNullOrEmpty(text))
		{
			text = SaveLoader.GetAutosaveFilePath();
		}
		else if (!text.Contains(".sav"))
		{
			text += ".sav";
		}
		this.LogActiveMods();
		this.Save(text, false, true);
	}

	// Token: 0x060045AB RID: 17835 RVA: 0x00188D5C File Offset: 0x00186F5C
	public string Save(string filename, bool isAutoSave = false, bool updateSavePointer = true)
	{
		KSerialization.Manager.Clear();
		string directoryName = System.IO.Path.GetDirectoryName(filename);
		try
		{
			if (directoryName != null && !System.IO.Directory.Exists(directoryName))
			{
				System.IO.Directory.CreateDirectory(directoryName);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("Problem creating save folder for " + filename + "!\n" + ex.ToString());
		}
		this.ReportSaveMetrics(isAutoSave);
		RetireColonyUtility.SaveColonySummaryData();
		if (isAutoSave && !GenericGameSettings.instance.keepAllAutosaves)
		{
			List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(SaveLoader.GetActiveAutoSavePath(), true, SearchOption.AllDirectories);
			List<string> list = new List<string>();
			foreach (SaveLoader.SaveFileEntry saveFileEntry in saveFiles)
			{
				global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(saveFileEntry.path);
				if (fileInfo != null && SaveGame.GetSaveUniqueID(fileInfo.second) == SaveLoader.Instance.GameInfo.colonyGuid.ToString())
				{
					list.Add(saveFileEntry.path);
				}
			}
			for (int i = list.Count - 1; i >= 9; i--)
			{
				string text = list[i];
				try
				{
					global::Debug.Log("Deleting old autosave: " + text);
					File.Delete(text);
				}
				catch (Exception ex2)
				{
					global::Debug.LogWarning("Problem deleting autosave: " + text + "\n" + ex2.ToString());
				}
				string text2 = System.IO.Path.ChangeExtension(text, ".png");
				try
				{
					if (File.Exists(text2))
					{
						File.Delete(text2);
					}
				}
				catch (Exception ex3)
				{
					global::Debug.LogWarning("Problem deleting autosave screenshot: " + text2 + "\n" + ex3.ToString());
				}
			}
		}
		using (MemoryStream memoryStream = new MemoryStream((int)((float)this.lastUncompressedSize * 1.1f)))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				this.Save(binaryWriter);
				this.lastUncompressedSize = (int)memoryStream.Length;
				try
				{
					using (BinaryWriter binaryWriter2 = new BinaryWriter(File.Open(filename, FileMode.Create)))
					{
						SaveGame.Header header;
						byte[] saveHeader = SaveGame.Instance.GetSaveHeader(isAutoSave, this.compressSaveData, out header);
						binaryWriter2.Write(header.buildVersion);
						binaryWriter2.Write(header.headerSize);
						binaryWriter2.Write(header.headerVersion);
						binaryWriter2.Write(header.compression);
						binaryWriter2.Write(saveHeader);
						KSerialization.Manager.SerializeDirectory(binaryWriter2);
						if (this.compressSaveData)
						{
							SaveLoader.CompressContents(binaryWriter2, memoryStream.GetBuffer(), (int)memoryStream.Length);
						}
						else
						{
							binaryWriter2.Write(memoryStream.ToArray());
						}
						KCrashReporter.MOST_RECENT_SAVEFILE = filename;
						Stats.Print();
					}
				}
				catch (Exception ex4)
				{
					if (ex4 is UnauthorizedAccessException)
					{
						DebugUtil.LogArgs(new object[]
						{
							"UnauthorizedAccessException for " + filename
						});
						((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.CRASHSCREEN.SAVEFAILED, "Unauthorized Access Exception"), null, null, null, null, null, null, null, null);
						return SaveLoader.GetActiveSaveFilePath();
					}
					if (ex4 is IOException)
					{
						DebugUtil.LogArgs(new object[]
						{
							"IOException (probably out of disk space) for " + filename
						});
						((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.CRASHSCREEN.SAVEFAILED, "IOException. You may not have enough free space!"), null, null, null, null, null, null, null, null);
						return SaveLoader.GetActiveSaveFilePath();
					}
					throw ex4;
				}
			}
		}
		if (updateSavePointer)
		{
			SaveLoader.SetActiveSaveFilePath(filename);
		}
		Game.Instance.timelapser.SaveColonyPreview(filename);
		DebugUtil.LogArgs(new object[]
		{
			"Saved to",
			"[" + filename + "]"
		});
		GC.Collect();
		return filename;
	}

	// Token: 0x060045AC RID: 17836 RVA: 0x00189204 File Offset: 0x00187404
	public static SaveGame.GameInfo LoadHeader(string filename, out SaveGame.Header header)
	{
		byte[] array = new byte[512];
		SaveGame.GameInfo header2;
		using (FileStream fileStream = File.OpenRead(filename))
		{
			fileStream.Read(array, 0, 512);
			header2 = SaveGame.GetHeader(new FastReader(array), out header, filename);
		}
		return header2;
	}

	// Token: 0x060045AD RID: 17837 RVA: 0x0018925C File Offset: 0x0018745C
	public bool Load(string filename)
	{
		SaveLoader.SetActiveSaveFilePath(filename);
		try
		{
			KSerialization.Manager.Clear();
			byte[] array = File.ReadAllBytes(filename);
			IReader reader = new FastReader(array);
			SaveGame.Header header;
			this.GameInfo = SaveGame.GetHeader(reader, out header, filename);
			DebugUtil.LogArgs(new object[]
			{
				string.Format("Loading save file: {4}\n headerVersion:{0}, buildVersion:{1}, headerSize:{2}, IsCompressed:{3}", new object[]
				{
					header.headerVersion,
					header.buildVersion,
					header.headerSize,
					header.IsCompressed,
					filename
				})
			});
			DebugUtil.LogArgs(new object[]
			{
				string.Format("GameInfo loaded from save header:\n  numberOfCycles:{0},\n  numberOfDuplicants:{1},\n  baseName:{2},\n  isAutoSave:{3},\n  originalSaveName:{4},\n  clusterId:{5},\n  worldTraits:{6},\n  colonyGuid:{7},\n  saveVersion:{8}.{9}", new object[]
				{
					this.GameInfo.numberOfCycles,
					this.GameInfo.numberOfDuplicants,
					this.GameInfo.baseName,
					this.GameInfo.isAutoSave,
					this.GameInfo.originalSaveName,
					this.GameInfo.clusterId,
					(this.GameInfo.worldTraits != null && this.GameInfo.worldTraits.Length != 0) ? string.Join(", ", this.GameInfo.worldTraits) : "<i>none</i>",
					this.GameInfo.colonyGuid,
					this.GameInfo.saveMajorVersion,
					this.GameInfo.saveMinorVersion
				})
			});
			string originalSaveName = this.GameInfo.originalSaveName;
			if (originalSaveName.Contains("/") || originalSaveName.Contains("\\"))
			{
				string originalSaveFileName = SaveLoader.GetOriginalSaveFileName(originalSaveName);
				SaveGame.GameInfo gameInfo = this.GameInfo;
				gameInfo.originalSaveName = originalSaveFileName;
				this.GameInfo = gameInfo;
				global::Debug.Log(string.Concat(new string[]
				{
					"Migration / Save originalSaveName updated from: `",
					originalSaveName,
					"` => `",
					this.GameInfo.originalSaveName,
					"`"
				}));
			}
			if (this.GameInfo.saveMajorVersion == 7 && this.GameInfo.saveMinorVersion < 4)
			{
				Helper.SetTypeInfoMask((SerializationTypeInfo)191);
			}
			KSerialization.Manager.DeserializeDirectory(reader);
			if (header.IsCompressed)
			{
				int num = array.Length - reader.Position;
				byte[] array2 = new byte[num];
				Array.Copy(array, reader.Position, array2, 0, num);
				byte[] array3 = SaveLoader.DecompressContents(array2);
				this.lastUncompressedSize = array3.Length;
				IReader reader2 = new FastReader(array3);
				this.Load(reader2);
			}
			else
			{
				this.lastUncompressedSize = array.Length;
				this.Load(reader);
			}
			KCrashReporter.MOST_RECENT_SAVEFILE = filename;
			if (this.GameInfo.isAutoSave && !string.IsNullOrEmpty(this.GameInfo.originalSaveName))
			{
				string originalSaveFileName2 = SaveLoader.GetOriginalSaveFileName(this.GameInfo.originalSaveName);
				string text;
				if (SaveLoader.IsSaveCloud(filename))
				{
					string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
					if (cloudSavePrefix != null)
					{
						text = System.IO.Path.Combine(cloudSavePrefix, this.GameInfo.baseName, originalSaveFileName2);
					}
					else
					{
						text = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filename).Replace("auto_save", ""), this.GameInfo.baseName, originalSaveFileName2);
					}
				}
				else
				{
					text = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), this.GameInfo.baseName, originalSaveFileName2);
				}
				if (text != null)
				{
					SaveLoader.SetActiveSaveFilePath(text);
				}
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\n" + ex.Message + "\n" + ex.StackTrace
			});
			Sim.Shutdown();
			SaveLoader.SetActiveSaveFilePath(null);
			return false;
		}
		Stats.Print();
		DebugUtil.LogArgs(new object[]
		{
			"Loaded",
			"[" + filename + "]"
		});
		DebugUtil.LogArgs(new object[]
		{
			"World Seeds",
			string.Concat(new string[]
			{
				"[",
				this.clusterDetailSave.globalWorldSeed.ToString(),
				"/",
				this.clusterDetailSave.globalWorldLayoutSeed.ToString(),
				"/",
				this.clusterDetailSave.globalTerrainSeed.ToString(),
				"/",
				this.clusterDetailSave.globalNoiseSeed.ToString(),
				"]"
			})
		});
		GC.Collect();
		return true;
	}

	// Token: 0x060045AE RID: 17838 RVA: 0x001896DC File Offset: 0x001878DC
	public bool LoadFromWorldGen()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Attempting to start a new game with current world gen"
		});
		WorldGen.LoadSettings(false);
		this.m_clusterLayout = Cluster.Load();
		ListPool<SimSaveFileStructure, SaveLoader>.PooledList pooledList = ListPool<SimSaveFileStructure, SaveLoader>.Allocate();
		this.m_clusterLayout.LoadClusterLayoutSim(pooledList);
		SaveGame.GameInfo gameInfo = this.GameInfo;
		gameInfo.clusterId = this.m_clusterLayout.Id;
		gameInfo.colonyGuid = Guid.NewGuid();
		this.GameInfo = gameInfo;
		if (pooledList.Count != this.m_clusterLayout.worlds.Count)
		{
			global::Debug.LogError("Attempt failed. Failed to load all worlds.");
			pooledList.Recycle();
			return false;
		}
		GridSettings.Reset(this.m_clusterLayout.size.x, this.m_clusterLayout.size.y);
		if (Application.isPlaying)
		{
			Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
		}
		this.clusterDetailSave = new WorldDetailSave();
		foreach (SimSaveFileStructure simSaveFileStructure in pooledList)
		{
			this.clusterDetailSave.globalNoiseSeed = simSaveFileStructure.worldDetail.globalNoiseSeed;
			this.clusterDetailSave.globalTerrainSeed = simSaveFileStructure.worldDetail.globalTerrainSeed;
			this.clusterDetailSave.globalWorldLayoutSeed = simSaveFileStructure.worldDetail.globalWorldLayoutSeed;
			this.clusterDetailSave.globalWorldSeed = simSaveFileStructure.worldDetail.globalWorldSeed;
			Vector2 b = Grid.CellToPos2D(Grid.PosToCell(new Vector2I(simSaveFileStructure.x, simSaveFileStructure.y)));
			foreach (WorldDetailSave.OverworldCell overworldCell in simSaveFileStructure.worldDetail.overworldCells)
			{
				for (int num = 0; num != overworldCell.poly.Vertices.Count; num++)
				{
					List<Vector2> vertices = overworldCell.poly.Vertices;
					int index = num;
					vertices[index] += b;
				}
				overworldCell.poly.RefreshBounds();
			}
			this.clusterDetailSave.overworldCells.AddRange(simSaveFileStructure.worldDetail.overworldCells);
		}
		Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
		SimMessages.CreateSimElementsTable(ElementLoader.elements);
		Sim.AllocateCells(this.m_clusterLayout.size.x, this.m_clusterLayout.size.y, false);
		SimMessages.DefineWorldOffsets((from world in this.m_clusterLayout.worlds
		select new SimMessages.WorldOffsetData
		{
			worldOffsetX = world.WorldOffset.x,
			worldOffsetY = world.WorldOffset.y,
			worldSizeX = world.WorldSize.x,
			worldSizeY = world.WorldSize.y
		}).ToList<SimMessages.WorldOffsetData>());
		SimMessages.CreateDiseaseTable(Db.Get().Diseases);
		Sim.HandleMessage(SimMessageHashes.ClearUnoccupiedCells, 0, null);
		try
		{
			foreach (SimSaveFileStructure simSaveFileStructure2 in pooledList)
			{
				FastReader reader = new FastReader(simSaveFileStructure2.Sim);
				if (Sim.Load(reader) != 0)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"\n--- Error loading save ---\nSimDLL found bad data\n"
					});
					Sim.Shutdown();
					pooledList.Recycle();
					return false;
				}
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("--- Error loading Sim FROM NEW WORLDGEN ---" + ex.Message + "\n" + ex.StackTrace);
			Sim.Shutdown();
			pooledList.Recycle();
			return false;
		}
		global::Debug.Log("Attempt success");
		Sim.Start();
		SceneInitializer.Instance.PostLoadPrefabs();
		SceneInitializer.Instance.NewSaveGamePrefab();
		this.cachedGSD = this.m_clusterLayout.currentWorld.SpawnData;
		this.OnWorldGenComplete.Signal(this.m_clusterLayout);
		OniMetrics.LogEvent(OniMetrics.Event.NewSave, "NewGame", true);
		StoryManager.Instance.InitialSaveSetup();
		ThreadedHttps<KleiMetrics>.Instance.IncrementGameCount();
		OniMetrics.SendEvent(OniMetrics.Event.NewSave, "New Save");
		pooledList.Recycle();
		return true;
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x060045AF RID: 17839 RVA: 0x00189B28 File Offset: 0x00187D28
	// (set) Token: 0x060045B0 RID: 17840 RVA: 0x00189B30 File Offset: 0x00187D30
	public GameSpawnData cachedGSD { get; private set; }

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x060045B1 RID: 17841 RVA: 0x00189B39 File Offset: 0x00187D39
	// (set) Token: 0x060045B2 RID: 17842 RVA: 0x00189B41 File Offset: 0x00187D41
	public WorldDetailSave clusterDetailSave { get; private set; }

	// Token: 0x060045B3 RID: 17843 RVA: 0x00189B4A File Offset: 0x00187D4A
	public void SetWorldDetail(WorldDetailSave worldDetail)
	{
		this.clusterDetailSave = worldDetail;
	}

	// Token: 0x060045B4 RID: 17844 RVA: 0x00189B54 File Offset: 0x00187D54
	private void ReportSaveMetrics(bool is_auto_save)
	{
		if (ThreadedHttps<KleiMetrics>.Instance == null || !ThreadedHttps<KleiMetrics>.Instance.enabled || this.saveManager == null)
		{
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary[GameClock.NewCycleKey] = GameClock.Instance.GetCycle() + 1;
		dictionary["IsAutoSave"] = is_auto_save;
		dictionary["SavedPrefabs"] = this.GetSavedPrefabMetrics();
		dictionary["ResourcesAccessible"] = this.GetWorldInventoryMetrics();
		dictionary["MinionMetrics"] = this.GetMinionMetrics();
		if (is_auto_save)
		{
			dictionary["DailyReport"] = this.GetDailyReportMetrics();
			dictionary["PerformanceMeasurements"] = this.GetPerformanceMeasurements();
			dictionary["AverageFrameTime"] = this.GetFrameTime();
		}
		dictionary["CustomGameSettings"] = CustomGameSettings.Instance.GetSettingsForMetrics();
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(dictionary, "ReportSaveMetrics");
	}

	// Token: 0x060045B5 RID: 17845 RVA: 0x00189C4C File Offset: 0x00187E4C
	private List<SaveLoader.MinionMetricsData> GetMinionMetrics()
	{
		List<SaveLoader.MinionMetricsData> list = new List<SaveLoader.MinionMetricsData>();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if (!(minionIdentity == null))
			{
				Amounts amounts = minionIdentity.gameObject.GetComponent<Modifiers>().amounts;
				List<SaveLoader.MinionAttrFloatData> list2 = new List<SaveLoader.MinionAttrFloatData>(amounts.Count);
				foreach (AmountInstance amountInstance in amounts)
				{
					float value = amountInstance.value;
					if (!float.IsNaN(value) && !float.IsInfinity(value))
					{
						list2.Add(new SaveLoader.MinionAttrFloatData
						{
							Name = amountInstance.modifier.Id,
							Value = amountInstance.value
						});
					}
				}
				MinionResume component = minionIdentity.gameObject.GetComponent<MinionResume>();
				float totalExperienceGained = component.TotalExperienceGained;
				List<string> list3 = new List<string>();
				foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
				{
					if (keyValuePair.Value)
					{
						list3.Add(keyValuePair.Key);
					}
				}
				list.Add(new SaveLoader.MinionMetricsData
				{
					Name = minionIdentity.name,
					Modifiers = list2,
					TotalExperienceGained = totalExperienceGained,
					Skills = list3
				});
			}
		}
		return list;
	}

	// Token: 0x060045B6 RID: 17846 RVA: 0x00189E1C File Offset: 0x0018801C
	private List<SaveLoader.SavedPrefabMetricsData> GetSavedPrefabMetrics()
	{
		Dictionary<Tag, List<SaveLoadRoot>> lists = this.saveManager.GetLists();
		List<SaveLoader.SavedPrefabMetricsData> list = new List<SaveLoader.SavedPrefabMetricsData>(lists.Count);
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in lists)
		{
			Tag key = keyValuePair.Key;
			List<SaveLoadRoot> value = keyValuePair.Value;
			if (value.Count > 0)
			{
				list.Add(new SaveLoader.SavedPrefabMetricsData
				{
					PrefabName = key.ToString(),
					Count = value.Count
				});
			}
		}
		return list;
	}

	// Token: 0x060045B7 RID: 17847 RVA: 0x00189EC8 File Offset: 0x001880C8
	private List<SaveLoader.WorldInventoryMetricsData> GetWorldInventoryMetrics()
	{
		Dictionary<Tag, float> allWorldsAccessibleAmounts = ClusterManager.Instance.GetAllWorldsAccessibleAmounts();
		List<SaveLoader.WorldInventoryMetricsData> list = new List<SaveLoader.WorldInventoryMetricsData>(allWorldsAccessibleAmounts.Count);
		foreach (KeyValuePair<Tag, float> keyValuePair in allWorldsAccessibleAmounts)
		{
			float value = keyValuePair.Value;
			if (!float.IsInfinity(value) && !float.IsNaN(value))
			{
				list.Add(new SaveLoader.WorldInventoryMetricsData
				{
					Name = keyValuePair.Key.ToString(),
					Amount = value
				});
			}
		}
		return list;
	}

	// Token: 0x060045B8 RID: 17848 RVA: 0x00189F74 File Offset: 0x00188174
	private List<SaveLoader.DailyReportMetricsData> GetDailyReportMetrics()
	{
		List<SaveLoader.DailyReportMetricsData> list = new List<SaveLoader.DailyReportMetricsData>();
		int cycle = GameClock.Instance.GetCycle();
		ReportManager.DailyReport dailyReport = ReportManager.Instance.FindReport(cycle);
		if (dailyReport != null)
		{
			foreach (ReportManager.ReportEntry reportEntry in dailyReport.reportEntries)
			{
				SaveLoader.DailyReportMetricsData item = default(SaveLoader.DailyReportMetricsData);
				item.Name = reportEntry.reportType.ToString();
				if (!float.IsInfinity(reportEntry.Net) && !float.IsNaN(reportEntry.Net))
				{
					item.Net = new float?(reportEntry.Net);
				}
				if (SaveLoader.force_infinity)
				{
					item.Net = null;
				}
				if (!float.IsInfinity(reportEntry.Positive) && !float.IsNaN(reportEntry.Positive))
				{
					item.Positive = new float?(reportEntry.Positive);
				}
				if (!float.IsInfinity(reportEntry.Negative) && !float.IsNaN(reportEntry.Negative))
				{
					item.Negative = new float?(reportEntry.Negative);
				}
				list.Add(item);
			}
			list.Add(new SaveLoader.DailyReportMetricsData
			{
				Name = "MinionCount",
				Net = new float?((float)Components.LiveMinionIdentities.Count),
				Positive = new float?(0f),
				Negative = new float?(0f)
			});
		}
		return list;
	}

	// Token: 0x060045B9 RID: 17849 RVA: 0x0018A10C File Offset: 0x0018830C
	private List<SaveLoader.PerformanceMeasurement> GetPerformanceMeasurements()
	{
		List<SaveLoader.PerformanceMeasurement> list = new List<SaveLoader.PerformanceMeasurement>();
		if (Global.Instance != null)
		{
			PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
			list.Add(new SaveLoader.PerformanceMeasurement
			{
				name = "FramesAbove30",
				value = component.NumFramesAbove30
			});
			list.Add(new SaveLoader.PerformanceMeasurement
			{
				name = "FramesBelow30",
				value = component.NumFramesBelow30
			});
			component.Reset();
		}
		return list;
	}

	// Token: 0x060045BA RID: 17850 RVA: 0x0018A194 File Offset: 0x00188394
	private float GetFrameTime()
	{
		PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
		DebugUtil.LogArgs(new object[]
		{
			"Average frame time:",
			1f / component.FPS
		});
		return 1f / component.FPS;
	}

	// Token: 0x04002E21 RID: 11809
	[MyCmpGet]
	private GridSettings gridSettings;

	// Token: 0x04002E23 RID: 11811
	private bool saveFileCorrupt;

	// Token: 0x04002E24 RID: 11812
	private bool compressSaveData = true;

	// Token: 0x04002E25 RID: 11813
	private int lastUncompressedSize;

	// Token: 0x04002E26 RID: 11814
	public bool saveAsText;

	// Token: 0x04002E27 RID: 11815
	public const string MAINMENU_LEVELNAME = "launchscene";

	// Token: 0x04002E28 RID: 11816
	public const string FRONTEND_LEVELNAME = "frontend";

	// Token: 0x04002E29 RID: 11817
	public const string BACKEND_LEVELNAME = "backend";

	// Token: 0x04002E2A RID: 11818
	public const string SAVE_EXTENSION = ".sav";

	// Token: 0x04002E2B RID: 11819
	public const string AUTOSAVE_FOLDER = "auto_save";

	// Token: 0x04002E2C RID: 11820
	public const string CLOUDSAVE_FOLDER = "cloud_save_files";

	// Token: 0x04002E2D RID: 11821
	public const string SAVE_FOLDER = "save_files";

	// Token: 0x04002E2E RID: 11822
	public const int MAX_AUTOSAVE_FILES = 10;

	// Token: 0x04002E30 RID: 11824
	[NonSerialized]
	public SaveManager saveManager;

	// Token: 0x04002E32 RID: 11826
	private Cluster m_clusterLayout;

	// Token: 0x04002E34 RID: 11828
	private const string CorruptFileSuffix = "_";

	// Token: 0x04002E35 RID: 11829
	private const float SAVE_BUFFER_HEAD_ROOM = 0.1f;

	// Token: 0x04002E36 RID: 11830
	private bool mustRestartOnFail;

	// Token: 0x04002E39 RID: 11833
	public const string METRIC_SAVED_PREFAB_KEY = "SavedPrefabs";

	// Token: 0x04002E3A RID: 11834
	public const string METRIC_IS_AUTO_SAVE_KEY = "IsAutoSave";

	// Token: 0x04002E3B RID: 11835
	public const string METRIC_WAS_DEBUG_EVER_USED = "WasDebugEverUsed";

	// Token: 0x04002E3C RID: 11836
	public const string METRIC_IS_SANDBOX_ENABLED = "IsSandboxEnabled";

	// Token: 0x04002E3D RID: 11837
	public const string METRIC_RESOURCES_ACCESSIBLE_KEY = "ResourcesAccessible";

	// Token: 0x04002E3E RID: 11838
	public const string METRIC_DAILY_REPORT_KEY = "DailyReport";

	// Token: 0x04002E3F RID: 11839
	public const string METRIC_MINION_METRICS_KEY = "MinionMetrics";

	// Token: 0x04002E40 RID: 11840
	public const string METRIC_CUSTOM_GAME_SETTINGS = "CustomGameSettings";

	// Token: 0x04002E41 RID: 11841
	public const string METRIC_PERFORMANCE_MEASUREMENTS = "PerformanceMeasurements";

	// Token: 0x04002E42 RID: 11842
	public const string METRIC_FRAME_TIME = "AverageFrameTime";

	// Token: 0x04002E43 RID: 11843
	private static bool force_infinity;

	// Token: 0x020017A2 RID: 6050
	public class FlowUtilityNetworkInstance
	{
		// Token: 0x04006F79 RID: 28537
		public int id = -1;

		// Token: 0x04006F7A RID: 28538
		public SimHashes containedElement = SimHashes.Vacuum;

		// Token: 0x04006F7B RID: 28539
		public float containedMass;

		// Token: 0x04006F7C RID: 28540
		public float containedTemperature;
	}

	// Token: 0x020017A3 RID: 6051
	[SerializationConfig(KSerialization.MemberSerialization.OptOut)]
	public class FlowUtilityNetworkSaver : ISaveLoadable
	{
		// Token: 0x06008F0A RID: 36618 RVA: 0x0032129A File Offset: 0x0031F49A
		public FlowUtilityNetworkSaver()
		{
			this.gas = new List<SaveLoader.FlowUtilityNetworkInstance>();
			this.liquid = new List<SaveLoader.FlowUtilityNetworkInstance>();
		}

		// Token: 0x04006F7D RID: 28541
		public List<SaveLoader.FlowUtilityNetworkInstance> gas;

		// Token: 0x04006F7E RID: 28542
		public List<SaveLoader.FlowUtilityNetworkInstance> liquid;
	}

	// Token: 0x020017A4 RID: 6052
	public struct SaveFileEntry
	{
		// Token: 0x04006F7F RID: 28543
		public string path;

		// Token: 0x04006F80 RID: 28544
		public System.DateTime timeStamp;
	}

	// Token: 0x020017A5 RID: 6053
	public enum SaveType
	{
		// Token: 0x04006F82 RID: 28546
		local,
		// Token: 0x04006F83 RID: 28547
		cloud,
		// Token: 0x04006F84 RID: 28548
		both
	}

	// Token: 0x020017A6 RID: 6054
	private struct MinionAttrFloatData
	{
		// Token: 0x04006F85 RID: 28549
		public string Name;

		// Token: 0x04006F86 RID: 28550
		public float Value;
	}

	// Token: 0x020017A7 RID: 6055
	private struct MinionMetricsData
	{
		// Token: 0x04006F87 RID: 28551
		public string Name;

		// Token: 0x04006F88 RID: 28552
		public List<SaveLoader.MinionAttrFloatData> Modifiers;

		// Token: 0x04006F89 RID: 28553
		public float TotalExperienceGained;

		// Token: 0x04006F8A RID: 28554
		public List<string> Skills;
	}

	// Token: 0x020017A8 RID: 6056
	private struct SavedPrefabMetricsData
	{
		// Token: 0x04006F8B RID: 28555
		public string PrefabName;

		// Token: 0x04006F8C RID: 28556
		public int Count;
	}

	// Token: 0x020017A9 RID: 6057
	private struct WorldInventoryMetricsData
	{
		// Token: 0x04006F8D RID: 28557
		public string Name;

		// Token: 0x04006F8E RID: 28558
		public float Amount;
	}

	// Token: 0x020017AA RID: 6058
	private struct DailyReportMetricsData
	{
		// Token: 0x04006F8F RID: 28559
		public string Name;

		// Token: 0x04006F90 RID: 28560
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Net;

		// Token: 0x04006F91 RID: 28561
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Positive;

		// Token: 0x04006F92 RID: 28562
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Negative;
	}

	// Token: 0x020017AB RID: 6059
	private struct PerformanceMeasurement
	{
		// Token: 0x04006F93 RID: 28563
		public string name;

		// Token: 0x04006F94 RID: 28564
		public float value;
	}
}
