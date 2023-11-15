using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005F8 RID: 1528
[SerializationConfig(MemberSerialization.OptIn)]
public class Electrolyzer : StateMachineComponent<Electrolyzer.StatesInstance>
{
	// Token: 0x06002646 RID: 9798 RVA: 0x000CFFF8 File Offset: 0x000CE1F8
	protected override void OnSpawn()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.hasMeter)
		{
			this.meter = new MeterController(component, "U2H_meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new Vector3(-0.4f, 0.5f, -0.1f), new string[]
			{
				"U2H_meter_target",
				"U2H_meter_tank",
				"U2H_meter_waterbody",
				"U2H_meter_level"
			});
		}
		base.smi.StartSM();
		this.UpdateMeter();
		Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
	}

	// Token: 0x06002647 RID: 9799 RVA: 0x000D008D File Offset: 0x000CE28D
	protected override void OnCleanUp()
	{
		Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06002648 RID: 9800 RVA: 0x000D00AC File Offset: 0x000CE2AC
	public void UpdateMeter()
	{
		if (this.hasMeter)
		{
			float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06002649 RID: 9801 RVA: 0x000D00EC File Offset: 0x000CE2EC
	private bool RoomForPressure
	{
		get
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			num = Grid.OffsetCell(num, this.emissionOffset);
			return !GameUtil.FloodFillCheck<Electrolyzer>(new Func<int, Electrolyzer, bool>(Electrolyzer.OverPressure), this, num, 3, true, true);
		}
	}

	// Token: 0x0600264A RID: 9802 RVA: 0x000D0130 File Offset: 0x000CE330
	private static bool OverPressure(int cell, Electrolyzer electrolyzer)
	{
		return Grid.Mass[cell] > electrolyzer.maxMass;
	}

	// Token: 0x040015E3 RID: 5603
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x040015E4 RID: 5604
	[SerializeField]
	public bool hasMeter = true;

	// Token: 0x040015E5 RID: 5605
	[SerializeField]
	public CellOffset emissionOffset = CellOffset.none;

	// Token: 0x040015E6 RID: 5606
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x040015E7 RID: 5607
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x040015E8 RID: 5608
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040015E9 RID: 5609
	private MeterController meter;

	// Token: 0x0200129D RID: 4765
	public class StatesInstance : GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.GameInstance
	{
		// Token: 0x06007DFA RID: 32250 RVA: 0x002E619A File Offset: 0x002E439A
		public StatesInstance(Electrolyzer smi) : base(smi)
		{
		}
	}

	// Token: 0x0200129E RID: 4766
	public class States : GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer>
	{
		// Token: 0x06007DFB RID: 32251 RVA: 0x002E61A4 File Offset: 0x002E43A4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (Electrolyzer.StatesInstance smi) => !smi.master.operational.IsOperational).EventHandler(GameHashes.OnStorageChange, delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (Electrolyzer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (Electrolyzer.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (Electrolyzer.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).Transition(this.overpressure, (Electrolyzer.StatesInstance smi) => !smi.master.RoomForPressure, UpdateRate.SIM_200ms);
			this.overpressure.Enter("OverPressure", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null).Transition(this.converting, (Electrolyzer.StatesInstance smi) => smi.master.RoomForPressure, UpdateRate.SIM_200ms);
		}

		// Token: 0x0400602D RID: 24621
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State disabled;

		// Token: 0x0400602E RID: 24622
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State waiting;

		// Token: 0x0400602F RID: 24623
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State converting;

		// Token: 0x04006030 RID: 24624
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State overpressure;
	}
}
