using System;
using System.Text;
using InControl;
using Steamworks;
using Team17.Online;
using UnityEngine;

// Token: 0x02000751 RID: 1873
public class SteamPlayerManager : PCPlayerManager
{
	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x060023F5 RID: 9205 RVA: 0x000AA74F File Offset: 0x000A8B4F
	public static bool Initialized
	{
		get
		{
			return SteamPlayerManager.s_instance != null && SteamPlayerManager.s_instance.m_bInitialized;
		}
	}

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x060023F6 RID: 9206 RVA: 0x000AA76E File Offset: 0x000A8B6E
	public AppId_t SteamAppID
	{
		get
		{
			return SteamPlayerManager.c_appID;
		}
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000AA775 File Offset: 0x000A8B75
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
	}

	// Token: 0x060023F8 RID: 9208 RVA: 0x000AA778 File Offset: 0x000A8B78
	protected override void Awake()
	{
		base.Awake();
		if (SteamPlayerManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamPlayerManager.s_instance = this;
		if (SteamPlayerManager.s_EverInialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
		}
		if (!DllCheck.Test())
		{
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(SteamPlayerManager.c_appID))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			return;
		}
		this.m_durationControlCallback = Callback<DurationControl_t>.Create(new Callback<DurationControl_t>.DispatchDelegate(this.OnDurationControl));
		SteamPlayerManager.s_EverInialized = true;
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x000AA850 File Offset: 0x000A8C50
	protected override PCPlayerManager.PCPlayerProfile EngagePadToSlot(ControlPadInput.PadNum _engagingPadNum, EngagementSlot _intendedSlot)
	{
		PlayerActionSet input = PCPadInputProvider.EngagePad(_engagingPadNum, (ControlPadInput.PadNum)_intendedSlot);
		SteamPlayerManager.SteamPlayerProfile steamPlayerProfile = new SteamPlayerManager.SteamPlayerProfile(SteamUser.GetSteamID(), SteamFriends.GetPersonaName(), input);
		if (this.m_lostUsers.Count > 0 && this.m_lostUsers[0].UID != steamPlayerProfile.UID)
		{
			PCPadInputProvider.DisengagePad((ControlPadInput.PadNum)_intendedSlot);
			return null;
		}
		steamPlayerProfile.StickyEngagement = (_intendedSlot == EngagementSlot.One);
		base.AssignProfileToSlot(_intendedSlot, steamPlayerProfile);
		return steamPlayerProfile;
	}

	// Token: 0x060023FA RID: 9210 RVA: 0x000AA8C4 File Offset: 0x000A8CC4
	private void OnEnable()
	{
		if (SteamPlayerManager.s_instance == null)
		{
			SteamPlayerManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamPlayerManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x000AA91B File Offset: 0x000A8D1B
	private new void OnDestroy()
	{
		if (SteamPlayerManager.s_instance != this)
		{
			return;
		}
		SteamPlayerManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x000AA945 File Offset: 0x000A8D45
	private void OnApplicationQuit()
	{
		this.m_applicationQuitting = true;
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x000AA94E File Offset: 0x000A8D4E
	protected override void Update()
	{
		base.Update();
		if (!this.m_bInitialized)
		{
			return;
		}
		if (!this.m_applicationQuitting)
		{
			SteamAPI.RunCallbacks();
		}
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000AA974 File Offset: 0x000A8D74
	public override void ShowGamerCard(GamepadUser localUser)
	{
		CSteamID steamID = SteamUser.GetSteamID();
		SteamFriends.ActivateGameOverlayToUser("steamid", steamID);
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x000AA992 File Offset: 0x000A8D92
	public override void ShowGamerCard(OnlineUserPlatformId onlineUser)
	{
		if (onlineUser == null)
		{
			return;
		}
		SteamFriends.ActivateGameOverlayToUser("steamid", onlineUser.m_steamId);
	}

	// Token: 0x06002400 RID: 9216 RVA: 0x000AA9AB File Offset: 0x000A8DAB
	public override bool SupportsInvitesForAnyUser()
	{
		return true;
	}

	// Token: 0x06002401 RID: 9217 RVA: 0x000AA9B0 File Offset: 0x000A8DB0
	private void OnGameOverlayActivated(GameOverlayActivated_t param)
	{
		if (T17EventSystemsManager.Instance != null)
		{
			bool flag = Convert.ToBoolean(param.m_bActive);
			if (flag)
			{
				T17EventSystemsManager.Instance.DisableAllEventSystemsExceptFor(null);
			}
			else
			{
				T17EventSystemsManager.Instance.EnableAllEventSystems();
			}
			SteamPlayerManager.OverlayVisbilityChanged(flag);
		}
	}

	// Token: 0x06002402 RID: 9218 RVA: 0x000AA9FF File Offset: 0x000A8DFF
	private void OnDurationControl(DurationControl_t param)
	{
		this.m_durationControlProgress = param.m_progress;
		this.m_bApplicable = param.m_bApplicable;
	}

	// Token: 0x06002403 RID: 9219 RVA: 0x000AAA1B File Offset: 0x000A8E1B
	public static EDurationControlProgress GetDurationControlProgress()
	{
		return SteamPlayerManager.s_instance.m_durationControlProgress;
	}

	// Token: 0x06002404 RID: 9220 RVA: 0x000AAA28 File Offset: 0x000A8E28
	public static float GetDurationControlModifier()
	{
		EDurationControlProgress durationControlProgress = SteamPlayerManager.s_instance.m_durationControlProgress;
		if (durationControlProgress == EDurationControlProgress.k_EDurationControlProgress_Full)
		{
			return 1f;
		}
		if (durationControlProgress == EDurationControlProgress.k_EDurationControlProgress_Half)
		{
			return 0.5f;
		}
		if (durationControlProgress != EDurationControlProgress.k_EDurationControlProgress_None)
		{
			return 1f;
		}
		return 0f;
	}

	// Token: 0x06002405 RID: 9221 RVA: 0x000AAA70 File Offset: 0x000A8E70
	public static bool IsProgressModifierApplicable()
	{
		return SteamUtils.IsSteamChinaLauncher() && SteamPlayerManager.s_instance.m_bApplicable;
	}

	// Token: 0x04001B83 RID: 7043
	private static readonly AppId_t c_appID = new AppId_t(728880U);

	// Token: 0x04001B84 RID: 7044
	private static SteamPlayerManager s_instance;

	// Token: 0x04001B85 RID: 7045
	protected Callback<GameOverlayActivated_t> m_gameOverlayActivatedCallback;

	// Token: 0x04001B86 RID: 7046
	protected Callback<DurationControl_t> m_durationControlCallback;

	// Token: 0x04001B87 RID: 7047
	private EDurationControlProgress m_durationControlProgress;

	// Token: 0x04001B88 RID: 7048
	private bool m_bApplicable;

	// Token: 0x04001B89 RID: 7049
	private static bool s_EverInialized;

	// Token: 0x04001B8A RID: 7050
	private bool m_bInitialized;

	// Token: 0x04001B8B RID: 7051
	private bool m_applicationQuitting;

	// Token: 0x04001B8C RID: 7052
	public static GenericVoid<bool> OverlayVisbilityChanged = delegate(bool _visible)
	{
	};

	// Token: 0x04001B8D RID: 7053
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	// Token: 0x02000752 RID: 1874
	public class SteamPlayerProfile : PCPlayerManager.PCPlayerProfile
	{
		// Token: 0x06002408 RID: 9224 RVA: 0x000AAAB0 File Offset: 0x000A8EB0
		public SteamPlayerProfile(CSteamID _steamID, string _username, PlayerActionSet _input) : base(_input)
		{
			this.m_steamID = _steamID;
			this.m_UID = this.m_steamID.m_SteamID.ToString() + ((_input.Device == null) ? "Keyboard" : _input.Device.Meta);
			this.m_username = _username;
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06002409 RID: 9225 RVA: 0x000AAB29 File Offset: 0x000A8F29
		public override string UID
		{
			get
			{
				return this.m_UID;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x0600240A RID: 9226 RVA: 0x000AAB31 File Offset: 0x000A8F31
		public override string DisplayName
		{
			get
			{
				return this.m_username;
			}
		}

		// Token: 0x04001B8E RID: 7054
		public CSteamID m_steamID;

		// Token: 0x04001B8F RID: 7055
		private string m_UID = string.Empty;

		// Token: 0x04001B90 RID: 7056
		private string m_username = string.Empty;
	}
}
