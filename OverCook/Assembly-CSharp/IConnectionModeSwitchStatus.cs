using System;

// Token: 0x0200085A RID: 2138
public interface IConnectionModeSwitchStatus
{
	// Token: 0x06002930 RID: 10544
	eConnectionModeSwitchProgress GetProgress();

	// Token: 0x06002931 RID: 10545
	string GetLocalisedProgressDescription();

	// Token: 0x06002932 RID: 10546
	eConnectionModeSwitchResult GetResult();

	// Token: 0x06002933 RID: 10547
	string GetLocalisedResultDescription();

	// Token: 0x06002934 RID: 10548
	bool DisplayPlatformDialog();

	// Token: 0x06002935 RID: 10549
	IConnectionModeSwitchStatus Clone();
}
