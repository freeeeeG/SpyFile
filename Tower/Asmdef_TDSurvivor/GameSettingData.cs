using System;
using UnityEngine;

// Token: 0x02000083 RID: 131
public abstract class GameSettingData : ScriptableObject
{
	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060002BD RID: 701 RVA: 0x0000B637 File Offset: 0x00009837
	public int DefaultValue
	{
		get
		{
			return this.defaultValue;
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060002BE RID: 702 RVA: 0x0000B63F File Offset: 0x0000983F
	public int SettingValue
	{
		get
		{
			return this.settingValue;
		}
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0000B647 File Offset: 0x00009847
	public void ApplySetting(int value)
	{
		this.settingValue = value;
		PlayerPrefs.SetInt(this.type.ToString(), this.settingValue);
		PlayerPrefs.Save();
		this.ApplySettingToGame();
	}

	// Token: 0x060002C0 RID: 704
	protected abstract void ApplySettingToGame();

	// Token: 0x060002C1 RID: 705 RVA: 0x0000B677 File Offset: 0x00009877
	public int LoadSetting()
	{
		this.settingValue = PlayerPrefs.GetInt(this.type.ToString(), this.defaultValue);
		this.ApplySettingToGame();
		return this.settingValue;
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x0000B6A7 File Offset: 0x000098A7
	public void ResetToDefault()
	{
		this.settingValue = this.defaultValue;
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x0000B6B5 File Offset: 0x000098B5
	public string GetLocString()
	{
		return this.type.ToString();
	}

	// Token: 0x04000335 RID: 821
	[SerializeField]
	protected int defaultValue;

	// Token: 0x04000336 RID: 822
	[SerializeField]
	protected int settingValue;

	// Token: 0x04000337 RID: 823
	[SerializeField]
	protected eSettingItemType type;
}
