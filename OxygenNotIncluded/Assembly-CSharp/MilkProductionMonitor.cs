using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000885 RID: 2181
public class MilkProductionMonitor : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>
{
	// Token: 0x06003F77 RID: 16247 RVA: 0x00162C70 File Offset: 0x00160E70
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.producing;
		this.producing.DefaultState(this.producing.paused).EventHandler(GameHashes.CaloriesConsumed, delegate(MilkProductionMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		});
		this.producing.paused.Transition(this.producing.full, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull), UpdateRate.SIM_1000ms).Transition(this.producing.producing, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsProducing), UpdateRate.SIM_1000ms);
		this.producing.producing.Transition(this.producing.full, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull), UpdateRate.SIM_1000ms).Transition(this.producing.paused, GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Not(new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsProducing)), UpdateRate.SIM_1000ms);
		this.producing.full.ToggleStatusItem(Db.Get().CreatureStatusItems.MilkFull, null).Transition(this.producing.paused, GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Not(new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull)), UpdateRate.SIM_1000ms).Enter(delegate(MilkProductionMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Creatures.RequiresMilking);
		});
	}

	// Token: 0x06003F78 RID: 16248 RVA: 0x00162DC4 File Offset: 0x00160FC4
	private static bool IsProducing(MilkProductionMonitor.Instance smi)
	{
		return !smi.IsFull && smi.IsUnderProductionEffect;
	}

	// Token: 0x06003F79 RID: 16249 RVA: 0x00162DD6 File Offset: 0x00160FD6
	private static bool IsFull(MilkProductionMonitor.Instance smi)
	{
		return smi.IsFull;
	}

	// Token: 0x06003F7A RID: 16250 RVA: 0x00162DDE File Offset: 0x00160FDE
	private static bool HasCapacity(MilkProductionMonitor.Instance smi)
	{
		return !smi.IsFull;
	}

	// Token: 0x0400293C RID: 10556
	public MilkProductionMonitor.ProducingStates producing;

	// Token: 0x02001690 RID: 5776
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06008B68 RID: 35688 RVA: 0x0031661E File Offset: 0x0031481E
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.MilkProduction.Id);
		}

		// Token: 0x04006C2B RID: 27691
		public const SimHashes element = SimHashes.Milk;

		// Token: 0x04006C2C RID: 27692
		public string effectId;

		// Token: 0x04006C2D RID: 27693
		public float Capacity = 200f;

		// Token: 0x04006C2E RID: 27694
		public float CaloriesPerCycle = 1000f;

		// Token: 0x04006C2F RID: 27695
		public float HappinessRequired;
	}

	// Token: 0x02001691 RID: 5777
	public class ProducingStates : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State
	{
		// Token: 0x04006C30 RID: 27696
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State paused;

		// Token: 0x04006C31 RID: 27697
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State producing;

		// Token: 0x04006C32 RID: 27698
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State full;
	}

	// Token: 0x02001692 RID: 5778
	public new class Instance : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.GameInstance
	{
		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06008B6B RID: 35691 RVA: 0x0031666A File Offset: 0x0031486A
		public float MilkAmount
		{
			get
			{
				return this.MilkPercentage / 100f * base.def.Capacity;
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06008B6C RID: 35692 RVA: 0x00316684 File Offset: 0x00314884
		public float MilkPercentage
		{
			get
			{
				return this.milkAmountInstance.value;
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06008B6D RID: 35693 RVA: 0x00316691 File Offset: 0x00314891
		public bool IsFull
		{
			get
			{
				return this.MilkPercentage >= this.milkAmountInstance.GetMax();
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06008B6E RID: 35694 RVA: 0x003166A9 File Offset: 0x003148A9
		public bool IsUnderProductionEffect
		{
			get
			{
				return this.milkAmountInstance.GetDelta() > 0f;
			}
		}

		// Token: 0x06008B6F RID: 35695 RVA: 0x003166BD File Offset: 0x003148BD
		public Instance(IStateMachineTarget master, MilkProductionMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008B70 RID: 35696 RVA: 0x003166C8 File Offset: 0x003148C8
		public override void StartSM()
		{
			this.milkAmountInstance = Db.Get().Amounts.MilkProduction.Lookup(base.gameObject);
			if (base.def.effectId != null)
			{
				this.effectInstance = this.effects.Get(base.smi.def.effectId);
			}
			base.StartSM();
		}

		// Token: 0x06008B71 RID: 35697 RVA: 0x0031672C File Offset: 0x0031492C
		public void OnCaloriesConsumed(object data)
		{
			if (base.def.effectId == null)
			{
				return;
			}
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			this.effectInstance = this.effects.Get(base.smi.def.effectId);
			if (this.effectInstance == null)
			{
				this.effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			this.effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.CaloriesPerCycle * 600f;
		}

		// Token: 0x06008B72 RID: 35698 RVA: 0x003167C8 File Offset: 0x003149C8
		private void RemoveMilk(float amount)
		{
			if (this.milkAmountInstance != null)
			{
				float value = Mathf.Min(this.milkAmountInstance.GetMin(), this.MilkPercentage - amount);
				this.milkAmountInstance.SetValue(value);
			}
		}

		// Token: 0x06008B73 RID: 35699 RVA: 0x00316804 File Offset: 0x00314A04
		public PrimaryElement ExtractMilk(float desiredAmount)
		{
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			if (num <= 0f)
			{
				return null;
			}
			this.RemoveMilk(num);
			PrimaryElement component = LiquidSourceManager.Instance.CreateChunk(SimHashes.Milk, num, temperature, 0, 0, base.transform.GetPosition()).GetComponent<PrimaryElement>();
			component.KeepZeroMassObject = false;
			return component;
		}

		// Token: 0x06008B74 RID: 35700 RVA: 0x00316868 File Offset: 0x00314A68
		public PrimaryElement ExtractMilkIntoElementChunk(float desiredAmount, PrimaryElement elementChunk)
		{
			if (elementChunk == null || elementChunk.ElementID != SimHashes.Milk)
			{
				return null;
			}
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			this.RemoveMilk(num);
			float mass = elementChunk.Mass;
			float finalTemperature = GameUtil.GetFinalTemperature(elementChunk.Temperature, mass, temperature, num);
			elementChunk.SetMassTemperature(mass + num, finalTemperature);
			return elementChunk;
		}

		// Token: 0x06008B75 RID: 35701 RVA: 0x003168D0 File Offset: 0x00314AD0
		public PrimaryElement ExtractMilkIntoStorage(float desiredAmount, Storage storage)
		{
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			this.RemoveMilk(num);
			return storage.AddLiquid(SimHashes.Milk, num, temperature, 0, 0, false, true);
		}

		// Token: 0x04006C33 RID: 27699
		public Action<float> OnMilkAmountChanged;

		// Token: 0x04006C34 RID: 27700
		public AmountInstance milkAmountInstance;

		// Token: 0x04006C35 RID: 27701
		public EffectInstance effectInstance;

		// Token: 0x04006C36 RID: 27702
		[MyCmpGet]
		private Effects effects;
	}
}
