using System;
using Team17.Online;

// Token: 0x0200085D RID: 2141
public class JoinSessionStatus : BaseStatus
{
	// Token: 0x06002941 RID: 10561 RVA: 0x000C0E8C File Offset: 0x000BF28C
	public override string GetLocalisedProgressDescription()
	{
		eConnectionModeSwitchProgress progress = this.Progress;
		if (progress == eConnectionModeSwitchProgress.NotStarted)
		{
			return Localization.Get("Online.ConnectionMode.Progress.NotStarted", new LocToken[0]);
		}
		if (progress == eConnectionModeSwitchProgress.InProgress)
		{
			return Localization.Get("Online.ConnectionMode.JoinSession.InProgress", new LocToken[]
			{
				new LocToken("[NAME]", (this.currentUser == null) ? string.Empty : this.currentUser.m_userName)
			});
		}
		if (progress != eConnectionModeSwitchProgress.Complete)
		{
			return Localization.Get("Online.ConnectionMode.Progress.Unhandled", new LocToken[0]);
		}
		return Localization.Get("Online.ConnectionMode.Progress.Complete", new LocToken[0]);
	}

	// Token: 0x06002942 RID: 10562 RVA: 0x000C0F34 File Offset: 0x000BF334
	public override string GetLocalisedResultDescription()
	{
		eConnectionModeSwitchResult result = this.Result;
		if (result == eConnectionModeSwitchResult.NotAvailableYet)
		{
			return Localization.Get("Online.ConnectionMode.Result.NotAvailable", new LocToken[0]);
		}
		if (result != eConnectionModeSwitchResult.Failure && result != eConnectionModeSwitchResult.Success)
		{
			return Localization.Get("Online.ConnectionMode.Result.Unhandled", new LocToken[0]);
		}
		return this.GetSpecificDescription();
	}

	// Token: 0x06002943 RID: 10563 RVA: 0x000C0F89 File Offset: 0x000BF389
	public string GetSpecificDescription()
	{
		return Localization.Get("Online.ConnectionMode.JoinSession.Result." + this.sessionJoinResult.m_returnCode.ToString(), new LocToken[0]);
	}

	// Token: 0x06002944 RID: 10564 RVA: 0x000C0FB6 File Offset: 0x000BF3B6
	public override bool DisplayPlatformDialog()
	{
		return this.sessionJoinResult.DisplayPlatformSpecificError(false);
	}

	// Token: 0x06002945 RID: 10565 RVA: 0x000C0FC4 File Offset: 0x000BF3C4
	public override IConnectionModeSwitchStatus Clone()
	{
		return new JoinSessionStatus
		{
			Result = this.Result,
			Progress = this.Progress,
			currentUser = this.currentUser,
			sessionJoinResult = this.sessionJoinResult
		};
	}

	// Token: 0x040020A4 RID: 8356
	public OnlineMultiplayerLocalUserId currentUser;

	// Token: 0x040020A5 RID: 8357
	public OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult> sessionJoinResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult>();
}
