using System;
using Klei.AI;

// Token: 0x0200087F RID: 2175
public class HygieneMonitor : GameStateMachine<HygieneMonitor, HygieneMonitor.Instance>
{
	// Token: 0x06003F63 RID: 16227 RVA: 0x00162244 File Offset: 0x00160444
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.needsshower;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.clean.EventTransition(GameHashes.EffectRemoved, this.needsshower, (HygieneMonitor.Instance smi) => smi.NeedsShower());
		this.needsshower.EventTransition(GameHashes.EffectAdded, this.clean, (HygieneMonitor.Instance smi) => !smi.NeedsShower()).ToggleUrge(Db.Get().Urges.Shower).Enter(delegate(HygieneMonitor.Instance smi)
		{
			smi.SetDirtiness(1f);
		});
	}

	// Token: 0x04002922 RID: 10530
	public StateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.FloatParameter dirtiness;

	// Token: 0x04002923 RID: 10531
	public GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State clean;

	// Token: 0x04002924 RID: 10532
	public GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State needsshower;

	// Token: 0x02001682 RID: 5762
	public new class Instance : GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008B30 RID: 35632 RVA: 0x00316030 File Offset: 0x00314230
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.effects = master.GetComponent<Effects>();
		}

		// Token: 0x06008B31 RID: 35633 RVA: 0x00316045 File Offset: 0x00314245
		public float GetDirtiness()
		{
			return base.sm.dirtiness.Get(this);
		}

		// Token: 0x06008B32 RID: 35634 RVA: 0x00316058 File Offset: 0x00314258
		public void SetDirtiness(float dirtiness)
		{
			base.sm.dirtiness.Set(dirtiness, this, false);
		}

		// Token: 0x06008B33 RID: 35635 RVA: 0x0031606E File Offset: 0x0031426E
		public bool NeedsShower()
		{
			return !this.effects.HasEffect(Shower.SHOWER_EFFECT);
		}

		// Token: 0x04006BFD RID: 27645
		private Effects effects;
	}
}
