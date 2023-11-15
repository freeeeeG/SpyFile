using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x0200145B RID: 5211
	[Serializable]
	public class SharedVector2Int : SharedVariable<Vector2Int>
	{
		// Token: 0x060065DC RID: 26076 RVA: 0x00126461 File Offset: 0x00124661
		public static implicit operator SharedVector2Int(Vector2Int value)
		{
			return new SharedVector2Int
			{
				mValue = value
			};
		}
	}
}
