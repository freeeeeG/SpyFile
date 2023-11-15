using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000667 RID: 1639
public class OreScrubber : StateMachineComponent<OreScrubber.SMInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06002B63 RID: 11107 RVA: 0x000E6D7C File Offset: 0x000E4F7C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.FindOrAddComponent<Workable>();
	}

	// Token: 0x06002B64 RID: 11108 RVA: 0x000E6D90 File Offset: 0x000E4F90
	private void RefreshMeters()
	{
		float positionPercent = 0f;
		PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.consumedElement);
		if (primaryElement != null)
		{
			positionPercent = Mathf.Clamp01(primaryElement.Mass / base.GetComponent<ConduitConsumer>().capacityKG);
		}
		this.cleanMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06002B65 RID: 11109 RVA: 0x000E6DE4 File Offset: 0x000E4FE4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.cleanMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_clean_target", "meter_clean", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_clean_target"
		});
		this.RefreshMeters();
		base.Subscribe<OreScrubber>(-1697596308, OreScrubber.OnStorageChangeDelegate);
		DirectionControl component = base.GetComponent<DirectionControl>();
		component.onDirectionChanged = (Action<WorkableReactable.AllowedDirection>)Delegate.Combine(component.onDirectionChanged, new Action<WorkableReactable.AllowedDirection>(this.OnDirectionChanged));
		this.OnDirectionChanged(base.GetComponent<DirectionControl>().allowedDirection);
	}

	// Token: 0x06002B66 RID: 11110 RVA: 0x000E6E7D File Offset: 0x000E507D
	private void OnDirectionChanged(WorkableReactable.AllowedDirection allowed_direction)
	{
		if (this.reactable != null)
		{
			this.reactable.allowedDirection = allowed_direction;
		}
	}

	// Token: 0x06002B67 RID: 11111 RVA: 0x000E6E94 File Offset: 0x000E5094
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string name = ElementLoader.FindElementByHash(this.consumedElement).name;
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		return list;
	}

	// Token: 0x06002B68 RID: 11112 RVA: 0x000E6F0C File Offset: 0x000E510C
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.outputElement != SimHashes.Vacuum)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTEDPERUSE, ElementLoader.FindElementByHash(this.outputElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTEDPERUSE, ElementLoader.FindElementByHash(this.outputElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect, false));
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASECONSUMEDPERUSE, GameUtil.GetFormattedDiseaseAmount(this.diseaseRemovalCount, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASECONSUMEDPERUSE, GameUtil.GetFormattedDiseaseAmount(this.diseaseRemovalCount, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x000E6FE1 File Offset: 0x000E51E1
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x06002B6A RID: 11114 RVA: 0x000E7000 File Offset: 0x000E5200
	private void OnStorageChange(object data)
	{
		this.RefreshMeters();
	}

	// Token: 0x06002B6B RID: 11115 RVA: 0x000E7008 File Offset: 0x000E5208
	private static PrimaryElement GetFirstInfected(Storage storage)
	{
		foreach (GameObject gameObject in storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.DiseaseIdx != 255 && !gameObject.HasTag(GameTags.Edible))
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x0400196B RID: 6507
	public float massConsumedPerUse = 1f;

	// Token: 0x0400196C RID: 6508
	public SimHashes consumedElement = SimHashes.BleachStone;

	// Token: 0x0400196D RID: 6509
	public int diseaseRemovalCount = 10000;

	// Token: 0x0400196E RID: 6510
	public SimHashes outputElement = SimHashes.Vacuum;

	// Token: 0x0400196F RID: 6511
	private WorkableReactable reactable;

	// Token: 0x04001970 RID: 6512
	private MeterController cleanMeter;

	// Token: 0x04001971 RID: 6513
	[Serialize]
	public int maxPossiblyRemoved;

	// Token: 0x04001972 RID: 6514
	private static readonly EventSystem.IntraObjectHandler<OreScrubber> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OreScrubber>(delegate(OreScrubber component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001359 RID: 4953
	private class ScrubOreReactable : WorkableReactable
	{
		// Token: 0x060080C7 RID: 32967 RVA: 0x002F2753 File Offset: 0x002F0953
		public ScrubOreReactable(Workable workable, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable, "ScrubOre", chore_type, allowed_direction)
		{
		}

		// Token: 0x060080C8 RID: 32968 RVA: 0x002F2768 File Offset: 0x002F0968
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				Storage component = new_reactor.GetComponent<Storage>();
				if (component != null && OreScrubber.GetFirstInfected(component) != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x0200135A RID: 4954
	public class SMInstance : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.GameInstance
	{
		// Token: 0x060080C9 RID: 32969 RVA: 0x002F27A0 File Offset: 0x002F09A0
		public SMInstance(OreScrubber master) : base(master)
		{
		}

		// Token: 0x060080CA RID: 32970 RVA: 0x002F27AC File Offset: 0x002F09AC
		public bool HasSufficientMass()
		{
			bool result = false;
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(base.master.consumedElement);
			if (primaryElement != null)
			{
				result = (primaryElement.Mass > 0f);
			}
			return result;
		}

		// Token: 0x060080CB RID: 32971 RVA: 0x002F27EA File Offset: 0x002F09EA
		public Dictionary<Tag, float> GetNeededMass()
		{
			return new Dictionary<Tag, float>
			{
				{
					base.master.consumedElement.CreateTag(),
					base.master.massConsumedPerUse
				}
			};
		}

		// Token: 0x060080CC RID: 32972 RVA: 0x002F2812 File Offset: 0x002F0A12
		public void OnCompleteWork(Worker worker)
		{
		}

		// Token: 0x060080CD RID: 32973 RVA: 0x002F2814 File Offset: 0x002F0A14
		public void DumpOutput()
		{
			Storage component = base.master.GetComponent<Storage>();
			if (base.master.outputElement != SimHashes.Vacuum)
			{
				component.Drop(ElementLoader.FindElementByHash(base.master.outputElement).tag);
			}
		}
	}

	// Token: 0x0200135B RID: 4955
	public class States : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber>
	{
		// Token: 0x060080CE RID: 32974 RVA: 0x002F285C File Offset: 0x002F0A5C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notready;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.notoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.notready, false);
			this.notready.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready, (OreScrubber.SMInstance smi) => smi.HasSufficientMass()).ToggleStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, (OreScrubber.SMInstance smi) => smi.GetNeededMass()).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((OreScrubber.SMInstance smi) => smi.master.reactable = new OreScrubber.ScrubOreReactable(smi.master.GetComponent<OreScrubber.Work>(), Db.Get().ChoreTypes.ScrubOre, smi.master.GetComponent<DirectionControl>().allowedDirection)).EventTransition(GameHashes.OnStorageChange, this.notready, (OreScrubber.SMInstance smi) => !smi.HasSufficientMass()).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((OreScrubber.SMInstance smi) => smi.GetComponent<OreScrubber.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((OreScrubber.SMInstance smi) => smi.GetComponent<OreScrubber.Work>(), this.ready);
		}

		// Token: 0x04006246 RID: 25158
		public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State notready;

		// Token: 0x04006247 RID: 25159
		public OreScrubber.States.ReadyStates ready;

		// Token: 0x04006248 RID: 25160
		public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State notoperational;

		// Token: 0x04006249 RID: 25161
		public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State full;

		// Token: 0x0400624A RID: 25162
		public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State empty;

		// Token: 0x02002105 RID: 8453
		public class ReadyStates : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State
		{
			// Token: 0x040093E5 RID: 37861
			public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State free;

			// Token: 0x040093E6 RID: 37862
			public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State occupied;
		}
	}

	// Token: 0x0200135C RID: 4956
	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		// Token: 0x060080D0 RID: 32976 RVA: 0x002F2A32 File Offset: 0x002F0C32
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.shouldTransferDiseaseWithWorker = false;
		}

		// Token: 0x060080D1 RID: 32977 RVA: 0x002F2A48 File Offset: 0x002F0C48
		protected override void OnStartWork(Worker worker)
		{
			base.OnStartWork(worker);
			this.diseaseRemoved = 0;
		}

		// Token: 0x060080D2 RID: 32978 RVA: 0x002F2A58 File Offset: 0x002F0C58
		protected override bool OnWorkTick(Worker worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			OreScrubber component = base.GetComponent<OreScrubber>();
			Storage component2 = base.GetComponent<Storage>();
			PrimaryElement firstInfected = OreScrubber.GetFirstInfected(worker.GetComponent<Storage>());
			int num = 0;
			SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
			if (firstInfected != null)
			{
				num = Math.Min((int)(dt / this.workTime * (float)component.diseaseRemovalCount), firstInfected.DiseaseCount);
				this.diseaseRemoved += num;
				invalid.idx = firstInfected.DiseaseIdx;
				invalid.count = num;
				firstInfected.ModifyDiseaseCount(-num, "OreScrubber.OnWorkTick");
			}
			component.maxPossiblyRemoved += num;
			float amount = component.massConsumedPerUse * dt / this.workTime;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
			float mass;
			float temperature;
			component2.ConsumeAndGetDisease(ElementLoader.FindElementByHash(component.consumedElement).tag, amount, out mass, out diseaseInfo, out temperature);
			if (component.outputElement != SimHashes.Vacuum)
			{
				diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(invalid, diseaseInfo);
				component2.AddLiquid(component.outputElement, mass, temperature, diseaseInfo.idx, diseaseInfo.count, false, true);
			}
			return this.diseaseRemoved > component.diseaseRemovalCount;
		}

		// Token: 0x060080D3 RID: 32979 RVA: 0x002F2B72 File Offset: 0x002F0D72
		protected override void OnCompleteWork(Worker worker)
		{
			base.OnCompleteWork(worker);
		}

		// Token: 0x0400624B RID: 25163
		private int diseaseRemoved;
	}
}
