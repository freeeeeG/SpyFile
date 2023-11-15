using System;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x0200144C RID: 5196
	[Serializable]
	public class SharedGameObjectList : SharedVariable<List<GameObject>>
	{
		// Token: 0x060065BE RID: 26046 RVA: 0x00126317 File Offset: 0x00124517
		public static implicit operator SharedGameObjectList(List<GameObject> value)
		{
			return new SharedGameObjectList
			{
				mValue = value
			};
		}
	}
}
