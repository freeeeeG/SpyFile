using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000057 RID: 87
	[AddComponentMenu("Pathfinding/Navmesh/RecastTileUpdateHandler")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_recast_tile_update_handler.php")]
	public class RecastTileUpdateHandler : MonoBehaviour
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x00014DEF File Offset: 0x00012FEF
		public void SetGraph(RecastGraph graph)
		{
			this.graph = graph;
			if (graph == null)
			{
				return;
			}
			this.dirtyTiles = new bool[graph.tileXCount * graph.tileZCount];
			this.anyDirtyTiles = false;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00014E1C File Offset: 0x0001301C
		public void ScheduleUpdate(Bounds bounds)
		{
			if (this.graph == null)
			{
				if (AstarPath.active != null)
				{
					this.SetGraph(AstarPath.active.data.recastGraph);
				}
				if (this.graph == null)
				{
					Debug.LogError("Received tile update request (from RecastTileUpdate), but no RecastGraph could be found to handle it");
					return;
				}
			}
			int num = Mathf.CeilToInt(this.graph.characterRadius / this.graph.cellSize) + 3;
			bounds.Expand(new Vector3((float)num, 0f, (float)num) * this.graph.cellSize * 2f);
			IntRect touchingTiles = this.graph.GetTouchingTiles(bounds, 0f);
			if (touchingTiles.Width * touchingTiles.Height > 0)
			{
				if (!this.anyDirtyTiles)
				{
					this.earliestDirty = Time.time;
					this.anyDirtyTiles = true;
				}
				for (int i = touchingTiles.ymin; i <= touchingTiles.ymax; i++)
				{
					for (int j = touchingTiles.xmin; j <= touchingTiles.xmax; j++)
					{
						this.dirtyTiles[i * this.graph.tileXCount + j] = true;
					}
				}
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00014F35 File Offset: 0x00013135
		private void OnEnable()
		{
			RecastTileUpdate.OnNeedUpdates += this.ScheduleUpdate;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00014F48 File Offset: 0x00013148
		private void OnDisable()
		{
			RecastTileUpdate.OnNeedUpdates -= this.ScheduleUpdate;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00014F5B File Offset: 0x0001315B
		private void Update()
		{
			if (this.anyDirtyTiles && Time.time - this.earliestDirty >= this.maxThrottlingDelay && this.graph != null)
			{
				this.UpdateDirtyTiles();
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00014F88 File Offset: 0x00013188
		public void UpdateDirtyTiles()
		{
			if (this.graph == null)
			{
				new InvalidOperationException("No graph is set on this object");
			}
			if (this.graph.tileXCount * this.graph.tileZCount != this.dirtyTiles.Length)
			{
				Debug.LogError("Graph has changed dimensions. Clearing queued graph updates and resetting.");
				this.SetGraph(this.graph);
				return;
			}
			for (int i = 0; i < this.graph.tileZCount; i++)
			{
				for (int j = 0; j < this.graph.tileXCount; j++)
				{
					if (this.dirtyTiles[i * this.graph.tileXCount + j])
					{
						this.dirtyTiles[i * this.graph.tileXCount + j] = false;
						Bounds tileBounds = this.graph.GetTileBounds(j, i, 1, 1);
						tileBounds.extents *= 0.5f;
						GraphUpdateObject graphUpdateObject = new GraphUpdateObject(tileBounds);
						graphUpdateObject.nnConstraint.graphMask = 1 << (int)this.graph.graphIndex;
						AstarPath.active.UpdateGraphs(graphUpdateObject);
					}
				}
			}
			this.anyDirtyTiles = false;
		}

		// Token: 0x0400027A RID: 634
		private RecastGraph graph;

		// Token: 0x0400027B RID: 635
		private bool[] dirtyTiles;

		// Token: 0x0400027C RID: 636
		private bool anyDirtyTiles;

		// Token: 0x0400027D RID: 637
		private float earliestDirty = float.NegativeInfinity;

		// Token: 0x0400027E RID: 638
		public float maxThrottlingDelay = 0.5f;
	}
}
