using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200089C RID: 2204
public class TemperatureMonitor : GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance>
{
	// Token: 0x06003FF4 RID: 16372 RVA: 0x00165F20 File Offset: 0x00164120
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.homeostatic;
		this.root.Enter(delegate(TemperatureMonitor.Instance smi)
		{
			smi.averageTemperature = smi.primaryElement.Temperature;
			SicknessTrigger component = smi.master.GetComponent<SicknessTrigger>();
			if (component != null)
			{
				component.AddTrigger(GameHashes.TooHotSickness, new string[]
				{
					"HeatSickness"
				}, (GameObject s, GameObject t) => DUPLICANTS.DISEASES.INFECTIONSOURCES.INTERNAL_TEMPERATURE);
				component.AddTrigger(GameHashes.TooColdSickness, new string[]
				{
					"ColdSickness"
				}, (GameObject s, GameObject t) => DUPLICANTS.DISEASES.INFECTIONSOURCES.INTERNAL_TEMPERATURE);
			}
		}).Update("UpdateTemperature", delegate(TemperatureMonitor.Instance smi, float dt)
		{
			smi.UpdateTemperature(dt);
		}, UpdateRate.SIM_200ms, false);
		this.homeostatic.Transition(this.hyperthermic_pre, (TemperatureMonitor.Instance smi) => smi.IsHyperthermic(), UpdateRate.SIM_200ms).Transition(this.hypothermic_pre, (TemperatureMonitor.Instance smi) => smi.IsHypothermic(), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, null);
		this.hyperthermic_pre.Enter(delegate(TemperatureMonitor.Instance smi)
		{
			smi.master.Trigger(-1174019026, smi.master.gameObject);
			smi.GoTo(this.hyperthermic);
		});
		this.hypothermic_pre.Enter(delegate(TemperatureMonitor.Instance smi)
		{
			smi.master.Trigger(54654253, smi.master.gameObject);
			smi.GoTo(this.hypothermic);
		});
		this.hyperthermic.Transition(this.homeostatic, (TemperatureMonitor.Instance smi) => !smi.IsHyperthermic(), UpdateRate.SIM_200ms).ToggleUrge(Db.Get().Urges.CoolDown);
		this.hypothermic.Transition(this.homeostatic, (TemperatureMonitor.Instance smi) => !smi.IsHypothermic(), UpdateRate.SIM_200ms).ToggleUrge(Db.Get().Urges.WarmUp);
		this.deathcold.Enter("KillCold", delegate(TemperatureMonitor.Instance smi)
		{
			smi.KillCold();
		}).TriggerOnEnter(GameHashes.TooColdFatal, null);
		this.deathhot.Enter("KillHot", delegate(TemperatureMonitor.Instance smi)
		{
			smi.KillHot();
		}).TriggerOnEnter(GameHashes.TooHotFatal, null);
	}

	// Token: 0x040029A1 RID: 10657
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State homeostatic;

	// Token: 0x040029A2 RID: 10658
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hyperthermic;

	// Token: 0x040029A3 RID: 10659
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hypothermic;

	// Token: 0x040029A4 RID: 10660
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hyperthermic_pre;

	// Token: 0x040029A5 RID: 10661
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hypothermic_pre;

	// Token: 0x040029A6 RID: 10662
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State deathcold;

	// Token: 0x040029A7 RID: 10663
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State deathhot;

	// Token: 0x040029A8 RID: 10664
	private const float TEMPERATURE_AVERAGING_RANGE = 4f;

	// Token: 0x040029A9 RID: 10665
	public StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.IntParameter warmUpCell;

	// Token: 0x040029AA RID: 10666
	public StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.IntParameter coolDownCell;

	// Token: 0x020016C6 RID: 5830
	public new class Instance : GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008C46 RID: 35910 RVA: 0x00318384 File Offset: 0x00316584
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.primaryElement = base.GetComponent<PrimaryElement>();
			this.temperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.warmUpQuery = new SafetyQuery(Game.Instance.safetyConditions.WarmUpChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.coolDownQuery = new SafetyQuery(Game.Instance.safetyConditions.CoolDownChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.navigator = base.GetComponent<Navigator>();
		}

		// Token: 0x06008C47 RID: 35911 RVA: 0x00318448 File Offset: 0x00316648
		public void UpdateTemperature(float dt)
		{
			base.smi.averageTemperature *= 1f - dt / 4f;
			base.smi.averageTemperature += base.smi.primaryElement.Temperature * (dt / 4f);
			base.smi.temperature.SetValue(base.smi.averageTemperature);
		}

		// Token: 0x06008C48 RID: 35912 RVA: 0x003184BA File Offset: 0x003166BA
		public bool IsHyperthermic()
		{
			return this.temperature.value > this.HyperthermiaThreshold;
		}

		// Token: 0x06008C49 RID: 35913 RVA: 0x003184CF File Offset: 0x003166CF
		public bool IsHypothermic()
		{
			return this.temperature.value < this.HypothermiaThreshold;
		}

		// Token: 0x06008C4A RID: 35914 RVA: 0x003184E4 File Offset: 0x003166E4
		public bool IsFatalHypothermic()
		{
			return this.temperature.value < this.FatalHypothermia;
		}

		// Token: 0x06008C4B RID: 35915 RVA: 0x003184F9 File Offset: 0x003166F9
		public bool IsFatalHyperthermic()
		{
			return this.temperature.value > this.FatalHyperthermia;
		}

		// Token: 0x06008C4C RID: 35916 RVA: 0x0031850E File Offset: 0x0031670E
		public void KillHot()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Overheating);
		}

		// Token: 0x06008C4D RID: 35917 RVA: 0x0031852F File Offset: 0x0031672F
		public void KillCold()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Frozen);
		}

		// Token: 0x06008C4E RID: 35918 RVA: 0x00318550 File Offset: 0x00316750
		public float ExtremeTemperatureDelta()
		{
			if (this.temperature.value > this.HyperthermiaThreshold)
			{
				return this.temperature.value - this.HyperthermiaThreshold;
			}
			if (this.temperature.value < this.HypothermiaThreshold)
			{
				return this.temperature.value - this.HypothermiaThreshold;
			}
			return 0f;
		}

		// Token: 0x06008C4F RID: 35919 RVA: 0x003185AE File Offset: 0x003167AE
		public float IdealTemperatureDelta()
		{
			return this.temperature.value - 310.15f;
		}

		// Token: 0x06008C50 RID: 35920 RVA: 0x003185C1 File Offset: 0x003167C1
		public int GetWarmUpCell()
		{
			return base.sm.warmUpCell.Get(base.smi);
		}

		// Token: 0x06008C51 RID: 35921 RVA: 0x003185D9 File Offset: 0x003167D9
		public int GetCoolDownCell()
		{
			return base.sm.coolDownCell.Get(base.smi);
		}

		// Token: 0x06008C52 RID: 35922 RVA: 0x003185F4 File Offset: 0x003167F4
		public void UpdateWarmUpCell()
		{
			this.warmUpQuery.Reset();
			this.navigator.RunQuery(this.warmUpQuery);
			base.sm.warmUpCell.Set(this.warmUpQuery.GetResultCell(), base.smi, false);
		}

		// Token: 0x06008C53 RID: 35923 RVA: 0x00318640 File Offset: 0x00316840
		public void UpdateCoolDownCell()
		{
			this.coolDownQuery.Reset();
			this.navigator.RunQuery(this.coolDownQuery);
			base.sm.coolDownCell.Set(this.coolDownQuery.GetResultCell(), base.smi, false);
		}

		// Token: 0x04006CCB RID: 27851
		public AmountInstance temperature;

		// Token: 0x04006CCC RID: 27852
		public PrimaryElement primaryElement;

		// Token: 0x04006CCD RID: 27853
		private Navigator navigator;

		// Token: 0x04006CCE RID: 27854
		private SafetyQuery warmUpQuery;

		// Token: 0x04006CCF RID: 27855
		private SafetyQuery coolDownQuery;

		// Token: 0x04006CD0 RID: 27856
		public float averageTemperature;

		// Token: 0x04006CD1 RID: 27857
		public float HypothermiaThreshold = 307.15f;

		// Token: 0x04006CD2 RID: 27858
		public float HyperthermiaThreshold = 313.15f;

		// Token: 0x04006CD3 RID: 27859
		public float FatalHypothermia = 305.15f;

		// Token: 0x04006CD4 RID: 27860
		public float FatalHyperthermia = 315.15f;
	}
}
