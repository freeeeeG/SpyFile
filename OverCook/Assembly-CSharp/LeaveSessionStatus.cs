using System;

// Token: 0x0200085E RID: 2142
public class LeaveSessionStatus : BaseStatus
{
	// Token: 0x06002947 RID: 10567 RVA: 0x000C1618 File Offset: 0x000BFA18
	public override string GetLocalisedProgressDescription()
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

	// Token: 0x06002948 RID: 10568 RVA: 0x000C1688 File Offset: 0x000BFA88
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

	// Token: 0x06002949 RID: 10569 RVA: 0x000C16F8 File Offset: 0x000BFAF8
	public override IConnectionModeSwitchStatus Clone()
	{
		return new LeaveSessionStatus
		{
			Result = this.Result,
			Progress = this.Progress
		};
	}
}
