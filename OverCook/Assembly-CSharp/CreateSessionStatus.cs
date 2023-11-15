using System;
using Team17.Online;

// Token: 0x0200085B RID: 2139
public class CreateSessionStatus : BaseStatus
{
	// Token: 0x06002937 RID: 10551 RVA: 0x000C1380 File Offset: 0x000BF780
	public override string GetLocalisedProgressDescription()
	{
		eConnectionModeSwitchProgress progress = this.Progress;
		if (progress == eConnectionModeSwitchProgress.NotStarted)
		{
			return Localization.Get("Online.ConnectionMode.Progress.NotStarted", new LocToken[0]);
		}
		if (progress == eConnectionModeSwitchProgress.InProgress)
		{
			return Localization.Get("Online.ConnectionMode.CreateSession.InProgress", new LocToken[]
			{
				new LocToken("[NAME]", this.currentUser.m_userName)
			});
		}
		if (progress != eConnectionModeSwitchProgress.Complete)
		{
			return Localization.Get("Online.ConnectionMode.Progress.Unhandled", new LocToken[0]);
		}
		return Localization.Get("Online.ConnectionMode.Progress.Complete", new LocToken[0]);
	}

	// Token: 0x06002938 RID: 10552 RVA: 0x000C1414 File Offset: 0x000BF814
	public override string GetLocalisedResultDescription()
	{
		eConnectionModeSwitchResult result = this.Result;
		if (result == eConnectionModeSwitchResult.NotAvailableYet)
		{
			return Localization.Get("Online.ConnectionMode.Result.NotAvailable", new LocToken[0]);
		}
		if (result == eConnectionModeSwitchResult.Failure)
		{
			return this.GetSpecificErrorCodeDescription();
		}
		if (result != eConnectionModeSwitchResult.Success)
		{
			return Localization.Get("Online.ConnectionMode.Result.Unhandled", new LocToken[0]);
		}
		return Localization.Get("Online.ConnectionMode.Result.Success", new LocToken[0]);
	}

	// Token: 0x06002939 RID: 10553 RVA: 0x000C147A File Offset: 0x000BF87A
	public string GetSpecificErrorCodeDescription()
	{
		return Localization.Get("Online.ConnectionMode.CreateSession.Result." + this.sessionCreateResult.m_returnCode.ToString(), new LocToken[0]);
	}

	// Token: 0x0600293A RID: 10554 RVA: 0x000C14A7 File Offset: 0x000BF8A7
	public override bool DisplayPlatformDialog()
	{
		return this.sessionCreateResult.DisplayPlatformSpecificError(false);
	}

	// Token: 0x0600293B RID: 10555 RVA: 0x000C14B8 File Offset: 0x000BF8B8
	public override IConnectionModeSwitchStatus Clone()
	{
		return new CreateSessionStatus
		{
			Result = this.Result,
			Progress = this.Progress,
			currentUser = this.currentUser,
			sessionCreateResult = this.sessionCreateResult
		};
	}

	// Token: 0x040020A2 RID: 8354
	public OnlineMultiplayerLocalUserId currentUser;

	// Token: 0x040020A3 RID: 8355
	public OnlineMultiplayerReturnCode<OnlineMultiplayerSessionCreateResult> sessionCreateResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionCreateResult>();
}
