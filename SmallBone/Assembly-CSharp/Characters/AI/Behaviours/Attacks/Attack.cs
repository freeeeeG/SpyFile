using System;
using UnityEditor;

namespace Characters.AI.Behaviours.Attacks
{
	// Token: 0x020013DA RID: 5082
	public abstract class Attack : Behaviour
	{
		// Token: 0x040050C0 RID: 20672
		protected bool gaveDamage;

		// Token: 0x020013DB RID: 5083
		[AttributeUsage(AttributeTargets.Field)]
		public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06006424 RID: 25636 RVA: 0x001228EC File Offset: 0x00120AEC
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, Attack.SubcomponentAttribute.types)
			{
			}

			// Token: 0x040050C1 RID: 20673
			public new static readonly Type[] types = new Type[]
			{
				typeof(ActionAttack),
				typeof(CircularProjectileAttack),
				typeof(HorizontalProjectileAttack),
				typeof(MultiCircularProjectileAttack)
			};
		}
	}
}
