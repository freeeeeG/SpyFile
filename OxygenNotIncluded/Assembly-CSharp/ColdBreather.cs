using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008DF RID: 2271
[SkipSaveFileSerialization]
public class ColdBreather : StateMachineComponent<ColdBreather.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060041AB RID: 16811 RVA: 0x0016F953 File Offset: 0x0016DB53
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(ColdBreather.OnSimEmittedCallback), this, "ColdBreather");
		base.smi.StartSM();
	}

	// Token: 0x060041AC RID: 16812 RVA: 0x0016F98D File Offset: 0x0016DB8D
	protected override void OnPrefabInit()
	{
		this.elementConsumer.EnableConsumption(false);
		base.Subscribe<ColdBreather>(1309017699, ColdBreather.OnReplantedDelegate);
		base.OnPrefabInit();
	}

	// Token: 0x060041AD RID: 16813 RVA: 0x0016F9B4 File Offset: 0x0016DBB4
	private void OnReplanted(object data = null)
	{
		ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
		if (component == null)
		{
			return;
		}
		ElementConsumer component2 = base.GetComponent<ElementConsumer>();
		if (component.Replanted)
		{
			component2.consumptionRate = this.consumptionRate;
		}
		else
		{
			component2.consumptionRate = this.consumptionRate * 0.25f;
		}
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.emitRads = 480f;
			this.radiationEmitter.Refresh();
		}
	}

	// Token: 0x060041AE RID: 16814 RVA: 0x0016FA2C File Offset: 0x0016DC2C
	protected override void OnCleanUp()
	{
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "coldbreather");
		this.simEmitCBHandle.Clear();
		if (this.storage)
		{
			this.storage.DropAll(true, false, default(Vector3), true, null);
		}
		base.OnCleanUp();
	}

	// Token: 0x060041AF RID: 16815 RVA: 0x0016FA8A File Offset: 0x0016DC8A
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060041B0 RID: 16816 RVA: 0x0016FAA2 File Offset: 0x0016DCA2
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.COLDBREATHER, UI.GAMEOBJECTEFFECTS.TOOLTIPS.COLDBREATHER, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x060041B1 RID: 16817 RVA: 0x0016FACA File Offset: 0x0016DCCA
	private void SetEmitting(bool emitting)
	{
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.SetEmitting(emitting);
		}
	}

	// Token: 0x060041B2 RID: 16818 RVA: 0x0016FAE8 File Offset: 0x0016DCE8
	private void Exhale()
	{
		if (this.lastEmitTag != Tag.Invalid)
		{
			return;
		}
		this.gases.Clear();
		this.storage.Find(GameTags.Gas, this.gases);
		if (this.nextGasEmitIndex >= this.gases.Count)
		{
			this.nextGasEmitIndex = 0;
		}
		while (this.nextGasEmitIndex < this.gases.Count)
		{
			int num = this.nextGasEmitIndex;
			this.nextGasEmitIndex = num + 1;
			int index = num;
			PrimaryElement component = this.gases[index].GetComponent<PrimaryElement>();
			if (component != null && component.Mass > 0f && this.simEmitCBHandle.IsValid())
			{
				float temperature = Mathf.Max(component.Element.lowTemp + 5f, component.Temperature + this.deltaEmitTemperature);
				int gameCell = Grid.PosToCell(base.transform.GetPosition() + this.emitOffsetCell);
				ushort idx = component.Element.idx;
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				SimMessages.EmitMass(gameCell, idx, component.Mass, temperature, component.DiseaseIdx, component.DiseaseCount, this.simEmitCBHandle.index);
				this.lastEmitTag = component.Element.tag;
				return;
			}
		}
	}

	// Token: 0x060041B3 RID: 16819 RVA: 0x0016FC4B File Offset: 0x0016DE4B
	private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((ColdBreather)data).OnSimEmitted(info);
	}

	// Token: 0x060041B4 RID: 16820 RVA: 0x0016FC5C File Offset: 0x0016DE5C
	private void OnSimEmitted(Sim.MassEmittedCallback info)
	{
		if (info.suceeded == 1 && this.storage && this.lastEmitTag.IsValid)
		{
			this.storage.ConsumeIgnoringDisease(this.lastEmitTag, info.mass);
		}
		this.lastEmitTag = Tag.Invalid;
	}

	// Token: 0x04002AC3 RID: 10947
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04002AC4 RID: 10948
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04002AC5 RID: 10949
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04002AC6 RID: 10950
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x04002AC7 RID: 10951
	[MyCmpGet]
	private RadiationEmitter radiationEmitter;

	// Token: 0x04002AC8 RID: 10952
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x04002AC9 RID: 10953
	private const float EXHALE_PERIOD = 1f;

	// Token: 0x04002ACA RID: 10954
	public float consumptionRate;

	// Token: 0x04002ACB RID: 10955
	public float deltaEmitTemperature = -5f;

	// Token: 0x04002ACC RID: 10956
	public Vector3 emitOffsetCell = new Vector3(0f, 0f);

	// Token: 0x04002ACD RID: 10957
	private List<GameObject> gases = new List<GameObject>();

	// Token: 0x04002ACE RID: 10958
	private Tag lastEmitTag;

	// Token: 0x04002ACF RID: 10959
	private int nextGasEmitIndex;

	// Token: 0x04002AD0 RID: 10960
	private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

	// Token: 0x04002AD1 RID: 10961
	private static readonly EventSystem.IntraObjectHandler<ColdBreather> OnReplantedDelegate = new EventSystem.IntraObjectHandler<ColdBreather>(delegate(ColdBreather component, object data)
	{
		component.OnReplanted(data);
	});

	// Token: 0x0200171D RID: 5917
	public class StatesInstance : GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.GameInstance
	{
		// Token: 0x06008D7D RID: 36221 RVA: 0x0031BC30 File Offset: 0x00319E30
		public StatesInstance(ColdBreather master) : base(master)
		{
		}
	}

	// Token: 0x0200171E RID: 5918
	public class States : GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather>
	{
		// Token: 0x06008D7E RID: 36222 RVA: 0x0031BC3C File Offset: 0x00319E3C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.grow;
			this.statusItemCooling = new StatusItem("cooling", CREATURES.STATUSITEMS.COOLING.NAME, CREATURES.STATUSITEMS.COOLING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			this.dead.ToggleStatusItem(CREATURES.STATUSITEMS.DEAD.NAME, CREATURES.STATUSITEMS.DEAD.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Enter(delegate(ColdBreather.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (ColdBreather.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (ColdBreather.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (ColdBreather.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(ColdBreather.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature).Update(delegate(ColdBreather.StatesInstance smi, float dt)
			{
				smi.master.Exhale();
			}, UpdateRate.SIM_200ms, false);
			this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (ColdBreather.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop).ToggleMainStatusItem(this.statusItemCooling, null).Enter(delegate(ColdBreather.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
				smi.master.SetEmitting(true);
			}).Exit(delegate(ColdBreather.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
				smi.master.SetEmitting(false);
			});
			this.alive.wilting.PlayAnim("wilt1").EventTransition(GameHashes.WiltRecover, this.alive.mature, (ColdBreather.StatesInstance smi) => !smi.master.wiltCondition.IsWilting()).Enter(delegate(ColdBreather.StatesInstance smi)
			{
				smi.master.SetEmitting(false);
			});
		}

		// Token: 0x04006DB7 RID: 28087
		public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State grow;

		// Token: 0x04006DB8 RID: 28088
		public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State blocked_from_growing;

		// Token: 0x04006DB9 RID: 28089
		public ColdBreather.States.AliveStates alive;

		// Token: 0x04006DBA RID: 28090
		public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State dead;

		// Token: 0x04006DBB RID: 28091
		private StatusItem statusItemCooling;

		// Token: 0x020021B7 RID: 8631
		public class AliveStates : GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.PlantAliveSubState
		{
			// Token: 0x04009701 RID: 38657
			public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State mature;

			// Token: 0x04009702 RID: 38658
			public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State wilting;
		}
	}
}
