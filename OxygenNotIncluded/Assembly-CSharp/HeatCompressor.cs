using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000615 RID: 1557
public class HeatCompressor : StateMachineComponent<HeatCompressor.StatesInstance>
{
	// Token: 0x06002731 RID: 10033 RVA: 0x000D4E00 File Offset: 0x000D3000
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("HeatCube"), base.transform.GetPosition());
		gameObject.SetActive(true);
		this.heatCubeStorage.Store(gameObject, true, false, true, false);
		base.smi.StartSM();
	}

	// Token: 0x06002732 RID: 10034 RVA: 0x000D4EAF File Offset: 0x000D30AF
	public void SetStorage(Storage inputStorage, Storage outputStorage, Storage heatCubeStorage)
	{
		this.inputStorage = inputStorage;
		this.outputStorage = outputStorage;
		this.heatCubeStorage = heatCubeStorage;
	}

	// Token: 0x06002733 RID: 10035 RVA: 0x000D4EC8 File Offset: 0x000D30C8
	public void CompressHeat(HeatCompressor.StatesInstance smi, float dt)
	{
		smi.heatRemovalTimer -= dt;
		float num = this.heatRemovalRate * dt / (float)this.inputStorage.items.Count;
		foreach (GameObject gameObject in this.inputStorage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			float lowTemp = component.Element.lowTemp;
			GameUtil.DeltaThermalEnergy(component, -num, lowTemp);
			this.energyCompressed += num;
		}
		if (smi.heatRemovalTimer <= 0f)
		{
			for (int i = this.inputStorage.items.Count; i > 0; i--)
			{
				GameObject gameObject2 = this.inputStorage.items[i - 1];
				if (gameObject2)
				{
					this.inputStorage.Transfer(gameObject2, this.outputStorage, false, true);
				}
			}
			smi.StartNewHeatRemoval();
		}
		foreach (GameObject gameObject3 in this.heatCubeStorage.items)
		{
			GameUtil.DeltaThermalEnergy(gameObject3.GetComponent<PrimaryElement>(), this.energyCompressed / (float)this.heatCubeStorage.items.Count, 100000f);
		}
		this.energyCompressed = 0f;
	}

	// Token: 0x06002734 RID: 10036 RVA: 0x000D503C File Offset: 0x000D323C
	public void EjectHeatCube()
	{
		this.heatCubeStorage.DropAll(base.transform.GetPosition(), false, false, default(Vector3), true, null);
	}

	// Token: 0x04001685 RID: 5765
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001686 RID: 5766
	private MeterController meter;

	// Token: 0x04001687 RID: 5767
	public Storage inputStorage;

	// Token: 0x04001688 RID: 5768
	public Storage outputStorage;

	// Token: 0x04001689 RID: 5769
	public Storage heatCubeStorage;

	// Token: 0x0400168A RID: 5770
	public float heatRemovalRate = 100f;

	// Token: 0x0400168B RID: 5771
	public float heatRemovalTime = 100f;

	// Token: 0x0400168C RID: 5772
	[Serialize]
	public float energyCompressed;

	// Token: 0x0400168D RID: 5773
	public float heat_sink_active_time = 9000f;

	// Token: 0x0400168E RID: 5774
	[Serialize]
	public float time_active;

	// Token: 0x0400168F RID: 5775
	public float MAX_CUBE_TEMPERATURE = 3000f;

	// Token: 0x020012D3 RID: 4819
	public class StatesInstance : GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.GameInstance
	{
		// Token: 0x06007EBC RID: 32444 RVA: 0x002E9B62 File Offset: 0x002E7D62
		public StatesInstance(HeatCompressor master) : base(master)
		{
		}

		// Token: 0x06007EBD RID: 32445 RVA: 0x002E9B6C File Offset: 0x002E7D6C
		public void UpdateMeter()
		{
			float remainingCharge = this.GetRemainingCharge();
			base.master.meter.SetPositionPercent(remainingCharge);
		}

		// Token: 0x06007EBE RID: 32446 RVA: 0x002E9B94 File Offset: 0x002E7D94
		public float GetRemainingCharge()
		{
			PrimaryElement primaryElement = base.smi.master.heatCubeStorage.FindFirstWithMass(GameTags.IndustrialIngredient, 0f);
			float result = 1f;
			if (primaryElement != null)
			{
				result = Mathf.Clamp01(primaryElement.GetComponent<PrimaryElement>().Temperature / base.smi.master.MAX_CUBE_TEMPERATURE);
			}
			return result;
		}

		// Token: 0x06007EBF RID: 32447 RVA: 0x002E9BF3 File Offset: 0x002E7DF3
		public bool CanWork()
		{
			return this.GetRemainingCharge() < 1f && base.smi.master.heatCubeStorage.items.Count > 0;
		}

		// Token: 0x06007EC0 RID: 32448 RVA: 0x002E9C21 File Offset: 0x002E7E21
		public void StartNewHeatRemoval()
		{
			this.heatRemovalTimer = base.smi.master.heatRemovalTime;
		}

		// Token: 0x040060DE RID: 24798
		[Serialize]
		public float heatRemovalTimer;
	}

	// Token: 0x020012D4 RID: 4820
	public class States : GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor>
	{
		// Token: 0x06007EC1 RID: 32449 RVA: 0x002E9C3C File Offset: 0x002E7E3C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inactive;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventTransition(GameHashes.OperationalChanged, this.inactive, (HeatCompressor.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.inactive.Enter(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.UpdateMeter();
			}).PlayAnim("idle").Transition(this.dropCube, (HeatCompressor.StatesInstance smi) => smi.GetRemainingCharge() >= 1f, UpdateRate.SIM_200ms).Transition(this.active, (HeatCompressor.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational && smi.CanWork(), UpdateRate.SIM_200ms);
			this.active.Enter(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
				smi.StartNewHeatRemoval();
			}).PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(delegate(HeatCompressor.StatesInstance smi, float dt)
			{
				smi.master.time_active += dt;
				smi.UpdateMeter();
				smi.master.CompressHeat(smi, dt);
			}, UpdateRate.SIM_200ms, false).Transition(this.dropCube, (HeatCompressor.StatesInstance smi) => smi.GetRemainingCharge() >= 1f, UpdateRate.SIM_200ms).Transition(this.inactive, (HeatCompressor.StatesInstance smi) => !smi.CanWork(), UpdateRate.SIM_200ms).Exit(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.dropCube.Enter(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.master.EjectHeatCube();
				smi.GoTo(this.inactive);
			});
		}

		// Token: 0x040060DF RID: 24799
		public GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.State active;

		// Token: 0x040060E0 RID: 24800
		public GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.State inactive;

		// Token: 0x040060E1 RID: 24801
		public GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.State dropCube;
	}
}
