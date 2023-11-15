using System;

// Token: 0x020007F5 RID: 2037
public interface IClientCookingNotifed
{
	// Token: 0x0600271D RID: 10013
	void OnCookingStarted();

	// Token: 0x0600271E RID: 10014
	void OnCookingFinished();

	// Token: 0x0600271F RID: 10015
	void OnCookingPropChanged(float newProp);
}
