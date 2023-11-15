using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class SeedPlantingStates : GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>
{
	// Token: 0x060003E8 RID: 1000 RVA: 0x0001E3D8 File Offset: 0x0001C5D8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findSeed;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.PLANTINGSEED.NAME, CREATURES.STATUSITEMS.PLANTINGSEED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.UnreserveSeed)).Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.DropAll)).Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.RemoveMouthOverride));
		this.findSeed.Enter(delegate(SeedPlantingStates.Instance smi)
		{
			SeedPlantingStates.FindSeed(smi);
			if (smi.targetSeed == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			SeedPlantingStates.ReserveSeed(smi);
			smi.GoTo(this.moveToSeed);
		});
		this.moveToSeed.MoveTo(new Func<SeedPlantingStates.Instance, int>(SeedPlantingStates.GetSeedCell), this.findPlantLocation, this.behaviourcomplete, false);
		this.findPlantLocation.Enter(delegate(SeedPlantingStates.Instance smi)
		{
			if (!smi.targetSeed)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			SeedPlantingStates.FindDirtPlot(smi);
			if (smi.targetPlot != null || smi.targetDirtPlotCell != Grid.InvalidCell)
			{
				smi.GoTo(this.pickupSeed);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.pickupSeed.PlayAnim("gather").Enter(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.PickupComplete)).OnAnimQueueComplete(this.moveToPlantLocation);
		this.moveToPlantLocation.Enter(delegate(SeedPlantingStates.Instance smi)
		{
			if (smi.targetSeed == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			if (smi.targetPlot != null)
			{
				smi.GoTo(this.moveToPlot);
				return;
			}
			if (smi.targetDirtPlotCell != Grid.InvalidCell)
			{
				smi.GoTo(this.moveToDirt);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.moveToDirt.MoveTo((SeedPlantingStates.Instance smi) => smi.targetDirtPlotCell, this.planting, this.behaviourcomplete, false);
		this.moveToPlot.Enter(delegate(SeedPlantingStates.Instance smi)
		{
			if (smi.targetPlot == null || smi.targetSeed == null)
			{
				smi.GoTo(this.behaviourcomplete);
			}
		}).MoveTo(new Func<SeedPlantingStates.Instance, int>(SeedPlantingStates.GetPlantableCell), this.planting, this.behaviourcomplete, false);
		this.planting.Enter(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.RemoveMouthOverride)).PlayAnim("plant").Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.PlantComplete)).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToPlantSeed, false);
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0001E5C0 File Offset: 0x0001C7C0
	private static void AddMouthOverride(SeedPlantingStates.Instance smi)
	{
		SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
		KAnim.Build.Symbol symbol = smi.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(smi.def.prefix + "sq_mouth_cheeks");
		if (symbol != null)
		{
			component.AddSymbolOverride("sq_mouth", symbol, 1);
		}
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x0001E621 File Offset: 0x0001C821
	private static void RemoveMouthOverride(SeedPlantingStates.Instance smi)
	{
		smi.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride("sq_mouth", 1);
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0001E63C File Offset: 0x0001C83C
	private static void PickupComplete(SeedPlantingStates.Instance smi)
	{
		if (!smi.targetSeed)
		{
			global::Debug.LogWarningFormat("PickupComplete seed {0} is null", new object[]
			{
				smi.targetSeed
			});
			return;
		}
		SeedPlantingStates.UnreserveSeed(smi);
		int num = Grid.PosToCell(smi.targetSeed);
		if (smi.seed_cell != num)
		{
			global::Debug.LogWarningFormat("PickupComplete seed {0} moved {1} != {2}", new object[]
			{
				smi.targetSeed,
				num,
				smi.seed_cell
			});
			smi.targetSeed = null;
			return;
		}
		if (smi.targetSeed.HasTag(GameTags.Stored))
		{
			global::Debug.LogWarningFormat("PickupComplete seed {0} was stored by {1}", new object[]
			{
				smi.targetSeed,
				smi.targetSeed.storage
			});
			smi.targetSeed = null;
			return;
		}
		smi.targetSeed = EntitySplitter.Split(smi.targetSeed, 1f, null);
		smi.GetComponent<Storage>().Store(smi.targetSeed.gameObject, false, false, true, false);
		SeedPlantingStates.AddMouthOverride(smi);
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0001E73C File Offset: 0x0001C93C
	private static void PlantComplete(SeedPlantingStates.Instance smi)
	{
		PlantableSeed plantableSeed = smi.targetSeed ? smi.targetSeed.GetComponent<PlantableSeed>() : null;
		PlantablePlot plantablePlot;
		if (plantableSeed && SeedPlantingStates.CheckValidPlotCell(smi, plantableSeed, smi.targetDirtPlotCell, out plantablePlot))
		{
			if (plantablePlot)
			{
				if (plantablePlot.Occupant == null)
				{
					plantablePlot.ForceDeposit(smi.targetSeed.gameObject);
				}
			}
			else
			{
				plantableSeed.TryPlant(true);
			}
		}
		smi.targetSeed = null;
		smi.seed_cell = Grid.InvalidCell;
		smi.targetPlot = null;
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x0001E7C8 File Offset: 0x0001C9C8
	private static void DropAll(SeedPlantingStates.Instance smi)
	{
		smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x0001E7F0 File Offset: 0x0001C9F0
	private static int GetPlantableCell(SeedPlantingStates.Instance smi)
	{
		int num = Grid.PosToCell(smi.targetPlot);
		if (Grid.IsValidCell(num))
		{
			return Grid.CellAbove(num);
		}
		return num;
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0001E81C File Offset: 0x0001CA1C
	private static void FindDirtPlot(SeedPlantingStates.Instance smi)
	{
		smi.targetDirtPlotCell = Grid.InvalidCell;
		PlantableSeed component = smi.targetSeed.GetComponent<PlantableSeed>();
		PlantableCellQuery plantableCellQuery = PathFinderQueries.plantableCellQuery.Reset(component, 20);
		smi.GetComponent<Navigator>().RunQuery(plantableCellQuery);
		if (plantableCellQuery.result_cells.Count > 0)
		{
			smi.targetDirtPlotCell = plantableCellQuery.result_cells[UnityEngine.Random.Range(0, plantableCellQuery.result_cells.Count)];
		}
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0001E88C File Offset: 0x0001CA8C
	private static bool CheckValidPlotCell(SeedPlantingStates.Instance smi, PlantableSeed seed, int cell, out PlantablePlot plot)
	{
		plot = null;
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int num;
		if (seed.Direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			num = Grid.CellAbove(cell);
		}
		else
		{
			num = Grid.CellBelow(cell);
		}
		if (!Grid.IsValidCell(num))
		{
			return false;
		}
		if (!Grid.Solid[num])
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[num, 1];
		if (gameObject)
		{
			plot = gameObject.GetComponent<PlantablePlot>();
			return plot != null;
		}
		return seed.TestSuitableGround(cell);
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0001E905 File Offset: 0x0001CB05
	private static int GetSeedCell(SeedPlantingStates.Instance smi)
	{
		global::Debug.Assert(smi.targetSeed);
		global::Debug.Assert(smi.seed_cell != Grid.InvalidCell);
		return smi.seed_cell;
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0001E934 File Offset: 0x0001CB34
	private static void FindSeed(SeedPlantingStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		Pickupable targetSeed = null;
		int num = 100;
		foreach (object obj in Components.PlantableSeeds)
		{
			PlantableSeed plantableSeed = (PlantableSeed)obj;
			if ((plantableSeed.HasTag(GameTags.Seed) || plantableSeed.HasTag(GameTags.CropSeed)) && !plantableSeed.HasTag(GameTags.Creatures.ReservedByCreature) && Vector2.Distance(smi.transform.position, plantableSeed.transform.position) <= 25f)
			{
				int navigationCost = component.GetNavigationCost(Grid.PosToCell(plantableSeed));
				if (navigationCost != -1 && navigationCost < num)
				{
					targetSeed = plantableSeed.GetComponent<Pickupable>();
					num = navigationCost;
				}
			}
		}
		smi.targetSeed = targetSeed;
		smi.seed_cell = (smi.targetSeed ? Grid.PosToCell(smi.targetSeed) : Grid.InvalidCell);
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0001EA44 File Offset: 0x0001CC44
	private static void ReserveSeed(SeedPlantingStates.Instance smi)
	{
		GameObject gameObject = smi.targetSeed ? smi.targetSeed.gameObject : null;
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0001EA94 File Offset: 0x0001CC94
	private static void UnreserveSeed(SeedPlantingStates.Instance smi)
	{
		GameObject go = smi.targetSeed ? smi.targetSeed.gameObject : null;
		if (smi.targetSeed != null)
		{
			go.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x04000298 RID: 664
	private const int MAX_NAVIGATE_DISTANCE = 100;

	// Token: 0x04000299 RID: 665
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State findSeed;

	// Token: 0x0400029A RID: 666
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToSeed;

	// Token: 0x0400029B RID: 667
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State pickupSeed;

	// Token: 0x0400029C RID: 668
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State findPlantLocation;

	// Token: 0x0400029D RID: 669
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToPlantLocation;

	// Token: 0x0400029E RID: 670
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToPlot;

	// Token: 0x0400029F RID: 671
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToDirt;

	// Token: 0x040002A0 RID: 672
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State planting;

	// Token: 0x040002A1 RID: 673
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State behaviourcomplete;

	// Token: 0x02000F1A RID: 3866
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600710A RID: 28938 RVA: 0x002BC0F6 File Offset: 0x002BA2F6
		public Def(string prefix)
		{
			this.prefix = prefix;
		}

		// Token: 0x040054F0 RID: 21744
		public string prefix;
	}

	// Token: 0x02000F1B RID: 3867
	public new class Instance : GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.GameInstance
	{
		// Token: 0x0600710B RID: 28939 RVA: 0x002BC108 File Offset: 0x002BA308
		public Instance(Chore<SeedPlantingStates.Instance> chore, SeedPlantingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToPlantSeed);
		}

		// Token: 0x040054F1 RID: 21745
		public PlantablePlot targetPlot;

		// Token: 0x040054F2 RID: 21746
		public int targetDirtPlotCell = Grid.InvalidCell;

		// Token: 0x040054F3 RID: 21747
		public Element plantElement = ElementLoader.FindElementByHash(SimHashes.Dirt);

		// Token: 0x040054F4 RID: 21748
		public Pickupable targetSeed;

		// Token: 0x040054F5 RID: 21749
		public int seed_cell = Grid.InvalidCell;
	}
}
