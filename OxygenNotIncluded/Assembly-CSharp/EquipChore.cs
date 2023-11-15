using System;

// Token: 0x020003B1 RID: 945
public class EquipChore : Chore<EquipChore.StatesInstance>
{
	// Token: 0x060013A9 RID: 5033 RVA: 0x00068860 File Offset: 0x00066A60
	public EquipChore(IStateMachineTarget equippable) : base(Db.Get().ChoreTypes.Equip, equippable, null, false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EquipChore.StatesInstance(this);
		base.smi.sm.equippable_source.Set(equippable.gameObject, base.smi, false);
		base.smi.sm.requested_units.Set(1f, base.smi, false);
		this.showAvailabilityInHoverText = false;
		Prioritizable.AddRef(equippable.gameObject);
		Game.Instance.Trigger(1980521255, equippable.gameObject);
		base.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, equippable.GetComponent<Assignable>());
		base.AddPrecondition(ChorePreconditions.instance.CanPickup, equippable.GetComponent<Pickupable>());
	}

	// Token: 0x060013AA RID: 5034 RVA: 0x00068934 File Offset: 0x00066B34
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			Debug.LogError("EquipChore null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			Debug.LogError("EquipChore null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			Debug.LogError("EquipChore null smi.sm");
			return;
		}
		if (base.smi.sm.equippable_source == null)
		{
			Debug.LogError("EquipChore null smi.sm.equippable_source");
			return;
		}
		base.smi.sm.equipper.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x02000FF4 RID: 4084
	public class StatesInstance : GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.GameInstance
	{
		// Token: 0x06007433 RID: 29747 RVA: 0x002C5E55 File Offset: 0x002C4055
		public StatesInstance(EquipChore master) : base(master)
		{
		}
	}

	// Token: 0x02000FF5 RID: 4085
	public class States : GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore>
	{
		// Token: 0x06007434 RID: 29748 RVA: 0x002C5E60 File Offset: 0x002C4060
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.equipper);
			this.root.DoNothing();
			this.fetch.InitializeStates(this.equipper, this.equippable_source, this.equippable_result, this.requested_units, this.actual_units, this.equip, null);
			this.equip.ToggleWork<EquippableWorkable>(this.equippable_result, null, null, null);
		}

		// Token: 0x0400579F RID: 22431
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.TargetParameter equipper;

		// Token: 0x040057A0 RID: 22432
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.TargetParameter equippable_source;

		// Token: 0x040057A1 RID: 22433
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.TargetParameter equippable_result;

		// Token: 0x040057A2 RID: 22434
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.FloatParameter requested_units;

		// Token: 0x040057A3 RID: 22435
		public StateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.FloatParameter actual_units;

		// Token: 0x040057A4 RID: 22436
		public GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.FetchSubState fetch;

		// Token: 0x040057A5 RID: 22437
		public EquipChore.States.Equip equip;

		// Token: 0x02001FD0 RID: 8144
		public class Equip : GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.State
		{
			// Token: 0x04008F37 RID: 36663
			public GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.State pre;

			// Token: 0x04008F38 RID: 36664
			public GameStateMachine<EquipChore.States, EquipChore.StatesInstance, EquipChore, object>.State pst;
		}
	}
}
