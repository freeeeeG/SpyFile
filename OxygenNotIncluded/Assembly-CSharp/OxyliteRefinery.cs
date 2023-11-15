using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000669 RID: 1641
[SerializationConfig(MemberSerialization.OptIn)]
public class OxyliteRefinery : StateMachineComponent<OxyliteRefinery.StatesInstance>
{
	// Token: 0x06002B73 RID: 11123 RVA: 0x000E71D5 File Offset: 0x000E53D5
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x04001976 RID: 6518
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001977 RID: 6519
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001978 RID: 6520
	public Tag emitTag;

	// Token: 0x04001979 RID: 6521
	public float emitMass;

	// Token: 0x0400197A RID: 6522
	public Vector3 dropOffset;

	// Token: 0x0200135E RID: 4958
	public class StatesInstance : GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.GameInstance
	{
		// Token: 0x060080D8 RID: 32984 RVA: 0x002F2BA0 File Offset: 0x002F0DA0
		public StatesInstance(OxyliteRefinery smi) : base(smi)
		{
		}

		// Token: 0x060080D9 RID: 32985 RVA: 0x002F2BAC File Offset: 0x002F0DAC
		public void TryEmit()
		{
			Storage storage = base.smi.master.storage;
			GameObject gameObject = storage.FindFirst(base.smi.master.emitTag);
			if (gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass >= base.master.emitMass)
			{
				Vector3 position = base.transform.GetPosition() + base.master.dropOffset;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				gameObject.transform.SetPosition(position);
				storage.Drop(gameObject, true);
			}
		}
	}

	// Token: 0x0200135F RID: 4959
	public class States : GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery>
	{
		// Token: 0x060080DA RID: 32986 RVA: 0x002F2C44 File Offset: 0x002F0E44
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (OxyliteRefinery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (OxyliteRefinery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.EventTransition(GameHashes.OnStorageChange, this.converting, (OxyliteRefinery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter(delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Transition(this.waiting, (OxyliteRefinery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).EventHandler(GameHashes.OnStorageChange, delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.TryEmit();
			});
		}

		// Token: 0x0400624D RID: 25165
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State disabled;

		// Token: 0x0400624E RID: 25166
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State waiting;

		// Token: 0x0400624F RID: 25167
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State converting;
	}
}
