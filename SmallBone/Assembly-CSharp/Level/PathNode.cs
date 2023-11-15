using System;
using GameResources;
using UnityEngine;

namespace Level
{
	// Token: 0x02000505 RID: 1285
	public class PathNode
	{
		// Token: 0x06001953 RID: 6483 RVA: 0x00003709 File Offset: 0x00001909
		public PathNode()
		{
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0004F759 File Offset: 0x0004D959
		public PathNode(MapReference reference, MapReward.Type reward, Gate.Type gate)
		{
			this.reference = reference;
			this.reward = reward;
			this.gate = gate;
		}

		// Token: 0x04001614 RID: 5652
		[HideInInspector]
		public MapReference reference;

		// Token: 0x04001615 RID: 5653
		public MapReward.Type reward;

		// Token: 0x04001616 RID: 5654
		public Gate.Type gate;

		// Token: 0x04001617 RID: 5655
		public static readonly PathNode none = new PathNode(null, MapReward.Type.None, Gate.Type.None);
	}
}
