using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001448 RID: 5192
	[Serializable]
	public class SharedCollider : SharedVariable<Collider2D>
	{
		// Token: 0x060065B6 RID: 26038 RVA: 0x001262BF File Offset: 0x001244BF
		public static implicit operator SharedCollider(Collider2D value)
		{
			return new SharedCollider
			{
				mValue = value
			};
		}
	}
}
