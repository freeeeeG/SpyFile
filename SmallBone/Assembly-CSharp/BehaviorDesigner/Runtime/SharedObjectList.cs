using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001452 RID: 5202
	[Serializable]
	public class SharedObjectList : SharedVariable<List<UnityEngine.Object>>
	{
		// Token: 0x060065CA RID: 26058 RVA: 0x0012639B File Offset: 0x0012459B
		public static implicit operator SharedObjectList(List<UnityEngine.Object> value)
		{
			return new SharedObjectList
			{
				mValue = value
			};
		}
	}
}
