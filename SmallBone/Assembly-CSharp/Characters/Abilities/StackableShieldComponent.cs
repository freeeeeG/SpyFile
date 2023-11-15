using System;

namespace Characters.Abilities
{
	// Token: 0x02000ABE RID: 2750
	public sealed class StackableShieldComponent : AbilityComponent<StackableShield>, IStackable
	{
		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x0600389D RID: 14493 RVA: 0x000A6FA1 File Offset: 0x000A51A1
		// (set) Token: 0x0600389E RID: 14494 RVA: 0x000A6FB0 File Offset: 0x000A51B0
		public float stack
		{
			get
			{
				return base.baseAbility.amount;
			}
			set
			{
				Character componentInParent = base.GetComponentInParent<Character>();
				if (componentInParent == null)
				{
					base.baseAbility.amount = value;
					return;
				}
				base.baseAbility.Load(componentInParent, (int)value);
			}
		}
	}
}
