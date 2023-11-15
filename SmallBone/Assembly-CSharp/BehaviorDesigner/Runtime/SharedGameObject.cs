using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x0200144B RID: 5195
	[Serializable]
	public class SharedGameObject : SharedVariable<GameObject>
	{
		// Token: 0x060065BC RID: 26044 RVA: 0x00126301 File Offset: 0x00124501
		public static implicit operator SharedGameObject(GameObject value)
		{
			return new SharedGameObject
			{
				mValue = value
			};
		}
	}
}
