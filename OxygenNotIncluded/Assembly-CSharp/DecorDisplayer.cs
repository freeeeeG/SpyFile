using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000A98 RID: 2712
public class DecorDisplayer : StandardAmountDisplayer
{
	// Token: 0x060052F5 RID: 21237 RVA: 0x001DC21E File Offset: 0x001DA41E
	public DecorDisplayer() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new DecorDisplayer.DecorAttributeFormatter();
	}

	// Token: 0x060052F6 RID: 21238 RVA: 0x001DC238 File Offset: 0x001DA438
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = string.Format(LocText.ParseText(master.description), this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		int cell = Grid.PosToCell(instance.gameObject);
		if (Grid.IsValidCell(cell))
		{
			text += string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_CURRENT, GameUtil.GetDecorAtCell(cell));
		}
		text += "\n";
		DecorMonitor.Instance smi = instance.gameObject.GetSMI<DecorMonitor.Instance>();
		if (smi != null)
		{
			text += string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_TODAY, this.formatter.GetFormattedValue(smi.GetTodaysAverageDecor(), GameUtil.TimeSlice.None));
			text += string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_YESTERDAY, this.formatter.GetFormattedValue(smi.GetYesterdaysAverageDecor(), GameUtil.TimeSlice.None));
		}
		return text;
	}

	// Token: 0x020019B4 RID: 6580
	public class DecorAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x06009517 RID: 38167 RVA: 0x00338EC5 File Offset: 0x003370C5
		public DecorAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
		{
		}
	}
}
