using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001449 RID: 5193
	[Serializable]
	public class SharedColor : SharedVariable<Color>
	{
		// Token: 0x060065B8 RID: 26040 RVA: 0x001262D5 File Offset: 0x001244D5
		public static implicit operator SharedColor(Color value)
		{
			return new SharedColor
			{
				mValue = value
			};
		}
	}
}
