using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000348 RID: 840
public class SpecialCargoBayCluster : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>
{
	// Token: 0x06001111 RID: 4369 RVA: 0x0005BEB8 File Offset: 0x0005A0B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.close;
		this.close.DefaultState(this.close.idle);
		this.close.closing.Target(this.Door).PlayAnim("close").OnAnimQueueComplete(this.close.idle).Target(this.masterTarget);
		this.close.idle.Target(this.Door).PlayAnim("close_idle").ParamTransition<bool>(this.IsDoorOpen, this.open.opening, GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.IsTrue).Target(this.masterTarget);
		this.close.cloud.Target(this.Door).PlayAnim("play_cloud").OnAnimQueueComplete(this.close.idle).Target(this.masterTarget);
		this.open.DefaultState(this.close.idle);
		this.open.opening.Target(this.Door).PlayAnim("open").OnAnimQueueComplete(this.open.idle).Target(this.masterTarget);
		this.open.idle.Target(this.Door).PlayAnim("open_idle").Enter(new StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State.Callback(SpecialCargoBayCluster.DropInventory)).Enter(new StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State.Callback(SpecialCargoBayCluster.CloseDoorAutomatically)).ParamTransition<bool>(this.IsDoorOpen, this.close.closing, GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.IsFalse).Target(this.masterTarget);
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x0005C066 File Offset: 0x0005A266
	public static void CloseDoorAutomatically(SpecialCargoBayCluster.Instance smi)
	{
		smi.CloseDoorAutomatically();
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x0005C06E File Offset: 0x0005A26E
	public static void DropInventory(SpecialCargoBayCluster.Instance smi)
	{
		smi.DropInventory();
	}

	// Token: 0x04000943 RID: 2371
	public const string DOOR_METER_TARGET_NAME = "fg_meter_target";

	// Token: 0x04000944 RID: 2372
	public const string TRAPPED_CRITTER_PIVOT_SYMBOL_NAME = "critter";

	// Token: 0x04000945 RID: 2373
	public const string LOOT_SYMBOL_NAME = "loot";

	// Token: 0x04000946 RID: 2374
	public const string DEATH_CLOUD_ANIM_NAME = "play_cloud";

	// Token: 0x04000947 RID: 2375
	private const string OPEN_DOOR_ANIM_NAME = "open";

	// Token: 0x04000948 RID: 2376
	private const string CLOSE_DOOR_ANIM_NAME = "close";

	// Token: 0x04000949 RID: 2377
	private const string OPEN_DOOR_IDLE_ANIM_NAME = "open_idle";

	// Token: 0x0400094A RID: 2378
	private const string CLOSE_DOOR_IDLE_ANIM_NAME = "close_idle";

	// Token: 0x0400094B RID: 2379
	public SpecialCargoBayCluster.OpenStates open;

	// Token: 0x0400094C RID: 2380
	public SpecialCargoBayCluster.CloseStates close;

	// Token: 0x0400094D RID: 2381
	public StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.BoolParameter IsDoorOpen;

	// Token: 0x0400094E RID: 2382
	public StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.TargetParameter Door;

	// Token: 0x02000F99 RID: 3993
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005644 RID: 22084
		public Vector2 trappedOffset = new Vector2(0f, -0.3f);
	}

	// Token: 0x02000F9A RID: 3994
	public class OpenStates : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State
	{
		// Token: 0x04005645 RID: 22085
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State opening;

		// Token: 0x04005646 RID: 22086
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State idle;
	}

	// Token: 0x02000F9B RID: 3995
	public class CloseStates : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State
	{
		// Token: 0x04005647 RID: 22087
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State closing;

		// Token: 0x04005648 RID: 22088
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State idle;

		// Token: 0x04005649 RID: 22089
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State cloud;
	}

	// Token: 0x02000F9C RID: 3996
	public new class Instance : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.GameInstance
	{
		// Token: 0x06007279 RID: 29305 RVA: 0x002BF76D File Offset: 0x002BD96D
		public void PlayDeathCloud()
		{
			if (base.IsInsideState(base.sm.close.idle))
			{
				this.GoTo(base.sm.close.cloud);
			}
		}

		// Token: 0x0600727A RID: 29306 RVA: 0x002BF79D File Offset: 0x002BD99D
		public void CloseDoor()
		{
			base.sm.IsDoorOpen.Set(false, base.smi, false);
		}

		// Token: 0x0600727B RID: 29307 RVA: 0x002BF7B8 File Offset: 0x002BD9B8
		public void OpenDoor()
		{
			base.sm.IsDoorOpen.Set(true, base.smi, false);
		}

		// Token: 0x0600727C RID: 29308 RVA: 0x002BF7D4 File Offset: 0x002BD9D4
		public Instance(IStateMachineTarget master, SpecialCargoBayCluster.Def def) : base(master, def)
		{
			this.buildingAnimController = base.GetComponent<KBatchedAnimController>();
			this.doorMeter = new MeterController(this.buildingAnimController, "fg_meter_target", "close_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			this.doorAnimController = this.doorMeter.meterController;
			KBatchedAnimTracker componentInChildren = this.doorAnimController.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
			base.sm.Door.Set(this.doorAnimController.gameObject, base.smi, false);
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.critterStorage = components[0];
			this.sideProductStorage = components[1];
			base.Subscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
		}

		// Token: 0x0600727D RID: 29309 RVA: 0x002BF899 File Offset: 0x002BDA99
		public void CloseDoorAutomatically()
		{
			this.CloseDoor();
		}

		// Token: 0x0600727E RID: 29310 RVA: 0x002BF8A1 File Offset: 0x002BDAA1
		public override void StartSM()
		{
			base.StartSM();
		}

		// Token: 0x0600727F RID: 29311 RVA: 0x002BF8AC File Offset: 0x002BDAAC
		private void OnLaunchConditionChanged(object obj)
		{
			if (this.rocketModuleCluster.CraftInterface != null)
			{
				Clustercraft component = this.rocketModuleCluster.CraftInterface.GetComponent<Clustercraft>();
				if (component != null && component.Status == Clustercraft.CraftStatus.Launching)
				{
					this.CloseDoor();
				}
			}
		}

		// Token: 0x06007280 RID: 29312 RVA: 0x002BF8F8 File Offset: 0x002BDAF8
		public void DropInventory()
		{
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			foreach (GameObject gameObject in this.critterStorage.items)
			{
				if (gameObject != null)
				{
					Baggable component = gameObject.GetComponent<Baggable>();
					if (component != null)
					{
						component.keepWrangledNextTimeRemovedFromStorage = true;
					}
				}
			}
			this.critterStorage.DropAll(false, false, default(Vector3), true, list);
			this.sideProductStorage.DropAll(false, false, default(Vector3), true, list2);
			foreach (GameObject gameObject2 in list)
			{
				KBatchedAnimController component2 = gameObject2.GetComponent<KBatchedAnimController>();
				Vector3 storePositionForCritter = this.GetStorePositionForCritter(gameObject2);
				gameObject2.transform.SetPosition(storePositionForCritter);
				component2.SetSceneLayer(Grid.SceneLayer.Creatures);
				component2.Play("trussed", KAnim.PlayMode.Loop, 1f, 0f);
			}
			foreach (GameObject gameObject3 in list2)
			{
				KBatchedAnimController component3 = gameObject3.GetComponent<KBatchedAnimController>();
				Vector3 storePositionForDrops = this.GetStorePositionForDrops();
				gameObject3.transform.SetPosition(storePositionForDrops);
				component3.SetSceneLayer(Grid.SceneLayer.Ore);
			}
		}

		// Token: 0x06007281 RID: 29313 RVA: 0x002BFA80 File Offset: 0x002BDC80
		public Vector3 GetCritterPositionOffet(GameObject critter)
		{
			KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
			Vector3 zero = Vector3.zero;
			zero.x = base.def.trappedOffset.x - component.Offset.x;
			zero.y = base.def.trappedOffset.y - component.Offset.y;
			return zero;
		}

		// Token: 0x06007282 RID: 29314 RVA: 0x002BFAE4 File Offset: 0x002BDCE4
		public Vector3 GetStorePositionForCritter(GameObject critter)
		{
			Vector3 critterPositionOffet = this.GetCritterPositionOffet(critter);
			bool flag;
			return this.buildingAnimController.GetSymbolTransform("critter", out flag).GetColumn(3) + critterPositionOffet;
		}

		// Token: 0x06007283 RID: 29315 RVA: 0x002BFB24 File Offset: 0x002BDD24
		public Vector3 GetStorePositionForDrops()
		{
			bool flag;
			return this.buildingAnimController.GetSymbolTransform("loot", out flag).GetColumn(3);
		}

		// Token: 0x0400564A RID: 22090
		public MeterController doorMeter;

		// Token: 0x0400564B RID: 22091
		private Storage critterStorage;

		// Token: 0x0400564C RID: 22092
		private Storage sideProductStorage;

		// Token: 0x0400564D RID: 22093
		private KBatchedAnimController buildingAnimController;

		// Token: 0x0400564E RID: 22094
		private KBatchedAnimController doorAnimController;

		// Token: 0x0400564F RID: 22095
		[MyCmpGet]
		private RocketModuleCluster rocketModuleCluster;
	}
}
