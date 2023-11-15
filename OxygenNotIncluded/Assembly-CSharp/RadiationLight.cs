using System;
using UnityEngine;

// Token: 0x02000900 RID: 2304
public class RadiationLight : StateMachineComponent<RadiationLight.StatesInstance>
{
	// Token: 0x060042CE RID: 17102 RVA: 0x00175B70 File Offset: 0x00173D70
	public void UpdateMeter()
	{
		this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
	}

	// Token: 0x060042CF RID: 17103 RVA: 0x00175B99 File Offset: 0x00173D99
	public bool HasEnoughFuel()
	{
		return this.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x060042D0 RID: 17104 RVA: 0x00175BA7 File Offset: 0x00173DA7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.UpdateMeter();
	}

	// Token: 0x04002B90 RID: 11152
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002B91 RID: 11153
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002B92 RID: 11154
	[MyCmpGet]
	private RadiationEmitter emitter;

	// Token: 0x04002B93 RID: 11155
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x04002B94 RID: 11156
	private MeterController meter;

	// Token: 0x04002B95 RID: 11157
	public Tag elementToConsume;

	// Token: 0x04002B96 RID: 11158
	public float consumptionRate;

	// Token: 0x02001754 RID: 5972
	public class StatesInstance : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.GameInstance
	{
		// Token: 0x06008E19 RID: 36377 RVA: 0x0031E87C File Offset: 0x0031CA7C
		public StatesInstance(RadiationLight smi) : base(smi)
		{
			if (base.GetComponent<Rotatable>().IsRotated)
			{
				RadiationEmitter component = base.GetComponent<RadiationEmitter>();
				component.emitDirection = 180f;
				component.emissionOffset = Vector3.left;
			}
			this.ToggleEmitter(false);
			smi.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target"
			});
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
		}

		// Token: 0x06008E1A RID: 36378 RVA: 0x0031E8F9 File Offset: 0x0031CAF9
		public void ToggleEmitter(bool on)
		{
			base.smi.master.operational.SetActive(on, false);
			base.smi.master.emitter.SetEmitting(on);
		}
	}

	// Token: 0x02001755 RID: 5973
	public class States : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight>
	{
		// Token: 0x06008E1B RID: 36379 RVA: 0x0031E928 File Offset: 0x0031CB28
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready.idle;
			this.root.EventHandler(GameHashes.OnStorageChange, delegate(RadiationLight.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.waiting.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.ready.idle, (RadiationLight.StatesInstance smi) => smi.master.operational.IsOperational);
			this.ready.EventTransition(GameHashes.OperationalChanged, this.waiting, (RadiationLight.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.ready.idle);
			this.ready.idle.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready.on, (RadiationLight.StatesInstance smi) => smi.master.HasEnoughFuel());
			this.ready.on.PlayAnim("on").Enter(delegate(RadiationLight.StatesInstance smi)
			{
				smi.ToggleEmitter(true);
			}).EventTransition(GameHashes.OnStorageChange, this.ready.idle, (RadiationLight.StatesInstance smi) => !smi.master.HasEnoughFuel()).Exit(delegate(RadiationLight.StatesInstance smi)
			{
				smi.ToggleEmitter(false);
			});
		}

		// Token: 0x04006E6A RID: 28266
		public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State waiting;

		// Token: 0x04006E6B RID: 28267
		public RadiationLight.States.ReadyStates ready;

		// Token: 0x020021D0 RID: 8656
		public class ReadyStates : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State
		{
			// Token: 0x0400977C RID: 38780
			public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State idle;

			// Token: 0x0400977D RID: 38781
			public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State on;
		}
	}
}
