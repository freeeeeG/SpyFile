using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/攻擊距離_百分比增加", order = 1)]
public class PercentageRangeBuff : ABaseBuffSettingData
{
	// Token: 0x0600007A RID: 122 RVA: 0x000035C0 File Offset: 0x000017C0
	protected override void ApplyEffect()
	{
		float value = base.GetTowerStats(eStatType.ATTACK_RANGE).list_Modifiers[0].Value;
		float baseValue = this.tower.SettingData.GetTowerStats(eStatType.ATTACK_RANGE).BaseValue;
		float num = baseValue * value;
		this.buffModifierStats = new TowerStats
		{
			StatType = eStatType.ATTACK_RANGE
		};
		StatModifier modifier = new StatModifier(eModifierType.ADD, num);
		this.buffModifierStats.AddModifier(modifier);
		this.tower.SettingData.AddBuffMultiplier(this.buffModifierStats);
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, string.Format("套用攻擊距離buff (+{0:0}%): {1} -> {2}", value * 100f, baseValue, baseValue + num), null);
	}

	// Token: 0x0600007B RID: 123 RVA: 0x0000366B File Offset: 0x0000186B
	protected override void RemoveEffect()
	{
		this.tower.SettingData.RemoveBuffMultiplier(this.buffModifierStats);
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00003683 File Offset: 0x00001883
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x0600007D RID: 125 RVA: 0x000036AC File Offset: 0x000018AC
	public override string GetLocStatsString()
	{
		int num = Mathf.RoundToInt(base.GetTowerStats(eStatType.ATTACK_RANGE).list_Modifiers[0].Value * 100f);
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), new object[]
		{
			num
		});
	}

	// Token: 0x0400005A RID: 90
	private TowerStats buffModifierStats;
}
