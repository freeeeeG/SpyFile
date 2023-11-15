using System;

// Token: 0x02000861 RID: 2145
public class SearchStatus : BaseStatus
{
	// Token: 0x06002955 RID: 10581 RVA: 0x000C1B68 File Offset: 0x000BFF68
	public override string GetLocalisedProgressDescription()
	{
		eConnectionModeSwitchProgress progress = this.Progress;
		if (progress == eConnectionModeSwitchProgress.NotStarted)
		{
			return Localization.Get("Online.ConnectionMode.Progress.NotStarted", new LocToken[0]);
		}
		if (progress == eConnectionModeSwitchProgress.InProgress)
		{
			return Localization.Get("Online.ConnectionMode.Matchmaking.Search.InProgress", new LocToken[0]);
		}
		if (progress != eConnectionModeSwitchProgress.Complete)
		{
			return Localization.Get("Online.ConnectionMode.Progress.Unhandled", new LocToken[0]);
		}
		return Localization.Get("Online.ConnectionMode.Progress.Complete", new LocToken[0]);
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x000C1BD8 File Offset: 0x000BFFD8
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

	// Token: 0x06002957 RID: 10583 RVA: 0x000C1C48 File Offset: 0x000C0048
	public override IConnectionModeSwitchStatus Clone()
	{
		return new SearchStatus
		{
			Result = this.Result,
			Progress = this.Progress
		};
	}
}
