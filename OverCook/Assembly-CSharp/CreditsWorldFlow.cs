using System;
using System.Collections;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000652 RID: 1618
public class CreditsWorldFlow : Manager
{
	// Token: 0x06001ECD RID: 7885 RVA: 0x00096BE0 File Offset: 0x00094FE0
	private void Awake()
	{
		this.m_startId = Animator.StringToHash(this.m_start);
		this.m_finishedId = Animator.StringToHash(this.m_finished);
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		ClientMessenger.GameState(GameState.RanLevelIntro);
		DisconnectionHandler.LocalDisconnectionEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>)Delegate.Combine(DisconnectionHandler.LocalDisconnectionEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>(this.OnLocalDisconnection));
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			if (this.m_skipUIPrefab != null)
			{
				GameObject skipCanvas = GameUtils.InstantiateUIController(this.m_skipUIPrefab, "UICanvas");
				this.m_skipCanvas = skipCanvas;
				this.m_skipCanvas.SetActive(false);
			}
			this.m_skipRoutine = this.RunSkipRoutine();
		}
	}

	// Token: 0x06001ECE RID: 7886 RVA: 0x00096CA3 File Offset: 0x000950A3
	private void OnLocalDisconnection(OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult> result)
	{
		ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, delegate(IConnectionModeSwitchStatus status)
		{
			ServerGameSetup.Mode = GameMode.OnlineKitchen;
			ServerMessenger.LoadLevel("StartScreen", GameState.MainMenu, true, GameState.NotSet);
		});
	}

	// Token: 0x06001ECF RID: 7887 RVA: 0x00096CCA File Offset: 0x000950CA
	private void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		DisconnectionHandler.LocalDisconnectionEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>)Delegate.Remove(DisconnectionHandler.LocalDisconnectionEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>(this.OnLocalDisconnection));
	}

	// Token: 0x06001ED0 RID: 7888 RVA: 0x00096D04 File Offset: 0x00095104
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		GameState state = gameStateMessage.m_State;
		if (state == GameState.InLevel)
		{
			this.m_levelRoutine = this.RunLevel();
			base.StartCoroutine(this.m_levelRoutine);
		}
	}

	// Token: 0x06001ED1 RID: 7889 RVA: 0x00096D4C File Offset: 0x0009514C
	private void Update()
	{
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			GameState state = this.m_state;
			if (state != GameState.LoadKitchen)
			{
				if (state == GameState.InLevel)
				{
					if (!this.m_skipped && this.m_skipRoutine != null && !this.m_skipRoutine.MoveNext())
					{
						this.m_skipped = true;
					}
					if (this.AreAllUsersInGameState(GameState.RanLevelOutro) || this.m_skipped)
					{
						this.m_state = GameState.RanLevelOutro;
						MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
						multiplayerController.StopSynchronisation();
						GameSession gameSession = GameUtils.GetGameSession();
						gameSession.FillShownMetaDialogStatus();
						ServerMessenger.GameProgressData(gameSession.Progress.SaveData, gameSession.m_shownMetaDialogs);
						ServerMessenger.LoadLevel(gameSession.TypeSettings.WorldMapScene, GameState.CampaignMap, true, GameState.RunMapUnfoldRoutine);
					}
				}
			}
			else if (this.AreAllUsersInGameState(GameState.RanLevelIntro))
			{
				this.ChangeGameState(GameState.InLevel);
			}
		}
	}

	// Token: 0x06001ED2 RID: 7890 RVA: 0x00096E38 File Offset: 0x00095238
	private IEnumerator RunLevel()
	{
		this.m_animator.SetTrigger(this.m_startId);
		for (;;)
		{
			if (base.enabled)
			{
				this.m_isFinished = this.m_animator.GetBool(this.m_finishedId);
				if (this.HasFinished())
				{
					break;
				}
			}
			yield return null;
		}
		ClientMessenger.GameState(GameState.RanLevelOutro);
		yield break;
	}

	// Token: 0x06001ED3 RID: 7891 RVA: 0x00096E54 File Offset: 0x00095254
	private IEnumerator RunSkipRoutine()
	{
		if (this.m_skipCanvas != null)
		{
			ILogicalButton startSkip = PlayerInputLookup.GetAnyButton(PlayerInputLookup.LogicalButtonID.UISkip, PadSide.Both);
			while (!startSkip.JustReleased())
			{
				yield return null;
			}
		}
		ILogicalButton rawSkipButton = PlayerInputLookup.GetAnyButton(PlayerInputLookup.LogicalButtonID.UISkip, PadSide.Both);
		TimedLogicalButton skipButton = new TimedLogicalButton(rawSkipButton, TimedLogicalButton.Condition.HeldLonger, 1f);
		if (this.m_skipCanvas != null)
		{
			this.m_skipCanvas.SetActive(true);
			TimedInputUIController timedInputUIController = this.m_skipCanvas.gameObject.RequestComponentRecursive<TimedInputUIController>();
			timedInputUIController.SetDisplayInput(skipButton, 1f);
		}
		bool skipped = false;
		while (!skipped)
		{
			if (this.m_skipCanvas != null)
			{
				if (!this.m_skipCanvas.activeSelf)
				{
					while (!rawSkipButton.IsDown())
					{
						yield return null;
					}
				}
				this.m_skipCanvas.SetActive(true);
				IEnumerator timoutRoutine = null;
				while (!skipped && (timoutRoutine == null || timoutRoutine.MoveNext()))
				{
					if (timoutRoutine == null)
					{
						if (!rawSkipButton.IsDown())
						{
							timoutRoutine = CoroutineUtils.TimerRoutine(2f, LayerMask.NameToLayer("UI"));
						}
					}
					else if (rawSkipButton.IsDown())
					{
						timoutRoutine = null;
					}
					if (skipButton.JustPressed())
					{
						skipped = true;
					}
					yield return null;
				}
				this.m_skipCanvas.SetActive(false);
			}
			else
			{
				while (!skipButton.JustPressed())
				{
					yield return null;
				}
				skipped = true;
			}
		}
		yield break;
	}

	// Token: 0x06001ED4 RID: 7892 RVA: 0x00096E6F File Offset: 0x0009526F
	private bool HasFinished()
	{
		return this.m_isFinished;
	}

	// Token: 0x06001ED5 RID: 7893 RVA: 0x00096E77 File Offset: 0x00095277
	private void ChangeGameState(GameState state)
	{
		UserSystemUtils.ChangeGameState(state, null);
		this.m_state = state;
	}

	// Token: 0x06001ED6 RID: 7894 RVA: 0x00096E87 File Offset: 0x00095287
	private bool AreAllUsersInGameState(GameState state)
	{
		return UserSystemUtils.AreAllUsersInGameState(ServerUserSystem.m_Users, state);
	}

	// Token: 0x06001ED7 RID: 7895 RVA: 0x00096E94 File Offset: 0x00095294
	public void SkipToEnd()
	{
		this.m_isFinished = true;
	}

	// Token: 0x04001798 RID: 6040
	private GameState m_state = GameState.LoadKitchen;

	// Token: 0x04001799 RID: 6041
	private IEnumerator m_levelRoutine;

	// Token: 0x0400179A RID: 6042
	[SerializeField]
	private Animator m_animator;

	// Token: 0x0400179B RID: 6043
	[SerializeField]
	private string m_start = "Start";

	// Token: 0x0400179C RID: 6044
	[SerializeField]
	private string m_finished = "Finished";

	// Token: 0x0400179D RID: 6045
	private int m_startId;

	// Token: 0x0400179E RID: 6046
	private int m_finishedId;

	// Token: 0x0400179F RID: 6047
	private GameObject m_skipCanvas;

	// Token: 0x040017A0 RID: 6048
	[SerializeField]
	[AssignResource("CutsceneSkipUI", Editorbility.NonEditable)]
	private GameObject m_skipUIPrefab;

	// Token: 0x040017A1 RID: 6049
	private bool m_isFinished;

	// Token: 0x040017A2 RID: 6050
	private bool m_skipped;

	// Token: 0x040017A3 RID: 6051
	private IEnumerator m_skipRoutine;

	// Token: 0x040017A4 RID: 6052
	private const float c_skipHoldDuration = 1f;

	// Token: 0x040017A5 RID: 6053
	private const float c_skipPromptTimout = 2f;
}
