using System;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000A28 RID: 2600
public class WarmBlooded : StateMachineComponent<WarmBlooded.StatesInstance>
{
	// Token: 0x06004DDD RID: 19933 RVA: 0x001B49D0 File Offset: 0x001B2BD0
	protected override void OnPrefabInit()
	{
		this.externalTemperature = Db.Get().Amounts.ExternalTemperature.Lookup(base.gameObject);
		this.externalTemperature.value = Grid.Temperature[Grid.PosToCell(this)];
		this.temperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
		this.primaryElement = base.GetComponent<PrimaryElement>();
	}

	// Token: 0x06004DDE RID: 19934 RVA: 0x001B4A44 File Offset: 0x001B2C44
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004DDF RID: 19935 RVA: 0x001B4A51 File Offset: 0x001B2C51
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06004DE0 RID: 19936 RVA: 0x001B4A59 File Offset: 0x001B2C59
	public bool IsAtReasonableTemperature()
	{
		return !base.smi.IsHot() && !base.smi.IsCold();
	}

	// Token: 0x06004DE1 RID: 19937 RVA: 0x001B4A78 File Offset: 0x001B2C78
	public void SetTemperatureImmediate(float t)
	{
		this.temperature.value = t;
	}

	// Token: 0x040032C7 RID: 12999
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x040032C8 RID: 13000
	private AmountInstance externalTemperature;

	// Token: 0x040032C9 RID: 13001
	public AmountInstance temperature;

	// Token: 0x040032CA RID: 13002
	private PrimaryElement primaryElement;

	// Token: 0x040032CB RID: 13003
	public const float TRANSITION_DELAY_HOT = 3f;

	// Token: 0x040032CC RID: 13004
	public const float TRANSITION_DELAY_COLD = 3f;

	// Token: 0x020018AF RID: 6319
	public class StatesInstance : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.GameInstance
	{
		// Token: 0x0600927C RID: 37500 RVA: 0x0032C05C File Offset: 0x0032A25C
		public StatesInstance(WarmBlooded smi) : base(smi)
		{
			this.baseTemperatureModification = new AttributeModifier("TemperatureDelta", 0f, DUPLICANTS.MODIFIERS.BASEDUPLICANT.NAME, false, true, false);
			this.bodyRegulator = new AttributeModifier("TemperatureDelta", 0f, DUPLICANTS.MODIFIERS.HOMEOSTASIS.NAME, false, true, false);
			this.burningCalories = new AttributeModifier("CaloriesDelta", 0f, DUPLICANTS.MODIFIERS.BURNINGCALORIES.NAME, false, false, false);
			base.master.GetAttributes().Add(this.bodyRegulator);
			base.master.GetAttributes().Add(this.burningCalories);
			base.master.GetAttributes().Add(this.baseTemperatureModification);
			base.master.SetTemperatureImmediate(310.15f);
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x0600927D RID: 37501 RVA: 0x0032C128 File Offset: 0x0032A328
		public float TemperatureDelta
		{
			get
			{
				return this.bodyRegulator.Value;
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x0600927E RID: 37502 RVA: 0x0032C135 File Offset: 0x0032A335
		public float BodyTemperature
		{
			get
			{
				return base.master.primaryElement.Temperature;
			}
		}

		// Token: 0x0600927F RID: 37503 RVA: 0x0032C147 File Offset: 0x0032A347
		public bool IsHot()
		{
			return this.BodyTemperature > 310.15f;
		}

		// Token: 0x06009280 RID: 37504 RVA: 0x0032C156 File Offset: 0x0032A356
		public bool IsCold()
		{
			return this.BodyTemperature < 310.15f;
		}

		// Token: 0x040072AC RID: 29356
		public AttributeModifier baseTemperatureModification;

		// Token: 0x040072AD RID: 29357
		public AttributeModifier bodyRegulator;

		// Token: 0x040072AE RID: 29358
		public AttributeModifier averageBodyRegulation;

		// Token: 0x040072AF RID: 29359
		public AttributeModifier burningCalories;

		// Token: 0x040072B0 RID: 29360
		public float averageInternalTemperature;
	}

	// Token: 0x020018B0 RID: 6320
	public class States : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded>
	{
		// Token: 0x06009281 RID: 37505 RVA: 0x0032C168 File Offset: 0x0032A368
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.alive.normal;
			this.root.TagTransition(GameTags.Dead, this.dead, false).Enter(delegate(WarmBlooded.StatesInstance smi)
			{
				PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
				float value = SimUtil.EnergyFlowToTemperatureDelta(0.08368001f, component.Element.specificHeatCapacity, component.Mass);
				smi.baseTemperatureModification.SetValue(value);
				CreatureSimTemperatureTransfer component2 = smi.master.GetComponent<CreatureSimTemperatureTransfer>();
				component2.NonSimTemperatureModifiers.Add(smi.baseTemperatureModification);
				component2.NonSimTemperatureModifiers.Add(smi.bodyRegulator);
			});
			this.alive.normal.Transition(this.alive.cold.transition, (WarmBlooded.StatesInstance smi) => smi.IsCold(), UpdateRate.SIM_200ms).Transition(this.alive.hot.transition, (WarmBlooded.StatesInstance smi) => smi.IsHot(), UpdateRate.SIM_200ms);
			this.alive.cold.transition.ScheduleGoTo(3f, this.alive.cold.regulating).Transition(this.alive.normal, (WarmBlooded.StatesInstance smi) => !smi.IsCold(), UpdateRate.SIM_200ms);
			this.alive.cold.regulating.Transition(this.alive.normal, (WarmBlooded.StatesInstance smi) => !smi.IsCold(), UpdateRate.SIM_200ms).Update("ColdRegulating", delegate(WarmBlooded.StatesInstance smi, float dt)
			{
				PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
				float num = SimUtil.EnergyFlowToTemperatureDelta(0.08368001f, component.Element.specificHeatCapacity, component.Mass);
				float num2 = SimUtil.EnergyFlowToTemperatureDelta(0.5578667f, component.Element.specificHeatCapacity, component.Mass);
				float num3 = 310.15f - smi.BodyTemperature;
				float num4 = 1f;
				if (num2 + num > num3)
				{
					num4 = Mathf.Max(0f, num3 - num) / num2;
				}
				smi.bodyRegulator.SetValue(num2 * num4);
				smi.burningCalories.SetValue(-0.5578667f * num4 * 1000f / 4184f);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.bodyRegulator.SetValue(0f);
				smi.burningCalories.SetValue(0f);
			});
			this.alive.hot.transition.ScheduleGoTo(3f, this.alive.hot.regulating).Transition(this.alive.normal, (WarmBlooded.StatesInstance smi) => !smi.IsHot(), UpdateRate.SIM_200ms);
			this.alive.hot.regulating.Transition(this.alive.normal, (WarmBlooded.StatesInstance smi) => !smi.IsHot(), UpdateRate.SIM_200ms).Update("WarmRegulating", delegate(WarmBlooded.StatesInstance smi, float dt)
			{
				PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
				float num = SimUtil.EnergyFlowToTemperatureDelta(0.5578667f, component.Element.specificHeatCapacity, component.Mass);
				float num2 = 310.15f - smi.BodyTemperature;
				float num3 = 1f;
				if ((num - smi.baseTemperatureModification.Value) * dt < num2)
				{
					num3 = Mathf.Clamp(num2 / ((num - smi.baseTemperatureModification.Value) * dt), 0f, 1f);
				}
				smi.bodyRegulator.SetValue(-num * num3);
				smi.burningCalories.SetValue(-0.5578667f * num3 / 4184f);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.bodyRegulator.SetValue(0f);
			});
			this.dead.Enter(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.master.enabled = false;
			});
		}

		// Token: 0x040072B1 RID: 29361
		public WarmBlooded.States.AliveState alive;

		// Token: 0x040072B2 RID: 29362
		public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State dead;

		// Token: 0x02002207 RID: 8711
		public class RegulatingState : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State
		{
			// Token: 0x04009855 RID: 38997
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State transition;

			// Token: 0x04009856 RID: 38998
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State regulating;
		}

		// Token: 0x02002208 RID: 8712
		public class AliveState : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State
		{
			// Token: 0x04009857 RID: 38999
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State normal;

			// Token: 0x04009858 RID: 39000
			public WarmBlooded.States.RegulatingState cold;

			// Token: 0x04009859 RID: 39001
			public WarmBlooded.States.RegulatingState hot;
		}
	}
}
