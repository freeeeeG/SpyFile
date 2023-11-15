using System;
using STRINGS;

// Token: 0x0200070F RID: 1807
public class CreatureDiseaseCleaner : GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>
{
	// Token: 0x060031B6 RID: 12726 RVA: 0x00108134 File Offset: 0x00106334
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cleaning;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.CLEANING.NAME, CREATURES.STATUSITEMS.CLEANING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.cleaning.DefaultState(this.cleaning.clean_pre).ScheduleGoTo((CreatureDiseaseCleaner.Instance smi) => smi.def.cleanDuration, this.cleaning.clean_pst);
		this.cleaning.clean_pre.PlayAnim("clean_water_pre").OnAnimQueueComplete(this.cleaning.clean);
		this.cleaning.clean.Enter(delegate(CreatureDiseaseCleaner.Instance smi)
		{
			smi.EnableDiseaseEmitter(true);
		}).QueueAnim("clean_water_loop", true, null).Transition(this.cleaning.clean_pst, (CreatureDiseaseCleaner.Instance smi) => !smi.GetSMI<CleaningMonitor.Instance>().CanCleanElementState(), UpdateRate.SIM_1000ms).Exit(delegate(CreatureDiseaseCleaner.Instance smi)
		{
			smi.EnableDiseaseEmitter(false);
		});
		this.cleaning.clean_pst.PlayAnim("clean_water_pst").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Cleaning, false);
	}

	// Token: 0x04001DC3 RID: 7619
	public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State behaviourcomplete;

	// Token: 0x04001DC4 RID: 7620
	public CreatureDiseaseCleaner.CleaningStates cleaning;

	// Token: 0x04001DC5 RID: 7621
	public StateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.Signal cellChangedSignal;

	// Token: 0x0200146C RID: 5228
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06008492 RID: 33938 RVA: 0x00302D79 File Offset: 0x00300F79
		public Def(float duration)
		{
			this.cleanDuration = duration;
		}

		// Token: 0x04006560 RID: 25952
		public float cleanDuration;
	}

	// Token: 0x0200146D RID: 5229
	public class CleaningStates : GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State
	{
		// Token: 0x04006561 RID: 25953
		public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State clean_pre;

		// Token: 0x04006562 RID: 25954
		public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State clean;

		// Token: 0x04006563 RID: 25955
		public GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.State clean_pst;
	}

	// Token: 0x0200146E RID: 5230
	public new class Instance : GameStateMachine<CreatureDiseaseCleaner, CreatureDiseaseCleaner.Instance, IStateMachineTarget, CreatureDiseaseCleaner.Def>.GameInstance
	{
		// Token: 0x06008494 RID: 33940 RVA: 0x00302D90 File Offset: 0x00300F90
		public Instance(Chore<CreatureDiseaseCleaner.Instance> chore, CreatureDiseaseCleaner.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Cleaning);
		}

		// Token: 0x06008495 RID: 33941 RVA: 0x00302DB4 File Offset: 0x00300FB4
		public void EnableDiseaseEmitter(bool enable = true)
		{
			DiseaseEmitter component = base.GetComponent<DiseaseEmitter>();
			if (component != null)
			{
				component.SetEnable(enable);
			}
		}
	}
}
