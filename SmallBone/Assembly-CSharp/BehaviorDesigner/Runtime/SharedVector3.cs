using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x0200145C RID: 5212
	[Serializable]
	public class SharedVector3 : SharedVariable<Vector3>
	{
		// Token: 0x060065DE RID: 26078 RVA: 0x00126477 File Offset: 0x00124677
		public static implicit operator SharedVector3(Vector3 value)
		{
			return new SharedVector3
			{
				mValue = value
			};
		}
	}
}
