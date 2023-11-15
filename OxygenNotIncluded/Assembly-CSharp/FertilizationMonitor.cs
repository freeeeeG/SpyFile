using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000721 RID: 1825
public class FertilizationMonitor : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>
{
	// Token: 0x06003224 RID: 12836 RVA: 0x0010A334 File Offset: 0x00108534
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.wild;
		base.serializable = StateMachine.SerializeType.Never;
		this.wild.ParamTransition<GameObject>(this.fertilizerStorage, this.unfertilizable, (FertilizationMonitor.Instance smi, GameObject p) => p != null);
		this.unfertilizable.Enter(delegate(FertilizationMonitor.Instance smi)
		{
			if (smi.AcceptsFertilizer())
			{
				smi.GoTo(this.replanted.fertilized);
			}
		});
		this.replanted.Enter(delegate(FertilizationMonitor.Instance smi)
		{
			ManualDeliveryKG[] components = smi.gameObject.GetComponents<ManualDeliveryKG>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].Pause(false, "replanted");
			}
			smi.UpdateFertilization(0.033333335f);
		}).Target(this.fertilizerStorage).EventHandler(GameHashes.OnStorageChange, delegate(FertilizationMonitor.Instance smi)
		{
			smi.UpdateFertilization(0.2f);
		}).Target(this.masterTarget);
		this.replanted.fertilized.DefaultState(this.replanted.fertilized.decaying).TriggerOnEnter(this.ResourceRecievedEvent, null);
		this.replanted.fertilized.decaying.DefaultState(this.replanted.fertilized.decaying.normal).ToggleAttributeModifier("Consuming", (FertilizationMonitor.Instance smi) => smi.consumptionRate, null).ParamTransition<bool>(this.hasCorrectFertilizer, this.replanted.fertilized.absorbing, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue).Update("Decaying", delegate(FertilizationMonitor.Instance smi, float dt)
		{
			if (smi.Starved())
			{
				smi.GoTo(this.replanted.starved);
			}
		}, UpdateRate.SIM_200ms, false);
		this.replanted.fertilized.decaying.normal.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.decaying.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.fertilized.decaying.wrongFert.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.decaying.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
		this.replanted.fertilized.absorbing.DefaultState(this.replanted.fertilized.absorbing.normal).ParamTransition<bool>(this.hasCorrectFertilizer, this.replanted.fertilized.decaying, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse).ToggleAttributeModifier("Absorbing", (FertilizationMonitor.Instance smi) => smi.absorptionRate, null).Enter(delegate(FertilizationMonitor.Instance smi)
		{
			smi.StartAbsorbing();
		}).EventHandler(GameHashes.Wilt, delegate(FertilizationMonitor.Instance smi)
		{
			smi.StopAbsorbing();
		}).EventHandler(GameHashes.WiltRecover, delegate(FertilizationMonitor.Instance smi)
		{
			smi.StartAbsorbing();
		}).Exit(delegate(FertilizationMonitor.Instance smi)
		{
			smi.StopAbsorbing();
		});
		this.replanted.fertilized.absorbing.normal.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.absorbing.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.fertilized.absorbing.wrongFert.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.absorbing.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
		this.replanted.starved.DefaultState(this.replanted.starved.normal).TriggerOnEnter(this.ResourceDepletedEvent, null).ParamTransition<bool>(this.hasCorrectFertilizer, this.replanted.fertilized, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.starved.normal.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.starved.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.starved.wrongFert.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.starved.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
	}

	// Token: 0x04001E10 RID: 7696
	public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.TargetParameter fertilizerStorage;

	// Token: 0x04001E11 RID: 7697
	public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.BoolParameter hasCorrectFertilizer;

	// Token: 0x04001E12 RID: 7698
	public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.BoolParameter hasIncorrectFertilizer;

	// Token: 0x04001E13 RID: 7699
	public GameHashes ResourceRecievedEvent = GameHashes.Fertilized;

	// Token: 0x04001E14 RID: 7700
	public GameHashes ResourceDepletedEvent = GameHashes.Unfertilized;

	// Token: 0x04001E15 RID: 7701
	public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wild;

	// Token: 0x04001E16 RID: 7702
	public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State unfertilizable;

	// Token: 0x04001E17 RID: 7703
	public FertilizationMonitor.ReplantedStates replanted;

	// Token: 0x02001497 RID: 5271
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06008531 RID: 34097 RVA: 0x00304D74 File Offset: 0x00302F74
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			if (this.consumedElements.Length != 0)
			{
				List<Descriptor> list = new List<Descriptor>();
				float preModifiedAttributeValue = obj.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.FertilizerUsageMod);
				foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in this.consumedElements)
				{
					float num = consumeInfo.massConsumptionRate * preModifiedAttributeValue;
					list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.IDEAL_FERTILIZER, consumeInfo.tag.ProperName(), GameUtil.GetFormattedMass(-num, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.IDEAL_FERTILIZER, consumeInfo.tag.ProperName(), GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
				}
				return list;
			}
			return null;
		}

		// Token: 0x040065DA RID: 26074
		public Tag wrongFertilizerTestTag;

		// Token: 0x040065DB RID: 26075
		public PlantElementAbsorber.ConsumeInfo[] consumedElements;
	}

	// Token: 0x02001498 RID: 5272
	public class VariableFertilizerStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
	{
		// Token: 0x040065DC RID: 26076
		public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State normal;

		// Token: 0x040065DD RID: 26077
		public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wrongFert;
	}

	// Token: 0x02001499 RID: 5273
	public class FertilizedStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
	{
		// Token: 0x040065DE RID: 26078
		public FertilizationMonitor.VariableFertilizerStates decaying;

		// Token: 0x040065DF RID: 26079
		public FertilizationMonitor.VariableFertilizerStates absorbing;

		// Token: 0x040065E0 RID: 26080
		public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wilting;
	}

	// Token: 0x0200149A RID: 5274
	public class ReplantedStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
	{
		// Token: 0x040065E1 RID: 26081
		public FertilizationMonitor.FertilizedStates fertilized;

		// Token: 0x040065E2 RID: 26082
		public FertilizationMonitor.VariableFertilizerStates starved;
	}

	// Token: 0x0200149B RID: 5275
	public new class Instance : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.GameInstance, IWiltCause
	{
		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06008536 RID: 34102 RVA: 0x00304E5C File Offset: 0x0030305C
		public float total_fertilizer_available
		{
			get
			{
				return this.total_available_mass;
			}
		}

		// Token: 0x06008537 RID: 34103 RVA: 0x00304E64 File Offset: 0x00303064
		public Instance(IStateMachineTarget master, FertilizationMonitor.Def def) : base(master, def)
		{
			this.AddAmounts(base.gameObject);
			this.MakeModifiers();
			master.Subscribe(1309017699, new Action<object>(this.SetStorage));
		}

		// Token: 0x06008538 RID: 34104 RVA: 0x00304EA3 File Offset: 0x003030A3
		public virtual StatusItem GetStarvedStatusItem()
		{
			return Db.Get().CreatureStatusItems.NeedsFertilizer;
		}

		// Token: 0x06008539 RID: 34105 RVA: 0x00304EB4 File Offset: 0x003030B4
		public virtual StatusItem GetIncorrectFertStatusItem()
		{
			return Db.Get().CreatureStatusItems.WrongFertilizer;
		}

		// Token: 0x0600853A RID: 34106 RVA: 0x00304EC5 File Offset: 0x003030C5
		public virtual StatusItem GetIncorrectFertStatusItemMajor()
		{
			return Db.Get().CreatureStatusItems.WrongFertilizerMajor;
		}

		// Token: 0x0600853B RID: 34107 RVA: 0x00304ED8 File Offset: 0x003030D8
		protected virtual void AddAmounts(GameObject gameObject)
		{
			Amounts amounts = gameObject.GetAmounts();
			this.fertilization = amounts.Add(new AmountInstance(Db.Get().Amounts.Fertilization, gameObject));
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x0600853C RID: 34108 RVA: 0x00304F0D File Offset: 0x0030310D
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.Fertilized
				};
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x0600853D RID: 34109 RVA: 0x00304F1C File Offset: 0x0030311C
		public string WiltStateString
		{
			get
			{
				string result = "";
				if (base.smi.IsInsideState(base.smi.sm.replanted.fertilized.decaying.wrongFert))
				{
					result = this.GetIncorrectFertStatusItemMajor().resolveStringCallback(CREATURES.STATUSITEMS.WRONGFERTILIZERMAJOR.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.fertilized.absorbing.wrongFert))
				{
					result = this.GetIncorrectFertStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.WRONGFERTILIZER.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved))
				{
					result = this.GetStarvedStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.NEEDSFERTILIZER.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved.wrongFert))
				{
					result = this.GetIncorrectFertStatusItemMajor().resolveStringCallback(CREATURES.STATUSITEMS.WRONGFERTILIZERMAJOR.NAME, this);
				}
				return result;
			}
		}

		// Token: 0x0600853E RID: 34110 RVA: 0x00305050 File Offset: 0x00303250
		protected virtual void MakeModifiers()
		{
			this.consumptionRate = new AttributeModifier(Db.Get().Amounts.Fertilization.deltaAttribute.Id, -0.16666667f, CREATURES.STATS.FERTILIZATION.CONSUME_MODIFIER, false, false, true);
			this.absorptionRate = new AttributeModifier(Db.Get().Amounts.Fertilization.deltaAttribute.Id, 1.6666666f, CREATURES.STATS.FERTILIZATION.ABSORBING_MODIFIER, false, false, true);
		}

		// Token: 0x0600853F RID: 34111 RVA: 0x003050CC File Offset: 0x003032CC
		public void SetStorage(object obj)
		{
			this.storage = (Storage)obj;
			base.sm.fertilizerStorage.Set(this.storage, base.smi);
			IrrigationMonitor.Instance.DumpIncorrectFertilizers(this.storage, base.smi.gameObject);
			foreach (ManualDeliveryKG manualDeliveryKG in base.smi.gameObject.GetComponents<ManualDeliveryKG>())
			{
				bool flag = false;
				foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in base.def.consumedElements)
				{
					if (manualDeliveryKG.RequestedItemTag == consumeInfo.tag)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					manualDeliveryKG.SetStorage(this.storage);
					manualDeliveryKG.enabled = true;
				}
			}
		}

		// Token: 0x06008540 RID: 34112 RVA: 0x00305198 File Offset: 0x00303398
		public virtual bool AcceptsFertilizer()
		{
			PlantablePlot component = base.sm.fertilizerStorage.Get(this).GetComponent<PlantablePlot>();
			return component != null && component.AcceptsFertilizer;
		}

		// Token: 0x06008541 RID: 34113 RVA: 0x003051CD File Offset: 0x003033CD
		public bool Starved()
		{
			return this.fertilization.value == 0f;
		}

		// Token: 0x06008542 RID: 34114 RVA: 0x003051E4 File Offset: 0x003033E4
		public void UpdateFertilization(float dt)
		{
			if (base.def.consumedElements == null)
			{
				return;
			}
			if (this.storage == null)
			{
				return;
			}
			bool value = true;
			bool value2 = false;
			List<GameObject> items = this.storage.items;
			for (int i = 0; i < base.def.consumedElements.Length; i++)
			{
				PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
				float num = 0f;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (gameObject.HasTag(consumeInfo.tag))
					{
						num += gameObject.GetComponent<PrimaryElement>().Mass;
					}
					else if (gameObject.HasTag(base.def.wrongFertilizerTestTag))
					{
						value2 = true;
					}
				}
				this.total_available_mass = num;
				float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
				if (num < consumeInfo.massConsumptionRate * totalValue * dt)
				{
					value = false;
					break;
				}
			}
			base.sm.hasCorrectFertilizer.Set(value, base.smi, false);
			base.sm.hasIncorrectFertilizer.Set(value2, base.smi, false);
		}

		// Token: 0x06008543 RID: 34115 RVA: 0x00305324 File Offset: 0x00303524
		public void StartAbsorbing()
		{
			if (this.absorberHandle.IsValid())
			{
				return;
			}
			if (base.def.consumedElements == null || base.def.consumedElements.Length == 0)
			{
				return;
			}
			GameObject gameObject = base.smi.gameObject;
			float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
			PlantElementAbsorber.ConsumeInfo[] array = new PlantElementAbsorber.ConsumeInfo[base.def.consumedElements.Length];
			for (int i = 0; i < base.def.consumedElements.Length; i++)
			{
				PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
				consumeInfo.massConsumptionRate *= totalValue;
				array[i] = consumeInfo;
			}
			this.absorberHandle = Game.Instance.plantElementAbsorbers.Add(this.storage, array);
		}

		// Token: 0x06008544 RID: 34116 RVA: 0x003053F9 File Offset: 0x003035F9
		public void StopAbsorbing()
		{
			if (!this.absorberHandle.IsValid())
			{
				return;
			}
			this.absorberHandle = Game.Instance.plantElementAbsorbers.Remove(this.absorberHandle);
		}

		// Token: 0x040065E3 RID: 26083
		public AttributeModifier consumptionRate;

		// Token: 0x040065E4 RID: 26084
		public AttributeModifier absorptionRate;

		// Token: 0x040065E5 RID: 26085
		protected AmountInstance fertilization;

		// Token: 0x040065E6 RID: 26086
		private Storage storage;

		// Token: 0x040065E7 RID: 26087
		private HandleVector<int>.Handle absorberHandle = HandleVector<int>.InvalidHandle;

		// Token: 0x040065E8 RID: 26088
		private float total_available_mass;
	}
}
