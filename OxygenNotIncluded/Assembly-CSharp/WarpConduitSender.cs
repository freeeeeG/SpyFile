using System;
using UnityEngine;

// Token: 0x020006B7 RID: 1719
public class WarpConduitSender : StateMachineComponent<WarpConduitSender.StatesInstance>, ISecondaryInput
{
	// Token: 0x06002EC2 RID: 11970 RVA: 0x000F7060 File Offset: 0x000F5260
	private bool IsSending()
	{
		return base.smi.master.gasPort.IsOn() || base.smi.master.liquidPort.IsOn() || base.smi.master.solidPort.IsOn();
	}

	// Token: 0x06002EC3 RID: 11971 RVA: 0x000F70B4 File Offset: 0x000F52B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Storage[] components = base.GetComponents<Storage>();
		this.gasStorage = components[0];
		this.liquidStorage = components[1];
		this.solidStorage = components[2];
		this.gasPort = new WarpConduitSender.ConduitPort(base.gameObject, this.gasPortInfo, 1, this.gasStorage);
		this.liquidPort = new WarpConduitSender.ConduitPort(base.gameObject, this.liquidPortInfo, 2, this.liquidStorage);
		this.solidPort = new WarpConduitSender.ConduitPort(base.gameObject, this.solidPortInfo, 3, this.solidStorage);
		Vector3 position = this.liquidPort.airlock.gameObject.transform.position;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().transform.position = position + new Vector3(0f, 0f, -0.1f);
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = false;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = true;
		this.FindPartner();
		WarpConduitStatus.UpdateWarpConduitsOperational(base.gameObject, (this.receiver != null) ? this.receiver.gameObject : null);
		base.smi.StartSM();
	}

	// Token: 0x06002EC4 RID: 11972 RVA: 0x000F7205 File Offset: 0x000F5405
	public void OnActivatedChanged(object data)
	{
		WarpConduitStatus.UpdateWarpConduitsOperational(base.gameObject, (this.receiver != null) ? this.receiver.gameObject : null);
	}

	// Token: 0x06002EC5 RID: 11973 RVA: 0x000F7230 File Offset: 0x000F5430
	private void FindPartner()
	{
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag("WarpConduitReceiver");
		foreach (WarpConduitReceiver component in UnityEngine.Object.FindObjectsOfType<WarpConduitReceiver>())
		{
			if (component.GetMyWorldId() != this.GetMyWorldId())
			{
				this.receiver = component;
				break;
			}
		}
		if (this.receiver == null)
		{
			global::Debug.LogWarning("No warp conduit receiver found - maybe POI stomping or failure to spawn?");
			return;
		}
		this.receiver.SetStorage(this.gasStorage, this.liquidStorage, this.solidStorage);
	}

	// Token: 0x06002EC6 RID: 11974 RVA: 0x000F72B8 File Offset: 0x000F54B8
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidPort.inputCell, this.liquidPort.networkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasPort.inputCell, this.gasPort.networkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidPort.inputCell, this.solidPort.solidConsumer, true);
		base.OnCleanUp();
	}

	// Token: 0x06002EC7 RID: 11975 RVA: 0x000F7349 File Offset: 0x000F5549
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.liquidPortInfo.conduitType == type || this.gasPortInfo.conduitType == type || this.solidPortInfo.conduitType == type;
	}

	// Token: 0x06002EC8 RID: 11976 RVA: 0x000F7378 File Offset: 0x000F5578
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.liquidPortInfo.conduitType == type)
		{
			return this.liquidPortInfo.offset;
		}
		if (this.gasPortInfo.conduitType == type)
		{
			return this.gasPortInfo.offset;
		}
		if (this.solidPortInfo.conduitType == type)
		{
			return this.solidPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04001BB1 RID: 7089
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001BB2 RID: 7090
	public Storage gasStorage;

	// Token: 0x04001BB3 RID: 7091
	public Storage liquidStorage;

	// Token: 0x04001BB4 RID: 7092
	public Storage solidStorage;

	// Token: 0x04001BB5 RID: 7093
	public WarpConduitReceiver receiver;

	// Token: 0x04001BB6 RID: 7094
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x04001BB7 RID: 7095
	private WarpConduitSender.ConduitPort liquidPort;

	// Token: 0x04001BB8 RID: 7096
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x04001BB9 RID: 7097
	private WarpConduitSender.ConduitPort gasPort;

	// Token: 0x04001BBA RID: 7098
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x04001BBB RID: 7099
	private WarpConduitSender.ConduitPort solidPort;

	// Token: 0x020013F5 RID: 5109
	private class ConduitPort
	{
		// Token: 0x060082EE RID: 33518 RVA: 0x002FB938 File Offset: 0x002F9B38
		public ConduitPort(GameObject parent, ConduitPortInfo info, int number, Storage targetStorage)
		{
			this.portInfo = info;
			this.inputCell = Grid.OffsetCell(Grid.PosToCell(parent), this.portInfo.offset);
			if (this.portInfo.conduitType != ConduitType.Solid)
			{
				ConduitConsumer conduitConsumer = parent.AddComponent<ConduitConsumer>();
				conduitConsumer.conduitType = this.portInfo.conduitType;
				conduitConsumer.useSecondaryInput = true;
				conduitConsumer.storage = targetStorage;
				conduitConsumer.capacityKG = targetStorage.capacityKg;
				conduitConsumer.alwaysConsume = false;
				this.conduitConsumer = conduitConsumer;
				this.conduitConsumer.keepZeroMassObject = false;
				IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
				this.networkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.inputCell, parent);
				networkManager.AddToNetworks(this.inputCell, this.networkItem, true);
			}
			else
			{
				this.solidConsumer = parent.AddComponent<SolidConduitConsumer>();
				this.solidConsumer.useSecondaryInput = true;
				this.solidConsumer.storage = targetStorage;
				this.networkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Sink, this.inputCell, parent);
				Game.Instance.solidConduitSystem.AddToNetworks(this.inputCell, this.networkItem, true);
			}
			string meter_animation = "airlock_" + number.ToString();
			string text = "airlock_target_" + number.ToString();
			this.pre = "airlock_" + number.ToString() + "_pre";
			this.loop = "airlock_" + number.ToString() + "_loop";
			this.pst = "airlock_" + number.ToString() + "_pst";
			this.airlock = new MeterController(parent.GetComponent<KBatchedAnimController>(), text, meter_animation, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				text
			});
		}

		// Token: 0x060082EF RID: 33519 RVA: 0x002FBAFC File Offset: 0x002F9CFC
		public bool IsOn()
		{
			if (this.solidConsumer != null)
			{
				return this.solidConsumer.IsConsuming;
			}
			return this.conduitConsumer != null && (this.conduitConsumer.IsConnected && this.conduitConsumer.IsSatisfied) && this.conduitConsumer.consumedLastTick;
		}

		// Token: 0x060082F0 RID: 33520 RVA: 0x002FBB5C File Offset: 0x002F9D5C
		public void Update()
		{
			bool flag = this.IsOn();
			if (flag != this.open)
			{
				this.open = flag;
				if (this.open)
				{
					this.airlock.meterController.Play(this.pre, KAnim.PlayMode.Once, 1f, 0f);
					this.airlock.meterController.Queue(this.loop, KAnim.PlayMode.Loop, 1f, 0f);
					return;
				}
				this.airlock.meterController.Play(this.pst, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x040063E6 RID: 25574
		public ConduitPortInfo portInfo;

		// Token: 0x040063E7 RID: 25575
		public int inputCell;

		// Token: 0x040063E8 RID: 25576
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x040063E9 RID: 25577
		private ConduitConsumer conduitConsumer;

		// Token: 0x040063EA RID: 25578
		public SolidConduitConsumer solidConsumer;

		// Token: 0x040063EB RID: 25579
		public MeterController airlock;

		// Token: 0x040063EC RID: 25580
		private bool open;

		// Token: 0x040063ED RID: 25581
		private string pre;

		// Token: 0x040063EE RID: 25582
		private string loop;

		// Token: 0x040063EF RID: 25583
		private string pst;
	}

	// Token: 0x020013F6 RID: 5110
	public class StatesInstance : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.GameInstance
	{
		// Token: 0x060082F1 RID: 33521 RVA: 0x002FBBFE File Offset: 0x002F9DFE
		public StatesInstance(WarpConduitSender smi) : base(smi)
		{
		}
	}

	// Token: 0x020013F7 RID: 5111
	public class States : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender>
	{
		// Token: 0x060082F2 RID: 33522 RVA: 0x002FBC08 File Offset: 0x002F9E08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventHandler(GameHashes.BuildingActivated, delegate(WarpConduitSender.StatesInstance smi, object data)
			{
				smi.master.OnActivatedChanged(data);
			});
			this.off.PlayAnim("off").Enter(delegate(WarpConduitSender.StatesInstance smi)
			{
				smi.master.gasPort.Update();
				smi.master.liquidPort.Update();
				smi.master.solidPort.Update();
			}).EventTransition(GameHashes.OperationalChanged, this.on, (WarpConduitSender.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.waiting).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				smi.master.gasPort.Update();
				smi.master.liquidPort.Update();
				smi.master.solidPort.Update();
			}, UpdateRate.SIM_1000ms, false);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				if (!smi.master.IsSending())
				{
					smi.GoTo(this.on.waiting);
				}
			}, UpdateRate.SIM_1000ms, false).Exit(delegate(WarpConduitSender.StatesInstance smi)
			{
				smi.Play("working_pst", KAnim.PlayMode.Once);
			});
			this.on.waiting.QueueAnim("idle", false, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				if (smi.master.IsSending())
				{
					smi.GoTo(this.on.working);
				}
			}, UpdateRate.SIM_1000ms, false);
		}

		// Token: 0x040063F0 RID: 25584
		public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State off;

		// Token: 0x040063F1 RID: 25585
		public WarpConduitSender.States.onStates on;

		// Token: 0x02002144 RID: 8516
		public class onStates : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State
		{
			// Token: 0x04009532 RID: 38194
			public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State working;

			// Token: 0x04009533 RID: 38195
			public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State waiting;
		}
	}
}
