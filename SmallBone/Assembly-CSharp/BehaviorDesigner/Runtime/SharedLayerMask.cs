using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	// Token: 0x0200144F RID: 5199
	[Serializable]
	public class SharedLayerMask : SharedVariable<LayerMask>
	{
		// Token: 0x060065C4 RID: 26052 RVA: 0x00126359 File Offset: 0x00124559
		public static implicit operator SharedLayerMask(LayerMask value)
		{
			return new SharedLayerMask
			{
				Value = value
			};
		}
	}
}
