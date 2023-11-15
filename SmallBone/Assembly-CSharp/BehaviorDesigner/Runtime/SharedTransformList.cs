using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001458 RID: 5208
	[Serializable]
	public class SharedTransformList : SharedVariable<List<Transform>>
	{
		// Token: 0x060065D6 RID: 26070 RVA: 0x0012641F File Offset: 0x0012461F
		public static implicit operator SharedTransformList(List<Transform> value)
		{
			return new SharedTransformList
			{
				mValue = value
			};
		}
	}
}
