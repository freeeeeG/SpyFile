﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000703 RID: 1795
public static class CreatureHelpers
{
	// Token: 0x06003155 RID: 12629 RVA: 0x00106468 File Offset: 0x00104668
	public static bool isClear(int cell)
	{
		return Grid.IsValidCell(cell) && !Grid.Solid[cell] && !Grid.IsSubstantialLiquid(cell, 0.9f) && (!Grid.IsValidCell(Grid.CellBelow(cell)) || !Grid.IsLiquid(cell) || !Grid.IsLiquid(Grid.CellBelow(cell)));
	}

	// Token: 0x06003156 RID: 12630 RVA: 0x001064C0 File Offset: 0x001046C0
	public static int FindNearbyBreathableCell(int currentLocation, SimHashes breathableElement)
	{
		return currentLocation;
	}

	// Token: 0x06003157 RID: 12631 RVA: 0x001064C4 File Offset: 0x001046C4
	public static bool cellsAreClear(int[] cells)
	{
		for (int i = 0; i < cells.Length; i++)
		{
			if (!Grid.IsValidCell(cells[i]))
			{
				return false;
			}
			if (!CreatureHelpers.isClear(cells[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003158 RID: 12632 RVA: 0x001064F8 File Offset: 0x001046F8
	public static Vector3 PositionOfCurrentCell(Vector3 transformPosition)
	{
		return Grid.CellToPos(Grid.PosToCell(transformPosition));
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x00106505 File Offset: 0x00104705
	public static Vector3 CenterPositionOfCell(int cell)
	{
		return Grid.CellToPos(cell) + new Vector3(0.5f, 0.5f, -2f);
	}

	// Token: 0x0600315A RID: 12634 RVA: 0x00106528 File Offset: 0x00104728
	public static void DeselectCreature(GameObject creature)
	{
		KSelectable component = creature.GetComponent<KSelectable>();
		if (component != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(null, false);
		}
	}

	// Token: 0x0600315B RID: 12635 RVA: 0x00106563 File Offset: 0x00104763
	public static bool isSwimmable(int cell)
	{
		return Grid.IsValidCell(cell) && !Grid.Solid[cell] && Grid.IsSubstantialLiquid(cell, 0.35f);
	}

	// Token: 0x0600315C RID: 12636 RVA: 0x0010658E File Offset: 0x0010478E
	public static bool isSolidGround(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.Solid[cell];
	}

	// Token: 0x0600315D RID: 12637 RVA: 0x001065AA File Offset: 0x001047AA
	public static void FlipAnim(KAnimControllerBase anim, Vector3 heading)
	{
		if (heading.x < 0f)
		{
			anim.FlipX = true;
			return;
		}
		if (heading.x > 0f)
		{
			anim.FlipX = false;
		}
	}

	// Token: 0x0600315E RID: 12638 RVA: 0x001065D5 File Offset: 0x001047D5
	public static void FlipAnim(KBatchedAnimController anim, Vector3 heading)
	{
		if (heading.x < 0f)
		{
			anim.FlipX = true;
			return;
		}
		if (heading.x > 0f)
		{
			anim.FlipX = false;
		}
	}

	// Token: 0x0600315F RID: 12639 RVA: 0x00106600 File Offset: 0x00104800
	public static Vector3 GetWalkMoveTarget(Transform transform, Vector2 Heading)
	{
		int cell = Grid.PosToCell(transform.GetPosition());
		if (Heading.x == 1f)
		{
			if (CreatureHelpers.isClear(Grid.CellRight(cell)) && CreatureHelpers.isClear(Grid.CellDownRight(cell)) && CreatureHelpers.isClear(Grid.CellRight(Grid.CellRight(cell))) && !CreatureHelpers.isClear(Grid.PosToCell(transform.GetPosition() + Vector3.right * 2f + Vector3.down)))
			{
				return transform.GetPosition() + Vector3.right * 2f;
			}
			if (CreatureHelpers.cellsAreClear(new int[]
			{
				Grid.CellRight(cell),
				Grid.CellDownRight(cell)
			}) && !CreatureHelpers.isClear(Grid.CellBelow(Grid.CellDownRight(cell))))
			{
				return transform.GetPosition() + Vector3.right + Vector3.down;
			}
			if (CreatureHelpers.cellsAreClear(new int[]
			{
				Grid.OffsetCell(cell, 1, 0),
				Grid.OffsetCell(cell, 1, -1),
				Grid.OffsetCell(cell, 1, -2)
			}) && !CreatureHelpers.isClear(Grid.OffsetCell(cell, 1, -3)))
			{
				return transform.GetPosition() + Vector3.right + Vector3.down + Vector3.down;
			}
			if (CreatureHelpers.cellsAreClear(new int[]
			{
				Grid.OffsetCell(cell, 1, 0),
				Grid.OffsetCell(cell, 1, -1),
				Grid.OffsetCell(cell, 1, -2),
				Grid.OffsetCell(cell, 1, -3)
			}))
			{
				return transform.GetPosition();
			}
			if (CreatureHelpers.isClear(Grid.CellRight(cell)))
			{
				return transform.GetPosition() + Vector3.right;
			}
			if (CreatureHelpers.isClear(Grid.CellUpRight(cell)) && !Grid.Solid[Grid.CellAbove(cell)] && Grid.Solid[Grid.CellRight(cell)])
			{
				return transform.GetPosition() + Vector3.up + Vector3.right;
			}
			if (!Grid.Solid[Grid.CellAbove(cell)] && !Grid.Solid[Grid.CellAbove(Grid.CellAbove(cell))] && Grid.Solid[Grid.CellAbove(Grid.CellRight(cell))] && CreatureHelpers.isClear(Grid.CellRight(Grid.CellAbove(Grid.CellAbove(cell)))))
			{
				return transform.GetPosition() + Vector3.up + Vector3.up + Vector3.right;
			}
		}
		if (Heading.x == -1f)
		{
			if (CreatureHelpers.isClear(Grid.CellLeft(cell)) && CreatureHelpers.isClear(Grid.CellDownLeft(cell)) && CreatureHelpers.isClear(Grid.CellLeft(Grid.CellLeft(cell))) && !CreatureHelpers.isClear(Grid.PosToCell(transform.GetPosition() + Vector3.left * 2f + Vector3.down)))
			{
				return transform.GetPosition() + Vector3.left * 2f;
			}
			if (CreatureHelpers.cellsAreClear(new int[]
			{
				Grid.CellLeft(cell),
				Grid.CellDownLeft(cell)
			}) && !CreatureHelpers.isClear(Grid.CellBelow(Grid.CellDownLeft(cell))))
			{
				return transform.GetPosition() + Vector3.left + Vector3.down;
			}
			if (CreatureHelpers.cellsAreClear(new int[]
			{
				Grid.OffsetCell(cell, -1, 0),
				Grid.OffsetCell(cell, -1, -1),
				Grid.OffsetCell(cell, -1, -2)
			}) && !CreatureHelpers.isClear(Grid.OffsetCell(cell, -1, -3)))
			{
				return transform.GetPosition() + Vector3.left + Vector3.down + Vector3.down;
			}
			if (CreatureHelpers.cellsAreClear(new int[]
			{
				Grid.OffsetCell(cell, -1, 0),
				Grid.OffsetCell(cell, -1, -1),
				Grid.OffsetCell(cell, -1, -2),
				Grid.OffsetCell(cell, -1, -3)
			}))
			{
				return transform.GetPosition();
			}
			if (CreatureHelpers.isClear(Grid.CellLeft(Grid.PosToCell(transform.GetPosition()))))
			{
				return transform.GetPosition() + Vector3.left;
			}
			if (CreatureHelpers.isClear(Grid.CellUpLeft(cell)) && !Grid.Solid[Grid.CellAbove(cell)] && Grid.Solid[Grid.CellLeft(cell)])
			{
				return transform.GetPosition() + Vector3.up + Vector3.left;
			}
			if (!Grid.Solid[Grid.CellAbove(cell)] && !Grid.Solid[Grid.CellAbove(Grid.CellAbove(cell))] && Grid.Solid[Grid.CellAbove(Grid.CellLeft(cell))] && CreatureHelpers.isClear(Grid.CellLeft(Grid.CellAbove(Grid.CellAbove(cell)))))
			{
				return transform.GetPosition() + Vector3.up + Vector3.up + Vector3.left;
			}
		}
		return transform.GetPosition();
	}

	// Token: 0x06003160 RID: 12640 RVA: 0x00106AE8 File Offset: 0x00104CE8
	public static bool CrewNearby(Transform transform, int range = 6)
	{
		int cell = Grid.PosToCell(transform.gameObject);
		for (int i = 1; i < range; i++)
		{
			int cell2 = Grid.OffsetCell(cell, i, 0);
			int cell3 = Grid.OffsetCell(cell, -i, 0);
			if (Grid.Objects[cell2, 0] != null)
			{
				return true;
			}
			if (Grid.Objects[cell3, 0] != null)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003161 RID: 12641 RVA: 0x00106B50 File Offset: 0x00104D50
	public static bool CheckHorizontalClear(Vector3 startPosition, Vector3 endPosition)
	{
		int cell = Grid.PosToCell(startPosition);
		int num = 1;
		if (endPosition.x < startPosition.x)
		{
			num = -1;
		}
		float num2 = Mathf.Abs(endPosition.x - startPosition.x);
		int num3 = 0;
		while ((float)num3 < num2)
		{
			int i = Grid.OffsetCell(cell, num3 * num, 0);
			if (Grid.Solid[i])
			{
				return false;
			}
			num3++;
		}
		return true;
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x00106BB4 File Offset: 0x00104DB4
	public static GameObject GetFleeTargetLocatorObject(GameObject self, GameObject threat)
	{
		if (threat == null)
		{
			global::Debug.LogWarning(self.name + " is trying to flee, bus has no threats");
			return null;
		}
		int num = Grid.PosToCell(threat);
		int num2 = Grid.PosToCell(self);
		Navigator nav = self.GetComponent<Navigator>();
		if (nav == null)
		{
			global::Debug.LogWarning(self.name + " is trying to flee, bus has no navigator component attached.");
			return null;
		}
		HashSet<int> hashSet = GameUtil.FloodCollectCells(Grid.PosToCell(self), (int cell) => CreatureHelpers.CanFleeTo(cell, nav), 300, null, true);
		int num3 = -1;
		int num4 = -1;
		foreach (int num5 in hashSet)
		{
			if (nav.CanReach(num5) && num5 != num2)
			{
				int num6 = -1;
				num6 += Grid.GetCellDistance(num5, num);
				if (CreatureHelpers.isInFavoredFleeDirection(num5, num, self))
				{
					num6 += 2;
				}
				if (num6 > num4)
				{
					num4 = num6;
					num3 = num5;
				}
			}
		}
		if (num3 != -1)
		{
			return ChoreHelpers.CreateLocator("GoToLocator", Grid.CellToPos(num3));
		}
		return null;
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x00106CDC File Offset: 0x00104EDC
	private static bool isInFavoredFleeDirection(int targetFleeCell, int threatCell, GameObject self)
	{
		bool flag = Grid.CellToPos(threatCell).x < self.transform.GetPosition().x;
		bool flag2 = Grid.CellToPos(threatCell).x < Grid.CellToPos(targetFleeCell).x;
		return flag == flag2;
	}

	// Token: 0x06003164 RID: 12644 RVA: 0x00106D2C File Offset: 0x00104F2C
	private static bool CanFleeTo(int cell, Navigator nav)
	{
		return nav.CanReach(cell) || nav.CanReach(Grid.OffsetCell(cell, -1, -1)) || nav.CanReach(Grid.OffsetCell(cell, 1, -1)) || nav.CanReach(Grid.OffsetCell(cell, -1, 1)) || nav.CanReach(Grid.OffsetCell(cell, 1, 1));
	}
}
