using System;
using Team17.Online;
using UnityEngine;

// Token: 0x02000774 RID: 1908
public class T17InGameFlow : MonoBehaviour
{
	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x060024D5 RID: 9429 RVA: 0x000AE719 File Offset: 0x000ACB19
	public static T17InGameFlow Instance
	{
		get
		{
			return T17InGameFlow.s_Instance;
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x060024D6 RID: 9430 RVA: 0x000AE720 File Offset: 0x000ACB20
	public bool WasPauseMenuOpen
	{
		get
		{
			return this.m_wasPauseMenuOpen;
		}
	}

	// Token: 0x060024D7 RID: 9431 RVA: 0x000AE728 File Offset: 0x000ACB28
	public void Awake()
	{
		if (T17InGameFlow.s_Instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			T17InGameFlow.s_Instance = this;
		}
		if (this.m_BackgroundImage != null)
		{
			this.m_BackgroundImage.SetActive(false);
		}
		this.m_IPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		GameObject gameMetaEnvironment = GameUtils.GetGameMetaEnvironment();
		this.m_EventSystem = GameObject.Find("EventSystems");
		if (this.m_EventSystem != null)
		{
			this.m_EventSystem.SetActive(false);
		}
	}

	// Token: 0x060024D8 RID: 9432 RVA: 0x000AE7B1 File Offset: 0x000ACBB1
	private void OnDestroy()
	{
		if (T17InGameFlow.s_Instance != null)
		{
			UnityEngine.Object.Destroy(T17InGameFlow.s_Instance);
			T17InGameFlow.s_Instance = null;
		}
		if (this.m_EventSystem != null)
		{
			this.m_EventSystem.SetActive(true);
		}
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x000AE7F0 File Offset: 0x000ACBF0
	private void Start()
	{
		if (!this.m_IPlayerManager.HasPlayer())
		{
		}
		GamepadUser user = this.m_IPlayerManager.GetUser(EngagementSlot.One);
		if (this.m_Rootmenu != null)
		{
			this.m_Rootmenu.Show(user, null, null, true);
		}
	}

	// Token: 0x060024DA RID: 9434 RVA: 0x000AE83C File Offset: 0x000ACC3C
	private void Update()
	{
		if (this.m_EventSystem != null)
		{
			if (T17DialogBoxManager.HasAnyOpenDialogs() || this.m_Rootmenu.GetCurrentOpenMenu() != null || this.m_bInRoundResults)
			{
				this.m_EventSystem.SetActive(true);
			}
			else
			{
				this.m_EventSystem.SetActive(false);
			}
		}
		if (this.m_wasPauseMenuOpen)
		{
			this.m_wasPauseMenuOpen = this.IsPauseMenuOpen();
		}
	}

	// Token: 0x060024DB RID: 9435 RVA: 0x000AE8BC File Offset: 0x000ACCBC
	public void OpenPauseMenu(bool isWorldMap, PlayerInputLookup.Player playerThatRequestedPause = PlayerInputLookup.Player.One)
	{
		if (playerThatRequestedPause >= (PlayerInputLookup.Player)ClientUserSystem.m_Users.Count)
		{
			return;
		}
		User user = ClientUserSystem.m_Users._items[(int)playerThatRequestedPause];
		GamepadUser gamepadUser = (user == null) ? null : user.GamepadUser;
		int num = (!isWorldMap) ? 1 : 0;
		if (this.m_EventSystem != null)
		{
			this.m_EventSystem.SetActive(true);
		}
		if (!this.IsPauseMenuOpen())
		{
			this.m_pauseMenu.Show(gamepadUser, null, null, true);
			this.m_pauseMenu.SetPauseType(isWorldMap);
		}
		if (this.m_BackgroundImage != null)
		{
			this.m_BackgroundImage.SetActive(true);
		}
		this.m_wasPauseMenuOpen = true;
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x000AE970 File Offset: 0x000ACD70
	public bool IsPauseMenuOpen()
	{
		return this.m_Rootmenu.IsCurrentOpenMenuOfType(InGameRootMenu.IngameMenuTypeToOpen.InLevelPause) || this.m_Rootmenu.IsCurrentOpenMenuOfType(InGameRootMenu.IngameMenuTypeToOpen.WorldMapPause);
	}

	// Token: 0x060024DD RID: 9437 RVA: 0x000AE992 File Offset: 0x000ACD92
	public void RequestClosePauseMenu()
	{
		if (this.m_Rootmenu.IsCurrentOpenMenuOfType(InGameRootMenu.IngameMenuTypeToOpen.WorldMapPause) || this.m_Rootmenu.IsCurrentOpenMenuOfType(InGameRootMenu.IngameMenuTypeToOpen.InLevelPause))
		{
			this.ClosePauseMenu();
		}
	}

	// Token: 0x060024DE RID: 9438 RVA: 0x000AE9BC File Offset: 0x000ACDBC
	public void ClosePauseMenu()
	{
		if (this.m_pauseMenu != null)
		{
			this.m_pauseMenu.Hide(true, false);
		}
		if (this.m_BackgroundImage != null)
		{
			this.m_BackgroundImage.SetActive(false);
		}
		if (this.m_DoThisOnClose != null)
		{
			this.m_DoThisOnClose();
		}
	}

	// Token: 0x060024DF RID: 9439 RVA: 0x000AEA1B File Offset: 0x000ACE1B
	public void RegisterWhatToDoOnPauseMenuClose(CallbackVoid doThis)
	{
		this.m_DoThisOnClose = doThis;
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x000AEA24 File Offset: 0x000ACE24
	public void RegisterOnPauseMenuVisibilityChanged(BaseMenuBehaviour.BaseMenuBehaviourEvent _callback)
	{
		if (this.m_pauseMenu != null)
		{
			InGamePauseMenu pauseMenu = this.m_pauseMenu;
			pauseMenu.OnShow = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Combine(pauseMenu.OnShow, _callback);
			InGamePauseMenu pauseMenu2 = this.m_pauseMenu;
			pauseMenu2.OnHide = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Combine(pauseMenu2.OnHide, _callback);
		}
	}

	// Token: 0x060024E1 RID: 9441 RVA: 0x000AEA7C File Offset: 0x000ACE7C
	public void UnRegisterOnPauseMenuVisibilityChanged(BaseMenuBehaviour.BaseMenuBehaviourEvent _callback)
	{
		if (this.m_pauseMenu != null)
		{
			InGamePauseMenu pauseMenu = this.m_pauseMenu;
			pauseMenu.OnShow = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Remove(pauseMenu.OnShow, _callback);
			InGamePauseMenu pauseMenu2 = this.m_pauseMenu;
			pauseMenu2.OnHide = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Remove(pauseMenu2.OnHide, _callback);
		}
	}

	// Token: 0x060024E2 RID: 9442 RVA: 0x000AEAD2 File Offset: 0x000ACED2
	public void SetInRoundResults(bool bState)
	{
		this.m_bInRoundResults = bState;
	}

	// Token: 0x04001C6F RID: 7279
	private static T17InGameFlow s_Instance;

	// Token: 0x04001C70 RID: 7280
	[SerializeField]
	public InGameRootMenu m_Rootmenu;

	// Token: 0x04001C71 RID: 7281
	[SerializeField]
	public GameObject m_BackgroundImage;

	// Token: 0x04001C72 RID: 7282
	[SerializeField]
	private InGamePauseMenu m_pauseMenu;

	// Token: 0x04001C73 RID: 7283
	private IPlayerManager m_IPlayerManager;

	// Token: 0x04001C74 RID: 7284
	private CallbackVoid m_DoThisOnClose;

	// Token: 0x04001C75 RID: 7285
	private GameObject m_ObjectBeforeFocusSwitch;

	// Token: 0x04001C76 RID: 7286
	private GameObject m_EventSystem;

	// Token: 0x04001C77 RID: 7287
	private bool m_bInRoundResults;

	// Token: 0x04001C78 RID: 7288
	private bool m_wasPauseMenuOpen;
}
