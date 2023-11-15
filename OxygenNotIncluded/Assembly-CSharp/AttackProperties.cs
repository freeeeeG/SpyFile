using System;
using System.Collections.Generic;

// Token: 0x020006E3 RID: 1763
[Serializable]
public class AttackProperties
{
	// Token: 0x04001C82 RID: 7298
	public Weapon attacker;

	// Token: 0x04001C83 RID: 7299
	public AttackProperties.DamageType damageType;

	// Token: 0x04001C84 RID: 7300
	public AttackProperties.TargetType targetType;

	// Token: 0x04001C85 RID: 7301
	public float base_damage_min;

	// Token: 0x04001C86 RID: 7302
	public float base_damage_max;

	// Token: 0x04001C87 RID: 7303
	public int maxHits;

	// Token: 0x04001C88 RID: 7304
	public float aoe_radius = 2f;

	// Token: 0x04001C89 RID: 7305
	public List<AttackEffect> effects;

	// Token: 0x0200142D RID: 5165
	public enum DamageType
	{
		// Token: 0x04006493 RID: 25747
		Standard
	}

	// Token: 0x0200142E RID: 5166
	public enum TargetType
	{
		// Token: 0x04006495 RID: 25749
		Single,
		// Token: 0x04006496 RID: 25750
		AreaOfEffect
	}
}
