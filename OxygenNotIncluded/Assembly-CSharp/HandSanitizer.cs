using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000614 RID: 1556
public class HandSanitizer : StateMachineComponent<HandSanitizer.SMInstance>, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x06002726 RID: 10022 RVA: 0x000D4A1B File Offset: 0x000D2C1B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.FindOrAddComponent<Workable>();
	}

	// Token: 0x06002727 RID: 10023 RVA: 0x000D4A30 File Offset: 0x000D2C30
	private void RefreshMeters()
	{
		float positionPercent = 0f;
		PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.consumedElement);
		float num = (float)this.maxUses * this.massConsumedPerUse;
		ConduitConsumer component = base.GetComponent<ConduitConsumer>();
		if (component != null)
		{
			num = component.capacityKG;
		}
		if (primaryElement != null)
		{
			positionPercent = Mathf.Clamp01(primaryElement.Mass / num);
		}
		float positionPercent2 = 0f;
		PrimaryElement primaryElement2 = base.GetComponent<Storage>().FindPrimaryElement(this.outputElement);
		if (primaryElement2 != null)
		{
			positionPercent2 = Mathf.Clamp01(primaryElement2.Mass / ((float)this.maxUses * this.massConsumedPerUse));
		}
		this.cleanMeter.SetPositionPercent(positionPercent);
		this.dirtyMeter.SetPositionPercent(positionPercent2);
	}

	// Token: 0x06002728 RID: 10024 RVA: 0x000D4AEC File Offset: 0x000D2CEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.cleanMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_clean_target", "meter_clean", this.cleanMeterOffset, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_clean_target"
		});
		this.dirtyMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_dirty_target", "meter_dirty", this.dirtyMeterOffset, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_dirty_target"
		});
		this.RefreshMeters();
		Components.HandSanitizers.Add(this);
		Components.BasicBuildings.Add(this);
		base.Subscribe<HandSanitizer>(-1697596308, HandSanitizer.OnStorageChangeDelegate);
		DirectionControl component = base.GetComponent<DirectionControl>();
		component.onDirectionChanged = (Action<WorkableReactable.AllowedDirection>)Delegate.Combine(component.onDirectionChanged, new Action<WorkableReactable.AllowedDirection>(this.OnDirectionChanged));
		this.OnDirectionChanged(base.GetComponent<DirectionControl>().allowedDirection);
	}

	// Token: 0x06002729 RID: 10025 RVA: 0x000D4BD1 File Offset: 0x000D2DD1
	protected override void OnCleanUp()
	{
		Components.BasicBuildings.Remove(this);
		Components.HandSanitizers.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600272A RID: 10026 RVA: 0x000D4BEF File Offset: 0x000D2DEF
	private void OnDirectionChanged(WorkableReactable.AllowedDirection allowed_direction)
	{
		if (this.reactable != null)
		{
			this.reactable.allowedDirection = allowed_direction;
		}
	}

	// Token: 0x0600272B RID: 10027 RVA: 0x000D4C08 File Offset: 0x000D2E08
	public List<Descriptor> RequirementDescriptors()
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, ElementLoader.FindElementByHash(this.consumedElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, ElementLoader.FindElementByHash(this.consumedElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x0600272C RID: 10028 RVA: 0x000D4C8C File Offset: 0x000D2E8C
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

	// Token: 0x0600272D RID: 10029 RVA: 0x000D4D61 File Offset: 0x000D2F61
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x0600272E RID: 10030 RVA: 0x000D4D80 File Offset: 0x000D2F80
	private void OnStorageChange(object data)
	{
		if (this.dumpWhenFull && base.smi.OutputFull())
		{
			base.smi.DumpOutput();
		}
		this.RefreshMeters();
	}

	// Token: 0x04001675 RID: 5749
	public float massConsumedPerUse = 1f;

	// Token: 0x04001676 RID: 5750
	public SimHashes consumedElement = SimHashes.BleachStone;

	// Token: 0x04001677 RID: 5751
	public int diseaseRemovalCount = 10000;

	// Token: 0x04001678 RID: 5752
	public int maxUses = 10;

	// Token: 0x04001679 RID: 5753
	public SimHashes outputElement = SimHashes.Vacuum;

	// Token: 0x0400167A RID: 5754
	public bool dumpWhenFull;

	// Token: 0x0400167B RID: 5755
	public bool alwaysUse;

	// Token: 0x0400167C RID: 5756
	public bool canSanitizeSuit;

	// Token: 0x0400167D RID: 5757
	public bool canSanitizeStorage;

	// Token: 0x0400167E RID: 5758
	private WorkableReactable reactable;

	// Token: 0x0400167F RID: 5759
	private MeterController cleanMeter;

	// Token: 0x04001680 RID: 5760
	private MeterController dirtyMeter;

	// Token: 0x04001681 RID: 5761
	public Meter.Offset cleanMeterOffset;

	// Token: 0x04001682 RID: 5762
	public Meter.Offset dirtyMeterOffset;

	// Token: 0x04001683 RID: 5763
	[Serialize]
	public int maxPossiblyRemoved;

	// Token: 0x04001684 RID: 5764
	private static readonly EventSystem.IntraObjectHandler<HandSanitizer> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<HandSanitizer>(delegate(HandSanitizer component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x020012CE RID: 4814
	private class WashHandsReactable : WorkableReactable
	{
		// Token: 0x06007EAA RID: 32426 RVA: 0x002E952E File Offset: 0x002E772E
		public WashHandsReactable(Workable workable, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable, "WashHands", chore_type, allowed_direction)
		{
		}

		// Token: 0x06007EAB RID: 32427 RVA: 0x002E9544 File Offset: 0x002E7744
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				HandSanitizer component = this.workable.GetComponent<HandSanitizer>();
				if (!component.smi.IsReady())
				{
					return false;
				}
				if (component.alwaysUse)
				{
					return true;
				}
				PrimaryElement component2 = new_reactor.GetComponent<PrimaryElement>();
				if (component2 != null)
				{
					return component2.DiseaseIdx != byte.MaxValue;
				}
			}
			return false;
		}
	}

	// Token: 0x020012CF RID: 4815
	public class SMInstance : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.GameInstance
	{
		// Token: 0x06007EAC RID: 32428 RVA: 0x002E95A2 File Offset: 0x002E77A2
		public SMInstance(HandSanitizer master) : base(master)
		{
		}

		// Token: 0x06007EAD RID: 32429 RVA: 0x002E95AC File Offset: 0x002E77AC
		private bool HasSufficientMass()
		{
			bool result = false;
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(base.master.consumedElement);
			if (primaryElement != null)
			{
				result = (primaryElement.Mass >= base.master.massConsumedPerUse);
			}
			return result;
		}

		// Token: 0x06007EAE RID: 32430 RVA: 0x002E95F4 File Offset: 0x002E77F4
		public bool OutputFull()
		{
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(base.master.outputElement);
			return primaryElement != null && primaryElement.Mass >= (float)base.master.maxUses * base.master.massConsumedPerUse;
		}

		// Token: 0x06007EAF RID: 32431 RVA: 0x002E9646 File Offset: 0x002E7846
		public bool IsReady()
		{
			return this.HasSufficientMass() && !this.OutputFull();
		}

		// Token: 0x06007EB0 RID: 32432 RVA: 0x002E9660 File Offset: 0x002E7860
		public void DumpOutput()
		{
			Storage component = base.master.GetComponent<Storage>();
			if (base.master.outputElement != SimHashes.Vacuum)
			{
				component.Drop(ElementLoader.FindElementByHash(base.master.outputElement).tag);
			}
		}
	}

	// Token: 0x020012D0 RID: 4816
	public class States : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer>
	{
		// Token: 0x06007EB1 RID: 32433 RVA: 0x002E96A8 File Offset: 0x002E78A8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notready;
			this.root.Update(new Action<HandSanitizer.SMInstance, float>(this.UpdateStatusItems), UpdateRate.SIM_200ms, false);
			this.notoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.notready, false);
			this.notready.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready, (HandSanitizer.SMInstance smi) => smi.IsReady()).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((HandSanitizer.SMInstance smi) => smi.master.reactable = new HandSanitizer.WashHandsReactable(smi.master.GetComponent<HandSanitizer.Work>(), Db.Get().ChoreTypes.WashHands, smi.master.GetComponent<DirectionControl>().allowedDirection)).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((HandSanitizer.SMInstance smi) => smi.GetComponent<HandSanitizer.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim("working_pre").QueueAnim("working_loop", true, null).Enter(delegate(HandSanitizer.SMInstance smi)
			{
				ConduitConsumer component = smi.GetComponent<ConduitConsumer>();
				if (component != null)
				{
					component.enabled = false;
				}
			}).Exit(delegate(HandSanitizer.SMInstance smi)
			{
				ConduitConsumer component = smi.GetComponent<ConduitConsumer>();
				if (component != null)
				{
					component.enabled = true;
				}
			}).WorkableStopTransition((HandSanitizer.SMInstance smi) => smi.GetComponent<HandSanitizer.Work>(), this.notready);
		}

		// Token: 0x06007EB2 RID: 32434 RVA: 0x002E9870 File Offset: 0x002E7A70
		private void UpdateStatusItems(HandSanitizer.SMInstance smi, float dt)
		{
			if (smi.OutputFull())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, this);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, false);
		}

		// Token: 0x040060D6 RID: 24790
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State notready;

		// Token: 0x040060D7 RID: 24791
		public HandSanitizer.States.ReadyStates ready;

		// Token: 0x040060D8 RID: 24792
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State notoperational;

		// Token: 0x040060D9 RID: 24793
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State full;

		// Token: 0x040060DA RID: 24794
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State empty;

		// Token: 0x020020DD RID: 8413
		public class ReadyStates : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State
		{
			// Token: 0x0400930F RID: 37647
			public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State free;

			// Token: 0x04009310 RID: 37648
			public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State occupied;
		}
	}

	// Token: 0x020012D1 RID: 4817
	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		// Token: 0x06007EB4 RID: 32436 RVA: 0x002E98D0 File Offset: 0x002E7AD0
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.shouldTransferDiseaseWithWorker = false;
			GameScheduler.Instance.Schedule("WaterFetchingTutorial", 2f, delegate(object obj)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, true);
			}, null, null);
		}

		// Token: 0x06007EB5 RID: 32437 RVA: 0x002E9927 File Offset: 0x002E7B27
		protected override void OnStartWork(Worker worker)
		{
			base.OnStartWork(worker);
			this.diseaseRemoved = 0;
		}

		// Token: 0x06007EB6 RID: 32438 RVA: 0x002E9938 File Offset: 0x002E7B38
		protected override bool OnWorkTick(Worker worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			HandSanitizer component = base.GetComponent<HandSanitizer>();
			Storage component2 = base.GetComponent<Storage>();
			float massAvailable = component2.GetMassAvailable(component.consumedElement);
			if (massAvailable == 0f)
			{
				return true;
			}
			PrimaryElement component3 = worker.GetComponent<PrimaryElement>();
			float amount = Mathf.Min(component.massConsumedPerUse * dt / this.workTime, massAvailable);
			int num = Math.Min((int)(dt / this.workTime * (float)component.diseaseRemovalCount), component3.DiseaseCount);
			this.diseaseRemoved += num;
			SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
			invalid.idx = component3.DiseaseIdx;
			invalid.count = num;
			component3.ModifyDiseaseCount(-num, "HandSanitizer.OnWorkTick");
			component.maxPossiblyRemoved += num;
			if (component.canSanitizeStorage && worker.GetComponent<Storage>())
			{
				foreach (GameObject gameObject in worker.GetComponent<Storage>().GetItems())
				{
					PrimaryElement component4 = gameObject.GetComponent<PrimaryElement>();
					if (component4)
					{
						int num2 = Math.Min((int)(dt / this.workTime * (float)component.diseaseRemovalCount), component4.DiseaseCount);
						component4.ModifyDiseaseCount(-num2, "HandSanitizer.OnWorkTick");
						component.maxPossiblyRemoved += num2;
					}
				}
			}
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
			float mass;
			float temperature;
			component2.ConsumeAndGetDisease(ElementLoader.FindElementByHash(component.consumedElement).tag, amount, out mass, out diseaseInfo, out temperature);
			if (component.outputElement != SimHashes.Vacuum)
			{
				diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(invalid, diseaseInfo);
				component2.AddLiquid(component.outputElement, mass, temperature, diseaseInfo.idx, diseaseInfo.count, false, true);
			}
			return false;
		}

		// Token: 0x06007EB7 RID: 32439 RVA: 0x002E9B04 File Offset: 0x002E7D04
		protected override void OnCompleteWork(Worker worker)
		{
			base.OnCompleteWork(worker);
			if (this.removeIrritation && !worker.HasTag(GameTags.HasSuitTank))
			{
				GasLiquidExposureMonitor.Instance smi = worker.GetSMI<GasLiquidExposureMonitor.Instance>();
				if (smi != null)
				{
					smi.ResetExposure();
				}
			}
		}

		// Token: 0x040060DB RID: 24795
		public bool removeIrritation;

		// Token: 0x040060DC RID: 24796
		private int diseaseRemoved;
	}
}
