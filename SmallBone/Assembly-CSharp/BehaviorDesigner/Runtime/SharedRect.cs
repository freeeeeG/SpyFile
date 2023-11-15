using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001455 RID: 5205
	[Serializable]
	public class SharedRect : SharedVariable<Rect>
	{
		// Token: 0x060065D0 RID: 26064 RVA: 0x001263DD File Offset: 0x001245DD
		public static implicit operator SharedRect(Rect value)
		{
			return new SharedRect
			{
				mValue = value
			};
		}
	}
}
