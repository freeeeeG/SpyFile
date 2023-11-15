using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000B5C RID: 2908
public class CrewRationsEntry : CrewListEntry
{
	// Token: 0x06005A08 RID: 23048 RVA: 0x0020F56E File Offset: 0x0020D76E
	public override void Populate(MinionIdentity _identity)
	{
		base.Populate(_identity);
		this.rationMonitor = _identity.GetSMI<RationMonitor.Instance>();
		this.Refresh();
	}

	// Token: 0x06005A09 RID: 23049 RVA: 0x0020F58C File Offset: 0x0020D78C
	public override void Refresh()
	{
		base.Refresh();
		this.rationsEatenToday.text = GameUtil.GetFormattedCalories(this.rationMonitor.GetRationsAteToday(), GameUtil.TimeSlice.None, true);
		if (this.identity == null)
		{
			return;
		}
		foreach (AmountInstance amountInstance in this.identity.GetAmounts())
		{
			float min = amountInstance.GetMin();
			float max = amountInstance.GetMax();
			float num = max - min;
			string str = Mathf.RoundToInt((num - (max - amountInstance.value)) / num * 100f).ToString();
			if (amountInstance.amount == Db.Get().Amounts.Stress)
			{
				this.currentStressText.text = amountInstance.GetValueString();
				this.currentStressText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
				this.stressTrendImage.SetValue(amountInstance);
			}
			else if (amountInstance.amount == Db.Get().Amounts.Calories)
			{
				this.currentCaloriesText.text = str + "%";
				this.currentCaloriesText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
			}
			else if (amountInstance.amount == Db.Get().Amounts.HitPoints)
			{
				this.currentHealthText.text = str + "%";
				this.currentHealthText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
			}
		}
	}

	// Token: 0x04003CFD RID: 15613
	public KButton incRationPerDayButton;

	// Token: 0x04003CFE RID: 15614
	public KButton decRationPerDayButton;

	// Token: 0x04003CFF RID: 15615
	public LocText rationPerDayText;

	// Token: 0x04003D00 RID: 15616
	public LocText rationsEatenToday;

	// Token: 0x04003D01 RID: 15617
	public LocText currentCaloriesText;

	// Token: 0x04003D02 RID: 15618
	public LocText currentStressText;

	// Token: 0x04003D03 RID: 15619
	public LocText currentHealthText;

	// Token: 0x04003D04 RID: 15620
	public ValueTrendImageToggle stressTrendImage;

	// Token: 0x04003D05 RID: 15621
	private RationMonitor.Instance rationMonitor;
}
