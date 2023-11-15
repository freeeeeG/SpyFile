using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x0200145D RID: 5213
	[Serializable]
	public class SharedVector3Int : SharedVariable<Vector3Int>
	{
		// Token: 0x060065E0 RID: 26080 RVA: 0x0012648D File Offset: 0x0012468D
		public static implicit operator SharedVector3Int(Vector3Int value)
		{
			return new SharedVector3Int
			{
				mValue = value
			};
		}
	}
}
