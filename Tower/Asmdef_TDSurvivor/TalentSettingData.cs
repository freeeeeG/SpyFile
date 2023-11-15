using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000034 RID: 52
[CreateAssetMenu(fileName = "TalentSettingData", menuName = "設定檔/TalentSettingData")]
public class TalentSettingData : ScriptableObject
{
	// Token: 0x06000100 RID: 256 RVA: 0x00005024 File Offset: 0x00003224
	public TalentSetting GetTalentSettingByType(eTalentType talentType)
	{
		foreach (TalentSetting talentSetting in this.list_talentSettings)
		{
			if (talentSetting.TalentType == talentType)
			{
				return talentSetting;
			}
		}
		Debug.LogError("Can't find talent setting by talent type: " + talentType.ToString());
		return null;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x0000509C File Offset: 0x0000329C
	public TalentSetting GetTalentSettingByIndex(int index)
	{
		if (index < 0 || index >= this.list_talentSettings.Count)
		{
			Debug.LogError("Can't find talent setting by index: " + index.ToString());
			return null;
		}
		return this.list_talentSettings[index];
	}

	// Token: 0x06000102 RID: 258 RVA: 0x000050D4 File Offset: 0x000032D4
	private void OnValidate()
	{
		foreach (TalentSetting talentSetting in this.list_talentSettings)
		{
			if (talentSetting.TalentType == eTalentType.NONE)
			{
				talentSetting.SetTalentType(this.list_talentSettings.IndexOf(talentSetting) + eTalentType.TIME_MANAGEMENT);
			}
			if (talentSetting.Icon == null)
			{
				talentSetting.SetIcon(this.defaultIcon);
			}
		}
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00005158 File Offset: 0x00003358
	private void LockAllTalentsInDemoVersion()
	{
		foreach (TalentSetting talentSetting in this.list_talentSettings)
		{
			talentSetting.SetLockInDemoVersion(true);
		}
	}

	// Token: 0x06000104 RID: 260 RVA: 0x000051AC File Offset: 0x000033AC
	private void UnlockAllTalentsInDemoVersion()
	{
		foreach (TalentSetting talentSetting in this.list_talentSettings)
		{
			talentSetting.SetLockInDemoVersion(false);
		}
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00005200 File Offset: 0x00003400
	private void SwapTalentByIndex()
	{
		TalentSetting value = this.list_talentSettings[this.switchItemIndex.x];
		this.list_talentSettings[this.switchItemIndex.x] = this.list_talentSettings[this.switchItemIndex.y];
		this.list_talentSettings[this.switchItemIndex.y] = value;
	}

	// Token: 0x040000B7 RID: 183
	[SerializeField]
	private Sprite defaultIcon;

	// Token: 0x040000B8 RID: 184
	[SerializeField]
	private List<TalentSetting> list_talentSettings;

	// Token: 0x040000B9 RID: 185
	[SerializeField]
	[Header("在這邊輸入要交換的兩格天賦的index")]
	private Vector2Int switchItemIndex;
}
