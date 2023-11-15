using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000A9C RID: 2716
public class StandardAttributeFormatter : IAttributeFormatter
{
	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x06005303 RID: 21251 RVA: 0x001DC5DC File Offset: 0x001DA7DC
	// (set) Token: 0x06005304 RID: 21252 RVA: 0x001DC5E4 File Offset: 0x001DA7E4
	public GameUtil.TimeSlice DeltaTimeSlice { get; set; }

	// Token: 0x06005305 RID: 21253 RVA: 0x001DC5ED File Offset: 0x001DA7ED
	public StandardAttributeFormatter(GameUtil.UnitClass unitClass, GameUtil.TimeSlice deltaTimeSlice)
	{
		this.unitClass = unitClass;
		this.DeltaTimeSlice = deltaTimeSlice;
	}

	// Token: 0x06005306 RID: 21254 RVA: 0x001DC603 File Offset: 0x001DA803
	public virtual string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.None);
	}

	// Token: 0x06005307 RID: 21255 RVA: 0x001DC612 File Offset: 0x001DA812
	public virtual string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, this.DeltaTimeSlice);
	}

	// Token: 0x06005308 RID: 21256 RVA: 0x001DC628 File Offset: 0x001DA828
	public virtual string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		switch (this.unitClass)
		{
		case GameUtil.UnitClass.SimpleInteger:
			return GameUtil.GetFormattedInt(value, timeSlice);
		case GameUtil.UnitClass.Temperature:
			return GameUtil.GetFormattedTemperature(value, timeSlice, (timeSlice == GameUtil.TimeSlice.None) ? GameUtil.TemperatureInterpretation.Absolute : GameUtil.TemperatureInterpretation.Relative, true, false);
		case GameUtil.UnitClass.Mass:
			return GameUtil.GetFormattedMass(value, timeSlice, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		case GameUtil.UnitClass.Calories:
			return GameUtil.GetFormattedCalories(value, timeSlice, true);
		case GameUtil.UnitClass.Percent:
			return GameUtil.GetFormattedPercent(value, timeSlice);
		case GameUtil.UnitClass.Distance:
			return GameUtil.GetFormattedDistance(value);
		case GameUtil.UnitClass.Disease:
			return GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(value), GameUtil.TimeSlice.None);
		case GameUtil.UnitClass.Radiation:
			return GameUtil.GetFormattedRads(value, timeSlice);
		case GameUtil.UnitClass.Energy:
			return GameUtil.GetFormattedJoules(value, "F1", timeSlice);
		case GameUtil.UnitClass.Power:
			return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Automatic, true);
		case GameUtil.UnitClass.Lux:
			return GameUtil.GetFormattedLux(Mathf.FloorToInt(value));
		case GameUtil.UnitClass.Time:
			return GameUtil.GetFormattedCycles(value, "F1", false);
		case GameUtil.UnitClass.Seconds:
			return GameUtil.GetFormattedTime(value, "F0");
		case GameUtil.UnitClass.Cycles:
			return GameUtil.GetFormattedCycles(value * 600f, "F1", false);
		}
		return GameUtil.GetFormattedSimple(value, timeSlice, null);
	}

	// Token: 0x06005309 RID: 21257 RVA: 0x001DC72E File Offset: 0x001DA92E
	public virtual string GetTooltipDescription(Klei.AI.Attribute master)
	{
		return master.Description;
	}

	// Token: 0x0600530A RID: 21258 RVA: 0x001DC738 File Offset: 0x001DA938
	public virtual string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance)
	{
		List<AttributeModifier> list = new List<AttributeModifier>();
		for (int i = 0; i < instance.Modifiers.Count; i++)
		{
			list.Add(instance.Modifiers[i]);
		}
		return this.GetTooltip(master, list, instance.GetComponent<AttributeConverters>());
	}

	// Token: 0x0600530B RID: 21259 RVA: 0x001DC784 File Offset: 0x001DA984
	public string GetTooltip(Klei.AI.Attribute master, List<AttributeModifier> modifiers, AttributeConverters converters)
	{
		string text = this.GetTooltipDescription(master);
		text += string.Format(DUPLICANTS.ATTRIBUTES.TOTAL_VALUE, this.GetFormattedValue(AttributeInstance.GetTotalDisplayValue(master, modifiers), GameUtil.TimeSlice.None), master.Name);
		if (master.BaseValue != 0f)
		{
			text += string.Format(DUPLICANTS.ATTRIBUTES.BASE_VALUE, master.BaseValue);
		}
		List<AttributeModifier> list = new List<AttributeModifier>(modifiers);
		list.Sort((AttributeModifier p1, AttributeModifier p2) => p2.Value.CompareTo(p1.Value));
		for (int num = 0; num != list.Count; num++)
		{
			AttributeModifier attributeModifier = list[num];
			string formattedString = attributeModifier.GetFormattedString();
			if (formattedString != null)
			{
				text += string.Format(DUPLICANTS.ATTRIBUTES.MODIFIER_ENTRY, attributeModifier.GetDescription(), formattedString);
			}
		}
		string text2 = "";
		if (converters != null && master.converters.Count > 0)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in converters.converters)
			{
				if (attributeConverterInstance.converter.attribute == master)
				{
					string text3 = attributeConverterInstance.DescriptionFromAttribute(attributeConverterInstance.Evaluate(), attributeConverterInstance.gameObject);
					if (text3 != null)
					{
						text2 = text2 + "\n" + text3;
					}
				}
			}
		}
		if (text2.Length > 0)
		{
			text = text + "\n" + text2;
		}
		return text;
	}

	// Token: 0x0400374D RID: 14157
	public GameUtil.UnitClass unitClass;
}
