using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Constraints
{
	// Token: 0x02000C28 RID: 3112
	public abstract class Constraint : MonoBehaviour
	{
		// Token: 0x06004004 RID: 16388
		public abstract bool Pass();

		// Token: 0x06004005 RID: 16389 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x0400314B RID: 12619
		public static readonly Type[] types = new Type[]
		{
			typeof(LetterBox),
			typeof(Dialogue),
			typeof(Story),
			typeof(EndingCredit)
		};

		// Token: 0x02000C29 RID: 3113
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06004008 RID: 16392 RVA: 0x000BA028 File Offset: 0x000B8228
			public SubcomponentAttribute() : base(true, Constraint.types)
			{
			}
		}

		// Token: 0x02000C2A RID: 3114
		[Serializable]
		public class Subcomponents : SubcomponentArray<Constraint>
		{
			// Token: 0x06004009 RID: 16393 RVA: 0x000BA036 File Offset: 0x000B8236
			public bool Pass()
			{
				return base.components.Pass();
			}
		}
	}
}
