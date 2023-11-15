using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x0200145A RID: 5210
	[Serializable]
	public class SharedVector2 : SharedVariable<Vector2>
	{
		// Token: 0x060065DA RID: 26074 RVA: 0x0012644B File Offset: 0x0012464B
		public static implicit operator SharedVector2(Vector2 value)
		{
			return new SharedVector2
			{
				mValue = value
			};
		}
	}
}
