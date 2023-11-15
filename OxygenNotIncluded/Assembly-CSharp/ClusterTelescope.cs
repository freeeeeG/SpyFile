using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005CB RID: 1483
public class ClusterTelescope : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>
{
	// Token: 0x0600249C RID: 9372 RVA: 0x000C7968 File Offset: 0x000C5B68
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.ready.no_visibility;
		this.root.Update(delegate(ClusterTelescope.Instance smi, float dt)
		{
			KSelectable component = smi.GetComponent<KSelectable>();
			bool flag = Mathf.Approximately(0f, smi.PercentClear) || smi.PercentClear < 0f;
			bool flag2 = Mathf.Approximately(1f, smi.PercentClear) || smi.PercentClear > 1f;
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, flag, smi);
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, !flag && !flag2, smi);
		}, UpdateRate.SIM_200ms, false);
		this.ready.DoNothing();
		this.ready.no_visibility.UpdateTransition(this.ready.ready_to_work, (ClusterTelescope.Instance smi, float dt) => smi.HasSkyVisibility(), UpdateRate.SIM_200ms, false);
		this.ready.ready_to_work.UpdateTransition(this.ready.no_visibility, (ClusterTelescope.Instance smi, float dt) => !smi.HasSkyVisibility(), UpdateRate.SIM_200ms, false).DefaultState(this.ready.ready_to_work.decide);
		this.ready.ready_to_work.decide.EnterTransition(this.ready.ready_to_work.identifyMeteorShower, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification()).EnterTransition(this.ready.ready_to_work.revealTile, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnRevealingTile()).EnterTransition(this.all_work_complete, (ClusterTelescope.Instance smi) => !smi.IsAnyAvailableWorkToBeDone());
		this.ready.ready_to_work.identifyMeteorShower.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnMeteorIdentification()).ParamTransition<GameObject>(this.meteorShowerTarget, this.ready.ready_to_work.decide, GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.IsNull).EventTransition(GameHashes.ClusterMapMeteorShowerIdentified, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnMeteorIdentification()).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnMeteorIdentification()).ToggleChore((ClusterTelescope.Instance smi) => smi.CreateIdentifyMeteorChore(), this.ready.no_visibility);
		this.ready.ready_to_work.revealTile.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification()).EventTransition(GameHashes.ClusterFogOfWarRevealed, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnRevealingTile()).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification()).ToggleChore((ClusterTelescope.Instance smi) => smi.CreateRevealTileChore(), this.ready.no_visibility);
		this.all_work_complete.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.no_visibility, (ClusterTelescope.Instance smi) => smi.IsAnyAvailableWorkToBeDone()).ToggleMainStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeAllWorkComplete, null).EventTransition(GameHashes.ClusterLocationChanged, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.no_visibility, (ClusterTelescope.Instance smi) => smi.IsAnyAvailableWorkToBeDone()).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.no_visibility, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification());
	}

	// Token: 0x040014FA RID: 5370
	public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State all_work_complete;

	// Token: 0x040014FB RID: 5371
	public ClusterTelescope.ReadyStates ready;

	// Token: 0x040014FC RID: 5372
	public StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.TargetParameter meteorShowerTarget;

	// Token: 0x040014FD RID: 5373
	public StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Signal MeteorIdenificationPriorityChangeSignal;

	// Token: 0x02001262 RID: 4706
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005F62 RID: 24418
		public int clearScanCellRadius = 15;

		// Token: 0x04005F63 RID: 24419
		public int analyzeClusterRadius = 3;

		// Token: 0x04005F64 RID: 24420
		public KAnimFile[] workableOverrideAnims;

		// Token: 0x04005F65 RID: 24421
		public bool providesOxygen;

		// Token: 0x04005F66 RID: 24422
		public SkyVisibilityInfo skyVisibilityInfo;
	}

	// Token: 0x02001263 RID: 4707
	public class WorkStates : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State
	{
		// Token: 0x04005F67 RID: 24423
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State decide;

		// Token: 0x04005F68 RID: 24424
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State identifyMeteorShower;

		// Token: 0x04005F69 RID: 24425
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State revealTile;
	}

	// Token: 0x02001264 RID: 4708
	public class ReadyStates : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State
	{
		// Token: 0x04005F6A RID: 24426
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State no_visibility;

		// Token: 0x04005F6B RID: 24427
		public ClusterTelescope.WorkStates ready_to_work;
	}

	// Token: 0x02001265 RID: 4709
	public new class Instance : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.GameInstance, ICheckboxControl, BuildingStatusItems.ISkyVisInfo
	{
		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06007CF3 RID: 31987 RVA: 0x002E2B24 File Offset: 0x002E0D24
		public float PercentClear
		{
			get
			{
				return this.m_percentClear;
			}
		}

		// Token: 0x06007CF4 RID: 31988 RVA: 0x002E2B2C File Offset: 0x002E0D2C
		float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
		{
			return this.m_percentClear;
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06007CF5 RID: 31989 RVA: 0x002E2B34 File Offset: 0x002E0D34
		private bool hasMeteorShowerTarget
		{
			get
			{
				return this.meteorShowerTarget != null;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06007CF6 RID: 31990 RVA: 0x002E2B3F File Offset: 0x002E0D3F
		private ClusterMapMeteorShower.Instance meteorShowerTarget
		{
			get
			{
				GameObject gameObject = base.sm.meteorShowerTarget.Get(this);
				if (gameObject == null)
				{
					return null;
				}
				return gameObject.GetSMI<ClusterMapMeteorShower.Instance>();
			}
		}

		// Token: 0x06007CF7 RID: 31991 RVA: 0x002E2B5D File Offset: 0x002E0D5D
		public Instance(IStateMachineTarget smi, ClusterTelescope.Def def) : base(smi, def)
		{
			this.workableOverrideAnims = def.workableOverrideAnims;
			this.providesOxygen = def.providesOxygen;
		}

		// Token: 0x06007CF8 RID: 31992 RVA: 0x002E2B86 File Offset: 0x002E0D86
		public bool ShouldBeWorkingOnRevealingTile()
		{
			return this.CheckHasAnalyzeTarget() && (!this.allowMeteorIdentification || !this.CheckHasValidMeteorTarget());
		}

		// Token: 0x06007CF9 RID: 31993 RVA: 0x002E2BA5 File Offset: 0x002E0DA5
		public bool ShouldBeWorkingOnMeteorIdentification()
		{
			return this.allowMeteorIdentification && this.CheckHasValidMeteorTarget();
		}

		// Token: 0x06007CFA RID: 31994 RVA: 0x002E2BB7 File Offset: 0x002E0DB7
		public bool IsAnyAvailableWorkToBeDone()
		{
			return this.CheckHasAnalyzeTarget() || this.ShouldBeWorkingOnMeteorIdentification();
		}

		// Token: 0x06007CFB RID: 31995 RVA: 0x002E2BCC File Offset: 0x002E0DCC
		public bool CheckHasValidMeteorTarget()
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			if (this.HasValidMeteor())
			{
				return true;
			}
			ClusterMapMeteorShower.Instance instance = null;
			AxialI myWorldLocation = this.GetMyWorldLocation();
			ClusterGrid.Instance.GetVisibleUnidentifiedMeteorShowerWithinRadius(myWorldLocation, base.def.analyzeClusterRadius, out instance);
			base.sm.meteorShowerTarget.Set((instance == null) ? null : instance.gameObject, this, false);
			return instance != null;
		}

		// Token: 0x06007CFC RID: 31996 RVA: 0x002E2C34 File Offset: 0x002E0E34
		public bool CheckHasAnalyzeTarget()
		{
			ClusterFogOfWarManager.Instance smi = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			if (this.m_hasAnalyzeTarget && !smi.IsLocationRevealed(this.m_analyzeTarget))
			{
				return true;
			}
			AxialI myWorldLocation = this.GetMyWorldLocation();
			this.m_hasAnalyzeTarget = smi.GetUnrevealedLocationWithinRadius(myWorldLocation, base.def.analyzeClusterRadius, out this.m_analyzeTarget);
			return this.m_hasAnalyzeTarget;
		}

		// Token: 0x06007CFD RID: 31997 RVA: 0x002E2C90 File Offset: 0x002E0E90
		private bool HasValidMeteor()
		{
			if (!this.hasMeteorShowerTarget)
			{
				return false;
			}
			AxialI myWorldLocation = this.GetMyWorldLocation();
			bool flag = ClusterGrid.Instance.IsInRange(this.meteorShowerTarget.ClusterGridPosition(), myWorldLocation, base.def.analyzeClusterRadius);
			bool flag2 = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().IsLocationRevealed(this.meteorShowerTarget.ClusterGridPosition());
			bool hasBeenIdentified = this.meteorShowerTarget.HasBeenIdentified;
			return flag && flag2 && !hasBeenIdentified;
		}

		// Token: 0x06007CFE RID: 31998 RVA: 0x002E2D08 File Offset: 0x002E0F08
		public Chore CreateRevealTileChore()
		{
			WorkChore<ClusterTelescope.ClusterTelescopeWorkable> workChore = new WorkChore<ClusterTelescope.ClusterTelescopeWorkable>(Db.Get().ChoreTypes.Research, this.m_workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			if (this.providesOxygen)
			{
				workChore.AddPrecondition(Telescope.ContainsOxygen, null);
			}
			return workChore;
		}

		// Token: 0x06007CFF RID: 31999 RVA: 0x002E2D58 File Offset: 0x002E0F58
		public Chore CreateIdentifyMeteorChore()
		{
			WorkChore<ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable> workChore = new WorkChore<ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable>(Db.Get().ChoreTypes.Research, this.m_identifyMeteorWorkable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			if (this.providesOxygen)
			{
				workChore.AddPrecondition(Telescope.ContainsOxygen, null);
			}
			return workChore;
		}

		// Token: 0x06007D00 RID: 32000 RVA: 0x002E2DA6 File Offset: 0x002E0FA6
		public ClusterMapMeteorShower.Instance GetMeteorTarget()
		{
			return this.meteorShowerTarget;
		}

		// Token: 0x06007D01 RID: 32001 RVA: 0x002E2DAE File Offset: 0x002E0FAE
		public AxialI GetAnalyzeTarget()
		{
			global::Debug.Assert(this.m_hasAnalyzeTarget, "GetAnalyzeTarget called but this telescope has no target assigned.");
			return this.m_analyzeTarget;
		}

		// Token: 0x06007D02 RID: 32002 RVA: 0x002E2DC8 File Offset: 0x002E0FC8
		public bool HasSkyVisibility()
		{
			ValueTuple<bool, float> visibilityOf = base.def.skyVisibilityInfo.GetVisibilityOf(base.gameObject);
			bool item = visibilityOf.Item1;
			float item2 = visibilityOf.Item2;
			this.m_percentClear = item2;
			return item;
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06007D03 RID: 32003 RVA: 0x002E2E00 File Offset: 0x002E1000
		public string CheckboxTitleKey
		{
			get
			{
				return "STRINGS.UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.TITLE";
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06007D04 RID: 32004 RVA: 0x002E2E07 File Offset: 0x002E1007
		public string CheckboxLabel
		{
			get
			{
				return UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.CHECKBOX_METEORS;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06007D05 RID: 32005 RVA: 0x002E2E13 File Offset: 0x002E1013
		public string CheckboxTooltip
		{
			get
			{
				return UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.CHECKBOX_TOOLTIP_METEORS;
			}
		}

		// Token: 0x06007D06 RID: 32006 RVA: 0x002E2E1F File Offset: 0x002E101F
		public bool GetCheckboxValue()
		{
			return this.allowMeteorIdentification;
		}

		// Token: 0x06007D07 RID: 32007 RVA: 0x002E2E27 File Offset: 0x002E1027
		public void SetCheckboxValue(bool value)
		{
			this.allowMeteorIdentification = value;
			base.sm.MeteorIdenificationPriorityChangeSignal.Trigger(this);
		}

		// Token: 0x04005F6C RID: 24428
		private float m_percentClear;

		// Token: 0x04005F6D RID: 24429
		[Serialize]
		public bool allowMeteorIdentification = true;

		// Token: 0x04005F6E RID: 24430
		[Serialize]
		private bool m_hasAnalyzeTarget;

		// Token: 0x04005F6F RID: 24431
		[Serialize]
		private AxialI m_analyzeTarget;

		// Token: 0x04005F70 RID: 24432
		[MyCmpAdd]
		private ClusterTelescope.ClusterTelescopeWorkable m_workable;

		// Token: 0x04005F71 RID: 24433
		[MyCmpAdd]
		private ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable m_identifyMeteorWorkable;

		// Token: 0x04005F72 RID: 24434
		public KAnimFile[] workableOverrideAnims;

		// Token: 0x04005F73 RID: 24435
		public bool providesOxygen;
	}

	// Token: 0x02001266 RID: 4710
	public class ClusterTelescopeWorkable : Workable, OxygenBreather.IGasProvider
	{
		// Token: 0x06007D08 RID: 32008 RVA: 0x002E2E44 File Offset: 0x002E1044
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
			this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
			this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
			this.requiredSkillPerk = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.radiationShielding = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING, STRINGS.BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME, false, false, true);
		}

		// Token: 0x06007D09 RID: 32009 RVA: 0x002E2EEF File Offset: 0x002E10EF
		protected override void OnCleanUp()
		{
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.OnCleanUp();
		}

		// Token: 0x06007D0A RID: 32010 RVA: 0x002E2F10 File Offset: 0x002E1110
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			base.SetWorkTime(float.PositiveInfinity);
			this.overrideAnims = this.m_telescope.workableOverrideAnims;
		}

		// Token: 0x06007D0B RID: 32011 RVA: 0x002E2F74 File Offset: 0x002E1174
		private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
		{
			Worker worker = base.worker;
			if (worker == null)
			{
				return;
			}
			KPrefabID component = worker.GetComponent<KPrefabID>();
			OxygenBreather component2 = worker.GetComponent<OxygenBreather>();
			Klei.AI.Attributes attributes = worker.GetAttributes();
			KSelectable component3 = base.GetComponent<KSelectable>();
			if (ev == Workable.WorkableEvent.WorkStarted)
			{
				base.ShowProgressBar(true);
				this.progressBar.SetUpdateFunc(() => this.m_fowManager.GetRevealCompleteFraction(this.currentTarget));
				this.currentTarget = this.m_telescope.GetAnalyzeTarget();
				if (!ClusterGrid.Instance.GetEntityOfLayerAtCell(this.currentTarget, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(this.currentTarget);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().SetTargetMeteorShower(null);
				}
				if (this.m_telescope.providesOxygen)
				{
					attributes.Add(this.radiationShielding);
					this.workerGasProvider = component2.GetGasProvider();
					component2.SetGasProvider(this);
					component2.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
					component.AddTag(GameTags.Shaded, false);
				}
				base.GetComponent<Operational>().SetActive(true, false);
				this.checkMarkerFrequency = UnityEngine.Random.Range(2f, 5f);
				component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
				return;
			}
			if (ev != Workable.WorkableEvent.WorkStopped)
			{
				return;
			}
			if (this.m_telescope.providesOxygen)
			{
				attributes.Remove(this.radiationShielding);
				component2.SetGasProvider(this.workerGasProvider);
				component2.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
				component.RemoveTag(GameTags.Shaded);
			}
			base.GetComponent<Operational>().SetActive(false, false);
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.ShowProgressBar(false);
			component3.RemoveStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
		}

		// Token: 0x06007D0C RID: 32012 RVA: 0x002E3158 File Offset: 0x002E1358
		public override List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> descriptors = base.GetDescriptors(go);
			Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
			descriptors.Add(item);
			return descriptors;
		}

		// Token: 0x06007D0D RID: 32013 RVA: 0x002E31B3 File Offset: 0x002E13B3
		public override float GetEfficiencyMultiplier(Worker worker)
		{
			return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.m_telescope.PercentClear);
		}

		// Token: 0x06007D0E RID: 32014 RVA: 0x002E31D0 File Offset: 0x002E13D0
		protected override bool OnWorkTick(Worker worker, float dt)
		{
			AxialI analyzeTarget = this.m_telescope.GetAnalyzeTarget();
			bool flag = false;
			if (analyzeTarget != this.currentTarget)
			{
				if (this.telescopeTargetMarker)
				{
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(analyzeTarget);
				}
				this.currentTarget = analyzeTarget;
				flag = true;
			}
			if (!flag && this.checkMarkerTimer > this.checkMarkerFrequency)
			{
				this.checkMarkerTimer = 0f;
				if (!this.telescopeTargetMarker && !ClusterGrid.Instance.GetEntityOfLayerAtCell(this.currentTarget, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(this.currentTarget);
				}
			}
			this.checkMarkerTimer += dt;
			float num = ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL / ROCKETRY.CLUSTER_FOW.DEFAULT_CYCLES_PER_REVEAL / 600f;
			float points = dt * num;
			this.m_fowManager.EarnRevealPointsForLocation(this.currentTarget, points);
			return base.OnWorkTick(worker, dt);
		}

		// Token: 0x06007D0F RID: 32015 RVA: 0x002E32DE File Offset: 0x002E14DE
		public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06007D10 RID: 32016 RVA: 0x002E32E0 File Offset: 0x002E14E0
		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06007D11 RID: 32017 RVA: 0x002E32E2 File Offset: 0x002E14E2
		public bool ShouldEmitCO2()
		{
			return false;
		}

		// Token: 0x06007D12 RID: 32018 RVA: 0x002E32E5 File Offset: 0x002E14E5
		public bool ShouldStoreCO2()
		{
			return false;
		}

		// Token: 0x06007D13 RID: 32019 RVA: 0x002E32E8 File Offset: 0x002E14E8
		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			if (this.storage.items.Count <= 0)
			{
				return false;
			}
			GameObject gameObject = this.storage.items[0];
			if (gameObject == null)
			{
				return false;
			}
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			bool result = component.Mass >= amount;
			component.Mass = Mathf.Max(0f, component.Mass - amount);
			return result;
		}

		// Token: 0x06007D14 RID: 32020 RVA: 0x002E3354 File Offset: 0x002E1554
		public bool IsLowOxygen()
		{
			if (this.storage.items.Count <= 0)
			{
				return true;
			}
			PrimaryElement primaryElement = this.storage.FindFirstWithMass(GameTags.Breathable, 0f);
			return primaryElement == null || primaryElement.Mass == 0f;
		}

		// Token: 0x04005F74 RID: 24436
		[MySmiReq]
		private ClusterTelescope.Instance m_telescope;

		// Token: 0x04005F75 RID: 24437
		private ClusterFogOfWarManager.Instance m_fowManager;

		// Token: 0x04005F76 RID: 24438
		private GameObject telescopeTargetMarker;

		// Token: 0x04005F77 RID: 24439
		private AxialI currentTarget;

		// Token: 0x04005F78 RID: 24440
		private OxygenBreather.IGasProvider workerGasProvider;

		// Token: 0x04005F79 RID: 24441
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04005F7A RID: 24442
		private AttributeModifier radiationShielding;

		// Token: 0x04005F7B RID: 24443
		private float checkMarkerTimer;

		// Token: 0x04005F7C RID: 24444
		private float checkMarkerFrequency = 1f;
	}

	// Token: 0x02001267 RID: 4711
	public class ClusterTelescopeIdentifyMeteorWorkable : Workable, OxygenBreather.IGasProvider
	{
		// Token: 0x06007D17 RID: 32023 RVA: 0x002E33CC File Offset: 0x002E15CC
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
			this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
			this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
			this.requiredSkillPerk = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.radiationShielding = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING, STRINGS.BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME, false, false, true);
		}

		// Token: 0x06007D18 RID: 32024 RVA: 0x002E3477 File Offset: 0x002E1677
		protected override void OnCleanUp()
		{
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.OnCleanUp();
		}

		// Token: 0x06007D19 RID: 32025 RVA: 0x002E3498 File Offset: 0x002E1698
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			base.SetWorkTime(float.PositiveInfinity);
			this.overrideAnims = this.m_telescope.workableOverrideAnims;
		}

		// Token: 0x06007D1A RID: 32026 RVA: 0x002E34FC File Offset: 0x002E16FC
		private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
		{
			Worker worker = base.worker;
			if (worker == null)
			{
				return;
			}
			KPrefabID component = worker.GetComponent<KPrefabID>();
			OxygenBreather component2 = worker.GetComponent<OxygenBreather>();
			Klei.AI.Attributes attributes = worker.GetAttributes();
			KSelectable component3 = base.GetComponent<KSelectable>();
			if (ev == Workable.WorkableEvent.WorkStarted)
			{
				base.ShowProgressBar(true);
				this.progressBar.SetUpdateFunc(delegate
				{
					if (this.currentTarget == null)
					{
						return 0f;
					}
					return this.currentTarget.IdentifyingProgress;
				});
				this.currentTarget = this.m_telescope.GetMeteorTarget();
				AxialI axialI = this.currentTarget.ClusterGridPosition();
				if (!ClusterGrid.Instance.GetEntityOfLayerAtCell(axialI, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					TelescopeTarget component4 = this.telescopeTargetMarker.GetComponent<TelescopeTarget>();
					component4.Init(axialI);
					component4.SetTargetMeteorShower(this.currentTarget);
				}
				if (this.m_telescope.providesOxygen)
				{
					attributes.Add(this.radiationShielding);
					this.workerGasProvider = component2.GetGasProvider();
					component2.SetGasProvider(this);
					component2.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
					component.AddTag(GameTags.Shaded, false);
				}
				base.GetComponent<Operational>().SetActive(true, false);
				this.checkMarkerFrequency = UnityEngine.Random.Range(2f, 5f);
				component3.AddStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeMeteorWorking, this);
				return;
			}
			if (ev != Workable.WorkableEvent.WorkStopped)
			{
				return;
			}
			if (this.m_telescope.providesOxygen)
			{
				attributes.Remove(this.radiationShielding);
				component2.SetGasProvider(this.workerGasProvider);
				component2.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
				component.RemoveTag(GameTags.Shaded);
			}
			base.GetComponent<Operational>().SetActive(false, false);
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.ShowProgressBar(false);
			component3.RemoveStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeMeteorWorking, this);
		}

		// Token: 0x06007D1B RID: 32027 RVA: 0x002E36E0 File Offset: 0x002E18E0
		public override List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> descriptors = base.GetDescriptors(go);
			Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
			descriptors.Add(item);
			return descriptors;
		}

		// Token: 0x06007D1C RID: 32028 RVA: 0x002E373C File Offset: 0x002E193C
		protected override bool OnWorkTick(Worker worker, float dt)
		{
			ClusterMapMeteorShower.Instance meteorTarget = this.m_telescope.GetMeteorTarget();
			AxialI axialI = meteorTarget.ClusterGridPosition();
			bool flag = false;
			if (meteorTarget != this.currentTarget)
			{
				if (this.telescopeTargetMarker)
				{
					TelescopeTarget component = this.telescopeTargetMarker.GetComponent<TelescopeTarget>();
					component.Init(axialI);
					component.SetTargetMeteorShower(meteorTarget);
				}
				this.currentTarget = meteorTarget;
				flag = true;
			}
			if (!flag && this.checkMarkerTimer > this.checkMarkerFrequency)
			{
				this.checkMarkerTimer = 0f;
				if (!this.telescopeTargetMarker && !ClusterGrid.Instance.GetEntityOfLayerAtCell(axialI, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(axialI);
				}
			}
			this.checkMarkerTimer += dt;
			float num = 20f;
			float points = dt / num;
			this.currentTarget.ProgressIdentifiction(points);
			return base.OnWorkTick(worker, dt);
		}

		// Token: 0x06007D1D RID: 32029 RVA: 0x002E3839 File Offset: 0x002E1A39
		public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06007D1E RID: 32030 RVA: 0x002E383B File Offset: 0x002E1A3B
		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06007D1F RID: 32031 RVA: 0x002E383D File Offset: 0x002E1A3D
		public bool ShouldEmitCO2()
		{
			return false;
		}

		// Token: 0x06007D20 RID: 32032 RVA: 0x002E3840 File Offset: 0x002E1A40
		public bool ShouldStoreCO2()
		{
			return false;
		}

		// Token: 0x06007D21 RID: 32033 RVA: 0x002E3844 File Offset: 0x002E1A44
		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			if (this.storage.items.Count <= 0)
			{
				return false;
			}
			GameObject gameObject = this.storage.items[0];
			if (gameObject == null)
			{
				return false;
			}
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			bool result = component.Mass >= amount;
			component.Mass = Mathf.Max(0f, component.Mass - amount);
			return result;
		}

		// Token: 0x06007D22 RID: 32034 RVA: 0x002E38B0 File Offset: 0x002E1AB0
		public bool IsLowOxygen()
		{
			if (this.storage.items.Count <= 0)
			{
				return true;
			}
			GameObject gameObject = this.storage.items[0];
			return !(gameObject == null) && gameObject.GetComponent<PrimaryElement>().Mass > 0f;
		}

		// Token: 0x04005F7D RID: 24445
		[MySmiReq]
		private ClusterTelescope.Instance m_telescope;

		// Token: 0x04005F7E RID: 24446
		private ClusterFogOfWarManager.Instance m_fowManager;

		// Token: 0x04005F7F RID: 24447
		private GameObject telescopeTargetMarker;

		// Token: 0x04005F80 RID: 24448
		private ClusterMapMeteorShower.Instance currentTarget;

		// Token: 0x04005F81 RID: 24449
		private OxygenBreather.IGasProvider workerGasProvider;

		// Token: 0x04005F82 RID: 24450
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04005F83 RID: 24451
		private AttributeModifier radiationShielding;

		// Token: 0x04005F84 RID: 24452
		private float checkMarkerTimer;

		// Token: 0x04005F85 RID: 24453
		private float checkMarkerFrequency = 1f;
	}
}
