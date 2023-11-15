using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/修改砲塔屬性", order = 1)]
public class SwichElementBuff : ABaseBuffSettingData
{
	// Token: 0x06000091 RID: 145 RVA: 0x00003BB0 File Offset: 0x00001DB0
	protected override void ApplyEffect()
	{
		this.originalDamageType = this.tower.SettingData.DamageType;
		this.tower.OverrideDamageType(this.damageType);
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, string.Format("套用修改屬性buff ({0} -> {1}\ud83e\uddca)", this.originalDamageType, this.damageType), null);
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00003C0C File Offset: 0x00001E0C
	protected override void RemoveEffect()
	{
		this.tower.OverrideDamageType(this.originalDamageType);
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00003C1F File Offset: 0x00001E1F
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00003C48 File Offset: 0x00001E48
	public override string GetLocStatsString()
	{
		int num = Mathf.RoundToInt(base.GetTowerStats(eStatType.DAMAGE).list_Modifiers[0].Value * 100f);
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), new object[]
		{
			num
		});
	}

	// Token: 0x04000065 RID: 101
	[SerializeField]
	private eDamageType damageType;

	// Token: 0x04000066 RID: 102
	private eDamageType originalDamageType;
}
