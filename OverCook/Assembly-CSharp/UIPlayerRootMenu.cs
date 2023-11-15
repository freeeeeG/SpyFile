using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000AE4 RID: 2788
public class UIPlayerRootMenu : RootMenu
{
	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06003879 RID: 14457 RVA: 0x0010A2FB File Offset: 0x001086FB
	// (set) Token: 0x0600387A RID: 14458 RVA: 0x0010A303 File Offset: 0x00108703
	public bool AllowSettingFocus
	{
		get
		{
			return this.m_bShouldSetFocus;
		}
		set
		{
			this.m_bShouldSetFocus = value;
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x0600387B RID: 14459 RVA: 0x0010A30C File Offset: 0x0010870C
	public List<UIPlayerMenuBehaviour> UIPlayers
	{
		get
		{
			return this.m_players;
		}
	}

	// Token: 0x0600387C RID: 14460 RVA: 0x0010A314 File Offset: 0x00108714
	protected override void Start()
	{
		base.Start();
		this.m_IPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		this.m_selectionCache.Clear();
		this.ReCreateUIPlayers();
		if (this.m_bShouldSetFocus && this.m_isActive && base.CachedEventSystem != null && this.m_players.Count > 0 && this.m_players[0].CanBeInteractedWith())
		{
			base.CachedEventSystem.SetSelectedGameObject(this.m_players[0].MenuButton.gameObject);
		}
		else
		{
			this.PollForEventSystem();
		}
		this.m_chefCamera.cullingMask = LayerMask.GetMask(new string[]
		{
			this.m_uiChefLayer
		});
		this.m_cameraResizer = this.m_chefCamera.gameObject.RequireComponent<OrthoCameraResizer>();
		this.m_target.material = this.m_renderMat;
		this.m_shadowTarget.material = this.m_renderMat;
		this.CreateRenderTexture();
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
	}

	// Token: 0x0600387D RID: 14461 RVA: 0x0010A43C File Offset: 0x0010883C
	public void ReCreateUIPlayers()
	{
		this.CacheSelection();
		UIPlayerMenuBehaviour[] componentsInChildren = this.m_playersContainer.GetComponentsInChildren<UIPlayerMenuBehaviour>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
		this.m_players.Clear();
		bool canKick = this.m_canKickUsers;
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			canKick = false;
		}
		int num = ClientUserSystem.m_Users.Count;
		if (this.IsMatchmaking())
		{
			num = 4;
		}
		Selectable selectable = null;
		for (int j = 0; j < num; j++)
		{
			UIPlayerMenuBehaviour uiplayerMenuBehaviour = UnityEngine.Object.Instantiate<UIPlayerMenuBehaviour>(this.m_uiPlayerPrefab, this.m_playersContainer);
			uiplayerMenuBehaviour.m_rootMenu = this;
			uiplayerMenuBehaviour.PlayerNumber = (j + 1).ToString();
			uiplayerMenuBehaviour.UserInfo = ((j >= ClientUserSystem.m_Users.Count) ? null : ClientUserSystem.m_Users._items[j]);
			uiplayerMenuBehaviour.m_canKick = canKick;
			if (j < this.m_playerRotations.Count)
			{
				uiplayerMenuBehaviour.transform.localRotation = Quaternion.Euler(this.m_playerRotations[j]);
			}
			if (selectable != null && uiplayerMenuBehaviour.UserInfo != null && uiplayerMenuBehaviour.CanBeInteractedWith())
			{
				this.ConnectHorizontal(selectable, uiplayerMenuBehaviour.MenuButton);
			}
			if (!uiplayerMenuBehaviour.CanBeInteractedWith())
			{
				ColorBlock colors = uiplayerMenuBehaviour.MenuButton.colors;
				colors.highlightedColor = colors.normalColor;
				uiplayerMenuBehaviour.MenuButton.colors = colors;
				Navigation navigation = default(Navigation);
				navigation.mode = Navigation.Mode.None;
				uiplayerMenuBehaviour.MenuButton.navigation = navigation;
			}
			this.m_players.Add(uiplayerMenuBehaviour);
			if (uiplayerMenuBehaviour.UserInfo != null)
			{
				selectable = uiplayerMenuBehaviour.MenuButton;
			}
		}
		this.m_BorderSelectables.selectOnUp = this.m_players[0].MenuButton;
		this.RestoreSelectionFromCache();
	}

	// Token: 0x0600387E RID: 14462 RVA: 0x0010A63C File Offset: 0x00108A3C
	protected void ConnectHorizontal(Selectable _left, Selectable _right)
	{
		Navigation navigation = _left.navigation;
		Navigation navigation2 = _right.navigation;
		navigation.mode = Navigation.Mode.Explicit;
		navigation2.mode = Navigation.Mode.Explicit;
		navigation.selectOnRight = _right;
		navigation2.selectOnLeft = _left;
		_left.navigation = navigation;
		_right.navigation = navigation2;
	}

	// Token: 0x0600387F RID: 14463 RVA: 0x0010A688 File Offset: 0x00108A88
	protected void CreateRenderTexture()
	{
		this.m_chefCamera.targetTexture = null;
		if (this.m_rendTex != null)
		{
			RenderTargetManager.ReleaseRenderTarget(ref this.m_rendTexID);
			this.m_rendTex = null;
		}
		Vector2 targetDimensions = this.m_cameraResizer.TargetDimensions;
		this.m_rendTex = RenderTargetManager.RequestRenderTarget(Mathf.RoundToInt(targetDimensions.x), Mathf.RoundToInt(targetDimensions.y), 16, RenderTextureFormat.ARGB32, ref this.m_rendTexID, "DN");
		this.m_target.material.SetTexture("_MainTex", this.m_rendTex);
		this.m_target.SetMaterialDirty();
		this.m_shadowTarget.material.SetTexture("_MainTex", this.m_rendTex);
		this.m_shadowTarget.SetMaterialDirty();
		this.m_rendTex.filterMode = FilterMode.Bilinear;
		this.m_chefCamera.targetTexture = this.m_rendTex;
	}

	// Token: 0x06003880 RID: 14464 RVA: 0x0010A76C File Offset: 0x00108B6C
	protected override void Update()
	{
		base.Update();
		Vector2 targetDimensions = this.m_cameraResizer.TargetDimensions;
		if (this.m_rendTex == null || (float)this.m_rendTex.width != targetDimensions.x || (float)this.m_rendTex.height != targetDimensions.y)
		{
			this.CreateRenderTexture();
		}
		this.PollForEventSystem();
	}

	// Token: 0x06003881 RID: 14465 RVA: 0x0010A7D8 File Offset: 0x00108BD8
	protected void PollForEventSystem()
	{
		if (!this.m_isActive)
		{
			this.m_CachedEventSystem = null;
			return;
		}
		bool flag = base.CachedEventSystem != null;
		GamepadUser user = this.m_IPlayerManager.GetUser(EngagementSlot.One);
		if (!flag)
		{
			this.m_CachedEventSystem = ((!(user != null)) ? null : T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user));
		}
		if (this.m_bShouldSetFocus && !flag)
		{
			this.FocusOnFirstPlayer(false);
		}
	}

	// Token: 0x06003882 RID: 14466 RVA: 0x0010A854 File Offset: 0x00108C54
	public UIPlayerMenuBehaviour GetUIPlayerForUser(string _id)
	{
		for (int i = 0; i < this.m_players.Count; i++)
		{
			UIPlayerMenuBehaviour uiplayerMenuBehaviour = this.m_players[i];
			if (uiplayerMenuBehaviour != null && uiplayerMenuBehaviour.UserInfo != null && uiplayerMenuBehaviour.UserInfo.DisplayName == _id)
			{
				return this.m_players[i];
			}
		}
		return null;
	}

	// Token: 0x06003883 RID: 14467 RVA: 0x0010A8C8 File Offset: 0x00108CC8
	public UIPlayerMenuBehaviour GetUIPlayerForUser(User _user)
	{
		for (int i = 0; i < this.m_players.Count; i++)
		{
			if (this.m_players[i] != null && this.m_players[i].UserInfo == _user)
			{
				return this.m_players[i];
			}
		}
		return null;
	}

	// Token: 0x06003884 RID: 14468 RVA: 0x0010A930 File Offset: 0x00108D30
	protected void OnUsersChanged()
	{
		this.CacheSelection();
		Selectable selectable = null;
		for (int i = 0; i < this.m_players.Count; i++)
		{
			UIPlayerMenuBehaviour uiplayerMenuBehaviour = this.m_players[i];
			uiplayerMenuBehaviour.UserInfo = ((i >= ClientUserSystem.m_Users.Count) ? null : ClientUserSystem.m_Users._items[i]);
			if (selectable != null && uiplayerMenuBehaviour.UserInfo != null && uiplayerMenuBehaviour.CanBeInteractedWith())
			{
				this.ConnectHorizontal(selectable, uiplayerMenuBehaviour.MenuButton);
			}
			if (!uiplayerMenuBehaviour.CanBeInteractedWith())
			{
				ColorBlock colors = uiplayerMenuBehaviour.MenuButton.colors;
				colors.highlightedColor = colors.normalColor;
				uiplayerMenuBehaviour.MenuButton.colors = colors;
				Navigation navigation = default(Navigation);
				navigation.mode = Navigation.Mode.None;
				uiplayerMenuBehaviour.MenuButton.navigation = navigation;
			}
			if (uiplayerMenuBehaviour.UserInfo != null)
			{
				selectable = uiplayerMenuBehaviour.MenuButton;
			}
		}
		this.m_BorderSelectables.selectOnUp = this.m_players[0].MenuButton;
		this.RestoreSelectionFromCache();
	}

	// Token: 0x06003885 RID: 14469 RVA: 0x0010AA48 File Offset: 0x00108E48
	public bool IsMatchmaking()
	{
		LobbySetupInfo instance = LobbySetupInfo.Instance;
		if (SceneManager.GetActiveScene().name == "Lobbies")
		{
			if (instance != null)
			{
				if (instance.m_connectionMode == OnlineMultiplayerConnectionMode.eInternet && instance.m_visiblity == OnlineMultiplayerSessionVisibility.eMatchmaking)
				{
					return true;
				}
				if (instance.m_visiblity == OnlineMultiplayerSessionVisibility.eClosed || instance.m_visiblity == OnlineMultiplayerSessionVisibility.ePrivate)
				{
					return false;
				}
			}
			else if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Matchmake)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003886 RID: 14470 RVA: 0x0010AACC File Offset: 0x00108ECC
	private void CacheSelection()
	{
		this.m_selectionCache.Clear();
		if (base.CachedEventSystem != null)
		{
			GameObject gameObject = base.CachedEventSystem.GetPendingSelectedGameObject();
			if (gameObject == null)
			{
				gameObject = base.CachedEventSystem.currentSelectedGameObject;
			}
			if (gameObject != null && gameObject.IsInHierarchyOf(base.gameObject))
			{
				UIPlayerMenuBehaviour uiPlayer = gameObject.RequestComponentRecursive<UIPlayerMenuBehaviour>();
				if (uiPlayer == null)
				{
					uiPlayer = gameObject.RequestComponentUpwardsRecursive<UIPlayerMenuBehaviour>();
				}
				if (uiPlayer != null)
				{
					this.m_selectionCache.m_selectedUserName = uiPlayer.LastSetDisplayName;
					this.m_selectionCache.m_selectedIdx = this.m_players.FindIndex((UIPlayerMenuBehaviour x) => x == uiPlayer);
					this.m_selectionCache.m_selectedOption = uiPlayer.GetSelectedMenuOption();
				}
			}
			this.m_selectionCache.m_hasCached = true;
		}
	}

	// Token: 0x06003887 RID: 14471 RVA: 0x0010ABD0 File Offset: 0x00108FD0
	private void RestoreSelectionFromCache()
	{
		if (!this.m_selectionCache.m_hasCached)
		{
			return;
		}
		if (this.m_selectionCache.m_selectedUserName != null && base.CachedEventSystem != null)
		{
			GameObject pendingSelectedGameObject = base.CachedEventSystem.GetPendingSelectedGameObject();
			if (pendingSelectedGameObject == null || pendingSelectedGameObject.IsInHierarchyOf(base.gameObject))
			{
				UIPlayerMenuBehaviour uiplayerForUser = this.GetUIPlayerForUser(this.m_selectionCache.m_selectedUserName);
				if (uiplayerForUser != null)
				{
					UIPlayerMenuBehaviour.UIPlayerMenuOptions? selectedOption = this.m_selectionCache.m_selectedOption;
					if (selectedOption != null)
					{
						uiplayerForUser.ShowMenu(this.m_selectionCache.m_selectedOption);
					}
					else
					{
						base.CachedEventSystem.SetSelectedGameObject(uiplayerForUser.MenuButton.gameObject);
					}
				}
				else if (this.m_selectionCache.m_selectedIdx > 0)
				{
					int num = Mathf.Min(this.m_selectionCache.m_selectedIdx, this.m_players.Count - 1);
					for (int i = num; i >= 0; i--)
					{
						Selectable menuButton = this.m_players[i].MenuButton;
						if (menuButton.isActiveAndEnabled && menuButton.IsInteractable())
						{
							base.CachedEventSystem.SetSelectedGameObject(menuButton.gameObject);
							break;
						}
					}
				}
				else
				{
					this.FocusOnFirstPlayer(true);
				}
			}
		}
		this.m_selectionCache.Clear();
	}

	// Token: 0x06003888 RID: 14472 RVA: 0x0010AD40 File Offset: 0x00109140
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_rendTex != null)
		{
			this.m_chefCamera.targetTexture = null;
			RenderTargetManager.ReleaseRenderTarget(ref this.m_rendTexID);
			this.m_rendTex = null;
		}
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
	}

	// Token: 0x06003889 RID: 14473 RVA: 0x0010ADA4 File Offset: 0x001091A4
	public bool CanFocusOnFirstPlayer(bool _force = false)
	{
		return base.CachedEventSystem != null && this.m_players.Count > 0 && this.m_players[0].CanBeInteractedWith() && (_force || base.CachedEventSystem.currentSelectedGameObject == null || !base.CachedEventSystem.currentSelectedGameObject.activeInHierarchy) && (_force || base.CachedEventSystem.GetLastRequestedSelectedGameobject() == null);
	}

	// Token: 0x0600388A RID: 14474 RVA: 0x0010AE39 File Offset: 0x00109239
	public void FocusOnFirstPlayer(bool _force = false)
	{
		if (this.CanFocusOnFirstPlayer(_force))
		{
			base.CachedEventSystem.SetSelectedGameObject(this.m_players[0].MenuButton.gameObject);
		}
	}

	// Token: 0x0600388B RID: 14475 RVA: 0x0010AE68 File Offset: 0x00109268
	public void CloseAllPlayerMenus()
	{
		for (int i = 0; i < this.m_players.Count; i++)
		{
			if (this.m_players[i] != null)
			{
				this.m_players[i].HideMenu(true);
			}
		}
	}

	// Token: 0x04002D2C RID: 11564
	[SerializeField]
	[AssignResource("UIPlayer", Editorbility.Editable)]
	private UIPlayerMenuBehaviour m_uiPlayerPrefab;

	// Token: 0x04002D2D RID: 11565
	private List<UIPlayerMenuBehaviour> m_players = new List<UIPlayerMenuBehaviour>();

	// Token: 0x04002D2E RID: 11566
	[SerializeField]
	[AssignChildRecursive("Background", Editorbility.Editable)]
	public Transform m_backgroundsContainer;

	// Token: 0x04002D2F RID: 11567
	[SerializeField]
	[AssignChildRecursive("Background", Editorbility.Editable)]
	private Transform m_playersContainer;

	// Token: 0x04002D30 RID: 11568
	[SerializeField]
	private List<Vector3> m_playerRotations = new List<Vector3>();

	// Token: 0x04002D31 RID: 11569
	private IPlayerManager m_IPlayerManager;

	// Token: 0x04002D32 RID: 11570
	[SerializeField]
	private Camera m_chefCamera;

	// Token: 0x04002D33 RID: 11571
	[SerializeField]
	private Material m_renderMat;

	// Token: 0x04002D34 RID: 11572
	[SerializeField]
	private T17Image m_target;

	// Token: 0x04002D35 RID: 11573
	[SerializeField]
	private T17Image m_shadowTarget;

	// Token: 0x04002D36 RID: 11574
	private OrthoCameraResizer m_cameraResizer;

	// Token: 0x04002D37 RID: 11575
	[SerializeField]
	public string m_uiChefLayer;

	// Token: 0x04002D38 RID: 11576
	private RenderTexture m_rendTex;

	// Token: 0x04002D39 RID: 11577
	public bool m_canKickUsers;

	// Token: 0x04002D3A RID: 11578
	public bool m_isActive = true;

	// Token: 0x04002D3B RID: 11579
	private bool m_bShouldSetFocus = true;

	// Token: 0x04002D3C RID: 11580
	private UIPlayerRootMenu.SelectionCache m_selectionCache;

	// Token: 0x04002D3D RID: 11581
	private int m_rendTexID = -1;

	// Token: 0x02000AE5 RID: 2789
	private struct SelectionCache
	{
		// Token: 0x0600388C RID: 14476 RVA: 0x0010AEBC File Offset: 0x001092BC
		public void Clear()
		{
			this.m_hasCached = false;
			this.m_selectedUserName = null;
			this.m_selectedIdx = -1;
			this.m_selectedOption = null;
		}

		// Token: 0x04002D3E RID: 11582
		public bool m_hasCached;

		// Token: 0x04002D3F RID: 11583
		public string m_selectedUserName;

		// Token: 0x04002D40 RID: 11584
		public int m_selectedIdx;

		// Token: 0x04002D41 RID: 11585
		public UIPlayerMenuBehaviour.UIPlayerMenuOptions? m_selectedOption;
	}
}
