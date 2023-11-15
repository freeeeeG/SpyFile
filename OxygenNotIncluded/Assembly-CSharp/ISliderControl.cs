using System;

// Token: 0x02000C4C RID: 3148
public interface ISliderControl
{
	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x060063C8 RID: 25544
	string SliderTitleKey { get; }

	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x060063C9 RID: 25545
	string SliderUnits { get; }

	// Token: 0x060063CA RID: 25546
	int SliderDecimalPlaces(int index);

	// Token: 0x060063CB RID: 25547
	float GetSliderMin(int index);

	// Token: 0x060063CC RID: 25548
	float GetSliderMax(int index);

	// Token: 0x060063CD RID: 25549
	float GetSliderValue(int index);

	// Token: 0x060063CE RID: 25550
	void SetSliderValue(float percent, int index);

	// Token: 0x060063CF RID: 25551
	string GetSliderTooltipKey(int index);

	// Token: 0x060063D0 RID: 25552
	string GetSliderTooltip(int index);
}
