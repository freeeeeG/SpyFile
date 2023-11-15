using System;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020000DE RID: 222
	[AddComponentMenu("Pathfinding/Local Avoidance/Square Obstacle")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_r_v_o_1_1_r_v_o_square_obstacle.php")]
	public class RVOSquareObstacle : RVOObstacle
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x0003DCC0 File Offset: 0x0003BEC0
		protected override bool StaticObstacle
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x0003DCC3 File Offset: 0x0003BEC3
		protected override bool ExecuteInEditor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x0003DCC6 File Offset: 0x0003BEC6
		protected override bool LocalCoordinates
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x0003DCC9 File Offset: 0x0003BEC9
		protected override float Height
		{
			get
			{
				return this.height;
			}
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0003DCD1 File Offset: 0x0003BED1
		protected override bool AreGizmosDirty()
		{
			return false;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0003DCD4 File Offset: 0x0003BED4
		protected override void CreateObstacles()
		{
			this.size.x = Mathf.Abs(this.size.x);
			this.size.y = Mathf.Abs(this.size.y);
			this.height = Mathf.Abs(this.height);
			Vector3[] array = new Vector3[]
			{
				new Vector3(1f, 0f, -1f),
				new Vector3(1f, 0f, 1f),
				new Vector3(-1f, 0f, 1f),
				new Vector3(-1f, 0f, -1f)
			};
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Scale(new Vector3(this.size.x * 0.5f, 0f, this.size.y * 0.5f));
				array[i] += new Vector3(this.center.x, 0f, this.center.y);
			}
			base.AddObstacle(array, this.height);
		}

		// Token: 0x04000588 RID: 1416
		public float height = 1f;

		// Token: 0x04000589 RID: 1417
		public Vector2 size = Vector3.one;

		// Token: 0x0400058A RID: 1418
		public Vector2 center = Vector3.zero;
	}
}
