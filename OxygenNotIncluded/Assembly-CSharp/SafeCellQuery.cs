using System;

// Token: 0x0200040C RID: 1036
public class SafeCellQuery : PathFinderQuery
{
	// Token: 0x060015D1 RID: 5585 RVA: 0x00072FA1 File Offset: 0x000711A1
	public SafeCellQuery Reset(MinionBrain brain, bool avoid_light)
	{
		this.brain = brain;
		this.targetCell = PathFinder.InvalidCell;
		this.targetCost = int.MaxValue;
		this.targetCellFlags = (SafeCellQuery.SafeFlags)0;
		this.avoid_light = avoid_light;
		return this;
	}

	// Token: 0x060015D2 RID: 5586 RVA: 0x00072FD0 File Offset: 0x000711D0
	public static SafeCellQuery.SafeFlags GetFlags(int cell, MinionBrain brain, bool avoid_light = false)
	{
		int num = Grid.CellAbove(cell);
		if (!Grid.IsValidCell(num))
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		if (Grid.Solid[cell] || Grid.Solid[num])
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		if (Grid.IsTileUnderConstruction[cell] || Grid.IsTileUnderConstruction[num])
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		bool flag = brain.IsCellClear(cell);
		bool flag2 = !Grid.Element[cell].IsLiquid;
		bool flag3 = !Grid.Element[num].IsLiquid;
		bool flag4 = Grid.Temperature[cell] > 285.15f && Grid.Temperature[cell] < 303.15f;
		bool flag5 = Grid.Radiation[cell] < 250f;
		bool flag6 = brain.OxygenBreather.IsBreathableElementAtCell(cell, Grid.DefaultOffset);
		bool flag7 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole);
		bool flag8 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Tube);
		bool flag9 = !avoid_light || SleepChore.IsDarkAtCell(cell);
		if (cell == Grid.PosToCell(brain))
		{
			flag6 = !brain.OxygenBreather.IsSuffocating;
		}
		SafeCellQuery.SafeFlags safeFlags = (SafeCellQuery.SafeFlags)0;
		if (flag)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsClear;
		}
		if (flag4)
		{
			safeFlags |= SafeCellQuery.SafeFlags.CorrectTemperature;
		}
		if (flag5)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotRadiated;
		}
		if (flag6)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsBreathable;
		}
		if (flag7)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLadder;
		}
		if (flag8)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotTube;
		}
		if (flag2)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquid;
		}
		if (flag3)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace;
		}
		if (flag9)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsLightOk;
		}
		return safeFlags;
	}

	// Token: 0x060015D3 RID: 5587 RVA: 0x00073180 File Offset: 0x00071380
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, this.avoid_light);
		bool flag = flags > this.targetCellFlags;
		bool flag2 = flags == this.targetCellFlags && cost < this.targetCost;
		if (flag || flag2)
		{
			this.targetCellFlags = flags;
			this.targetCost = cost;
			this.targetCell = cell;
		}
		return false;
	}

	// Token: 0x060015D4 RID: 5588 RVA: 0x000731D9 File Offset: 0x000713D9
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04000C28 RID: 3112
	private MinionBrain brain;

	// Token: 0x04000C29 RID: 3113
	private int targetCell;

	// Token: 0x04000C2A RID: 3114
	private int targetCost;

	// Token: 0x04000C2B RID: 3115
	public SafeCellQuery.SafeFlags targetCellFlags;

	// Token: 0x04000C2C RID: 3116
	private bool avoid_light;

	// Token: 0x0200108E RID: 4238
	public enum SafeFlags
	{
		// Token: 0x04005984 RID: 22916
		IsClear = 1,
		// Token: 0x04005985 RID: 22917
		IsLightOk,
		// Token: 0x04005986 RID: 22918
		IsNotLadder = 4,
		// Token: 0x04005987 RID: 22919
		IsNotTube = 8,
		// Token: 0x04005988 RID: 22920
		CorrectTemperature = 16,
		// Token: 0x04005989 RID: 22921
		IsNotRadiated = 32,
		// Token: 0x0400598A RID: 22922
		IsBreathable = 64,
		// Token: 0x0400598B RID: 22923
		IsNotLiquidOnMyFace = 128,
		// Token: 0x0400598C RID: 22924
		IsNotLiquid = 256
	}
}
