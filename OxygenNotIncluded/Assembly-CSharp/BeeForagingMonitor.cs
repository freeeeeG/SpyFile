using System;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class BeeForagingMonitor : GameStateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def>
{
	// Token: 0x0600030A RID: 778 RVA: 0x000186F4 File Offset: 0x000168F4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToForage, new StateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def>.Transition.ConditionCallback(BeeForagingMonitor.ShouldForage), delegate(BeeForagingMonitor.Instance smi)
		{
			smi.RefreshSearchTime();
		});
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00018748 File Offset: 0x00016948
	public static bool ShouldForage(BeeForagingMonitor.Instance smi)
	{
		bool flag = GameClock.Instance.GetTimeInCycles() >= smi.nextSearchTime;
		KPrefabID kprefabID = smi.master.GetComponent<Bee>().FindHiveInRoom();
		if (kprefabID != null)
		{
			BeehiveCalorieMonitor.Instance smi2 = kprefabID.GetSMI<BeehiveCalorieMonitor.Instance>();
			if (smi2 == null || !smi2.IsHungry())
			{
				flag = false;
			}
		}
		return flag && kprefabID != null;
	}

	// Token: 0x02000E7E RID: 3710
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040053B2 RID: 21426
		public float searchMinInterval = 0.25f;

		// Token: 0x040053B3 RID: 21427
		public float searchMaxInterval = 0.3f;
	}

	// Token: 0x02000E7F RID: 3711
	public new class Instance : GameStateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def>.GameInstance
	{
		// Token: 0x06006F9C RID: 28572 RVA: 0x002B998F File Offset: 0x002B7B8F
		public Instance(IStateMachineTarget master, BeeForagingMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x06006F9D RID: 28573 RVA: 0x002B999F File Offset: 0x002B7B9F
		public void RefreshSearchTime()
		{
			this.nextSearchTime = GameClock.Instance.GetTimeInCycles() + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x040053B4 RID: 21428
		public float nextSearchTime;
	}
}
