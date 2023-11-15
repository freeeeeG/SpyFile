using System;
using Team17.Online;

// Token: 0x02000860 RID: 2144
public class PrivilegeStatus : BaseStatus
{
	// Token: 0x0600294F RID: 10575 RVA: 0x000C184C File Offset: 0x000BFC4C
	public override string GetLocalisedProgressDescription()
	{
		eConnectionModeSwitchProgress progress = this.Progress;
		if (progress != eConnectionModeSwitchProgress.NotStarted)
		{
			if (progress != eConnectionModeSwitchProgress.InProgress)
			{
				if (progress != eConnectionModeSwitchProgress.Complete)
				{
					return Localization.Get("Online.ConnectionMode.Progress.Unhandled", new LocToken[0]);
				}
				return Localization.Get("Online.ConnectionMode.Progress.Complete", new LocToken[0]);
			}
			else
			{
				if (null != this.currentUser)
				{
					return Localization.Get("Online.ConnectionMode.Privilege.InProgress", new LocToken[]
					{
						new LocToken("[NAME]", this.currentUser.DisplayName)
					});
				}
				return "In progress with no user?!?!";
			}
		}
		else
		{
			if (null != this.currentUser)
			{
				return Localization.Get("Online.ConnectionMode.Privilege.InProgress", new LocToken[]
				{
					new LocToken("[NAME]", this.currentUser.DisplayName)
				});
			}
			return string.Empty;
		}
	}

	// Token: 0x06002950 RID: 10576 RVA: 0x000C192C File Offset: 0x000BFD2C
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

	// Token: 0x06002951 RID: 10577 RVA: 0x000C1994 File Offset: 0x000BFD94
	public string GetSpecificErrorCodeDescription()
	{
		switch (this.privilegeCheckResult.m_returnCode)
		{
		case OnlineMultiplayerPrivilegeCheckResult.eSuccess:
			return Localization.Get("Online.ConnectionMode.Privilege.Result.eSuccess", new LocToken[0]);
		case OnlineMultiplayerPrivilegeCheckResult.eNoNetwork:
			return Localization.Get("Online.ConnectionMode.Privilege.Result.eNoNetwork", new LocToken[0]);
		case OnlineMultiplayerPrivilegeCheckResult.ePatchRequired:
			return Localization.Get("Online.ConnectionMode.Privilege.Result.ePatchRequired", new LocToken[0]);
		case OnlineMultiplayerPrivilegeCheckResult.eSystemSoftwareUpdateRequired:
			return Localization.Get("Online.ConnectionMode.Privilege.Result.eSystemSoftwareUpdateRequired", new LocToken[0]);
		case OnlineMultiplayerPrivilegeCheckResult.eNotSignedInToPlatform:
			return Localization.Get("Online.ConnectionMode.Privilege.Result.eNotSignedInToPlatform", new LocToken[]
			{
				new LocToken("[NAME]", this.currentUser.DisplayName)
			});
		case OnlineMultiplayerPrivilegeCheckResult.eNoOnlineAccount:
			return Localization.Get("Online.ConnectionMode.Privilege.Result.eNoOnlineAccount", new LocToken[]
			{
				new LocToken("[NAME]", this.currentUser.DisplayName)
			});
		case OnlineMultiplayerPrivilegeCheckResult.eUnderAge:
			return Localization.Get("Online.ConnectionMode.Privilege.Result.eUnderAge", new LocToken[]
			{
				new LocToken("[NAME]", this.currentUser.DisplayName)
			});
		case OnlineMultiplayerPrivilegeCheckResult.eApplicationSuspended:
			return Localization.Get("Online.ConnectionMode.Privilege.Result.eApplicationSuspended", new LocToken[0]);
		case OnlineMultiplayerPrivilegeCheckResult.eGenericFailure:
			return Localization.Get("Online.ConnectionMode.Privilege.Result.eGenericFailure", new LocToken[0]);
		}
		return Localization.Get("Online.Privileges.CheckFailed", new LocToken[]
		{
			new LocToken("[NAME]", this.currentUser.DisplayName)
		});
	}

	// Token: 0x06002952 RID: 10578 RVA: 0x000C1B0D File Offset: 0x000BFF0D
	public override bool DisplayPlatformDialog()
	{
		return this.privilegeCheckResult.DisplayPlatformSpecificError(false);
	}

	// Token: 0x06002953 RID: 10579 RVA: 0x000C1B1C File Offset: 0x000BFF1C
	public override IConnectionModeSwitchStatus Clone()
	{
		return new PrivilegeStatus
		{
			Result = this.Result,
			Progress = this.Progress,
			currentUser = this.currentUser,
			privilegeCheckResult = this.privilegeCheckResult
		};
	}

	// Token: 0x040020A6 RID: 8358
	public GamepadUser currentUser;

	// Token: 0x040020A7 RID: 8359
	public OnlineMultiplayerReturnCode<OnlineMultiplayerPrivilegeCheckResult> privilegeCheckResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerPrivilegeCheckResult>();
}
