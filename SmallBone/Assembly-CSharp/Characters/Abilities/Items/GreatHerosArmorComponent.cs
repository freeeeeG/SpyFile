using System;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CD0 RID: 3280
	public sealed class GreatHerosArmorComponent : AbilityComponent<GreatHerosArmor>, IAttackDamage
	{
		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06004270 RID: 17008 RVA: 0x000C17C9 File Offset: 0x000BF9C9
		// (set) Token: 0x06004271 RID: 17009 RVA: 0x000C17D1 File Offset: 0x000BF9D1
		public float amount { get; set; }
	}
}
