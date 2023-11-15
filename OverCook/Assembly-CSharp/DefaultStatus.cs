using System;

// Token: 0x0200085C RID: 2140
public class DefaultStatus : BaseStatus
{
	// Token: 0x0600293D RID: 10557 RVA: 0x000C1504 File Offset: 0x000BF904
	public override string GetLocalisedProgressDescription()
	{
		eConnectionModeSwitchProgress progress = this.Progress;
		if (progress == eConnectionModeSwitchProgress.NotStarted)
		{
			return Localization.Get("Online.ConnectionMode.Progress.NotStarted", new LocToken[0]);
		}
		if (progress == eConnectionModeSwitchProgress.InProgress)
		{
			return Localization.Get("Online.ConnectionMode.Offline.InProgress", new LocToken[0]);
		}
		if (progress != eConnectionModeSwitchProgress.Complete)
		{
			return Localization.Get("Online.ConnectionMode.Progress.Unhandled", new LocToken[0]);
		}
		return Localization.Get("Online.ConnectionMode.Progress.Complete", new LocToken[0]);
	}

	// Token: 0x0600293E RID: 10558 RVA: 0x000C1574 File Offset: 0x000BF974
	public override string GetLocalisedResultDescription()
	{
		eConnectionModeSwitchResult result = this.Result;
		if (result == eConnectionModeSwitchResult.NotAvailableYet)
		{
			return Localization.Get("Online.ConnectionMode.Result.NotAvailable", new LocToken[0]);
		}
		if (result == eConnectionModeSwitchResult.Failure)
		{
			return Localization.Get("Online.ConnectionMode.Result.Failure", new LocToken[0]);
		}
		if (result != eConnectionModeSwitchResult.Success)
		{
			return Localization.Get("Online.ConnectionMode.Result.Unhandled", new LocToken[0]);
		}
		return Localization.Get("Online.ConnectionMode.Result.Success", new LocToken[0]);
	}

	// Token: 0x0600293F RID: 10559 RVA: 0x000C15E4 File Offset: 0x000BF9E4
	public override IConnectionModeSwitchStatus Clone()
	{
		return new DefaultStatus
		{
			Result = this.Result,
			Progress = this.Progress
		};
	}
}
