using System;
using UnityEngine;

// Token: 0x0200070D RID: 1805
public class CallAdultMonitor : GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>
{
	// Token: 0x0600319C RID: 12700 RVA: 0x00107AB4 File Offset: 0x00105CB4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.CallAdultBehaviour, new StateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.Transition.ConditionCallback(CallAdultMonitor.ShouldCallAdult), delegate(CallAdultMonitor.Instance smi)
		{
			smi.RefreshCallTime();
		});
	}

	// Token: 0x0600319D RID: 12701 RVA: 0x00107B05 File Offset: 0x00105D05
	public static bool ShouldCallAdult(CallAdultMonitor.Instance smi)
	{
		return Time.time >= smi.nextCallTime;
	}

	// Token: 0x02001468 RID: 5224
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400655A RID: 25946
		public float callMinInterval = 120f;

		// Token: 0x0400655B RID: 25947
		public float callMaxInterval = 240f;
	}

	// Token: 0x02001469 RID: 5225
	public new class Instance : GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.GameInstance
	{
		// Token: 0x06008488 RID: 33928 RVA: 0x00302CE7 File Offset: 0x00300EE7
		public Instance(IStateMachineTarget master, CallAdultMonitor.Def def) : base(master, def)
		{
			this.RefreshCallTime();
		}

		// Token: 0x06008489 RID: 33929 RVA: 0x00302CF7 File Offset: 0x00300EF7
		public void RefreshCallTime()
		{
			this.nextCallTime = Time.time + UnityEngine.Random.value * (base.def.callMaxInterval - base.def.callMinInterval) + base.def.callMinInterval;
		}

		// Token: 0x0400655C RID: 25948
		public float nextCallTime;
	}
}
