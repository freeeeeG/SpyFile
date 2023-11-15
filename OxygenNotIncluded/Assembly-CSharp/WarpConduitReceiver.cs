using System;
using UnityEngine;

// Token: 0x020006B5 RID: 1717
public class WarpConduitReceiver : StateMachineComponent<WarpConduitReceiver.StatesInstance>, ISecondaryOutput
{
	// Token: 0x06002EB7 RID: 11959 RVA: 0x000F6B88 File Offset: 0x000F4D88
	private bool IsReceiving()
	{
		return base.smi.master.gasPort.IsOn() || base.smi.master.liquidPort.IsOn() || base.smi.master.solidPort.IsOn();
	}

	// Token: 0x06002EB8 RID: 11960 RVA: 0x000F6BDA File Offset: 0x000F4DDA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FindPartner();
		base.smi.StartSM();
	}

	// Token: 0x06002EB9 RID: 11961 RVA: 0x000F6BF4 File Offset: 0x000F4DF4
	private void FindPartner()
	{
		if (this.senderGasStorage != null)
		{
			return;
		}
		WarpConduitSender warpConduitSender = null;
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag("WarpConduitSender");
		foreach (WarpConduitSender warpConduitSender2 in UnityEngine.Object.FindObjectsOfType<WarpConduitSender>())
		{
			if (warpConduitSender2.GetMyWorldId() != this.GetMyWorldId())
			{
				warpConduitSender = warpConduitSender2;
				break;
			}
		}
		if (warpConduitSender == null)
		{
			global::Debug.LogWarning("No warp conduit sender found - maybe POI stomping or failure to spawn?");
			return;
		}
		this.SetStorage(warpConduitSender.gasStorage, warpConduitSender.liquidStorage, warpConduitSender.solidStorage);
		WarpConduitStatus.UpdateWarpConduitsOperational(warpConduitSender.gameObject, base.gameObject);
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x000F6C90 File Offset: 0x000F4E90
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidPort.outputCell, this.liquidPort.networkItem, true);
		if (this.gasPort.portInfo != null)
		{
			Conduit.GetNetworkManager(this.gasPort.portInfo.conduitType).RemoveFromNetworks(this.gasPort.outputCell, this.gasPort.networkItem, true);
		}
		else
		{
			global::Debug.LogWarning("Conduit Receiver gasPort portInfo is null in OnCleanUp");
		}
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidPort.outputCell, this.solidPort.networkItem, true);
		base.OnCleanUp();
	}

	// Token: 0x06002EBB RID: 11963 RVA: 0x000F6D3F File Offset: 0x000F4F3F
	public void OnActivatedChanged(object data)
	{
		if (this.senderGasStorage == null)
		{
			this.FindPartner();
		}
		WarpConduitStatus.UpdateWarpConduitsOperational((this.senderGasStorage != null) ? this.senderGasStorage.gameObject : null, base.gameObject);
	}

	// Token: 0x06002EBC RID: 11964 RVA: 0x000F6D7C File Offset: 0x000F4F7C
	public void SetStorage(Storage gasStorage, Storage liquidStorage, Storage solidStorage)
	{
		this.senderGasStorage = gasStorage;
		this.senderLiquidStorage = liquidStorage;
		this.senderSolidStorage = solidStorage;
		this.gasPort.SetPortInfo(base.gameObject, this.gasPortInfo, gasStorage, 1);
		this.liquidPort.SetPortInfo(base.gameObject, this.liquidPortInfo, liquidStorage, 2);
		this.solidPort.SetPortInfo(base.gameObject, this.solidPortInfo, solidStorage, 3);
		Vector3 position = this.liquidPort.airlock.gameObject.transform.position;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().transform.position = position + new Vector3(0f, 0f, -0.1f);
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = false;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = true;
	}

	// Token: 0x06002EBD RID: 11965 RVA: 0x000F6E73 File Offset: 0x000F5073
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return type == this.gasPortInfo.conduitType || type == this.liquidPortInfo.conduitType || type == this.solidPortInfo.conduitType;
	}

	// Token: 0x06002EBE RID: 11966 RVA: 0x000F6EA4 File Offset: 0x000F50A4
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.gasPortInfo.conduitType)
		{
			return this.gasPortInfo.offset;
		}
		if (type == this.liquidPortInfo.conduitType)
		{
			return this.liquidPortInfo.offset;
		}
		if (type == this.solidPortInfo.conduitType)
		{
			return this.solidPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04001BA7 RID: 7079
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x04001BA8 RID: 7080
	private WarpConduitReceiver.ConduitPort liquidPort;

	// Token: 0x04001BA9 RID: 7081
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x04001BAA RID: 7082
	private WarpConduitReceiver.ConduitPort solidPort;

	// Token: 0x04001BAB RID: 7083
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x04001BAC RID: 7084
	private WarpConduitReceiver.ConduitPort gasPort;

	// Token: 0x04001BAD RID: 7085
	public Storage senderGasStorage;

	// Token: 0x04001BAE RID: 7086
	public Storage senderLiquidStorage;

	// Token: 0x04001BAF RID: 7087
	public Storage senderSolidStorage;

	// Token: 0x020013F2 RID: 5106
	public struct ConduitPort
	{
		// Token: 0x060082E6 RID: 33510 RVA: 0x002FB4AC File Offset: 0x002F96AC
		public void SetPortInfo(GameObject parent, ConduitPortInfo info, Storage senderStorage, int number)
		{
			this.portInfo = info;
			this.outputCell = Grid.OffsetCell(Grid.PosToCell(parent), this.portInfo.offset);
			if (this.portInfo.conduitType != ConduitType.Solid)
			{
				ConduitDispenser conduitDispenser = parent.AddComponent<ConduitDispenser>();
				conduitDispenser.conduitType = this.portInfo.conduitType;
				conduitDispenser.useSecondaryOutput = true;
				conduitDispenser.alwaysDispense = true;
				conduitDispenser.storage = senderStorage;
				this.dispenser = conduitDispenser;
				IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
				this.networkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Source, this.outputCell, parent);
				networkManager.AddToNetworks(this.outputCell, this.networkItem, true);
			}
			else
			{
				SolidConduitDispenser solidConduitDispenser = parent.AddComponent<SolidConduitDispenser>();
				solidConduitDispenser.storage = senderStorage;
				solidConduitDispenser.alwaysDispense = true;
				solidConduitDispenser.useSecondaryOutput = true;
				solidConduitDispenser.solidOnly = true;
				this.solidDispenser = solidConduitDispenser;
				this.networkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Source, this.outputCell, parent);
				Game.Instance.solidConduitSystem.AddToNetworks(this.outputCell, this.networkItem, true);
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

		// Token: 0x060082E7 RID: 33511 RVA: 0x002FB654 File Offset: 0x002F9854
		public bool IsOn()
		{
			if (this.solidDispenser != null)
			{
				return this.solidDispenser.IsDispensing;
			}
			return this.dispenser != null && !this.dispenser.blocked && !this.dispenser.empty;
		}

		// Token: 0x060082E8 RID: 33512 RVA: 0x002FB6A8 File Offset: 0x002F98A8
		public void UpdatePortAnim()
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

		// Token: 0x040063DA RID: 25562
		public ConduitPortInfo portInfo;

		// Token: 0x040063DB RID: 25563
		public int outputCell;

		// Token: 0x040063DC RID: 25564
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x040063DD RID: 25565
		public ConduitDispenser dispenser;

		// Token: 0x040063DE RID: 25566
		public SolidConduitDispenser solidDispenser;

		// Token: 0x040063DF RID: 25567
		public MeterController airlock;

		// Token: 0x040063E0 RID: 25568
		private bool open;

		// Token: 0x040063E1 RID: 25569
		private string pre;

		// Token: 0x040063E2 RID: 25570
		private string loop;

		// Token: 0x040063E3 RID: 25571
		private string pst;
	}

	// Token: 0x020013F3 RID: 5107
	public class StatesInstance : GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.GameInstance
	{
		// Token: 0x060082E9 RID: 33513 RVA: 0x002FB74A File Offset: 0x002F994A
		public StatesInstance(WarpConduitReceiver master) : base(master)
		{
		}
	}

	// Token: 0x020013F4 RID: 5108
	public class States : GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver>
	{
		// Token: 0x060082EA RID: 33514 RVA: 0x002FB754 File Offset: 0x002F9954
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventHandler(GameHashes.BuildingActivated, delegate(WarpConduitReceiver.StatesInstance smi, object data)
			{
				smi.master.OnActivatedChanged(data);
			});
			this.off.PlayAnim("off").Enter(delegate(WarpConduitReceiver.StatesInstance smi)
			{
				smi.master.gasPort.UpdatePortAnim();
				smi.master.liquidPort.UpdatePortAnim();
				smi.master.solidPort.UpdatePortAnim();
			}).EventTransition(GameHashes.OperationalFlagChanged, this.on, (WarpConduitReceiver.StatesInstance smi) => smi.GetComponent<Operational>().GetFlag(WarpConduitStatus.warpConnectedFlag));
			this.on.DefaultState(this.on.idle).Update(delegate(WarpConduitReceiver.StatesInstance smi, float dt)
			{
				smi.master.gasPort.UpdatePortAnim();
				smi.master.liquidPort.UpdatePortAnim();
				smi.master.solidPort.UpdatePortAnim();
			}, UpdateRate.SIM_1000ms, false);
			this.on.idle.QueueAnim("idle", false, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).Update(delegate(WarpConduitReceiver.StatesInstance smi, float dt)
			{
				if (smi.master.IsReceiving())
				{
					smi.GoTo(this.on.working);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).Update(delegate(WarpConduitReceiver.StatesInstance smi, float dt)
			{
				if (!smi.master.IsReceiving())
				{
					smi.GoTo(this.on.idle);
				}
			}, UpdateRate.SIM_1000ms, false).Exit(delegate(WarpConduitReceiver.StatesInstance smi)
			{
				smi.Play("working_pst", KAnim.PlayMode.Once);
			});
		}

		// Token: 0x040063E4 RID: 25572
		public GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State off;

		// Token: 0x040063E5 RID: 25573
		public WarpConduitReceiver.States.onStates on;

		// Token: 0x02002142 RID: 8514
		public class onStates : GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State
		{
			// Token: 0x0400952A RID: 38186
			public GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State working;

			// Token: 0x0400952B RID: 38187
			public GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State idle;
		}
	}
}
