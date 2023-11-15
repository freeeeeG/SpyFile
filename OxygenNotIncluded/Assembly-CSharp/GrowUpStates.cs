using System;
using STRINGS;

// Token: 0x020000C5 RID: 197
public class GrowUpStates : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>
{
	// Token: 0x0600038E RID: 910 RVA: 0x0001BE74 File Offset: 0x0001A074
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grow_up_pre;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.GROWINGUP.NAME, CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.grow_up_pre.Enter(delegate(GrowUpStates.Instance smi)
		{
			smi.PlayPreGrowAnimation();
		}).OnAnimQueueComplete(this.spawn_adult).ScheduleGoTo(4f, this.spawn_adult);
		this.spawn_adult.Enter(new StateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State.Callback(GrowUpStates.SpawnAdult));
	}

	// Token: 0x0600038F RID: 911 RVA: 0x0001BF2E File Offset: 0x0001A12E
	private static void SpawnAdult(GrowUpStates.Instance smi)
	{
		smi.GetSMI<BabyMonitor.Instance>().SpawnAdult();
	}

	// Token: 0x04000262 RID: 610
	public const float GROW_PRE_TIMEOUT = 4f;

	// Token: 0x04000263 RID: 611
	public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State grow_up_pre;

	// Token: 0x04000264 RID: 612
	public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State spawn_adult;

	// Token: 0x02000ED5 RID: 3797
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000ED6 RID: 3798
	public new class Instance : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.GameInstance
	{
		// Token: 0x06007059 RID: 28761 RVA: 0x002BAA09 File Offset: 0x002B8C09
		public Instance(Chore<GrowUpStates.Instance> chore, GrowUpStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.GrowUpBehaviour);
		}

		// Token: 0x0600705A RID: 28762 RVA: 0x002BAA30 File Offset: 0x002B8C30
		public void PlayPreGrowAnimation()
		{
			if (base.gameObject.HasTag(GameTags.Creatures.PreventGrowAnimation))
			{
				return;
			}
			KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("growup_pre", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}
}
