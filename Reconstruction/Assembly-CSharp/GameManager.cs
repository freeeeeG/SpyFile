using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class GameManager : Singleton<GameManager>
{
	// Token: 0x1700032D RID: 813
	// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00017557 File Offset: 0x00015757
	// (set) Token: 0x060008A5 RID: 2213 RVA: 0x0001755F File Offset: 0x0001575F
	public BattleOperationState OperationState
	{
		get
		{
			return this.operationState;
		}
		set
		{
			this.operationState = value;
		}
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x00017568 File Offset: 0x00015768
	public void Initinal()
	{
		Singleton<LevelManager>.Instance.LevelEnd = false;
		Singleton<LevelManager>.Instance.LevelWin = false;
		ConstructHelper.Initialize();
		TechnologyFactory.SetBattleTechs();
		GameRes.Initialize(this.m_MainUI, this.m_FuncUI, this.m_WaveSystem, this.m_BluePrintShopUI);
		this.m_MainUI.Initialize();
		this.m_WaveSystem.Initialize();
		this.m_CamControl.Initialize(this.m_MainUI);
		this.m_BoardSystem.Initialize();
		this.m_TechnologySystem.Initialize();
		this.m_ChallengeSystem.Initialize();
		this.m_FuncUI.Initialize();
		this.m_BluePrintShopUI.Initialize();
		this.m_ShapeSelectUI.Initialize();
		this.m_GameEndUI.Initialize();
		this.m_TechSelectUI.Initialize();
		this.m_SettingPanel.Initialize();
		this.m_RangeContainer.Initialize();
		this.m_HealthWarning.Initialize();
		this.inputManager = Singleton<InputManager>.Instance;
		this.buildingState = new BuildingState(this, this.m_BoardSystem, this.m_FuncUI, this.m_ShapeSelectUI, this.m_TechSelectUI);
		this.waveState = new WaveState(this, this.m_WaveSystem, this.m_BoardSystem);
		this.pickingState = new PickingState(this, this.m_FuncUI);
		this.endState = new EndState(this);
		this.wonState = new WonState(this);
		this.StateDIC.Add(this.buildingState.StateName, this.buildingState);
		this.StateDIC.Add(this.waveState.StateName, this.waveState);
		this.StateDIC.Add(this.pickingState.StateName, this.pickingState);
		this.StateDIC.Add(this.endState.StateName, this.endState);
		this.StateDIC.Add(this.wonState.StateName, this.wonState);
		this.m_MainUI.Show();
		this.m_BoardSystem.SetTutorialPoss(false);
		Singleton<GuideGirlSystem>.Instance.Initialize();
		if (Singleton<LevelManager>.Instance.LastGameSave.HasLastGame)
		{
			this.LoadGame();
			return;
		}
		this.StartNewGame();
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00017790 File Offset: 0x00015990
	private void LoadGame()
	{
		GameRes.LoadSaveRes();
		Singleton<StaticData>.Instance.ContentFactory.LoadRareList();
		TechnologyFactory.SetRecipeTechs();
		RuleFactory.LoadSaveRules();
		RuleFactory.BeforeRules();
		RuleFactory.LoadRules();
		this.m_MainUI.SetRules();
		this.m_WaveSystem.LoadSaveWave();
		this.m_BoardSystem.LoadSaveGame();
		this.m_TechnologySystem.LoadSaveGame();
		this.m_BluePrintShopUI.LoadSaveGame();
		this.m_ShapeSelectUI.LoadSaveGame();
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Challenge)
		{
			this.m_ChallengeSystem.LoadSaveGame();
			if (!Singleton<LevelManager>.Instance.LastGameSave.ChallengeChoicePicked)
			{
				this.ShowChoices(true, false, true);
			}
		}
		else
		{
			this.m_TechSelectUI.LoadSaveGame();
		}
		this.ContinueWave();
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x00017854 File Offset: 0x00015A54
	private void StartNewGame()
	{
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Challenge)
		{
			this.m_BoardSystem.FirstGameSet();
			this.m_BluePrintShopUI.SetShopBtnActive(false);
			if (Singleton<LevelManager>.Instance.CurrentLevel.SaveSequences.Count <= 0)
			{
				this.m_WaveSystem.LevelInitialize();
			}
			else
			{
				this.m_WaveSystem.LoadChallengeWave();
			}
		}
		else
		{
			RuleFactory.BeforeRules();
			this.m_BoardSystem.FirstGameSet();
			this.m_BluePrintShopUI.SetShopBtnActive(true);
			Singleton<StaticData>.Instance.ContentFactory.SetRareLists();
			TechnologyFactory.SetRecipeTechs();
			RuleFactory.InitRules();
			RuleFactory.LoadRules();
			this.RefreshShop(0);
			Singleton<GuideGirlSystem>.Instance.PrepareTutorial();
			this.m_WaveSystem.LevelInitialize();
		}
		this.PrepareNextWave();
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x00017918 File Offset: 0x00015B18
	public void Release()
	{
		this.m_BoardSystem.Release();
		this.m_WaveSystem.Release();
		this.m_CamControl.Release();
		this.m_MainUI.Release();
		this.m_FuncUI.Release();
		this.m_BluePrintShopUI.Release();
		this.m_ShapeSelectUI.Release();
		this.m_GameEndUI.Release();
		this.m_TechSelectUI.Release();
		GameRes.GameSpeed = 1;
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x00017990 File Offset: 0x00015B90
	public void GameUpdate()
	{
		this.m_CamControl.GameUpdate();
		this.m_BoardSystem.GameUpdate();
		this.m_WaveSystem.GameUpdate();
		this.enemies.GameUpdate();
		Physics2D.SyncTransforms();
		this.elementTurrets.GameUpdate();
		this.refactorTurrets.GameUpdate();
		this.Buildings.GameUpdate();
		this.nonEnemies.GameUpdate();
		this.OperationState.StateUpdate();
		this.KeyboardControl();
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x00017A0C File Offset: 0x00015C0C
	private void KeyboardControl()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.PauseGame();
		}
		if (StaticData.LockKeyboard)
		{
			return;
		}
		if (this.inputManager.GetKeyDown(KeyBindingActions.ChangeSpeed))
		{
			GameRes.GameSpeed++;
			return;
		}
		if (this.inputManager.GetKeyDown(KeyBindingActions.Build))
		{
			this.m_FuncUI.DrawBtnClick();
			return;
		}
		if (this.inputManager.GetKeyDown(KeyBindingActions.Refresh))
		{
			this.RefreshShop(GameRes.RefreshShopCost);
			return;
		}
		if (this.inputManager.GetKeyDown(KeyBindingActions.OpenShop))
		{
			this.m_BluePrintShopUI.ShopBtnClick();
			return;
		}
		if (this.inputManager.GetKeyDown(KeyBindingActions.NextWave))
		{
			this.m_FuncUI.NextWaveBtnClick();
		}
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x00017AB2 File Offset: 0x00015CB2
	public void StartNewWave()
	{
		if (this.OperationState.StateName == StateName.BuildingState)
		{
			this.m_FuncUI.Hide();
			this.TransitionToState(StateName.WaveState);
			Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.NextWaveBtnClick);
			this.m_TechnologySystem.OnTurnEnd();
		}
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x00017AEC File Offset: 0x00015CEC
	public void PrepareNextWave()
	{
		if (GameRes.Life <= 0)
		{
			this.TransitionToState(StateName.LoseState);
			return;
		}
		if (GameRes.CurrentWave >= Singleton<LevelManager>.Instance.CurrentLevel.Wave)
		{
			this.TransitionToState(StateName.WonState);
			this.GameEnd(true);
			return;
		}
		GameRes.PrepareNextWave();
		this.m_WaveSystem.PrepareNextWave();
		this.m_MainUI.PrepareNextWave(this.m_WaveSystem.RunningSequence, this.m_WaveSystem.NextBoss, this.m_WaveSystem.NextBossWave);
		Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.NextWaveStart);
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType != ModeType.Challenge)
		{
			if (Singleton<LevelManager>.Instance.CurrentLevel.ModeID > 1 && GameRes.CurrentWave <= 99 && (GameRes.CurrentWave + 4) % 10 == 0)
			{
				this.ShowTechSelect(true, false);
			}
		}
		else
		{
			this.ShowChoices(true, false, true);
		}
		this.m_TechnologySystem.OnTurnStart();
		this.TransitionToState(StateName.BuildingState);
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x00017BD3 File Offset: 0x00015DD3
	public void ContinueWave()
	{
		this.TransitionToState(StateName.BuildingState);
		this.m_WaveSystem.PrepareNextWave();
		this.m_MainUI.PrepareNextWave(this.m_WaveSystem.RunningSequence, this.m_WaveSystem.NextBoss, this.m_WaveSystem.NextBossWave);
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x00017C13 File Offset: 0x00015E13
	public void GameEnd(bool win)
	{
		this.m_GameEndUI.Show();
		this.m_GameEndUI.SetGameResult(win);
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x00017C2C File Offset: 0x00015E2C
	public void TransitionToState(StateName stateName)
	{
		BattleOperationState newState = this.StateDIC[stateName];
		if (this.OperationState == null)
		{
			this.OperationState = newState;
			base.StartCoroutine(this.OperationState.EnterState());
			return;
		}
		base.StartCoroutine(this.OperationState.ExitState(newState));
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x00017C7B File Offset: 0x00015E7B
	public void RestartGame()
	{
		if (Singleton<Game>.Instance.OnTransition)
		{
			return;
		}
		Singleton<LevelManager>.Instance.StartNewGame(Singleton<LevelManager>.Instance.CurrentLevel.ModeID);
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x00017CA3 File Offset: 0x00015EA3
	public void ReturnToMenu()
	{
		if (Singleton<Game>.Instance.OnTransition)
		{
			return;
		}
		Singleton<LevelManager>.Instance.SaveAll();
		Singleton<Game>.Instance.LoadScene(0);
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x00017CC7 File Offset: 0x00015EC7
	public void QuitGame()
	{
		if (Singleton<Game>.Instance.OnTransition)
		{
			return;
		}
		Singleton<Game>.Instance.QuitGame();
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x00017CE0 File Offset: 0x00015EE0
	public void DrawShapes()
	{
		Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.DrawBtnClick);
		this.m_ShapeSelectUI.ShowThreeShapes();
		this.m_FuncUI.Hide();
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00017D03 File Offset: 0x00015F03
	public void SelectShape(TileShape shape, bool leveldown)
	{
		ConstructHelper.GetTutorialShape(shape.m_ShapeInfo, leveldown).SetPreviewPlace();
		this.m_ShapeSelectUI.Hide();
		this.m_BoardSystem.SetTutorialPoss(true);
		this.TransitionToState(StateName.PickingState);
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x00017D34 File Offset: 0x00015F34
	public void ConfirmShape()
	{
		this.TransitionToState(StateName.BuildingState);
		this.m_BoardSystem.CheckPathTrap();
		this.CheckAllBlueprints();
		this.m_BoardSystem.SetTutorialPoss(false);
		GameRes.ForcePlace = null;
		GameRes.PreSetShape = new ShapeInfo[3];
		Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.ConfirmShape);
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00017D81 File Offset: 0x00015F81
	public void UndoShape()
	{
		this.m_ShapeSelectUI.Show();
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x00017D90 File Offset: 0x00015F90
	public void CompositeShape(BluePrintGrid grid)
	{
		if (this.operationState.StateName == StateName.PickingState)
		{
			Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("PUTFIRST"));
			return;
		}
		if (this.operationState.StateName == StateName.WaveState)
		{
			Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("NOTBUILDSTATE"));
			return;
		}
		if (grid.Strategy.CheckBuildable())
		{
			this.TransitionToState(StateName.PickingState);
			this.m_BluePrintShopUI.RefactorBluePrint(grid);
			this.m_BoardSystem.SetTutorialPoss(true);
			Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.RefactorBtnClick);
			return;
		}
		Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("LACKMATERIAL"));
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x00017E30 File Offset: 0x00016030
	public void RefactorLanded(RefactorTurret turret)
	{
		this.refactorTurrets.Add(turret);
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x00017E3E File Offset: 0x0001603E
	public void PreviewComposition(bool value, ElementType element = ElementType.DUST, int quality = 1)
	{
		this.m_BluePrintShopUI.PreviewComposition(value, element, quality);
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x00017E4E File Offset: 0x0001604E
	public void PauseGame()
	{
		if (!this.m_SettingPanel.IsVisible())
		{
			this.m_SettingPanel.Show();
			return;
		}
		this.m_SettingPanel.ClosePanel();
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x00017E74 File Offset: 0x00016074
	public bool ConsumeMoney(int cost)
	{
		return this.m_MainUI.ConsumeMoney(cost);
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x00017E84 File Offset: 0x00016084
	public void GainMoney(int amount)
	{
		int num = Mathf.RoundToInt((float)amount * GameRes.CoinAdjust);
		GameRes.Coin += num;
		GameRes.GainGold += num;
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x00017EB7 File Offset: 0x000160B7
	public Enemy SpawnEnemy(EnemyType eType, int pathIndex, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		return this.m_WaveSystem.SpawnEnemy(eType, pathIndex, intensify, dmgResist, pathPoints);
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x00017ECB File Offset: 0x000160CB
	public void RefreshShop(int cost)
	{
		this.m_BluePrintShopUI.RefreshShop(cost);
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00017ED9 File Offset: 0x000160D9
	public void BuyOneGround()
	{
		this.m_BoardSystem.BuyOneEmptyTile();
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x00017EE8 File Offset: 0x000160E8
	public void SwitchConcrete(GameTileContent content, int cost)
	{
		if (this.operationState.StateName == StateName.PickingState)
		{
			Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("PUTFIRST"));
			return;
		}
		if (this.operationState.StateName != StateName.BuildingState)
		{
			Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("NOTBUILDSTATE"));
			return;
		}
		if (this.ConsumeMoney(cost))
		{
			GameRes.SwitchInfo = new SwitchInfo
			{
				SwitchSpend = cost,
				InitPos = new Vector2Int(Mathf.RoundToInt(content.transform.position.x), Mathf.RoundToInt(content.transform.position.y)),
				InitDir = (int)content.m_GameTile.TileDirection
			};
			this.m_BoardSystem.SwitchContent(content);
			this.TransitionToState(StateName.PickingState);
		}
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x00017FAD File Offset: 0x000161AD
	public void GainPerfectElement(int amount)
	{
		GameRes.PerfectElementCount += amount;
		this.CheckAllBlueprints();
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00017FC1 File Offset: 0x000161C1
	public void AddtoWishList()
	{
		Application.OpenURL("https://store.steampowered.com/app/1664670/_Refactor");
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x00017FCD File Offset: 0x000161CD
	public void JoinDiscord()
	{
		Application.OpenURL("https://discord.gg/bPgMZ6kgBH");
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x00017FD9 File Offset: 0x000161D9
	public void JoinQQ()
	{
		Application.OpenURL("https://jq.qq.com/?_wv=1027&k=wuuN4Bll");
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x00017FE5 File Offset: 0x000161E5
	public void LocateCamPos(Vector2 pos)
	{
		this.m_CamControl.LocatePos(pos);
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x00017FF3 File Offset: 0x000161F3
	public void SetCamMovable(bool value)
	{
		this.m_CamControl.CanControl = value;
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x00018001 File Offset: 0x00016201
	public void ShakeCam()
	{
		this.m_CamControl.ShakeCam();
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0001800E File Offset: 0x0001620E
	public void SetSizeTutorial(bool value)
	{
		this.m_CamControl.SizeTutorial = value;
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0001801C File Offset: 0x0001621C
	public void SetMoveTutorial(bool value)
	{
		this.m_CamControl.MoveTurorial = value;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0001802A File Offset: 0x0001622A
	public void ManualSetSequence(EnemyType type, float stage, int wave)
	{
		this.m_WaveSystem.ManualSetSequence(type, stage, wave);
		this.m_MainUI.PrepareNextWave(this.m_WaveSystem.RunningSequence, this.m_WaveSystem.NextBoss, this.m_WaveSystem.NextBossWave);
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00018066 File Offset: 0x00016266
	public void CheckAllBlueprints()
	{
		this.m_BluePrintShopUI.CheckAllBluePrint();
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x00018073 File Offset: 0x00016273
	public void AddBluePrint(RefactorStrategy strategy)
	{
		this.m_BluePrintShopUI.AddBluePrint(strategy);
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x00018081 File Offset: 0x00016281
	public void RemoveBluePrint(int id)
	{
		this.m_BluePrintShopUI.RemoveGrid(BluePrintShopUI.ShopBluePrints[id]);
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x00018099 File Offset: 0x00016299
	public void RemoveUnlockedRecipes()
	{
		this.m_BluePrintShopUI.RemoveUnlockedRecipes();
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x000180A6 File Offset: 0x000162A6
	public void ShowNormalChoices(bool show, ChallengeChoice choice)
	{
		if (show)
		{
			this.m_TechSelectUI.Show();
			this.m_TechSelectUI.GetCurrentChoices(choice, false);
		}
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x000180C4 File Offset: 0x000162C4
	public void ShowChoices(bool show, bool picking = false, bool newChoice = true)
	{
		if (show)
		{
			this.m_TechSelectUI.Show();
			if (newChoice)
			{
				ChallengeChoice currentChoice = this.m_ChallengeSystem.GetCurrentChoice();
				if (currentChoice != null)
				{
					this.m_TechSelectUI.GetCurrentChoices(currentChoice, true);
					return;
				}
				this.ConfirmChoice();
				return;
			}
		}
		else
		{
			this.m_TechSelectUI.Hide();
			if (picking)
			{
				this.TransitionToState(StateName.PickingState);
				return;
			}
			this.ConfirmChoice();
		}
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x00018122 File Offset: 0x00016322
	public void ConfirmChoice()
	{
		GameRes.ChallengeChoicePicked = true;
		this.m_FuncUI.Show();
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x00018138 File Offset: 0x00016338
	public void ShowTechSelect(bool show, bool picking = false)
	{
		if (show)
		{
			this.m_TechSelectUI.Show();
			this.m_TechSelectUI.GetRandomTechs();
			return;
		}
		this.m_TechSelectUI.Hide();
		if (picking)
		{
			this.TransitionToState(StateName.PickingState);
			return;
		}
		this.m_FuncUI.Show();
		this.ConfirmTechSelect();
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x00018186 File Offset: 0x00016386
	public void GetTech(Technology tech)
	{
		this.m_TechnologySystem.AddTech(tech);
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x00018194 File Offset: 0x00016394
	public void RemoveTech(Technology tech)
	{
		this.m_TechnologySystem.RemoveTech(tech);
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x000181A2 File Offset: 0x000163A2
	public void ConfirmTechSelect()
	{
		this.m_TechnologySystem.ConfirmTechSelect();
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x000181B0 File Offset: 0x000163B0
	public void TriggerDetectSkills()
	{
		foreach (IGameBehavior gameBehavior in this.elementTurrets.behaviors)
		{
			((TurretContent)gameBehavior).Strategy.DetectSkills();
		}
		foreach (IGameBehavior gameBehavior2 in this.refactorTurrets.behaviors)
		{
			((TurretContent)gameBehavior2).Strategy.DetectSkills();
		}
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x00018260 File Offset: 0x00016460
	public void DieProtect()
	{
		if (GameRes.DieProtect <= 0)
		{
			Singleton<GameEvents>.Instance.TempWordTrigger(new TempWord(TempWordType.DieProtect, 0));
		}
		Singleton<Sound>.Instance.PlayUISound("Sound_Warning");
		foreach (IGameBehavior gameBehavior in this.enemies.behaviors.ToList<IGameBehavior>())
		{
			((Enemy)gameBehavior).DamageStrategy.IsDie = true;
		}
		this.m_HealthWarning.Show();
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x000182F8 File Offset: 0x000164F8
	public List<ShapeInfo> GetCurrentPickingShapes()
	{
		return this.m_ShapeSelectUI.GetCurrent3ShapeInfos();
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00018305 File Offset: 0x00016505
	public void ShowConcreteRange(ConcreteContent concrete, bool value)
	{
		this.m_RangeContainer.SetRange(concrete, value);
	}

	// Token: 0x04000482 RID: 1154
	[Header("系统")]
	[SerializeField]
	private BoardSystem m_BoardSystem;

	// Token: 0x04000483 RID: 1155
	[SerializeField]
	private WaveSystem m_WaveSystem;

	// Token: 0x04000484 RID: 1156
	[SerializeField]
	private TechnologySystem m_TechnologySystem;

	// Token: 0x04000485 RID: 1157
	[SerializeField]
	private ScaleAndMove m_CamControl;

	// Token: 0x04000486 RID: 1158
	[SerializeField]
	private ChallengeSystem m_ChallengeSystem;

	// Token: 0x04000487 RID: 1159
	[Header("UI")]
	[SerializeField]
	private BluePrintShopUI m_BluePrintShopUI;

	// Token: 0x04000488 RID: 1160
	[SerializeField]
	private ShapeSelectUI m_ShapeSelectUI;

	// Token: 0x04000489 RID: 1161
	[SerializeField]
	private MainUI m_MainUI;

	// Token: 0x0400048A RID: 1162
	[SerializeField]
	private FuncUI m_FuncUI;

	// Token: 0x0400048B RID: 1163
	[SerializeField]
	private GameEndUI m_GameEndUI;

	// Token: 0x0400048C RID: 1164
	[SerializeField]
	private UISetting m_SettingPanel;

	// Token: 0x0400048D RID: 1165
	[SerializeField]
	private TechSelectUI m_TechSelectUI;

	// Token: 0x0400048E RID: 1166
	[Header("其他")]
	[SerializeField]
	private RangeContainer m_RangeContainer;

	// Token: 0x0400048F RID: 1167
	[SerializeField]
	private HealthWarning m_HealthWarning;

	// Token: 0x04000490 RID: 1168
	[Header("集合")]
	public GameBehaviorCollection enemies = new GameBehaviorCollection();

	// Token: 0x04000491 RID: 1169
	public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();

	// Token: 0x04000492 RID: 1170
	public GameBehaviorCollection elementTurrets = new GameBehaviorCollection();

	// Token: 0x04000493 RID: 1171
	public GameBehaviorCollection refactorTurrets = new GameBehaviorCollection();

	// Token: 0x04000494 RID: 1172
	public GameBehaviorCollection Buildings = new GameBehaviorCollection();

	// Token: 0x04000495 RID: 1173
	[Header("流程")]
	private BattleOperationState operationState;

	// Token: 0x04000496 RID: 1174
	private BuildingState buildingState;

	// Token: 0x04000497 RID: 1175
	private PickingState pickingState;

	// Token: 0x04000498 RID: 1176
	private WaveState waveState;

	// Token: 0x04000499 RID: 1177
	private EndState endState;

	// Token: 0x0400049A RID: 1178
	private WonState wonState;

	// Token: 0x0400049B RID: 1179
	private Dictionary<StateName, BattleOperationState> StateDIC = new Dictionary<StateName, BattleOperationState>();

	// Token: 0x0400049C RID: 1180
	private InputManager inputManager;
}
