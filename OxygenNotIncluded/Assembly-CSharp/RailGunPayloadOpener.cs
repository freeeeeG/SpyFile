using System;
using UnityEngine;

// Token: 0x02000678 RID: 1656
public class RailGunPayloadOpener : StateMachineComponent<RailGunPayloadOpener.StatesInstance>, ISecondaryOutput
{
	// Token: 0x06002BFC RID: 11260 RVA: 0x000E9B08 File Offset: 0x000E7D08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.gasOutputCell = Grid.OffsetCell(Grid.PosToCell(this), this.gasPortInfo.offset);
		this.gasDispenser = this.CreateConduitDispenser(ConduitType.Gas, this.gasOutputCell, out this.gasNetworkItem);
		this.liquidOutputCell = Grid.OffsetCell(Grid.PosToCell(this), this.liquidPortInfo.offset);
		this.liquidDispenser = this.CreateConduitDispenser(ConduitType.Liquid, this.liquidOutputCell, out this.liquidNetworkItem);
		this.solidOutputCell = Grid.OffsetCell(Grid.PosToCell(this), this.solidPortInfo.offset);
		this.solidDispenser = this.CreateSolidConduitDispenser(this.solidOutputCell, out this.solidNetworkItem);
		this.deliveryComponents = base.GetComponents<ManualDeliveryKG>();
		this.payloadStorage.gunTargetOffset = new Vector2(-1f, 1.5f);
		this.payloadMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_storage_target", "meter_storage", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.smi.StartSM();
	}

	// Token: 0x06002BFD RID: 11261 RVA: 0x000E9C10 File Offset: 0x000E7E10
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidOutputCell, this.liquidNetworkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasOutputCell, this.gasNetworkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidOutputCell, this.solidDispenser, true);
		base.OnCleanUp();
	}

	// Token: 0x06002BFE RID: 11262 RVA: 0x000E9C84 File Offset: 0x000E7E84
	private ConduitDispenser CreateConduitDispenser(ConduitType outputType, int outputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		ConduitDispenser conduitDispenser = base.gameObject.AddComponent<ConduitDispenser>();
		conduitDispenser.conduitType = outputType;
		conduitDispenser.useSecondaryOutput = true;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.storage = this.resourceStorage;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(outputType);
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(outputType, Endpoint.Source, outputCell, base.gameObject);
		networkManager.AddToNetworks(outputCell, flowNetworkItem, true);
		return conduitDispenser;
	}

	// Token: 0x06002BFF RID: 11263 RVA: 0x000E9CDC File Offset: 0x000E7EDC
	private SolidConduitDispenser CreateSolidConduitDispenser(int outputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		SolidConduitDispenser solidConduitDispenser = base.gameObject.AddComponent<SolidConduitDispenser>();
		solidConduitDispenser.storage = this.resourceStorage;
		solidConduitDispenser.alwaysDispense = true;
		solidConduitDispenser.useSecondaryOutput = true;
		solidConduitDispenser.solidOnly = true;
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Source, outputCell, base.gameObject);
		Game.Instance.solidConduitSystem.AddToNetworks(outputCell, flowNetworkItem, true);
		return solidConduitDispenser;
	}

	// Token: 0x06002C00 RID: 11264 RVA: 0x000E9D38 File Offset: 0x000E7F38
	public void EmptyPayload()
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null && component.items.Count > 0)
		{
			GameObject gameObject = this.payloadStorage.items[0];
			gameObject.GetComponent<Storage>().Transfer(this.resourceStorage, false, false);
			Util.KDestroyGameObject(gameObject);
			component.ConsumeIgnoringDisease(this.payloadStorage.items[0]);
		}
	}

	// Token: 0x06002C01 RID: 11265 RVA: 0x000E9DA4 File Offset: 0x000E7FA4
	public bool PowerOperationalChanged()
	{
		EnergyConsumer component = base.GetComponent<EnergyConsumer>();
		return component != null && component.IsPowered;
	}

	// Token: 0x06002C02 RID: 11266 RVA: 0x000E9DC9 File Offset: 0x000E7FC9
	bool ISecondaryOutput.HasSecondaryConduitType(ConduitType type)
	{
		return type == this.gasPortInfo.conduitType || type == this.liquidPortInfo.conduitType || type == this.solidPortInfo.conduitType;
	}

	// Token: 0x06002C03 RID: 11267 RVA: 0x000E9DF8 File Offset: 0x000E7FF8
	CellOffset ISecondaryOutput.GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.gasPortInfo.conduitType)
		{
			return this.gasPortInfo.offset;
		}
		if (type == this.liquidPortInfo.conduitType)
		{
			return this.liquidPortInfo.offset;
		}
		if (type != this.solidPortInfo.conduitType)
		{
			return CellOffset.none;
		}
		return this.solidPortInfo.offset;
	}

	// Token: 0x040019ED RID: 6637
	public static float delivery_time = 10f;

	// Token: 0x040019EE RID: 6638
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x040019EF RID: 6639
	private int liquidOutputCell = -1;

	// Token: 0x040019F0 RID: 6640
	private FlowUtilityNetwork.NetworkItem liquidNetworkItem;

	// Token: 0x040019F1 RID: 6641
	private ConduitDispenser liquidDispenser;

	// Token: 0x040019F2 RID: 6642
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x040019F3 RID: 6643
	private int gasOutputCell = -1;

	// Token: 0x040019F4 RID: 6644
	private FlowUtilityNetwork.NetworkItem gasNetworkItem;

	// Token: 0x040019F5 RID: 6645
	private ConduitDispenser gasDispenser;

	// Token: 0x040019F6 RID: 6646
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x040019F7 RID: 6647
	private int solidOutputCell = -1;

	// Token: 0x040019F8 RID: 6648
	private FlowUtilityNetwork.NetworkItem solidNetworkItem;

	// Token: 0x040019F9 RID: 6649
	private SolidConduitDispenser solidDispenser;

	// Token: 0x040019FA RID: 6650
	public Storage payloadStorage;

	// Token: 0x040019FB RID: 6651
	public Storage resourceStorage;

	// Token: 0x040019FC RID: 6652
	private ManualDeliveryKG[] deliveryComponents;

	// Token: 0x040019FD RID: 6653
	private MeterController payloadMeter;

	// Token: 0x02001378 RID: 4984
	public class StatesInstance : GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.GameInstance
	{
		// Token: 0x06008127 RID: 33063 RVA: 0x002F3F1E File Offset: 0x002F211E
		public StatesInstance(RailGunPayloadOpener master) : base(master)
		{
		}

		// Token: 0x06008128 RID: 33064 RVA: 0x002F3F27 File Offset: 0x002F2127
		public bool HasPayload()
		{
			return base.smi.master.payloadStorage.items.Count > 0;
		}

		// Token: 0x06008129 RID: 33065 RVA: 0x002F3F46 File Offset: 0x002F2146
		public bool HasResources()
		{
			return base.smi.master.resourceStorage.MassStored() > 0f;
		}
	}

	// Token: 0x02001379 RID: 4985
	public class States : GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener>
	{
		// Token: 0x0600812A RID: 33066 RVA: 0x002F3F64 File Offset: 0x002F2164
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.unoperational.PlayAnim("off").EventTransition(GameHashes.OperationalFlagChanged, this.operational, (RailGunPayloadOpener.StatesInstance smi) => smi.master.PowerOperationalChanged()).Enter(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, true);
				smi.GetComponent<ManualDeliveryKG>().Pause(true, "no_power");
			});
			this.operational.Enter(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.GetComponent<ManualDeliveryKG>().Pause(false, "power");
			}).EventTransition(GameHashes.OperationalFlagChanged, this.unoperational, (RailGunPayloadOpener.StatesInstance smi) => !smi.master.PowerOperationalChanged()).DefaultState(this.operational.idle).EventHandler(GameHashes.OnStorageChange, delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.master.payloadMeter.SetPositionPercent(Mathf.Clamp01((float)smi.master.payloadStorage.items.Count / smi.master.payloadStorage.capacityKg));
			});
			this.operational.idle.PlayAnim("on").EventTransition(GameHashes.OnStorageChange, this.operational.pre, (RailGunPayloadOpener.StatesInstance smi) => smi.HasPayload());
			this.operational.pre.Enter(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, true);
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.operational.loop);
			this.operational.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(10f, this.operational.pst);
			this.operational.pst.PlayAnim("working_pst").Exit(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.master.EmptyPayload();
				smi.GetComponent<Operational>().SetActive(false, true);
			}).OnAnimQueueComplete(this.operational.idle);
		}

		// Token: 0x04006287 RID: 25223
		public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State unoperational;

		// Token: 0x04006288 RID: 25224
		public RailGunPayloadOpener.States.OperationalStates operational;

		// Token: 0x0200210F RID: 8463
		public class OperationalStates : GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State
		{
			// Token: 0x04009428 RID: 37928
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State idle;

			// Token: 0x04009429 RID: 37929
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State pre;

			// Token: 0x0400942A RID: 37930
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State loop;

			// Token: 0x0400942B RID: 37931
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State pst;
		}
	}
}
