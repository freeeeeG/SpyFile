using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x02001450 RID: 5200
	[Serializable]
	public class SharedMaterial : SharedVariable<Material>
	{
		// Token: 0x060065C6 RID: 26054 RVA: 0x0012636F File Offset: 0x0012456F
		public static implicit operator SharedMaterial(Material value)
		{
			return new SharedMaterial
			{
				mValue = value
			};
		}
	}
}
