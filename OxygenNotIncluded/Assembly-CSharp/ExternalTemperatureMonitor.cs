using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000879 RID: 2169
public class ExternalTemperatureMonitor : GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance>
{
	// Token: 0x06003F43 RID: 16195 RVA: 0x00161164 File Offset: 0x0015F364
	public static float GetExternalColdThreshold(Attributes affected_attributes)
	{
		if (affected_attributes == null)
		{
			return -0.36261335f;
		}
		return -(0.36261335f - affected_attributes.GetValue(Db.Get().Attributes.RoomTemperaturePreference.Id));
	}

	// Token: 0x06003F44 RID: 16196 RVA: 0x00161190 File Offset: 0x0015F390
	public static float GetExternalWarmThreshold(Attributes affected_attributes)
	{
		if (affected_attributes == null)
		{
			return 0.19525334f;
		}
		return -(-0.19525334f - affected_attributes.GetValue(Db.Get().Attributes.RoomTemperaturePreference.Id));
	}

	// Token: 0x06003F45 RID: 16197 RVA: 0x001611BC File Offset: 0x0015F3BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.comfortable;
		this.root.Enter(delegate(ExternalTemperatureMonitor.Instance smi)
		{
			smi.AverageExternalTemperature = smi.GetCurrentExternalTemperature;
		}).Update(delegate(ExternalTemperatureMonitor.Instance smi, float dt)
		{
			smi.AverageExternalTemperature *= Mathf.Max(0f, 1f - dt / 6f);
			smi.AverageExternalTemperature += smi.GetCurrentExternalTemperature * (dt / 6f);
		}, UpdateRate.SIM_200ms, false);
		this.comfortable.Transition(this.transitionToTooWarm, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooHot() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).Transition(this.transitionToTooCool, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooCold() && smi.timeinstate > 6f, UpdateRate.SIM_200ms);
		this.transitionToTooWarm.Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooHot(), UpdateRate.SIM_200ms).Transition(this.tooWarm, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooHot() && smi.timeinstate > 1f, UpdateRate.SIM_200ms);
		this.transitionToTooCool.Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooCold(), UpdateRate.SIM_200ms).Transition(this.tooCool, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooCold() && smi.timeinstate > 1f, UpdateRate.SIM_200ms);
		this.transitionToScalding.Transition(this.tooWarm, (ExternalTemperatureMonitor.Instance smi) => !smi.IsScalding(), UpdateRate.SIM_200ms).Transition(this.scalding, (ExternalTemperatureMonitor.Instance smi) => smi.IsScalding() && smi.timeinstate > 1f, UpdateRate.SIM_200ms);
		this.tooWarm.Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooHot() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).Transition(this.transitionToScalding, (ExternalTemperatureMonitor.Instance smi) => smi.IsScalding(), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Hot, null).ToggleThought(Db.Get().Thoughts.Hot, null).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hot, (ExternalTemperatureMonitor.Instance smi) => smi).ToggleEffect("WarmAir").Enter(delegate(ExternalTemperatureMonitor.Instance smi)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, true);
		});
		this.scalding.Transition(this.tooWarm, (ExternalTemperatureMonitor.Instance smi) => !smi.IsScalding() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Hot, null).ToggleThought(Db.Get().Thoughts.Hot, null).ToggleStatusItem(Db.Get().CreatureStatusItems.Scalding, (ExternalTemperatureMonitor.Instance smi) => smi).Update("ScaldDamage", delegate(ExternalTemperatureMonitor.Instance smi, float dt)
		{
			smi.ScaldDamage(dt);
		}, UpdateRate.SIM_1000ms, false);
		this.tooCool.Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooCold() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Cold, null).ToggleThought(Db.Get().Thoughts.Cold, null).ToggleStatusItem(Db.Get().DuplicantStatusItems.Cold, (ExternalTemperatureMonitor.Instance smi) => smi).ToggleEffect("ColdAir").Enter(delegate(ExternalTemperatureMonitor.Instance smi)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, true);
		});
	}

	// Token: 0x040028FD RID: 10493
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State comfortable;

	// Token: 0x040028FE RID: 10494
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToTooWarm;

	// Token: 0x040028FF RID: 10495
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State tooWarm;

	// Token: 0x04002900 RID: 10496
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToTooCool;

	// Token: 0x04002901 RID: 10497
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State tooCool;

	// Token: 0x04002902 RID: 10498
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToScalding;

	// Token: 0x04002903 RID: 10499
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State scalding;

	// Token: 0x04002904 RID: 10500
	private const float SCALDING_DAMAGE_AMOUNT = 10f;

	// Token: 0x04002905 RID: 10501
	private const float BODY_TEMPERATURE_AFFECT_EXTERNAL_FEEL_THRESHOLD = 0.5f;

	// Token: 0x04002906 RID: 10502
	public const float BASE_STRESS_TOLERANCE_COLD = 0.27893335f;

	// Token: 0x04002907 RID: 10503
	public const float BASE_STRESS_TOLERANCE_WARM = 0.27893335f;

	// Token: 0x04002908 RID: 10504
	private const float START_GAME_AVERAGING_DELAY = 6f;

	// Token: 0x04002909 RID: 10505
	private const float TRANSITION_TO_DELAY = 1f;

	// Token: 0x0400290A RID: 10506
	private const float TRANSITION_OUT_DELAY = 6f;

	// Token: 0x0400290B RID: 10507
	private const float TEMPERATURE_AVERAGING_RANGE = 6f;

	// Token: 0x02001671 RID: 5745
	public new class Instance : GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06008AC4 RID: 35524 RVA: 0x00314868 File Offset: 0x00312A68
		public float GetCurrentExternalTemperature
		{
			get
			{
				int num = Grid.PosToCell(base.gameObject);
				if (this.occupyArea != null)
				{
					float num2 = 0f;
					int num3 = 0;
					for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
					{
						int num4 = Grid.OffsetCell(num, this.occupyArea.OccupiedCellsOffsets[i]);
						if (Grid.IsValidCell(num4))
						{
							num3++;
							num2 += Grid.Temperature[num4];
						}
					}
					return num2 / (float)Mathf.Max(1, num3);
				}
				return Grid.Temperature[num];
			}
		}

		// Token: 0x06008AC5 RID: 35525 RVA: 0x003148FE File Offset: 0x00312AFE
		public override void StartSM()
		{
			base.StartSM();
			base.smi.attributes.Get(Db.Get().Attributes.ScaldingThreshold).Add(this.baseScalindingThreshold);
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06008AC6 RID: 35526 RVA: 0x00314930 File Offset: 0x00312B30
		public float GetCurrentColdThreshold
		{
			get
			{
				if (this.internalTemperatureMonitor.IdealTemperatureDelta() > 0.5f)
				{
					return 0f;
				}
				return CreatureSimTemperatureTransfer.PotentialEnergyFlowToCreature(Grid.PosToCell(base.gameObject), this.primaryElement, this.temperatureTransferer, 1f);
			}
		}

		// Token: 0x06008AC7 RID: 35527 RVA: 0x0031496B File Offset: 0x00312B6B
		public float GetScaldingThreshold()
		{
			return base.smi.attributes.GetValue("ScaldingThreshold");
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06008AC8 RID: 35528 RVA: 0x00314982 File Offset: 0x00312B82
		public float GetCurrentHotThreshold
		{
			get
			{
				return this.HotThreshold;
			}
		}

		// Token: 0x06008AC9 RID: 35529 RVA: 0x0031498C File Offset: 0x00312B8C
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.health = base.GetComponent<Health>();
			this.occupyArea = base.GetComponent<OccupyArea>();
			this.internalTemperatureMonitor = base.gameObject.GetSMI<TemperatureMonitor.Instance>();
			this.internalTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.temperatureTransferer = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			this.primaryElement = base.gameObject.GetComponent<PrimaryElement>();
			this.attributes = base.gameObject.GetAttributes();
		}

		// Token: 0x06008ACA RID: 35530 RVA: 0x00314A54 File Offset: 0x00312C54
		public bool IsTooHot()
		{
			return this.internalTemperatureMonitor.IdealTemperatureDelta() >= -0.5f && base.smi.temperatureTransferer.average_kilowatts_exchanged.GetWeightedAverage > ExternalTemperatureMonitor.GetExternalWarmThreshold(base.smi.attributes);
		}

		// Token: 0x06008ACB RID: 35531 RVA: 0x00314A94 File Offset: 0x00312C94
		public bool IsTooCold()
		{
			return this.internalTemperatureMonitor.IdealTemperatureDelta() <= 0.5f && base.smi.temperatureTransferer.average_kilowatts_exchanged.GetWeightedAverage < ExternalTemperatureMonitor.GetExternalColdThreshold(base.smi.attributes);
		}

		// Token: 0x06008ACC RID: 35532 RVA: 0x00314AD4 File Offset: 0x00312CD4
		public bool IsScalding()
		{
			return this.AverageExternalTemperature > base.smi.attributes.GetValue("ScaldingThreshold");
		}

		// Token: 0x06008ACD RID: 35533 RVA: 0x00314AF3 File Offset: 0x00312CF3
		public void ScaldDamage(float dt)
		{
			if (this.health != null && Time.time - this.lastScaldTime > 5f)
			{
				this.lastScaldTime = Time.time;
				this.health.Damage(dt * 10f);
			}
		}

		// Token: 0x06008ACE RID: 35534 RVA: 0x00314B33 File Offset: 0x00312D33
		public float CurrentWorldTransferWattage()
		{
			return this.temperatureTransferer.currentExchangeWattage;
		}

		// Token: 0x04006B99 RID: 27545
		public float AverageExternalTemperature;

		// Token: 0x04006B9A RID: 27546
		public float ColdThreshold = 283.15f;

		// Token: 0x04006B9B RID: 27547
		public float HotThreshold = 306.15f;

		// Token: 0x04006B9C RID: 27548
		private AttributeModifier baseScalindingThreshold = new AttributeModifier("ScaldingThreshold", 345f, DUPLICANTS.STATS.SKIN_DURABILITY.NAME, false, false, true);

		// Token: 0x04006B9D RID: 27549
		public Attributes attributes;

		// Token: 0x04006B9E RID: 27550
		public OccupyArea occupyArea;

		// Token: 0x04006B9F RID: 27551
		public AmountInstance internalTemperature;

		// Token: 0x04006BA0 RID: 27552
		private TemperatureMonitor.Instance internalTemperatureMonitor;

		// Token: 0x04006BA1 RID: 27553
		public CreatureSimTemperatureTransfer temperatureTransferer;

		// Token: 0x04006BA2 RID: 27554
		public Health health;

		// Token: 0x04006BA3 RID: 27555
		public PrimaryElement primaryElement;

		// Token: 0x04006BA4 RID: 27556
		private const float MIN_SCALD_INTERVAL = 5f;

		// Token: 0x04006BA5 RID: 27557
		private float lastScaldTime;
	}
}
