using System;
using Klei.AI;

// Token: 0x02000A9D RID: 2717
public class RadsPerCycleAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x0600530C RID: 21260 RVA: 0x001DC910 File Offset: 0x001DAB10
	public RadsPerCycleAttributeFormatter() : base(GameUtil.UnitClass.Radiation, GameUtil.TimeSlice.PerCycle)
	{
	}

	// Token: 0x0600530D RID: 21261 RVA: 0x001DC91A File Offset: 0x001DAB1A
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x0600530E RID: 21262 RVA: 0x001DC929 File Offset: 0x001DAB29
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return base.GetFormattedValue(value / 600f, timeSlice);
	}
}
