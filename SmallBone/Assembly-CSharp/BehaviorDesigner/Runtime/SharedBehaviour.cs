using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001443 RID: 5187
	[Serializable]
	public class SharedBehaviour : SharedVariable<Behaviour>
	{
		// Token: 0x060065AC RID: 26028 RVA: 0x00126251 File Offset: 0x00124451
		public static explicit operator SharedBehaviour(Behaviour value)
		{
			return new SharedBehaviour
			{
				mValue = value
			};
		}
	}
}
