using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000724 RID: 1828
public class Growing : StateMachineComponent<Growing.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06003232 RID: 12850 RVA: 0x0010AA6B File Offset: 0x00108C6B
	private Crop crop
	{
		get
		{
			if (this._crop == null)
			{
				this._crop = base.GetComponent<Crop>();
			}
			return this._crop;
		}
	}

	// Token: 0x06003233 RID: 12851 RVA: 0x0010AA90 File Offset: 0x00108C90
	protected override void OnPrefabInit()
	{
		Amounts amounts = base.gameObject.GetAmounts();
		this.maturity = amounts.Get(Db.Get().Amounts.Maturity);
		this.oldAge = amounts.Add(new AmountInstance(Db.Get().Amounts.OldAge, base.gameObject));
		this.oldAge.maxAttribute.ClearModifiers();
		this.oldAge.maxAttribute.Add(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, this.maxAge, null, false, false, true));
		base.OnPrefabInit();
		base.Subscribe<Growing>(1119167081, Growing.OnNewGameSpawnDelegate);
		base.Subscribe<Growing>(1272413801, Growing.ResetGrowthDelegate);
	}

	// Token: 0x06003234 RID: 12852 RVA: 0x0010AB5A File Offset: 0x00108D5A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		base.gameObject.AddTag(GameTags.GrowingPlant);
	}

	// Token: 0x06003235 RID: 12853 RVA: 0x0010AB7D File Offset: 0x00108D7D
	private void OnNewGameSpawn(object data)
	{
		this.maturity.SetValue(this.maturity.maxAttribute.GetTotalValue() * UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x06003236 RID: 12854 RVA: 0x0010ABAC File Offset: 0x00108DAC
	public void OverrideMaturityLevel(float percent)
	{
		float value = this.maturity.GetMax() * percent;
		this.maturity.SetValue(value);
	}

	// Token: 0x06003237 RID: 12855 RVA: 0x0010ABD4 File Offset: 0x00108DD4
	public bool ReachedNextHarvest()
	{
		return this.PercentOfCurrentHarvest() >= 1f;
	}

	// Token: 0x06003238 RID: 12856 RVA: 0x0010ABE6 File Offset: 0x00108DE6
	public bool IsGrown()
	{
		return this.maturity.value == this.maturity.GetMax();
	}

	// Token: 0x06003239 RID: 12857 RVA: 0x0010AC00 File Offset: 0x00108E00
	public bool CanGrow()
	{
		return !this.IsGrown();
	}

	// Token: 0x0600323A RID: 12858 RVA: 0x0010AC0B File Offset: 0x00108E0B
	public bool IsGrowing()
	{
		return this.maturity.GetDelta() > 0f;
	}

	// Token: 0x0600323B RID: 12859 RVA: 0x0010AC1F File Offset: 0x00108E1F
	public void ClampGrowthToHarvest()
	{
		this.maturity.value = this.maturity.GetMax();
	}

	// Token: 0x0600323C RID: 12860 RVA: 0x0010AC37 File Offset: 0x00108E37
	public float GetMaxMaturity()
	{
		return this.maturity.GetMax();
	}

	// Token: 0x0600323D RID: 12861 RVA: 0x0010AC44 File Offset: 0x00108E44
	public float PercentOfCurrentHarvest()
	{
		return this.maturity.value / this.maturity.GetMax();
	}

	// Token: 0x0600323E RID: 12862 RVA: 0x0010AC5D File Offset: 0x00108E5D
	public float TimeUntilNextHarvest()
	{
		return (this.maturity.GetMax() - this.maturity.value) / this.maturity.GetDelta();
	}

	// Token: 0x0600323F RID: 12863 RVA: 0x0010AC82 File Offset: 0x00108E82
	public float DomesticGrowthTime()
	{
		return this.maturity.GetMax() / base.smi.baseGrowingRate.Value;
	}

	// Token: 0x06003240 RID: 12864 RVA: 0x0010ACA0 File Offset: 0x00108EA0
	public float WildGrowthTime()
	{
		return this.maturity.GetMax() / base.smi.wildGrowingRate.Value;
	}

	// Token: 0x06003241 RID: 12865 RVA: 0x0010ACBE File Offset: 0x00108EBE
	public float PercentGrown()
	{
		return this.maturity.value / this.maturity.GetMax();
	}

	// Token: 0x06003242 RID: 12866 RVA: 0x0010ACD7 File Offset: 0x00108ED7
	public void ResetGrowth(object data = null)
	{
		this.maturity.value = 0f;
	}

	// Token: 0x06003243 RID: 12867 RVA: 0x0010ACE9 File Offset: 0x00108EE9
	public float PercentOldAge()
	{
		if (!this.shouldGrowOld)
		{
			return 0f;
		}
		return this.oldAge.value / this.oldAge.GetMax();
	}

	// Token: 0x06003244 RID: 12868 RVA: 0x0010AD10 File Offset: 0x00108F10
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Klei.AI.Attribute maxAttribute = Db.Get().Amounts.Maturity.maxAttribute;
		list.Add(new Descriptor(go.GetComponent<Modifiers>().GetPreModifiedAttributeDescription(maxAttribute), go.GetComponent<Modifiers>().GetPreModifiedAttributeToolTip(maxAttribute), Descriptor.DescriptorType.Requirement, false));
		return list;
	}

	// Token: 0x06003245 RID: 12869 RVA: 0x0010AD5C File Offset: 0x00108F5C
	public void ConsumeMass(float mass_to_consume)
	{
		float value = this.maturity.value;
		mass_to_consume = Mathf.Min(mass_to_consume, value);
		this.maturity.value = this.maturity.value - mass_to_consume;
		base.gameObject.Trigger(-1793167409, null);
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x0010ADA8 File Offset: 0x00108FA8
	public void ConsumeGrowthUnits(float units_to_consume, float unit_maturity_ratio)
	{
		float num = units_to_consume / unit_maturity_ratio;
		global::Debug.Assert(num <= this.maturity.value);
		this.maturity.value -= num;
		base.gameObject.Trigger(-1793167409, null);
	}

	// Token: 0x04001E1F RID: 7711
	public bool shouldGrowOld = true;

	// Token: 0x04001E20 RID: 7712
	public float maxAge = 2400f;

	// Token: 0x04001E21 RID: 7713
	private AmountInstance maturity;

	// Token: 0x04001E22 RID: 7714
	private AmountInstance oldAge;

	// Token: 0x04001E23 RID: 7715
	[MyCmpGet]
	private WiltCondition wiltCondition;

	// Token: 0x04001E24 RID: 7716
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001E25 RID: 7717
	[MyCmpReq]
	private Modifiers modifiers;

	// Token: 0x04001E26 RID: 7718
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x04001E27 RID: 7719
	private Crop _crop;

	// Token: 0x04001E28 RID: 7720
	private static readonly EventSystem.IntraObjectHandler<Growing> OnNewGameSpawnDelegate = new EventSystem.IntraObjectHandler<Growing>(delegate(Growing component, object data)
	{
		component.OnNewGameSpawn(data);
	});

	// Token: 0x04001E29 RID: 7721
	private static readonly EventSystem.IntraObjectHandler<Growing> ResetGrowthDelegate = new EventSystem.IntraObjectHandler<Growing>(delegate(Growing component, object data)
	{
		component.ResetGrowth(data);
	});

	// Token: 0x0200149F RID: 5279
	public class StatesInstance : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.GameInstance
	{
		// Token: 0x06008553 RID: 34131 RVA: 0x003054E4 File Offset: 0x003036E4
		public StatesInstance(Growing master) : base(master)
		{
			this.baseGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, 0.0016666667f, CREATURES.STATS.MATURITY.GROWING, false, false, true);
			this.wildGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, 0.00041666668f, CREATURES.STATS.MATURITY.GROWINGWILD, false, false, true);
			this.getOldRate = new AttributeModifier(master.oldAge.deltaAttribute.Id, master.shouldGrowOld ? 1f : 0f, null, false, false, true);
		}

		// Token: 0x06008554 RID: 34132 RVA: 0x00305585 File Offset: 0x00303785
		public bool IsGrown()
		{
			return base.master.IsGrown();
		}

		// Token: 0x06008555 RID: 34133 RVA: 0x00305592 File Offset: 0x00303792
		public bool ReachedNextHarvest()
		{
			return base.master.ReachedNextHarvest();
		}

		// Token: 0x06008556 RID: 34134 RVA: 0x0030559F File Offset: 0x0030379F
		public void ClampGrowthToHarvest()
		{
			base.master.ClampGrowthToHarvest();
		}

		// Token: 0x06008557 RID: 34135 RVA: 0x003055AC File Offset: 0x003037AC
		public bool IsWilting()
		{
			return base.master.wiltCondition != null && base.master.wiltCondition.IsWilting();
		}

		// Token: 0x06008558 RID: 34136 RVA: 0x003055D4 File Offset: 0x003037D4
		public bool IsSleeping()
		{
			CropSleepingMonitor.Instance smi = base.master.GetSMI<CropSleepingMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x06008559 RID: 34137 RVA: 0x003055F8 File Offset: 0x003037F8
		public bool CanExitStalled()
		{
			return !this.IsWilting() && !this.IsSleeping();
		}

		// Token: 0x040065F5 RID: 26101
		public AttributeModifier baseGrowingRate;

		// Token: 0x040065F6 RID: 26102
		public AttributeModifier wildGrowingRate;

		// Token: 0x040065F7 RID: 26103
		public AttributeModifier getOldRate;
	}

	// Token: 0x020014A0 RID: 5280
	public class States : GameStateMachine<Growing.States, Growing.StatesInstance, Growing>
	{
		// Token: 0x0600855A RID: 34138 RVA: 0x00305610 File Offset: 0x00303810
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.growing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.growing.EventTransition(GameHashes.Wilt, this.stalled, (Growing.StatesInstance smi) => smi.IsWilting()).EventTransition(GameHashes.CropSleep, this.stalled, (Growing.StatesInstance smi) => smi.IsSleeping()).EventTransition(GameHashes.PlanterStorage, this.growing.planted, (Growing.StatesInstance smi) => smi.master.rm.Replanted).EventTransition(GameHashes.PlanterStorage, this.growing.wild, (Growing.StatesInstance smi) => !smi.master.rm.Replanted).TriggerOnEnter(GameHashes.Grow, null).Update("CheckGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (smi.ReachedNextHarvest())
				{
					smi.GoTo(this.grown);
				}
			}, UpdateRate.SIM_4000ms, false).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, (Growing.StatesInstance smi) => smi.master.GetComponent<Growing>()).Enter(delegate(Growing.StatesInstance smi)
			{
				GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State state = smi.master.rm.Replanted ? this.growing.planted : this.growing.wild;
				smi.GoTo(state);
			});
			this.growing.wild.ToggleAttributeModifier("GrowingWild", (Growing.StatesInstance smi) => smi.wildGrowingRate, null);
			this.growing.planted.ToggleAttributeModifier("Growing", (Growing.StatesInstance smi) => smi.baseGrowingRate, null);
			this.stalled.EventTransition(GameHashes.WiltRecover, this.growing, (Growing.StatesInstance smi) => smi.CanExitStalled()).EventTransition(GameHashes.CropWakeUp, this.growing, (Growing.StatesInstance smi) => smi.CanExitStalled());
			this.grown.DefaultState(this.grown.idle).TriggerOnEnter(GameHashes.Grow, null).Update("CheckNotGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (!smi.ReachedNextHarvest())
				{
					smi.GoTo(this.growing);
				}
			}, UpdateRate.SIM_4000ms, false).ToggleAttributeModifier("GettingOld", (Growing.StatesInstance smi) => smi.getOldRate, null).Enter(delegate(Growing.StatesInstance smi)
			{
				smi.ClampGrowthToHarvest();
			}).Exit(delegate(Growing.StatesInstance smi)
			{
				smi.master.oldAge.SetValue(0f);
			});
			this.grown.idle.Update("CheckNotGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (smi.master.shouldGrowOld && smi.master.oldAge.value >= smi.master.oldAge.GetMax())
				{
					smi.GoTo(this.grown.try_self_harvest);
				}
			}, UpdateRate.SIM_4000ms, false);
			this.grown.try_self_harvest.Enter(delegate(Growing.StatesInstance smi)
			{
				Harvestable component = smi.master.GetComponent<Harvestable>();
				if (component && component.CanBeHarvested)
				{
					bool harvestWhenReady = component.harvestDesignatable.HarvestWhenReady;
					component.ForceCancelHarvest(null);
					component.Harvest();
					if (harvestWhenReady && component != null)
					{
						component.harvestDesignatable.SetHarvestWhenReady(true);
					}
				}
				smi.master.maturity.SetValue(0f);
				smi.master.oldAge.SetValue(0f);
			}).GoTo(this.grown.idle);
		}

		// Token: 0x040065F8 RID: 26104
		public Growing.States.GrowingStates growing;

		// Token: 0x040065F9 RID: 26105
		public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State stalled;

		// Token: 0x040065FA RID: 26106
		public Growing.States.GrownStates grown;

		// Token: 0x02002167 RID: 8551
		public class GrowingStates : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
		{
			// Token: 0x0400958E RID: 38286
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State wild;

			// Token: 0x0400958F RID: 38287
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State planted;
		}

		// Token: 0x02002168 RID: 8552
		public class GrownStates : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
		{
			// Token: 0x04009590 RID: 38288
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State idle;

			// Token: 0x04009591 RID: 38289
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State try_self_harvest;
		}
	}
}
