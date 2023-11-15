using System;

// Token: 0x020000C4 RID: 196
public class AirProtector : Aircraft
{
	// Token: 0x06000509 RID: 1289 RVA: 0x0000DE34 File Offset: 0x0000C034
	public override void Initiate(AircraftCarrier boss, float maxHealth, float dmgIntenWhenDie, float dmgResist)
	{
		base.Initiate(boss, maxHealth, dmgIntenWhenDie, dmgResist);
		if (this.fsm == null)
		{
			this.boss = boss;
			this.fsm = new FSMSystem();
			FSMState fsmstate = new AirProtectorPatrolState(this.fsm);
			fsmstate.AddTransition(Transition.ProtectBoss, StateID.Protect);
			base.PickRandomDes();
			FSMState fsmstate2 = new ProtectState(this.fsm);
			fsmstate2.AddTransition(Transition.LureTarget, StateID.Lure);
			FSMState fsmstate3 = new AirProtectorLureState(this.fsm);
			fsmstate3.AddTransition(Transition.ProtectBoss, StateID.Protect);
			this.fsm.AddState(fsmstate);
			this.fsm.AddState(fsmstate2);
			this.fsm.AddState(fsmstate3);
			Singleton<GameManager>.Instance.nonEnemies.Add(this);
		}
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x0000DEDF File Offset: 0x0000C0DF
	public override bool GameUpdate()
	{
		this.fsm.Update(this);
		return base.GameUpdate();
	}
}
