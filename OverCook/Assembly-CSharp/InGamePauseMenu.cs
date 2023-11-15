using System;
using GameModes;
using Team17.Online;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000ACB RID: 2763
public class InGamePauseMenu : InGameMenuBehaviour
{
	// Token: 0x060037AF RID: 14255 RVA: 0x00106397 File Offset: 0x00104797
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
	}

	// Token: 0x060037B0 RID: 14256 RVA: 0x001063A0 File Offset: 0x001047A0
	private void SetupForPlayer(int playerIdx)
	{
		bool flag = playerIdx == 0;
		this.m_buttons.SetActive(flag);
		this.m_PausedText.gameObject.SetActive(!flag);
		if (!flag)
		{
			string nonLocalizedText = Localization.Get("Text.Menu.PlayerPaused", new LocToken[]
			{
				new LocToken("[Name]", (playerIdx + 1).ToString())
			});
			this.m_PausedText.SetNonLocalizedText(nonLocalizedText);
		}
	}

	// Token: 0x060037B1 RID: 14257 RVA: 0x0010641C File Offset: 0x0010481C
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		int num = 0;
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			User user = ClientUserSystem.m_Users._items[i];
			if (user.IsLocal)
			{
				if (user.GamepadUser == currentGamer)
				{
					this.SetupForPlayer(num);
					break;
				}
				num++;
			}
		}
		if (this.m_IPlayerManager != null)
		{
			this.m_IPlayerManager.EngagementChangeCallback += this.OnEngagementChanged;
		}
		if (this.m_header != null)
		{
			GameSession gameSession = GameUtils.GetGameSession();
			if (gameSession == null || SceneManager.GetActiveScene().name == gameSession.TypeSettings.WorldMapScene)
			{
				this.m_header.SetLocalisedTextCatchAll(this.MenuName);
			}
			else if (gameSession != null)
			{
				SceneDirectoryData sceneDirectory = gameSession.Progress.GetSceneDirectory();
				if (sceneDirectory != null)
				{
					SceneDirectoryData.SceneDirectoryEntry[] scenes = sceneDirectory.Scenes;
					int levelID = GameUtils.GetLevelID();
					if (levelID >= 0 && levelID < scenes.Length)
					{
						this.m_header.SetLocalisedTextCatchAll(scenes[levelID].Label);
					}
				}
			}
		}
		GameSession gameSession2 = GameUtils.GetGameSession();
		this.m_gameModeSettingsButton.gameObject.SetActive((ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession()) && gameSession2 != null && this.m_gameModeUIData.m_gameModes[(int)gameSession2.GameModeKind].m_supportedSettings.Length > 0);
		return true;
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x001065D4 File Offset: 0x001049D4
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (this.m_IPlayerManager != null)
		{
			this.m_IPlayerManager.EngagementChangeCallback -= this.OnEngagementChanged;
		}
		if (this.m_uiPlayers != null)
		{
			this.m_uiPlayers.CloseAllPlayerMenus();
		}
		return base.Hide(restoreInvokerState, isTabSwitch);
	}

	// Token: 0x060037B3 RID: 14259 RVA: 0x00106632 File Offset: 0x00104A32
	protected override void Update()
	{
		base.Update();
	}

	// Token: 0x060037B4 RID: 14260 RVA: 0x0010663C File Offset: 0x00104A3C
	public void SetPauseType(bool isWorldMapPause)
	{
		bool flag = !isWorldMapPause;
		if (flag && ConnectionStatus.IsInSession())
		{
			flag = (ClientGameSetup.Mode == GameMode.Campaign && ConnectionStatus.IsHost());
		}
		this.m_RestartButton.gameObject.SetActive(flag);
		if (this.m_CustomisationButton != null)
		{
			this.m_CustomisationButton.gameObject.SetActive(isWorldMapPause);
		}
	}

	// Token: 0x060037B5 RID: 14261 RVA: 0x001066AC File Offset: 0x00104AAC
	private void ShowRestartDialog()
	{
		if (this.m_confirmDialog == null)
		{
			this.m_confirmDialog = T17DialogBoxManager.GetDialog(false);
			if (this.m_confirmDialog != null)
			{
				this.m_confirmDialog.Initialize("Text.Pause.Restart.Title", "Text.Pause.Restart.Message", "Text.Button.Restart", null, "Text.Button.Cancel", T17DialogBox.Symbols.Unassigned, true, true, false);
				T17DialogBox confirmDialog = this.m_confirmDialog;
				confirmDialog.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(confirmDialog.OnConfirm, new T17DialogBox.DialogEvent(this.OnRestartConfirmed));
				T17DialogBox confirmDialog2 = this.m_confirmDialog;
				confirmDialog2.OnCancel = (T17DialogBox.DialogEvent)Delegate.Combine(confirmDialog2.OnCancel, new T17DialogBox.DialogEvent(this.HideConfirmDialog));
				this.m_confirmDialog.Show();
			}
		}
	}

	// Token: 0x060037B6 RID: 14262 RVA: 0x00106764 File Offset: 0x00104B64
	private void OnRestartConfirmed()
	{
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
			multiplayerController.StopSynchronisation();
			ServerMessenger.LoadLevel(SceneManager.GetActiveScene().name, GameState.LoadKitchen, true, GameState.RunKitchen);
		}
		if (GameUtils.RequestManager<FlowControllerBase>() != null)
		{
			Analytics.LogEvent("Restart", 0L, Analytics.Flags.LevelName);
		}
	}

	// Token: 0x060037B7 RID: 14263 RVA: 0x001067C9 File Offset: 0x00104BC9
	public void OnRestartSelected()
	{
		this.ShowRestartDialog();
	}

	// Token: 0x060037B8 RID: 14264 RVA: 0x001067D4 File Offset: 0x00104BD4
	private void ShowQuitDialog()
	{
		if (this.m_confirmDialog == null)
		{
			this.m_confirmDialog = T17DialogBoxManager.GetDialog(false);
			if (this.m_confirmDialog != null)
			{
				this.m_confirmDialog.Initialize("Text.Pause.Quit.Title", "Text.Pause.Quit.Message", "Text.Button.Quit", null, "Text.Button.Cancel", T17DialogBox.Symbols.Unassigned, true, true, false);
				T17DialogBox confirmDialog = this.m_confirmDialog;
				confirmDialog.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(confirmDialog.OnConfirm, new T17DialogBox.DialogEvent(this.OnQuitConfirmed));
				T17DialogBox confirmDialog2 = this.m_confirmDialog;
				confirmDialog2.OnCancel = (T17DialogBox.DialogEvent)Delegate.Combine(confirmDialog2.OnCancel, new T17DialogBox.DialogEvent(this.HideConfirmDialog));
				this.m_confirmDialog.Show();
			}
		}
	}

	// Token: 0x060037B9 RID: 14265 RVA: 0x0010688C File Offset: 0x00104C8C
	private void HideConfirmDialog()
	{
		if (this.m_confirmDialog != null)
		{
			this.m_confirmDialog.Hide();
			this.m_confirmDialog = null;
		}
	}

	// Token: 0x060037BA RID: 14266 RVA: 0x001068B4 File Offset: 0x00104CB4
	private void OnQuitConfirmed()
	{
		if (GameUtils.RequestManager<FlowControllerBase>() != null)
		{
			Analytics.LogEvent(((!ConnectionStatus.IsHost()) ? "Client" : "Host") + " Quit", 0L, Analytics.Flags.LevelName);
		}
		MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
		multiplayerController.StopSynchronisation();
		GameUtils.QuitLevel();
	}

	// Token: 0x060037BB RID: 14267 RVA: 0x0010690D File Offset: 0x00104D0D
	public void OnQuitSelected()
	{
		this.ShowQuitDialog();
	}

	// Token: 0x060037BC RID: 14268 RVA: 0x00106918 File Offset: 0x00104D18
	private void OnEngagementChanged(EngagementSlot slot, GamepadUser prevUser, GamepadUser newUser)
	{
		if (slot == EngagementSlot.One && prevUser == null && newUser != null && this.m_IPlayerManager != null)
		{
			GamepadUser user = this.m_IPlayerManager.GetUser(slot);
			this.m_CurrentGamepadUser = user;
			this.m_CachedEventSystem = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(this.m_CurrentGamepadUser);
			if (this.m_CachedEventSystem != null && this.m_CachedEventSystem.GetLastRequestedSelectedGameobject() == null && this.m_BorderSelectables.selectOnUp != null)
			{
				this.m_CachedEventSystem.SetSelectedGameObject(this.m_BorderSelectables.selectOnUp.gameObject);
			}
		}
	}

	// Token: 0x060037BD RID: 14269 RVA: 0x001069D6 File Offset: 0x00104DD6
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.HideConfirmDialog();
	}

	// Token: 0x060037BE RID: 14270 RVA: 0x001069E4 File Offset: 0x00104DE4
	private void OnApplicationFocus(bool focus)
	{
		if (focus && this.m_CachedEventSystem != null)
		{
			this.m_CachedEventSystem.SetSelectedGameObject(this.m_CachedEventSystem.GetLastRequestedSelectedGameobject());
		}
	}

	// Token: 0x04002C9F RID: 11423
	public T17Text m_header;

	// Token: 0x04002CA0 RID: 11424
	public GameObject m_buttons;

	// Token: 0x04002CA1 RID: 11425
	public T17Button m_RestartButton;

	// Token: 0x04002CA2 RID: 11426
	public T17Button m_CustomisationButton;

	// Token: 0x04002CA3 RID: 11427
	[SerializeField]
	public T17Button m_gameModeSettingsButton;

	// Token: 0x04002CA4 RID: 11428
	public T17Text m_PausedText;

	// Token: 0x04002CA5 RID: 11429
	private T17DialogBox m_confirmDialog;

	// Token: 0x04002CA6 RID: 11430
	[SerializeField]
	private UIPlayerRootMenu m_uiPlayers;

	// Token: 0x04002CA7 RID: 11431
	[SerializeField]
	[AssignResource("GameModeUIData", Editorbility.Editable)]
	private GameModeUIData m_gameModeUIData;
}
