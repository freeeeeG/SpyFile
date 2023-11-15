using System;

// Token: 0x02000739 RID: 1849
public class RichPresenceMangerBase : Manager
{
	// Token: 0x06002369 RID: 9065 RVA: 0x000AAB4C File Offset: 0x000A8F4C
	private void Awake()
	{
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
		this.m_playerManager.EngagementChangeCallback += this.OnEngagementChanged;
		RichPresenceMangerBase.OnGameModeSet = (GenericVoid)Delegate.Combine(RichPresenceMangerBase.OnGameModeSet, new GenericVoid(this.OnNewGameMode));
		this.Initialise();
	}

	// Token: 0x0600236A RID: 9066 RVA: 0x000AABA1 File Offset: 0x000A8FA1
	private void OnDestroy()
	{
		this.m_playerManager.EngagementChangeCallback -= this.OnEngagementChanged;
		RichPresenceMangerBase.OnGameModeSet = (GenericVoid)Delegate.Remove(RichPresenceMangerBase.OnGameModeSet, new GenericVoid(this.OnNewGameMode));
	}

	// Token: 0x0600236B RID: 9067 RVA: 0x000AABDA File Offset: 0x000A8FDA
	private void OnNewGameMode()
	{
		this.RefreshPresence();
	}

	// Token: 0x0600236C RID: 9068 RVA: 0x000AABE2 File Offset: 0x000A8FE2
	private void OnEngagementChanged(EngagementSlot _slot, GamepadUser _prevUser, GamepadUser _nextUser)
	{
		if (_nextUser != null)
		{
			this.RefreshPresence();
		}
		if (_prevUser != null)
		{
			this.OnUserDisengaged(_prevUser);
		}
	}

	// Token: 0x0600236D RID: 9069 RVA: 0x000AAC09 File Offset: 0x000A9009
	protected virtual void Initialise()
	{
	}

	// Token: 0x0600236E RID: 9070 RVA: 0x000AAC0B File Offset: 0x000A900B
	protected virtual bool UserIsValidForPresence(GamepadUser user)
	{
		return true;
	}

	// Token: 0x0600236F RID: 9071 RVA: 0x000AAC10 File Offset: 0x000A9010
	private void RefreshPresence()
	{
		for (int i = 0; i < 4; i++)
		{
			EngagementSlot slot = (EngagementSlot)i;
			GamepadUser user = this.m_playerManager.GetUser(slot);
			if (this.UserIsValidForPresence(user))
			{
				switch (RichPresenceMangerBase.m_gameMode)
				{
				case GameMode.OnlineKitchen:
					this.SetOnlineKitchenPresence(user);
					break;
				case GameMode.Campaign:
					this.SetCampaignPresence(user);
					break;
				case GameMode.Party:
					this.SetPartyPresence(user);
					break;
				case GameMode.Versus:
					this.SetVersusPresence(user);
					break;
				default:
					this.SetDefaultPresence(user);
					break;
				}
			}
		}
	}

	// Token: 0x06002370 RID: 9072 RVA: 0x000AACA7 File Offset: 0x000A90A7
	protected virtual void SetDefaultPresence(GamepadUser _user)
	{
	}

	// Token: 0x06002371 RID: 9073 RVA: 0x000AACA9 File Offset: 0x000A90A9
	protected virtual void SetOnlineKitchenPresence(GamepadUser _user)
	{
	}

	// Token: 0x06002372 RID: 9074 RVA: 0x000AACAB File Offset: 0x000A90AB
	protected virtual void SetCampaignPresence(GamepadUser _user)
	{
	}

	// Token: 0x06002373 RID: 9075 RVA: 0x000AACAD File Offset: 0x000A90AD
	protected virtual void SetPartyPresence(GamepadUser _user)
	{
	}

	// Token: 0x06002374 RID: 9076 RVA: 0x000AACAF File Offset: 0x000A90AF
	protected virtual void SetVersusPresence(GamepadUser _user)
	{
	}

	// Token: 0x06002375 RID: 9077 RVA: 0x000AACB1 File Offset: 0x000A90B1
	protected virtual void OnUserDisengaged(GamepadUser _user)
	{
	}

	// Token: 0x04001AE5 RID: 6885
	protected const string sc_onlineKitchenId = "OnlineKitchen";

	// Token: 0x04001AE6 RID: 6886
	protected const string sc_campaignId = "Campaign";

	// Token: 0x04001AE7 RID: 6887
	protected const string sc_versusId = "Versus";

	// Token: 0x04001AE8 RID: 6888
	protected const string sc_partyId = "Party";

	// Token: 0x04001AE9 RID: 6889
	protected const string sc_engagedId = "Engaged";

	// Token: 0x04001AEA RID: 6890
	protected static GameMode m_gameMode = GameMode.COUNT;

	// Token: 0x04001AEB RID: 6891
	protected static GenericVoid OnGameModeSet;

	// Token: 0x04001AEC RID: 6892
	protected PlayerManager m_playerManager;
}
