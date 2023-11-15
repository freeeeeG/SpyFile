using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x0200145E RID: 5214
	[Serializable]
	public class SharedVector4 : SharedVariable<Vector4>
	{
		// Token: 0x060065E2 RID: 26082 RVA: 0x001264A3 File Offset: 0x001246A3
		public static implicit operator SharedVector4(Vector4 value)
		{
			return new SharedVector4
			{
				mValue = value
			};
		}
	}
}
