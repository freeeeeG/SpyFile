using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000A97 RID: 2711
public class RadiationBalanceDisplayer : StandardAmountDisplayer
{
	// Token: 0x060052F2 RID: 21234 RVA: 0x001DC0EE File Offset: 0x001DA2EE
	public RadiationBalanceDisplayer() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new RadiationBalanceDisplayer.RadiationAttributeFormatter();
	}

	// Token: 0x060052F3 RID: 21235 RVA: 0x001DC105 File Offset: 0x001DA305
	public override string GetValueString(Amount master, AmountInstance instance)
	{
		return base.GetValueString(master, instance) + UI.UNITSUFFIXES.RADIATION.RADS;
	}

	// Token: 0x060052F4 RID: 21236 RVA: 0x001DC120 File Offset: 0x001DA320
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = "";
		if (instance.gameObject.GetSMI<RadiationMonitor.Instance>() != null)
		{
			int num = Grid.PosToCell(instance.gameObject);
			if (Grid.IsValidCell(num))
			{
				text += DUPLICANTS.STATS.RADIATIONBALANCE.TOOLTIP_CURRENT_BALANCE;
			}
			text += "\n\n";
			float num2 = Mathf.Clamp01(1f - Db.Get().Attributes.RadiationResistance.Lookup(instance.gameObject).GetTotalValue());
			text += string.Format(DUPLICANTS.STATS.RADIATIONBALANCE.CURRENT_EXPOSURE, Mathf.RoundToInt(Grid.Radiation[num] * num2));
			text += "\n";
			text += string.Format(DUPLICANTS.STATS.RADIATIONBALANCE.CURRENT_REJUVENATION, Mathf.RoundToInt(Db.Get().Attributes.RadiationRecovery.Lookup(instance.gameObject).GetTotalValue() * 600f));
		}
		return text;
	}

	// Token: 0x020019B3 RID: 6579
	public class RadiationAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x06009516 RID: 38166 RVA: 0x00338EBB File Offset: 0x003370BB
		public RadiationAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
		{
		}
	}
}
