using System;
using System.Collections.Generic;

// Token: 0x02000C14 RID: 3092
public interface IDispenser
{
	// Token: 0x060061E9 RID: 25065
	List<Tag> DispensedItems();

	// Token: 0x060061EA RID: 25066
	Tag SelectedItem();

	// Token: 0x060061EB RID: 25067
	void SelectItem(Tag tag);

	// Token: 0x060061EC RID: 25068
	void OnOrderDispense();

	// Token: 0x060061ED RID: 25069
	void OnCancelDispense();

	// Token: 0x060061EE RID: 25070
	bool HasOpenChore();

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x060061EF RID: 25071
	// (remove) Token: 0x060061F0 RID: 25072
	event System.Action OnStopWorkEvent;
}
