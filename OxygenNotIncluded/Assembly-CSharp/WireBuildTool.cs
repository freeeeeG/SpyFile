﻿using System;

// Token: 0x02000832 RID: 2098
public class WireBuildTool : BaseUtilityBuildTool
{
	// Token: 0x06003D00 RID: 15616 RVA: 0x00152E12 File Offset: 0x00151012
	public static void DestroyInstance()
	{
		WireBuildTool.Instance = null;
	}

	// Token: 0x06003D01 RID: 15617 RVA: 0x00152E1A File Offset: 0x0015101A
	protected override void OnPrefabInit()
	{
		WireBuildTool.Instance = this;
		base.OnPrefabInit();
		this.viewMode = OverlayModes.Power.ID;
	}

	// Token: 0x06003D02 RID: 15618 RVA: 0x00152E34 File Offset: 0x00151034
	protected override void ApplyPathToConduitSystem()
	{
		if (this.path.Count < 2)
		{
			return;
		}
		for (int i = 1; i < this.path.Count; i++)
		{
			if (this.path[i - 1].valid && this.path[i].valid)
			{
				int cell = this.path[i - 1].cell;
				int cell2 = this.path[i].cell;
				UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, this.path[i].cell);
				if (utilityConnections != (UtilityConnections)0)
				{
					UtilityConnections new_connection = utilityConnections.InverseDirection();
					this.conduitMgr.AddConnection(utilityConnections, cell, false);
					this.conduitMgr.AddConnection(new_connection, cell2, false);
				}
			}
		}
	}

	// Token: 0x040027D1 RID: 10193
	public static WireBuildTool Instance;
}
