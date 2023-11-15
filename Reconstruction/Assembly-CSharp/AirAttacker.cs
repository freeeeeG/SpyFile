using System;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class AirAttacker : Aircraft
{
	// Token: 0x060004DB RID: 1243 RVA: 0x0000D330 File Offset: 0x0000B530
	public override void Initiate(AircraftCarrier boss, float maxHealth, float dmgIntenWhenDie, float dmgResist)
	{
		base.Initiate(boss, maxHealth, dmgIntenWhenDie, dmgResist);
		this.freezeTime = Mathf.Min(12f, 6f + 0.5f * (float)((GameRes.CurrentWave + 1) / 20));
		this.freezeRange = (this.isSuperAircraft ? Mathf.Min(6f, 1.5f + 0.25f * (float)((GameRes.CurrentWave + 1) / 20)) : 0.5f);
		if (this.fsm == null)
		{
			this.boss = boss;
			this.fsm = new FSMSystem();
			FSMState fsmstate = new AirAttackerPatrolState(this.fsm);
			fsmstate.AddTransition(Transition.AttackTarget, StateID.Track);
			fsmstate.AddTransition(Transition.LureTarget, StateID.Lure);
			fsmstate.AddTransition(Transition.ProtectBoss, StateID.Protect);
			base.PickRandomDes();
			FSMState fsmstate2 = new TrackState(this.fsm);
			fsmstate2.AddTransition(Transition.Attacked, StateID.Back);
			fsmstate2.AddTransition(Transition.ProtectBoss, StateID.Protect);
			FSMState fsmstate3 = new AirAttackerLureState(this.fsm);
			fsmstate3.AddTransition(Transition.Attacked, StateID.Back);
			fsmstate3.AddTransition(Transition.ProtectBoss, StateID.Protect);
			FSMState fsmstate4 = new BackState(this.fsm);
			fsmstate4.AddTransition(Transition.ProtectBoss, StateID.Protect);
			fsmstate4.AddTransition(Transition.Patrol, StateID.Patrol);
			FSMState fsmstate5 = new ProtectState(this.fsm);
			fsmstate5.AddTransition(Transition.Patrol, StateID.Patrol);
			fsmstate5.AddTransition(Transition.ProtectBoss, StateID.Protect);
			this.fsm.AddState(fsmstate);
			this.fsm.AddState(fsmstate2);
			this.fsm.AddState(fsmstate4);
			this.fsm.AddState(fsmstate3);
			this.fsm.AddState(fsmstate5);
		}
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x0000D49C File Offset: 0x0000B69C
	public override bool GameUpdate()
	{
		this.fsm.Update(this);
		return base.GameUpdate();
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x0000D4B0 File Offset: 0x0000B6B0
	public override void Attack()
	{
		if (this.targetTurret.Activated)
		{
			Singleton<StaticData>.Instance.FrostTurretEffect(this.targetTurret.transform.position, this.freezeRange, this.freezeTime);
		}
		this.targetTurret = null;
	}

	// Token: 0x040001DD RID: 477
	[SerializeField]
	protected float freezeTime;

	// Token: 0x040001DE RID: 478
	[SerializeField]
	protected float freezeRange;

	// Token: 0x040001DF RID: 479
	[SerializeField]
	private bool isSuperAircraft;
}
