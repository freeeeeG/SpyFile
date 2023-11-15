using System;
using System.Collections.Generic;

// Token: 0x02000BDC RID: 3036
public class SandboxSettings
{
	// Token: 0x06005FDD RID: 24541 RVA: 0x002364DD File Offset: 0x002346DD
	public void AddIntSetting(string prefsKey, Action<int> setAction, int defaultValue)
	{
		this.intSettings.Add(new SandboxSettings.Setting<int>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x06005FDE RID: 24542 RVA: 0x002364F2 File Offset: 0x002346F2
	public int GetIntSetting(string prefsKey)
	{
		return KPlayerPrefs.GetInt(prefsKey);
	}

	// Token: 0x06005FDF RID: 24543 RVA: 0x002364FC File Offset: 0x002346FC
	public void SetIntSetting(string prefsKey, int value)
	{
		SandboxSettings.Setting<int> setting = this.intSettings.Find((SandboxSettings.Setting<int> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No intSetting named: ",
				prefsKey,
				" could be found amongst ",
				this.intSettings.Count.ToString(),
				" int settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x06005FE0 RID: 24544 RVA: 0x0023657F File Offset: 0x0023477F
	public void RestoreIntSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetIntSetting(prefsKey, this.GetIntSetting(prefsKey));
			return;
		}
		this.ForceDefaultIntSetting(prefsKey);
	}

	// Token: 0x06005FE1 RID: 24545 RVA: 0x002365A0 File Offset: 0x002347A0
	public void ForceDefaultIntSetting(string prefsKey)
	{
		this.SetIntSetting(prefsKey, this.intSettings.Find((SandboxSettings.Setting<int> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x06005FE2 RID: 24546 RVA: 0x002365E2 File Offset: 0x002347E2
	public void AddFloatSetting(string prefsKey, Action<float> setAction, float defaultValue)
	{
		this.floatSettings.Add(new SandboxSettings.Setting<float>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x06005FE3 RID: 24547 RVA: 0x002365F7 File Offset: 0x002347F7
	public float GetFloatSetting(string prefsKey)
	{
		return KPlayerPrefs.GetFloat(prefsKey);
	}

	// Token: 0x06005FE4 RID: 24548 RVA: 0x00236600 File Offset: 0x00234800
	public void SetFloatSetting(string prefsKey, float value)
	{
		SandboxSettings.Setting<float> setting = this.floatSettings.Find((SandboxSettings.Setting<float> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No KPlayerPrefs float setting named: ",
				prefsKey,
				" could be found amongst ",
				this.floatSettings.Count.ToString(),
				" float settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x06005FE5 RID: 24549 RVA: 0x00236683 File Offset: 0x00234883
	public void RestoreFloatSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetFloatSetting(prefsKey, this.GetFloatSetting(prefsKey));
			return;
		}
		this.ForceDefaultFloatSetting(prefsKey);
	}

	// Token: 0x06005FE6 RID: 24550 RVA: 0x002366A4 File Offset: 0x002348A4
	public void ForceDefaultFloatSetting(string prefsKey)
	{
		this.SetFloatSetting(prefsKey, this.floatSettings.Find((SandboxSettings.Setting<float> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x06005FE7 RID: 24551 RVA: 0x002366E6 File Offset: 0x002348E6
	public void AddStringSetting(string prefsKey, Action<string> setAction, string defaultValue)
	{
		this.stringSettings.Add(new SandboxSettings.Setting<string>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x06005FE8 RID: 24552 RVA: 0x002366FB File Offset: 0x002348FB
	public string GetStringSetting(string prefsKey)
	{
		return KPlayerPrefs.GetString(prefsKey);
	}

	// Token: 0x06005FE9 RID: 24553 RVA: 0x00236704 File Offset: 0x00234904
	public void SetStringSetting(string prefsKey, string value)
	{
		SandboxSettings.Setting<string> setting = this.stringSettings.Find((SandboxSettings.Setting<string> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No KPlayerPrefs string setting named: ",
				prefsKey,
				" could be found amongst ",
				this.stringSettings.Count.ToString(),
				" settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x06005FEA RID: 24554 RVA: 0x00236787 File Offset: 0x00234987
	public void RestoreStringSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetStringSetting(prefsKey, this.GetStringSetting(prefsKey));
			return;
		}
		this.ForceDefaultStringSetting(prefsKey);
	}

	// Token: 0x06005FEB RID: 24555 RVA: 0x002367A8 File Offset: 0x002349A8
	public void ForceDefaultStringSetting(string prefsKey)
	{
		this.SetStringSetting(prefsKey, this.stringSettings.Find((SandboxSettings.Setting<string> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x06005FEC RID: 24556 RVA: 0x002367EC File Offset: 0x002349EC
	public SandboxSettings()
	{
		this.AddStringSetting("SandboxTools.SelectedEntity", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedEntity", data);
			this.OnChangeEntity();
		}, "MushBar");
		this.AddIntSetting("SandboxTools.SelectedElement", delegate(int data)
		{
			KPlayerPrefs.SetInt("SandboxTools.SelectedElement", data);
			this.OnChangeElement(this.hasRestoredElement);
			this.hasRestoredElement = true;
		}, (int)ElementLoader.GetElementIndex(SimHashes.Oxygen));
		this.AddStringSetting("SandboxTools.SelectedDisease", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedDisease", data);
			this.OnChangeDisease();
		}, Db.Get().Diseases.FoodGerms.Id);
		this.AddIntSetting("SandboxTools.DiseaseCount", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandboxTools.DiseaseCount", val);
			this.OnChangeDiseaseCount();
		}, 0);
		this.AddIntSetting("SandboxTools.BrushSize", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandboxTools.BrushSize", val);
			this.OnChangeBrushSize();
		}, 1);
		this.AddFloatSetting("SandboxTools.NoiseScale", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.NoiseScale", val);
			this.OnChangeNoiseScale();
		}, 1f);
		this.AddFloatSetting("SandboxTools.NoiseDensity", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.NoiseDensity", val);
			this.OnChangeNoiseDensity();
		}, 1f);
		this.AddFloatSetting("SandboxTools.Mass", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.Mass", val);
			this.OnChangeMass();
		}, 1f);
		this.AddFloatSetting("SandbosTools.Temperature", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.Temperature", val);
			this.OnChangeTemperature();
		}, 300f);
		this.AddFloatSetting("SandbosTools.TemperatureAdditive", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.TemperatureAdditive", val);
			this.OnChangeAdditiveTemperature();
		}, 5f);
		this.AddFloatSetting("SandbosTools.StressAdditive", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.StressAdditive", val);
			this.OnChangeAdditiveStress();
		}, 50f);
		this.AddIntSetting("SandbosTools.MoraleAdjustment", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandbosTools.MoraleAdjustment", val);
			this.OnChangeMoraleAdjustment();
		}, 50);
	}

	// Token: 0x06005FED RID: 24557 RVA: 0x00236980 File Offset: 0x00234B80
	public void RestorePrefs()
	{
		foreach (SandboxSettings.Setting<int> setting in this.intSettings)
		{
			this.RestoreIntSetting(setting.PrefsKey);
		}
		foreach (SandboxSettings.Setting<float> setting2 in this.floatSettings)
		{
			this.RestoreFloatSetting(setting2.PrefsKey);
		}
		foreach (SandboxSettings.Setting<string> setting3 in this.stringSettings)
		{
			this.RestoreStringSetting(setting3.PrefsKey);
		}
	}

	// Token: 0x0400413C RID: 16700
	private List<SandboxSettings.Setting<int>> intSettings = new List<SandboxSettings.Setting<int>>();

	// Token: 0x0400413D RID: 16701
	private List<SandboxSettings.Setting<float>> floatSettings = new List<SandboxSettings.Setting<float>>();

	// Token: 0x0400413E RID: 16702
	private List<SandboxSettings.Setting<string>> stringSettings = new List<SandboxSettings.Setting<string>>();

	// Token: 0x0400413F RID: 16703
	public bool InstantBuild = true;

	// Token: 0x04004140 RID: 16704
	private bool hasRestoredElement;

	// Token: 0x04004141 RID: 16705
	public Action<bool> OnChangeElement;

	// Token: 0x04004142 RID: 16706
	public System.Action OnChangeMass;

	// Token: 0x04004143 RID: 16707
	public System.Action OnChangeDisease;

	// Token: 0x04004144 RID: 16708
	public System.Action OnChangeDiseaseCount;

	// Token: 0x04004145 RID: 16709
	public System.Action OnChangeEntity;

	// Token: 0x04004146 RID: 16710
	public System.Action OnChangeBrushSize;

	// Token: 0x04004147 RID: 16711
	public System.Action OnChangeNoiseScale;

	// Token: 0x04004148 RID: 16712
	public System.Action OnChangeNoiseDensity;

	// Token: 0x04004149 RID: 16713
	public System.Action OnChangeTemperature;

	// Token: 0x0400414A RID: 16714
	public System.Action OnChangeAdditiveTemperature;

	// Token: 0x0400414B RID: 16715
	public System.Action OnChangeAdditiveStress;

	// Token: 0x0400414C RID: 16716
	public System.Action OnChangeMoraleAdjustment;

	// Token: 0x0400414D RID: 16717
	public const string KEY_SELECTED_ENTITY = "SandboxTools.SelectedEntity";

	// Token: 0x0400414E RID: 16718
	public const string KEY_SELECTED_ELEMENT = "SandboxTools.SelectedElement";

	// Token: 0x0400414F RID: 16719
	public const string KEY_SELECTED_DISEASE = "SandboxTools.SelectedDisease";

	// Token: 0x04004150 RID: 16720
	public const string KEY_DISEASE_COUNT = "SandboxTools.DiseaseCount";

	// Token: 0x04004151 RID: 16721
	public const string KEY_BRUSH_SIZE = "SandboxTools.BrushSize";

	// Token: 0x04004152 RID: 16722
	public const string KEY_NOISE_SCALE = "SandboxTools.NoiseScale";

	// Token: 0x04004153 RID: 16723
	public const string KEY_NOISE_DENSITY = "SandboxTools.NoiseDensity";

	// Token: 0x04004154 RID: 16724
	public const string KEY_MASS = "SandboxTools.Mass";

	// Token: 0x04004155 RID: 16725
	public const string KEY_TEMPERATURE = "SandbosTools.Temperature";

	// Token: 0x04004156 RID: 16726
	public const string KEY_TEMPERATURE_ADDITIVE = "SandbosTools.TemperatureAdditive";

	// Token: 0x04004157 RID: 16727
	public const string KEY_STRESS_ADDITIVE = "SandbosTools.StressAdditive";

	// Token: 0x04004158 RID: 16728
	public const string KEY_MORALE_ADJUSTMENT = "SandbosTools.MoraleAdjustment";

	// Token: 0x02001B2E RID: 6958
	public class Setting<T>
	{
		// Token: 0x06009938 RID: 39224 RVA: 0x00343CE3 File Offset: 0x00341EE3
		public Setting(string prefsKey, Action<T> setAction, T defaultValue)
		{
			this.prefsKey = prefsKey;
			this.SetAction = setAction;
			this.defaultValue = defaultValue;
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06009939 RID: 39225 RVA: 0x00343D00 File Offset: 0x00341F00
		public string PrefsKey
		{
			get
			{
				return this.prefsKey;
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (set) Token: 0x0600993A RID: 39226 RVA: 0x00343D08 File Offset: 0x00341F08
		public T Value
		{
			set
			{
				this.SetAction(value);
			}
		}

		// Token: 0x04007BF0 RID: 31728
		private string prefsKey;

		// Token: 0x04007BF1 RID: 31729
		private Action<T> SetAction;

		// Token: 0x04007BF2 RID: 31730
		public T defaultValue;
	}
}
