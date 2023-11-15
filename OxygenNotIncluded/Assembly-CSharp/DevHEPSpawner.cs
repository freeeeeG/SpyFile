using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005EB RID: 1515
[SerializationConfig(MemberSerialization.OptIn)]
public class DevHEPSpawner : StateMachineComponent<DevHEPSpawner.StatesInstance>, IHighEnergyParticleDirection, ISingleSliderControl, ISliderControl
{
	// Token: 0x170001FE RID: 510
	// (get) Token: 0x060025C7 RID: 9671 RVA: 0x000CD7E0 File Offset: 0x000CB9E0
	// (set) Token: 0x060025C8 RID: 9672 RVA: 0x000CD7E8 File Offset: 0x000CB9E8
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

	// Token: 0x060025C9 RID: 9673 RVA: 0x000CD840 File Offset: 0x000CBA40
	private void OnCopySettings(object data)
	{
		DevHEPSpawner component = ((GameObject)data).GetComponent<DevHEPSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
			this.boltAmount = component.boltAmount;
		}
	}

	// Token: 0x060025CA RID: 9674 RVA: 0x000CD87A File Offset: 0x000CBA7A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<DevHEPSpawner>(-905833192, DevHEPSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x060025CB RID: 9675 RVA: 0x000CD894 File Offset: 0x000CBA94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		this.particleController = new MeterController(base.GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleController.gameObject.AddOrGet<LoopingSounds>();
		this.progressMeterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x060025CC RID: 9676 RVA: 0x000CD934 File Offset: 0x000CBB34
	public void LauncherUpdate(float dt)
	{
		if (this.boltAmount <= 0f)
		{
			return;
		}
		this.launcherTimer += dt;
		this.progressMeterController.SetPositionPercent(this.launcherTimer / 5f);
		if (this.launcherTimer > 5f)
		{
			this.launcherTimer -= 5f;
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.boltAmount;
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Play("orb_send", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Queue("orb_off", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x060025CD RID: 9677 RVA: 0x000CDA73 File Offset: 0x000CBC73
	public string SliderTitleKey
	{
		get
		{
			return "";
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060025CE RID: 9678 RVA: 0x000CDA7A File Offset: 0x000CBC7A
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x060025CF RID: 9679 RVA: 0x000CDA86 File Offset: 0x000CBC86
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x060025D0 RID: 9680 RVA: 0x000CDA89 File Offset: 0x000CBC89
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x060025D1 RID: 9681 RVA: 0x000CDA90 File Offset: 0x000CBC90
	public float GetSliderMax(int index)
	{
		return 500f;
	}

	// Token: 0x060025D2 RID: 9682 RVA: 0x000CDA97 File Offset: 0x000CBC97
	public float GetSliderValue(int index)
	{
		return this.boltAmount;
	}

	// Token: 0x060025D3 RID: 9683 RVA: 0x000CDA9F File Offset: 0x000CBC9F
	public void SetSliderValue(float value, int index)
	{
		this.boltAmount = value;
	}

	// Token: 0x060025D4 RID: 9684 RVA: 0x000CDAA8 File Offset: 0x000CBCA8
	public string GetSliderTooltipKey(int index)
	{
		return "";
	}

	// Token: 0x060025D5 RID: 9685 RVA: 0x000CDAAF File Offset: 0x000CBCAF
	string ISliderControl.GetSliderTooltip(int index)
	{
		return "";
	}

	// Token: 0x04001591 RID: 5521
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001592 RID: 5522
	[Serialize]
	private EightDirection _direction;

	// Token: 0x04001593 RID: 5523
	public float boltAmount;

	// Token: 0x04001594 RID: 5524
	private EightDirectionController directionController;

	// Token: 0x04001595 RID: 5525
	private float launcherTimer;

	// Token: 0x04001596 RID: 5526
	private MeterController particleController;

	// Token: 0x04001597 RID: 5527
	private MeterController progressMeterController;

	// Token: 0x04001598 RID: 5528
	[Serialize]
	public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();

	// Token: 0x04001599 RID: 5529
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400159A RID: 5530
	private static readonly EventSystem.IntraObjectHandler<DevHEPSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DevHEPSpawner>(delegate(DevHEPSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200128B RID: 4747
	public class StatesInstance : GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.GameInstance
	{
		// Token: 0x06007DBC RID: 32188 RVA: 0x002E5889 File Offset: 0x002E3A89
		public StatesInstance(DevHEPSpawner smi) : base(smi)
		{
		}
	}

	// Token: 0x0200128C RID: 4748
	public class States : GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner>
	{
		// Token: 0x06007DBD RID: 32189 RVA: 0x002E5894 File Offset: 0x002E3A94
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.ready.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).Update(delegate(DevHEPSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate(dt);
			}, UpdateRate.SIM_EVERY_TICK, false);
		}

		// Token: 0x04005FFB RID: 24571
		public StateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.BoolParameter isAbsorbingRadiation;

		// Token: 0x04005FFC RID: 24572
		public GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.State ready;

		// Token: 0x04005FFD RID: 24573
		public GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.State inoperational;
	}
}
