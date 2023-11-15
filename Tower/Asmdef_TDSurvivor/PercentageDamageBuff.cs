using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/傷害_百分比增加", order = 1)]
public class PercentageDamageBuff : ABaseBuffSettingData
{
	// Token: 0x06000075 RID: 117 RVA: 0x00003450 File Offset: 0x00001650
	protected override void ApplyEffect()
	{
		float value = base.GetTowerStats(eStatType.DAMAGE).list_Modifiers[0].Value;
		int num = Mathf.CeilToInt(this.tower.SettingData.GetTowerStats(eStatType.DAMAGE).BaseValue);
		int num2 = Mathf.Max(1, (int)((float)num * value));
		this.buffModifierStats = new TowerStats
		{
			StatType = eStatType.DAMAGE
		};
		StatModifier modifier = new StatModifier(eModifierType.ADD, (float)num2);
		this.buffModifierStats.AddModifier(modifier);
		this.tower.SettingData.AddBuffMultiplier(this.buffModifierStats);
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, string.Format("套用傷害buff (+{0:0}%): {1} -> {2}", value * 100f, num, num + num2), null);
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00003509 File Offset: 0x00001709
	protected override void RemoveEffect()
	{
		this.tower.SettingData.RemoveBuffMultiplier(this.buffModifierStats);
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00003521 File Offset: 0x00001721
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00003548 File Offset: 0x00001748
	public override string GetLocStatsString()
	{
		int num = Mathf.RoundToInt(base.GetTowerStats(eStatType.DAMAGE).list_Modifiers[0].Value * 100f);
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), new object[]
		{
			num
		});
	}

	// Token: 0x04000059 RID: 89
	private TowerStats buffModifierStats;
}
