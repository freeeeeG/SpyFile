using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020004ED RID: 1261
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Placeable")]
public class Placeable : KMonoBehaviour
{
	// Token: 0x06001D87 RID: 7559 RVA: 0x0009CDBC File Offset: 0x0009AFBC
	public bool IsValidPlaceLocation(int cell, out string reason)
	{
		if (this.placementRules.Contains(Placeable.PlacementRules.RestrictToWorld) && (int)Grid.WorldIdx[cell] != this.restrictWorldId)
		{
			reason = UI.TOOLS.PLACE.REASONS.RESTRICT_TO_WORLD;
			return false;
		}
		if (!this.occupyArea.CanOccupyArea(cell, this.occupyArea.objectLayers[0]))
		{
			reason = UI.TOOLS.PLACE.REASONS.CAN_OCCUPY_AREA;
			return false;
		}
		if (this.placementRules.Contains(Placeable.PlacementRules.OnFoundation))
		{
			bool flag = this.occupyArea.TestAreaBelow(cell, null, new Func<int, object, bool>(this.FoundationTest));
			if (this.checkRootCellOnly)
			{
				flag = this.FoundationTest(Grid.CellBelow(cell), null);
			}
			if (!flag)
			{
				reason = UI.TOOLS.PLACE.REASONS.ON_FOUNDATION;
				return false;
			}
		}
		if (this.placementRules.Contains(Placeable.PlacementRules.VisibleToSpace))
		{
			bool flag2 = this.occupyArea.TestArea(cell, null, new Func<int, object, bool>(this.SunnySpaceTest));
			if (this.checkRootCellOnly)
			{
				flag2 = this.SunnySpaceTest(cell, null);
			}
			if (!flag2)
			{
				reason = UI.TOOLS.PLACE.REASONS.VISIBLE_TO_SPACE;
				return false;
			}
		}
		reason = "ok!";
		return true;
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x0009CEC0 File Offset: 0x0009B0C0
	private bool SunnySpaceTest(int cell, object data)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int x;
		int startY;
		Grid.CellToXY(cell, out x, out startY);
		int num = (int)Grid.WorldIdx[cell];
		if (num == 255)
		{
			return false;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld(num);
		int top = world.WorldOffset.y + world.WorldSize.y;
		return !Grid.Solid[cell] && !Grid.Foundation[cell] && (Grid.ExposedToSunlight[cell] >= 253 || this.ClearPathToSky(x, startY, top));
	}

	// Token: 0x06001D89 RID: 7561 RVA: 0x0009CF54 File Offset: 0x0009B154
	private bool ClearPathToSky(int x, int startY, int top)
	{
		for (int i = startY; i < top; i++)
		{
			int i2 = Grid.XYToCell(x, i);
			if (Grid.Solid[i2] || Grid.Foundation[i2])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001D8A RID: 7562 RVA: 0x0009CF92 File Offset: 0x0009B192
	private bool FoundationTest(int cell, object data)
	{
		return Grid.IsValidBuildingCell(cell) && (Grid.Solid[cell] || Grid.Foundation[cell]);
	}

	// Token: 0x0400105D RID: 4189
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x0400105E RID: 4190
	public string kAnimName;

	// Token: 0x0400105F RID: 4191
	public string animName;

	// Token: 0x04001060 RID: 4192
	public List<Placeable.PlacementRules> placementRules = new List<Placeable.PlacementRules>();

	// Token: 0x04001061 RID: 4193
	[NonSerialized]
	public int restrictWorldId;

	// Token: 0x04001062 RID: 4194
	public bool checkRootCellOnly;

	// Token: 0x02001191 RID: 4497
	public enum PlacementRules
	{
		// Token: 0x04005CAF RID: 23727
		OnFoundation,
		// Token: 0x04005CB0 RID: 23728
		VisibleToSpace,
		// Token: 0x04005CB1 RID: 23729
		RestrictToWorld
	}
}
