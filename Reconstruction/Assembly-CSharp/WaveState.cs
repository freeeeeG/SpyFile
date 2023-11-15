using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class WaveState : BattleOperationState
{
	// Token: 0x06000A14 RID: 2580 RVA: 0x0001B674 File Offset: 0x00019874
	public WaveState(GameManager gameManager, WaveSystem waveSystem, BoardSystem boardSystem) : base(gameManager)
	{
		this.m_BoardSystem = boardSystem;
		this.m_WaveSystem = waveSystem;
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0001B68B File Offset: 0x0001988B
	public override StateName StateName
	{
		get
		{
			return StateName.WaveState;
		}
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x0001B68E File Offset: 0x0001988E
	public override IEnumerator EnterState()
	{
		this.gameManager.OperationState = this;
		StrategyBase.CooporativeAttackIntensify = 0f;
		this.m_BoardSystem.GetPathTiles();
		EnemyType enemyType = this.m_WaveSystem.RunningSequence[0].EnemyType;
		if (enemyType <= EnemyType.Leader || enemyType == EnemyType.GoldKeeper)
		{
			Singleton<Sound>.Instance.PlayBg("Music_Normal");
		}
		else
		{
			Singleton<Sound>.Instance.PlayBg("Music_Boss");
		}
		this.m_BoardSystem.TransparentPath(new Color(1f, 0.4f, 0.2f, 0.35f), 0.5f);
		foreach (IGameBehavior gameBehavior in Singleton<GameManager>.Instance.refactorTurrets.behaviors)
		{
			((ConcreteContent)gameBehavior).Strategy.ClearTurnAnalysis();
			((ConcreteContent)gameBehavior).Strategy.StartTurnSkills();
			((ConcreteContent)gameBehavior).Strategy.StartTurnSkill2();
			((ConcreteContent)gameBehavior).Strategy.StartTurnSkill3();
		}
		foreach (IGameBehavior gameBehavior2 in Singleton<GameManager>.Instance.elementTurrets.behaviors)
		{
			((ConcreteContent)gameBehavior2).Strategy.ClearTurnAnalysis();
		}
		foreach (IGameBehavior gameBehavior3 in Singleton<GameManager>.Instance.Buildings.behaviors)
		{
			((ConcreteContent)gameBehavior3).Strategy.StartTurnSkills();
			((ConcreteContent)gameBehavior3).Strategy.StartTurnSkill2();
			((ConcreteContent)gameBehavior3).Strategy.StartTurnSkill3();
		}
		GameRes.MaxPath = BoardSystem.shortestPath.Count;
		yield return new WaitForSeconds(0.5f);
		this.m_WaveSystem.RunningSpawn = true;
		yield break;
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x0001B69D File Offset: 0x0001989D
	public override IEnumerator ExitState(BattleOperationState newState)
	{
		yield return new WaitForSeconds(0.2f);
		Singleton<GameEvents>.Instance.TempWordTrigger(new TempWord(TempWordType.WaveEnd, GameRes.CurrentWave));
		StrategyBase.CooporativeAttackIntensify = 0f;
		Singleton<GameManager>.Instance.nonEnemies.RemoveAll();
		foreach (IGameBehavior gameBehavior in Singleton<GameManager>.Instance.elementTurrets.behaviors)
		{
			((ConcreteContent)gameBehavior).Strategy.ClearTurnIntensify();
			((ConcreteContent)gameBehavior).TurnClear();
		}
		foreach (IGameBehavior gameBehavior2 in Singleton<GameManager>.Instance.refactorTurrets.behaviors)
		{
			((ConcreteContent)gameBehavior2).Strategy.ClearTurnIntensify();
			((ConcreteContent)gameBehavior2).TurnClear();
		}
		foreach (IGameBehavior gameBehavior3 in Singleton<GameManager>.Instance.Buildings.behaviors)
		{
			((ConcreteContent)gameBehavior3).Strategy.ClearTurnIntensify();
			((ConcreteContent)gameBehavior3).TurnClear();
		}
		yield return new WaitForSeconds(0.1f);
		this.gameManager.StartCoroutine(newState.EnterState());
		yield break;
	}

	// Token: 0x04000557 RID: 1367
	private WaveSystem m_WaveSystem;

	// Token: 0x04000558 RID: 1368
	private BoardSystem m_BoardSystem;
}
