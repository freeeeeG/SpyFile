using System;
using Team17.Online;
using UnityEngine;

// Token: 0x02000A8B RID: 2699
public class EmoteWheel : MonoBehaviour
{
	// Token: 0x170003BC RID: 956
	// (get) Token: 0x0600356C RID: 13676 RVA: 0x000F977C File Offset: 0x000F7B7C
	public bool IsLocal
	{
		get
		{
			if (this.ForUI)
			{
				int player = (int)this.m_player;
				return player < ClientUserSystem.m_Users.Count && this.m_player != PlayerInputLookup.Player.Count && ClientUserSystem.m_Users._items[player].IsLocal;
			}
			return this.m_playerIDProvider.GetID() != PlayerInputLookup.Player.Count;
		}
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x0600356D RID: 13677 RVA: 0x000F97DE File Offset: 0x000F7BDE
	public bool ForUI
	{
		get
		{
			return this.m_uiPlayer != null;
		}
	}

	// Token: 0x0600356E RID: 13678 RVA: 0x000F97EC File Offset: 0x000F7BEC
	public void Awake()
	{
		if (!this.m_showInPauseMenu && base.transform.root.GetComponentInChildren<InGamePauseMenu>() != null)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x0600356F RID: 13679 RVA: 0x000F981A File Offset: 0x000F7C1A
	public void Start()
	{
		this.Setup();
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		this.SpawnNetworkComponents();
	}

	// Token: 0x06003570 RID: 13680 RVA: 0x000F9848 File Offset: 0x000F7C48
	protected void SpawnNetworkComponents()
	{
		if (this.ForUI)
		{
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				base.gameObject.AddComponent<ServerEmoteWheel>();
			}
			base.gameObject.AddComponent<ClientEmoteWheel>();
		}
	}

	// Token: 0x06003571 RID: 13681 RVA: 0x000F9881 File Offset: 0x000F7C81
	private void Setup()
	{
		this.m_player = this.GetPlayer();
		if (!this.ForUI)
		{
			this.SetupForIngame();
		}
	}

	// Token: 0x06003572 RID: 13682 RVA: 0x000F98A0 File Offset: 0x000F7CA0
	private void SetupForIngame()
	{
		this.m_playerControls = base.gameObject.RequireComponent<PlayerControls>();
		this.m_playerSwitchManager = GameUtils.RequireManager<PlayerSwitchingManager>();
		this.m_playerIDProvider = base.gameObject.RequireComponent<PlayerIDProvider>();
		this.m_playerRespawn = base.gameObject.RequestComponent<ClientPlayerRespawnBehaviour>();
	}

	// Token: 0x06003573 RID: 13683 RVA: 0x000F98E0 File Offset: 0x000F7CE0
	public bool CanShow()
	{
		if (T17DialogBoxManager.HasAnyOpenDialogs())
		{
			return false;
		}
		if (this.ForUI)
		{
			if (!this.IsLocal)
			{
				return false;
			}
		}
		else
		{
			if (T17InGameFlow.Instance != null && T17InGameFlow.Instance.IsPauseMenuOpen())
			{
				return false;
			}
			if (TimeManager.IsPaused(TimeManager.PauseLayer.Main) || TimeManager.IsPaused(TimeManager.PauseLayer.Network))
			{
				return false;
			}
			if (!this.IsLocal)
			{
				return false;
			}
			if (this.m_playerRespawn != null && this.m_playerRespawn.IsRespawning)
			{
				return false;
			}
			if (this.m_playerSwitchManager != null)
			{
				PlayerControls x = this.m_playerSwitchManager.SelectedAvatar(this.m_playerIDProvider.GetID());
				return x != null && x == this.m_playerControls;
			}
		}
		return this.m_player != PlayerInputLookup.Player.Count;
	}

	// Token: 0x06003574 RID: 13684 RVA: 0x000F99D0 File Offset: 0x000F7DD0
	protected PlayerInputLookup.Player GetPlayer()
	{
		if (this.m_uiPlayer == null && this.m_player == PlayerInputLookup.Player.Count)
		{
			this.m_uiPlayer = base.gameObject.RequestComponent<UIPlayerMenuBehaviour>();
		}
		if (this.m_uiPlayer != null)
		{
			int num = ClientUserSystem.m_Users.FindIndex((User user) => user == this.m_uiPlayer.UserInfo);
			return (PlayerInputLookup.Player)((num == -1) ? 11 : num);
		}
		if (this.m_player != PlayerInputLookup.Player.Count)
		{
			return this.m_player;
		}
		if (ClientUserSystem.m_Users.Count != 0)
		{
			PlayerIDProvider playerIDProvider = base.gameObject.RequestComponent<PlayerIDProvider>();
			if (playerIDProvider != null)
			{
				return playerIDProvider.GetID();
			}
		}
		return PlayerInputLookup.Player.Count;
	}

	// Token: 0x06003575 RID: 13685 RVA: 0x000F9A88 File Offset: 0x000F7E88
	protected void OnUsersChanged()
	{
		this.Setup();
	}

	// Token: 0x06003576 RID: 13686 RVA: 0x000F9A90 File Offset: 0x000F7E90
	protected virtual void OnDestroy()
	{
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
	}

	// Token: 0x04002AD4 RID: 10964
	public EmoteWheelOptions m_emoteWheelOptions;

	// Token: 0x04002AD5 RID: 10965
	public Animator m_animationTarget;

	// Token: 0x04002AD6 RID: 10966
	public GameObject m_codeTriggerTarget;

	// Token: 0x04002AD7 RID: 10967
	[SerializeField]
	public bool m_showInPauseMenu;

	// Token: 0x04002AD8 RID: 10968
	[SerializeField]
	[Range(0f, 1f)]
	public float m_analogEnableThreshold = 0.25f;

	// Token: 0x04002AD9 RID: 10969
	[HideInInspector]
	public PlayerInputLookup.Player m_player = PlayerInputLookup.Player.Count;

	// Token: 0x04002ADA RID: 10970
	[HideInInspector]
	public UIPlayerMenuBehaviour m_uiPlayer;

	// Token: 0x04002ADB RID: 10971
	[HideInInspector]
	public PlayerControls m_playerControls;

	// Token: 0x04002ADC RID: 10972
	[HideInInspector]
	public ClientPlayerRespawnBehaviour m_playerRespawn;

	// Token: 0x04002ADD RID: 10973
	[HideInInspector]
	public PlayerSwitchingManager m_playerSwitchManager;

	// Token: 0x04002ADE RID: 10974
	[HideInInspector]
	public PlayerIDProvider m_playerIDProvider;
}
