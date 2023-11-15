using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008BC RID: 2236
public class NuclearResearchCenter : StateMachineComponent<NuclearResearchCenter.StatesInstance>, IResearchCenter, IGameObjectEffectDescriptor
{
	// Token: 0x060040BF RID: 16575 RVA: 0x00169CC8 File Offset: 0x00167EC8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.ResearchCenters.Add(this);
		this.particleMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", this.particleMeterOffset, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target"
		});
		base.Subscribe<NuclearResearchCenter>(-1837862626, NuclearResearchCenter.OnStorageChangeDelegate);
		this.RefreshMeter();
		base.smi.StartSM();
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x060040C0 RID: 16576 RVA: 0x00169D47 File Offset: 0x00167F47
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.ResearchCenters.Remove(this);
	}

	// Token: 0x060040C1 RID: 16577 RVA: 0x00169D5A File Offset: 0x00167F5A
	public string GetResearchType()
	{
		return this.researchTypeID;
	}

	// Token: 0x060040C2 RID: 16578 RVA: 0x00169D62 File Offset: 0x00167F62
	private void OnStorageChange(object data)
	{
		this.RefreshMeter();
	}

	// Token: 0x060040C3 RID: 16579 RVA: 0x00169D6C File Offset: 0x00167F6C
	private void RefreshMeter()
	{
		float positionPercent = Mathf.Clamp01(this.particleStorage.Particles / this.particleStorage.Capacity());
		this.particleMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x060040C4 RID: 16580 RVA: 0x00169DA4 File Offset: 0x00167FA4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.materialPerPoint, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.materialPerPoint, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Requirement, false),
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.researchTypeID).name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.researchTypeID).name), Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04002A28 RID: 10792
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002A29 RID: 10793
	public string researchTypeID;

	// Token: 0x04002A2A RID: 10794
	public float materialPerPoint = 50f;

	// Token: 0x04002A2B RID: 10795
	public float timePerPoint;

	// Token: 0x04002A2C RID: 10796
	public Tag inputMaterial;

	// Token: 0x04002A2D RID: 10797
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x04002A2E RID: 10798
	public Meter.Offset particleMeterOffset;

	// Token: 0x04002A2F RID: 10799
	private MeterController particleMeter;

	// Token: 0x04002A30 RID: 10800
	private static readonly EventSystem.IntraObjectHandler<NuclearResearchCenter> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<NuclearResearchCenter>(delegate(NuclearResearchCenter component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001703 RID: 5891
	public class States : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter>
	{
		// Token: 0x06008D33 RID: 36147 RVA: 0x0031ADF8 File Offset: 0x00318FF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.requirements, false);
			this.requirements.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.requirements.highEnergyParticlesNeeded);
			this.requirements.highEnergyParticlesNeeded.ToggleMainStatusItem(Db.Get().BuildingStatusItems.WaitingForHighEnergyParticles, null).EventTransition(GameHashes.OnParticleStorageChanged, this.requirements.noResearchSelected, new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsReady));
			this.requirements.noResearchSelected.Enter(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				this.UpdateNoResearchSelectedStatusItem(smi, true);
			}).Exit(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				this.UpdateNoResearchSelectedStatusItem(smi, false);
			}).EventTransition(GameHashes.ActiveResearchChanged, this.requirements.noApplicableResearch, new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchSelected));
			this.requirements.noApplicableResearch.EventTransition(GameHashes.ActiveResearchChanged, this.ready, new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchApplicable)).EventTransition(GameHashes.ActiveResearchChanged, this.requirements, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchSelected)));
			this.ready.Enter(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.CreateChore();
			}).TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).Exit(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.DestroyChore();
			}).EventTransition(GameHashes.ActiveResearchChanged, this.requirements.noResearchSelected, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchSelected))).EventTransition(GameHashes.ActiveResearchChanged, this.requirements.noApplicableResearch, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchApplicable))).EventTransition(GameHashes.ResearchPointsChanged, this.requirements.noApplicableResearch, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchApplicable))).EventTransition(GameHashes.OnParticleStorageEmpty, this.requirements.highEnergyParticlesNeeded, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.HasRadiation)));
			this.ready.idle.WorkableStartTransition((NuclearResearchCenter.StatesInstance smi) => smi.master.GetComponent<NuclearResearchCenterWorkable>(), this.ready.working);
			this.ready.working.Enter("SetActive(true)", delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).WorkableStopTransition((NuclearResearchCenter.StatesInstance smi) => smi.master.GetComponent<NuclearResearchCenterWorkable>(), this.ready.idle).WorkableCompleteTransition((NuclearResearchCenter.StatesInstance smi) => smi.master.GetComponent<NuclearResearchCenterWorkable>(), this.ready.idle);
		}

		// Token: 0x06008D34 RID: 36148 RVA: 0x0031B13C File Offset: 0x0031933C
		protected bool IsAllResearchComplete()
		{
			using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsComplete())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06008D35 RID: 36149 RVA: 0x0031B1A0 File Offset: 0x003193A0
		private void UpdateNoResearchSelectedStatusItem(NuclearResearchCenter.StatesInstance smi, bool entering)
		{
			bool flag = entering && !this.IsResearchSelected(smi) && !this.IsAllResearchComplete();
			KSelectable component = smi.GetComponent<KSelectable>();
			if (flag)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.NoResearchSelected, null);
				return;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
		}

		// Token: 0x06008D36 RID: 36150 RVA: 0x0031B20C File Offset: 0x0031940C
		private bool IsReady(NuclearResearchCenter.StatesInstance smi)
		{
			return smi.GetComponent<HighEnergyParticleStorage>().Particles > smi.master.materialPerPoint;
		}

		// Token: 0x06008D37 RID: 36151 RVA: 0x0031B226 File Offset: 0x00319426
		private bool IsResearchSelected(NuclearResearchCenter.StatesInstance smi)
		{
			return Research.Instance.GetActiveResearch() != null;
		}

		// Token: 0x06008D38 RID: 36152 RVA: 0x0031B238 File Offset: 0x00319438
		private bool IsResearchApplicable(NuclearResearchCenter.StatesInstance smi)
		{
			TechInstance activeResearch = Research.Instance.GetActiveResearch();
			if (activeResearch != null && activeResearch.tech.costsByResearchTypeID.ContainsKey(smi.master.researchTypeID))
			{
				float num = activeResearch.progressInventory.PointsByTypeID[smi.master.researchTypeID];
				float num2 = activeResearch.tech.costsByResearchTypeID[smi.master.researchTypeID];
				return num < num2;
			}
			return false;
		}

		// Token: 0x06008D39 RID: 36153 RVA: 0x0031B2AC File Offset: 0x003194AC
		private bool HasRadiation(NuclearResearchCenter.StatesInstance smi)
		{
			return !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty();
		}

		// Token: 0x04006D8A RID: 28042
		public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State inoperational;

		// Token: 0x04006D8B RID: 28043
		public NuclearResearchCenter.States.RequirementsState requirements;

		// Token: 0x04006D8C RID: 28044
		public NuclearResearchCenter.States.ReadyState ready;

		// Token: 0x020021AF RID: 8623
		public class RequirementsState : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State
		{
			// Token: 0x040096DB RID: 38619
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State highEnergyParticlesNeeded;

			// Token: 0x040096DC RID: 38620
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State noResearchSelected;

			// Token: 0x040096DD RID: 38621
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State noApplicableResearch;
		}

		// Token: 0x020021B0 RID: 8624
		public class ReadyState : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State
		{
			// Token: 0x040096DE RID: 38622
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State idle;

			// Token: 0x040096DF RID: 38623
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State working;
		}
	}

	// Token: 0x02001704 RID: 5892
	public class StatesInstance : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.GameInstance
	{
		// Token: 0x06008D3D RID: 36157 RVA: 0x0031B2D8 File Offset: 0x003194D8
		public StatesInstance(NuclearResearchCenter master) : base(master)
		{
		}

		// Token: 0x06008D3E RID: 36158 RVA: 0x0031B2E4 File Offset: 0x003194E4
		public void CreateChore()
		{
			Workable component = base.smi.master.GetComponent<NuclearResearchCenterWorkable>();
			this.chore = new WorkChore<NuclearResearchCenterWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			this.chore.preemption_cb = new Func<Chore.Precondition.Context, bool>(NuclearResearchCenter.StatesInstance.CanPreemptCB);
		}

		// Token: 0x06008D3F RID: 36159 RVA: 0x0031B345 File Offset: 0x00319545
		public void DestroyChore()
		{
			this.chore.Cancel("destroy me!");
			this.chore = null;
		}

		// Token: 0x06008D40 RID: 36160 RVA: 0x0031B360 File Offset: 0x00319560
		private static bool CanPreemptCB(Chore.Precondition.Context context)
		{
			Worker component = context.chore.driver.GetComponent<Worker>();
			float num = Db.Get().AttributeConverters.ResearchSpeed.Lookup(component).Evaluate();
			Worker worker = context.consumerState.worker;
			float num2 = Db.Get().AttributeConverters.ResearchSpeed.Lookup(worker).Evaluate();
			TechInstance activeResearch = Research.Instance.GetActiveResearch();
			if (activeResearch != null)
			{
				NuclearResearchCenter.StatesInstance smi = context.chore.gameObject.GetSMI<NuclearResearchCenter.StatesInstance>();
				if (smi != null)
				{
					return num2 > num && activeResearch.PercentageCompleteResearchType(smi.master.researchTypeID) < 1f;
				}
			}
			return false;
		}

		// Token: 0x04006D8D RID: 28045
		private WorkChore<NuclearResearchCenterWorkable> chore;
	}
}
