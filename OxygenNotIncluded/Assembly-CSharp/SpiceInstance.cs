using System;
using Klei.AI;

// Token: 0x0200034D RID: 845
[Serializable]
public struct SpiceInstance
{
	// Token: 0x1700004B RID: 75
	// (get) Token: 0x0600113E RID: 4414 RVA: 0x0005CF52 File Offset: 0x0005B152
	public AttributeModifier CalorieModifier
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].Spice.CalorieModifier;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600113F RID: 4415 RVA: 0x0005CF6E File Offset: 0x0005B16E
	public AttributeModifier FoodModifier
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].Spice.FoodModifier;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06001140 RID: 4416 RVA: 0x0005CF8A File Offset: 0x0005B18A
	public Effect StatBonus
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].StatBonus;
		}
	}

	// Token: 0x0400096A RID: 2410
	public Tag Id;

	// Token: 0x0400096B RID: 2411
	public float TotalKG;
}
