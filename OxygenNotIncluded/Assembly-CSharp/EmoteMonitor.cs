using System;
using UnityEngine;

// Token: 0x02000878 RID: 2168
public class EmoteMonitor : GameStateMachine<EmoteMonitor, EmoteMonitor.Instance>
{
	// Token: 0x06003F41 RID: 16193 RVA: 0x001610E0 File Offset: 0x0015F2E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.ScheduleGoTo((float)UnityEngine.Random.Range(30, 90), this.ready);
		this.ready.ToggleUrge(Db.Get().Urges.Emote).EventHandler(GameHashes.BeginChore, delegate(EmoteMonitor.Instance smi, object o)
		{
			smi.OnStartChore(o);
		});
	}

	// Token: 0x040028FB RID: 10491
	public GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040028FC RID: 10492
	public GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.State ready;

	// Token: 0x0200166F RID: 5743
	public new class Instance : GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008ABF RID: 35519 RVA: 0x00314811 File Offset: 0x00312A11
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008AC0 RID: 35520 RVA: 0x0031481A File Offset: 0x00312A1A
		public void OnStartChore(object o)
		{
			if (((Chore)o).SatisfiesUrge(Db.Get().Urges.Emote))
			{
				this.GoTo(base.sm.satisfied);
			}
		}
	}
}
