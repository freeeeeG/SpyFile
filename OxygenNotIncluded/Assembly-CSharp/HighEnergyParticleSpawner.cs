using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000617 RID: 1559
[SerializationConfig(MemberSerialization.OptIn)]
public class HighEnergyParticleSpawner : StateMachineComponent<HighEnergyParticleSpawner.StatesInstance>, IHighEnergyParticleDirection, IProgressBarSideScreen, ISingleSliderControl, ISliderControl
{
	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06002747 RID: 10055 RVA: 0x000D5524 File Offset: 0x000D3724
	public float PredictedPerCycleConsumptionRate
	{
		get
		{
			return (float)Mathf.FloorToInt(this.recentPerSecondConsumptionRate * 0.1f * 600f);
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06002748 RID: 10056 RVA: 0x000D553E File Offset: 0x000D373E
	// (set) Token: 0x06002749 RID: 10057 RVA: 0x000D5548 File Offset: 0x000D3748
	public EightDirection Direction
	{
		get
		{
			return this._direction;
		}
		set
		{
			this._direction = value;
			if (this.directionController != null)
			{
				this.directionController.SetRotation((float)(45 * EightDirectionUtil.GetDirectionIndex(this._direction)));
				this.directionController.controller.enabled = false;
				this.directionController.controller.enabled = true;
			}
		}
	}

	// Token: 0x0600274A RID: 10058 RVA: 0x000D55A0 File Offset: 0x000D37A0
	private void OnCopySettings(object data)
	{
		HighEnergyParticleSpawner component = ((GameObject)data).GetComponent<HighEnergyParticleSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
			this.particleThreshold = component.particleThreshold;
		}
	}

	// Token: 0x0600274B RID: 10059 RVA: 0x000D55DA File Offset: 0x000D37DA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HighEnergyParticleSpawner>(-905833192, HighEnergyParticleSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x0600274C RID: 10060 RVA: 0x000D55F4 File Offset: 0x000D37F4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		this.particleController = new MeterController(base.GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleController.gameObject.AddOrGet<LoopingSounds>();
		this.progressMeterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x0600274D RID: 10061 RVA: 0x000D569F File Offset: 0x000D389F
	public float GetProgressBarMaxValue()
	{
		return this.particleThreshold;
	}

	// Token: 0x0600274E RID: 10062 RVA: 0x000D56A7 File Offset: 0x000D38A7
	public float GetProgressBarFillPercentage()
	{
		return this.particleStorage.Particles / this.particleThreshold;
	}

	// Token: 0x0600274F RID: 10063 RVA: 0x000D56BB File Offset: 0x000D38BB
	public string GetProgressBarTitleLabel()
	{
		return UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.PROGRESS_BAR_LABEL;
	}

	// Token: 0x06002750 RID: 10064 RVA: 0x000D56C8 File Offset: 0x000D38C8
	public string GetProgressBarLabel()
	{
		return Mathf.FloorToInt(this.particleStorage.Particles).ToString() + "/" + Mathf.FloorToInt(this.particleThreshold).ToString();
	}

	// Token: 0x06002751 RID: 10065 RVA: 0x000D570A File Offset: 0x000D390A
	public string GetProgressBarTooltip()
	{
		return UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.PROGRESS_BAR_TOOLTIP;
	}

	// Token: 0x06002752 RID: 10066 RVA: 0x000D5716 File Offset: 0x000D3916
	public void DoConsumeParticlesWhileDisabled(float dt)
	{
		this.particleStorage.ConsumeAndGet(dt * 1f);
		this.progressMeterController.SetPositionPercent(this.GetProgressBarFillPercentage());
	}

	// Token: 0x06002753 RID: 10067 RVA: 0x000D573C File Offset: 0x000D393C
	public void LauncherUpdate(float dt)
	{
		this.radiationSampleTimer += dt;
		if (this.radiationSampleTimer >= this.radiationSampleRate)
		{
			this.radiationSampleTimer -= this.radiationSampleRate;
			int i = Grid.PosToCell(this);
			float num = Grid.Radiation[i];
			if (num != 0f && this.particleStorage.RemainingCapacity() > 0f)
			{
				base.smi.sm.isAbsorbingRadiation.Set(true, base.smi, false);
				this.recentPerSecondConsumptionRate = num / 600f;
				this.particleStorage.Store(this.recentPerSecondConsumptionRate * this.radiationSampleRate * 0.1f);
			}
			else
			{
				this.recentPerSecondConsumptionRate = 0f;
				base.smi.sm.isAbsorbingRadiation.Set(false, base.smi, false);
			}
		}
		this.progressMeterController.SetPositionPercent(this.GetProgressBarFillPercentage());
		if (!this.particleVisualPlaying && this.particleStorage.Particles > this.particleThreshold / 2f)
		{
			this.particleController.meterController.Play("orb_pre", KAnim.PlayMode.Once, 1f, 0f);
			this.particleController.meterController.Queue("orb_idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.particleVisualPlaying = true;
		}
		this.launcherTimer += dt;
		if (this.launcherTimer < this.minLaunchInterval)
		{
			return;
		}
		if (this.particleStorage.Particles >= this.particleThreshold)
		{
			this.launcherTimer = 0f;
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.particleStorage.ConsumeAndGet(this.particleThreshold);
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Play("orb_send", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Queue("orb_off", KAnim.PlayMode.Once, 1f, 0f);
				this.particleVisualPlaying = false;
			}
		}
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06002754 RID: 10068 RVA: 0x000D59CD File Offset: 0x000D3BCD
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";
		}
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06002755 RID: 10069 RVA: 0x000D59D4 File Offset: 0x000D3BD4
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x06002756 RID: 10070 RVA: 0x000D59E0 File Offset: 0x000D3BE0
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06002757 RID: 10071 RVA: 0x000D59E3 File Offset: 0x000D3BE3
	public float GetSliderMin(int index)
	{
		return (float)this.minSlider;
	}

	// Token: 0x06002758 RID: 10072 RVA: 0x000D59EC File Offset: 0x000D3BEC
	public float GetSliderMax(int index)
	{
		return (float)this.maxSlider;
	}

	// Token: 0x06002759 RID: 10073 RVA: 0x000D59F5 File Offset: 0x000D3BF5
	public float GetSliderValue(int index)
	{
		return this.particleThreshold;
	}

	// Token: 0x0600275A RID: 10074 RVA: 0x000D59FD File Offset: 0x000D3BFD
	public void SetSliderValue(float value, int index)
	{
		this.particleThreshold = value;
	}

	// Token: 0x0600275B RID: 10075 RVA: 0x000D5A06 File Offset: 0x000D3C06
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";
	}

	// Token: 0x0600275C RID: 10076 RVA: 0x000D5A0D File Offset: 0x000D3C0D
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"), this.particleThreshold);
	}

	// Token: 0x0400169E RID: 5790
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x0400169F RID: 5791
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040016A0 RID: 5792
	private float recentPerSecondConsumptionRate;

	// Token: 0x040016A1 RID: 5793
	public int minSlider;

	// Token: 0x040016A2 RID: 5794
	public int maxSlider;

	// Token: 0x040016A3 RID: 5795
	[Serialize]
	private EightDirection _direction;

	// Token: 0x040016A4 RID: 5796
	public float minLaunchInterval;

	// Token: 0x040016A5 RID: 5797
	public float radiationSampleRate;

	// Token: 0x040016A6 RID: 5798
	[Serialize]
	public float particleThreshold = 50f;

	// Token: 0x040016A7 RID: 5799
	private EightDirectionController directionController;

	// Token: 0x040016A8 RID: 5800
	private float launcherTimer;

	// Token: 0x040016A9 RID: 5801
	private float radiationSampleTimer;

	// Token: 0x040016AA RID: 5802
	private MeterController particleController;

	// Token: 0x040016AB RID: 5803
	private bool particleVisualPlaying;

	// Token: 0x040016AC RID: 5804
	private MeterController progressMeterController;

	// Token: 0x040016AD RID: 5805
	[Serialize]
	public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();

	// Token: 0x040016AE RID: 5806
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040016AF RID: 5807
	private static readonly EventSystem.IntraObjectHandler<HighEnergyParticleSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HighEnergyParticleSpawner>(delegate(HighEnergyParticleSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x020012D8 RID: 4824
	public class StatesInstance : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.GameInstance
	{
		// Token: 0x06007ECB RID: 32459 RVA: 0x002E9F43 File Offset: 0x002E8143
		public StatesInstance(HighEnergyParticleSpawner smi) : base(smi)
		{
		}
	}

	// Token: 0x020012D9 RID: 4825
	public class States : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner>
	{
		// Token: 0x06007ECC RID: 32460 RVA: 0x002E9F4C File Offset: 0x002E814C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false).DefaultState(this.inoperational.empty);
			this.inoperational.empty.EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational.losing, (HighEnergyParticleSpawner.StatesInstance smi) => !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
			this.inoperational.losing.ToggleStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts, null).Update(delegate(HighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.DoConsumeParticlesWhileDisabled(dt);
			}, UpdateRate.SIM_1000ms, false).EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational.empty, (HighEnergyParticleSpawner.StatesInstance smi) => smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).Update(delegate(HighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate(dt);
			}, UpdateRate.SIM_EVERY_TICK, false);
			this.ready.idle.ParamTransition<bool>(this.isAbsorbingRadiation, this.ready.absorbing, GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.IsTrue).PlayAnim("on");
			this.ready.absorbing.Enter("SetActive(true)", delegate(HighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(HighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ParamTransition<bool>(this.isAbsorbingRadiation, this.ready.idle, GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.CollectingHEP, (HighEnergyParticleSpawner.StatesInstance smi) => smi.master).PlayAnim("working_loop", KAnim.PlayMode.Loop);
		}

		// Token: 0x040060E7 RID: 24807
		public StateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.BoolParameter isAbsorbingRadiation;

		// Token: 0x040060E8 RID: 24808
		public HighEnergyParticleSpawner.States.ReadyStates ready;

		// Token: 0x040060E9 RID: 24809
		public HighEnergyParticleSpawner.States.InoperationalStates inoperational;

		// Token: 0x020020E2 RID: 8418
		public class InoperationalStates : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State
		{
			// Token: 0x04009327 RID: 37671
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State empty;

			// Token: 0x04009328 RID: 37672
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State losing;
		}

		// Token: 0x020020E3 RID: 8419
		public class ReadyStates : GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State
		{
			// Token: 0x04009329 RID: 37673
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State idle;

			// Token: 0x0400932A RID: 37674
			public GameStateMachine<HighEnergyParticleSpawner.States, HighEnergyParticleSpawner.StatesInstance, HighEnergyParticleSpawner, object>.State absorbing;
		}
	}
}
