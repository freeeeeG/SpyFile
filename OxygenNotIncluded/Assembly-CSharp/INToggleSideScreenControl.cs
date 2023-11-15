using System;
using System.Collections.Generic;

// Token: 0x02000C33 RID: 3123
public interface INToggleSideScreenControl
{
	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x060062CD RID: 25293
	string SidescreenTitleKey { get; }

	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x060062CE RID: 25294
	List<LocString> Options { get; }

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x060062CF RID: 25295
	List<LocString> Tooltips { get; }

	// Token: 0x170006DA RID: 1754
	// (get) Token: 0x060062D0 RID: 25296
	string Description { get; }

	// Token: 0x170006DB RID: 1755
	// (get) Token: 0x060062D1 RID: 25297
	int SelectedOption { get; }

	// Token: 0x170006DC RID: 1756
	// (get) Token: 0x060062D2 RID: 25298
	int QueuedOption { get; }

	// Token: 0x060062D3 RID: 25299
	void QueueSelectedOption(int option);
}
