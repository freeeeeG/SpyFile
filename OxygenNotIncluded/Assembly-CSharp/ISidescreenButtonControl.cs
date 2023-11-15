using System;

// Token: 0x02000BFE RID: 3070
public interface ISidescreenButtonControl
{
	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x0600612C RID: 24876
	string SidescreenButtonText { get; }

	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x0600612D RID: 24877
	string SidescreenButtonTooltip { get; }

	// Token: 0x0600612E RID: 24878
	void SetButtonTextOverride(ButtonMenuTextOverride textOverride);

	// Token: 0x0600612F RID: 24879
	bool SidescreenEnabled();

	// Token: 0x06006130 RID: 24880
	bool SidescreenButtonInteractable();

	// Token: 0x06006131 RID: 24881
	void OnSidescreenButtonPressed();

	// Token: 0x06006132 RID: 24882
	int HorizontalGroupID();

	// Token: 0x06006133 RID: 24883
	int ButtonSideScreenSortOrder();
}
