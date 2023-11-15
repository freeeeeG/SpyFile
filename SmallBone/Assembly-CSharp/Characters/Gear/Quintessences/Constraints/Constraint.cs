using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Quintessences.Constraints
{
	// Token: 0x020008F3 RID: 2291
	public abstract class Constraint : MonoBehaviour
	{
		// Token: 0x060030F8 RID: 12536
		public abstract bool Pass();

		// Token: 0x060030F9 RID: 12537 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x0400284E RID: 10318
		public static readonly Type[] types = new Type[]
		{
			typeof(EnemyWithinRangeConstraint),
			typeof(EnemyCountConstraint)
		};

		// Token: 0x020008F4 RID: 2292
		[AttributeUsage(AttributeTargets.Field)]
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x060030FC RID: 12540 RVA: 0x00092745 File Offset: 0x00090945
			public SubcomponentAttribute() : base(true, Constraint.types)
			{
			}
		}

		// Token: 0x020008F5 RID: 2293
		[Serializable]
		public class Subcomponents : SubcomponentArray<Constraint>
		{
		}
	}
}
