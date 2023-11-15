using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200049F RID: 1183
public class HugMonitor : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>
{
	// Token: 0x06001AA5 RID: 6821 RVA: 0x0008E67C File Offset: 0x0008C87C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Update(new Action<HugMonitor.Instance, float>(this.UpdateHugEggCooldownTimer), UpdateRate.SIM_1000ms, false).ToggleBehaviour(GameTags.Creatures.WantsToTendEgg, (HugMonitor.Instance smi) => smi.UpdateHasTarget(), delegate(HugMonitor.Instance smi)
		{
			smi.hugTarget = null;
		});
		this.normal.DefaultState(this.normal.idle).ParamTransition<float>(this.hugFrenzyTimer, this.hugFrenzy, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsGTZero);
		this.normal.idle.ParamTransition<float>(this.wantsHugCooldownTimer, this.normal.hugReady.seekingHug, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsLTEZero).Update(new Action<HugMonitor.Instance, float>(this.UpdateWantsHugCooldownTimer), UpdateRate.SIM_1000ms, false);
		this.normal.hugReady.ToggleReactable(new Func<HugMonitor.Instance, Reactable>(this.GetHugReactable));
		this.normal.hugReady.passiveHug.ParamTransition<float>(this.wantsHugCooldownTimer, this.normal.hugReady.seekingHug, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsLTEZero).Update(new Action<HugMonitor.Instance, float>(this.UpdateWantsHugCooldownTimer), UpdateRate.SIM_1000ms, false).ToggleStatusItem(CREATURES.STATUSITEMS.HUGMINIONWAITING.NAME, CREATURES.STATUSITEMS.HUGMINIONWAITING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.normal.hugReady.seekingHug.ToggleBehaviour(GameTags.Creatures.WantsAHug, (HugMonitor.Instance smi) => true, delegate(HugMonitor.Instance smi)
		{
			this.wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldownFailed, smi, false);
			smi.GoTo(this.normal.hugReady.passiveHug);
		});
		this.hugFrenzy.ParamTransition<float>(this.hugFrenzyTimer, this.normal, (HugMonitor.Instance smi, float p) => p <= 0f && !smi.IsHugging()).Update(new Action<HugMonitor.Instance, float>(this.UpdateHugFrenzyTimer), UpdateRate.SIM_1000ms, false).ToggleEffect((HugMonitor.Instance smi) => smi.frenzyEffect).ToggleLoopingSound(HugMonitor.soundPath, null, true, true, true).Enter(delegate(HugMonitor.Instance smi)
		{
			smi.hugParticleFx = Util.KInstantiate(EffectPrefabs.Instance.HugFrenzyFX, smi.master.transform.GetPosition() + smi.hugParticleOffset);
			smi.hugParticleFx.transform.SetParent(smi.master.transform);
			smi.hugParticleFx.SetActive(true);
		}).Exit(delegate(HugMonitor.Instance smi)
		{
			Util.KDestroyGameObject(smi.hugParticleFx);
			this.wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldown, smi, false);
		});
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x0008E8FE File Offset: 0x0008CAFE
	private Reactable GetHugReactable(HugMonitor.Instance smi)
	{
		return new HugMinionReactable(smi.gameObject);
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x0008E90B File Offset: 0x0008CB0B
	private void UpdateWantsHugCooldownTimer(HugMonitor.Instance smi, float dt)
	{
		this.wantsHugCooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x0008E926 File Offset: 0x0008CB26
	private void UpdateHugEggCooldownTimer(HugMonitor.Instance smi, float dt)
	{
		this.hugEggCooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x0008E941 File Offset: 0x0008CB41
	private void UpdateHugFrenzyTimer(HugMonitor.Instance smi, float dt)
	{
		this.hugFrenzyTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x04000ED0 RID: 3792
	private static string soundPath = GlobalAssets.GetSound("Squirrel_hug_frenzyFX", false);

	// Token: 0x04000ED1 RID: 3793
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter hugFrenzyTimer;

	// Token: 0x04000ED2 RID: 3794
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter wantsHugCooldownTimer;

	// Token: 0x04000ED3 RID: 3795
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter hugEggCooldownTimer;

	// Token: 0x04000ED4 RID: 3796
	public HugMonitor.NormalStates normal;

	// Token: 0x04000ED5 RID: 3797
	public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State hugFrenzy;

	// Token: 0x02001142 RID: 4418
	public class HUGTUNING
	{
		// Token: 0x04005BCF RID: 23503
		public const float HUG_EGG_TIME = 15f;

		// Token: 0x04005BD0 RID: 23504
		public const float HUG_DUPE_WAIT = 60f;

		// Token: 0x04005BD1 RID: 23505
		public const float FRENZY_EGGS_PER_CYCLE = 6f;

		// Token: 0x04005BD2 RID: 23506
		public const float FRENZY_EGG_TRAVEL_TIME_BUFFER = 5f;

		// Token: 0x04005BD3 RID: 23507
		public const float HUG_FRENZY_DURATION = 120f;
	}

	// Token: 0x02001143 RID: 4419
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005BD4 RID: 23508
		public float hugsPerCycle = 2f;

		// Token: 0x04005BD5 RID: 23509
		public float scanningInterval = 30f;

		// Token: 0x04005BD6 RID: 23510
		public float hugFrenzyDuration = 120f;

		// Token: 0x04005BD7 RID: 23511
		public float hugFrenzyCooldown = 480f;

		// Token: 0x04005BD8 RID: 23512
		public float hugFrenzyCooldownFailed = 120f;

		// Token: 0x04005BD9 RID: 23513
		public float scanningIntervalFrenzy = 15f;
	}

	// Token: 0x02001144 RID: 4420
	public class HugReadyStates : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State
	{
		// Token: 0x04005BDA RID: 23514
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State passiveHug;

		// Token: 0x04005BDB RID: 23515
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State seekingHug;
	}

	// Token: 0x02001145 RID: 4421
	public class NormalStates : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State
	{
		// Token: 0x04005BDC RID: 23516
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State idle;

		// Token: 0x04005BDD RID: 23517
		public HugMonitor.HugReadyStates hugReady;
	}

	// Token: 0x02001146 RID: 4422
	public new class Instance : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.GameInstance
	{
		// Token: 0x060078E8 RID: 30952 RVA: 0x002D7D54 File Offset: 0x002D5F54
		public Instance(IStateMachineTarget master, HugMonitor.Def def) : base(master, def)
		{
			this.frenzyEffect = Db.Get().effects.Get("HuggingFrenzy");
			this.RefreshSearchTime();
			base.smi.sm.wantsHugCooldownTimer.Set(UnityEngine.Random.Range(base.smi.def.hugFrenzyCooldownFailed, base.smi.def.hugFrenzyCooldown), base.smi, false);
		}

		// Token: 0x060078E9 RID: 30953 RVA: 0x002D7DCC File Offset: 0x002D5FCC
		private void RefreshSearchTime()
		{
			if (this.hugTarget == null)
			{
				base.smi.sm.hugEggCooldownTimer.Set(this.GetScanningInterval(), base.smi, false);
				return;
			}
			base.smi.sm.hugEggCooldownTimer.Set(this.GetHugInterval(), base.smi, false);
		}

		// Token: 0x060078EA RID: 30954 RVA: 0x002D7E2E File Offset: 0x002D602E
		private float GetScanningInterval()
		{
			if (!this.IsHuggingFrenzy())
			{
				return base.def.scanningInterval;
			}
			return base.def.scanningIntervalFrenzy;
		}

		// Token: 0x060078EB RID: 30955 RVA: 0x002D7E4F File Offset: 0x002D604F
		private float GetHugInterval()
		{
			if (this.IsHuggingFrenzy())
			{
				return 0f;
			}
			return 600f / base.def.hugsPerCycle;
		}

		// Token: 0x060078EC RID: 30956 RVA: 0x002D7E70 File Offset: 0x002D6070
		public bool IsHuggingFrenzy()
		{
			return base.smi.GetCurrentState() == base.smi.sm.hugFrenzy;
		}

		// Token: 0x060078ED RID: 30957 RVA: 0x002D7E8F File Offset: 0x002D608F
		public bool IsHugging()
		{
			return base.smi.GetSMI<AnimInterruptMonitor.Instance>().anims != null;
		}

		// Token: 0x060078EE RID: 30958 RVA: 0x002D7EA4 File Offset: 0x002D60A4
		public bool UpdateHasTarget()
		{
			if (this.hugTarget == null)
			{
				if (base.smi.sm.hugEggCooldownTimer.Get(base.smi) > 0f)
				{
					return false;
				}
				this.FindEgg();
				this.RefreshSearchTime();
			}
			return this.hugTarget != null;
		}

		// Token: 0x060078EF RID: 30959 RVA: 0x002D7EFC File Offset: 0x002D60FC
		public void EnterHuggingFrenzy()
		{
			base.smi.sm.hugFrenzyTimer.Set(base.smi.def.hugFrenzyDuration, base.smi, false);
			base.smi.sm.hugEggCooldownTimer.Set(0f, base.smi, false);
		}

		// Token: 0x060078F0 RID: 30960 RVA: 0x002D7F58 File Offset: 0x002D6158
		private void FindEgg()
		{
			this.hugTarget = null;
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			ListPool<KMonoBehaviour, SquirrelHugConfig>.PooledList pooledList2 = ListPool<KMonoBehaviour, SquirrelHugConfig>.Allocate();
			Vector3 position = base.master.transform.GetPosition();
			Extents extents = new Extents(Grid.PosToCell(position), 10);
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.completeBuildings, pooledList);
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
			Navigator component = base.GetComponent<Navigator>();
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				KMonoBehaviour kmonoBehaviour = scenePartitionerEntry.obj as KMonoBehaviour;
				KPrefabID component2 = kmonoBehaviour.GetComponent<KPrefabID>();
				if (!component2.HasTag(GameTags.Creatures.ReservedByCreature))
				{
					int cell = Grid.PosToCell(kmonoBehaviour);
					if (component.CanReach(cell))
					{
						EggIncubator component3 = kmonoBehaviour.GetComponent<EggIncubator>();
						if (component3 != null)
						{
							if (component3.Occupant == null || component3.Occupant.HasTag(GameTags.Creatures.ReservedByCreature) || !component3.Occupant.HasTag(GameTags.Egg))
							{
								continue;
							}
							if (component3.Occupant.GetComponent<Effects>().HasEffect("EggHug"))
							{
								continue;
							}
						}
						else if (!component2.HasTag(GameTags.Egg) || kmonoBehaviour.GetComponent<Effects>().HasEffect("EggHug"))
						{
							continue;
						}
						pooledList2.Add(kmonoBehaviour);
					}
				}
			}
			if (pooledList2.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, pooledList2.Count);
				KMonoBehaviour kmonoBehaviour2 = pooledList2[index];
				this.hugTarget = kmonoBehaviour2.gameObject;
			}
			pooledList.Recycle();
			pooledList2.Recycle();
		}

		// Token: 0x04005BDE RID: 23518
		public GameObject hugParticleFx;

		// Token: 0x04005BDF RID: 23519
		public Vector3 hugParticleOffset;

		// Token: 0x04005BE0 RID: 23520
		public GameObject hugTarget;

		// Token: 0x04005BE1 RID: 23521
		[MyCmpGet]
		private Effects effects;

		// Token: 0x04005BE2 RID: 23522
		public Effect frenzyEffect;
	}
}
