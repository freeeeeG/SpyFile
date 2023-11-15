using System;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CAB RID: 3243
	public sealed class ChosenWarriorsArmorComponent : AbilityComponent<ChosenWarriorsArmor>, IStackable
	{
		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x060041E9 RID: 16873 RVA: 0x000BFE0D File Offset: 0x000BE00D
		// (set) Token: 0x060041EA RID: 16874 RVA: 0x000BFE1C File Offset: 0x000BE01C
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
