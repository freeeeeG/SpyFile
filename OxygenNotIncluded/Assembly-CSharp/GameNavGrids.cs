using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007CF RID: 1999
public class GameNavGrids
{
	// Token: 0x060037CF RID: 14287 RVA: 0x0012F2FC File Offset: 0x0012D4FC
	public GameNavGrids(Pathfinding pathfinding)
	{
		this.CreateDuplicantNavigation(pathfinding);
		this.WalkerGrid1x1 = this.CreateWalkerNavigation(pathfinding, "WalkerNavGrid1x1", new CellOffset[]
		{
			new CellOffset(0, 0)
		});
		this.WalkerBabyGrid1x1 = this.CreateWalkerBabyNavigation(pathfinding, "WalkerBabyNavGrid", new CellOffset[]
		{
			new CellOffset(0, 0)
		});
		this.WalkerGrid1x2 = this.CreateWalkerNavigation(pathfinding, "WalkerNavGrid1x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		});
		this.CreateDreckoNavigation(pathfinding);
		this.CreateDreckoBabyNavigation(pathfinding);
		this.CreateFloaterNavigation(pathfinding);
		this.FlyerGrid1x1 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid1x1", new CellOffset[]
		{
			new CellOffset(0, 0)
		});
		this.FlyerGrid1x2 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid1x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		});
		this.FlyerGrid2x2 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid2x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1),
			new CellOffset(1, 0),
			new CellOffset(1, 1)
		});
		this.CreateSwimmerNavigation(pathfinding);
		this.CreateDiggerNavigation(pathfinding);
		this.CreateSquirrelNavigation(pathfinding);
	}

	// Token: 0x060037D0 RID: 14288 RVA: 0x0012F46C File Offset: 0x0012D66C
	private void CreateDuplicantNavigation(Pathfinding pathfinding)
	{
		NavOffset[] invalid_nav_offsets = new NavOffset[]
		{
			new NavOffset(NavType.Floor, 1, 0),
			new NavOffset(NavType.Ladder, 1, 0),
			new NavOffset(NavType.Pole, 1, 0)
		};
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		};
		NavGrid.Transition[] setA = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1),
				new CellOffset(1, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 1),
				new NavOffset(NavType.Ladder, 1, 1),
				new NavOffset(NavType.Pole, 1, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, -1),
				new NavOffset(NavType.Ladder, 1, -1),
				new NavOffset(NavType.Pole, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Teleport, 0, 0, NavAxis.NA, false, false, false, 14, "fall_pre", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Teleport, NavType.Floor, 0, 0, NavAxis.NA, false, false, false, 1, "fall_pst", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 0, 1),
				new NavOffset(NavType.Floor, 0, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, -1),
				new NavOffset(NavType.Ladder, 0, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 1, 0, NavAxis.NA, false, false, true, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 0, 1, NavAxis.NA, true, true, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 0, -1, NavAxis.NA, true, true, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 2, 0, NavAxis.NA, false, false, true, 25, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, 1, NavAxis.NA, false, false, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, 1, NavAxis.NA, false, false, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 2, 0, NavAxis.NA, false, false, true, 50, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 0, 1),
				new NavOffset(NavType.Floor, 0, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, -1),
				new NavOffset(NavType.Pole, 0, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 1, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 2, 0, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 1, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 2, 0, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 0, 1, NavAxis.NA, true, true, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 0, -1, NavAxis.NA, true, true, false, 6, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 2, 0, NavAxis.NA, false, false, true, 50, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Tube, 0, 2, NavAxis.NA, false, false, false, 40, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -2)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, 2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 0, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -2)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, 2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 0, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -2)
			}, false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 0, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 0, 1, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 0, -1, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 1, NavAxis.Y, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 0, 1)
			}, new NavOffset[0], false, 2.2f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 1, NavAxis.X, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 1, 0)
			}, new NavOffset[0], false, 2.2f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, -1, NavAxis.Y, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 0, -1)
			}, new NavOffset[0], false, 2.2f),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, -1, NavAxis.X, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 1, 0)
			}, new NavOffset[0], false, 2.2f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, true, false, false, 15, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, true, false, false, 15, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, false, false, false, 25, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 0),
				new NavOffset(NavType.Hover, 0, 1)
			}, new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, false, false, false, 25, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 0),
				new NavOffset(NavType.Hover, 0, -1)
			}, new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Hover, 1, 0, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Hover, 0, 1, NavAxis.NA, false, false, false, 20, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f)
		};
		NavGrid.Transition[] setB = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 30, "climb_down_2_-1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[]
			{
				new CellOffset(1, 1)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, -1),
				new NavOffset(NavType.Ladder, 1, -1),
				new NavOffset(NavType.Pole, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 30, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[]
			{
				new CellOffset(1, 2)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 1),
				new NavOffset(NavType.Ladder, 1, 1),
				new NavOffset(NavType.Pole, 1, 1)
			}, false, 1f)
		};
		NavGrid.Transition[] transitions = this.MirrorTransitions(this.CombineTransitions(setA, setB));
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_default"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ladder,
				idleAnim = "ladder_idle"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Pole,
				idleAnim = "pole_idle"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Tube,
				idleAnim = "tube_idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Hover,
				idleAnim = "hover_hover_1_0_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Teleport,
				idleAnim = "idle_default"
			}
		};
		this.DuplicantGrid = new NavGrid("MinionNavGrid", transitions, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(true),
			new GameNavGrids.LadderValidator(),
			new GameNavGrids.PoleValidator(),
			new GameNavGrids.TubeValidator(),
			new GameNavGrids.TeleporterValidator(),
			new GameNavGrids.FlyingValidator(true, true, true)
		}, 2, 3, 32);
		this.DuplicantGrid.updateEveryFrame = true;
		pathfinding.AddNavGrid(this.DuplicantGrid);
		NavGrid.Transition[] setB2 = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 30, "climb_down_2_-1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[]
			{
				new CellOffset(1, 1)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, -1),
				new NavOffset(NavType.Ladder, 1, -1),
				new NavOffset(NavType.Pole, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 30, "climb_up_2_1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[]
			{
				new CellOffset(1, 2)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 1),
				new NavOffset(NavType.Ladder, 1, 1),
				new NavOffset(NavType.Pole, 1, 1)
			}, false, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(this.CombineTransitions(setA, setB2));
		this.RobotGrid = new NavGrid("RobotNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(true),
			new GameNavGrids.LadderValidator()
		}, 2, 3, 22);
		this.RobotGrid.updateEveryFrame = true;
		pathfinding.AddNavGrid(this.RobotGrid);
	}

	// Token: 0x060037D1 RID: 14289 RVA: 0x001319F0 File Offset: 0x0012FBF0
	private NavGrid CreateWalkerNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false)
		}, 2, 3, array.Length);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x060037D2 RID: 14290 RVA: 0x00131C24 File Offset: 0x0012FE24
	private NavGrid CreateWalkerBabyNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false)
		}, 2, 3, array.Length);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x060037D3 RID: 14291 RVA: 0x00131CD0 File Offset: 0x0012FED0
	private void CreateDreckoNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 4, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 1, -2)
			}, true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 2, NavAxis.NA, false, false, true, 5, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 2)
			}, new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 0)
			}, true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 4, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 0),
				new NavOffset(NavType.Floor, 2, 2)
			}, true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 1, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 0)
			}, new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 1)
			}, false, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, -1, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, 0, 0)
			}, new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, -1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, -1, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 0, 0)
			}, new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 0, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, 0)
			}, new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -2, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 1, -1)
			}, new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 1, -2)
			}, true, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -2, -1, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(0, -1),
				new CellOffset(-1, -1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, -1, -1)
			}, new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, -2, -1)
			}, true, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 2, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(-1, 0),
				new CellOffset(-1, 1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, -1, 1)
			}, new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, -1, 2)
			}, true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 2, 1)
			}, true, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			}
		};
		this.DreckoGrid = new NavGrid("DreckoNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 16);
		pathfinding.AddNavGrid(this.DreckoGrid);
	}

	// Token: 0x060037D4 RID: 14292 RVA: 0x00132620 File Offset: 0x00130820
	private void CreateDreckoBabyNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			}
		};
		this.DreckoBabyGrid = new NavGrid("DreckoBabyNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 16);
		pathfinding.AddNavGrid(this.DreckoBabyGrid);
	}

	// Token: 0x060037D5 RID: 14293 RVA: 0x00132AA4 File Offset: 0x00130CA4
	private void CreateFloaterNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, -1)
			}, false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 0)
			}, true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, -2)
			}, true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 0, 0)
			}, false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 0, -2)
			}, false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, 1, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 2, 0)
			}, true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, 0, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 2, -1)
			}, true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, -1, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(1, -2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 2, -2)
			}, true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 2, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 1)
			}, true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -2, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, -3)
			}, true, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Hover,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "swim_idle_loop"
			}
		};
		this.FloaterGrid = new NavGrid("FloaterNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.HoverValidator(),
			new GameNavGrids.SwimValidator()
		}, 2, 2, 22);
		pathfinding.AddNavGrid(this.FloaterGrid);
	}

	// Token: 0x060037D6 RID: 14294 RVA: 0x0013309C File Offset: 0x0013129C
	private NavGrid CreateFlyerNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 2, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, true, true, true, 2, "hover_hover_1_0", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, true, true, true, 2, "hover_hover_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 3, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, true, true, true, 3, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Hover,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FlyingValidator(false, false, false),
			new GameNavGrids.SwimValidator()
		}, 2, 2, 16);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x060037D7 RID: 14295 RVA: 0x0013343C File Offset: 0x0013163C
	private void CreateSwimmerNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "idle_loop"
			}
		};
		this.SwimmerGrid = new NavGrid("SwimmerNavGrid", array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.SwimValidator()
		}, 1, 1, array.Length);
		pathfinding.AddNavGrid(this.SwimmerGrid);
	}

	// Token: 0x060037D8 RID: 14296 RVA: 0x00133620 File Offset: 0x00131820
	private void CreateDiggerNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, 0, NavAxis.NA, false, false, true, 1, "idle1", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, 1, NavAxis.NA, false, false, true, 1, "idle2", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 0, 1, NavAxis.NA, false, false, true, 1, "idle3", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, -1, NavAxis.NA, false, false, true, 1, "idle4", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Solid, 0, -1, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 1, "drill_out", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.Solid, 0, 1, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.Ceiling, 0, -1, NavAxis.NA, false, false, true, 1, "drill_out_ceiling", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.LeftWall, 1, 0, NavAxis.NA, false, false, true, 1, "drill_out_left_wall", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Solid, -1, 0, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Solid, NavType.RightWall, -1, 0, NavAxis.NA, false, false, true, 1, "drill_out_right_wall", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Solid, 1, 0, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Solid,
				idleAnim = "idle1"
			}
		};
		this.DiggerGrid = new NavGrid("DiggerNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.SolidValidator(),
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 22);
		pathfinding.AddNavGrid(this.DiggerGrid);
	}

	// Token: 0x060037D9 RID: 14297 RVA: 0x00133DA0 File Offset: 0x00131FA0
	private void CreateSquirrelNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			}
		};
		this.SquirrelGrid = new NavGrid("SquirrelNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 20);
		pathfinding.AddNavGrid(this.SquirrelGrid);
	}

	// Token: 0x060037DA RID: 14298 RVA: 0x001343B0 File Offset: 0x001325B0
	private CellOffset[] MirrorOffsets(CellOffset[] offsets)
	{
		List<CellOffset> list = new List<CellOffset>();
		foreach (CellOffset cellOffset in offsets)
		{
			cellOffset.x = -cellOffset.x;
			list.Add(cellOffset);
		}
		return list.ToArray();
	}

	// Token: 0x060037DB RID: 14299 RVA: 0x001343F8 File Offset: 0x001325F8
	private NavOffset[] MirrorNavOffsets(NavOffset[] offsets)
	{
		List<NavOffset> list = new List<NavOffset>();
		foreach (NavOffset navOffset in offsets)
		{
			navOffset.navType = NavGrid.MirrorNavType(navOffset.navType);
			navOffset.offset.x = -navOffset.offset.x;
			list.Add(navOffset);
		}
		return list.ToArray();
	}

	// Token: 0x060037DC RID: 14300 RVA: 0x0013445C File Offset: 0x0013265C
	private NavGrid.Transition[] MirrorTransitions(NavGrid.Transition[] transitions)
	{
		List<NavGrid.Transition> list = new List<NavGrid.Transition>();
		foreach (NavGrid.Transition transition in transitions)
		{
			list.Add(transition);
			if (transition.x != 0 || transition.start == NavType.RightWall || transition.end == NavType.RightWall || transition.start == NavType.LeftWall || transition.end == NavType.LeftWall)
			{
				NavGrid.Transition transition2 = transition;
				transition2.x = -transition2.x;
				transition2.voidOffsets = this.MirrorOffsets(transition.voidOffsets);
				transition2.solidOffsets = this.MirrorOffsets(transition.solidOffsets);
				transition2.validNavOffsets = this.MirrorNavOffsets(transition.validNavOffsets);
				transition2.invalidNavOffsets = this.MirrorNavOffsets(transition.invalidNavOffsets);
				transition2.start = NavGrid.MirrorNavType(transition2.start);
				transition2.end = NavGrid.MirrorNavType(transition2.end);
				list.Add(transition2);
			}
		}
		list.Sort((NavGrid.Transition x, NavGrid.Transition y) => x.cost.CompareTo(y.cost));
		return list.ToArray();
	}

	// Token: 0x060037DD RID: 14301 RVA: 0x0013457C File Offset: 0x0013277C
	private NavGrid.Transition[] CombineTransitions(NavGrid.Transition[] setA, NavGrid.Transition[] setB)
	{
		NavGrid.Transition[] array = new NavGrid.Transition[setA.Length + setB.Length];
		Array.Copy(setA, array, setA.Length);
		Array.Copy(setB, 0, array, setA.Length, setB.Length);
		Array.Sort<NavGrid.Transition>(array, (NavGrid.Transition x, NavGrid.Transition y) => x.cost.CompareTo(y.cost));
		return array;
	}

	// Token: 0x04002459 RID: 9305
	public NavGrid DuplicantGrid;

	// Token: 0x0400245A RID: 9306
	public NavGrid WalkerGrid1x1;

	// Token: 0x0400245B RID: 9307
	public NavGrid WalkerBabyGrid1x1;

	// Token: 0x0400245C RID: 9308
	public NavGrid WalkerGrid1x2;

	// Token: 0x0400245D RID: 9309
	public NavGrid DreckoGrid;

	// Token: 0x0400245E RID: 9310
	public NavGrid DreckoBabyGrid;

	// Token: 0x0400245F RID: 9311
	public NavGrid FloaterGrid;

	// Token: 0x04002460 RID: 9312
	public NavGrid FlyerGrid1x2;

	// Token: 0x04002461 RID: 9313
	public NavGrid FlyerGrid1x1;

	// Token: 0x04002462 RID: 9314
	public NavGrid FlyerGrid2x2;

	// Token: 0x04002463 RID: 9315
	public NavGrid SwimmerGrid;

	// Token: 0x04002464 RID: 9316
	public NavGrid DiggerGrid;

	// Token: 0x04002465 RID: 9317
	public NavGrid SquirrelGrid;

	// Token: 0x04002466 RID: 9318
	public NavGrid RobotGrid;

	// Token: 0x02001559 RID: 5465
	public class SwimValidator : NavTableValidator
	{
		// Token: 0x06008773 RID: 34675 RVA: 0x0030B26C File Offset: 0x0030946C
		public SwimValidator()
		{
			World instance = World.Instance;
			instance.OnLiquidChanged = (Action<int>)Delegate.Combine(instance.OnLiquidChanged, new Action<int>(this.OnLiquidChanged));
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[9], new Action<int, object>(this.OnFoundationTileChanged));
		}

		// Token: 0x06008774 RID: 34676 RVA: 0x0030B2C8 File Offset: 0x003094C8
		private void OnFoundationTileChanged(int cell, object unused)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x06008775 RID: 34677 RVA: 0x0030B2E0 File Offset: 0x003094E0
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = Grid.IsSubstantialLiquid(cell, 0.35f);
			if (!flag)
			{
				flag = Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.35f);
			}
			bool is_valid = Grid.IsWorldValidCell(cell) && flag && base.IsClear(cell, bounding_offsets, false);
			nav_table.SetValid(cell, NavType.Swim, is_valid);
		}

		// Token: 0x06008776 RID: 34678 RVA: 0x0030B32D File Offset: 0x0030952D
		private void OnLiquidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}
	}

	// Token: 0x0200155A RID: 5466
	public class FloorValidator : NavTableValidator
	{
		// Token: 0x06008777 RID: 34679 RVA: 0x0030B344 File Offset: 0x00309544
		public FloorValidator(bool is_dupe)
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			Components.Ladders.Register(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
			this.isDupe = is_dupe;
		}

		// Token: 0x06008778 RID: 34680 RVA: 0x0030B3A8 File Offset: 0x003095A8
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = GameNavGrids.FloorValidator.IsWalkableCell(cell, Grid.CellBelow(cell), this.isDupe);
			nav_table.SetValid(cell, NavType.Floor, flag && base.IsClear(cell, bounding_offsets, this.isDupe));
		}

		// Token: 0x06008779 RID: 34681 RVA: 0x0030B3E4 File Offset: 0x003095E4
		public static bool IsWalkableCell(int cell, int anchor_cell, bool is_dupe)
		{
			if (!Grid.IsWorldValidCell(cell))
			{
				return false;
			}
			if (!Grid.IsWorldValidCell(anchor_cell))
			{
				return false;
			}
			if (!NavTableValidator.IsCellPassable(cell, is_dupe))
			{
				return false;
			}
			if (Grid.FakeFloor[anchor_cell])
			{
				return true;
			}
			if (Grid.Solid[anchor_cell])
			{
				return !Grid.DupePassable[anchor_cell];
			}
			return is_dupe && (Grid.NavValidatorMasks[cell] & (Grid.NavValidatorFlags.Ladder | Grid.NavValidatorFlags.Pole)) == (Grid.NavValidatorFlags)0 && (Grid.NavValidatorMasks[anchor_cell] & (Grid.NavValidatorFlags.Ladder | Grid.NavValidatorFlags.Pole)) > (Grid.NavValidatorFlags)0;
		}

		// Token: 0x0600877A RID: 34682 RVA: 0x0030B45C File Offset: 0x0030965C
		private void OnAddLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x0600877B RID: 34683 RVA: 0x0030B484 File Offset: 0x00309684
		private void OnRemoveLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x0600877C RID: 34684 RVA: 0x0030B4AC File Offset: 0x003096AC
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x0600877D RID: 34685 RVA: 0x0030B4C4 File Offset: 0x003096C4
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			Components.Ladders.Unregister(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
		}

		// Token: 0x04006804 RID: 26628
		private bool isDupe;
	}

	// Token: 0x0200155B RID: 5467
	public class WallValidator : NavTableValidator
	{
		// Token: 0x0600877E RID: 34686 RVA: 0x0030B519 File Offset: 0x00309719
		public WallValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}

		// Token: 0x0600877F RID: 34687 RVA: 0x0030B548 File Offset: 0x00309748
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = GameNavGrids.WallValidator.IsWalkableCell(cell, Grid.CellRight(cell));
			bool flag2 = GameNavGrids.WallValidator.IsWalkableCell(cell, Grid.CellLeft(cell));
			nav_table.SetValid(cell, NavType.RightWall, flag && base.IsClear(cell, bounding_offsets, false));
			nav_table.SetValid(cell, NavType.LeftWall, flag2 && base.IsClear(cell, bounding_offsets, false));
		}

		// Token: 0x06008780 RID: 34688 RVA: 0x0030B59D File Offset: 0x0030979D
		private static bool IsWalkableCell(int cell, int anchor_cell)
		{
			if (Grid.IsWorldValidCell(cell) && Grid.IsWorldValidCell(anchor_cell))
			{
				if (!NavTableValidator.IsCellPassable(cell, false))
				{
					return false;
				}
				if (Grid.Solid[anchor_cell])
				{
					return true;
				}
				if (Grid.CritterImpassable[anchor_cell])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008781 RID: 34689 RVA: 0x0030B5D9 File Offset: 0x003097D9
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x06008782 RID: 34690 RVA: 0x0030B5EF File Offset: 0x003097EF
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}
	}

	// Token: 0x0200155C RID: 5468
	public class CeilingValidator : NavTableValidator
	{
		// Token: 0x06008783 RID: 34691 RVA: 0x0030B617 File Offset: 0x00309817
		public CeilingValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}

		// Token: 0x06008784 RID: 34692 RVA: 0x0030B648 File Offset: 0x00309848
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = GameNavGrids.CeilingValidator.IsWalkableCell(cell, Grid.CellAbove(cell));
			nav_table.SetValid(cell, NavType.Ceiling, flag && base.IsClear(cell, bounding_offsets, false));
		}

		// Token: 0x06008785 RID: 34693 RVA: 0x0030B67C File Offset: 0x0030987C
		private static bool IsWalkableCell(int cell, int anchor_cell)
		{
			if (Grid.IsWorldValidCell(cell) && Grid.IsWorldValidCell(anchor_cell))
			{
				if (!NavTableValidator.IsCellPassable(cell, false))
				{
					return false;
				}
				if (Grid.Solid[anchor_cell])
				{
					return true;
				}
				if (Grid.HasDoor[cell] && !Grid.FakeFloor[cell])
				{
					return false;
				}
				if (Grid.FakeFloor[anchor_cell])
				{
					return true;
				}
				if (Grid.HasDoor[anchor_cell])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008786 RID: 34694 RVA: 0x0030B6EE File Offset: 0x003098EE
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x06008787 RID: 34695 RVA: 0x0030B704 File Offset: 0x00309904
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}
	}

	// Token: 0x0200155D RID: 5469
	public class LadderValidator : NavTableValidator
	{
		// Token: 0x06008788 RID: 34696 RVA: 0x0030B72C File Offset: 0x0030992C
		public LadderValidator()
		{
			Components.Ladders.Register(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
		}

		// Token: 0x06008789 RID: 34697 RVA: 0x0030B758 File Offset: 0x00309958
		private void OnAddLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x0600878A RID: 34698 RVA: 0x0030B780 File Offset: 0x00309980
		private void OnRemoveLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x0600878B RID: 34699 RVA: 0x0030B7A8 File Offset: 0x003099A8
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			nav_table.SetValid(cell, NavType.Ladder, base.IsClear(cell, bounding_offsets, true) && Grid.HasLadder[cell]);
		}

		// Token: 0x0600878C RID: 34700 RVA: 0x0030B7CB File Offset: 0x003099CB
		public override void Clear()
		{
			Components.Ladders.Unregister(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
		}
	}

	// Token: 0x0200155E RID: 5470
	public class PoleValidator : GameNavGrids.LadderValidator
	{
		// Token: 0x0600878D RID: 34701 RVA: 0x0030B7EF File Offset: 0x003099EF
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			nav_table.SetValid(cell, NavType.Pole, base.IsClear(cell, bounding_offsets, true) && Grid.HasPole[cell]);
		}
	}

	// Token: 0x0200155F RID: 5471
	public class TubeValidator : NavTableValidator
	{
		// Token: 0x0600878F RID: 34703 RVA: 0x0030B81A File Offset: 0x00309A1A
		public TubeValidator()
		{
			Components.ITravelTubePieces.Register(new Action<ITravelTubePiece>(this.OnAddLadder), new Action<ITravelTubePiece>(this.OnRemoveLadder));
		}

		// Token: 0x06008790 RID: 34704 RVA: 0x0030B844 File Offset: 0x00309A44
		private void OnAddLadder(ITravelTubePiece tube)
		{
			int obj = Grid.PosToCell(tube.Position);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x06008791 RID: 34705 RVA: 0x0030B874 File Offset: 0x00309A74
		private void OnRemoveLadder(ITravelTubePiece tube)
		{
			int obj = Grid.PosToCell(tube.Position);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x06008792 RID: 34706 RVA: 0x0030B8A1 File Offset: 0x00309AA1
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			nav_table.SetValid(cell, NavType.Tube, Grid.HasTube[cell]);
		}

		// Token: 0x06008793 RID: 34707 RVA: 0x0030B8B6 File Offset: 0x00309AB6
		public override void Clear()
		{
			Components.ITravelTubePieces.Unregister(new Action<ITravelTubePiece>(this.OnAddLadder), new Action<ITravelTubePiece>(this.OnRemoveLadder));
		}
	}

	// Token: 0x02001560 RID: 5472
	public class TeleporterValidator : NavTableValidator
	{
		// Token: 0x06008794 RID: 34708 RVA: 0x0030B8DA File Offset: 0x00309ADA
		public TeleporterValidator()
		{
			Components.NavTeleporters.Register(new Action<NavTeleporter>(this.OnAddTeleporter), new Action<NavTeleporter>(this.OnRemoveTeleporter));
		}

		// Token: 0x06008795 RID: 34709 RVA: 0x0030B904 File Offset: 0x00309B04
		private void OnAddTeleporter(NavTeleporter teleporter)
		{
			int obj = Grid.PosToCell(teleporter);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x06008796 RID: 34710 RVA: 0x0030B92C File Offset: 0x00309B2C
		private void OnRemoveTeleporter(NavTeleporter teleporter)
		{
			int obj = Grid.PosToCell(teleporter);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x06008797 RID: 34711 RVA: 0x0030B954 File Offset: 0x00309B54
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool is_valid = Grid.IsWorldValidCell(cell) && Grid.HasNavTeleporter[cell];
			nav_table.SetValid(cell, NavType.Teleport, is_valid);
		}

		// Token: 0x06008798 RID: 34712 RVA: 0x0030B982 File Offset: 0x00309B82
		public override void Clear()
		{
			Components.NavTeleporters.Unregister(new Action<NavTeleporter>(this.OnAddTeleporter), new Action<NavTeleporter>(this.OnRemoveTeleporter));
		}
	}

	// Token: 0x02001561 RID: 5473
	public class FlyingValidator : NavTableValidator
	{
		// Token: 0x06008799 RID: 34713 RVA: 0x0030B9A8 File Offset: 0x00309BA8
		public FlyingValidator(bool exclude_floor = false, bool exclude_jet_suit_blockers = false, bool allow_door_traversal = false)
		{
			this.exclude_floor = exclude_floor;
			this.exclude_jet_suit_blockers = exclude_jet_suit_blockers;
			this.allow_door_traversal = allow_door_traversal;
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Combine(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingChange));
		}

		// Token: 0x0600879A RID: 34714 RVA: 0x0030BA40 File Offset: 0x00309C40
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = false;
			if (Grid.IsWorldValidCell(Grid.CellAbove(cell)))
			{
				flag = (!Grid.IsSubstantialLiquid(cell, 0.35f) && base.IsClear(cell, bounding_offsets, this.allow_door_traversal));
				if (flag && this.exclude_floor)
				{
					int cell2 = Grid.CellBelow(cell);
					if (Grid.IsWorldValidCell(cell2))
					{
						flag = base.IsClear(cell2, bounding_offsets, this.allow_door_traversal);
					}
				}
				if (flag && this.exclude_jet_suit_blockers)
				{
					GameObject gameObject = Grid.Objects[cell, 1];
					flag = (gameObject == null || !gameObject.HasTag(GameTags.JetSuitBlocker));
				}
			}
			nav_table.SetValid(cell, NavType.Hover, flag);
		}

		// Token: 0x0600879B RID: 34715 RVA: 0x0030BAE0 File Offset: 0x00309CE0
		private void OnBuildingChange(int cell, object data)
		{
			this.MarkCellDirty(cell);
		}

		// Token: 0x0600879C RID: 34716 RVA: 0x0030BAE9 File Offset: 0x00309CE9
		private void MarkCellDirty(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x0600879D RID: 34717 RVA: 0x0030BB00 File Offset: 0x00309D00
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Remove(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
			GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingChange));
		}

		// Token: 0x04006805 RID: 26629
		private bool exclude_floor;

		// Token: 0x04006806 RID: 26630
		private bool exclude_jet_suit_blockers;

		// Token: 0x04006807 RID: 26631
		private bool allow_door_traversal;

		// Token: 0x04006808 RID: 26632
		private HandleVector<int>.Handle buildingParititonerEntry;
	}

	// Token: 0x02001562 RID: 5474
	public class HoverValidator : NavTableValidator
	{
		// Token: 0x0600879E RID: 34718 RVA: 0x0030BB7C File Offset: 0x00309D7C
		public HoverValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Combine(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
		}

		// Token: 0x0600879F RID: 34719 RVA: 0x0030BBDC File Offset: 0x00309DDC
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			int num = Grid.CellBelow(cell);
			if (Grid.IsWorldValidCell(num))
			{
				bool flag = Grid.Solid[num] || Grid.FakeFloor[num] || Grid.IsSubstantialLiquid(num, 0.35f);
				nav_table.SetValid(cell, NavType.Hover, !Grid.IsSubstantialLiquid(cell, 0.35f) && flag && base.IsClear(cell, bounding_offsets, false));
			}
		}

		// Token: 0x060087A0 RID: 34720 RVA: 0x0030BC47 File Offset: 0x00309E47
		private void MarkCellDirty(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060087A1 RID: 34721 RVA: 0x0030BC60 File Offset: 0x00309E60
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Remove(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
		}
	}

	// Token: 0x02001563 RID: 5475
	public class SolidValidator : NavTableValidator
	{
		// Token: 0x060087A2 RID: 34722 RVA: 0x0030BCB9 File Offset: 0x00309EB9
		public SolidValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}

		// Token: 0x060087A3 RID: 34723 RVA: 0x0030BCE8 File Offset: 0x00309EE8
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool is_valid = GameNavGrids.SolidValidator.IsDiggable(cell, Grid.CellBelow(cell));
			nav_table.SetValid(cell, NavType.Solid, is_valid);
		}

		// Token: 0x060087A4 RID: 34724 RVA: 0x0030BD0C File Offset: 0x00309F0C
		public static bool IsDiggable(int cell, int anchor_cell)
		{
			if (Grid.IsWorldValidCell(cell) && Grid.Solid[cell])
			{
				if (!Grid.HasDoor[cell] && !Grid.Foundation[cell])
				{
					ushort index = Grid.ElementIdx[cell];
					Element element = ElementLoader.elements[(int)index];
					return Grid.Element[cell].hardness < 150 && !element.HasTag(GameTags.RefinedMetal);
				}
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					return Grid.Element[cell].hardness < 150 && !component.Element.HasTag(GameTags.RefinedMetal);
				}
			}
			return false;
		}

		// Token: 0x060087A5 RID: 34725 RVA: 0x0030BDD1 File Offset: 0x00309FD1
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060087A6 RID: 34726 RVA: 0x0030BDE7 File Offset: 0x00309FE7
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}
	}
}
