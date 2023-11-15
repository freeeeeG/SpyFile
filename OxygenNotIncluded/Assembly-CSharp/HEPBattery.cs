using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007F0 RID: 2032
public class HEPBattery : GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>
{
	// Token: 0x060039C4 RID: 14788 RVA: 0x00142448 File Offset: 0x00140648
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false).Update(delegate(HEPBattery.Instance smi, float dt)
		{
			smi.DoConsumeParticlesWhileDisabled(dt);
			smi.UpdateDecayStatusItem(false);
		}, UpdateRate.SIM_200ms, false);
		this.operational.Enter("SetActive(true)", delegate(HEPBattery.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(HEPBattery.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("on", KAnim.PlayMode.Loop).TagTransition(GameTags.Operational, this.inoperational, true).Update(new Action<HEPBattery.Instance, float>(this.LauncherUpdate), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x060039C5 RID: 14789 RVA: 0x00142530 File Offset: 0x00140730
	public void LauncherUpdate(HEPBattery.Instance smi, float dt)
	{
		smi.UpdateDecayStatusItem(true);
		smi.UpdateMeter(null);
		smi.operational.SetActive(smi.particleStorage.Particles > 0f, false);
		smi.launcherTimer += dt;
		if (smi.launcherTimer < smi.def.minLaunchInterval || !smi.AllowSpawnParticles)
		{
			return;
		}
		if (smi.particleStorage.Particles >= smi.particleThreshold)
		{
			smi.launcherTimer = 0f;
			this.Fire(smi);
		}
	}

	// Token: 0x060039C6 RID: 14790 RVA: 0x001425B8 File Offset: 0x001407B8
	public void Fire(HEPBattery.Instance smi)
	{
		int highEnergyParticleOutputCell = smi.GetComponent<Building>().GetHighEnergyParticleOutputCell();
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
		gameObject.SetActive(true);
		if (gameObject != null)
		{
			HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
			component.payload = smi.particleStorage.ConsumeAndGet(smi.particleThreshold);
			component.SetDirection(smi.def.direction);
		}
	}

	// Token: 0x0400267F RID: 9855
	public static readonly HashedString FIRE_PORT_ID = "HEPBatteryFire";

	// Token: 0x04002680 RID: 9856
	public GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State inoperational;

	// Token: 0x04002681 RID: 9857
	public GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State operational;

	// Token: 0x020015CD RID: 5581
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400698C RID: 27020
		public float particleDecayRate;

		// Token: 0x0400698D RID: 27021
		public float minLaunchInterval;

		// Token: 0x0400698E RID: 27022
		public float minSlider;

		// Token: 0x0400698F RID: 27023
		public float maxSlider;

		// Token: 0x04006990 RID: 27024
		public EightDirection direction;
	}

	// Token: 0x020015CE RID: 5582
	public new class Instance : GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.GameInstance, ISingleSliderControl, ISliderControl
	{
		// Token: 0x06008871 RID: 34929 RVA: 0x0030E404 File Offset: 0x0030C604
		public Instance(IStateMachineTarget master, HEPBattery.Def def) : base(master, def)
		{
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			this.UpdateMeter(null);
		}

		// Token: 0x06008872 RID: 34930 RVA: 0x0030E477 File Offset: 0x0030C677
		public void DoConsumeParticlesWhileDisabled(float dt)
		{
			if (this.m_skipFirstUpdate)
			{
				this.m_skipFirstUpdate = false;
				return;
			}
			this.particleStorage.ConsumeAndGet(dt * base.def.particleDecayRate);
			this.UpdateMeter(null);
		}

		// Token: 0x06008873 RID: 34931 RVA: 0x0030E4A9 File Offset: 0x0030C6A9
		public void UpdateMeter(object data = null)
		{
			this.meterController.SetPositionPercent(this.particleStorage.Particles / this.particleStorage.Capacity());
		}

		// Token: 0x06008874 RID: 34932 RVA: 0x0030E4D0 File Offset: 0x0030C6D0
		public void UpdateDecayStatusItem(bool hasPower)
		{
			if (!hasPower)
			{
				if (this.particleStorage.Particles > 0f)
				{
					if (this.statusHandle == Guid.Empty)
					{
						this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts, null);
						return;
					}
				}
				else if (this.statusHandle != Guid.Empty)
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
					this.statusHandle = Guid.Empty;
					return;
				}
			}
			else if (this.statusHandle != Guid.Empty)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
				this.statusHandle = Guid.Empty;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06008875 RID: 34933 RVA: 0x0030E58A File Offset: 0x0030C78A
		public bool AllowSpawnParticles
		{
			get
			{
				return this.hasLogicWire && this.isLogicActive;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06008876 RID: 34934 RVA: 0x0030E59C File Offset: 0x0030C79C
		public bool HasLogicWire
		{
			get
			{
				return this.hasLogicWire;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06008877 RID: 34935 RVA: 0x0030E5A4 File Offset: 0x0030C7A4
		public bool IsLogicActive
		{
			get
			{
				return this.isLogicActive;
			}
		}

		// Token: 0x06008878 RID: 34936 RVA: 0x0030E5AC File Offset: 0x0030C7AC
		private LogicCircuitNetwork GetNetwork()
		{
			int portCell = base.GetComponent<LogicPorts>().GetPortCell(HEPBattery.FIRE_PORT_ID);
			return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}

		// Token: 0x06008879 RID: 34937 RVA: 0x0030E5DC File Offset: 0x0030C7DC
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == HEPBattery.FIRE_PORT_ID)
			{
				this.isLogicActive = (logicValueChanged.newValue > 0);
				this.hasLogicWire = (this.GetNetwork() != null);
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x0600887A RID: 34938 RVA: 0x0030E620 File Offset: 0x0030C820
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x0600887B RID: 34939 RVA: 0x0030E627 File Offset: 0x0030C827
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
			}
		}

		// Token: 0x0600887C RID: 34940 RVA: 0x0030E633 File Offset: 0x0030C833
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x0600887D RID: 34941 RVA: 0x0030E636 File Offset: 0x0030C836
		public float GetSliderMin(int index)
		{
			return base.def.minSlider;
		}

		// Token: 0x0600887E RID: 34942 RVA: 0x0030E643 File Offset: 0x0030C843
		public float GetSliderMax(int index)
		{
			return base.def.maxSlider;
		}

		// Token: 0x0600887F RID: 34943 RVA: 0x0030E650 File Offset: 0x0030C850
		public float GetSliderValue(int index)
		{
			return this.particleThreshold;
		}

		// Token: 0x06008880 RID: 34944 RVA: 0x0030E658 File Offset: 0x0030C858
		public void SetSliderValue(float value, int index)
		{
			this.particleThreshold = value;
		}

		// Token: 0x06008881 RID: 34945 RVA: 0x0030E661 File Offset: 0x0030C861
		public string GetSliderTooltipKey(int index)
		{
			return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";
		}

		// Token: 0x06008882 RID: 34946 RVA: 0x0030E668 File Offset: 0x0030C868
		string ISliderControl.GetSliderTooltip(int index)
		{
			return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"), this.particleThreshold);
		}

		// Token: 0x04006991 RID: 27025
		[MyCmpReq]
		public HighEnergyParticleStorage particleStorage;

		// Token: 0x04006992 RID: 27026
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04006993 RID: 27027
		[Serialize]
		public float launcherTimer;

		// Token: 0x04006994 RID: 27028
		[Serialize]
		public float particleThreshold = 50f;

		// Token: 0x04006995 RID: 27029
		public bool ShowWorkingStatus;

		// Token: 0x04006996 RID: 27030
		private bool m_skipFirstUpdate = true;

		// Token: 0x04006997 RID: 27031
		private MeterController meterController;

		// Token: 0x04006998 RID: 27032
		private Guid statusHandle = Guid.Empty;

		// Token: 0x04006999 RID: 27033
		private bool hasLogicWire;

		// Token: 0x0400699A RID: 27034
		private bool isLogicActive;
	}
}
