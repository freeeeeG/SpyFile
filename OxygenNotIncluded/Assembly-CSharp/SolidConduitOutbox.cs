using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000690 RID: 1680
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitOutbox : StateMachineComponent<SolidConduitOutbox.SMInstance>
{
	// Token: 0x06002CF9 RID: 11513 RVA: 0x000EECAB File Offset: 0x000ECEAB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002CFA RID: 11514 RVA: 0x000EECB3 File Offset: 0x000ECEB3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.Subscribe<SolidConduitOutbox>(-1697596308, SolidConduitOutbox.OnStorageChangedDelegate);
		this.UpdateMeter();
		base.smi.StartSM();
	}

	// Token: 0x06002CFB RID: 11515 RVA: 0x000EECF1 File Offset: 0x000ECEF1
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06002CFC RID: 11516 RVA: 0x000EECF9 File Offset: 0x000ECEF9
	private void OnStorageChanged(object data)
	{
		this.UpdateMeter();
	}

	// Token: 0x06002CFD RID: 11517 RVA: 0x000EED04 File Offset: 0x000ECF04
	private void UpdateMeter()
	{
		float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06002CFE RID: 11518 RVA: 0x000EED3A File Offset: 0x000ECF3A
	private void UpdateConsuming()
	{
		base.smi.sm.consuming.Set(this.consumer.IsConsuming, base.smi, false);
	}

	// Token: 0x04001A7D RID: 6781
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001A7E RID: 6782
	[MyCmpReq]
	private SolidConduitConsumer consumer;

	// Token: 0x04001A7F RID: 6783
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001A80 RID: 6784
	private MeterController meter;

	// Token: 0x04001A81 RID: 6785
	private static readonly EventSystem.IntraObjectHandler<SolidConduitOutbox> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<SolidConduitOutbox>(delegate(SolidConduitOutbox component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x020013A7 RID: 5031
	public class SMInstance : GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.GameInstance
	{
		// Token: 0x060081DD RID: 33245 RVA: 0x002F6B40 File Offset: 0x002F4D40
		public SMInstance(SolidConduitOutbox master) : base(master)
		{
		}
	}

	// Token: 0x020013A8 RID: 5032
	public class States : GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox>
	{
		// Token: 0x060081DE RID: 33246 RVA: 0x002F6B4C File Offset: 0x002F4D4C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Update("RefreshConsuming", delegate(SolidConduitOutbox.SMInstance smi, float dt)
			{
				smi.master.UpdateConsuming();
			}, UpdateRate.SIM_1000ms, false);
			this.idle.PlayAnim("on").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.IsTrue);
			this.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ParamTransition<bool>(this.consuming, this.post, GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.IsFalse);
			this.post.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x0400630A RID: 25354
		public StateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.BoolParameter consuming;

		// Token: 0x0400630B RID: 25355
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State idle;

		// Token: 0x0400630C RID: 25356
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State working;

		// Token: 0x0400630D RID: 25357
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State post;
	}
}
