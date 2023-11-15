using System;
using STRINGS;

// Token: 0x020003CB RID: 971
public class TakeMedicineChore : Chore<TakeMedicineChore.StatesInstance>
{
	// Token: 0x06001409 RID: 5129 RVA: 0x0006ADAC File Offset: 0x00068FAC
	public TakeMedicineChore(MedicinalPillWorkable master) : base(Db.Get().ChoreTypes.TakeMedicine, master, null, false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.medicine = master;
		this.pickupable = this.medicine.GetComponent<Pickupable>();
		base.smi = new TakeMedicineChore.StatesInstance(this);
		base.AddPrecondition(ChorePreconditions.instance.CanPickup, this.pickupable);
		base.AddPrecondition(TakeMedicineChore.CanCure, this);
		base.AddPrecondition(TakeMedicineChore.IsConsumptionPermitted, this);
		base.AddPrecondition(ChorePreconditions.instance.IsNotARobot, this);
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x0006AE40 File Offset: 0x00069040
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.source.Set(this.pickupable.gameObject, base.smi, false);
		base.smi.sm.requestedpillcount.Set(1f, base.smi, false);
		base.smi.sm.eater.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
		new TakeMedicineChore(this.medicine);
	}

	// Token: 0x04000ACA RID: 2762
	private Pickupable pickupable;

	// Token: 0x04000ACB RID: 2763
	private MedicinalPillWorkable medicine;

	// Token: 0x04000ACC RID: 2764
	public static readonly Chore.Precondition CanCure = new Chore.Precondition
	{
		id = "CanCure",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_CURE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((TakeMedicineChore)data).medicine.CanBeTakenBy(context.consumerState.gameObject);
		}
	};

	// Token: 0x04000ACD RID: 2765
	public static readonly Chore.Precondition IsConsumptionPermitted = new Chore.Precondition
	{
		id = "IsConsumptionPermitted",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_CONSUMPTION_PERMITTED,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			TakeMedicineChore takeMedicineChore = (TakeMedicineChore)data;
			ConsumableConsumer consumableConsumer = context.consumerState.consumableConsumer;
			return consumableConsumer == null || consumableConsumer.IsPermitted(takeMedicineChore.medicine.PrefabID().Name);
		}
	};

	// Token: 0x02001031 RID: 4145
	public class StatesInstance : GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.GameInstance
	{
		// Token: 0x060074FE RID: 29950 RVA: 0x002CB561 File Offset: 0x002C9761
		public StatesInstance(TakeMedicineChore master) : base(master)
		{
		}
	}

	// Token: 0x02001032 RID: 4146
	public class States : GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore>
	{
		// Token: 0x060074FF RID: 29951 RVA: 0x002CB56C File Offset: 0x002C976C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.eater);
			this.fetch.InitializeStates(this.eater, this.source, this.chunk, this.requestedpillcount, this.actualpillcount, this.takemedicine, null);
			this.takemedicine.ToggleAnims("anim_eat_floor_kanim", 0f, "").ToggleTag(GameTags.TakingMedicine).ToggleWork("TakeMedicine", delegate(TakeMedicineChore.StatesInstance smi)
			{
				MedicinalPillWorkable workable = this.chunk.Get<MedicinalPillWorkable>(smi);
				this.eater.Get<Worker>(smi).StartWork(new Worker.StartWorkInfo(workable));
			}, (TakeMedicineChore.StatesInstance smi) => this.chunk.Get<MedicinalPill>(smi) != null, null, null);
		}

		// Token: 0x0400586A RID: 22634
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.TargetParameter eater;

		// Token: 0x0400586B RID: 22635
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.TargetParameter source;

		// Token: 0x0400586C RID: 22636
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.TargetParameter chunk;

		// Token: 0x0400586D RID: 22637
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.FloatParameter requestedpillcount;

		// Token: 0x0400586E RID: 22638
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.FloatParameter actualpillcount;

		// Token: 0x0400586F RID: 22639
		public GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.FetchSubState fetch;

		// Token: 0x04005870 RID: 22640
		public GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.State takemedicine;
	}
}
