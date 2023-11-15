using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000689 RID: 1673
[AddComponentMenu("KMonoBehaviour/Workable/Shower")]
public class Shower : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06002CAB RID: 11435 RVA: 0x000ED75D File Offset: 0x000EB95D
	private Shower()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06002CAC RID: 11436 RVA: 0x000ED76D File Offset: 0x000EB96D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetProgressOnStop = true;
		this.smi = new Shower.ShowerSM.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x06002CAD RID: 11437 RVA: 0x000ED794 File Offset: 0x000EB994
	protected override void OnStartWork(Worker worker)
	{
		HygieneMonitor.Instance instance = worker.GetSMI<HygieneMonitor.Instance>();
		base.WorkTimeRemaining = this.workTime * instance.GetDirtiness();
		this.accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
		this.smi.SetActive(true);
		base.OnStartWork(worker);
	}

	// Token: 0x06002CAE RID: 11438 RVA: 0x000ED7D9 File Offset: 0x000EB9D9
	protected override void OnStopWork(Worker worker)
	{
		this.smi.SetActive(false);
	}

	// Token: 0x06002CAF RID: 11439 RVA: 0x000ED7E8 File Offset: 0x000EB9E8
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < Shower.EffectsRemoved.Length; i++)
		{
			string effect_id = Shower.EffectsRemoved[i];
			component.Remove(effect_id);
		}
		if (!worker.HasTag(GameTags.HasSuitTank))
		{
			GasLiquidExposureMonitor.Instance instance = worker.GetSMI<GasLiquidExposureMonitor.Instance>();
			if (instance != null)
			{
				instance.ResetExposure();
			}
		}
		component.Add(Shower.SHOWER_EFFECT, true);
		HygieneMonitor.Instance instance2 = worker.GetSMI<HygieneMonitor.Instance>();
		if (instance2 != null)
		{
			instance2.SetDirtiness(0f);
		}
	}

	// Token: 0x06002CB0 RID: 11440 RVA: 0x000ED868 File Offset: 0x000EBA68
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		PrimaryElement component = worker.GetComponent<PrimaryElement>();
		if (component.DiseaseCount > 0)
		{
			SimUtil.DiseaseInfo diseaseInfo = new SimUtil.DiseaseInfo
			{
				idx = component.DiseaseIdx,
				count = Mathf.CeilToInt((float)component.DiseaseCount * (1f - Mathf.Pow(this.fractionalDiseaseRemoval, dt)) - (float)this.absoluteDiseaseRemoval)
			};
			component.ModifyDiseaseCount(-diseaseInfo.count, "Shower.RemoveDisease");
			this.accumulatedDisease = SimUtil.CalculateFinalDiseaseInfo(this.accumulatedDisease, diseaseInfo);
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.outputTargetElement);
			if (primaryElement != null)
			{
				primaryElement.GetComponent<PrimaryElement>().AddDisease(this.accumulatedDisease.idx, this.accumulatedDisease.count, "Shower.RemoveDisease");
				this.accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
			}
		}
		return false;
	}

	// Token: 0x06002CB1 RID: 11441 RVA: 0x000ED940 File Offset: 0x000EBB40
	protected override void OnAbortWork(Worker worker)
	{
		base.OnAbortWork(worker);
		HygieneMonitor.Instance instance = worker.GetSMI<HygieneMonitor.Instance>();
		if (instance != null)
		{
			instance.SetDirtiness(1f - this.GetPercentComplete());
		}
	}

	// Token: 0x06002CB2 RID: 11442 RVA: 0x000ED970 File Offset: 0x000EBB70
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (Shower.EffectsRemoved.Length != 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE, Descriptor.DescriptorType.Effect);
			descriptors.Add(item);
			for (int i = 0; i < Shower.EffectsRemoved.Length; i++)
			{
				string text = Shower.EffectsRemoved[i];
				string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
				string arg2 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".CAUSE");
				Descriptor item2 = default(Descriptor);
				item2.IncreaseIndent();
				item2.SetupDescriptor("• " + string.Format(UI.BUILDINGEFFECTS.REMOVEDEFFECT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, arg2), Descriptor.DescriptorType.Effect);
				descriptors.Add(item2);
			}
		}
		Effect.AddModifierDescriptions(base.gameObject, descriptors, Shower.SHOWER_EFFECT, true);
		return descriptors;
	}

	// Token: 0x04001A50 RID: 6736
	private Shower.ShowerSM.Instance smi;

	// Token: 0x04001A51 RID: 6737
	public static string SHOWER_EFFECT = "Showered";

	// Token: 0x04001A52 RID: 6738
	public SimHashes outputTargetElement;

	// Token: 0x04001A53 RID: 6739
	public float fractionalDiseaseRemoval;

	// Token: 0x04001A54 RID: 6740
	public int absoluteDiseaseRemoval;

	// Token: 0x04001A55 RID: 6741
	private SimUtil.DiseaseInfo accumulatedDisease;

	// Token: 0x04001A56 RID: 6742
	public const float WATER_PER_USE = 5f;

	// Token: 0x04001A57 RID: 6743
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoakingWet",
		"WetFeet",
		"MinorIrritation",
		"MajorIrritation"
	};

	// Token: 0x0200139C RID: 5020
	public class ShowerSM : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower>
	{
		// Token: 0x060081C4 RID: 33220 RVA: 0x002F65C8 File Offset: 0x002F47C8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.root.Update(new Action<Shower.ShowerSM.Instance, float>(this.UpdateStatusItems), UpdateRate.SIM_200ms, false);
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (Shower.ShowerSM.Instance smi) => smi.IsOperational).PlayAnim("off");
			this.operational.DefaultState(this.operational.not_ready).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Shower.ShowerSM.Instance smi) => !smi.IsOperational);
			this.operational.not_ready.EventTransition(GameHashes.OnStorageChange, this.operational.ready, (Shower.ShowerSM.Instance smi) => smi.IsReady()).PlayAnim("off");
			this.operational.ready.ToggleChore(new Func<Shower.ShowerSM.Instance, Chore>(this.CreateShowerChore), this.operational.not_ready);
		}

		// Token: 0x060081C5 RID: 33221 RVA: 0x002F66F0 File Offset: 0x002F48F0
		private Chore CreateShowerChore(Shower.ShowerSM.Instance smi)
		{
			return new WorkChore<Shower>(Db.Get().ChoreTypes.Shower, smi.master, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Hygiene, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		}

		// Token: 0x060081C6 RID: 33222 RVA: 0x002F6738 File Offset: 0x002F4938
		private void UpdateStatusItems(Shower.ShowerSM.Instance smi, float dt)
		{
			if (smi.OutputFull())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, this);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, false);
		}

		// Token: 0x040062F6 RID: 25334
		public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State unoperational;

		// Token: 0x040062F7 RID: 25335
		public Shower.ShowerSM.OperationalState operational;

		// Token: 0x0200211E RID: 8478
		public class OperationalState : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State
		{
			// Token: 0x04009475 RID: 38005
			public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State not_ready;

			// Token: 0x04009476 RID: 38006
			public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State ready;
		}

		// Token: 0x0200211F RID: 8479
		public new class Instance : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.GameInstance
		{
			// Token: 0x0600A8E4 RID: 43236 RVA: 0x003715B8 File Offset: 0x0036F7B8
			public Instance(Shower master) : base(master)
			{
				this.operational = master.GetComponent<Operational>();
				this.consumer = master.GetComponent<ConduitConsumer>();
				this.dispenser = master.GetComponent<ConduitDispenser>();
			}

			// Token: 0x17000A77 RID: 2679
			// (get) Token: 0x0600A8E5 RID: 43237 RVA: 0x003715E5 File Offset: 0x0036F7E5
			public bool IsOperational
			{
				get
				{
					return this.operational.IsOperational && this.consumer.IsConnected && this.dispenser.IsConnected;
				}
			}

			// Token: 0x0600A8E6 RID: 43238 RVA: 0x0037160E File Offset: 0x0036F80E
			public void SetActive(bool active)
			{
				this.operational.SetActive(active, false);
			}

			// Token: 0x0600A8E7 RID: 43239 RVA: 0x00371620 File Offset: 0x0036F820
			private bool HasSufficientMass()
			{
				bool result = false;
				PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
				if (primaryElement != null)
				{
					result = (primaryElement.Mass >= 5f);
				}
				return result;
			}

			// Token: 0x0600A8E8 RID: 43240 RVA: 0x0037165C File Offset: 0x0036F85C
			public bool OutputFull()
			{
				PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(SimHashes.DirtyWater);
				return primaryElement != null && primaryElement.Mass >= 5f;
			}

			// Token: 0x0600A8E9 RID: 43241 RVA: 0x00371695 File Offset: 0x0036F895
			public bool IsReady()
			{
				return this.HasSufficientMass() && !this.OutputFull();
			}

			// Token: 0x04009477 RID: 38007
			private Operational operational;

			// Token: 0x04009478 RID: 38008
			private ConduitConsumer consumer;

			// Token: 0x04009479 RID: 38009
			private ConduitDispenser dispenser;
		}
	}
}
