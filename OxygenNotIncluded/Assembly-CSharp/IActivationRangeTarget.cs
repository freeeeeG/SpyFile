using System;

// Token: 0x02000BF4 RID: 3060
public interface IActivationRangeTarget
{
	// Token: 0x170006AE RID: 1710
	// (get) Token: 0x060060BC RID: 24764
	// (set) Token: 0x060060BD RID: 24765
	float ActivateValue { get; set; }

	// Token: 0x170006AF RID: 1711
	// (get) Token: 0x060060BE RID: 24766
	// (set) Token: 0x060060BF RID: 24767
	float DeactivateValue { get; set; }

	// Token: 0x170006B0 RID: 1712
	// (get) Token: 0x060060C0 RID: 24768
	float MinValue { get; }

	// Token: 0x170006B1 RID: 1713
	// (get) Token: 0x060060C1 RID: 24769
	float MaxValue { get; }

	// Token: 0x170006B2 RID: 1714
	// (get) Token: 0x060060C2 RID: 24770
	bool UseWholeNumbers { get; }

	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x060060C3 RID: 24771
	string ActivationRangeTitleText { get; }

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x060060C4 RID: 24772
	string ActivateSliderLabelText { get; }

	// Token: 0x170006B5 RID: 1717
	// (get) Token: 0x060060C5 RID: 24773
	string DeactivateSliderLabelText { get; }

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x060060C6 RID: 24774
	string ActivateTooltip { get; }

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x060060C7 RID: 24775
	string DeactivateTooltip { get; }
}
