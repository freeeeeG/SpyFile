using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000725 RID: 1829
public class MetaGameProgress : MonoBehaviour, IByteSerialization
{
	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x060022B4 RID: 8884 RVA: 0x000A682C File Offset: 0x000A4C2C
	public uint SaveVersion
	{
		get
		{
			return 1U;
		}
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x060022B5 RID: 8885 RVA: 0x000A682F File Offset: 0x000A4C2F
	public GlobalSave SaveData
	{
		get
		{
			return this.m_saveData;
		}
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x060022B6 RID: 8886 RVA: 0x000A6837 File Offset: 0x000A4C37
	public OptionsData AccessOptionsData
	{
		get
		{
			return this.m_optionsData;
		}
	}

	// Token: 0x060022B7 RID: 8887 RVA: 0x000A6840 File Offset: 0x000A4C40
	private void Awake()
	{
		this.m_dlcManager = GameUtils.RequireManager<DLCManager>();
		this.m_achievementManager = GameUtils.RequireManager<OvercookedAchievementManager>();
		this.m_optionsData.OnAwake();
		this.m_saveData.Set("AvatarUnlocks", this.m_initialAvatarUnlocks);
		this.m_allAvatarDirectories = this.CollectAllAvatarDirectories();
		this.m_combinedAvatarDirectory = this.CombineAvatarDirectories(this.m_allAvatarDirectories);
	}

	// Token: 0x060022B8 RID: 8888 RVA: 0x000A68A2 File Offset: 0x000A4CA2
	private void Update()
	{
		this.m_optionsData.Update();
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x060022B9 RID: 8889 RVA: 0x000A68AF File Offset: 0x000A4CAF
	public AvatarDirectoryData AvatarDirectory
	{
		get
		{
			return this.m_combinedAvatarDirectory;
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x060022BA RID: 8890 RVA: 0x000A68B7 File Offset: 0x000A4CB7
	public int ByteSaveSize
	{
		get
		{
			return this.ByteSave().Length;
		}
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x000A68C1 File Offset: 0x000A4CC1
	private AvatarDirectoryData[] CollectAllAvatarDirectories()
	{
		return this.m_AvatarDirectoryData.AllData.AllRemoved_Predicate((AvatarDirectoryData x) => x == null);
	}

	// Token: 0x060022BC RID: 8892 RVA: 0x000A68F0 File Offset: 0x000A4CF0
	private AvatarDirectoryData CombineAvatarDirectories(AvatarDirectoryData[] avatarDirectories)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < avatarDirectories.Length; i++)
		{
			num += avatarDirectories[i].Avatars.Length;
			num2 += avatarDirectories[i].Colours.Length;
		}
		AvatarDirectoryData avatarDirectoryData = ScriptableObject.CreateInstance<AvatarDirectoryData>();
		avatarDirectoryData.Avatars = new ChefAvatarData[num];
		avatarDirectoryData.Colours = new ChefColourData[num2];
		int num3 = 0;
		for (int j = 0; j < avatarDirectories.Length; j++)
		{
			for (int k = 0; k < avatarDirectories[j].Avatars.Length; k++)
			{
				avatarDirectoryData.Avatars[num3 + k] = avatarDirectories[j].Avatars[k];
			}
			num3 += avatarDirectories[j].Avatars.Length;
		}
		List<ChefColourData> list = new List<ChefColourData>(5);
		foreach (AvatarDirectoryData avatarDirectoryData2 in avatarDirectories)
		{
			for (int m = 0; m < avatarDirectoryData2.Colours.Length; m++)
			{
				ChefColourData item = avatarDirectoryData2.Colours[m];
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		avatarDirectoryData.Colours = list.ToArray();
		return avatarDirectoryData;
	}

	// Token: 0x060022BD RID: 8893 RVA: 0x000A6A25 File Offset: 0x000A4E25
	public void SetLastPlayedTheme(SceneDirectoryData.LevelTheme _theme)
	{
		this.m_saveData.Set("LastThemePlayed", (int)_theme);
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x000A6A38 File Offset: 0x000A4E38
	public SceneDirectoryData.LevelTheme GetLastPlayedTheme()
	{
		int num;
		this.m_saveData.Get("LastThemePlayed", out num, -1);
		if (num > 22)
		{
			num = -1;
		}
		return (SceneDirectoryData.LevelTheme)num;
	}

	// Token: 0x060022BF RID: 8895 RVA: 0x000A6A64 File Offset: 0x000A4E64
	public void SetMetaDialogShown(MetaGameProgress.MetaDialogType _type)
	{
		this.m_saveData.Set("MetaDialogShown_" + (int)_type, true);
	}

	// Token: 0x060022C0 RID: 8896 RVA: 0x000A6A84 File Offset: 0x000A4E84
	public bool HasShownMetaDialog(MetaGameProgress.MetaDialogType _type)
	{
		bool result;
		this.m_saveData.Get("MetaDialogShown_" + (int)_type, out result, false);
		return result;
	}

	// Token: 0x060022C1 RID: 8897 RVA: 0x000A6AB1 File Offset: 0x000A4EB1
	public void SetDLCSeen(DLCFrontendData _data)
	{
		this.m_saveData.Set("DLC_" + _data.m_DLCID, true);
	}

	// Token: 0x060022C2 RID: 8898 RVA: 0x000A6AD4 File Offset: 0x000A4ED4
	public bool GetDLCSeen(DLCFrontendData _data)
	{
		bool result;
		this.m_saveData.Get("DLC_" + _data.m_DLCID, out result, false);
		return result;
	}

	// Token: 0x060022C3 RID: 8899 RVA: 0x000A6B08 File Offset: 0x000A4F08
	private static string GetLastSaveSlotKey(int _dlcNum)
	{
		string result = "LastSaveUsed";
		if (_dlcNum != -1)
		{
			_dlcNum = Mathf.Max(0, _dlcNum);
			result = "LastSaveUsed_DLC" + _dlcNum;
		}
		return result;
	}

	// Token: 0x060022C4 RID: 8900 RVA: 0x000A6B3D File Offset: 0x000A4F3D
	public void SetLastSaveSlot(int _dlcNum, int _slotNum)
	{
		this.m_saveData.Set(MetaGameProgress.GetLastSaveSlotKey(_dlcNum), _slotNum);
	}

	// Token: 0x060022C5 RID: 8901 RVA: 0x000A6B54 File Offset: 0x000A4F54
	public int GetLastSaveSlot(int _dlcNum)
	{
		int result;
		this.m_saveData.Get(MetaGameProgress.GetLastSaveSlotKey(_dlcNum), out result, -1);
		return result;
	}

	// Token: 0x060022C6 RID: 8902 RVA: 0x000A6B78 File Offset: 0x000A4F78
	public ChefAvatarData[] GetUnlockedAvatars()
	{
		List<ChefAvatarData> list = new List<ChefAvatarData>();
		int[] array;
		this.m_saveData.Get("AvatarUnlocks", out array, new int[0]);
		for (int i = 0; i < this.m_allAvatarDirectories.Length; i++)
		{
			AvatarDirectoryData avatarDirectoryData = this.m_allAvatarDirectories[i];
			for (int j = 0; j < avatarDirectoryData.Avatars.Length; j++)
			{
				ChefAvatarData chefAvatarData = avatarDirectoryData.Avatars[j];
				if (chefAvatarData != null && chefAvatarData.ActuallyAllowed && chefAvatarData.IsAvailableOnThisPlatform())
				{
					bool flag = false;
					if (chefAvatarData.ForDlc != null)
					{
						flag = (this.m_dlcManager.IsDLCAvailable(chefAvatarData.ForDlc) && (chefAvatarData.ForDlc.m_type == DLCType.Avatars || chefAvatarData.ForDlc.m_InstallUnlocksAvatars));
					}
					int storageId = GameProgress.UnlockData.AvatarDataType.GetStorageId(avatarDirectoryData, j);
					if (array.Contains(storageId) || flag || (DebugManager.Instance != null && DebugManager.Instance.GetOption("Unlock all chefs")))
					{
						list.Add(chefAvatarData);
					}
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x060022C7 RID: 8903 RVA: 0x000A6CB8 File Offset: 0x000A50B8
	public bool IsAvatarUnlocked(int _id)
	{
		int[] array;
		this.m_saveData.Get("AvatarUnlocks", out array, new int[0]);
		return array.Contains(_id);
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x000A6CE8 File Offset: 0x000A50E8
	public void UnlockAvatar(int _id)
	{
		int[] array;
		this.m_saveData.Get("AvatarUnlocks", out array, new int[0]);
		array = array.Union(new int[]
		{
			_id
		});
		this.m_saveData.Set("AvatarUnlocks", array);
		OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
		if (overcookedAchievementManager != null)
		{
			overcookedAchievementManager.AddIDStat(20, _id, ControlPadInput.PadNum.One);
		}
	}

	// Token: 0x060022C9 RID: 8905 RVA: 0x000A6D4C File Offset: 0x000A514C
	public static bool Validate(byte[] _bytes)
	{
		return new GlobalSave().ByteLoad(_bytes);
	}

	// Token: 0x060022CA RID: 8906 RVA: 0x000A6D59 File Offset: 0x000A5159
	public IOption GetOption(OptionsData.OptionType _type)
	{
		return this.m_optionsData.GetOption(_type);
	}

	// Token: 0x060022CB RID: 8907 RVA: 0x000A6D67 File Offset: 0x000A5167
	public int GetOptionsInCategory(OptionsData.Categories _category)
	{
		return this.m_optionsData.GetOptionsInCategory(_category);
	}

	// Token: 0x060022CC RID: 8908 RVA: 0x000A6D75 File Offset: 0x000A5175
	public IEnumerable<IOption> IterateOverCategory(OptionsData.Categories _category)
	{
		return this.m_optionsData.IterateOverCategory(_category);
	}

	// Token: 0x060022CD RID: 8909 RVA: 0x000A6D83 File Offset: 0x000A5183
	public void ConsoleReset()
	{
		this.m_optionsData.Unload();
	}

	// Token: 0x060022CE RID: 8910 RVA: 0x000A6D90 File Offset: 0x000A5190
	public byte[] ByteSave()
	{
		this.m_optionsData.AddToSave();
		PCPadInputProvider.SaveBindings(this.m_saveData);
		this.m_saveData.Set("VERSION", this.SaveVersion);
		return this.m_saveData.ByteSave();
	}

	// Token: 0x060022CF RID: 8911 RVA: 0x000A6DCC File Offset: 0x000A51CC
	public bool ByteLoad(byte[] _data)
	{
		if (!this.m_saveData.ByteLoad(_data))
		{
			return false;
		}
		int num;
		this.m_saveData.Get("VERSION", out num, 1);
		if (num != (int)this.SaveVersion)
		{
			return false;
		}
		this.m_optionsData.LoadFromSave();
		PCPadInputProvider.LoadBindings(this.m_saveData);
		this.m_achievementManager.Init();
		return true;
	}

	// Token: 0x04001AA3 RID: 6819
	[SerializeField]
	private MetaGameProgress.DLCSerializedAvatarDirectoryData m_AvatarDirectoryData = new MetaGameProgress.DLCSerializedAvatarDirectoryData();

	// Token: 0x04001AA4 RID: 6820
	private AvatarDirectoryData[] m_allAvatarDirectories = new AvatarDirectoryData[0];

	// Token: 0x04001AA5 RID: 6821
	private AvatarDirectoryData m_combinedAvatarDirectory;

	// Token: 0x04001AA6 RID: 6822
	[SerializeField]
	[ArrayIndex("m_AvatarDirectoryData.m_Data", "Avatars", SerializationUtils.RootType.Top)]
	private int[] m_initialAvatarUnlocks = new int[0];

	// Token: 0x04001AA7 RID: 6823
	private GlobalSave m_saveData = new GlobalSave();

	// Token: 0x04001AA8 RID: 6824
	private const string c_versionTag = "VERSION";

	// Token: 0x04001AA9 RID: 6825
	[SerializeField]
	private OptionsData m_optionsData = new OptionsData();

	// Token: 0x04001AAA RID: 6826
	private DLCManager m_dlcManager;

	// Token: 0x04001AAB RID: 6827
	private OvercookedAchievementManager m_achievementManager;

	// Token: 0x04001AAC RID: 6828
	private const string AVATAR_UNLOCKS_KEY = "AvatarUnlocks";

	// Token: 0x04001AAD RID: 6829
	private const string LAST_SAVE_USED = "LastSaveUsed";

	// Token: 0x04001AAE RID: 6830
	private const string LAST_THEME_PLAYED = "LastThemePlayed";

	// Token: 0x04001AAF RID: 6831
	private const string META_DIALOG_SHOWN_TAG = "MetaDialogShown_";

	// Token: 0x04001AB0 RID: 6832
	private const string DLC_SEEN_TAG = "DLC_";

	// Token: 0x02000726 RID: 1830
	[Serializable]
	private class DLCSerializedAvatarDirectoryData : DLCSerializedData<AvatarDirectoryData>
	{
	}

	// Token: 0x02000727 RID: 1831
	public enum MetaDialogType
	{
		// Token: 0x04001AB3 RID: 6835
		PracticeMode,
		// Token: 0x04001AB4 RID: 6836
		HordeMode,
		// Token: 0x04001AB5 RID: 6837
		COUNT
	}
}
