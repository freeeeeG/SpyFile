using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000094 RID: 148
public class MainGame : MonoBehaviour
{
	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000305 RID: 773 RVA: 0x0000BE85 File Offset: 0x0000A085
	public IngameData IngameData
	{
		get
		{
			return this.ingameData;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000306 RID: 774 RVA: 0x0000BE8D File Offset: 0x0000A08D
	public static MainGame Instance
	{
		get
		{
			if (MainGame.instance == null)
			{
				MainGame.instance = Object.FindObjectOfType<MainGame>();
			}
			return MainGame.instance;
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x0000BEAC File Offset: 0x0000A0AC
	private void Awake()
	{
		EnvSceneCollectionData envSceneCollectionData = Resources.Load<EnvSceneCollectionData>("EnvSceneCollectionData");
		IntermediateData intermediateData = GameDataManager.instance.IntermediateData;
		bool flag = intermediateData.missionType == eMissionType.BOSS;
		if (envSceneCollectionData != null)
		{
			string text;
			if (flag)
			{
				this.sceneEntry = envSceneCollectionData.GetSceneEntryByName("EnvScene_099_Boss");
				text = this.sceneEntry.name;
			}
			else if (intermediateData.stageName == "")
			{
				this.sceneEntry = envSceneCollectionData.GetRandomScene(intermediateData.worldType, flag, intermediateData.difficulty);
				if (this.sceneEntry == null)
				{
					Debug.LogError(string.Format("沒有可以使用的場景, 需要檢查場景設定!! ({0}, {1}, {2})", intermediateData.worldType, flag, intermediateData.difficulty));
					return;
				}
				text = this.sceneEntry.name;
				intermediateData.stageName = text;
			}
			else
			{
				this.sceneEntry = envSceneCollectionData.GetSceneEntryByName(intermediateData.stageName);
				text = intermediateData.stageName;
			}
			if (!GameDataManager.instance.Playerdata.IsFinishedTutorial(eTutorialType.BUILD_TETRIS))
			{
				text = "EnvScene_012";
				this.sceneEntry = envSceneCollectionData.GetSceneEntryByName(text);
			}
			SceneManager.LoadScene(text, LoadSceneMode.Additive);
			if (GameDataManager.instance.GameplayData.list_LoadoutTowerData.Count == 0)
			{
				EventMgr.SendEvent<TowerIngameData>(eGameEvents.RequestAddTowerCard, new TowerIngameData(eItemType._1000_BASIC_TOWER, 1));
			}
			EventMgr.SendEvent<List<TowerIngameData>, int>(eGameEvents.UI_ForceUpdateAllTowerCard, GameDataManager.instance.GameplayData.list_LoadoutTowerData, this.ingameData.Coin);
			return;
		}
		Debug.LogError("無法從 Resources 中讀取EnvSceneCollectionData");
	}

	// Token: 0x06000308 RID: 776 RVA: 0x0000C02C File Offset: 0x0000A22C
	private void Start()
	{
		int num = 50;
		if (GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.INITIAL_COIN_INCREASE))
		{
			num = Mathf.RoundToInt((float)num * 1.5f);
		}
		int curHP = GameDataManager.instance.GameplayData.CurHP;
		this.ingameData = new IngameData(num, curHP);
		this.stageDataReader = Singleton<StageDataReader>.Instance;
		if (this.sceneEntry.presetStageData == null)
		{
			this.stageDataReader.LoadStageData(null);
		}
		else
		{
			this.stageDataReader.LoadStageData(this.sceneEntry.presetStageData);
		}
		base.StartCoroutine(this.CR_GameProc());
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0000C0C9 File Offset: 0x0000A2C9
	private void OnDestroy()
	{
		this.ingameData.ClearEvents();
		this.ingameData = null;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0000C0DD File Offset: 0x0000A2DD
	private void Update()
	{
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0000C0DF File Offset: 0x0000A2DF
	private void PauseGame()
	{
		EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.PAUSE_GAME);
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0000C0EE File Offset: 0x0000A2EE
	private IEnumerator CR_Debug_BackToMapScene()
	{
		EventMgr.SendEvent(eGameEvents.UI_TriggerTransition_Show);
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("MapScene");
		yield break;
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0000C0F6 File Offset: 0x0000A2F6
	private IEnumerator CR_GameProc()
	{
		Debug.Log("開始遊戲流程");
		bool isRandomPlacementDone = false;
		int randomPlacementRetryCount = 0;
		while (!isRandomPlacementDone)
		{
			Debug.Log("送出隨機放置物件事件");
			EventMgr.SendEvent(eGameEvents.RequestStartRandomPlacement);
			yield return new WaitForSeconds(0.1f);
			AstarPath.active.Scan(null);
			if (!Singleton<MapManager>.Instance.IsPathBlocked())
			{
				isRandomPlacementDone = true;
				Debug.Log("隨機放置完成");
			}
			else
			{
				int num = randomPlacementRetryCount;
				randomPlacementRetryCount = num + 1;
				Debug.Log(string.Format("出現路徑阻擋, 重新隨機放置(重試:{0})", randomPlacementRetryCount));
			}
		}
		EventMgr.SendEvent(eGameEvents.OnGraphUpdated);
		EventMgr.SendEvent(eGameEvents.RequestShuffleDeck);
		EventMgr.SendEvent<List<TowerIngameData>, int>(eGameEvents.UI_ForceUpdateAllTowerCard, GameDataManager.instance.GameplayData.list_LoadoutTowerData, this.ingameData.Coin);
		yield return new WaitForSeconds(1f);
		EventMgr.SendEvent(eGameEvents.UI_TriggerTransition_Hide);
		EventMgr.SendEvent<int, float>(eGameEvents.UI_ShowStageAnnounce, 1, 2f);
		MainGame.PlayWorldBGM(0f, 1f, 1f);
		EventMgr.SendEvent(eGameEvents.OnGameInitReady);
		if (GameDataManager.instance.IntermediateData.missionType == eMissionType.BOSS)
		{
			AStageScript astageScript = Object.FindAnyObjectByType<AStageScript>();
			yield return base.StartCoroutine(astageScript.CR_Intro());
		}
		this.round = 0;
		int totalRound = this.stageDataReader.GetTotalWaveCount();
		bool isWaveFinished = false;
		float roundCountdown = 0f;
		bool isBossDeadInBossStage = false;
		Action <>9__3;
		while (this.stageDataReader.HasNextWave() && !isBossDeadInBossStage)
		{
			MainGame.<>c__DisplayClass18_1 CS$<>8__locals2 = new MainGame.<>c__DisplayClass18_1();
			isWaveFinished = false;
			yield return base.StartCoroutine(this.BeforeWaveStartProcess());
			this.round++;
			EventMgr.SendEvent<int, int>(eGameEvents.OnRoundStart, this.round, totalRound);
			EventMgr.SendEvent<bool, bool>(eGameEvents.UI_ToggleRoundTimerUI, true, this.round != 1);
			CS$<>8__locals2.isTutorialFinished = false;
			EventMgr.SendEvent<eTutorialType, float, Action>(eGameEvents.RequestTutorial, eTutorialType.BUILD_TETRIS, 0.5f, delegate()
			{
				CS$<>8__locals2.isTutorialFinished = true;
			});
			while (!CS$<>8__locals2.isTutorialFinished)
			{
				yield return null;
			}
			if (GameDataManager.instance.IntermediateData.isCorrupted)
			{
				CS$<>8__locals2.isTutorialFinished = false;
				EventMgr.SendEvent<eTutorialType, float, Action>(eGameEvents.RequestTutorial, eTutorialType.CORRUPTED_STAGE, 0.5f, delegate()
				{
					CS$<>8__locals2.isTutorialFinished = true;
				});
				while (!CS$<>8__locals2.isTutorialFinished)
				{
					yield return null;
				}
			}
			CS$<>8__locals2.isQueuedTutorialFinished = false;
			EventMgr.SendEvent<Action>(eGameEvents.RequestStartQueuedTutorial, delegate()
			{
				CS$<>8__locals2.isQueuedTutorialFinished = true;
			});
			while (!CS$<>8__locals2.isQueuedTutorialFinished)
			{
				yield return null;
			}
			if (this.round == 1)
			{
				roundCountdown = 1E+11f;
			}
			else
			{
				float num2 = GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.TIME_MANAGEMENT) ? 15f : 0f;
				roundCountdown = this.ROUND_PREPARE_TIME + num2;
			}
			SingleEventCapturer sc_RequestStartNextWave = new SingleEventCapturer(eGameEvents.RequestStartNextWave, null);
			while (!sc_RequestStartNextWave.IsEventReceived && roundCountdown > 0f)
			{
				roundCountdown -= Time.deltaTime;
				EventMgr.SendEvent<float, float>(eGameEvents.OnUpdateRoundTimer, roundCountdown, 1f - roundCountdown / this.ROUND_PREPARE_TIME);
				yield return null;
			}
			EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleNextWaveMonsterUI, false);
			StageDataReader stageDataReader = this.stageDataReader;
			Action waveFinishCallback;
			if ((waveFinishCallback = <>9__3) == null)
			{
				waveFinishCallback = (<>9__3 = delegate()
				{
					isWaveFinished = true;
				});
			}
			stageDataReader.StartNextWave(waveFinishCallback);
			MainGame.PlayWorldBGM(Singleton<SoundManager>.Instance.GetCurrentBGMTime(), 1.5f, 1.1f);
			EventMgr.SendEvent<bool, bool>(eGameEvents.UI_ToggleRoundTimerUI, false, this.round != 1);
			if (roundCountdown >= 0f)
			{
				EventMgr.SendEvent<float>(eGameEvents.RoundTimeFastForwardToNight, 1f - roundCountdown / this.ROUND_PREPARE_TIME);
			}
			EventMgr.SendEvent(eGameEvents.OnBattleStart);
			EventMgr.SendEvent<bool>(eGameEvents.RequestChangeBattleState, true);
			while (!isWaveFinished && this.ingameData.IsPlayerAlive())
			{
				yield return null;
			}
			if (!this.ingameData.IsPlayerAlive())
			{
				break;
			}
			yield return new WaitForSeconds(0.5f);
			EventMgr.SendEvent<bool>(eGameEvents.RequestChangeBattleState, false);
			EventMgr.SendEvent(eGameEvents.OnBattleEnd);
			EventMgr.SendEvent(eGameEvents.OnRoundEnd);
			MainGame.PlayWorldBGM(Singleton<SoundManager>.Instance.GetCurrentBGMTime(), 1.5f, 1f);
			if (GameDataManager.instance.IntermediateData.missionType == eMissionType.BOSS)
			{
				ABossStageScript abossStageScript = Object.FindAnyObjectByType<ABossStageScript>();
				isBossDeadInBossStage = abossStageScript.IsBossDead();
			}
			if (this.stageDataReader.HasNextWave() && !isBossDeadInBossStage)
			{
				this.ingameData.GetHandCardCount();
				EventMgr.SendEvent<int>(eGameEvents.RequestRemoveExcessHandCard, 10);
				int arg = 10 * (this.round + 1);
				EventMgr.SendEvent<int>(eGameEvents.UI_WaveClearUI_Show, arg);
				EventMgr.SendEvent<int>(eGameEvents.RequestAddCoin, arg);
				yield return new WaitForSeconds(2f);
			}
			CS$<>8__locals2 = null;
			sc_RequestStartNextWave = null;
		}
		SoundManager.StopMusic();
		if (this.ingameData.HP > 0)
		{
			yield return new WaitForSeconds(1f);
			EventMgr.SendEvent(eGameEvents.CancelPlacement);
			EventMgr.SendEvent<int>(eGameEvents.RequestOverrideMapHP, this.ingameData.HP);
			GameDataManager.instance.IntermediateData.mapNodeData.SetCleared();
			GameDataManager.instance.SaveData();
			EventMgr.SendEvent(eGameEvents.OnPlayerVictory);
			if (GameDataManager.instance.IntermediateData.stageName == "EnvScene_012")
			{
				EventMgr.SendEvent(eGameEvents.RequestSetTutorialStageCompleted);
			}
			if (GameDataManager.instance.IntermediateData.missionType == eMissionType.BOSS)
			{
				GameDataManager.instance.GameplayData.SetGameEnded();
				GameDataManager.instance.SaveData();
				yield return new WaitForSeconds(1.5f);
				UI_ThankYouForPlayDemo_Popup window = APopupWindow.CreateWindow<UI_ThankYouForPlayDemo_Popup>(APopupWindow.ePopupWindowLayer.TOP, null, false);
				while (!window.IsWindowFinished)
				{
					yield return null;
				}
				window = null;
			}
			else
			{
				APopupWindow.CreateWindow<UI_Victory>(APopupWindow.ePopupWindowLayer.TOP, null, false);
				SingleEventCapturer sc_RequestStartNextWave = new SingleEventCapturer(eGameEvents.UI_VictoryUICompleted, null);
				while (!sc_RequestStartNextWave.IsEventReceived)
				{
					yield return null;
				}
				EventMgr.SendEvent(eGameEvents.UI_TriggerTransition_Show);
				yield return new WaitForSecondsRealtime(1f);
				SceneManager.LoadScene("MapScene");
				sc_RequestStartNextWave = null;
			}
		}
		else
		{
			GameDataManager.instance.GameplayData.SetGameEnded();
			GameDataManager.instance.SaveData();
			yield return base.StartCoroutine(this.CR_SlowMotion());
			EventMgr.SendEvent<float>(eGameEvents.RequestModifySystemGameSpeed, 0f);
			EventMgr.SendEvent(eGameEvents.OnPlayerDefeat);
			UI_Defeat_Popup window2 = APopupWindow.CreateWindow<UI_Defeat_Popup>(APopupWindow.ePopupWindowLayer.TOP, null, false);
			while (!window2.isButtonPressed)
			{
				yield return null;
			}
			EventMgr.SendEvent(eGameEvents.UI_TriggerTransition_Show);
			yield return new WaitForSecondsRealtime(1f);
			GameDataManager.instance.SaveData();
			SceneManager.LoadScene("CoinPage");
			window2 = null;
		}
		yield break;
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0000C105 File Offset: 0x0000A305
	private IEnumerator CR_SlowMotion()
	{
		float time = 0f;
		float duration = 0.1f;
		while (time <= duration)
		{
			float num = time / duration;
			EventMgr.SendEvent<float>(eGameEvents.RequestModifySystemGameSpeed, 1f - 0.9f * num);
			yield return null;
			time += Time.deltaTime;
		}
		EventMgr.SendEvent<float>(eGameEvents.RequestModifySystemGameSpeed, 0.1f);
		yield break;
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0000C110 File Offset: 0x0000A310
	private static void PlayWorldBGM(float offset, float fadetime = 1f, float pitch = 1f)
	{
		int num = GameDataManager.instance.IntermediateData.worldType.ToIndex();
		if (GameDataManager.instance.IntermediateData.missionType == eMissionType.BOSS)
		{
			SoundManager.PlayMusic("Main", "BGM_Boss", true, fadetime, offset, pitch);
			return;
		}
		SoundManager.PlayMusic("Main", string.Format("BGM_MainGame_World{0}", num), true, fadetime, offset, pitch);
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0000C17A File Offset: 0x0000A37A
	private IEnumerator BeforeWaveStartProcess()
	{
		List<int> nextWaveSpawnIndexs = Singleton<StageDataReader>.Instance.GetNextWaveSpawnIndexs();
		EventMgr.SendEvent<int, List<int>>(eGameEvents.SetNextWaveSpawnIndex, this.round, nextWaveSpawnIndexs);
		float nextWaveDifficulty = Singleton<StageDataReader>.Instance.GetNextWaveDifficulty();
		Debug.Log(string.Format("目前難度: {0: 0.00}", nextWaveDifficulty));
		EventMgr.SendEvent(eGameEvents.RequestResetDrawCardCost);
		WaveInfoData nextWaveMonsterInfo = Singleton<StageDataReader>.Instance.GetNextWaveMonsterInfo();
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleNextWaveMonsterUI, true);
		EventMgr.SendEvent<WaveInfoData>(eGameEvents.UI_UpdateNextWaveMonster, nextWaveMonsterInfo);
		int drawCardCount = GameDataManager.instance.GameplayData.DrawCardPerRound;
		if (this.round == 0 && GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.INITIAL_HANDCARD_INCREASE))
		{
			drawCardCount += 2;
		}
		int num;
		for (int i = 0; i < drawCardCount; i = num + 1)
		{
			EventMgr.SendEvent<int, bool>(eGameEvents.RequestDrawCard, 1, false);
			yield return new WaitForSeconds(0.15f);
			num = i;
		}
		if (GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.OVERLOAD))
		{
			EventMgr.SendEvent<eItemType>(eGameEvents.RequestAddCardToHand, eItemType._3008_OVERCHARGE);
		}
		yield break;
	}

	// Token: 0x04000358 RID: 856
	[SerializeField]
	private IngameData ingameData;

	// Token: 0x04000359 RID: 857
	[SerializeField]
	[Header("直接從Maingame場景執行時的worldType")]
	private eWorldType debugWorldType = eWorldType.WORLD_1_FOREST;

	// Token: 0x0400035A RID: 858
	private static MainGame instance;

	// Token: 0x0400035B RID: 859
	private StageDataReader stageDataReader;

	// Token: 0x0400035C RID: 860
	private int round;

	// Token: 0x0400035D RID: 861
	private readonly float ROUND_PREPARE_TIME = 45f;

	// Token: 0x0400035E RID: 862
	private EnvSceneCollectionData.EnvSceneDataEntry sceneEntry;

	// Token: 0x0400035F RID: 863
	private int debug_TowerOverchargeTestIndex;
}
