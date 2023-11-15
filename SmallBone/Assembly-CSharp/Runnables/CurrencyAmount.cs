using System;
using Runnables.Cost;
using UnityEditor;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000311 RID: 785
	public abstract class CurrencyAmount : MonoBehaviour
	{
		// Token: 0x06000F43 RID: 3907
		public abstract int GetAmount();

		// Token: 0x04000C9E RID: 3230
		public static readonly Type[] types = new Type[]
		{
			typeof(CostEvent),
			typeof(Constant)
		};

		// Token: 0x02000312 RID: 786
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06000F46 RID: 3910 RVA: 0x0002EB32 File Offset: 0x0002CD32
			public SubcomponentAttribute() : base(true, CurrencyAmount.types)
			{
			}
		}
	}
}
