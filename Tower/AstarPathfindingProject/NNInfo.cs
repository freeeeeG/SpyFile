using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000015 RID: 21
	public struct NNInfo
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000878F File Offset: 0x0000698F
		[Obsolete("This field has been renamed to 'position'")]
		public Vector3 clampedPosition
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008797 File Offset: 0x00006997
		public NNInfo(NNInfoInternal internalInfo)
		{
			this.node = internalInfo.node;
			this.position = internalInfo.clampedPosition;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000087B1 File Offset: 0x000069B1
		public static explicit operator Vector3(NNInfo ob)
		{
			return ob.position;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000087B9 File Offset: 0x000069B9
		public static explicit operator GraphNode(NNInfo ob)
		{
			return ob.node;
		}

		// Token: 0x040000F4 RID: 244
		public readonly GraphNode node;

		// Token: 0x040000F5 RID: 245
		public readonly Vector3 position;
	}
}
