using System;
using System.Collections.Generic;

// Token: 0x02000409 RID: 1033
public class SafetyConditions
{
	// Token: 0x060015C8 RID: 5576 RVA: 0x00072B28 File Offset: 0x00070D28
	public SafetyConditions()
	{
		int num = 1;
		this.IsNearby = new SafetyChecker.Condition("IsNearby", num *= 2, (int cell, int cost, SafetyChecker.Context context) => cost > 5);
		this.IsNotLedge = new SafetyChecker.Condition("IsNotLedge", num *= 2, delegate(int cell, int cost, SafetyChecker.Context context)
		{
			int i = Grid.CellBelow(Grid.CellLeft(cell));
			if (Grid.Solid[i])
			{
				return false;
			}
			int i2 = Grid.CellBelow(Grid.CellRight(cell));
			return Grid.Solid[i2];
		});
		this.IsNotLiquid = new SafetyChecker.Condition("IsNotLiquid", num *= 2, (int cell, int cost, SafetyChecker.Context context) => !Grid.Element[cell].IsLiquid);
		this.IsNotLadder = new SafetyChecker.Condition("IsNotLadder", num *= 2, (int cell, int cost, SafetyChecker.Context context) => !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole));
		this.IsNotDoor = new SafetyChecker.Condition("IsNotDoor", num *= 2, delegate(int cell, int cost, SafetyChecker.Context context)
		{
			int num2 = Grid.CellAbove(cell);
			return !Grid.HasDoor[cell] && Grid.IsValidCell(num2) && !Grid.HasDoor[num2];
		});
		this.IsCorrectTemperature = new SafetyChecker.Condition("IsCorrectTemperature", num *= 2, (int cell, int cost, SafetyChecker.Context context) => Grid.Temperature[cell] > 285.15f && Grid.Temperature[cell] < 303.15f);
		this.IsWarming = new SafetyChecker.Condition("IsWarming", num *= 2, (int cell, int cost, SafetyChecker.Context context) => true);
		this.IsCooling = new SafetyChecker.Condition("IsCooling", num *= 2, (int cell, int cost, SafetyChecker.Context context) => false);
		this.HasSomeOxygen = new SafetyChecker.Condition("HasSomeOxygen", num *= 2, (int cell, int cost, SafetyChecker.Context context) => context.oxygenBreather.IsBreathableElementAtCell(cell, null));
		this.IsClear = new SafetyChecker.Condition("IsClear", num * 2, (int cell, int cost, SafetyChecker.Context context) => context.minionBrain.IsCellClear(cell));
		this.WarmUpChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsWarming
		}.ToArray());
		this.CoolDownChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsCooling
		}.ToArray());
		List<SafetyChecker.Condition> list = new List<SafetyChecker.Condition>();
		list.Add(this.HasSomeOxygen);
		list.Add(this.IsNotDoor);
		this.RecoverBreathChecker = new SafetyChecker(list.ToArray());
		List<SafetyChecker.Condition> list2 = new List<SafetyChecker.Condition>(list);
		list2.Add(this.IsNotLiquid);
		list2.Add(this.IsCorrectTemperature);
		this.SafeCellChecker = new SafetyChecker(list2.ToArray());
		this.IdleCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>(list2)
		{
			this.IsClear,
			this.IsNotLadder
		}.ToArray());
		this.VomitCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsNotLiquid,
			this.IsNotLedge,
			this.IsNearby
		}.ToArray());
	}

	// Token: 0x04000C0F RID: 3087
	public SafetyChecker.Condition IsNotLiquid;

	// Token: 0x04000C10 RID: 3088
	public SafetyChecker.Condition IsNotLadder;

	// Token: 0x04000C11 RID: 3089
	public SafetyChecker.Condition IsCorrectTemperature;

	// Token: 0x04000C12 RID: 3090
	public SafetyChecker.Condition IsWarming;

	// Token: 0x04000C13 RID: 3091
	public SafetyChecker.Condition IsCooling;

	// Token: 0x04000C14 RID: 3092
	public SafetyChecker.Condition HasSomeOxygen;

	// Token: 0x04000C15 RID: 3093
	public SafetyChecker.Condition IsClear;

	// Token: 0x04000C16 RID: 3094
	public SafetyChecker.Condition IsNotFoundation;

	// Token: 0x04000C17 RID: 3095
	public SafetyChecker.Condition IsNotDoor;

	// Token: 0x04000C18 RID: 3096
	public SafetyChecker.Condition IsNotLedge;

	// Token: 0x04000C19 RID: 3097
	public SafetyChecker.Condition IsNearby;

	// Token: 0x04000C1A RID: 3098
	public SafetyChecker WarmUpChecker;

	// Token: 0x04000C1B RID: 3099
	public SafetyChecker CoolDownChecker;

	// Token: 0x04000C1C RID: 3100
	public SafetyChecker RecoverBreathChecker;

	// Token: 0x04000C1D RID: 3101
	public SafetyChecker VomitCellChecker;

	// Token: 0x04000C1E RID: 3102
	public SafetyChecker SafeCellChecker;

	// Token: 0x04000C1F RID: 3103
	public SafetyChecker IdleCellChecker;
}
