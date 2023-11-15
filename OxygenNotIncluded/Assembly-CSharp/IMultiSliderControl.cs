using System;

// Token: 0x02000C31 RID: 3121
public interface IMultiSliderControl
{
	// Token: 0x170006D5 RID: 1749
	// (get) Token: 0x060062C6 RID: 25286
	string SidescreenTitleKey { get; }

	// Token: 0x060062C7 RID: 25287
	bool SidescreenEnabled();

	// Token: 0x170006D6 RID: 1750
	// (get) Token: 0x060062C8 RID: 25288
	ISliderControl[] sliderControls { get; }
}
