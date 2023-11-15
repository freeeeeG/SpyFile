using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
[Serializable]
public class Setting
{
	// Token: 0x17000042 RID: 66
	// (get) Token: 0x0600006A RID: 106 RVA: 0x000040CC File Offset: 0x000022CC
	public static Setting Inst
	{
		get
		{
			if (GameData.inst == null)
			{
				return null;
			}
			return GameData.inst.setting;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x0600006B RID: 107 RVA: 0x000040E7 File Offset: 0x000022E7
	public bool IfFullScrren
	{
		get
		{
			return this.setBools[0];
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x0600006C RID: 108 RVA: 0x000040F1 File Offset: 0x000022F1
	public bool Show_Battle_Ability
	{
		get
		{
			return this.setBools[1];
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x0600006D RID: 109 RVA: 0x000040FB File Offset: 0x000022FB
	public bool Show_Battle_FactorBattle
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x0600006E RID: 110 RVA: 0x000040FB File Offset: 0x000022FB
	public bool Show_Battle_Upgrade
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x0600006F RID: 111 RVA: 0x000040FE File Offset: 0x000022FE
	public bool Option_BulletParticle
	{
		get
		{
			return this.setBools[4];
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000070 RID: 112 RVA: 0x00004108 File Offset: 0x00002308
	public bool Option_EnemyBlastParticle
	{
		get
		{
			return this.setBools[5];
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000071 RID: 113 RVA: 0x00004112 File Offset: 0x00002312
	public bool Option_SoundEffectOn
	{
		get
		{
			return this.setBools[6];
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000072 RID: 114 RVA: 0x0000411C File Offset: 0x0000231C
	public bool Option_SoundBGMOn
	{
		get
		{
			return this.setBools[7];
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x06000073 RID: 115 RVA: 0x00004126 File Offset: 0x00002326
	public bool Option_VerticalSyn
	{
		get
		{
			return this.setBools[9];
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x06000074 RID: 116 RVA: 0x00004131 File Offset: 0x00002331
	public bool Option_BackgroundParticle
	{
		get
		{
			return this.setBools[10];
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06000075 RID: 117 RVA: 0x0000413C File Offset: 0x0000233C
	public bool Option_NoToolTipInBattle
	{
		get
		{
			return this.setBools[11];
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000076 RID: 118 RVA: 0x00004147 File Offset: 0x00002347
	public bool Option_ShowDamageText
	{
		get
		{
			return this.setBools[12];
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000077 RID: 119 RVA: 0x00004152 File Offset: 0x00002352
	public bool Option_DynamicFPSadjust
	{
		get
		{
			return this.setBools[13];
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000078 RID: 120 RVA: 0x0000415D File Offset: 0x0000235D
	public bool Option_ShowFPS
	{
		get
		{
			return this.setBools[14];
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000079 RID: 121 RVA: 0x00004168 File Offset: 0x00002368
	public bool Option_Network
	{
		get
		{
			return this.setBools[15];
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600007A RID: 122 RVA: 0x00004173 File Offset: 0x00002373
	public bool Option_BulletOptimize
	{
		get
		{
			return this.setBools[16];
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x0600007B RID: 123 RVA: 0x0000417E File Offset: 0x0000237E
	public bool Option_PositionIndicator
	{
		get
		{
			return this.setBools[17];
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x0600007C RID: 124 RVA: 0x00004189 File Offset: 0x00002389
	public bool Option_SimpleHP
	{
		get
		{
			return this.setBools[18];
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x0600007D RID: 125 RVA: 0x00004194 File Offset: 0x00002394
	public bool Option_DPS
	{
		get
		{
			return this.setBools[19];
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x0600007E RID: 126 RVA: 0x0000419F File Offset: 0x0000239F
	public bool Option_EnemyInfo
	{
		get
		{
			return this.setBools[20];
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600007F RID: 127 RVA: 0x000041AA File Offset: 0x000023AA
	public bool Option_SkipTutorial
	{
		get
		{
			return this.setBools[21];
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06000080 RID: 128 RVA: 0x000041B5 File Offset: 0x000023B5
	public double SetFloat_SoundEffectsVolume
	{
		get
		{
			return this.setFloats[0];
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000081 RID: 129 RVA: 0x000041BF File Offset: 0x000023BF
	public double SetFloat_BGMVolume
	{
		get
		{
			if (this.Option_SoundBGMOn)
			{
				return this.setFloats[1];
			}
			return 0.0;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000082 RID: 130 RVA: 0x000041DB File Offset: 0x000023DB
	public double SetFloat_BulletOptimizeVolume
	{
		get
		{
			return this.setFloats[3];
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x000041E8 File Offset: 0x000023E8
	public void DefaultSetting()
	{
		this.language = EnumLanguage.CHINESE_SIM;
		this.resolutionIndex = 6;
		this.setBools = new bool[22];
		for (int i = 0; i < this.setBools.Length; i++)
		{
			this.setBools[i] = true;
		}
		this.setBools[9] = true;
		this.setBools[11] = false;
		this.setBools[12] = true;
		this.setBools[13] = false;
		this.setBools[14] = false;
		this.setBools[17] = false;
		this.setBools[18] = false;
		this.setBools[19] = true;
		this.setBools[20] = false;
		this.setBools[21] = false;
		this.setInts = new int[]
		{
			2,
			2,
			2,
			2
		};
		this.setFloats = new double[4];
		for (int j = 0; j < this.setFloats.Length; j++)
		{
			if (j == 0 || j == 1 || j == 3)
			{
				this.setFloats[j] = 0.5;
			}
			else
			{
				this.setFloats[j] = 1.0;
			}
		}
		this.language = EnumLanguage.CHINESE_SIM;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x000042FC File Offset: 0x000024FC
	public void ApplySettingSuddenly()
	{
		Debug.Log("马上应用设置选项");
		int width = this.GetCurrentResolution().Width;
		int height = this.GetCurrentResolution().Height;
		Screen.SetResolution(width, height, this.IfFullScrren);
		this.ApplyVSync();
		this.ApplyLanguage();
		if (TempData.inst.currentSceneType == EnumSceneType.BATTLE && BattleMapCanvas.inst != null)
		{
			BattleMapCanvas.inst.ApplySetting();
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00004368 File Offset: 0x00002568
	public void ApplyLanguage()
	{
		EnumSceneType currentSceneType = TempData.inst.currentSceneType;
		if (currentSceneType != EnumSceneType.MAINMENU)
		{
			if (currentSceneType != EnumSceneType.BATTLE)
			{
				return;
			}
			BattleMapCanvas.inst.UpdateLanguageText();
			BattleMapCanvas.inst.panel_Pause.UpdateLanguage();
		}
		else if (MainCanvas.inst != null)
		{
			MainCanvas.inst.UpdateLanguage();
			return;
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x000043B9 File Offset: 0x000025B9
	public global::Resolution GetCurrentResolution()
	{
		this.resolutionIndex = Mathf.Clamp(this.resolutionIndex, 0, UI_Setting.Inst.resolutions.Length - 1);
		return UI_Setting.Inst.resolutions[this.resolutionIndex];
	}

	// Token: 0x06000087 RID: 135 RVA: 0x000043EC File Offset: 0x000025EC
	public static Setting NewOrLoad()
	{
		if (SaveFile.IsNewSaveSlot())
		{
			Debug.Log("Setting_Default");
			Setting setting = new Setting();
			setting.DefaultSetting();
			setting.ApplySettingSuddenly();
			Debug.Log("Setting_新建");
			return setting;
		}
		SaveFile saveFile = GameData.SaveFile;
		if (saveFile.version <= 805)
		{
			Setting setting2 = new Setting();
			setting2.DefaultSetting();
			setting2.resolutionIndex = saveFile.resolutionIndex;
			for (int i = 0; i < Mathf.Min(setting2.setBools.Length, saveFile.setBools.Length); i++)
			{
				setting2.setBools[i] = saveFile.setBools[i];
			}
			for (int j = 0; j < Mathf.Min(setting2.setFloats.Length, saveFile.setFloats.Length); j++)
			{
				setting2.setFloats[j] = saveFile.setFloats[j];
			}
			setting2.language = saveFile.set_Language;
			Debug.Log("Setting_读取自旧存档");
			return setting2;
		}
		Setting result = SaveFile_Settings.ReadByJson_Settings();
		Debug.Log("Setting_读取自设置文件");
		return result;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x000044DB File Offset: 0x000026DB
	public void ApplyVSync()
	{
		if (this.Option_VerticalSyn)
		{
			QualitySettings.vSyncCount = 1;
			return;
		}
		QualitySettings.vSyncCount = 0;
	}

	// Token: 0x040000B5 RID: 181
	public EnumLanguage language = EnumLanguage.CHINESE_SIM;

	// Token: 0x040000B6 RID: 182
	[Header("Settings")]
	public int resolutionIndex = 3;

	// Token: 0x040000B7 RID: 183
	public bool[] setBools = new bool[]
	{
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		false,
		false,
		false,
		false
	};

	// Token: 0x040000B8 RID: 184
	public double[] setFloats = new double[]
	{
		1.0,
		1.0,
		1.0,
		0.5
	};

	// Token: 0x040000B9 RID: 185
	public int[] setInts = new int[]
	{
		2,
		2,
		2,
		2
	};
}
