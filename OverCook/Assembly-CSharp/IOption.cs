using System;

// Token: 0x02000A32 RID: 2610
public interface IOption
{
	// Token: 0x1700038D RID: 909
	// (get) Token: 0x0600339D RID: 13213
	OptionsData.Categories Category { get; }

	// Token: 0x0600339E RID: 13214
	void SetOption(int _value);

	// Token: 0x0600339F RID: 13215
	int GetOption();

	// Token: 0x060033A0 RID: 13216
	void Commit();

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x060033A1 RID: 13217
	string Label { get; }
}
