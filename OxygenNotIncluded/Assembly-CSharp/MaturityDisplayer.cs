using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000A99 RID: 2713
public class MaturityDisplayer : AsPercentAmountDisplayer
{
	// Token: 0x060052F7 RID: 21239 RVA: 0x001DC309 File Offset: 0x001DA509
	public MaturityDisplayer() : base(GameUtil.TimeSlice.PerCycle)
	{
		this.formatter = new MaturityDisplayer.MaturityAttributeFormatter();
	}

	// Token: 0x060052F8 RID: 21240 RVA: 0x001DC320 File Offset: 0x001DA520
	public override string GetTooltipDescription(Amount master, AmountInstance instance)
	{
		string text = base.GetTooltipDescription(master, instance);
		Growing component = instance.gameObject.GetComponent<Growing>();
		if (component.IsGrowing())
		{
			float seconds = (instance.GetMax() - instance.value) / instance.GetDelta();
			if (component != null && component.IsGrowing())
			{
				text += string.Format(CREATURES.STATS.MATURITY.TOOLTIP_GROWING_CROP, GameUtil.GetFormattedCycles(seconds, "F1", false), GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest(), "F1", false));
			}
			else
			{
				text += string.Format(CREATURES.STATS.MATURITY.TOOLTIP_GROWING, GameUtil.GetFormattedCycles(seconds, "F1", false));
			}
		}
		else if (component.ReachedNextHarvest())
		{
			text += CREATURES.STATS.MATURITY.TOOLTIP_GROWN;
		}
		else
		{
			text += CREATURES.STATS.MATURITY.TOOLTIP_STALLED;
		}
		return text;
	}

	// Token: 0x060052F9 RID: 21241 RVA: 0x001DC3F8 File Offset: 0x001DA5F8
	public override string GetDescription(Amount master, AmountInstance instance)
	{
		Growing component = instance.gameObject.GetComponent<Growing>();
		if (component != null && component.IsGrowing())
		{
			return string.Format(CREATURES.STATS.MATURITY.AMOUNT_DESC_FMT, master.Name, this.formatter.GetFormattedValue(base.ToPercent(instance.value, instance), GameUtil.TimeSlice.None), GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest(), "F1", false));
		}
		return base.GetDescription(master, instance);
	}

	// Token: 0x020019B5 RID: 6581
	public class MaturityAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x06009518 RID: 38168 RVA: 0x00338ECF File Offset: 0x003370CF
		public MaturityAttributeFormatter() : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
		{
		}

		// Token: 0x06009519 RID: 38169 RVA: 0x00338EDC File Offset: 0x003370DC
		public override string GetFormattedModifier(AttributeModifier modifier)
		{
			float num = modifier.Value;
			GameUtil.TimeSlice timeSlice = base.DeltaTimeSlice;
			if (modifier.IsMultiplier)
			{
				num *= 100f;
				timeSlice = GameUtil.TimeSlice.None;
			}
			return this.GetFormattedValue(num, timeSlice);
		}
	}
}
