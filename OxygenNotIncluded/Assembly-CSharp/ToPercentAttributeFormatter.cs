using System;
using Klei.AI;

// Token: 0x02000AA1 RID: 2721
public class ToPercentAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06005318 RID: 21272 RVA: 0x001DCA80 File Offset: 0x001DAC80
	public ToPercentAttributeFormatter(float max, GameUtil.TimeSlice deltaTimeSlice = GameUtil.TimeSlice.None) : base(GameUtil.UnitClass.Percent, deltaTimeSlice)
	{
		this.max = max;
	}

	// Token: 0x06005319 RID: 21273 RVA: 0x001DCA9C File Offset: 0x001DAC9C
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), base.DeltaTimeSlice);
	}

	// Token: 0x0600531A RID: 21274 RVA: 0x001DCAB0 File Offset: 0x001DACB0
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, base.DeltaTimeSlice);
	}

	// Token: 0x0600531B RID: 21275 RVA: 0x001DCAC4 File Offset: 0x001DACC4
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return GameUtil.GetFormattedPercent(value / this.max * 100f, timeSlice);
	}

	// Token: 0x0400374F RID: 14159
	public float max = 1f;
}
