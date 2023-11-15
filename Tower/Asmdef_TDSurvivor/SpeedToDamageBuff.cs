using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/Buff/攻擊力提升但發射速度下降", order = 1)]
public class SpeedToDamageBuff : ABaseBuffSettingData
{
	// Token: 0x06000084 RID: 132 RVA: 0x00003890 File Offset: 0x00001A90
	protected override void ApplyEffect()
	{
		this.damageModifierStats = new TowerStats
		{
			StatType = eStatType.DAMAGE
		};
		this.shootRateModifierStats = new TowerStats
		{
			StatType = eStatType.SHOOT_RATE
		};
		StatModifier modifier = new StatModifier(eModifierType.MULTIPLY, this.damageMultiplier);
		StatModifier modifier2 = new StatModifier(eModifierType.MULTIPLY, this.shootRateMultiplier);
		this.damageModifierStats.AddModifier(modifier);
		this.shootRateModifierStats.AddModifier(modifier2);
		this.tower.SettingData.AddBuffMultiplier(this.damageModifierStats);
		this.tower.SettingData.AddBuffMultiplier(this.shootRateModifierStats);
		DebugManager.Log(eDebugKey.BUFF_SYSTEM, string.Format("套用 SpeedToDamageBuff: 攻擊力 x{0}, 發射速度 x{1}", this.damageMultiplier, this.shootRateMultiplier), null);
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00003949 File Offset: 0x00001B49
	protected override void RemoveEffect()
	{
		this.tower.SettingData.RemoveBuffMultiplier(this.damageModifierStats);
		this.tower.SettingData.RemoveBuffMultiplier(this.shootRateModifierStats);
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00003977 File Offset: 0x00001B77
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("BuffCardName", this.itemType.ToString(), Array.Empty<object>());
	}

	// Token: 0x06000087 RID: 135 RVA: 0x000039A0 File Offset: 0x00001BA0
	public override string GetLocStatsString()
	{
		int num = Mathf.RoundToInt(base.GetTowerStats(eStatType.DAMAGE).list_Modifiers[0].Value * 100f);
		int num2 = Mathf.Abs(Mathf.RoundToInt(base.GetTowerStats(eStatType.SHOOT_RATE).list_Modifiers[0].Value * 100f));
		return base.GetLocDurationString("") + LocalizationManager.Instance.GetString("BuffCardDescription", this.itemType.ToString(), new object[]
		{
			num,
			num2
		});
	}

	// Token: 0x0400005C RID: 92
	[SerializeField]
	private float damageMultiplier = 1.2f;

	// Token: 0x0400005D RID: 93
	[SerializeField]
	private float shootRateMultiplier = 0.8f;

	// Token: 0x0400005E RID: 94
	private TowerStats damageModifierStats;

	// Token: 0x0400005F RID: 95
	private TowerStats shootRateModifierStats;
}
