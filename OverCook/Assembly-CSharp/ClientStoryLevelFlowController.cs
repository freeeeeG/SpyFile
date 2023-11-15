using System;
using System.Collections;
using System.Diagnostics;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000A65 RID: 2661
public class ClientStoryLevelFlowController : ClientSynchroniserBase, IFlowController
{
	// Token: 0x06003487 RID: 13447 RVA: 0x000F6A80 File Offset: 0x000F4E80
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_flowController = (StoryLevelFlowController)synchronisedObject;
		this.m_flowController.m_onionKing.DialogueScript = this.m_levelConfig.OnionScript;
		this.m_flowController.m_onionKing.AutoStart = this.m_levelConfig.AutoStartDialogue;
		if (this.m_levelConfig.OnionReturnScript != null && this.m_levelConfig.OnionReturnScript.Length > 0)
		{
			this.m_flowController.m_onionKing.RegisterDialogueFinishedCallback(new GenericVoid<DialogueController.Dialogue>(this.OnOnionKingDialogueFinished));
		}
	}

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x06003488 RID: 13448 RVA: 0x000F6B18 File Offset: 0x000F4F18
	// (remove) Token: 0x06003489 RID: 13449 RVA: 0x000F6B50 File Offset: 0x000F4F50
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CallbackVoid RoundActivatedCallback = delegate()
	{
	};

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x0600348A RID: 13450 RVA: 0x000F6B88 File Offset: 0x000F4F88
	// (remove) Token: 0x0600348B RID: 13451 RVA: 0x000F6BC0 File Offset: 0x000F4FC0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CallbackVoid RoundDeactivatedCallback = delegate()
	{
	};

	// Token: 0x0600348C RID: 13452 RVA: 0x000F6BF8 File Offset: 0x000F4FF8
	private void Awake()
	{
		GameSession.GameLevelSettings levelSettings = GameUtils.GetGameSession().LevelSettings;
		if (levelSettings != null && levelSettings.SceneDirectoryVarientEntry != null)
		{
			this.m_levelConfig = (levelSettings.SceneDirectoryVarientEntry.LevelConfig as StoryLevelConfig);
		}
		this.m_campaignAudioManager = GameUtils.RequireManager<CampaignAudioManager>();
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x000F6C5A File Offset: 0x000F505A
	protected virtual void Start()
	{
		if (!LoadingScreenFlow.IsLoadingStartScreen())
		{
			GameUtils.LoadScene("InGameMenu", LoadSceneMode.Additive);
		}
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x000F6C74 File Offset: 0x000F5074
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		if (this.m_flowController != null && this.m_flowController.m_onionKing != null)
		{
			this.m_flowController.m_onionKing.UnregisterDialogueFinishedCallback(new GenericVoid<DialogueController.Dialogue>(this.OnOnionKingDialogueFinished));
		}
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x000F6CE4 File Offset: 0x000F50E4
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
					MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
					multiplayerController.StopSynchronisation();
					ClientMessenger.GameState(GameState.RanLevelOutro);
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
			base.StartCoroutine(this.WaitForLoading(delegate
			{
				ClientMessenger.GameState(GameState.RanLevelIntro);
			}));
		}
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x000F6D88 File Offset: 0x000F5188
	private IEnumerator WaitForLoading(CallbackVoid _callback)
	{
		while (LoadingScreenFlow.IsLoading)
		{
			yield return null;
		}
		if (this.m_levelConfig.AutoStartDialogue)
		{
			yield return new WaitForSeconds(2f);
		}
		_callback();
		yield break;
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x000F6DAC File Offset: 0x000F51AC
	private IEnumerator RunLevel()
	{
		int levelID = GameUtils.GetLevelID();
		if (levelID >= 0)
		{
			GameSession gameSession = GameUtils.GetGameSession();
			GameProgress.UnlockData[] array = new GameProgress.UnlockData[0];
			gameSession.Progress.RecordLevelProgress(levelID, 0, ref array);
			gameSession.SaveSession(null);
		}
		this.RoundActivatedCallback();
		this.m_campaignAudioManager.SetAudioState(CampaignAudioManager.AudioState.InLevel);
		while (!base.enabled || !this.HasFinished())
		{
			yield return null;
		}
		this.m_campaignAudioManager.SetMusic(null, false);
		this.RoundDeactivatedCallback();
		yield break;
	}

	// Token: 0x06003492 RID: 13458 RVA: 0x000F6DC7 File Offset: 0x000F51C7
	public bool HasFinished()
	{
		return this.m_isFinished;
	}

	// Token: 0x06003493 RID: 13459 RVA: 0x000F6DCF File Offset: 0x000F51CF
	public void OnOnionKingDialogueFinished(DialogueController.Dialogue _dialogue)
	{
		if (this.m_levelConfig.OnionReturnScript != null && this.m_levelConfig.OnionReturnScript.Length > 0)
		{
			this.m_flowController.m_onionKing.DialogueScript = this.m_levelConfig.OnionReturnScript;
		}
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x06003494 RID: 13460 RVA: 0x000F6E0F File Offset: 0x000F520F
	public bool InRound
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x000F6E12 File Offset: 0x000F5212
	public LevelConfigBase GetLevelConfig()
	{
		return this.m_levelConfig;
	}

	// Token: 0x06003496 RID: 13462 RVA: 0x000F6E1A File Offset: 0x000F521A
	public GameConfig GetGameConfig()
	{
		return null;
	}

	// Token: 0x04002A20 RID: 10784
	private StoryLevelFlowController m_flowController;

	// Token: 0x04002A21 RID: 10785
	private StoryLevelConfig m_levelConfig;

	// Token: 0x04002A22 RID: 10786
	protected CampaignAudioManager m_campaignAudioManager;

	// Token: 0x04002A23 RID: 10787
	private IEnumerator m_levelRoutine;

	// Token: 0x04002A24 RID: 10788
	private bool m_isFinished;
}
