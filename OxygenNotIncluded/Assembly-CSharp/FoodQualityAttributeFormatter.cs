using System;
using Klei.AI;

// Token: 0x02000A9E RID: 2718
public class FoodQualityAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x0600530F RID: 21263 RVA: 0x001DC939 File Offset: 0x001DAB39
	public FoodQualityAttributeFormatter() : base(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x06005310 RID: 21264 RVA: 0x001DC943 File Offset: 0x001DAB43
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.None);
	}

	// Token: 0x06005311 RID: 21265 RVA: 0x001DC952 File Offset: 0x001DAB52
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return GameUtil.GetFormattedInt(modifier.Value, GameUtil.TimeSlice.None);
	}

	// Token: 0x06005312 RID: 21266 RVA: 0x001DC960 File Offset: 0x001DAB60
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return Util.StripTextFormatting(GameUtil.GetFormattedFoodQuality((int)value));
	}
}
