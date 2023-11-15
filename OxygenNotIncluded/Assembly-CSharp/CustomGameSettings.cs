using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Database;
using Klei.CustomSettings;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02000742 RID: 1858
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CustomGameSettings")]
public class CustomGameSettings : KMonoBehaviour
{
	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x06003326 RID: 13094 RVA: 0x0010F7AD File Offset: 0x0010D9AD
	public static CustomGameSettings Instance
	{
		get
		{
			return CustomGameSettings.instance;
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x06003327 RID: 13095 RVA: 0x0010F7B4 File Offset: 0x0010D9B4
	public IReadOnlyDictionary<string, string> CurrentStoryLevelsBySetting
	{
		get
		{
			return this.currentStoryLevelsBySetting;
		}
	}

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06003328 RID: 13096 RVA: 0x0010F7BC File Offset: 0x0010D9BC
	// (remove) Token: 0x06003329 RID: 13097 RVA: 0x0010F7F4 File Offset: 0x0010D9F4
	public event Action<SettingConfig, SettingLevel> OnQualitySettingChanged;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x0600332A RID: 13098 RVA: 0x0010F82C File Offset: 0x0010DA2C
	// (remove) Token: 0x0600332B RID: 13099 RVA: 0x0010F864 File Offset: 0x0010DA64
	public event Action<SettingConfig, SettingLevel> OnStorySettingChanged;

	// Token: 0x0600332C RID: 13100 RVA: 0x0010F89C File Offset: 0x0010DA9C
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 6))
		{
			this.customGameMode = (this.is_custom_game ? CustomGameSettings.CustomGameMode.Custom : CustomGameSettings.CustomGameMode.Survival);
		}
		if (this.CurrentQualityLevelsBySetting.ContainsKey("CarePackages "))
		{
			if (!this.CurrentQualityLevelsBySetting.ContainsKey(CustomGameSettingConfigs.CarePackages.id))
			{
				this.CurrentQualityLevelsBySetting.Add(CustomGameSettingConfigs.CarePackages.id, this.CurrentQualityLevelsBySetting["CarePackages "]);
			}
			this.CurrentQualityLevelsBySetting.Remove("CarePackages ");
		}
		this.CurrentQualityLevelsBySetting.Remove("Expansion1Active");
		if (!DlcManager.IsExpansion1Active())
		{
			foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
			{
				SettingConfig value = keyValuePair.Value;
				if (!DlcManager.IsVanillaId(value.required_content))
				{
					global::Debug.Assert(value.required_content == "EXPANSION1_ID", "A new expansion setting has been added, but its deserialization has not been implemented.");
					if (this.CurrentQualityLevelsBySetting.ContainsKey(value.id))
					{
						global::Debug.Assert(this.CurrentQualityLevelsBySetting[value.id] == value.missing_content_default, string.Format("This save has Expansion1 content disabled, but its expansion1-dependent setting {0} is set to {1}", value.id, this.CurrentQualityLevelsBySetting[value.id]));
					}
					else
					{
						this.SetQualitySetting(value, value.missing_content_default);
					}
				}
			}
		}
		string clusterDefaultName;
		this.CurrentQualityLevelsBySetting.TryGetValue(CustomGameSettingConfigs.ClusterLayout.id, out clusterDefaultName);
		if (clusterDefaultName.IsNullOrWhiteSpace())
		{
			if (!DlcManager.IsExpansion1Active())
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Deserializing CustomGameSettings.ClusterLayout: ClusterLayout is blank, using default cluster instead"
				});
			}
			clusterDefaultName = WorldGenSettings.ClusterDefaultName;
			this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, clusterDefaultName);
		}
		if (!SettingsCache.clusterLayouts.clusterCache.ContainsKey(clusterDefaultName))
		{
			global::Debug.Log("Deserializing CustomGameSettings.ClusterLayout: '" + clusterDefaultName + "' doesn't exist in the clusterCache, trying to rewrite path to scoped path.");
			string text = SettingsCache.GetScope("EXPANSION1_ID") + clusterDefaultName;
			if (SettingsCache.clusterLayouts.clusterCache.ContainsKey(text))
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"Deserializing CustomGameSettings.ClusterLayout: Success in rewriting ClusterLayout '",
					clusterDefaultName,
					"' to '",
					text,
					"'"
				}));
				this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, text);
			}
			else
			{
				global::Debug.LogWarning("Deserializing CustomGameSettings.ClusterLayout: Failed to find cluster '" + clusterDefaultName + "' including the scoped path, setting to default cluster name.");
				global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
				this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, WorldGenSettings.ClusterDefaultName);
			}
		}
		this.CheckCustomGameMode();
	}

	// Token: 0x0600332D RID: 13101 RVA: 0x0010FB64 File Offset: 0x0010DD64
	protected override void OnPrefabInit()
	{
		bool flag = DlcManager.IsExpansion1Active();
		CustomGameSettings.instance = this;
		this.AddQualitySettingConfig(CustomGameSettingConfigs.ClusterLayout);
		this.AddQualitySettingConfig(CustomGameSettingConfigs.WorldgenSeed);
		this.AddQualitySettingConfig(CustomGameSettingConfigs.ImmuneSystem);
		this.AddQualitySettingConfig(CustomGameSettingConfigs.CalorieBurn);
		this.AddQualitySettingConfig(CustomGameSettingConfigs.Morale);
		this.AddQualitySettingConfig(CustomGameSettingConfigs.Durability);
		this.AddQualitySettingConfig(CustomGameSettingConfigs.MeteorShowers);
		if (flag)
		{
			this.AddQualitySettingConfig(CustomGameSettingConfigs.Radiation);
		}
		this.AddQualitySettingConfig(CustomGameSettingConfigs.Stress);
		this.AddQualitySettingConfig(CustomGameSettingConfigs.StressBreaks);
		this.AddQualitySettingConfig(CustomGameSettingConfigs.CarePackages);
		this.AddQualitySettingConfig(CustomGameSettingConfigs.SandboxMode);
		this.AddQualitySettingConfig(CustomGameSettingConfigs.FastWorkersMode);
		if (SaveLoader.GetCloudSavesAvailable())
		{
			this.AddQualitySettingConfig(CustomGameSettingConfigs.SaveToCloud);
		}
		if (flag)
		{
			this.AddQualitySettingConfig(CustomGameSettingConfigs.Teleporters);
		}
		foreach (Story story in Db.Get().Stories.resources)
		{
			long coordinate_dimension = (long)((story.kleiUseOnlyCoordinateOffset == -1) ? -1 : global::Util.IntPow(3, story.kleiUseOnlyCoordinateOffset));
			int num = (story.kleiUseOnlyCoordinateOffset == -1) ? -1 : 3;
			SettingConfig config = new ListSettingConfig(story.Id, "", "", new List<SettingLevel>
			{
				new SettingLevel("Disabled", "", "", 0L, null),
				new SettingLevel("Guaranteed", "", "", 1L, null)
			}, "Disabled", "Disabled", coordinate_dimension, (long)num, false, false, "", "", false);
			this.AddStorySettingConfig(config);
		}
		this.VerifySettingCoordinates();
	}

	// Token: 0x0600332E RID: 13102 RVA: 0x0010FD24 File Offset: 0x0010DF24
	public void DisableAllStories()
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.StorySettings)
		{
			this.SetStorySetting(keyValuePair.Value, false);
		}
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x0010FD80 File Offset: 0x0010DF80
	public void SetSurvivalDefaults()
	{
		this.customGameMode = CustomGameSettings.CustomGameMode.Survival;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			this.SetQualitySetting(keyValuePair.Value, keyValuePair.Value.GetDefaultLevelId());
		}
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x0010FDEC File Offset: 0x0010DFEC
	public void SetNosweatDefaults()
	{
		this.customGameMode = CustomGameSettings.CustomGameMode.Nosweat;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			this.SetQualitySetting(keyValuePair.Value, keyValuePair.Value.GetNoSweatDefaultLevelId());
		}
	}

	// Token: 0x06003331 RID: 13105 RVA: 0x0010FE58 File Offset: 0x0010E058
	public SettingLevel CycleSettingLevel(ListSettingConfig config, int direction)
	{
		this.SetQualitySetting(config, config.CycleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id], direction));
		return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
	}

	// Token: 0x06003332 RID: 13106 RVA: 0x0010FE90 File Offset: 0x0010E090
	public SettingLevel ToggleSettingLevel(ToggleSettingConfig config)
	{
		this.SetQualitySetting(config, config.ToggleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id]));
		return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
	}

	// Token: 0x06003333 RID: 13107 RVA: 0x0010FEC7 File Offset: 0x0010E0C7
	public void SetQualitySetting(SettingConfig config, string value)
	{
		this.SetQualitySetting(config, value, true);
	}

	// Token: 0x06003334 RID: 13108 RVA: 0x0010FED2 File Offset: 0x0010E0D2
	public void SetQualitySetting(SettingConfig config, string value, bool notify)
	{
		this.CurrentQualityLevelsBySetting[config.id] = value;
		this.CheckCustomGameMode();
		if (notify && this.OnQualitySettingChanged != null)
		{
			this.OnQualitySettingChanged(config, this.GetCurrentQualitySetting(config));
		}
	}

	// Token: 0x06003335 RID: 13109 RVA: 0x0010FF0C File Offset: 0x0010E10C
	private void CheckCustomGameMode()
	{
		bool flag = true;
		bool flag2 = true;
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			if (!this.QualitySettings.ContainsKey(keyValuePair.Key))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Quality settings missing " + keyValuePair.Key
				});
			}
			else if (this.QualitySettings[keyValuePair.Key].triggers_custom_game)
			{
				if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].GetDefaultLevelId())
				{
					flag = false;
				}
				if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].GetNoSweatDefaultLevelId())
				{
					flag2 = false;
				}
				if (!flag && !flag2)
				{
					break;
				}
			}
		}
		CustomGameSettings.CustomGameMode customGameMode;
		if (flag)
		{
			customGameMode = CustomGameSettings.CustomGameMode.Survival;
		}
		else if (flag2)
		{
			customGameMode = CustomGameSettings.CustomGameMode.Nosweat;
		}
		else
		{
			customGameMode = CustomGameSettings.CustomGameMode.Custom;
		}
		if (customGameMode != this.customGameMode)
		{
			DebugUtil.LogArgs(new object[]
			{
				"Game mode changed from",
				this.customGameMode,
				"to",
				customGameMode
			});
			this.customGameMode = customGameMode;
		}
	}

	// Token: 0x06003336 RID: 13110 RVA: 0x00110060 File Offset: 0x0010E260
	public SettingLevel GetCurrentQualitySetting(SettingConfig setting)
	{
		return this.GetCurrentQualitySetting(setting.id);
	}

	// Token: 0x06003337 RID: 13111 RVA: 0x00110070 File Offset: 0x0010E270
	public SettingLevel GetCurrentQualitySetting(string setting_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetDefaultLevelId());
		}
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetNoSweatDefaultLevelId());
		}
		if (!this.CurrentQualityLevelsBySetting.ContainsKey(setting_id))
		{
			this.CurrentQualityLevelsBySetting[setting_id] = this.QualitySettings[setting_id].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsContentActive(settingConfig.required_content) ? this.CurrentQualityLevelsBySetting[setting_id] : settingConfig.GetDefaultLevelId();
		return this.QualitySettings[setting_id].GetLevel(level_id);
	}

	// Token: 0x06003338 RID: 13112 RVA: 0x00110124 File Offset: 0x0010E324
	public string GetCurrentQualitySettingLevelId(SettingConfig config)
	{
		return this.CurrentQualityLevelsBySetting[config.id];
	}

	// Token: 0x06003339 RID: 13113 RVA: 0x00110138 File Offset: 0x0010E338
	public string GetSettingLevelLabel(string setting_id, string level_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (settingConfig != null)
		{
			SettingLevel level = settingConfig.GetLevel(level_id);
			if (level != null)
			{
				return level.label;
			}
		}
		global::Debug.LogWarning("No label string for setting: " + setting_id + " level: " + level_id);
		return "";
	}

	// Token: 0x0600333A RID: 13114 RVA: 0x00110184 File Offset: 0x0010E384
	public string GetQualitySettingLevelTooltip(string setting_id, string level_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (settingConfig != null)
		{
			SettingLevel level = settingConfig.GetLevel(level_id);
			if (level != null)
			{
				return level.tooltip;
			}
		}
		global::Debug.LogWarning("No tooltip string for setting: " + setting_id + " level: " + level_id);
		return "";
	}

	// Token: 0x0600333B RID: 13115 RVA: 0x001101D0 File Offset: 0x0010E3D0
	public void AddQualitySettingConfig(SettingConfig config)
	{
		this.QualitySettings.Add(config.id, config);
		if (!this.CurrentQualityLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.CurrentQualityLevelsBySetting[config.id]))
		{
			this.CurrentQualityLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x0600333C RID: 13116 RVA: 0x00110234 File Offset: 0x0010E434
	public ClusterLayout GetCurrentClusterLayout()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (currentQualitySetting == null)
		{
			return null;
		}
		return SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
	}

	// Token: 0x0600333D RID: 13117 RVA: 0x00110268 File Offset: 0x0010E468
	public void LoadClusters()
	{
		Dictionary<string, ClusterLayout> clusterCache = SettingsCache.clusterLayouts.clusterCache;
		List<SettingLevel> list = new List<SettingLevel>(clusterCache.Count);
		foreach (KeyValuePair<string, ClusterLayout> keyValuePair in clusterCache)
		{
			StringEntry stringEntry;
			string label = Strings.TryGet(new StringKey(keyValuePair.Value.name), out stringEntry) ? stringEntry.ToString() : keyValuePair.Value.name;
			string tooltip = Strings.TryGet(new StringKey(keyValuePair.Value.description), out stringEntry) ? stringEntry.ToString() : keyValuePair.Value.description;
			list.Add(new SettingLevel(keyValuePair.Key, label, tooltip, 0L, null));
		}
		CustomGameSettingConfigs.ClusterLayout.StompLevels(list, WorldGenSettings.ClusterDefaultName, WorldGenSettings.ClusterDefaultName);
	}

	// Token: 0x0600333E RID: 13118 RVA: 0x00110358 File Offset: 0x0010E558
	public void Print()
	{
		string text = "Custom Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair.Key,
				"=",
				keyValuePair.Value,
				","
			});
		}
		global::Debug.Log(text);
		text = "Story Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair2 in this.currentStoryLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair2.Key,
				"=",
				keyValuePair2.Value,
				","
			});
		}
		global::Debug.Log(text);
	}

	// Token: 0x0600333F RID: 13119 RVA: 0x00110460 File Offset: 0x0010E660
	private bool AllValuesMatch(Dictionary<string, string> data, CustomGameSettings.CustomGameMode mode)
	{
		bool result = true;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			if (!(keyValuePair.Key == CustomGameSettingConfigs.WorldgenSeed.id))
			{
				string b = null;
				if (mode != CustomGameSettings.CustomGameMode.Survival)
				{
					if (mode == CustomGameSettings.CustomGameMode.Nosweat)
					{
						b = keyValuePair.Value.GetNoSweatDefaultLevelId();
					}
				}
				else
				{
					b = keyValuePair.Value.GetDefaultLevelId();
				}
				if (data.ContainsKey(keyValuePair.Key) && data[keyValuePair.Key] != b)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x00110514 File Offset: 0x0010E714
	public List<CustomGameSettings.MetricSettingsData> GetSettingsForMetrics()
	{
		List<CustomGameSettings.MetricSettingsData> list = new List<CustomGameSettings.MetricSettingsData>();
		list.Add(new CustomGameSettings.MetricSettingsData
		{
			Name = "CustomGameMode",
			Value = this.customGameMode.ToString()
		});
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			list.Add(new CustomGameSettings.MetricSettingsData
			{
				Name = keyValuePair.Key,
				Value = keyValuePair.Value
			});
		}
		CustomGameSettings.MetricSettingsData item = new CustomGameSettings.MetricSettingsData
		{
			Name = "CustomGameModeActual",
			Value = CustomGameSettings.CustomGameMode.Custom.ToString()
		};
		foreach (object obj in Enum.GetValues(typeof(CustomGameSettings.CustomGameMode)))
		{
			CustomGameSettings.CustomGameMode customGameMode = (CustomGameSettings.CustomGameMode)obj;
			if (customGameMode != CustomGameSettings.CustomGameMode.Custom && this.AllValuesMatch(this.CurrentQualityLevelsBySetting, customGameMode))
			{
				item.Value = customGameMode.ToString();
				break;
			}
		}
		list.Add(item);
		return list;
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x00110680 File Offset: 0x0010E880
	public bool VerifySettingCoordinates()
	{
		bool flag = this.VerifySettingsDictionary(this.QualitySettings);
		bool flag2 = this.VerifySettingsDictionary(this.StorySettings);
		return flag || flag2;
	}

	// Token: 0x06003342 RID: 13122 RVA: 0x001106A8 File Offset: 0x0010E8A8
	private bool VerifySettingsDictionary(Dictionary<string, SettingConfig> configs)
	{
		Dictionary<long, string> dictionary = new Dictionary<long, string>();
		bool result = false;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in configs)
		{
			if (keyValuePair.Value.coordinate_dimension < 0L || keyValuePair.Value.coordinate_dimension_width < 0L)
			{
				if (keyValuePair.Value.coordinate_dimension >= 0L || keyValuePair.Value.coordinate_dimension_width >= 0L)
				{
					result = true;
					global::Debug.Assert(false, keyValuePair.Value.id + ": Both coordinate dimension props must be unset (-1) if either is unset.");
				}
			}
			else
			{
				List<SettingLevel> levels = keyValuePair.Value.GetLevels();
				if (keyValuePair.Value.coordinate_dimension_width < (long)levels.Count)
				{
					result = true;
					global::Debug.Assert(false, string.Concat(new string[]
					{
						keyValuePair.Value.id,
						": Range between coordinate min and max insufficient for all levels (",
						keyValuePair.Value.coordinate_dimension_width.ToString(),
						"<",
						levels.Count.ToString(),
						")"
					}));
				}
				foreach (SettingLevel settingLevel in levels)
				{
					long key = keyValuePair.Value.coordinate_dimension * settingLevel.coordinate_offset;
					string text = keyValuePair.Value.id + " > " + settingLevel.id;
					if (settingLevel.coordinate_offset < 0L)
					{
						result = true;
						global::Debug.Assert(false, text + ": Level coordinate offset must be >= 0");
					}
					else if (settingLevel.coordinate_offset == 0L)
					{
						if (settingLevel.id != keyValuePair.Value.GetDefaultLevelId())
						{
							result = true;
							global::Debug.Assert(false, text + ": Only the default level should have a coordinate offset of 0");
						}
					}
					else if (settingLevel.coordinate_offset > keyValuePair.Value.coordinate_dimension_width)
					{
						result = true;
						global::Debug.Assert(false, text + ": level coordinate must be <= dimension width");
					}
					else
					{
						string str;
						bool flag = !dictionary.TryGetValue(key, out str);
						dictionary[key] = text;
						if (settingLevel.id == keyValuePair.Value.GetDefaultLevelId())
						{
							result = true;
							global::Debug.Assert(false, text + ": Default level must be coordinate 0");
						}
						if (!flag)
						{
							result = true;
							global::Debug.Assert(false, text + ": Combined coordinate conflicts with another coordinate (" + str + "). Ensure this SettingConfig's min and max don't overlap with another SettingConfig's");
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06003343 RID: 13123 RVA: 0x00110970 File Offset: 0x0010EB70
	public static string[] ParseSettingCoordinate(string coord)
	{
		string[] array = CustomGameSettings.ParseCoordinate(coord, "(.*)-(\\d*)-(.*)-(.*)");
		if (array.Length == 1)
		{
			array = CustomGameSettings.ParseCoordinate(coord, "(.*)-(\\d*)-(.*)");
		}
		return array;
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x0011099C File Offset: 0x0010EB9C
	private static string[] ParseCoordinate(string coord, string pattern)
	{
		Match match = new Regex(pattern).Match(coord);
		string[] array = new string[match.Groups.Count];
		for (int i = 0; i < match.Groups.Count; i++)
		{
			array[i] = match.Groups[i].Value;
		}
		return array;
	}

	// Token: 0x06003345 RID: 13125 RVA: 0x001109F4 File Offset: 0x0010EBF4
	public string GetSettingsCoordinate()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (currentQualitySetting == null)
		{
			DebugUtil.DevLogError("GetSettingsCoordinate: clusterLayoutSetting is null, returning '0' coordinate");
			CustomGameSettings.Instance.Print();
			global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
			return "0-0-0-0";
		}
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
		SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
		string otherSettingsCode = this.GetOtherSettingsCode();
		string storyTraitSettingsCode = this.GetStoryTraitSettingsCode();
		return string.Format("{0}-{1}-{2}-{3}", new object[]
		{
			clusterData.GetCoordinatePrefix(),
			currentQualitySetting2.id,
			otherSettingsCode,
			storyTraitSettingsCode
		});
	}

	// Token: 0x06003346 RID: 13126 RVA: 0x00110AB4 File Offset: 0x0010ECB4
	public void ParseAndApplySettingsCode(string code)
	{
		long num = this.Base36toBase10(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			SettingConfig settingConfig = this.QualitySettings[keyValuePair.Key];
			if (settingConfig.coordinate_dimension >= 0L && settingConfig.coordinate_dimension_width >= 0L)
			{
				long num2 = 0L;
				long num3 = settingConfig.coordinate_dimension * settingConfig.coordinate_dimension_width;
				long num4 = num;
				if (num4 >= num3)
				{
					long num5 = num4 / num3 * num3;
					num4 -= num5;
				}
				if (num4 >= settingConfig.coordinate_dimension)
				{
					num2 = num4 / settingConfig.coordinate_dimension;
				}
				foreach (SettingLevel settingLevel in settingConfig.GetLevels())
				{
					if (settingLevel.coordinate_offset == num2)
					{
						dictionary[settingConfig] = settingLevel.id;
						break;
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair2 in dictionary)
		{
			this.SetQualitySetting(keyValuePair2.Key, keyValuePair2.Value);
		}
	}

	// Token: 0x06003347 RID: 13127 RVA: 0x00110C30 File Offset: 0x0010EE30
	private string GetOtherSettingsCode()
	{
		long num = 0L;
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			SettingConfig settingConfig;
			this.QualitySettings.TryGetValue(keyValuePair.Key, out settingConfig);
			if (settingConfig != null && settingConfig.coordinate_dimension >= 0L && settingConfig.coordinate_dimension_width >= 0L)
			{
				SettingLevel level = settingConfig.GetLevel(keyValuePair.Value);
				long num2 = settingConfig.coordinate_dimension * level.coordinate_offset;
				num += num2;
			}
		}
		return this.Base10toBase36(num);
	}

	// Token: 0x06003348 RID: 13128 RVA: 0x00110CD8 File Offset: 0x0010EED8
	private long Base36toBase10(string input)
	{
		if (input == "0")
		{
			return 0L;
		}
		long num = 0L;
		for (int i = input.Length - 1; i >= 0; i--)
		{
			num *= 36L;
			long num2 = (long)this.hexChars.IndexOf(input[i]);
			num += num2;
		}
		DebugUtil.LogArgs(new object[]
		{
			"tried converting",
			input,
			", got",
			num,
			"and returns to",
			this.Base10toBase36(num)
		});
		return num;
	}

	// Token: 0x06003349 RID: 13129 RVA: 0x00110D64 File Offset: 0x0010EF64
	private string Base10toBase36(long input)
	{
		if (input == 0L)
		{
			return "0";
		}
		long num = input;
		string text = "";
		while (num > 0L)
		{
			text += this.hexChars[(int)(num % 36L)].ToString();
			num /= 36L;
		}
		return text;
	}

	// Token: 0x0600334A RID: 13130 RVA: 0x00110DB0 File Offset: 0x0010EFB0
	public void AddStorySettingConfig(SettingConfig config)
	{
		this.StorySettings.Add(config.id, config);
		if (!this.currentStoryLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.currentStoryLevelsBySetting[config.id]))
		{
			this.currentStoryLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x0600334B RID: 13131 RVA: 0x00110E11 File Offset: 0x0010F011
	public void SetStorySetting(SettingConfig config, string value)
	{
		this.SetStorySetting(config, value == "Guaranteed");
	}

	// Token: 0x0600334C RID: 13132 RVA: 0x00110E25 File Offset: 0x0010F025
	public void SetStorySetting(SettingConfig config, bool value)
	{
		this.currentStoryLevelsBySetting[config.id] = (value ? "Guaranteed" : "Disabled");
		if (this.OnStorySettingChanged != null)
		{
			this.OnStorySettingChanged(config, this.GetCurrentStoryTraitSetting(config));
		}
	}

	// Token: 0x0600334D RID: 13133 RVA: 0x00110E64 File Offset: 0x0010F064
	public void ParseAndApplyStoryTraitSettingsCode(string code)
	{
		long num = this.Base36toBase10(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (KeyValuePair<string, string> keyValuePair in this.currentStoryLevelsBySetting)
		{
			SettingConfig settingConfig = this.StorySettings[keyValuePair.Key];
			if (settingConfig.coordinate_dimension >= 0L && settingConfig.coordinate_dimension_width >= 0L)
			{
				long num2 = 0L;
				long num3 = settingConfig.coordinate_dimension * settingConfig.coordinate_dimension_width;
				long num4 = num;
				if (num4 >= num3)
				{
					long num5 = num4 / num3 * num3;
					num4 -= num5;
				}
				if (num4 >= settingConfig.coordinate_dimension)
				{
					num2 = num4 / settingConfig.coordinate_dimension;
				}
				foreach (SettingLevel settingLevel in settingConfig.GetLevels())
				{
					if (settingLevel.coordinate_offset == num2)
					{
						dictionary[settingConfig] = settingLevel.id;
						break;
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair2 in dictionary)
		{
			this.SetStorySetting(keyValuePair2.Key, keyValuePair2.Value);
		}
	}

	// Token: 0x0600334E RID: 13134 RVA: 0x00110FE0 File Offset: 0x0010F1E0
	private string GetStoryTraitSettingsCode()
	{
		long num = 0L;
		foreach (KeyValuePair<string, string> keyValuePair in this.currentStoryLevelsBySetting)
		{
			SettingConfig settingConfig;
			this.StorySettings.TryGetValue(keyValuePair.Key, out settingConfig);
			if (settingConfig != null && settingConfig.coordinate_dimension >= 0L && settingConfig.coordinate_dimension_width >= 0L)
			{
				SettingLevel level = settingConfig.GetLevel(keyValuePair.Value);
				long num2 = settingConfig.coordinate_dimension * level.coordinate_offset;
				num += num2;
			}
		}
		return this.Base10toBase36(num);
	}

	// Token: 0x0600334F RID: 13135 RVA: 0x00111088 File Offset: 0x0010F288
	public SettingLevel GetCurrentStoryTraitSetting(SettingConfig setting)
	{
		return this.GetCurrentStoryTraitSetting(setting.id);
	}

	// Token: 0x06003350 RID: 13136 RVA: 0x00111098 File Offset: 0x0010F298
	public SettingLevel GetCurrentStoryTraitSetting(string settingId)
	{
		SettingConfig settingConfig = this.StorySettings[settingId];
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetDefaultLevelId());
		}
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetNoSweatDefaultLevelId());
		}
		if (!this.currentStoryLevelsBySetting.ContainsKey(settingId))
		{
			this.currentStoryLevelsBySetting[settingId] = this.StorySettings[settingId].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsContentActive(settingConfig.required_content) ? this.currentStoryLevelsBySetting[settingId] : settingConfig.GetDefaultLevelId();
		return this.StorySettings[settingId].GetLevel(level_id);
	}

	// Token: 0x06003351 RID: 13137 RVA: 0x0011114C File Offset: 0x0010F34C
	public List<string> GetCurrentStories()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> keyValuePair in this.currentStoryLevelsBySetting)
		{
			if (this.IsStoryActive(keyValuePair.Key, keyValuePair.Value))
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x06003352 RID: 13138 RVA: 0x001111C4 File Offset: 0x0010F3C4
	public bool IsStoryActive(string id, string level)
	{
		SettingConfig settingConfig;
		return this.StorySettings.TryGetValue(id, out settingConfig) && settingConfig != null && level == "Guaranteed";
	}

	// Token: 0x04001EBF RID: 7871
	private static CustomGameSettings instance;

	// Token: 0x04001EC0 RID: 7872
	private const int NUM_STORY_LEVELS = 3;

	// Token: 0x04001EC1 RID: 7873
	public const string STORY_DISABLED_LEVEL = "Disabled";

	// Token: 0x04001EC2 RID: 7874
	public const string STORY_GUARANTEED_LEVEL = "Guaranteed";

	// Token: 0x04001EC3 RID: 7875
	[Serialize]
	public bool is_custom_game;

	// Token: 0x04001EC4 RID: 7876
	[Serialize]
	public CustomGameSettings.CustomGameMode customGameMode;

	// Token: 0x04001EC5 RID: 7877
	[Serialize]
	private Dictionary<string, string> CurrentQualityLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x04001EC6 RID: 7878
	private Dictionary<string, string> currentStoryLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x04001EC7 RID: 7879
	public Dictionary<string, SettingConfig> QualitySettings = new Dictionary<string, SettingConfig>();

	// Token: 0x04001EC8 RID: 7880
	public Dictionary<string, SettingConfig> StorySettings = new Dictionary<string, SettingConfig>();

	// Token: 0x04001ECB RID: 7883
	private const string storyCoordinatePattern = "(.*)-(\\d*)-(.*)-(.*)";

	// Token: 0x04001ECC RID: 7884
	private const string noStoryCoordinatePattern = "(.*)-(\\d*)-(.*)";

	// Token: 0x04001ECD RID: 7885
	private string hexChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	// Token: 0x020014E0 RID: 5344
	public enum CustomGameMode
	{
		// Token: 0x040066B3 RID: 26291
		Survival,
		// Token: 0x040066B4 RID: 26292
		Nosweat,
		// Token: 0x040066B5 RID: 26293
		Custom = 255
	}

	// Token: 0x020014E1 RID: 5345
	public struct MetricSettingsData
	{
		// Token: 0x040066B6 RID: 26294
		public string Name;

		// Token: 0x040066B7 RID: 26295
		public string Value;
	}
}
