using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020000DB RID: 219
	[AddComponentMenu("Pathfinding/Local Avoidance/RVO Navmesh")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_r_v_o_1_1_r_v_o_navmesh.php")]
	public class RVONavmesh : GraphModifier
	{
		// Token: 0x0600094F RID: 2383 RVA: 0x0003D375 File Offset: 0x0003B575
		public override void OnPostCacheLoad()
		{
			this.OnLatePostScan();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0003D37D File Offset: 0x0003B57D
		public override void OnGraphsPostUpdate()
		{
			this.OnLatePostScan();
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0003D388 File Offset: 0x0003B588
		public override void OnLatePostScan()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.RemoveObstacles();
			NavGraph[] graphs = AstarPath.active.graphs;
			RVOSimulator active = RVOSimulator.active;
			if (active == null)
			{
				throw new NullReferenceException("No RVOSimulator could be found in the scene. Please add one to any GameObject");
			}
			this.lastSim = active.GetSimulator();
			for (int i = 0; i < graphs.Length; i++)
			{
				RecastGraph recastGraph = graphs[i] as RecastGraph;
				INavmesh navmesh = graphs[i] as INavmesh;
				GridGraph gridGraph = graphs[i] as GridGraph;
				if (recastGraph != null)
				{
					foreach (NavmeshTile navmesh2 in recastGraph.GetTiles())
					{
						this.AddGraphObstacles(this.lastSim, navmesh2);
					}
				}
				else if (navmesh != null)
				{
					this.AddGraphObstacles(this.lastSim, navmesh);
				}
				else if (gridGraph != null)
				{
					this.AddGraphObstacles(this.lastSim, gridGraph);
				}
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0003D45D File Offset: 0x0003B65D
		protected override void OnDisable()
		{
			base.OnDisable();
			this.RemoveObstacles();
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0003D46C File Offset: 0x0003B66C
		public void RemoveObstacles()
		{
			if (this.lastSim != null)
			{
				for (int i = 0; i < this.obstacles.Count; i++)
				{
					this.lastSim.RemoveObstacle(this.obstacles[i]);
				}
				this.lastSim = null;
			}
			this.obstacles.Clear();
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0003D4C0 File Offset: 0x0003B6C0
		private void AddGraphObstacles(Simulator sim, GridGraph grid)
		{
			bool reverse = Vector3.Dot(grid.transform.TransformVector(Vector3.up), (sim.movementPlane == MovementPlane.XY) ? Vector3.back : Vector3.up) > 0f;
			GraphUtilities.GetContours(grid, delegate(Vector3[] vertices)
			{
				if (reverse)
				{
					Array.Reverse<Vector3>(vertices);
				}
				this.obstacles.Add(sim.AddObstacle(vertices, this.wallHeight, true));
			}, this.wallHeight * 0.4f, null);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0003D53C File Offset: 0x0003B73C
		private void AddGraphObstacles(Simulator simulator, INavmesh navmesh)
		{
			GraphUtilities.GetContours(navmesh, delegate(List<Int3> vertices, bool cycle)
			{
				Vector3[] array = new Vector3[vertices.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = (Vector3)vertices[i];
				}
				ListPool<Int3>.Release(vertices);
				this.obstacles.Add(simulator.AddObstacle(array, this.wallHeight, cycle));
			});
		}

		// Token: 0x04000574 RID: 1396
		public float wallHeight = 5f;

		// Token: 0x04000575 RID: 1397
		private readonly List<ObstacleVertex> obstacles = new List<ObstacleVertex>();

		// Token: 0x04000576 RID: 1398
		private Simulator lastSim;
	}
}
