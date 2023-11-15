using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200072A RID: 1834
[ExecutionDependency(typeof(PlayerManager))]
public class OvercookedEngagementController : Manager
{
	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x060022DF RID: 8927 RVA: 0x000A72AD File Offset: 0x000A56AD
	public OvercookedEngagementController.LevelType GlobalLevelType
	{
		get
		{
			return this.m_levelType;
		}
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x060022E0 RID: 8928 RVA: 0x000A72B5 File Offset: 0x000A56B5
	// (set) Token: 0x060022E1 RID: 8929 RVA: 0x000A72BD File Offset: 0x000A56BD
	public bool IsClientMode
	{
		get
		{
			return this.m_isClientMode;
		}
		set
		{
			this.m_isClientMode = value;
			this.Refresh();
		}
	}

	// Token: 0x060022E2 RID: 8930 RVA: 0x000A72CC File Offset: 0x000A56CC
	private void Awake()
	{
		this.Refresh();
		this.m_playerManager.EngagementChangeCallback += this.OnEngagementChanged;
		LobbyUIController.OpenCloseCallback += this.OnLobbyOpenClosed;
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x060022E3 RID: 8931 RVA: 0x000A7318 File Offset: 0x000A5718
	private void OnLobbyOpenClosed(bool _isOpen)
	{
		this.Refresh();
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x000A7320 File Offset: 0x000A5720
	private void OnDestroy()
	{
		LobbyUIController.OpenCloseCallback -= this.OnLobbyOpenClosed;
		this.m_playerManager.EngagementChangeCallback -= this.OnEngagementChanged;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x060022E5 RID: 8933 RVA: 0x000A735C File Offset: 0x000A575C
	private void OnEngagementChanged(EngagementSlot _e, GamepadUser _prev, GamepadUser _new)
	{
		if (_prev != null && _new != null)
		{
			IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
			if (playerManager.GetUser(EngagementSlot.One) == _new)
			{
				T17EventSystem eventSystemForGamepadUser = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(_prev);
				SuppressionController suppressionController = new SuppressionController();
				if (eventSystemForGamepadUser != null)
				{
					eventSystemForGamepadUser.SuppressionController.MoveSuppressors(suppressionController);
					T17EventSystemsManager.Instance.ResetEventSystem(eventSystemForGamepadUser);
				}
				if (T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(_new) == null)
				{
					T17EventSystem t17EventSystem = T17EventSystemsManager.Instance.AssignFreeEventSystemToGamepadUser(_new);
					suppressionController.MoveSuppressors(t17EventSystem.SuppressionController);
				}
			}
		}
		this.Refresh();
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x000A7402 File Offset: 0x000A5802
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (mode != LoadSceneMode.Additive)
		{
			this.Refresh();
		}
	}

	// Token: 0x060022E7 RID: 8935 RVA: 0x000A7411 File Offset: 0x000A5811
	private bool UIShouldOpen()
	{
		return this.m_playerManager.HasPlayer() && this.m_levelType == OvercookedEngagementController.LevelType.WithoutChefs;
	}

	// Token: 0x060022E8 RID: 8936 RVA: 0x000A7430 File Offset: 0x000A5830
	private void Refresh()
	{
		this.m_levelType = OvercookedEngagementController.GetLevelType();
		this.m_playerManager.CanChangeSplitPads = (this.m_levelType == OvercookedEngagementController.LevelType.WithoutChefs);
		GameSession gameSession = GameUtils.GetGameSession();
		string name = SceneManager.GetActiveScene().name;
		bool usersSticky = this.m_levelType == OvercookedEngagementController.LevelType.WithChefs || (gameSession != null && name.Equals(gameSession.TypeSettings.WorldMapScene)) || this.IsClientMode;
		this.SetUsersSticky(usersSticky);
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x000A74B0 File Offset: 0x000A58B0
	private void SetUsersSticky(bool _sticky)
	{
		for (int i = 0; i < 4; i++)
		{
			if (i != 0)
			{
				GamepadUser user = this.m_playerManager.GetUser((EngagementSlot)i);
				FastList<User> users = ClientUserSystem.m_Users;
				User.MachineID s_LocalMachineId = ClientUserSystem.s_LocalMachineId;
				User user2 = UserSystemUtils.FindUser(users, null, s_LocalMachineId, (EngagementSlot)i, TeamID.Count, User.SplitStatus.Count);
				if (user != null && user2 != null)
				{
					user.StickyEngagement = _sticky;
				}
			}
		}
	}

	// Token: 0x060022EA RID: 8938 RVA: 0x000A7518 File Offset: 0x000A5918
	public static OvercookedEngagementController.LevelType GetLevelType()
	{
		CampaignKitchenLoaderManager x = GameUtils.RequestManager<CampaignKitchenLoaderManager>();
		CompetitiveKitchenLoaderManager x2 = GameUtils.RequestManager<CompetitiveKitchenLoaderManager>();
		string name = SceneManager.GetActiveScene().name;
		if (x == null && x2 == null && name != "Loading" && name != "Credits")
		{
			return OvercookedEngagementController.LevelType.WithoutChefs;
		}
		return OvercookedEngagementController.LevelType.WithChefs;
	}

	// Token: 0x04001AB8 RID: 6840
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private PlayerManager m_playerManager;

	// Token: 0x04001AB9 RID: 6841
	private OvercookedEngagementController.LevelType m_levelType;

	// Token: 0x04001ABA RID: 6842
	private bool m_isClientMode;

	// Token: 0x0200072B RID: 1835
	public enum LevelType
	{
		// Token: 0x04001ABC RID: 6844
		WithChefs,
		// Token: 0x04001ABD RID: 6845
		WithoutChefs
	}
}
