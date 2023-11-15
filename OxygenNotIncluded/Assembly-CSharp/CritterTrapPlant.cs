using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008E0 RID: 2272
public class CritterTrapPlant : StateMachineComponent<CritterTrapPlant.StatesInstance>
{
	// Token: 0x060041B7 RID: 16823 RVA: 0x0016FD08 File Offset: 0x0016DF08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.master.growing.enabled = false;
		base.Subscribe<CritterTrapPlant>(-216549700, CritterTrapPlant.OnUprootedDelegate);
		base.smi.StartSM();
	}

	// Token: 0x060041B8 RID: 16824 RVA: 0x0016FD42 File Offset: 0x0016DF42
	public void RefreshPositionPercent()
	{
		this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
	}

	// Token: 0x060041B9 RID: 16825 RVA: 0x0016FD5C File Offset: 0x0016DF5C
	private void OnUprooted(object data = null)
	{
		GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), base.gameObject.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
		base.gameObject.Trigger(1623392196, null);
		base.gameObject.GetComponent<KBatchedAnimController>().StopAndClear();
		UnityEngine.Object.Destroy(base.gameObject.GetComponent<KBatchedAnimController>());
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060041BA RID: 16826 RVA: 0x0016FDD3 File Offset: 0x0016DFD3
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060041BB RID: 16827 RVA: 0x0016FDEC File Offset: 0x0016DFEC
	public Notification CreateDeathNotification()
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x04002AD2 RID: 10962
	[MyCmpReq]
	private Crop crop;

	// Token: 0x04002AD3 RID: 10963
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04002AD4 RID: 10964
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x04002AD5 RID: 10965
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04002AD6 RID: 10966
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04002AD7 RID: 10967
	[MyCmpReq]
	private Harvestable harvestable;

	// Token: 0x04002AD8 RID: 10968
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04002AD9 RID: 10969
	public float gasOutputRate;

	// Token: 0x04002ADA RID: 10970
	public float gasVentThreshold;

	// Token: 0x04002ADB RID: 10971
	public SimHashes outputElement;

	// Token: 0x04002ADC RID: 10972
	private float GAS_TEMPERATURE_DELTA = 10f;

	// Token: 0x04002ADD RID: 10973
	private static readonly EventSystem.IntraObjectHandler<CritterTrapPlant> OnUprootedDelegate = new EventSystem.IntraObjectHandler<CritterTrapPlant>(delegate(CritterTrapPlant component, object data)
	{
		component.OnUprooted(data);
	});

	// Token: 0x02001720 RID: 5920
	public class StatesInstance : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.GameInstance
	{
		// Token: 0x06008D87 RID: 36231 RVA: 0x0031BFB0 File Offset: 0x0031A1B0
		public StatesInstance(CritterTrapPlant master) : base(master)
		{
		}

		// Token: 0x06008D88 RID: 36232 RVA: 0x0031BFB9 File Offset: 0x0031A1B9
		public void OnTrapTriggered(object data)
		{
			base.smi.sm.trapTriggered.Trigger(base.smi);
		}

		// Token: 0x06008D89 RID: 36233 RVA: 0x0031BFD8 File Offset: 0x0031A1D8
		public void AddGas(float dt)
		{
			float temperature = base.smi.GetComponent<PrimaryElement>().Temperature + base.smi.master.GAS_TEMPERATURE_DELTA;
			base.smi.master.storage.AddGasChunk(base.smi.master.outputElement, base.smi.master.gasOutputRate * dt, temperature, byte.MaxValue, 0, false, true);
			if (this.ShouldVentGas())
			{
				base.smi.sm.ventGas.Trigger(base.smi);
			}
		}

		// Token: 0x06008D8A RID: 36234 RVA: 0x0031C06C File Offset: 0x0031A26C
		public void VentGas()
		{
			PrimaryElement primaryElement = base.smi.master.storage.FindPrimaryElement(base.smi.master.outputElement);
			if (primaryElement != null)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.smi.transform.GetPosition()), primaryElement.ElementID, CellEventLogger.Instance.Dumpable, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount, true, -1);
				base.smi.master.storage.ConsumeIgnoringDisease(primaryElement.gameObject);
			}
		}

		// Token: 0x06008D8B RID: 36235 RVA: 0x0031C108 File Offset: 0x0031A308
		public bool ShouldVentGas()
		{
			PrimaryElement primaryElement = base.smi.master.storage.FindPrimaryElement(base.smi.master.outputElement);
			return !(primaryElement == null) && primaryElement.Mass >= base.smi.master.gasVentThreshold;
		}
	}

	// Token: 0x02001721 RID: 5921
	public class States : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant>
	{
		// Token: 0x06008D8C RID: 36236 RVA: 0x0031C164 File Offset: 0x0031A364
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.trap;
			this.trap.DefaultState(this.trap.open);
			this.trap.open.ToggleComponent<TrapTrigger>(false).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.VentGas();
				smi.master.storage.ConsumeAllIgnoringDisease();
			}).EventHandler(GameHashes.TrapTriggered, delegate(CritterTrapPlant.StatesInstance smi, object data)
			{
				smi.OnTrapTriggered(data);
			}).EventTransition(GameHashes.Wilt, this.trap.wilting, null).OnSignal(this.trapTriggered, this.trap.trigger).ParamTransition<bool>(this.hasEatenCreature, this.trap.digesting, GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.IsTrue).PlayAnim("idle_open", KAnim.PlayMode.Loop);
			this.trap.trigger.PlayAnim("trap", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.master.storage.ConsumeAllIgnoringDisease();
				smi.sm.hasEatenCreature.Set(true, smi, false);
			}).OnAnimQueueComplete(this.trap.digesting);
			this.trap.digesting.PlayAnim("digesting_loop", KAnim.PlayMode.Loop).ToggleComponent<Growing>(false).EventTransition(GameHashes.Grow, this.fruiting.enter, (CritterTrapPlant.StatesInstance smi) => smi.master.growing.ReachedNextHarvest()).EventTransition(GameHashes.Wilt, this.trap.wilting, null).DefaultState(this.trap.digesting.idle);
			this.trap.digesting.idle.PlayAnim("digesting_loop", KAnim.PlayMode.Loop).Update(delegate(CritterTrapPlant.StatesInstance smi, float dt)
			{
				smi.AddGas(dt);
			}, UpdateRate.SIM_4000ms, false).OnSignal(this.ventGas, this.trap.digesting.vent_pre);
			this.trap.digesting.vent_pre.PlayAnim("vent_pre").Exit(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.VentGas();
			}).OnAnimQueueComplete(this.trap.digesting.vent);
			this.trap.digesting.vent.PlayAnim("vent_loop", KAnim.PlayMode.Once).QueueAnim("vent_pst", false, null).OnAnimQueueComplete(this.trap.digesting.idle);
			this.trap.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.trap, (CritterTrapPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.fruiting.EventTransition(GameHashes.Wilt, this.fruiting.wilting, null).EventTransition(GameHashes.Harvest, this.harvest, null).DefaultState(this.fruiting.idle);
			this.fruiting.enter.PlayAnim("open_harvest", KAnim.PlayMode.Once).Exit(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.VentGas();
				smi.master.storage.ConsumeAllIgnoringDisease();
			}).OnAnimQueueComplete(this.fruiting.idle);
			this.fruiting.idle.PlayAnim("harvestable_loop", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).Transition(this.fruiting.old, new StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback(this.IsOld), UpdateRate.SIM_4000ms);
			this.fruiting.old.PlayAnim("wilt1", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).Transition(this.fruiting.idle, GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Not(new StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Transition.ConditionCallback(this.IsOld)), UpdateRate.SIM_4000ms);
			this.fruiting.wilting.PlayAnim("wilt1", KAnim.PlayMode.Once).EventTransition(GameHashes.WiltRecover, this.fruiting, (CritterTrapPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.harvest.PlayAnim("harvest", KAnim.PlayMode.Once).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (GameScheduler.Instance != null && smi.master != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.harvestable.SetCanBeHarvested(false);
			}).Exit(delegate(CritterTrapPlant.StatesInstance smi)
			{
				smi.sm.hasEatenCreature.Set(false, smi, false);
			}).OnAnimQueueComplete(this.trap.open);
			this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(CritterTrapPlant.StatesInstance smi)
			{
				if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
				{
					Notifier notifier = smi.master.gameObject.AddOrGet<Notifier>();
					Notification notification = smi.master.CreateDeathNotification();
					notifier.Add(notification, "");
				}
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				Harvestable harvestable = smi.master.harvestable;
				if (harvestable != null && harvestable.CanBeHarvested && GameScheduler.Instance != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
		}

		// Token: 0x06008D8D RID: 36237 RVA: 0x0031C671 File Offset: 0x0031A871
		public bool IsOld(CritterTrapPlant.StatesInstance smi)
		{
			return smi.master.growing.PercentOldAge() > 0.5f;
		}

		// Token: 0x04006DBD RID: 28093
		public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Signal trapTriggered;

		// Token: 0x04006DBE RID: 28094
		public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.Signal ventGas;

		// Token: 0x04006DBF RID: 28095
		public StateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.BoolParameter hasEatenCreature;

		// Token: 0x04006DC0 RID: 28096
		public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State dead;

		// Token: 0x04006DC1 RID: 28097
		public CritterTrapPlant.States.FruitingStates fruiting;

		// Token: 0x04006DC2 RID: 28098
		public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State harvest;

		// Token: 0x04006DC3 RID: 28099
		public CritterTrapPlant.States.TrapStates trap;

		// Token: 0x020021B9 RID: 8633
		public class DigestingStates : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
		{
			// Token: 0x0400970B RID: 38667
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State idle;

			// Token: 0x0400970C RID: 38668
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State vent_pre;

			// Token: 0x0400970D RID: 38669
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State vent;
		}

		// Token: 0x020021BA RID: 8634
		public class TrapStates : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
		{
			// Token: 0x0400970E RID: 38670
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State open;

			// Token: 0x0400970F RID: 38671
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State trigger;

			// Token: 0x04009710 RID: 38672
			public CritterTrapPlant.States.DigestingStates digesting;

			// Token: 0x04009711 RID: 38673
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State wilting;
		}

		// Token: 0x020021BB RID: 8635
		public class FruitingStates : GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State
		{
			// Token: 0x04009712 RID: 38674
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State enter;

			// Token: 0x04009713 RID: 38675
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State idle;

			// Token: 0x04009714 RID: 38676
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State old;

			// Token: 0x04009715 RID: 38677
			public GameStateMachine<CritterTrapPlant.States, CritterTrapPlant.StatesInstance, CritterTrapPlant, object>.State wilting;
		}
	}
}
