using System;

// Token: 0x02000859 RID: 2137
public abstract class BaseStatus : IConnectionModeSwitchStatus
{
	// Token: 0x0600292A RID: 10538 RVA: 0x000C0DF3 File Offset: 0x000BF1F3
	public eConnectionModeSwitchProgress GetProgress()
	{
		return this.Progress;
	}

	// Token: 0x0600292B RID: 10539 RVA: 0x000C0DFC File Offset: 0x000BF1FC
	public virtual string GetLocalisedProgressDescription()
	{
		eConnectionModeSwitchProgress progress = this.Progress;
		if (progress == eConnectionModeSwitchProgress.NotStarted)
		{
			return Localization.Get("Online.ConnectionMode.Progress.NotStarted", new LocToken[0]);
		}
		if (progress == eConnectionModeSwitchProgress.InProgress)
		{
			return Localization.Get("Online.ConnectionMode.LeaveSession.InProgress", new LocToken[0]);
		}
		if (progress != eConnectionModeSwitchProgress.Complete)
		{
			return Localization.Get("Online.ConnectionMode.Progress.Unhandled", new LocToken[0]);
		}
		return Localization.Get("Online.ConnectionMode.Progress.Complete", new LocToken[0]);
	}

	// Token: 0x0600292C RID: 10540 RVA: 0x000C0E6C File Offset: 0x000BF26C
	public eConnectionModeSwitchResult GetResult()
	{
		return this.Result;
	}

	// Token: 0x0600292D RID: 10541 RVA: 0x000C0E74 File Offset: 0x000BF274
	public virtual bool DisplayPlatformDialog()
	{
		return false;
	}

	// Token: 0x0600292E RID: 10542
	public abstract IConnectionModeSwitchStatus Clone();

	// Token: 0x0600292F RID: 10543
	public abstract string GetLocalisedResultDescription();

	// Token: 0x040020A0 RID: 8352
	public eConnectionModeSwitchResult Result;

	// Token: 0x040020A1 RID: 8353
	public eConnectionModeSwitchProgress Progress;
}
