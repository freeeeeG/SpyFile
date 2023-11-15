using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009CE RID: 2510
public class ConditionFlightPathIsClear : ProcessCondition
{
	// Token: 0x06004B1E RID: 19230 RVA: 0x001A6C9A File Offset: 0x001A4E9A
	public ConditionFlightPathIsClear(GameObject module, int bufferWidth)
	{
		this.module = module;
		this.bufferWidth = bufferWidth;
	}

	// Token: 0x06004B1F RID: 19231 RVA: 0x001A6CB7 File Offset: 0x001A4EB7
	public override ProcessCondition.Status EvaluateCondition()
	{
		this.Update();
		if (!this.hasClearSky)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B20 RID: 19232 RVA: 0x001A6CCA File Offset: 0x001A4ECA
	public override StatusItem GetStatusItem(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Failure)
		{
			return Db.Get().BuildingStatusItems.PathNotClear;
		}
		return null;
	}

	// Token: 0x06004B21 RID: 19233 RVA: 0x001A6CE0 File Offset: 0x001A4EE0
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.STATUS.FAILURE;
		}
		if (status != ProcessCondition.Status.Ready)
		{
			return Db.Get().BuildingStatusItems.PathNotClear.notificationText;
		}
		global::Debug.LogError("ConditionFlightPathIsClear: You'll need to add new strings/status items if you want to show the ready state");
		return "";
	}

	// Token: 0x06004B22 RID: 19234 RVA: 0x001A6D34 File Offset: 0x001A4F34
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.TOOLTIP.FAILURE;
		}
		if (status != ProcessCondition.Status.Ready)
		{
			return Db.Get().BuildingStatusItems.PathNotClear.notificationTooltipText;
		}
		global::Debug.LogError("ConditionFlightPathIsClear: You'll need to add new strings/status items if you want to show the ready state");
		return "";
	}

	// Token: 0x06004B23 RID: 19235 RVA: 0x001A6D86 File Offset: 0x001A4F86
	public override bool ShowInUI()
	{
		return DlcManager.FeatureClusterSpaceEnabled();
	}

	// Token: 0x06004B24 RID: 19236 RVA: 0x001A6D90 File Offset: 0x001A4F90
	public void Update()
	{
		Extents extents = this.module.GetComponent<Building>().GetExtents();
		int x = extents.x - this.bufferWidth;
		int x2 = extents.x + extents.width - 1 + this.bufferWidth;
		int y = extents.y;
		int num = Grid.XYToCell(x, y);
		int num2 = Grid.XYToCell(x2, y);
		this.hasClearSky = true;
		this.obstructedTile = -1;
		for (int i = num; i <= num2; i++)
		{
			if (!ConditionFlightPathIsClear.CanReachSpace(i, out this.obstructedTile))
			{
				this.hasClearSky = false;
				return;
			}
		}
	}

	// Token: 0x06004B25 RID: 19237 RVA: 0x001A6E20 File Offset: 0x001A5020
	public static int PadTopEdgeDistanceToOutOfScreenEdge(GameObject launchpad)
	{
		WorldContainer myWorld = launchpad.GetMyWorld();
		Vector2 maximumBounds = myWorld.maximumBounds;
		int y = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition).y;
		return (int)CameraController.GetHighestVisibleCell_Height((byte)myWorld.ParentWorldId) - y + 10;
	}

	// Token: 0x06004B26 RID: 19238 RVA: 0x001A6E64 File Offset: 0x001A5064
	public static int PadTopEdgeDistanceToCeilingEdge(GameObject launchpad)
	{
		Vector2 maximumBounds = launchpad.GetMyWorld().maximumBounds;
		int num = (int)launchpad.GetMyWorld().maximumBounds.y;
		int y = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition).y;
		return num - Grid.TopBorderHeight - y + 1;
	}

	// Token: 0x06004B27 RID: 19239 RVA: 0x001A6EB0 File Offset: 0x001A50B0
	public static bool CheckFlightPathClear(CraftModuleInterface craft, GameObject launchpad, out int obstruction)
	{
		Vector2I vector2I = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition);
		int num = ConditionFlightPathIsClear.PadTopEdgeDistanceToCeilingEdge(launchpad);
		foreach (Ref<RocketModuleCluster> @ref in craft.ClusterModules)
		{
			Building component = @ref.Get().GetComponent<Building>();
			int widthInCells = component.Def.WidthInCells;
			int moduleRelativeVerticalPosition = craft.GetModuleRelativeVerticalPosition(@ref.Get().gameObject);
			if (moduleRelativeVerticalPosition + component.Def.HeightInCells > num)
			{
				int num2 = Grid.XYToCell(vector2I.x, moduleRelativeVerticalPosition + vector2I.y);
				obstruction = num2;
				return false;
			}
			for (int i = moduleRelativeVerticalPosition; i < num; i++)
			{
				for (int j = 0; j < widthInCells; j++)
				{
					int num3 = Grid.XYToCell(j + (vector2I.x - widthInCells / 2), i + vector2I.y);
					bool flag = Grid.Solid[num3];
					if (!Grid.IsValidCell(num3) || Grid.WorldIdx[num3] != Grid.WorldIdx[launchpad.GetComponent<LaunchPad>().RocketBottomPosition] || flag)
					{
						obstruction = num3;
						return false;
					}
				}
			}
		}
		obstruction = -1;
		return true;
	}

	// Token: 0x06004B28 RID: 19240 RVA: 0x001A7010 File Offset: 0x001A5210
	private static bool CanReachSpace(int startCell, out int obstruction)
	{
		WorldContainer worldContainer = (startCell >= 0) ? ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[startCell]) : null;
		int num = (worldContainer == null) ? Grid.HeightInCells : ((int)worldContainer.maximumBounds.y);
		obstruction = -1;
		int num2 = startCell;
		while (Grid.CellRow(num2) < num)
		{
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2])
			{
				obstruction = num2;
				return false;
			}
			num2 = Grid.CellAbove(num2);
		}
		return true;
	}

	// Token: 0x06004B29 RID: 19241 RVA: 0x001A7088 File Offset: 0x001A5288
	public string GetObstruction()
	{
		if (this.obstructedTile == -1)
		{
			return null;
		}
		if (Grid.Objects[this.obstructedTile, 1] != null)
		{
			return Grid.Objects[this.obstructedTile, 1].GetComponent<Building>().Def.Name;
		}
		return string.Format(BUILDING.STATUSITEMS.PATH_NOT_CLEAR.TILE_FORMAT, Grid.Element[this.obstructedTile].tag.ProperName());
	}

	// Token: 0x04003139 RID: 12601
	private GameObject module;

	// Token: 0x0400313A RID: 12602
	private int bufferWidth;

	// Token: 0x0400313B RID: 12603
	private bool hasClearSky;

	// Token: 0x0400313C RID: 12604
	private int obstructedTile = -1;

	// Token: 0x0400313D RID: 12605
	public const int MAXIMUM_ROCKET_HEIGHT = 35;

	// Token: 0x0400313E RID: 12606
	public const float FIRE_FX_HEIGHT = 10f;
}
