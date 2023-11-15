using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000687 RID: 1671
[SerializationConfig(MemberSerialization.OptIn)]
public class RustDeoxidizer : StateMachineComponent<RustDeoxidizer.StatesInstance>
{
	// Token: 0x06002CA4 RID: 11428 RVA: 0x000ED58D File Offset: 0x000EB78D
	protected override void OnSpawn()
	{
		base.smi.StartSM();
		Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
	}

	// Token: 0x06002CA5 RID: 11429 RVA: 0x000ED5AF File Offset: 0x000EB7AF
	protected override void OnCleanUp()
	{
		Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06002CA6 RID: 11430 RVA: 0x000ED5D0 File Offset: 0x000EB7D0
	private bool RoomForPressure
	{
		get
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			num = Grid.CellAbove(num);
			return !GameUtil.FloodFillCheck<RustDeoxidizer>(new Func<int, RustDeoxidizer, bool>(RustDeoxidizer.OverPressure), this, num, 3, true, true);
		}
	}

	// Token: 0x06002CA7 RID: 11431 RVA: 0x000ED60E File Offset: 0x000EB80E
	private static bool OverPressure(int cell, RustDeoxidizer rustDeoxidizer)
	{
		return Grid.Mass[cell] > rustDeoxidizer.maxMass;
	}

	// Token: 0x04001A4B RID: 6731
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x04001A4C RID: 6732
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001A4D RID: 6733
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x04001A4E RID: 6734
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001A4F RID: 6735
	private MeterController meter;

	// Token: 0x02001397 RID: 5015
	public class StatesInstance : GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.GameInstance
	{
		// Token: 0x060081B4 RID: 33204 RVA: 0x002F6216 File Offset: 0x002F4416
		public StatesInstance(RustDeoxidizer smi) : base(smi)
		{
		}
	}

	// Token: 0x02001398 RID: 5016
	public class States : GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer>
	{
		// Token: 0x060081B5 RID: 33205 RVA: 0x002F6220 File Offset: 0x002F4420
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (RustDeoxidizer.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (RustDeoxidizer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(RustDeoxidizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (RustDeoxidizer.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(RustDeoxidizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (RustDeoxidizer.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).Transition(this.overpressure, (RustDeoxidizer.StatesInstance smi) => !smi.master.RoomForPressure, UpdateRate.SIM_200ms);
			this.overpressure.Enter("OverPressure", delegate(RustDeoxidizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null).Transition(this.converting, (RustDeoxidizer.StatesInstance smi) => smi.master.RoomForPressure, UpdateRate.SIM_200ms);
		}

		// Token: 0x040062E9 RID: 25321
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State disabled;

		// Token: 0x040062EA RID: 25322
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State waiting;

		// Token: 0x040062EB RID: 25323
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State converting;

		// Token: 0x040062EC RID: 25324
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State overpressure;
	}
}
