using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000803 RID: 2051
[SerializationConfig(MemberSerialization.OptIn)]
public class IceMachine : StateMachineComponent<IceMachine.StatesInstance>
{
	// Token: 0x06003A78 RID: 14968 RVA: 0x00144F78 File Offset: 0x00143178
	public void SetStorages(Storage waterStorage, Storage iceStorage)
	{
		this.waterStorage = waterStorage;
		this.iceStorage = iceStorage;
	}

	// Token: 0x06003A79 RID: 14969 RVA: 0x00144F88 File Offset: 0x00143188
	private bool CanMakeIce()
	{
		bool flag = this.waterStorage != null && this.waterStorage.GetMassAvailable(SimHashes.Water) >= 0.1f;
		bool flag2 = this.iceStorage != null && this.iceStorage.IsFull();
		return flag && !flag2;
	}

	// Token: 0x06003A7A RID: 14970 RVA: 0x00144FE8 File Offset: 0x001431E8
	private void MakeIce(IceMachine.StatesInstance smi, float dt)
	{
		float num = this.heatRemovalRate * dt / (float)this.waterStorage.items.Count;
		foreach (GameObject gameObject in this.waterStorage.items)
		{
			GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), -num, smi.master.targetTemperature);
		}
		for (int i = this.waterStorage.items.Count; i > 0; i--)
		{
			GameObject gameObject2 = this.waterStorage.items[i - 1];
			if (gameObject2 && gameObject2.GetComponent<PrimaryElement>().Temperature < gameObject2.GetComponent<PrimaryElement>().Element.lowTemp)
			{
				PrimaryElement component = gameObject2.GetComponent<PrimaryElement>();
				this.waterStorage.AddOre(component.Element.lowTempTransitionTarget, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, true);
				this.waterStorage.ConsumeIgnoringDisease(gameObject2);
			}
		}
		smi.UpdateIceState();
	}

	// Token: 0x06003A7B RID: 14971 RVA: 0x00145114 File Offset: 0x00143314
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x040026E6 RID: 9958
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040026E7 RID: 9959
	public Storage waterStorage;

	// Token: 0x040026E8 RID: 9960
	public Storage iceStorage;

	// Token: 0x040026E9 RID: 9961
	public float targetTemperature;

	// Token: 0x040026EA RID: 9962
	public float heatRemovalRate;

	// Token: 0x040026EB RID: 9963
	private static StatusItem iceStorageFullStatusItem;

	// Token: 0x020015E1 RID: 5601
	public class StatesInstance : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.GameInstance
	{
		// Token: 0x060088C1 RID: 35009 RVA: 0x0030F82C File Offset: 0x0030DA2C
		public StatesInstance(IceMachine smi) : base(smi)
		{
			this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_OL",
				"meter_frame",
				"meter_fill"
			});
			this.UpdateMeter();
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		}

		// Token: 0x060088C2 RID: 35010 RVA: 0x0030F89E File Offset: 0x0030DA9E
		private void OnStorageChange(object data)
		{
			this.UpdateMeter();
		}

		// Token: 0x060088C3 RID: 35011 RVA: 0x0030F8A6 File Offset: 0x0030DAA6
		public void UpdateMeter()
		{
			this.meter.SetPositionPercent(Mathf.Clamp01(base.smi.master.iceStorage.MassStored() / base.smi.master.iceStorage.Capacity()));
		}

		// Token: 0x060088C4 RID: 35012 RVA: 0x0030F8E4 File Offset: 0x0030DAE4
		public void UpdateIceState()
		{
			bool value = false;
			for (int i = base.smi.master.waterStorage.items.Count; i > 0; i--)
			{
				GameObject gameObject = base.smi.master.waterStorage.items[i - 1];
				if (gameObject && gameObject.GetComponent<PrimaryElement>().Temperature <= base.smi.master.targetTemperature)
				{
					value = true;
				}
			}
			base.sm.doneFreezingIce.Set(value, this, false);
		}

		// Token: 0x040069CC RID: 27084
		private MeterController meter;

		// Token: 0x040069CD RID: 27085
		public Chore emptyChore;
	}

	// Token: 0x020015E2 RID: 5602
	public class States : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine>
	{
		// Token: 0x060088C5 RID: 35013 RVA: 0x0030F974 File Offset: 0x0030DB74
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (IceMachine.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (IceMachine.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (IceMachine.StatesInstance smi) => smi.master.CanMakeIce());
			this.on.working_pre.Enter(delegate(IceMachine.StatesInstance smi)
			{
				smi.UpdateIceState();
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.QueueAnim("working_loop", true, null).Update("UpdateWorking", delegate(IceMachine.StatesInstance smi, float dt)
			{
				smi.master.MakeIce(smi, dt);
			}, UpdateRate.SIM_200ms, false).ParamTransition<bool>(this.doneFreezingIce, this.on.working_pst, GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.IsTrue).Enter(delegate(IceMachine.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(true, "Working");
			}).Exit(delegate(IceMachine.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(false, "Done Working");
			});
			this.on.working_pst.Exit(new StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State.Callback(this.DoTransfer)).PlayAnim("working_pst").OnAnimQueueComplete(this.on);
		}

		// Token: 0x060088C6 RID: 35014 RVA: 0x0030FB84 File Offset: 0x0030DD84
		private void DoTransfer(IceMachine.StatesInstance smi)
		{
			for (int i = smi.master.waterStorage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = smi.master.waterStorage.items[i];
				if (gameObject && gameObject.GetComponent<PrimaryElement>().Temperature <= smi.master.targetTemperature)
				{
					smi.master.waterStorage.Transfer(gameObject, smi.master.iceStorage, false, true);
				}
			}
			smi.UpdateMeter();
		}

		// Token: 0x040069CE RID: 27086
		public StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.BoolParameter doneFreezingIce;

		// Token: 0x040069CF RID: 27087
		public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State off;

		// Token: 0x040069D0 RID: 27088
		public IceMachine.States.OnStates on;

		// Token: 0x02002198 RID: 8600
		public class OnStates : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State
		{
			// Token: 0x04009689 RID: 38537
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State waiting;

			// Token: 0x0400968A RID: 38538
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working_pre;

			// Token: 0x0400968B RID: 38539
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working;

			// Token: 0x0400968C RID: 38540
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working_pst;
		}
	}
}
