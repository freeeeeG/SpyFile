using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000A20 RID: 2592
public class UtilityNetworkTubesManager : UtilityNetworkManager<TravelTubeNetwork, TravelTube>
{
	// Token: 0x06004DA7 RID: 19879 RVA: 0x001B385F File Offset: 0x001B1A5F
	public UtilityNetworkTubesManager(int game_width, int game_height, int tile_layer) : base(game_width, game_height, tile_layer)
	{
	}

	// Token: 0x06004DA8 RID: 19880 RVA: 0x001B386A File Offset: 0x001B1A6A
	public override bool CanAddConnection(UtilityConnections new_connection, int cell, bool is_physical_building, out string fail_reason)
	{
		return this.TestForUTurnLeft(cell, new_connection, is_physical_building, out fail_reason) && this.TestForUTurnRight(cell, new_connection, is_physical_building, out fail_reason) && this.TestForNoAdjacentBridge(cell, new_connection, out fail_reason);
	}

	// Token: 0x06004DA9 RID: 19881 RVA: 0x001B3892 File Offset: 0x001B1A92
	public override void SetConnections(UtilityConnections connections, int cell, bool is_physical_building)
	{
		base.SetConnections(connections, cell, is_physical_building);
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
	}

	// Token: 0x06004DAA RID: 19882 RVA: 0x001B38A8 File Offset: 0x001B1AA8
	private bool TestForUTurnLeft(int first_cell, UtilityConnections first_connection, bool is_physical_building, out string fail_reason)
	{
		int from_cell = first_cell;
		UtilityConnections direction = first_connection;
		int num = 1;
		for (int i = 0; i < 3; i++)
		{
			int num2 = direction.CellInDirection(from_cell);
			UtilityConnections utilityConnections = direction.LeftDirection();
			if (this.HasConnection(num2, utilityConnections, is_physical_building))
			{
				num++;
			}
			from_cell = num2;
			direction = utilityConnections;
		}
		fail_reason = UI.TOOLTIPS.HELP_TUBELOCATION_NO_UTURNS;
		return num <= 2;
	}

	// Token: 0x06004DAB RID: 19883 RVA: 0x001B3904 File Offset: 0x001B1B04
	private bool TestForUTurnRight(int first_cell, UtilityConnections first_connection, bool is_physical_building, out string fail_reason)
	{
		int from_cell = first_cell;
		UtilityConnections direction = first_connection;
		int num = 1;
		for (int i = 0; i < 3; i++)
		{
			int num2 = direction.CellInDirection(from_cell);
			UtilityConnections utilityConnections = direction.RightDirection();
			if (this.HasConnection(num2, utilityConnections, is_physical_building))
			{
				num++;
			}
			from_cell = num2;
			direction = utilityConnections;
		}
		fail_reason = UI.TOOLTIPS.HELP_TUBELOCATION_NO_UTURNS;
		return num <= 2;
	}

	// Token: 0x06004DAC RID: 19884 RVA: 0x001B3960 File Offset: 0x001B1B60
	private bool TestForNoAdjacentBridge(int cell, UtilityConnections connection, out string fail_reason)
	{
		UtilityConnections direction = connection.LeftDirection();
		UtilityConnections direction2 = connection.RightDirection();
		int cell2 = direction.CellInDirection(cell);
		int cell3 = direction2.CellInDirection(cell);
		GameObject gameObject = Grid.Objects[cell2, 9];
		GameObject gameObject2 = Grid.Objects[cell3, 9];
		fail_reason = UI.TOOLTIPS.HELP_TUBELOCATION_STRAIGHT_BRIDGES;
		return (gameObject == null || gameObject.GetComponent<TravelTubeBridge>() == null) && (gameObject2 == null || gameObject2.GetComponent<TravelTubeBridge>() == null);
	}

	// Token: 0x06004DAD RID: 19885 RVA: 0x001B39E4 File Offset: 0x001B1BE4
	private bool HasConnection(int cell, UtilityConnections connection, bool is_physical_building)
	{
		return (base.GetConnections(cell, is_physical_building) & connection) > (UtilityConnections)0;
	}
}
