using System;

// Token: 0x02000A79 RID: 2681
public interface IMenuEventDelegate
{
	// Token: 0x14000033 RID: 51
	// (add) Token: 0x06003509 RID: 13577
	// (remove) Token: 0x0600350A RID: 13578
	event MenuChangedHandler MenuChangedEvent;

	// Token: 0x0600350B RID: 13579
	void ChildMenuChanged(IMenuEventDelegate sender = null, IMenuEventDelegate changedItem = null);
}
