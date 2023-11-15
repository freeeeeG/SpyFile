using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000672 RID: 1650
[SerializationConfig(MemberSerialization.OptIn)]
public class Polymerizer : StateMachineComponent<Polymerizer.StatesInstance>
{
	// Token: 0x06002BB1 RID: 11185 RVA: 0x000E85CC File Offset: 0x000E67CC
	protected override void OnSpawn()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.plasticMeter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		this.oilMeter = new MeterController(component, "meter2_target", "meter2", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		component.SetSymbolVisiblity("meter_target", true);
		float positionPercent = 0f;
		PrimaryElement primaryElement = this.storage.FindPrimaryElement(SimHashes.Petroleum);
		if (primaryElement != null)
		{
			positionPercent = Mathf.Clamp01(primaryElement.Mass / this.consumer.capacityKG);
		}
		this.oilMeter.SetPositionPercent(positionPercent);
		base.smi.StartSM();
		base.Subscribe<Polymerizer>(-1697596308, Polymerizer.OnStorageChangedDelegate);
	}

	// Token: 0x06002BB2 RID: 11186 RVA: 0x000E86B0 File Offset: 0x000E68B0
	private void TryEmit()
	{
		GameObject gameObject = this.storage.FindFirst(this.emitTag);
		if (gameObject != null)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			this.UpdatePercentDone(component);
			this.TryEmit(component);
		}
	}

	// Token: 0x06002BB3 RID: 11187 RVA: 0x000E86F0 File Offset: 0x000E68F0
	private void TryEmit(PrimaryElement primary_elem)
	{
		if (primary_elem.Mass >= this.emitMass)
		{
			this.plasticMeter.SetPositionPercent(0f);
			GameObject gameObject = this.storage.Drop(primary_elem.gameObject, true);
			Rotatable component = base.GetComponent<Rotatable>();
			Vector3 vector = component.transform.GetPosition() + component.GetRotatedOffset(this.emitOffset);
			int i = Grid.PosToCell(vector);
			if (Grid.Solid[i])
			{
				vector += component.GetRotatedOffset(Vector3.left);
			}
			gameObject.transform.SetPosition(vector);
			PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.exhaustElement);
			if (primaryElement != null)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(vector), primaryElement.ElementID, null, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount, true, -1);
				primaryElement.Mass = 0f;
				primaryElement.ModifyDiseaseCount(int.MinValue, "Polymerizer.Exhaust");
			}
		}
	}

	// Token: 0x06002BB4 RID: 11188 RVA: 0x000E87E8 File Offset: 0x000E69E8
	private void UpdatePercentDone(PrimaryElement primary_elem)
	{
		float positionPercent = Mathf.Clamp01(primary_elem.Mass / this.emitMass);
		this.plasticMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x000E8814 File Offset: 0x000E6A14
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		if (component.ElementID == SimHashes.Petroleum)
		{
			float positionPercent = Mathf.Clamp01(component.Mass / this.consumer.capacityKG);
			this.oilMeter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x040019A2 RID: 6562
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x040019A3 RID: 6563
	[SerializeField]
	public float emitMass = 1f;

	// Token: 0x040019A4 RID: 6564
	[SerializeField]
	public Tag emitTag;

	// Token: 0x040019A5 RID: 6565
	[SerializeField]
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x040019A6 RID: 6566
	[SerializeField]
	public SimHashes exhaustElement = SimHashes.Vacuum;

	// Token: 0x040019A7 RID: 6567
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x040019A8 RID: 6568
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040019A9 RID: 6569
	[MyCmpGet]
	private ConduitConsumer consumer;

	// Token: 0x040019AA RID: 6570
	[MyCmpGet]
	private ElementConverter converter;

	// Token: 0x040019AB RID: 6571
	private MeterController plasticMeter;

	// Token: 0x040019AC RID: 6572
	private MeterController oilMeter;

	// Token: 0x040019AD RID: 6573
	private static readonly EventSystem.IntraObjectHandler<Polymerizer> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Polymerizer>(delegate(Polymerizer component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x0200136A RID: 4970
	public class StatesInstance : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.GameInstance
	{
		// Token: 0x060080F2 RID: 33010 RVA: 0x002F2F21 File Offset: 0x002F1121
		public StatesInstance(Polymerizer smi) : base(smi)
		{
		}
	}

	// Token: 0x0200136B RID: 4971
	public class States : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer>
	{
		// Token: 0x060080F3 RID: 33011 RVA: 0x002F2F2C File Offset: 0x002F112C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.EventTransition(GameHashes.OperationalChanged, this.off, (Polymerizer.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.off.EventTransition(GameHashes.OperationalChanged, this.on, (Polymerizer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.EventTransition(GameHashes.OnStorageChange, this.converting, (Polymerizer.StatesInstance smi) => smi.master.converter.CanConvertAtAll());
			this.converting.Enter("Ready", delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).EventHandler(GameHashes.OnStorageChange, delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.TryEmit();
			}).EventTransition(GameHashes.OnStorageChange, this.on, (Polymerizer.StatesInstance smi) => !smi.master.converter.CanConvertAtAll()).Exit("Ready", delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
		}

		// Token: 0x0400625D RID: 25181
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State off;

		// Token: 0x0400625E RID: 25182
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State on;

		// Token: 0x0400625F RID: 25183
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State converting;
	}
}
