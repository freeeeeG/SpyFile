using System;

// Token: 0x02000C4A RID: 3146
public interface ICheckboxControl
{
	// Token: 0x170006E1 RID: 1761
	// (get) Token: 0x060063BC RID: 25532
	string CheckboxTitleKey { get; }

	// Token: 0x170006E2 RID: 1762
	// (get) Token: 0x060063BD RID: 25533
	string CheckboxLabel { get; }

	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x060063BE RID: 25534
	string CheckboxTooltip { get; }

	// Token: 0x060063BF RID: 25535
	bool GetCheckboxValue();

	// Token: 0x060063C0 RID: 25536
	void SetCheckboxValue(bool value);
}
