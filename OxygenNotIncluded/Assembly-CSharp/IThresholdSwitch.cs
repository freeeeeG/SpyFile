using System;

// Token: 0x0200069D RID: 1693
public interface IThresholdSwitch
{
	// Token: 0x1700030B RID: 779
	// (get) Token: 0x06002DAF RID: 11695
	// (set) Token: 0x06002DB0 RID: 11696
	float Threshold { get; set; }

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x06002DB1 RID: 11697
	// (set) Token: 0x06002DB2 RID: 11698
	bool ActivateAboveThreshold { get; set; }

	// Token: 0x1700030D RID: 781
	// (get) Token: 0x06002DB3 RID: 11699
	float CurrentValue { get; }

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x06002DB4 RID: 11700
	float RangeMin { get; }

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06002DB5 RID: 11701
	float RangeMax { get; }

	// Token: 0x06002DB6 RID: 11702
	float GetRangeMinInputField();

	// Token: 0x06002DB7 RID: 11703
	float GetRangeMaxInputField();

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06002DB8 RID: 11704
	LocString Title { get; }

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06002DB9 RID: 11705
	LocString ThresholdValueName { get; }

	// Token: 0x06002DBA RID: 11706
	LocString ThresholdValueUnits();

	// Token: 0x06002DBB RID: 11707
	string Format(float value, bool units);

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06002DBC RID: 11708
	string AboveToolTip { get; }

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06002DBD RID: 11709
	string BelowToolTip { get; }

	// Token: 0x06002DBE RID: 11710
	float ProcessedSliderValue(float input);

	// Token: 0x06002DBF RID: 11711
	float ProcessedInputValue(float input);

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x06002DC0 RID: 11712
	ThresholdScreenLayoutType LayoutType { get; }

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x06002DC1 RID: 11713
	int IncrementScale { get; }

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06002DC2 RID: 11714
	NonLinearSlider.Range[] GetRanges { get; }
}
