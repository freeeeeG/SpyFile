using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005B4 RID: 1460
[SerializationConfig(MemberSerialization.OptIn)]
public class AlgaeDistillery : StateMachineComponent<AlgaeDistillery.StatesInstance>
{
	// Token: 0x060023C5 RID: 9157 RVA: 0x000C3D1A File Offset: 0x000C1F1A
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x04001476 RID: 5238
	[SerializeField]
	public Tag emitTag;

	// Token: 0x04001477 RID: 5239
	[SerializeField]
	public float emitMass;

	// Token: 0x04001478 RID: 5240
	[SerializeField]
	public Vector3 emitOffset;

	// Token: 0x04001479 RID: 5241
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x0400147A RID: 5242
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x0400147B RID: 5243
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0200123E RID: 4670
	public class StatesInstance : GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.GameInstance
	{
		// Token: 0x06007C6F RID: 31855 RVA: 0x002E12C4 File Offset: 0x002DF4C4
		public StatesInstance(AlgaeDistillery smi) : base(smi)
		{
		}

		// Token: 0x06007C70 RID: 31856 RVA: 0x002E12D0 File Offset: 0x002DF4D0
		public void TryEmit()
		{
			Storage storage = base.smi.master.storage;
			GameObject gameObject = storage.FindFirst(base.smi.master.emitTag);
			if (gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass >= base.master.emitMass)
			{
				storage.Drop(gameObject, true).transform.SetPosition(base.transform.GetPosition() + base.master.emitOffset);
			}
		}
	}

	// Token: 0x0200123F RID: 4671
	public class States : GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery>
	{
		// Token: 0x06007C71 RID: 31857 RVA: 0x002E1354 File Offset: 0x002DF554
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (AlgaeDistillery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (AlgaeDistillery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (AlgaeDistillery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (AlgaeDistillery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).EventHandler(GameHashes.OnStorageChange, delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.TryEmit();
			});
		}

		// Token: 0x04005EEC RID: 24300
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State disabled;

		// Token: 0x04005EED RID: 24301
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State waiting;

		// Token: 0x04005EEE RID: 24302
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State converting;

		// Token: 0x04005EEF RID: 24303
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State overpressure;
	}
}
