using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020005BF RID: 1471
public class BeeHive : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>
{
	// Token: 0x06002440 RID: 9280 RVA: 0x000C5B30 File Offset: 0x000C3D30
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.enabled.grownStates;
		this.root.DoTutorial(Tutorial.TutorialMessages.TM_Radiation).Enter(delegate(BeeHive.StatesInstance smi)
		{
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
			if (amountInstance != null)
			{
				amountInstance.hide = true;
			}
		}).EventHandler(GameHashes.Died, delegate(BeeHive.StatesInstance smi)
		{
			PrimaryElement component = smi.GetComponent<PrimaryElement>();
			Storage component2 = smi.GetComponent<Storage>();
			byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id);
			component2.AddOre(SimHashes.NuclearWaste, BeeHiveTuning.WASTE_DROPPED_ON_DEATH, component.Temperature, index, BeeHiveTuning.GERMS_DROPPED_ON_DEATH, false, true);
			component2.DropAll(smi.master.transform.position, true, true, default(Vector3), true, null);
		});
		this.disabled.ToggleTag(GameTags.Creatures.Behaviours.DisableCreature).EventTransition(GameHashes.FoundationChanged, this.enabled, (BeeHive.StatesInstance smi) => !smi.IsDisabled()).EventTransition(GameHashes.EntombedChanged, this.enabled, (BeeHive.StatesInstance smi) => !smi.IsDisabled()).EventTransition(GameHashes.EnteredBreathableArea, this.enabled, (BeeHive.StatesInstance smi) => !smi.IsDisabled());
		this.enabled.EventTransition(GameHashes.FoundationChanged, this.disabled, (BeeHive.StatesInstance smi) => smi.IsDisabled()).EventTransition(GameHashes.EntombedChanged, this.disabled, (BeeHive.StatesInstance smi) => smi.IsDisabled()).EventTransition(GameHashes.Drowning, this.disabled, (BeeHive.StatesInstance smi) => smi.IsDisabled()).DefaultState(this.enabled.grownStates);
		this.enabled.growingStates.ParamTransition<float>(this.hiveGrowth, this.enabled.grownStates, (BeeHive.StatesInstance smi, float f) => f >= 1f).DefaultState(this.enabled.growingStates.idle);
		this.enabled.growingStates.idle.Update(delegate(BeeHive.StatesInstance smi, float dt)
		{
			smi.DeltaGrowth(dt / 600f / BeeHiveTuning.HIVE_GROWTH_TIME);
		}, UpdateRate.SIM_4000ms, false);
		this.enabled.grownStates.ParamTransition<float>(this.hiveGrowth, this.enabled.growingStates, (BeeHive.StatesInstance smi, float f) => f < 1f).DefaultState(this.enabled.grownStates.dayTime);
		this.enabled.grownStates.dayTime.EventTransition(GameHashes.Nighttime, (BeeHive.StatesInstance smi) => GameClock.Instance, this.enabled.grownStates.nightTime, (BeeHive.StatesInstance smi) => GameClock.Instance.IsNighttime());
		this.enabled.grownStates.nightTime.EventTransition(GameHashes.NewDay, (BeeHive.StatesInstance smi) => GameClock.Instance, this.enabled.grownStates.dayTime, (BeeHive.StatesInstance smi) => GameClock.Instance.GetTimeSinceStartOfCycle() <= 1f).Exit(delegate(BeeHive.StatesInstance smi)
		{
			if (!GameClock.Instance.IsNighttime())
			{
				smi.SpawnNewLarvaFromHive();
			}
		});
	}

	// Token: 0x040014CA RID: 5322
	public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State disabled;

	// Token: 0x040014CB RID: 5323
	public BeeHive.EnabledStates enabled;

	// Token: 0x040014CC RID: 5324
	public StateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.FloatParameter hiveGrowth = new StateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.FloatParameter(1f);

	// Token: 0x0200124B RID: 4683
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005F0B RID: 24331
		public string beePrefabID;

		// Token: 0x04005F0C RID: 24332
		public string larvaPrefabID;
	}

	// Token: 0x0200124C RID: 4684
	public class GrowingStates : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State
	{
		// Token: 0x04005F0D RID: 24333
		public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State idle;
	}

	// Token: 0x0200124D RID: 4685
	public class GrownStates : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State
	{
		// Token: 0x04005F0E RID: 24334
		public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State dayTime;

		// Token: 0x04005F0F RID: 24335
		public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State nightTime;
	}

	// Token: 0x0200124E RID: 4686
	public class EnabledStates : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State
	{
		// Token: 0x04005F10 RID: 24336
		public BeeHive.GrowingStates growingStates;

		// Token: 0x04005F11 RID: 24337
		public BeeHive.GrownStates grownStates;
	}

	// Token: 0x0200124F RID: 4687
	public class StatesInstance : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.GameInstance
	{
		// Token: 0x06007C97 RID: 31895 RVA: 0x002E1DD6 File Offset: 0x002DFFD6
		public StatesInstance(IStateMachineTarget master, BeeHive.Def def) : base(master, def)
		{
			base.Subscribe(1119167081, new Action<object>(this.OnNewGameSpawn));
			Components.BeeHives.Add(this);
		}

		// Token: 0x06007C98 RID: 31896 RVA: 0x002E1E02 File Offset: 0x002E0002
		public void SetUpNewHive()
		{
			base.sm.hiveGrowth.Set(0f, this, false);
		}

		// Token: 0x06007C99 RID: 31897 RVA: 0x002E1E1C File Offset: 0x002E001C
		protected override void OnCleanUp()
		{
			Components.BeeHives.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x06007C9A RID: 31898 RVA: 0x002E1E2F File Offset: 0x002E002F
		private void OnNewGameSpawn(object data)
		{
			this.NewGamePopulateHive();
		}

		// Token: 0x06007C9B RID: 31899 RVA: 0x002E1E38 File Offset: 0x002E0038
		private void NewGamePopulateHive()
		{
			int num = 1;
			for (int i = 0; i < num; i++)
			{
				this.SpawnNewBeeFromHive();
			}
			num = 1;
			for (int j = 0; j < num; j++)
			{
				this.SpawnNewLarvaFromHive();
			}
		}

		// Token: 0x06007C9C RID: 31900 RVA: 0x002E1E6D File Offset: 0x002E006D
		public bool IsFullyGrown()
		{
			return base.sm.hiveGrowth.Get(this) >= 1f;
		}

		// Token: 0x06007C9D RID: 31901 RVA: 0x002E1E8C File Offset: 0x002E008C
		public void DeltaGrowth(float delta)
		{
			float num = base.sm.hiveGrowth.Get(this);
			num += delta;
			Mathf.Clamp01(num);
			base.sm.hiveGrowth.Set(num, this, false);
		}

		// Token: 0x06007C9E RID: 31902 RVA: 0x002E1ECA File Offset: 0x002E00CA
		public void SpawnNewLarvaFromHive()
		{
			Util.KInstantiate(Assets.GetPrefab(base.def.larvaPrefabID), base.transform.GetPosition()).SetActive(true);
		}

		// Token: 0x06007C9F RID: 31903 RVA: 0x002E1EF7 File Offset: 0x002E00F7
		public void SpawnNewBeeFromHive()
		{
			Util.KInstantiate(Assets.GetPrefab(base.def.beePrefabID), base.transform.GetPosition()).SetActive(true);
		}

		// Token: 0x06007CA0 RID: 31904 RVA: 0x002E1F24 File Offset: 0x002E0124
		public bool IsDisabled()
		{
			KPrefabID component = base.GetComponent<KPrefabID>();
			return component.HasTag(GameTags.Creatures.HasNoFoundation) || component.HasTag(GameTags.Entombed) || component.HasTag(GameTags.Creatures.Drowning);
		}
	}
}
