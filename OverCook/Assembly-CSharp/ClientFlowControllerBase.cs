using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000679 RID: 1657
public abstract class ClientFlowControllerBase : ClientSynchroniserBase, IFlowController
{
	// Token: 0x06001FBF RID: 8127 RVA: 0x000946A1 File Offset: 0x00092AA1
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_flowControllerBase = (FlowControllerBase)synchronisedObject;
	}

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x06001FC0 RID: 8128 RVA: 0x000946B8 File Offset: 0x00092AB8
	// (remove) Token: 0x06001FC1 RID: 8129 RVA: 0x000946F0 File Offset: 0x00092AF0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CallbackVoid RoundActivatedCallback = delegate()
	{
	};

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x06001FC2 RID: 8130 RVA: 0x00094728 File Offset: 0x00092B28
	// (remove) Token: 0x06001FC3 RID: 8131 RVA: 0x00094760 File Offset: 0x00092B60
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CallbackVoid RoundDeactivatedCallback = delegate()
	{
	};

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06001FC4 RID: 8132 RVA: 0x00094796 File Offset: 0x00092B96
	// (set) Token: 0x06001FC5 RID: 8133 RVA: 0x0009479E File Offset: 0x00092B9E
	protected LevelConfigBase LevelConfig { get; set; }

	// Token: 0x06001FC6 RID: 8134 RVA: 0x000947A8 File Offset: 0x00092BA8
	protected virtual void Awake()
	{
		this.m_iOnlineMultiplayerNotificationCoordinator = GameUtils.RequireManagerInterface<IOnlinePlatformManager>().OnlineMultiplayerNotificationCoordinator();
		GameSession.GameLevelSettings levelSettings = GameUtils.GetGameSession().LevelSettings;
		if (levelSettings != null && levelSettings.SceneDirectoryVarientEntry != null)
		{
			this.LevelConfig = levelSettings.SceneDirectoryVarientEntry.LevelConfig;
		}
		this.m_campaignAudioManager = GameUtils.RequireManager<CampaignAudioManager>();
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x00094815 File Offset: 0x00092C15
	protected virtual void Start()
	{
		if (!LoadingScreenFlow.IsLoadingStartScreen())
		{
			GameUtils.LoadScene("InGameMenu", LoadSceneMode.Additive);
		}
	}

	// Token: 0x06001FC8 RID: 8136 RVA: 0x0009482C File Offset: 0x00092C2C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06001FC9 RID: 8137 RVA: 0x0009484C File Offset: 0x00092C4C
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		GameState state = gameStateMessage.m_State;
		if (state != GameState.RunLevelIntro)
		{
			if (state != GameState.InLevel)
			{
				if (state == GameState.RunLevelOutro)
				{
					this.m_isFinished = true;
					this.m_levelRoutine = null;
					this.m_outroRoutine = this.RunLevelEnd();
				}
			}
			else
			{
				this.m_levelRoutine = this.RunLevel();
				base.StartCoroutine(this.m_levelRoutine);
			}
		}
		else
		{
			CallbackVoid finishedCallback = delegate()
			{
				ClientMessenger.GameState(GameState.RanLevelIntro);
			};
			base.StartCoroutine(this.RunLevelIntro(finishedCallback));
		}
	}

	// Token: 0x06001FCA RID: 8138 RVA: 0x000948F4 File Offset: 0x00092CF4
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_outroRoutine != null && !this.m_outroRoutine.MoveNext())
		{
			this.m_outroRoutine = null;
			if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
			{
				MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
				multiplayerController.StopSynchronisation();
			}
			ClientMessenger.GameState(GameState.RanLevelOutro);
		}
	}

	// Token: 0x06001FCB RID: 8139 RVA: 0x00094950 File Offset: 0x00092D50
	private IEnumerator RunLevelIntro(CallbackVoid _finishedCallback)
	{
		this.m_campaignAudioManager.SetAudioState(CampaignAudioManager.AudioState.IntroState);
		IntroFlowroutineBase levelIntroFlowroutine = base.gameObject.RequireComponent<IntroFlowroutineBase>();
		levelIntroFlowroutine.Setup(delegate
		{
		});
		IEnumerator levelIntro = levelIntroFlowroutine.Run();
		while (levelIntro.MoveNext())
		{
			object obj = levelIntro.Current;
			yield return obj;
		}
		_finishedCallback();
		yield break;
	}

	// Token: 0x06001FCC RID: 8140 RVA: 0x00094974 File Offset: 0x00092D74
	private IEnumerator RunLevel()
	{
		this.SetRoundBehaviourActivation(true);
		this.m_campaignAudioManager.SetAudioState(CampaignAudioManager.AudioState.InLevel);
		for (;;)
		{
			if (base.enabled)
			{
				this.OnUpdateInRound();
				this.UpdateOnlineMultiplayerNotifications();
				if (this.HasFinished())
				{
					break;
				}
			}
			yield return null;
		}
		this.SetRoundBehaviourActivation(false);
		yield break;
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x00094990 File Offset: 0x00092D90
	private void UpdateOnlineMultiplayerNotifications()
	{
		if (this.m_iOnlineMultiplayerNotificationCoordinator != null)
		{
			FastList<User> users = ClientUserSystem.m_Users;
			if (users != null)
			{
				for (int i = 0; i < users.Count; i++)
				{
					User user = users._items[i];
					if (user != null && user.IsLocal && user.GameState == GameState.InLevel)
					{
						this.m_iOnlineMultiplayerNotificationCoordinator.InGamePlay();
						return;
					}
				}
			}
		}
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x00094A00 File Offset: 0x00092E00
	private IEnumerator RunLevelEnd()
	{
		this.m_campaignAudioManager.SetAudioState(CampaignAudioManager.AudioState.SummaryScreen);
		IEnumerator outro = this.RunLevelOutro();
		while (outro != null && outro.MoveNext())
		{
			yield return outro.Current;
		}
		yield break;
	}

	// Token: 0x06001FCF RID: 8143 RVA: 0x00094A1B File Offset: 0x00092E1B
	protected void SetRoundBehaviourActivation(bool _enabled)
	{
		if (this.m_inRound != _enabled)
		{
			if (_enabled)
			{
				this.RoundActivatedCallback();
			}
			else
			{
				this.RoundDeactivatedCallback();
			}
			this.m_inRound = _enabled;
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x00094A51 File Offset: 0x00092E51
	public bool InRound
	{
		get
		{
			return this.m_inRound;
		}
	}

	// Token: 0x06001FD1 RID: 8145 RVA: 0x00094A59 File Offset: 0x00092E59
	public LevelConfigBase GetLevelConfig()
	{
		return this.LevelConfig;
	}

	// Token: 0x06001FD2 RID: 8146 RVA: 0x00094A61 File Offset: 0x00092E61
	public GameConfig GetGameConfig()
	{
		return (!(this.m_flowControllerBase != null)) ? null : this.m_flowControllerBase.m_gameConfig;
	}

	// Token: 0x06001FD3 RID: 8147 RVA: 0x00094A85 File Offset: 0x00092E85
	protected virtual void OnUpdateInRound()
	{
	}

	// Token: 0x06001FD4 RID: 8148
	protected abstract IEnumerator RunLevelOutro();

	// Token: 0x06001FD5 RID: 8149 RVA: 0x00094A87 File Offset: 0x00092E87
	private bool HasFinished()
	{
		return this.m_isFinished;
	}

	// Token: 0x04001829 RID: 6185
	private FlowControllerBase m_flowControllerBase;

	// Token: 0x0400182A RID: 6186
	private IOnlineMultiplayerNotificationCoordinator m_iOnlineMultiplayerNotificationCoordinator;

	// Token: 0x0400182B RID: 6187
	private IEnumerator m_levelRoutine;

	// Token: 0x0400182C RID: 6188
	private IEnumerator m_outroRoutine;

	// Token: 0x0400182D RID: 6189
	protected CampaignAudioManager m_campaignAudioManager;

	// Token: 0x0400182E RID: 6190
	private bool m_isFinished;

	// Token: 0x0400182F RID: 6191
	private bool m_inRound;
}
