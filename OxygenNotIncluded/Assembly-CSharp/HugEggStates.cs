using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class HugEggStates : GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>
{
	// Token: 0x060003A0 RID: 928 RVA: 0x0001C444 File Offset: 0x0001A644
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		this.root.Enter(new StateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State.Callback(HugEggStates.SetTarget)).Enter(delegate(HugEggStates.Instance smi)
		{
			if (!HugEggStates.Reserve(smi))
			{
				smi.GoTo(this.behaviourcomplete);
			}
		}).Exit(new StateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State.Callback(HugEggStates.Unreserve)).ToggleStatusItem(CREATURES.STATUSITEMS.HUGEGG.NAME, CREATURES.STATUSITEMS.HUGEGG.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).OnTargetLost(this.target, this.behaviourcomplete);
		this.moving.MoveTo(new Func<HugEggStates.Instance, int>(HugEggStates.GetClimbableCell), this.hug, this.behaviourcomplete, false);
		this.hug.DefaultState(this.hug.pre).Enter(delegate(HugEggStates.Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Front);
		}).Exit(delegate(HugEggStates.Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
		});
		this.hug.pre.Face(this.target, 0.5f).PlayAnim((HugEggStates.Instance smi) => HugEggStates.GetAnims(smi).pre, KAnim.PlayMode.Once).OnAnimQueueComplete(this.hug.loop);
		this.hug.loop.QueueAnim((HugEggStates.Instance smi) => HugEggStates.GetAnims(smi).loop, true, null).ScheduleGoTo((HugEggStates.Instance smi) => smi.def.hugTime, this.hug.pst);
		this.hug.pst.QueueAnim((HugEggStates.Instance smi) => HugEggStates.GetAnims(smi).pst, false, null).Enter(new StateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State.Callback(HugEggStates.ApplyEffect)).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete((HugEggStates.Instance smi) => smi.def.behaviourTag, false);
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0001C694 File Offset: 0x0001A894
	private static void SetTarget(HugEggStates.Instance smi)
	{
		smi.sm.target.Set(smi.GetSMI<HugMonitor.Instance>().hugTarget, smi, false);
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0001C6B4 File Offset: 0x0001A8B4
	private static HugEggStates.AnimSet GetAnims(HugEggStates.Instance smi)
	{
		if (!(smi.sm.target.Get(smi).GetComponent<EggIncubator>() != null))
		{
			return smi.def.hugAnims;
		}
		return smi.def.incubatorHugAnims;
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0001C6EC File Offset: 0x0001A8EC
	private static bool Reserve(HugEggStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null && !gameObject.HasTag(GameTags.Creatures.ReservedByCreature))
		{
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
			return true;
		}
		return false;
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0001C730 File Offset: 0x0001A930
	private static void Unreserve(HugEggStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0001C763 File Offset: 0x0001A963
	private static int GetClimbableCell(HugEggStates.Instance smi)
	{
		return Grid.PosToCell(smi.sm.target.Get(smi));
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0001C77C File Offset: 0x0001A97C
	private static void ApplyEffect(HugEggStates.Instance smi)
	{
		GameObject gameObject = smi.sm.target.Get(smi);
		if (gameObject != null)
		{
			EggIncubator component = gameObject.GetComponent<EggIncubator>();
			if (component != null && component.Occupant != null)
			{
				component.Occupant.GetComponent<Effects>().Add("EggHug", true);
				return;
			}
			if (gameObject.HasTag(GameTags.Egg))
			{
				gameObject.GetComponent<Effects>().Add("EggHug", true);
			}
		}
	}

	// Token: 0x0400026C RID: 620
	public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.ApproachSubState<EggIncubator> moving;

	// Token: 0x0400026D RID: 621
	public HugEggStates.HugState hug;

	// Token: 0x0400026E RID: 622
	public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State behaviourcomplete;

	// Token: 0x0400026F RID: 623
	public StateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.TargetParameter target;

	// Token: 0x02000EEA RID: 3818
	public class AnimSet
	{
		// Token: 0x04005476 RID: 21622
		public string pre;

		// Token: 0x04005477 RID: 21623
		public string loop;

		// Token: 0x04005478 RID: 21624
		public string pst;
	}

	// Token: 0x02000EEB RID: 3819
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007089 RID: 28809 RVA: 0x002BAF98 File Offset: 0x002B9198
		public Def(Tag behaviourTag)
		{
			this.behaviourTag = behaviourTag;
		}

		// Token: 0x04005479 RID: 21625
		public float hugTime = 15f;

		// Token: 0x0400547A RID: 21626
		public Tag behaviourTag;

		// Token: 0x0400547B RID: 21627
		public HugEggStates.AnimSet hugAnims = new HugEggStates.AnimSet
		{
			pre = "hug_egg_pre",
			loop = "hug_egg_loop",
			pst = "hug_egg_pst"
		};

		// Token: 0x0400547C RID: 21628
		public HugEggStates.AnimSet incubatorHugAnims = new HugEggStates.AnimSet
		{
			pre = "hug_incubator_pre",
			loop = "hug_incubator_loop",
			pst = "hug_incubator_pst"
		};
	}

	// Token: 0x02000EEC RID: 3820
	public new class Instance : GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.GameInstance
	{
		// Token: 0x0600708A RID: 28810 RVA: 0x002BB015 File Offset: 0x002B9215
		public Instance(Chore<HugEggStates.Instance> chore, HugEggStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.behaviourTag);
		}
	}

	// Token: 0x02000EED RID: 3821
	public class HugState : GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State
	{
		// Token: 0x0400547D RID: 21629
		public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State pre;

		// Token: 0x0400547E RID: 21630
		public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State loop;

		// Token: 0x0400547F RID: 21631
		public GameStateMachine<HugEggStates, HugEggStates.Instance, IStateMachineTarget, HugEggStates.Def>.State pst;
	}
}
