using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/砲塔超載Buff", order = 2)]
public class TowerOverchargeBuff : ABaseBuffSettingData
{
	// Token: 0x0600009E RID: 158 RVA: 0x00003E60 File Offset: 0x00002060
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
		this.tower.ToggleOverchargeAnim(true);
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, string.Format("套用射速buff (+{0:0}%): {1} -> {2}", value * 100f, baseValue, baseValue + num), null);
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00003F17 File Offset: 0x00002117
	protected override void RemoveEffect()
	{
		this.tower.ToggleOverchargeAnim(false);
		this.tower.SettingData.RemoveBuffMultiplier(this.buffModifierStats);
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00003F3B File Offset: 0x0000213B
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00003F64 File Offset: 0x00002164
	public override string GetLocStatsString()
	{
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), new object[]
		{
			300
		});
	}

	// Token: 0x0400006D RID: 109
	private TowerStats buffModifierStats;
}
