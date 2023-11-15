using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001454 RID: 5204
	[Serializable]
	public class SharedQuaternion : SharedVariable<Quaternion>
	{
		// Token: 0x060065CE RID: 26062 RVA: 0x001263C7 File Offset: 0x001245C7
		public static implicit operator SharedQuaternion(Quaternion value)
		{
			return new SharedQuaternion
			{
				mValue = value
			};
		}
	}
}
