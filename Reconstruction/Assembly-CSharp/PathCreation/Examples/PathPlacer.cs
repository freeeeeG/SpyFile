using System;
using UnityEngine;

namespace PathCreation.Examples
{
	// Token: 0x020002B6 RID: 694
	[ExecuteInEditMode]
	public class PathPlacer : PathSceneTool
	{
		// Token: 0x0600110C RID: 4364 RVA: 0x000301E4 File Offset: 0x0002E3E4
		private void Generate()
		{
			if (this.pathCreator != null && this.prefab != null && this.holder != null)
			{
				this.DestroyObjects();
				VertexPath path = this.pathCreator.path;
				this.spacing = Mathf.Max(0.1f, this.spacing);
				for (float num = 0f; num < path.length; num += this.spacing)
				{
					Vector3 pointAtDistance = path.GetPointAtDistance(num, EndOfPathInstruction.Loop);
					Quaternion rotationAtDistance = path.GetRotationAtDistance(num, EndOfPathInstruction.Loop);
					Object.Instantiate<GameObject>(this.prefab, pointAtDistance, rotationAtDistance, this.holder.transform);
				}
			}
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0003028C File Offset: 0x0002E48C
		private void DestroyObjects()
		{
			for (int i = this.holder.transform.childCount - 1; i >= 0; i--)
			{
				Object.DestroyImmediate(this.holder.transform.GetChild(i).gameObject, false);
			}
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x000302D2 File Offset: 0x0002E4D2
		protected override void PathUpdated()
		{
			if (this.pathCreator != null)
			{
				this.Generate();
			}
		}

		// Token: 0x04000948 RID: 2376
		public GameObject prefab;

		// Token: 0x04000949 RID: 2377
		public GameObject holder;

		// Token: 0x0400094A RID: 2378
		public float spacing = 3f;

		// Token: 0x0400094B RID: 2379
		private const float minSpacing = 0.1f;
	}
}
