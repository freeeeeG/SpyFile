using System;
using UnityEngine;

namespace PathCreation.Examples
{
	// Token: 0x020002B7 RID: 695
	[ExecuteInEditMode]
	public abstract class PathSceneTool : MonoBehaviour
	{
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06001110 RID: 4368 RVA: 0x000302FC File Offset: 0x0002E4FC
		// (remove) Token: 0x06001111 RID: 4369 RVA: 0x00030334 File Offset: 0x0002E534
		public event Action onDestroyed;

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001112 RID: 4370 RVA: 0x00030369 File Offset: 0x0002E569
		protected VertexPath path
		{
			get
			{
				return this.pathCreator.path;
			}
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00030376 File Offset: 0x0002E576
		public void TriggerUpdate()
		{
			this.PathUpdated();
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0003037E File Offset: 0x0002E57E
		protected virtual void OnDestroy()
		{
			if (this.onDestroyed != null)
			{
				this.onDestroyed();
			}
		}

		// Token: 0x06001115 RID: 4373
		protected abstract void PathUpdated();

		// Token: 0x0400094D RID: 2381
		public PathCreator pathCreator;

		// Token: 0x0400094E RID: 2382
		public bool autoUpdate = true;
	}
}
