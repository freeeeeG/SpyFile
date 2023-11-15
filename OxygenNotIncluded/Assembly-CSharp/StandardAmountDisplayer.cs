using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000A94 RID: 2708
public class StandardAmountDisplayer : IAmountDisplayer
{
	// Token: 0x17000601 RID: 1537
	// (get) Token: 0x060052DB RID: 21211 RVA: 0x001DBC73 File Offset: 0x001D9E73
	public IAttributeFormatter Formatter
	{
		get
		{
			return this.formatter;
		}
	}

	// Token: 0x17000602 RID: 1538
	// (get) Token: 0x060052DC RID: 21212 RVA: 0x001DBC7B File Offset: 0x001D9E7B
	// (set) Token: 0x060052DD RID: 21213 RVA: 0x001DBC88 File Offset: 0x001D9E88
	public GameUtil.TimeSlice DeltaTimeSlice
	{
		get
		{
			return this.formatter.DeltaTimeSlice;
		}
		set
		{
			this.formatter.DeltaTimeSlice = value;
		}
	}

	// Token: 0x060052DE RID: 21214 RVA: 0x001DBC96 File Offset: 0x001D9E96
	public StandardAmountDisplayer(GameUtil.UnitClass unitClass, GameUtil.TimeSlice deltaTimeSlice, StandardAttributeFormatter formatter = null, GameUtil.IdentityDescriptorTense tense = GameUtil.IdentityDescriptorTense.Normal)
	{
		this.tense = tense;
		if (formatter != null)
		{
			this.formatter = formatter;
			return;
		}
		this.formatter = new StandardAttributeFormatter(unitClass, deltaTimeSlice);
	}

	// Token: 0x060052DF RID: 21215 RVA: 0x001DBCC0 File Offset: 0x001D9EC0
	public virtual string GetValueString(Amount master, AmountInstance instance)
	{
		if (!master.showMax)
		{
			return this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None);
		}
		return string.Format("{0} / {1}", this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None), this.formatter.GetFormattedValue(instance.GetMax(), GameUtil.TimeSlice.None));
	}

	// Token: 0x060052E0 RID: 21216 RVA: 0x001DBD16 File Offset: 0x001D9F16
	public virtual string GetDescription(Amount master, AmountInstance instance)
	{
		return string.Format("{0}: {1}", master.Name, this.GetValueString(master, instance));
	}

	// Token: 0x060052E1 RID: 21217 RVA: 0x001DBD30 File Offset: 0x001D9F30
	public virtual string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = "";
		if (master.description.IndexOf("{1}") > -1)
		{
			text += string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None), GameUtil.GetIdentityDescriptor(instance.gameObject, this.tense));
		}
		else
		{
			text += string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		}
		text += "\n\n";
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += string.Format(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
		}
		else if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerSecond)
		{
			text += string.Format(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond));
		}
		for (int num = 0; num != instance.deltaAttribute.Modifiers.Count; num++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[num];
			text = text + "\n" + string.Format(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), this.formatter.GetFormattedModifier(attributeModifier));
		}
		return text;
	}

	// Token: 0x060052E2 RID: 21218 RVA: 0x001DBE90 File Offset: 0x001DA090
	public string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.formatter.GetFormattedAttribute(instance);
	}

	// Token: 0x060052E3 RID: 21219 RVA: 0x001DBE9E File Offset: 0x001DA09E
	public string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.formatter.GetFormattedModifier(modifier);
	}

	// Token: 0x060052E4 RID: 21220 RVA: 0x001DBEAC File Offset: 0x001DA0AC
	public string GetFormattedValue(float value, GameUtil.TimeSlice time_slice)
	{
		return this.formatter.GetFormattedValue(value, time_slice);
	}

	// Token: 0x0400374A RID: 14154
	protected StandardAttributeFormatter formatter;

	// Token: 0x0400374B RID: 14155
	public GameUtil.IdentityDescriptorTense tense;
}
