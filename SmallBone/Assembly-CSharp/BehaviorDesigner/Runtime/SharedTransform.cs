using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001457 RID: 5207
	[Serializable]
	public class SharedTransform : SharedVariable<Transform>
	{
		// Token: 0x060065D4 RID: 26068 RVA: 0x00126409 File Offset: 0x00124609
		public static implicit operator SharedTransform(Transform value)
		{
			return new SharedTransform
			{
				mValue = value
			};
		}
	}
}
