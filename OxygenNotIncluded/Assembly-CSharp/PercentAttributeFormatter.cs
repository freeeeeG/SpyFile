using System;
using Klei.AI;

// Token: 0x02000AA2 RID: 2722
public class PercentAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x0600531C RID: 21276 RVA: 0x001DCADA File Offset: 0x001DACDA
	public PercentAttributeFormatter() : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x0600531D RID: 21277 RVA: 0x001DCAE4 File Offset: 0x001DACE4
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), base.DeltaTimeSlice);
	}

	// Token: 0x0600531E RID: 21278 RVA: 0x001DCAF8 File Offset: 0x001DACF8
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, base.DeltaTimeSlice);
	}

	// Token: 0x0600531F RID: 21279 RVA: 0x001DCB0C File Offset: 0x001DAD0C
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return GameUtil.GetFormattedPercent(value * 100f, timeSlice);
	}
}
