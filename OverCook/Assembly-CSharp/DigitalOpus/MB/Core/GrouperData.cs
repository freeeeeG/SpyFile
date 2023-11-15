using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200005C RID: 92
	[Serializable]
	public class GrouperData
	{
		// Token: 0x04000196 RID: 406
		public bool clusterOnLMIndex;

		// Token: 0x04000197 RID: 407
		public bool clusterByLODLevel;

		// Token: 0x04000198 RID: 408
		public Vector3 origin;

		// Token: 0x04000199 RID: 409
		public Vector3 cellSize;

		// Token: 0x0400019A RID: 410
		public int pieNumSegments = 4;

		// Token: 0x0400019B RID: 411
		public Vector3 pieAxis = Vector3.up;

		// Token: 0x0400019C RID: 412
		public int height = 1;

		// Token: 0x0400019D RID: 413
		public float maxDistBetweenClusters = 1f;

		// Token: 0x0400019E RID: 414
		public bool includeCellsWithOnlyOneRenderer = true;
	}
}
