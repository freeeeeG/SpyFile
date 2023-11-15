using System;
using Klei.AI;

// Token: 0x02000A96 RID: 2710
public class CaloriesDisplayer : StandardAmountDisplayer
{
	// Token: 0x060052F1 RID: 21233 RVA: 0x001DC0D7 File Offset: 0x001DA2D7
	public CaloriesDisplayer() : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new CaloriesDisplayer.CaloriesAttributeFormatter();
	}

	// Token: 0x020019B2 RID: 6578
	public class CaloriesAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x06009514 RID: 38164 RVA: 0x00338E8C File Offset: 0x0033708C
		public CaloriesAttributeFormatter() : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle)
		{
		}

		// Token: 0x06009515 RID: 38165 RVA: 0x00338E96 File Offset: 0x00337096
		public override string GetFormattedModifier(AttributeModifier modifier)
		{
			if (modifier.IsMultiplier)
			{
				return GameUtil.GetFormattedPercent(-modifier.Value * 100f, GameUtil.TimeSlice.None);
			}
			return base.GetFormattedModifier(modifier);
		}
	}
}
