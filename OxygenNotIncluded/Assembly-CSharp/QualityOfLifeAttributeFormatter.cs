using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000A9F RID: 2719
public class QualityOfLifeAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x06005313 RID: 21267 RVA: 0x001DC96E File Offset: 0x001DAB6E
	public QualityOfLifeAttributeFormatter() : base(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x06005314 RID: 21268 RVA: 0x001DC978 File Offset: 0x001DAB78
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(instance.gameObject);
		return string.Format(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.DESC_FORMAT, this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.None), this.GetFormattedValue(attributeInstance.GetTotalDisplayValue(), GameUtil.TimeSlice.None));
	}

	// Token: 0x06005315 RID: 21269 RVA: 0x001DC9CC File Offset: 0x001DABCC
	public override string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance)
	{
		string text = base.GetTooltip(master, instance);
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(instance.gameObject);
		text = text + "\n\n" + string.Format(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION, this.GetFormattedValue(attributeInstance.GetTotalDisplayValue(), GameUtil.TimeSlice.None));
		if (instance.GetTotalDisplayValue() - attributeInstance.GetTotalDisplayValue() >= 0f)
		{
			text = text + "\n\n" + DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION_OVER;
		}
		else
		{
			text = text + "\n\n" + DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.TOOLTIP_EXPECTATION_UNDER;
		}
		return text;
	}
}
