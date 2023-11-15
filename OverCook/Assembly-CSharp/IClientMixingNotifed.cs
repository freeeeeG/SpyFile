using System;

// Token: 0x020007F6 RID: 2038
public interface IClientMixingNotifed
{
	// Token: 0x06002720 RID: 10016
	void OnMixingStarted();

	// Token: 0x06002721 RID: 10017
	void OnMixingFinished();

	// Token: 0x06002722 RID: 10018
	void OnMixingPropChanged(float newProp);
}
