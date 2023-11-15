using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001442 RID: 5186
	[Serializable]
	public class SharedAnimationCurve : SharedVariable<AnimationCurve>
	{
		// Token: 0x060065AA RID: 26026 RVA: 0x0012623B File Offset: 0x0012443B
		public static implicit operator SharedAnimationCurve(AnimationCurve value)
		{
			return new SharedAnimationCurve
			{
				mValue = value
			};
		}
	}
}
