using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008E9 RID: 2281
public class SapTree : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>
{
	// Token: 0x060041F0 RID: 16880 RVA: 0x00170938 File Offset: 0x0016EB38
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.dead.ToggleStatusItem(CREATURES.STATUSITEMS.DEAD.NAME, CREATURES.STATUSITEMS.DEAD.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(SapTree.StatesInstance smi)
		{
			GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
			smi.master.Trigger(1623392196, null);
			smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
		});
		this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.normal);
		this.alive.normal.DefaultState(this.alive.normal.idle).EventTransition(GameHashes.Wilt, this.alive.wilting, (SapTree.StatesInstance smi) => smi.wiltCondition.IsWilting()).Update(delegate(SapTree.StatesInstance smi, float dt)
		{
			smi.CheckForFood();
		}, UpdateRate.SIM_1000ms, false);
		this.alive.normal.idle.PlayAnim("idle", KAnim.PlayMode.Loop).ToggleStatusItem(CREATURES.STATUSITEMS.IDLE.NAME, CREATURES.STATUSITEMS.IDLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).ParamTransition<bool>(this.hasNearbyEnemy, this.alive.normal.attacking_pre, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsTrue).ParamTransition<float>(this.storedSap, this.alive.normal.oozing, (SapTree.StatesInstance smi, float p) => p >= smi.def.stomachSize).ParamTransition<GameObject>(this.foodItem, this.alive.normal.eating, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsNotNull);
		this.alive.normal.eating.PlayAnim("eat_pre", KAnim.PlayMode.Once).QueueAnim("eat_loop", true, null).Update(delegate(SapTree.StatesInstance smi, float dt)
		{
			smi.EatFoodItem(dt);
		}, UpdateRate.SIM_1000ms, false).ParamTransition<GameObject>(this.foodItem, this.alive.normal.eating_pst, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsNull).ParamTransition<float>(this.storedSap, this.alive.normal.eating_pst, (SapTree.StatesInstance smi, float p) => p >= smi.def.stomachSize);
		this.alive.normal.eating_pst.PlayAnim("eat_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
		this.alive.normal.oozing.PlayAnim("ooze_pre", KAnim.PlayMode.Once).QueueAnim("ooze_loop", true, null).Update(delegate(SapTree.StatesInstance smi, float dt)
		{
			smi.Ooze(dt);
		}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.storedSap, this.alive.normal.oozing_pst, (SapTree.StatesInstance smi, float p) => p <= 0f).ParamTransition<bool>(this.hasNearbyEnemy, this.alive.normal.oozing_pst, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsTrue);
		this.alive.normal.oozing_pst.PlayAnim("ooze_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
		this.alive.normal.attacking_pre.PlayAnim("attacking_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.attacking);
		this.alive.normal.attacking.PlayAnim("attacking_loop", KAnim.PlayMode.Once).Enter(delegate(SapTree.StatesInstance smi)
		{
			smi.DoAttack();
		}).OnAnimQueueComplete(this.alive.normal.attacking_cooldown);
		this.alive.normal.attacking_cooldown.PlayAnim("attacking_pst", KAnim.PlayMode.Once).QueueAnim("attack_cooldown", true, null).ParamTransition<bool>(this.hasNearbyEnemy, this.alive.normal.attacking_done, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsFalse).ScheduleGoTo((SapTree.StatesInstance smi) => smi.def.attackCooldown, this.alive.normal.attacking);
		this.alive.normal.attacking_done.PlayAnim("attack_to_idle", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
		this.alive.wilting.PlayAnim("withered", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.normal, null).ToggleTag(GameTags.PreventEmittingDisease);
	}

	// Token: 0x04002B09 RID: 11017
	public SapTree.AliveStates alive;

	// Token: 0x04002B0A RID: 11018
	public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State dead;

	// Token: 0x04002B0B RID: 11019
	private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.TargetParameter foodItem;

	// Token: 0x04002B0C RID: 11020
	private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.BoolParameter hasNearbyEnemy;

	// Token: 0x04002B0D RID: 11021
	private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.FloatParameter storedSap;

	// Token: 0x02001738 RID: 5944
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006DE9 RID: 28137
		public Vector2I foodSenseArea;

		// Token: 0x04006DEA RID: 28138
		public float massEatRate;

		// Token: 0x04006DEB RID: 28139
		public float kcalorieToKGConversionRatio;

		// Token: 0x04006DEC RID: 28140
		public float stomachSize;

		// Token: 0x04006DED RID: 28141
		public float oozeRate;

		// Token: 0x04006DEE RID: 28142
		public List<Vector3> oozeOffsets;

		// Token: 0x04006DEF RID: 28143
		public Vector2I attackSenseArea;

		// Token: 0x04006DF0 RID: 28144
		public float attackCooldown;
	}

	// Token: 0x02001739 RID: 5945
	public class AliveStates : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.PlantAliveSubState
	{
		// Token: 0x04006DF1 RID: 28145
		public SapTree.NormalStates normal;

		// Token: 0x04006DF2 RID: 28146
		public SapTree.WiltingState wilting;
	}

	// Token: 0x0200173A RID: 5946
	public class NormalStates : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State
	{
		// Token: 0x04006DF3 RID: 28147
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State idle;

		// Token: 0x04006DF4 RID: 28148
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State eating;

		// Token: 0x04006DF5 RID: 28149
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State eating_pst;

		// Token: 0x04006DF6 RID: 28150
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State oozing;

		// Token: 0x04006DF7 RID: 28151
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State oozing_pst;

		// Token: 0x04006DF8 RID: 28152
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_pre;

		// Token: 0x04006DF9 RID: 28153
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking;

		// Token: 0x04006DFA RID: 28154
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_cooldown;

		// Token: 0x04006DFB RID: 28155
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_done;
	}

	// Token: 0x0200173B RID: 5947
	public class WiltingState : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State
	{
		// Token: 0x04006DFC RID: 28156
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting_pre;

		// Token: 0x04006DFD RID: 28157
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting;

		// Token: 0x04006DFE RID: 28158
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting_pst;
	}

	// Token: 0x0200173C RID: 5948
	public class StatesInstance : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.GameInstance
	{
		// Token: 0x06008DDE RID: 36318 RVA: 0x0031DB1C File Offset: 0x0031BD1C
		public StatesInstance(IStateMachineTarget master, SapTree.Def def) : base(master, def)
		{
			Vector2I vector2I = Grid.PosToXY(base.gameObject.transform.GetPosition());
			Vector2I vector2I2 = new Vector2I(vector2I.x - def.attackSenseArea.x / 2, vector2I.y);
			this.attackExtents = new Extents(vector2I2.x, vector2I2.y, def.attackSenseArea.x, def.attackSenseArea.y);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("SapTreeAttacker", this, this.attackExtents, GameScenePartitioner.Instance.objectLayers[0], new Action<object>(this.OnMinionChanged));
			Vector2I vector2I3 = new Vector2I(vector2I.x - def.foodSenseArea.x / 2, vector2I.y);
			this.feedExtents = new Extents(vector2I3.x, vector2I3.y, def.foodSenseArea.x, def.foodSenseArea.y);
		}

		// Token: 0x06008DDF RID: 36319 RVA: 0x0031DC17 File Offset: 0x0031BE17
		protected override void OnCleanUp()
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		}

		// Token: 0x06008DE0 RID: 36320 RVA: 0x0031DC2C File Offset: 0x0031BE2C
		public void EatFoodItem(float dt)
		{
			Pickupable pickupable = base.sm.foodItem.Get(this).GetComponent<Pickupable>().Take(base.def.massEatRate * dt);
			if (pickupable != null)
			{
				float mass = pickupable.GetComponent<Edible>().Calories * 0.001f * base.def.kcalorieToKGConversionRatio;
				Util.KDestroyGameObject(pickupable.gameObject);
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				this.storage.AddLiquid(SimHashes.Resin, mass, component.Temperature, byte.MaxValue, 0, true, false);
				base.sm.storedSap.Set(this.storage.GetMassAvailable(SimHashes.Resin.CreateTag()), this, false);
			}
		}

		// Token: 0x06008DE1 RID: 36321 RVA: 0x0031DCE4 File Offset: 0x0031BEE4
		public void Ooze(float dt)
		{
			float num = Mathf.Min(base.sm.storedSap.Get(this), dt * base.def.oozeRate);
			if (num <= 0f)
			{
				return;
			}
			int index = Mathf.FloorToInt(GameClock.Instance.GetTime() % (float)base.def.oozeOffsets.Count);
			this.storage.DropSome(SimHashes.Resin.CreateTag(), num, false, true, base.def.oozeOffsets[index], true, false);
			base.sm.storedSap.Set(this.storage.GetMassAvailable(SimHashes.Resin.CreateTag()), this, false);
		}

		// Token: 0x06008DE2 RID: 36322 RVA: 0x0031DD94 File Offset: 0x0031BF94
		public void CheckForFood()
		{
			ListPool<ScenePartitionerEntry, SapTree>.PooledList pooledList = ListPool<ScenePartitionerEntry, SapTree>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(this.feedExtents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable.GetComponent<Edible>() != null)
				{
					base.sm.foodItem.Set(pickupable.gameObject, this, false);
					pooledList.Recycle();
					return;
				}
			}
			base.sm.foodItem.Set(null, this);
			pooledList.Recycle();
		}

		// Token: 0x06008DE3 RID: 36323 RVA: 0x0031DE50 File Offset: 0x0031C050
		public bool DoAttack()
		{
			int num = this.weapon.AttackArea(base.transform.GetPosition());
			base.sm.hasNearbyEnemy.Set(num > 0, this, false);
			return true;
		}

		// Token: 0x06008DE4 RID: 36324 RVA: 0x0031DE8C File Offset: 0x0031C08C
		private void OnMinionChanged(object obj)
		{
			if (obj as GameObject != null)
			{
				base.sm.hasNearbyEnemy.Set(true, this, false);
			}
		}

		// Token: 0x04006DFF RID: 28159
		[MyCmpReq]
		public WiltCondition wiltCondition;

		// Token: 0x04006E00 RID: 28160
		[MyCmpReq]
		public EntombVulnerable entombVulnerable;

		// Token: 0x04006E01 RID: 28161
		[MyCmpReq]
		private Storage storage;

		// Token: 0x04006E02 RID: 28162
		[MyCmpReq]
		private Weapon weapon;

		// Token: 0x04006E03 RID: 28163
		private HandleVector<int>.Handle partitionerEntry;

		// Token: 0x04006E04 RID: 28164
		private Extents feedExtents;

		// Token: 0x04006E05 RID: 28165
		private Extents attackExtents;
	}
}
