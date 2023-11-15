using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class LureableMonitor : GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>
{
	// Token: 0x06001AB1 RID: 6833 RVA: 0x0008EA78 File Offset: 0x0008CC78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		this.cooldown.ScheduleGoTo((LureableMonitor.Instance smi) => smi.def.cooldown, this.nolure);
		this.nolure.Update("FindLure", delegate(LureableMonitor.Instance smi, float dt)
		{
			smi.FindLure();
		}, UpdateRate.SIM_1000ms, false).ParamTransition<GameObject>(this.targetLure, this.haslure, (LureableMonitor.Instance smi, GameObject p) => p != null);
		this.haslure.ParamTransition<GameObject>(this.targetLure, this.nolure, (LureableMonitor.Instance smi, GameObject p) => p == null).Update("FindLure", delegate(LureableMonitor.Instance smi, float dt)
		{
			smi.FindLure();
		}, UpdateRate.SIM_1000ms, false).ToggleBehaviour(GameTags.Creatures.MoveToLure, (LureableMonitor.Instance smi) => smi.HasLure(), delegate(LureableMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		});
	}

	// Token: 0x04000ED8 RID: 3800
	public StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.TargetParameter targetLure;

	// Token: 0x04000ED9 RID: 3801
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State nolure;

	// Token: 0x04000EDA RID: 3802
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State haslure;

	// Token: 0x04000EDB RID: 3803
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State cooldown;

	// Token: 0x0200114A RID: 4426
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x060078FB RID: 30971 RVA: 0x002D81CE File Offset: 0x002D63CE
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_LURE, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_LURE, Descriptor.DescriptorType.Effect, false)
			};
		}

		// Token: 0x04005BEA RID: 23530
		public float cooldown = 20f;

		// Token: 0x04005BEB RID: 23531
		public Tag[] lures;
	}

	// Token: 0x0200114B RID: 4427
	public new class Instance : GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.GameInstance
	{
		// Token: 0x060078FD RID: 30973 RVA: 0x002D8209 File Offset: 0x002D6409
		public Instance(IStateMachineTarget master, LureableMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060078FE RID: 30974 RVA: 0x002D8214 File Offset: 0x002D6414
		public void FindLure()
		{
			LureableMonitor.Instance.LureIterator lureIterator = new LureableMonitor.Instance.LureIterator(base.GetComponent<Navigator>(), base.def.lures);
			GameScenePartitioner.Instance.Iterate<LureableMonitor.Instance.LureIterator>(Grid.PosToCell(base.smi.transform.GetPosition()), 1, GameScenePartitioner.Instance.lure, ref lureIterator);
			lureIterator.Cleanup();
			base.sm.targetLure.Set(lureIterator.result, this, false);
		}

		// Token: 0x060078FF RID: 30975 RVA: 0x002D8286 File Offset: 0x002D6486
		public bool HasLure()
		{
			return base.sm.targetLure.Get(this) != null;
		}

		// Token: 0x06007900 RID: 30976 RVA: 0x002D829F File Offset: 0x002D649F
		public GameObject GetTargetLure()
		{
			return base.sm.targetLure.Get(this);
		}

		// Token: 0x02002094 RID: 8340
		private struct LureIterator : GameScenePartitioner.Iterator
		{
			// Token: 0x17000A70 RID: 2672
			// (get) Token: 0x0600A616 RID: 42518 RVA: 0x0036D6A2 File Offset: 0x0036B8A2
			// (set) Token: 0x0600A617 RID: 42519 RVA: 0x0036D6AA File Offset: 0x0036B8AA
			public int cost { readonly get; private set; }

			// Token: 0x17000A71 RID: 2673
			// (get) Token: 0x0600A618 RID: 42520 RVA: 0x0036D6B3 File Offset: 0x0036B8B3
			// (set) Token: 0x0600A619 RID: 42521 RVA: 0x0036D6BB File Offset: 0x0036B8BB
			public GameObject result { readonly get; private set; }

			// Token: 0x0600A61A RID: 42522 RVA: 0x0036D6C4 File Offset: 0x0036B8C4
			public LureIterator(Navigator navigator, Tag[] lures)
			{
				this.navigator = navigator;
				this.lures = lures;
				this.cost = -1;
				this.result = null;
			}

			// Token: 0x0600A61B RID: 42523 RVA: 0x0036D6E4 File Offset: 0x0036B8E4
			public void Iterate(object target_obj)
			{
				Lure.Instance instance = target_obj as Lure.Instance;
				if (instance == null || !instance.IsActive() || !instance.HasAnyLure(this.lures))
				{
					return;
				}
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(instance.transform.GetPosition()), instance.LurePoints);
				if (navigationCost != -1 && (this.cost == -1 || navigationCost < this.cost))
				{
					this.cost = navigationCost;
					this.result = instance.gameObject;
				}
			}

			// Token: 0x0600A61C RID: 42524 RVA: 0x0036D75D File Offset: 0x0036B95D
			public void Cleanup()
			{
			}

			// Token: 0x04009190 RID: 37264
			private Navigator navigator;

			// Token: 0x04009191 RID: 37265
			private Tag[] lures;
		}
	}
}
