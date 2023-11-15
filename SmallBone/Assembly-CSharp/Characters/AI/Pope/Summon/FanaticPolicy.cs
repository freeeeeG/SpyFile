using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Pope.Summon
{
	// Token: 0x02001215 RID: 4629
	public abstract class FanaticPolicy : MonoBehaviour
	{
		// Token: 0x06005AD2 RID: 23250 RVA: 0x0010D0D4 File Offset: 0x0010B2D4
		public List<FanaticFactory.SummonType> GetToSummons(int count)
		{
			if (this._toSummons == null)
			{
				this._toSummons = new List<FanaticFactory.SummonType>(count);
			}
			this.GetToSummons(ref this._toSummons, count);
			return this._toSummons;
		}

		// Token: 0x06005AD3 RID: 23251
		protected abstract void GetToSummons(ref List<FanaticFactory.SummonType> results, int count);

		// Token: 0x0400494A RID: 18762
		private List<FanaticFactory.SummonType> _toSummons;

		// Token: 0x02001216 RID: 4630
		[AttributeUsage(AttributeTargets.Field)]
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06005AD5 RID: 23253 RVA: 0x0010D0FD File Offset: 0x0010B2FD
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, FanaticPolicy.SubcomponentAttribute.types)
			{
			}

			// Token: 0x0400494B RID: 18763
			public new static readonly Type[] types = new Type[]
			{
				typeof(RandomFanaticPolicy),
				typeof(WeightedFanaticPolicy),
				typeof(OneRandomFanaticPolicy)
			};
		}
	}
}
