using System;

// Token: 0x02000C37 RID: 3127
public interface IPlayerControlledToggle
{
	// Token: 0x060062F4 RID: 25332
	void ToggledByPlayer();

	// Token: 0x060062F5 RID: 25333
	bool ToggledOn();

	// Token: 0x060062F6 RID: 25334
	KSelectable GetSelectable();

	// Token: 0x170006DE RID: 1758
	// (get) Token: 0x060062F7 RID: 25335
	string SideScreenTitleKey { get; }

	// Token: 0x170006DF RID: 1759
	// (get) Token: 0x060062F8 RID: 25336
	// (set) Token: 0x060062F9 RID: 25337
	bool ToggleRequested { get; set; }
}
