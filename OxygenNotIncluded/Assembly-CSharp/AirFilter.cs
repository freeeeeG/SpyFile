using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000584 RID: 1412
[SerializationConfig(MemberSerialization.OptIn)]
public class AirFilter : StateMachineComponent<AirFilter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x0600222D RID: 8749 RVA: 0x000BBD63 File Offset: 0x000B9F63
	public bool HasFilter()
	{
		return this.elementConverter.HasEnoughMass(this.filterTag, false);
	}

	// Token: 0x0600222E RID: 8750 RVA: 0x000BBD77 File Offset: 0x000B9F77
	public bool IsConvertable()
	{
		return this.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x0600222F RID: 8751 RVA: 0x000BBD85 File Offset: 0x000B9F85
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06002230 RID: 8752 RVA: 0x000BBD98 File Offset: 0x000B9F98
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x04001371 RID: 4977
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001372 RID: 4978
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001373 RID: 4979
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x04001374 RID: 4980
	[MyCmpGet]
	private ElementConsumer elementConsumer;

	// Token: 0x04001375 RID: 4981
	public Tag filterTag;

	// Token: 0x0200121B RID: 4635
	public class StatesInstance : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.GameInstance
	{
		// Token: 0x06007BDE RID: 31710 RVA: 0x002DEFF2 File Offset: 0x002DD1F2
		public StatesInstance(AirFilter smi) : base(smi)
		{
		}
	}

	// Token: 0x0200121C RID: 4636
	public class States : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter>
	{
		// Token: 0x06007BDF RID: 31711 RVA: 0x002DEFFC File Offset: 0x002DD1FC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.waiting;
			this.waiting.EventTransition(GameHashes.OnStorageChange, this.hasFilter, (AirFilter.StatesInstance smi) => smi.master.HasFilter() && smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.hasFilter, (AirFilter.StatesInstance smi) => smi.master.HasFilter() && smi.master.operational.IsOperational);
			this.hasFilter.EventTransition(GameHashes.OperationalChanged, this.waiting, (AirFilter.StatesInstance smi) => !smi.master.operational.IsOperational).Enter("EnableConsumption", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit("DisableConsumption", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			}).DefaultState(this.hasFilter.idle);
			this.hasFilter.idle.EventTransition(GameHashes.OnStorageChange, this.hasFilter.converting, (AirFilter.StatesInstance smi) => smi.master.IsConvertable());
			this.hasFilter.converting.Enter("SetActive(true)", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.hasFilter.idle, (AirFilter.StatesInstance smi) => !smi.master.IsConvertable());
		}

		// Token: 0x04005E74 RID: 24180
		public AirFilter.States.ReadyStates hasFilter;

		// Token: 0x04005E75 RID: 24181
		public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State waiting;

		// Token: 0x020020AB RID: 8363
		public class ReadyStates : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State
		{
			// Token: 0x040091DC RID: 37340
			public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State idle;

			// Token: 0x040091DD RID: 37341
			public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State converting;
		}
	}
}
