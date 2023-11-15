using System;
using System.Collections;
using System.Diagnostics;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000678 RID: 1656
public abstract class ServerFlowControllerBase : ServerSynchroniserBase, IServerFlowController, IFlowController
{
	// Token: 0x06001FA6 RID: 8102 RVA: 0x00093501 File Offset: 0x00091901
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_flowControllerBase = (FlowControllerBase)synchronisedObject;
	}

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x06001FA7 RID: 8103 RVA: 0x00093518 File Offset: 0x00091918
	// (remove) Token: 0x06001FA8 RID: 8104 RVA: 0x00093550 File Offset: 0x00091950
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CallbackVoid RoundActivatedCallback = delegate()
	{
	};

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x06001FA9 RID: 8105 RVA: 0x00093588 File Offset: 0x00091988
	// (remove) Token: 0x06001FAA RID: 8106 RVA: 0x000935C0 File Offset: 0x000919C0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CallbackVoid RoundDeactivatedCallback = delegate()
	{
	};

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06001FAB RID: 8107 RVA: 0x000935F6 File Offset: 0x000919F6
	// (set) Token: 0x06001FAC RID: 8108 RVA: 0x000935FE File Offset: 0x000919FE
	protected LevelConfigBase LevelConfig { get; set; }

	// Token: 0x06001FAD RID: 8109 RVA: 0x00093608 File Offset: 0x00091A08
	protected virtual void Awake()
	{
		GameSession.GameLevelSettings levelSettings = GameUtils.GetGameSession().LevelSettings;
		if (levelSettings != null && levelSettings.SceneDirectoryVarientEntry != null)
		{
			this.LevelConfig = levelSettings.SceneDirectoryVarientEntry.LevelConfig;
		}
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x00093642 File Offset: 0x00091A42
	protected virtual void Start()
	{
	}

	// Token: 0x06001FAF RID: 8111 RVA: 0x00093644 File Offset: 0x00091A44
	public void StartFlow()
	{
		this.ChangeGameState(GameState.RunLevelIntro);
	}

	// Token: 0x06001FB0 RID: 8112 RVA: 0x00093650 File Offset: 0x00091A50
	public override void UpdateSynchronising()
	{
		GameState state = this.m_State;
		if (state != GameState.RunLevelIntro)
		{
			if (state != GameState.InLevel)
			{
				if (state == GameState.RunLevelOutro)
				{
					if (this.AreAllUsersInGameState(GameState.RanLevelOutro))
					{
						this.m_State = GameState.RanLevelOutro;
						MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
						multiplayerController.StopSynchronisation();
						GameState setAtLoadingBegin = GameState.NotSet;
						GameState waitForHide = GameState.NotSet;
						bool bUseLoadingScreen = true;
						string nextScene = this.GetNextScene(out setAtLoadingBegin, out waitForHide, out bUseLoadingScreen);
						if (!string.IsNullOrEmpty(nextScene))
						{
							GameSession gameSession = GameUtils.GetGameSession();
							gameSession.FillShownMetaDialogStatus();
							ServerMessenger.GameProgressData(gameSession.Progress.SaveData, gameSession.m_shownMetaDialogs);
							ServerMessenger.LoadLevel(nextScene, setAtLoadingBegin, bUseLoadingScreen, waitForHide);
						}
					}
				}
			}
			else if (this.m_levelRoutine == null || !this.m_levelRoutine.MoveNext())
			{
				this.ChangeGameState(GameState.RunLevelOutro);
			}
		}
		else if (this.AreAllUsersInGameState(GameState.RanLevelIntro))
		{
			this.m_levelRoutine = this.RunRound();
			this.ChangeGameState(GameState.InLevel);
		}
	}

	// Token: 0x06001FB1 RID: 8113 RVA: 0x0009374D File Offset: 0x00091B4D
	private void ChangeGameState(GameState state)
	{
		UserSystemUtils.ChangeGameState(state, null);
		this.m_State = state;
	}

	// Token: 0x06001FB2 RID: 8114 RVA: 0x0009375D File Offset: 0x00091B5D
	private bool AreAllUsersInGameState(GameState state)
	{
		return UserSystemUtils.AreAllUsersInGameState(ServerUserSystem.m_Users, state);
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x0009376C File Offset: 0x00091B6C
	private IEnumerator RunRound()
	{
		this.SetRoundBehaviourActivation(true);
		for (;;)
		{
			if (base.enabled)
			{
				this.OnUpdateInRound();
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

	// Token: 0x06001FB4 RID: 8116 RVA: 0x00093787 File Offset: 0x00091B87
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

	// Token: 0x06001FB5 RID: 8117 RVA: 0x000937BD File Offset: 0x00091BBD
	public virtual void SkipToEnd()
	{
		this.m_skipToEnd = true;
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x000937C6 File Offset: 0x00091BC6
	public bool InRound
	{
		get
		{
			return this.m_inRound;
		}
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x000937CE File Offset: 0x00091BCE
	public LevelConfigBase GetLevelConfig()
	{
		return this.LevelConfig;
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x000937D6 File Offset: 0x00091BD6
	public GameConfig GetGameConfig()
	{
		return (!(this.m_flowControllerBase != null)) ? null : this.m_flowControllerBase.m_gameConfig;
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x000937FA File Offset: 0x00091BFA
	protected virtual bool HasFinished()
	{
		return this.m_skipToEnd;
	}

	// Token: 0x06001FBA RID: 8122 RVA: 0x00093802 File Offset: 0x00091C02
	protected virtual void OnUpdateInRound()
	{
	}

	// Token: 0x06001FBB RID: 8123
	protected abstract string GetNextScene(out GameState o_loadState, out GameState o_loadEndState, out bool o_useLoadingScreen);

	// Token: 0x0400181F RID: 6175
	private FlowControllerBase m_flowControllerBase;

	// Token: 0x04001820 RID: 6176
	private GameState m_State;

	// Token: 0x04001821 RID: 6177
	private IEnumerator m_levelRoutine;

	// Token: 0x04001822 RID: 6178
	private bool m_inRound;

	// Token: 0x04001823 RID: 6179
	private bool m_skipToEnd;
}
