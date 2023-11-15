using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020000C6 RID: 198
public class HiveEatingStates : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>
{
	// Token: 0x06000391 RID: 913 RVA: 0x0001BF44 File Offset: 0x0001A144
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.eating;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.eating.ToggleStatusItem(CREATURES.STATUSITEMS.HIVE_DIGESTING.NAME, CREATURES.STATUSITEMS.HIVE_DIGESTING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).DefaultState(this.eating.pre).Enter(delegate(HiveEatingStates.Instance smi)
		{
			smi.TurnOn();
		}).Exit(delegate(HiveEatingStates.Instance smi)
		{
			smi.TurnOff();
		});
		this.eating.pre.PlayAnim("eating_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.eating.loop);
		this.eating.loop.PlayAnim("eating_loop", KAnim.PlayMode.Loop).Update(delegate(HiveEatingStates.Instance smi, float dt)
		{
			smi.EatOreFromStorage(smi, dt);
		}, UpdateRate.SIM_4000ms, false).EventTransition(GameHashes.OnStorageChange, this.eating.pst, (HiveEatingStates.Instance smi) => !smi.storage.FindFirst(smi.def.consumedOre));
		this.eating.pst.PlayAnim("eating_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToEat, false);
	}

	// Token: 0x04000265 RID: 613
	public HiveEatingStates.EatingStates eating;

	// Token: 0x04000266 RID: 614
	public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State behaviourcomplete;

	// Token: 0x02000ED8 RID: 3800
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600705E RID: 28766 RVA: 0x002BAA9C File Offset: 0x002B8C9C
		public Def(Tag consumedOre)
		{
			this.consumedOre = consumedOre;
		}

		// Token: 0x04005456 RID: 21590
		public Tag consumedOre;
	}

	// Token: 0x02000ED9 RID: 3801
	public class EatingStates : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State
	{
		// Token: 0x04005457 RID: 21591
		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State pre;

		// Token: 0x04005458 RID: 21592
		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State loop;

		// Token: 0x04005459 RID: 21593
		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State pst;
	}

	// Token: 0x02000EDA RID: 3802
	public new class Instance : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.GameInstance
	{
		// Token: 0x06007060 RID: 28768 RVA: 0x002BAAB3 File Offset: 0x002B8CB3
		public Instance(Chore<HiveEatingStates.Instance> chore, HiveEatingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToEat);
		}

		// Token: 0x06007061 RID: 28769 RVA: 0x002BAAD7 File Offset: 0x002B8CD7
		public void TurnOn()
		{
			this.emitter.emitRads = 600f * this.emitter.emitRate;
			this.emitter.Refresh();
		}

		// Token: 0x06007062 RID: 28770 RVA: 0x002BAB00 File Offset: 0x002B8D00
		public void TurnOff()
		{
			this.emitter.emitRads = 0f;
			this.emitter.Refresh();
		}

		// Token: 0x06007063 RID: 28771 RVA: 0x002BAB20 File Offset: 0x002B8D20
		public void EatOreFromStorage(HiveEatingStates.Instance smi, float dt)
		{
			GameObject gameObject = smi.storage.FindFirst(smi.def.consumedOre);
			if (!gameObject)
			{
				return;
			}
			float num = 0.25f;
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component == null)
			{
				return;
			}
			PrimaryElement component2 = component.GetComponent<PrimaryElement>();
			if (component2 == null)
			{
				return;
			}
			Diet.Info dietInfo = smi.gameObject.AddOrGetDef<BeehiveCalorieMonitor.Def>().diet.GetDietInfo(component.PrefabTag);
			if (dietInfo == null)
			{
				return;
			}
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
			float calories = amountInstance.GetMax() - amountInstance.value;
			float num2 = dietInfo.ConvertCaloriesToConsumptionMass(calories);
			float num3 = num * dt;
			if (num2 < num3)
			{
				num3 = num2;
			}
			num3 = Mathf.Min(num3, component2.Mass);
			component2.Mass -= num3;
			Pickupable component3 = component2.GetComponent<Pickupable>();
			if (component3.storage != null)
			{
				component3.storage.Trigger(-1452790913, smi.gameObject);
				component3.storage.Trigger(-1697596308, smi.gameObject);
			}
			float calories2 = dietInfo.ConvertConsumptionMassToCalories(num3);
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = new CreatureCalorieMonitor.CaloriesConsumedEvent
			{
				tag = component.PrefabTag,
				calories = calories2
			};
			smi.gameObject.Trigger(-2038961714, caloriesConsumedEvent);
		}

		// Token: 0x0400545A RID: 21594
		[MyCmpReq]
		public Storage storage;

		// Token: 0x0400545B RID: 21595
		[MyCmpReq]
		private RadiationEmitter emitter;
	}
}
