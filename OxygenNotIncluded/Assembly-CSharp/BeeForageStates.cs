using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000AA RID: 170
public class BeeForageStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>
{
	// Token: 0x060002F9 RID: 761 RVA: 0x00017D50 File Offset: 0x00015F50
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.collect.findTarget;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.FORAGINGMATERIAL.NAME, CREATURES.STATUSITEMS.FORAGINGMATERIAL.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.UnreserveTarget)).Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.DropAll));
		this.collect.findTarget.Enter(delegate(BeeForageStates.Instance smi)
		{
			BeeForageStates.FindTarget(smi);
			smi.targetHive = smi.master.GetComponent<Bee>().FindHiveInRoom();
			if (smi.targetHive != null)
			{
				if (smi.forageTarget != null)
				{
					BeeForageStates.ReserveTarget(smi);
					smi.GoTo(this.collect.forage.moveToTarget);
					return;
				}
				if (Grid.IsValidCell(smi.targetMiningCell))
				{
					smi.GoTo(this.collect.mine.moveToTarget);
					return;
				}
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.collect.forage.moveToTarget.MoveTo(new Func<BeeForageStates.Instance, int>(BeeForageStates.GetOreCell), this.collect.forage.pickupTarget, this.behaviourcomplete, false);
		this.collect.forage.pickupTarget.PlayAnim("pickup_pre").Enter(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.PickupComplete)).OnAnimQueueComplete(this.storage.moveToHive);
		this.collect.mine.moveToTarget.MoveTo((BeeForageStates.Instance smi) => smi.targetMiningCell, this.collect.mine.mineTarget, this.behaviourcomplete, false);
		this.collect.mine.mineTarget.PlayAnim("mining_pre").QueueAnim("mining_loop", false, null).QueueAnim("mining_pst", false, null).Enter(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.MineTarget)).OnAnimQueueComplete(this.storage.moveToHive);
		this.storage.Enter(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(this.HoldOre)).Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(this.DropOre));
		this.storage.moveToHive.Enter(delegate(BeeForageStates.Instance smi)
		{
			if (!smi.targetHive)
			{
				smi.targetHive = smi.master.GetComponent<Bee>().FindHiveInRoom();
			}
			if (!smi.targetHive)
			{
				smi.GoTo(this.storage.dropMaterial);
			}
		}).MoveTo((BeeForageStates.Instance smi) => Grid.OffsetCell(Grid.PosToCell(smi.targetHive.transform.GetPosition()), smi.hiveCellOffset), this.storage.storeMaterial, this.behaviourcomplete, false);
		this.storage.storeMaterial.PlayAnim("deposit").Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.StoreOre)).OnAnimQueueComplete(this.behaviourcomplete.pre);
		this.storage.dropMaterial.Enter(delegate(BeeForageStates.Instance smi)
		{
			smi.GoTo(this.behaviourcomplete);
		}).Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.DropAll));
		this.behaviourcomplete.DefaultState(this.behaviourcomplete.pst);
		this.behaviourcomplete.pre.PlayAnim("spawn").OnAnimQueueComplete(this.behaviourcomplete.pst);
		this.behaviourcomplete.pst.BehaviourComplete(GameTags.Creatures.WantsToForage, false);
	}

	// Token: 0x060002FA RID: 762 RVA: 0x0001803A File Offset: 0x0001623A
	private static void FindTarget(BeeForageStates.Instance smi)
	{
		if (BeeForageStates.FindOre(smi))
		{
			return;
		}
		BeeForageStates.FindMineableCell(smi);
	}

	// Token: 0x060002FB RID: 763 RVA: 0x0001804C File Offset: 0x0001624C
	private void HoldOre(BeeForageStates.Instance smi)
	{
		GameObject gameObject = smi.GetComponent<Storage>().FindFirst(smi.def.oreTag);
		if (!gameObject)
		{
			return;
		}
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		KAnim.Build.Symbol source_symbol = gameObject.GetComponent<KBatchedAnimController>().CurrentAnim.animFile.build.symbols[0];
		component.GetComponent<SymbolOverrideController>().AddSymbolOverride(smi.oreSymbolHash, source_symbol, 5);
		component.SetSymbolVisiblity(smi.oreSymbolHash, true);
		component.SetSymbolVisiblity(smi.oreLegSymbolHash, true);
		component.SetSymbolVisiblity(smi.noOreLegSymbolHash, false);
	}

	// Token: 0x060002FC RID: 764 RVA: 0x000180DB File Offset: 0x000162DB
	private void DropOre(BeeForageStates.Instance smi)
	{
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity(smi.oreSymbolHash, false);
		component.SetSymbolVisiblity(smi.oreLegSymbolHash, false);
		component.SetSymbolVisiblity(smi.noOreLegSymbolHash, true);
	}

	// Token: 0x060002FD RID: 765 RVA: 0x0001810C File Offset: 0x0001630C
	private static void PickupComplete(BeeForageStates.Instance smi)
	{
		if (!smi.forageTarget)
		{
			global::Debug.LogWarningFormat("PickupComplete forageTarget {0} is null", new object[]
			{
				smi.forageTarget
			});
			return;
		}
		BeeForageStates.UnreserveTarget(smi);
		int num = Grid.PosToCell(smi.forageTarget);
		if (smi.forageTarget_cell != num)
		{
			global::Debug.LogWarningFormat("PickupComplete forageTarget {0} moved {1} != {2}", new object[]
			{
				smi.forageTarget,
				num,
				smi.forageTarget_cell
			});
			smi.forageTarget = null;
			return;
		}
		if (smi.forageTarget.HasTag(GameTags.Stored))
		{
			global::Debug.LogWarningFormat("PickupComplete forageTarget {0} was stored by {1}", new object[]
			{
				smi.forageTarget,
				smi.forageTarget.storage
			});
			smi.forageTarget = null;
			return;
		}
		smi.forageTarget = EntitySplitter.Split(smi.forageTarget, 10f, null);
		smi.GetComponent<Storage>().Store(smi.forageTarget.gameObject, false, false, true, false);
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00018208 File Offset: 0x00016408
	private static void MineTarget(BeeForageStates.Instance smi)
	{
		Storage storage = smi.master.GetComponent<Storage>();
		HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(delegate(Sim.MassConsumedCallback mass_cb_info, object data)
		{
			if (mass_cb_info.mass > 0f)
			{
				storage.AddOre(ElementLoader.elements[(int)mass_cb_info.elemIdx].id, mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount, false, true);
			}
		}, null, "BeetaMine");
		SimMessages.ConsumeMass(smi.cellToMine, Grid.Element[smi.cellToMine].id, smi.def.amountToMine, 1, handle.index);
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0001827D File Offset: 0x0001647D
	private static void StoreOre(BeeForageStates.Instance smi)
	{
		smi.master.GetComponent<Storage>().Transfer(smi.targetHive.GetComponent<Storage>(), false, false);
		smi.forageTarget = null;
		smi.forageTarget_cell = Grid.InvalidCell;
		smi.targetHive = null;
	}

	// Token: 0x06000300 RID: 768 RVA: 0x000182B8 File Offset: 0x000164B8
	private static void DropAll(BeeForageStates.Instance smi)
	{
		smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x06000301 RID: 769 RVA: 0x000182E0 File Offset: 0x000164E0
	private static bool FindMineableCell(BeeForageStates.Instance smi)
	{
		smi.targetMiningCell = Grid.InvalidCell;
		MineableCellQuery mineableCellQuery = PathFinderQueries.mineableCellQuery.Reset(smi.def.oreTag, 20);
		smi.GetComponent<Navigator>().RunQuery(mineableCellQuery);
		if (mineableCellQuery.result_cells.Count > 0)
		{
			smi.targetMiningCell = mineableCellQuery.result_cells[UnityEngine.Random.Range(0, mineableCellQuery.result_cells.Count)];
			foreach (Direction d in MineableCellQuery.DIRECTION_CHECKS)
			{
				int cellInDirection = Grid.GetCellInDirection(smi.targetMiningCell, d);
				if (Grid.IsValidCell(cellInDirection) && Grid.IsSolidCell(cellInDirection) && Grid.Element[cellInDirection].tag == smi.def.oreTag)
				{
					smi.cellToMine = cellInDirection;
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06000302 RID: 770 RVA: 0x000183D8 File Offset: 0x000165D8
	private static bool FindOre(BeeForageStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		Vector3 position = smi.transform.GetPosition();
		Pickupable forageTarget = null;
		int num = 100;
		Extents extents = new Extents((int)position.x, (int)position.y, 15);
		ListPool<ScenePartitionerEntry, BeeForageStates>.PooledList pooledList = ListPool<ScenePartitionerEntry, BeeForageStates>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		Element element = ElementLoader.GetElement(smi.def.oreTag);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
			if (pickupable && pickupable.GetComponent<ElementChunk>() && pickupable.GetComponent<PrimaryElement>() && pickupable.GetComponent<PrimaryElement>().Element == element && !pickupable.KPrefabID.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				int navigationCost = component.GetNavigationCost(Grid.PosToCell(pickupable));
				if (navigationCost != -1 && navigationCost < num)
				{
					forageTarget = pickupable.GetComponent<Pickupable>();
					num = navigationCost;
				}
			}
		}
		pooledList.Recycle();
		smi.forageTarget = forageTarget;
		smi.forageTarget_cell = (smi.forageTarget ? Grid.PosToCell(smi.forageTarget) : Grid.InvalidCell);
		return smi.forageTarget != null;
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0001853C File Offset: 0x0001673C
	private static void ReserveTarget(BeeForageStates.Instance smi)
	{
		GameObject gameObject = smi.forageTarget ? smi.forageTarget.gameObject : null;
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0001858C File Offset: 0x0001678C
	private static void UnreserveTarget(BeeForageStates.Instance smi)
	{
		GameObject go = smi.forageTarget ? smi.forageTarget.gameObject : null;
		if (smi.forageTarget != null)
		{
			go.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x06000305 RID: 773 RVA: 0x000185CE File Offset: 0x000167CE
	private static int GetOreCell(BeeForageStates.Instance smi)
	{
		global::Debug.Assert(smi.forageTarget);
		global::Debug.Assert(smi.forageTarget_cell != Grid.InvalidCell);
		return smi.forageTarget_cell;
	}

	// Token: 0x0400020B RID: 523
	private const int MAX_NAVIGATE_DISTANCE = 100;

	// Token: 0x0400020C RID: 524
	private const string oreSymbol = "snapto_thing";

	// Token: 0x0400020D RID: 525
	private const string oreLegSymbol = "legBeeOre";

	// Token: 0x0400020E RID: 526
	private const string noOreLegSymbol = "legBeeNoOre";

	// Token: 0x0400020F RID: 527
	public BeeForageStates.CollectionBehaviourStates collect;

	// Token: 0x04000210 RID: 528
	public BeeForageStates.StorageBehaviourStates storage;

	// Token: 0x04000211 RID: 529
	public BeeForageStates.ExitStates behaviourcomplete;

	// Token: 0x02000E75 RID: 3701
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06006F8E RID: 28558 RVA: 0x002B97B3 File Offset: 0x002B79B3
		public Def(Tag tag, float amount_to_mine)
		{
			this.oreTag = tag;
			this.amountToMine = amount_to_mine;
		}

		// Token: 0x04005397 RID: 21399
		public Tag oreTag;

		// Token: 0x04005398 RID: 21400
		public float amountToMine;
	}

	// Token: 0x02000E76 RID: 3702
	public new class Instance : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.GameInstance
	{
		// Token: 0x06006F8F RID: 28559 RVA: 0x002B97CC File Offset: 0x002B79CC
		public Instance(Chore<BeeForageStates.Instance> chore, BeeForageStates.Def def) : base(chore, def)
		{
			this.oreSymbolHash = new KAnimHashedString("snapto_thing");
			this.oreLegSymbolHash = new KAnimHashedString("legBeeOre");
			this.noOreLegSymbolHash = new KAnimHashedString("legBeeNoOre");
			base.smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(base.smi.oreSymbolHash, false);
			base.smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(base.smi.oreLegSymbolHash, false);
			base.smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(base.smi.noOreLegSymbolHash, true);
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToForage);
		}

		// Token: 0x04005399 RID: 21401
		public int targetMiningCell = Grid.InvalidCell;

		// Token: 0x0400539A RID: 21402
		public int cellToMine = Grid.InvalidCell;

		// Token: 0x0400539B RID: 21403
		public Pickupable forageTarget;

		// Token: 0x0400539C RID: 21404
		public int forageTarget_cell = Grid.InvalidCell;

		// Token: 0x0400539D RID: 21405
		public KPrefabID targetHive;

		// Token: 0x0400539E RID: 21406
		public KAnimHashedString oreSymbolHash;

		// Token: 0x0400539F RID: 21407
		public KAnimHashedString oreLegSymbolHash;

		// Token: 0x040053A0 RID: 21408
		public KAnimHashedString noOreLegSymbolHash;

		// Token: 0x040053A1 RID: 21409
		public CellOffset hiveCellOffset = new CellOffset(1, 1);
	}

	// Token: 0x02000E77 RID: 3703
	public class ForageBehaviourStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x040053A2 RID: 21410
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State moveToTarget;

		// Token: 0x040053A3 RID: 21411
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State pickupTarget;
	}

	// Token: 0x02000E78 RID: 3704
	public class MiningBehaviourStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x040053A4 RID: 21412
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State moveToTarget;

		// Token: 0x040053A5 RID: 21413
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State mineTarget;
	}

	// Token: 0x02000E79 RID: 3705
	public class CollectionBehaviourStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x040053A6 RID: 21414
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State findTarget;

		// Token: 0x040053A7 RID: 21415
		public BeeForageStates.ForageBehaviourStates forage;

		// Token: 0x040053A8 RID: 21416
		public BeeForageStates.MiningBehaviourStates mine;
	}

	// Token: 0x02000E7A RID: 3706
	public class StorageBehaviourStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x040053A9 RID: 21417
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State moveToHive;

		// Token: 0x040053AA RID: 21418
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State storeMaterial;

		// Token: 0x040053AB RID: 21419
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State dropMaterial;
	}

	// Token: 0x02000E7B RID: 3707
	public class ExitStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x040053AC RID: 21420
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State pre;

		// Token: 0x040053AD RID: 21421
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State pst;
	}
}
