using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001451 RID: 5201
	[Serializable]
	public class SharedObject : SharedVariable<UnityEngine.Object>
	{
		// Token: 0x060065C8 RID: 26056 RVA: 0x00126385 File Offset: 0x00124585
		public static explicit operator SharedObject(UnityEngine.Object value)
		{
			return new SharedObject
			{
				mValue = value
			};
		}
	}
}
