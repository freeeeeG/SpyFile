using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/射速_百分比增加", order = 1)]
public class PercentageShootSpeedBuff : ABaseBuffSettingData
{
	// Token: 0x0600007F RID: 127 RVA: 0x00003724 File Offset: 0x00001924
	protected override void ApplyEffect()
	{
		float value = base.GetTowerStats(eStatType.SHOOT_RATE).list_Modifiers[0].Value;
		float baseValue = this.tower.SettingData.GetTowerStats(eStatType.SHOOT_RATE).BaseValue;
		float num = baseValue * value;
		this.buffModifierStats = new TowerStats
		{
			StatType = eStatType.SHOOT_RATE
		};
		StatModifier modifier = new StatModifier(eModifierType.ADD, num);
		this.buffModifierStats.AddModifier(modifier);
		this.tower.SettingData.AddBuffMultiplier(this.buffModifierStats);
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, string.Format("套用射速buff (+{0:0}%): {1} -> {2}", value * 100f, baseValue, baseValue + num), null);
	}

	// Token: 0x06000080 RID: 128 RVA: 0x000037CF File Offset: 0x000019CF
	protected override void RemoveEffect()
	{
		this.tower.SettingData.RemoveBuffMultiplier(this.buffModifierStats);
	}

	// Token: 0x06000081 RID: 129 RVA: 0x000037E7 File Offset: 0x000019E7
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00003810 File Offset: 0x00001A10
	public override string GetLocStatsString()
	{
		int num = Mathf.Abs(Mathf.RoundToInt(base.GetTowerStats(eStatType.SHOOT_RATE).list_Modifiers[0].Value * 100f));
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), new object[]
		{
			num
		});
	}

	// Token: 0x0400005B RID: 91
	private TowerStats buffModifierStats;
}
