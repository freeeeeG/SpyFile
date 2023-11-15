using System;
using Team17.Online;

// Token: 0x02000856 RID: 2134
public class ConnectionModeStatus : BaseStatus
{
	// Token: 0x06002924 RID: 10532 RVA: 0x000C1210 File Offset: 0x000BF610
	public override string GetLocalisedProgressDescription()
	{
		eConnectionModeSwitchProgress progress = this.Progress;
		if (progress == eConnectionModeSwitchProgress.NotStarted)
		{
			return Localization.Get("Online.ConnectionMode.ConnectionMode.InProgress", new LocToken[0]);
		}
		if (progress == eConnectionModeSwitchProgress.InProgress)
		{
			return Localization.Get("Online.ConnectionMode.ConnectionMode.InProgress", new LocToken[0]);
		}
		if (progress != eConnectionModeSwitchProgress.Complete)
		{
			return Localization.Get("Online.ConnectionMode.Progress.Unhandled", new LocToken[0]);
		}
		return Localization.Get("Online.ConnectionMode.Progress.Complete", new LocToken[0]);
	}

	// Token: 0x06002925 RID: 10533 RVA: 0x000C1280 File Offset: 0x000BF680
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

	// Token: 0x06002926 RID: 10534 RVA: 0x000C12E6 File Offset: 0x000BF6E6
	public string GetSpecificErrorCodeDescription()
	{
		if (this.m_Result != null && this.m_Result.m_returnCode == OnlineMultiplayerConnectionModeConnectResult.eGenericFailure)
		{
			return Localization.Get("Online.ConnectionMode.ConnectionMode.Result.eGeneric", new LocToken[0]);
		}
		return Localization.Get("Online.ConnectionMode.Result.Unhandled", new LocToken[0]);
	}

	// Token: 0x06002927 RID: 10535 RVA: 0x000C1325 File Offset: 0x000BF725
	public override bool DisplayPlatformDialog()
	{
		return this.m_Result.DisplayPlatformSpecificError(false);
	}

	// Token: 0x06002928 RID: 10536 RVA: 0x000C1334 File Offset: 0x000BF734
	public override IConnectionModeSwitchStatus Clone()
	{
		return new ConnectionModeStatus
		{
			Result = this.Result,
			Progress = this.Progress,
			m_Result = this.m_Result
		};
	}

	// Token: 0x04002097 RID: 8343
	public OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeConnectResult> m_Result;
}
