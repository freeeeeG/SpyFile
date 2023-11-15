using System;

// Token: 0x0200013B RID: 315
public class BattleState : ISceneState
{
	// Token: 0x0600082A RID: 2090 RVA: 0x00015629 File Offset: 0x00013829
	public BattleState(SceneStateController Controller) : base(Controller)
	{
		base.StateName = "BattleState";
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x0001563D File Offset: 0x0001383D
	public override void StateBegin()
	{
		Singleton<LevelManager>.Instance.Initialize();
		Singleton<GameManager>.Instance.Initinal();
		Singleton<LevelManager>.Instance.StartingGame = false;
		Singleton<TipsManager>.Instance.SetCanvasCam();
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x00015668 File Offset: 0x00013868
	public override void StateEnd()
	{
		DraggingShape.PickingShape = null;
		Singleton<GameManager>.Instance.Release();
		EnemyBuffFactory.Release();
		TurretSkillFactory.Release();
		Singleton<GuideGirlSystem>.Instance.Release();
		Singleton<GuideGirlSystem>.Instance.Hide();
		Singleton<TipsManager>.Instance.HideTips();
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x000156A2 File Offset: 0x000138A2
	public override void StateUpdate()
	{
		Singleton<GameManager>.Instance.GameUpdate();
	}
}
