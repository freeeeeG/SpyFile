using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000A95 RID: 2709
public class AsPercentAmountDisplayer : IAmountDisplayer
{
	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x060052E5 RID: 21221 RVA: 0x001DBEBB File Offset: 0x001DA0BB
	public IAttributeFormatter Formatter
	{
		get
		{
			return this.formatter;
		}
	}

	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x060052E6 RID: 21222 RVA: 0x001DBEC3 File Offset: 0x001DA0C3
	// (set) Token: 0x060052E7 RID: 21223 RVA: 0x001DBED0 File Offset: 0x001DA0D0
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

	// Token: 0x060052E8 RID: 21224 RVA: 0x001DBEDE File Offset: 0x001DA0DE
	public AsPercentAmountDisplayer(GameUtil.TimeSlice deltaTimeSlice)
	{
		this.formatter = new StandardAttributeFormatter(GameUtil.UnitClass.Percent, deltaTimeSlice);
	}

	// Token: 0x060052E9 RID: 21225 RVA: 0x001DBEF3 File Offset: 0x001DA0F3
	public string GetValueString(Amount master, AmountInstance instance)
	{
		return this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance), GameUtil.TimeSlice.None);
	}

	// Token: 0x060052EA RID: 21226 RVA: 0x001DBF0E File Offset: 0x001DA10E
	public virtual string GetDescription(Amount master, AmountInstance instance)
	{
		return string.Format("{0}: {1}", master.Name, this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance), GameUtil.TimeSlice.None));
	}

	// Token: 0x060052EB RID: 21227 RVA: 0x001DBF39 File Offset: 0x001DA139
	public virtual string GetTooltipDescription(Amount master, AmountInstance instance)
	{
		return string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
	}

	// Token: 0x060052EC RID: 21228 RVA: 0x001DBF58 File Offset: 0x001DA158
	public virtual string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		text += "\n\n";
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += string.Format(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerCycle));
		}
		else
		{
			text += string.Format(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerSecond));
		}
		for (int num = 0; num != instance.deltaAttribute.Modifiers.Count; num++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[num];
			float modifierContribution = instance.deltaAttribute.GetModifierContribution(attributeModifier);
			text = text + "\n" + string.Format(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), this.formatter.GetFormattedValue(this.ToPercent(modifierContribution, instance), this.formatter.DeltaTimeSlice));
		}
		return text;
	}

	// Token: 0x060052ED RID: 21229 RVA: 0x001DC081 File Offset: 0x001DA281
	public string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.formatter.GetFormattedAttribute(instance);
	}

	// Token: 0x060052EE RID: 21230 RVA: 0x001DC08F File Offset: 0x001DA28F
	public string GetFormattedModifier(AttributeModifier modifier)
	{
		if (modifier.IsMultiplier)
		{
			return GameUtil.GetFormattedPercent(modifier.Value * 100f, GameUtil.TimeSlice.None);
		}
		return this.formatter.GetFormattedModifier(modifier);
	}

	// Token: 0x060052EF RID: 21231 RVA: 0x001DC0B8 File Offset: 0x001DA2B8
	public string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return this.formatter.GetFormattedValue(value, timeSlice);
	}

	// Token: 0x060052F0 RID: 21232 RVA: 0x001DC0C7 File Offset: 0x001DA2C7
	protected float ToPercent(float value, AmountInstance instance)
	{
		return 100f * value / instance.GetMax();
	}

	// Token: 0x0400374C RID: 14156
	protected StandardAttributeFormatter formatter;
}
