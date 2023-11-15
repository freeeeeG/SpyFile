using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200075E RID: 1886
[RequireComponent(typeof(TimeManager))]
public class PauseMenuManager : Manager
{
	// Token: 0x0600244E RID: 9294 RVA: 0x000AC4AC File Offset: 0x000AA8AC
	private void Start()
	{
		this.m_timeManager = base.gameObject.RequireComponent<TimeManager>();
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
		this.m_startButtons = this.BuildStartButtons();
		this.ClaimStartPressEvent();
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
	}

	// Token: 0x0600244F RID: 9295 RVA: 0x000AC507 File Offset: 0x000AA907
	private void OnUsersChanged()
	{
		this.m_startButtons = this.BuildStartButtons();
	}

	// Token: 0x06002450 RID: 9296 RVA: 0x000AC515 File Offset: 0x000AA915
	private void OnDestroy()
	{
		if (this.m_sessionData != null)
		{
			this.Close();
		}
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
	}

	// Token: 0x06002451 RID: 9297 RVA: 0x000AC548 File Offset: 0x000AA948
	private void OnDisable()
	{
		if (this.m_sessionData != null && T17InGameFlow.Instance != null)
		{
			T17InGameFlow.Instance.RequestClosePauseMenu();
		}
	}

	// Token: 0x06002452 RID: 9298 RVA: 0x000AC570 File Offset: 0x000AA970
	private ILogicalButton[] BuildStartButtons()
	{
		ILogicalButton[] array = new ILogicalButton[4];
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			ILogicalButton logicalButton;
			if (i < ClientUserSystem.m_Users.Count && ClientUserSystem.m_Users._items[i].IsLocal)
			{
				logicalButton = PlayerInputLookup.GetEngagedButton(PlayerInputLookup.LogicalButtonID.Pause, (PlayerInputLookup.Player)num, PadSide.Both);
				num++;
			}
			else
			{
				logicalButton = new ComboLogicalButton(new ILogicalButton[0], ComboLogicalButton.ComboType.Or);
			}
			array[i] = logicalButton;
		}
		return array;
	}

	// Token: 0x06002453 RID: 9299 RVA: 0x000AC5E8 File Offset: 0x000AA9E8
	private void ClaimStartPressEvent()
	{
		for (int i = 0; i < this.m_startButtons.Length; i++)
		{
			this.m_startButtons[i].ClaimPressEvent();
		}
	}

	// Token: 0x06002454 RID: 9300 RVA: 0x000AC61C File Offset: 0x000AAA1C
	private PlayerInputLookup.Player GetFirstJustPressedStartButton()
	{
		PlayerInputLookup.Player player = PlayerInputLookup.Player.Count;
		for (int i = 0; i < this.m_startButtons.Length; i++)
		{
			if (this.m_startButtons[i].JustPressed() && player == PlayerInputLookup.Player.Count)
			{
				player = (PlayerInputLookup.Player)i;
			}
		}
		return player;
	}

	// Token: 0x06002455 RID: 9301 RVA: 0x000AC664 File Offset: 0x000AAA64
	private static ILogicalButton BuildButton(PlayerInputLookup.LogicalButtonID _buttonID)
	{
		OvercookedEngagementController.LevelType levelType = OvercookedEngagementController.GetLevelType();
		FastList<User> fastList = new FastList<User>();
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			if (ClientUserSystem.m_Users._items[i].IsLocal)
			{
				fastList.Add(ClientUserSystem.m_Users._items[i]);
			}
		}
		return PlayerInputLookup.GetAnyButton(_buttonID, PadSide.Both);
	}

	// Token: 0x06002456 RID: 9302 RVA: 0x000AC6C7 File Offset: 0x000AAAC7
	private void Update()
	{
		if (this.m_awaitDestroy)
		{
			return;
		}
		if (this.m_sessionData == null)
		{
			this.UpdateWhileClosed();
		}
		else
		{
			this.UpdateWhileOpen();
		}
	}

	// Token: 0x06002457 RID: 9303 RVA: 0x000AC6F4 File Offset: 0x000AAAF4
	private void UpdateWhileClosed()
	{
		PlayerInputLookup.Player firstJustPressedStartButton = this.GetFirstJustPressedStartButton();
		if (TimeManager.IsPaused(TimeManager.PauseLayer.System) || TimeManager.IsPaused(TimeManager.PauseLayer.Network) || firstJustPressedStartButton != PlayerInputLookup.Player.Count)
		{
			this.m_holdingDownFromOpen = true;
			this.Pause(firstJustPressedStartButton);
		}
	}

	// Token: 0x06002458 RID: 9304 RVA: 0x000AC734 File Offset: 0x000AAB34
	private void Pause(PlayerInputLookup.Player _playerThatRequestedPause = PlayerInputLookup.Player.One)
	{
		if (T17InGameFlow.Instance != null && T17InGameFlow.Instance.m_Rootmenu.GetCurrentOpenMenu() == null)
		{
			GameSession gameSession = GameUtils.GetGameSession();
			if (gameSession != null)
			{
				this.m_sessionData = new PauseMenuManager.SessionData(this.m_startGUIPrefab, this.m_uiCanvasName, this.m_entries);
				this.m_PausedLayers = this.GetLayersToPause();
				for (int i = 0; i < this.m_PausedLayers.Length; i++)
				{
					this.m_timeManager.SetPaused(this.m_PausedLayers[i], true, this);
				}
				if (_playerThatRequestedPause == PlayerInputLookup.Player.Count)
				{
					FastList<User> users = ClientUserSystem.m_Users;
					for (int j = 0; j < users.Count; j++)
					{
						User user = users._items[j];
						if (user.IsLocal && user.GamepadUser == null)
						{
							_playerThatRequestedPause = (PlayerInputLookup.Player)j;
							break;
						}
					}
				}
				GameSession.GameTypeSettings typeSettings = gameSession.TypeSettings;
				T17InGameFlow.Instance.OpenPauseMenu(SceneManager.GetActiveScene().name == typeSettings.WorldMapScene, _playerThatRequestedPause);
				T17InGameFlow.Instance.RegisterWhatToDoOnPauseMenuClose(new CallbackVoid(this.Close));
			}
		}
	}

	// Token: 0x06002459 RID: 9305 RVA: 0x000AC86C File Offset: 0x000AAC6C
	private TimeManager.PauseLayer[] GetLayersToPause()
	{
		if (ConnectionStatus.IsInSession() && UserSystemUtils.AnyRemoteUsers())
		{
			return new TimeManager.PauseLayer[]
			{
				TimeManager.PauseLayer.Network
			};
		}
		return new TimeManager.PauseLayer[]
		{
			TimeManager.PauseLayer.Main,
			TimeManager.PauseLayer.UI
		};
	}

	// Token: 0x0600245A RID: 9306 RVA: 0x000AC898 File Offset: 0x000AAC98
	private void ConsumeAllButtons()
	{
		this.ClaimStartPressEvent();
		if (this.m_sessionData != null)
		{
			this.m_sessionData.m_uiSelectButton.ClaimPressEvent();
			this.m_sessionData.m_uiSelectButton.ClaimReleaseEvent();
			this.m_sessionData.m_uiCancelButton.ClaimPressEvent();
			this.m_sessionData.m_uiCancelButton.ClaimReleaseEvent();
			this.m_sessionData.m_uiDownButton.ClaimPressEvent();
			this.m_sessionData.m_uiDownButton.ClaimReleaseEvent();
			this.m_sessionData.m_uiUpButton.ClaimPressEvent();
			this.m_sessionData.m_uiUpButton.ClaimReleaseEvent();
		}
	}

	// Token: 0x0600245B RID: 9307 RVA: 0x000AC938 File Offset: 0x000AAD38
	private void UpdateWhileOpen()
	{
		if (TimeManager.IsPaused(TimeManager.PauseLayer.System) || this.m_holdingDownFromOpen)
		{
			if (!this.m_sessionData.m_uiCancelButton.IsDown())
			{
				this.m_holdingDownFromOpen = false;
			}
			this.ConsumeAllButtons();
			return;
		}
		bool flag = this.GetFirstJustPressedStartButton() != PlayerInputLookup.Player.Count || this.m_sessionData.m_uiCancelButton.JustReleased();
		bool flag2 = T17DialogBoxManager.HasAnyOpenDialogs() || GameUtils.RequireManager<PlayerManager>().IsWarningActive(PlayerWarning.Disengaged);
		if (flag && !flag2)
		{
			T17InGameFlow.Instance.RequestClosePauseMenu();
			return;
		}
	}

	// Token: 0x0600245C RID: 9308 RVA: 0x000AC9D0 File Offset: 0x000AADD0
	private void OnControlsSelected()
	{
		if (this.m_uiCanvasName == "ScalingHUDCanvas")
		{
			this.m_optionsInstance = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_controlsScreenPrefab.gameObject);
		}
		else
		{
			this.m_optionsInstance = GameUtils.InstantiateUIController(this.m_controlsScreenPrefab.gameObject, this.m_uiCanvasName);
		}
	}

	// Token: 0x0600245D RID: 9309 RVA: 0x000ACA2C File Offset: 0x000AAE2C
	private void Close()
	{
		this.m_sessionData = null;
		for (int i = 0; i < this.m_PausedLayers.Length; i++)
		{
			this.m_timeManager.SetPaused(this.m_PausedLayers[i], false, this);
		}
		this.m_PausedLayers = null;
		this.ClaimStartPressEvent();
	}

	// Token: 0x0600245E RID: 9310 RVA: 0x000ACA7B File Offset: 0x000AAE7B
	private static FrontendListEntry.NameData[] GetMenuNames(PauseMenuManager.MenuEntries[] _menuEntries)
	{
		return _menuEntries.ConvertAll((PauseMenuManager.MenuEntries x) => new FrontendListEntry.NameData("PauseMenu." + x.ToString(), true, false));
	}

	// Token: 0x04001BB9 RID: 7097
	[SerializeField]
	private GameObject m_startGUIPrefab;

	// Token: 0x04001BBA RID: 7098
	[SerializeField]
	private string m_uiCanvasName = "ScalingHUDCanvas";

	// Token: 0x04001BBB RID: 7099
	[SerializeField]
	private PauseMenuManager.MenuEntries[] m_entries = new PauseMenuManager.MenuEntries[]
	{
		PauseMenuManager.MenuEntries.Resume,
		PauseMenuManager.MenuEntries.Restart,
		PauseMenuManager.MenuEntries.Quit
	};

	// Token: 0x04001BBC RID: 7100
	[SerializeField]
	[AssignResource("ControlsScreen", Editorbility.NonEditable)]
	private Image m_controlsScreenPrefab;

	// Token: 0x04001BBD RID: 7101
	[SerializeField]
	[AssignResource("UnsidedAmbiControlsMappingData", Editorbility.NonEditable)]
	private AmbiControlsMappingData m_unsidedAmbiMapping;

	// Token: 0x04001BBE RID: 7102
	private ILogicalButton[] m_startButtons;

	// Token: 0x04001BBF RID: 7103
	private PauseMenuManager.SessionData m_sessionData;

	// Token: 0x04001BC0 RID: 7104
	private TimeManager m_timeManager;

	// Token: 0x04001BC1 RID: 7105
	private GameObject m_optionsInstance;

	// Token: 0x04001BC2 RID: 7106
	private PlayerManager m_playerManager;

	// Token: 0x04001BC3 RID: 7107
	private bool m_awaitDestroy;

	// Token: 0x04001BC4 RID: 7108
	private TimeManager.PauseLayer[] m_PausedLayers;

	// Token: 0x04001BC5 RID: 7109
	private bool m_holdingDownFromOpen;

	// Token: 0x0200075F RID: 1887
	private enum MenuEntries
	{
		// Token: 0x04001BC8 RID: 7112
		Resume,
		// Token: 0x04001BC9 RID: 7113
		Restart,
		// Token: 0x04001BCA RID: 7114
		Quit,
		// Token: 0x04001BCB RID: 7115
		Controls
	}

	// Token: 0x02000760 RID: 1888
	private class SessionData
	{
		// Token: 0x06002460 RID: 9312 RVA: 0x000ACAC0 File Offset: 0x000AAEC0
		public SessionData(GameObject _startGUIPrefab, string _canvas, PauseMenuManager.MenuEntries[] _menuEntries)
		{
			this.m_uiUpButton = PauseMenuManager.BuildButton(PlayerInputLookup.LogicalButtonID.UIUp);
			this.m_uiDownButton = PauseMenuManager.BuildButton(PlayerInputLookup.LogicalButtonID.UIDown);
			this.m_uiSelectButton = PauseMenuManager.BuildButton(PlayerInputLookup.LogicalButtonID.UISelect);
			this.m_uiCancelButton = PauseMenuManager.BuildButton(PlayerInputLookup.LogicalButtonID.UICancel);
		}

		// Token: 0x04001BCC RID: 7116
		public ILogicalButton m_uiUpButton;

		// Token: 0x04001BCD RID: 7117
		public ILogicalButton m_uiDownButton;

		// Token: 0x04001BCE RID: 7118
		public ILogicalButton m_uiSelectButton;

		// Token: 0x04001BCF RID: 7119
		public ILogicalButton m_uiCancelButton;
	}
}
