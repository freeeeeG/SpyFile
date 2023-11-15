using System;

// Token: 0x0200085F RID: 2143
public class ModifySessionStatus : BaseStatus
{
	// Token: 0x0600294B RID: 10571 RVA: 0x000C172C File Offset: 0x000BFB2C
	public override string GetLocalisedProgressDescription()
	{
		eConnectionModeSwitchProgress progress = this.Progress;
		if (progress == eConnectionModeSwitchProgress.NotStarted)
		{
			return Localization.Get("Online.ConnectionMode.Progress.NotStarted", new LocToken[0]);
		}
		if (progress == eConnectionModeSwitchProgress.InProgress)
		{
			return Localization.Get("Online.ConnectionMode.ModifySession.InProgress", new LocToken[0]);
		}
		if (progress != eConnectionModeSwitchProgress.Complete)
		{
			return Localization.Get("Online.ConnectionMode.Progress.Unhandled", new LocToken[0]);
		}
		return Localization.Get("Online.ConnectionMode.Progress.Complete", new LocToken[0]);
	}

	// Token: 0x0600294C RID: 10572 RVA: 0x000C179C File Offset: 0x000BFB9C
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

	// Token: 0x0600294D RID: 10573 RVA: 0x000C180C File Offset: 0x000BFC0C
	public override IConnectionModeSwitchStatus Clone()
	{
		return new ModifySessionStatus
		{
			Result = this.Result,
			Progress = this.Progress
		};
	}
}
