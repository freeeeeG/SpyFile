using System;

// Token: 0x02000831 RID: 2097
public class UtilityBuildTool : BaseUtilityBuildTool
{
	// Token: 0x06003CFC RID: 15612 RVA: 0x00152CA3 File Offset: 0x00150EA3
	public static void DestroyInstance()
	{
		UtilityBuildTool.Instance = null;
	}

	// Token: 0x06003CFD RID: 15613 RVA: 0x00152CAB File Offset: 0x00150EAB
	protected override void OnPrefabInit()
	{
		UtilityBuildTool.Instance = this;
		base.OnPrefabInit();
		this.populateHitsList = true;
		this.canChangeDragAxis = false;
	}

	// Token: 0x06003CFE RID: 15614 RVA: 0x00152CC8 File Offset: 0x00150EC8
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
				UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, cell2);
				if (utilityConnections != (UtilityConnections)0)
				{
					UtilityConnections new_connection = utilityConnections.InverseDirection();
					string text;
					if (this.conduitMgr.CanAddConnection(utilityConnections, cell, false, out text) && this.conduitMgr.CanAddConnection(new_connection, cell2, false, out text))
					{
						this.conduitMgr.AddConnection(utilityConnections, cell, false);
						this.conduitMgr.AddConnection(new_connection, cell2, false);
					}
					else if (i == this.path.Count - 1 && this.lastPathHead != i)
					{
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, text, null, Grid.CellToPosCCC(cell2, (Grid.SceneLayer)0), 1.5f, false, false);
					}
				}
			}
		}
		this.lastPathHead = this.path.Count - 1;
	}

	// Token: 0x040027CF RID: 10191
	public static UtilityBuildTool Instance;

	// Token: 0x040027D0 RID: 10192
	private int lastPathHead = -1;
}
