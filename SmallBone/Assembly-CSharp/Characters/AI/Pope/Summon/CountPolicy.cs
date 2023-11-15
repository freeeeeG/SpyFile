using System;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Pope.Summon
{
	// Token: 0x02001213 RID: 4627
	public abstract class CountPolicy : MonoBehaviour
	{
		// Token: 0x06005ACE RID: 23246
		public abstract int GetCount();

		// Token: 0x02001214 RID: 4628
		[AttributeUsage(AttributeTargets.Field)]
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06005AD0 RID: 23248 RVA: 0x0010D09F File Offset: 0x0010B29F
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, CountPolicy.SubcomponentAttribute.types)
			{
			}

			// Token: 0x04004949 RID: 18761
			public new static readonly Type[] types = new Type[]
			{
				typeof(ConstantCountPolicy),
				typeof(RadnomCountPolicy)
			};
		}
	}
}
