using System;
using System.Collections;
using System.Diagnostics;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A64 RID: 2660
public class ServerStoryLevelFlowController : ServerSynchroniserBase, IServerFlowController, IFlowController
{
	// Token: 0x06003474 RID: 13428 RVA: 0x000F66DD File Offset: 0x000F4ADD
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_flowController = (StoryLevelFlowController)synchronisedObject;
	}

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06003475 RID: 13429 RVA: 0x000F66F4 File Offset: 0x000F4AF4
	// (remove) Token: 0x06003476 RID: 13430 RVA: 0x000F672C File Offset: 0x000F4B2C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CallbackVoid RoundActivatedCallback = delegate()
	{
	};

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x06003477 RID: 13431 RVA: 0x000F6764 File Offset: 0x000F4B64
	// (remove) Token: 0x06003478 RID: 13432 RVA: 0x000F679C File Offset: 0x000F4B9C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CallbackVoid RoundDeactivatedCallback = delegate()
	{
	};

	// Token: 0x06003479 RID: 13433 RVA: 0x000F67D4 File Offset: 0x000F4BD4
	private void Awake()
	{
		GameSession.GameLevelSettings levelSettings = GameUtils.GetGameSession().LevelSettings;
		if (levelSettings != null && levelSettings.SceneDirectoryVarientEntry != null)
		{
			this.m_levelConfig = (levelSettings.SceneDirectoryVarientEntry.LevelConfig as StoryLevelConfig);
		}
	}

	// Token: 0x0600347A RID: 13434 RVA: 0x000F6813 File Offset: 0x000F4C13
	public void StartFlow()
	{
		this.ChangeGameState(GameState.RunLevelIntro);
	}

	// Token: 0x0600347B RID: 13435 RVA: 0x000F6820 File Offset: 0x000F4C20
	private void Update()
	{
		GameState state = this.m_state;
		if (state != GameState.RunLevelIntro)
		{
			if (state != GameState.InLevel)
			{
				if (state == GameState.RunLevelOutro)
				{
					if (this.AreAllUsersInGameState(GameState.RanLevelOutro))
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
			else if (this.m_levelRoutine == null || !this.m_levelRoutine.MoveNext())
			{
				this.ChangeGameState(GameState.RunLevelOutro);
			}
		}
		else if (this.AreAllUsersInGameState(GameState.RanLevelIntro))
		{
			this.m_levelRoutine = this.RunLevel();
			this.ChangeGameState(GameState.InLevel);
		}
	}

	// Token: 0x0600347C RID: 13436 RVA: 0x000F68FC File Offset: 0x000F4CFC
	private IEnumerator RunLevel()
	{
		this.RoundActivatedCallback();
		while (!base.enabled || !this.HasFinished())
		{
			yield return null;
		}
		this.RoundDeactivatedCallback();
		yield break;
	}

	// Token: 0x0600347D RID: 13437 RVA: 0x000F6917 File Offset: 0x000F4D17
	private bool HasFinished()
	{
		return this.m_isFinished;
	}

	// Token: 0x0600347E RID: 13438 RVA: 0x000F691F File Offset: 0x000F4D1F
	private void ChangeGameState(GameState state)
	{
		UserSystemUtils.ChangeGameState(state, null);
		this.m_state = state;
	}

	// Token: 0x0600347F RID: 13439 RVA: 0x000F692F File Offset: 0x000F4D2F
	private bool AreAllUsersInGameState(GameState state)
	{
		return UserSystemUtils.AreAllUsersInGameState(ServerUserSystem.m_Users, state);
	}

	// Token: 0x06003480 RID: 13440 RVA: 0x000F693C File Offset: 0x000F4D3C
	public void SkipToEnd()
	{
		this.m_isFinished = true;
	}

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x06003481 RID: 13441 RVA: 0x000F6945 File Offset: 0x000F4D45
	public bool InRound
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x000F6948 File Offset: 0x000F4D48
	public LevelConfigBase GetLevelConfig()
	{
		return this.m_levelConfig;
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x000F6950 File Offset: 0x000F4D50
	public GameConfig GetGameConfig()
	{
		return null;
	}

	// Token: 0x04002A17 RID: 10775
	private StoryLevelFlowController m_flowController;

	// Token: 0x04002A18 RID: 10776
	private StoryLevelConfig m_levelConfig;

	// Token: 0x04002A19 RID: 10777
	private GameState m_state;

	// Token: 0x04002A1A RID: 10778
	private IEnumerator m_levelRoutine;

	// Token: 0x04002A1B RID: 10779
	private bool m_isFinished;
}
