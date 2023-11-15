using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Klei.CustomSettings;
using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x0200094F RID: 2383
[SerializationConfig(KSerialization.MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SaveGame")]
public class SaveGame : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x06004568 RID: 17768 RVA: 0x001877D3 File Offset: 0x001859D3
	// (set) Token: 0x06004569 RID: 17769 RVA: 0x001877DB File Offset: 0x001859DB
	public int AutoSaveCycleInterval
	{
		get
		{
			return this.autoSaveCycleInterval;
		}
		set
		{
			this.autoSaveCycleInterval = value;
		}
	}

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x0600456A RID: 17770 RVA: 0x001877E4 File Offset: 0x001859E4
	// (set) Token: 0x0600456B RID: 17771 RVA: 0x001877EC File Offset: 0x001859EC
	public Vector2I TimelapseResolution
	{
		get
		{
			return this.timelapseResolution;
		}
		set
		{
			this.timelapseResolution = value;
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x0600456C RID: 17772 RVA: 0x001877F5 File Offset: 0x001859F5
	public string BaseName
	{
		get
		{
			return this.baseName;
		}
	}

	// Token: 0x0600456D RID: 17773 RVA: 0x001877FD File Offset: 0x001859FD
	public static void DestroyInstance()
	{
		SaveGame.Instance = null;
	}

	// Token: 0x0600456E RID: 17774 RVA: 0x00187808 File Offset: 0x00185A08
	protected override void OnPrefabInit()
	{
		SaveGame.Instance = this;
		new ColonyRationMonitor.Instance(this).StartSM();
		this.entombedItemManager = base.gameObject.AddComponent<EntombedItemManager>();
		this.worldGenSpawner = base.gameObject.AddComponent<WorldGenSpawner>();
		base.gameObject.AddOrGetDef<GameplaySeasonManager.Def>();
		base.gameObject.AddOrGetDef<ClusterFogOfWarManager.Def>();
	}

	// Token: 0x0600456F RID: 17775 RVA: 0x00187860 File Offset: 0x00185A60
	[OnSerializing]
	private void OnSerialize()
	{
		this.speed = SpeedControlScreen.Instance.GetSpeed();
	}

	// Token: 0x06004570 RID: 17776 RVA: 0x00187872 File Offset: 0x00185A72
	[OnDeserializing]
	private void OnDeserialize()
	{
		this.baseName = SaveLoader.Instance.GameInfo.baseName;
	}

	// Token: 0x06004571 RID: 17777 RVA: 0x00187889 File Offset: 0x00185A89
	public int GetSpeed()
	{
		return this.speed;
	}

	// Token: 0x06004572 RID: 17778 RVA: 0x00187894 File Offset: 0x00185A94
	public byte[] GetSaveHeader(bool isAutoSave, bool isCompressed, out SaveGame.Header header)
	{
		string originalSaveFileName = SaveLoader.GetOriginalSaveFileName(SaveLoader.GetActiveSaveFilePath());
		string s = JsonConvert.SerializeObject(new SaveGame.GameInfo(GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, this.baseName, isAutoSave, originalSaveFileName, SaveLoader.Instance.GameInfo.clusterId, SaveLoader.Instance.GameInfo.worldTraits, SaveLoader.Instance.GameInfo.colonyGuid, DlcManager.GetHighestActiveDlcId(), this.sandboxEnabled));
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		header = default(SaveGame.Header);
		header.buildVersion = 577063U;
		header.headerSize = bytes.Length;
		header.headerVersion = 1U;
		header.compression = (isCompressed ? 1 : 0);
		return bytes;
	}

	// Token: 0x06004573 RID: 17779 RVA: 0x0018794E File Offset: 0x00185B4E
	public static string GetSaveUniqueID(SaveGame.GameInfo info)
	{
		if (!(info.colonyGuid != Guid.Empty))
		{
			return info.baseName + "/" + info.clusterId;
		}
		return info.colonyGuid.ToString();
	}

	// Token: 0x06004574 RID: 17780 RVA: 0x0018798C File Offset: 0x00185B8C
	public static global::Tuple<SaveGame.Header, SaveGame.GameInfo> GetFileInfo(string filename)
	{
		try
		{
			SaveGame.Header a;
			SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(filename, out a);
			if (gameInfo.saveMajorVersion >= 7)
			{
				return new global::Tuple<SaveGame.Header, SaveGame.GameInfo>(a, gameInfo);
			}
		}
		catch (Exception obj)
		{
			global::Debug.LogWarning("Exception while loading " + filename);
			global::Debug.LogWarning(obj);
		}
		return null;
	}

	// Token: 0x06004575 RID: 17781 RVA: 0x001879E4 File Offset: 0x00185BE4
	public static SaveGame.GameInfo GetHeader(IReader br, out SaveGame.Header header, string debugFileName)
	{
		header = default(SaveGame.Header);
		header.buildVersion = br.ReadUInt32();
		header.headerSize = br.ReadInt32();
		header.headerVersion = br.ReadUInt32();
		if (1U <= header.headerVersion)
		{
			header.compression = br.ReadInt32();
		}
		byte[] data = br.ReadBytes(header.headerSize);
		if (header.headerSize == 0 && !SaveGame.debug_SaveFileHeaderBlank_sent)
		{
			SaveGame.debug_SaveFileHeaderBlank_sent = true;
			global::Debug.LogWarning("SaveFileHeaderBlank - " + debugFileName);
		}
		SaveGame.GameInfo gameInfo = SaveGame.GetGameInfo(data);
		if (gameInfo.IsVersionOlderThan(7, 14) && gameInfo.worldTraits != null)
		{
			string[] worldTraits = gameInfo.worldTraits;
			for (int i = 0; i < worldTraits.Length; i++)
			{
				worldTraits[i] = worldTraits[i].Replace('\\', '/');
			}
		}
		if (gameInfo.IsVersionOlderThan(7, 20))
		{
			gameInfo.dlcId = "";
		}
		return gameInfo;
	}

	// Token: 0x06004576 RID: 17782 RVA: 0x00187AB9 File Offset: 0x00185CB9
	public static SaveGame.GameInfo GetGameInfo(byte[] data)
	{
		return JsonConvert.DeserializeObject<SaveGame.GameInfo>(Encoding.UTF8.GetString(data));
	}

	// Token: 0x06004577 RID: 17783 RVA: 0x00187ACB File Offset: 0x00185CCB
	public void SetBaseName(string newBaseName)
	{
		if (string.IsNullOrEmpty(newBaseName))
		{
			global::Debug.LogWarning("Cannot give the base an empty name");
			return;
		}
		this.baseName = newBaseName;
	}

	// Token: 0x06004578 RID: 17784 RVA: 0x00187AE7 File Offset: 0x00185CE7
	protected override void OnSpawn()
	{
		ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
		Game.Instance.Trigger(-1917495436, null);
	}

	// Token: 0x06004579 RID: 17785 RVA: 0x00187B04 File Offset: 0x00185D04
	public List<global::Tuple<string, TextStyleSetting>> GetColonyToolTip()
	{
		List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		ClusterLayout clusterLayout;
		SettingsCache.clusterLayouts.clusterCache.TryGetValue(currentQualitySetting.id, out clusterLayout);
		list.Add(new global::Tuple<string, TextStyleSetting>(this.baseName, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		if (DlcManager.IsExpansion1Active())
		{
			StringEntry entry = Strings.Get(clusterLayout.name);
			list.Add(new global::Tuple<string, TextStyleSetting>(entry, ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		if (GameClock.Instance != null)
		{
			list.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
			list.Add(new global::Tuple<string, TextStyleSetting>(string.Format(UI.ASTEROIDCLOCK.CYCLES_OLD, GameUtil.GetCurrentCycle()), ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			list.Add(new global::Tuple<string, TextStyleSetting>(string.Format(UI.ASTEROIDCLOCK.TIME_PLAYED, (GameClock.Instance.GetTimePlayedInSeconds() / 3600f).ToString("0.00")), ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		int cameraActiveCluster = CameraController.Instance.cameraActiveCluster;
		WorldContainer world = ClusterManager.Instance.GetWorld(cameraActiveCluster);
		list.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
		if (DlcManager.IsExpansion1Active())
		{
			list.Add(new global::Tuple<string, TextStyleSetting>(world.GetComponent<ClusterGridEntity>().Name, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		}
		else
		{
			StringEntry entry2 = Strings.Get(clusterLayout.name);
			list.Add(new global::Tuple<string, TextStyleSetting>(entry2, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		}
		if (SaveLoader.Instance.GameInfo.worldTraits != null && SaveLoader.Instance.GameInfo.worldTraits.Length != 0)
		{
			string[] worldTraits = SaveLoader.Instance.GameInfo.worldTraits;
			for (int i = 0; i < worldTraits.Length; i++)
			{
				WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(worldTraits[i], false);
				if (cachedWorldTrait != null)
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(Strings.Get(cachedWorldTrait.name), ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
				else
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.MISSING_TRAIT, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
			}
		}
		else if (world.WorldTraitIds != null)
		{
			foreach (string name in world.WorldTraitIds)
			{
				WorldTrait cachedWorldTrait2 = SettingsCache.GetCachedWorldTrait(name, false);
				if (cachedWorldTrait2 != null)
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(Strings.Get(cachedWorldTrait2.name), ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
				else
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.MISSING_TRAIT, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
			}
			if (world.WorldTraitIds.Count == 0)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.NO_TRAITS.NAME_SHORTHAND, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			}
		}
		return list;
	}

	// Token: 0x04002E14 RID: 11796
	[Serialize]
	private int speed;

	// Token: 0x04002E15 RID: 11797
	[Serialize]
	public List<Tag> expandedResourceTags = new List<Tag>();

	// Token: 0x04002E16 RID: 11798
	[Serialize]
	public int minGermCountForDisinfect = 10000;

	// Token: 0x04002E17 RID: 11799
	[Serialize]
	public bool enableAutoDisinfect = true;

	// Token: 0x04002E18 RID: 11800
	[Serialize]
	public bool sandboxEnabled;

	// Token: 0x04002E19 RID: 11801
	[Serialize]
	private int autoSaveCycleInterval = 1;

	// Token: 0x04002E1A RID: 11802
	[Serialize]
	private Vector2I timelapseResolution = new Vector2I(512, 768);

	// Token: 0x04002E1B RID: 11803
	private string baseName;

	// Token: 0x04002E1C RID: 11804
	public static SaveGame Instance;

	// Token: 0x04002E1D RID: 11805
	public EntombedItemManager entombedItemManager;

	// Token: 0x04002E1E RID: 11806
	public WorldGenSpawner worldGenSpawner;

	// Token: 0x04002E1F RID: 11807
	[MyCmpReq]
	public MaterialSelectorSerializer materialSelectorSerializer;

	// Token: 0x04002E20 RID: 11808
	private static bool debug_SaveFileHeaderBlank_sent;

	// Token: 0x020017A0 RID: 6048
	public struct Header
	{
		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06008F05 RID: 36613 RVA: 0x003211D4 File Offset: 0x0031F3D4
		public bool IsCompressed
		{
			get
			{
				return this.compression != 0;
			}
		}

		// Token: 0x04006F69 RID: 28521
		public uint buildVersion;

		// Token: 0x04006F6A RID: 28522
		public int headerSize;

		// Token: 0x04006F6B RID: 28523
		public uint headerVersion;

		// Token: 0x04006F6C RID: 28524
		public int compression;
	}

	// Token: 0x020017A1 RID: 6049
	public struct GameInfo
	{
		// Token: 0x06008F06 RID: 36614 RVA: 0x003211E0 File Offset: 0x0031F3E0
		public GameInfo(int numberOfCycles, int numberOfDuplicants, string baseName, bool isAutoSave, string originalSaveName, string clusterId, string[] worldTraits, Guid colonyGuid, string dlcId, bool sandboxEnabled = false)
		{
			this.numberOfCycles = numberOfCycles;
			this.numberOfDuplicants = numberOfDuplicants;
			this.baseName = baseName;
			this.isAutoSave = isAutoSave;
			this.originalSaveName = originalSaveName;
			this.clusterId = clusterId;
			this.worldTraits = worldTraits;
			this.colonyGuid = colonyGuid;
			this.sandboxEnabled = sandboxEnabled;
			this.dlcId = dlcId;
			this.saveMajorVersion = 7;
			this.saveMinorVersion = 32;
		}

		// Token: 0x06008F07 RID: 36615 RVA: 0x00321249 File Offset: 0x0031F449
		public bool IsVersionOlderThan(int major, int minor)
		{
			return this.saveMajorVersion < major || (this.saveMajorVersion == major && this.saveMinorVersion < minor);
		}

		// Token: 0x06008F08 RID: 36616 RVA: 0x0032126A File Offset: 0x0031F46A
		public bool IsVersionExactly(int major, int minor)
		{
			return this.saveMajorVersion == major && this.saveMinorVersion == minor;
		}

		// Token: 0x04006F6D RID: 28525
		public int numberOfCycles;

		// Token: 0x04006F6E RID: 28526
		public int numberOfDuplicants;

		// Token: 0x04006F6F RID: 28527
		public string baseName;

		// Token: 0x04006F70 RID: 28528
		public bool isAutoSave;

		// Token: 0x04006F71 RID: 28529
		public string originalSaveName;

		// Token: 0x04006F72 RID: 28530
		public int saveMajorVersion;

		// Token: 0x04006F73 RID: 28531
		public int saveMinorVersion;

		// Token: 0x04006F74 RID: 28532
		public string clusterId;

		// Token: 0x04006F75 RID: 28533
		public string[] worldTraits;

		// Token: 0x04006F76 RID: 28534
		public bool sandboxEnabled;

		// Token: 0x04006F77 RID: 28535
		public Guid colonyGuid;

		// Token: 0x04006F78 RID: 28536
		public string dlcId;
	}
}
