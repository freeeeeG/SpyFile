using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
public static class TagLayerUtil
{
	// Token: 0x060003F8 RID: 1016 RVA: 0x0001530C File Offset: 0x0001350C
	public static void Init()
	{
		TagLayerUtil.FogOfWar = LayerMask.NameToLayer("FogOfWar");
		TagLayerUtil.VisibleFog = LayerMask.NameToLayer("VisibleFog");
		TagLayerUtil.Pickup = LayerMask.NameToLayer("Pickup");
		TagLayerUtil.PlayerPickupper = LayerMask.NameToLayer("PlayerPickupper");
		TagLayerUtil.Player = LayerMask.NameToLayer("Player");
		TagLayerUtil.PlayerProjectile = LayerMask.NameToLayer("PlayerProjectile");
		TagLayerUtil.PlayerProjectileMod = LayerMask.NameToLayer("PlayerProjectileMod");
		TagLayerUtil.Enemy = LayerMask.NameToLayer("Enemy");
		TagLayerUtil.EnemyProjectile = LayerMask.NameToLayer("EnemyProjectile");
	}

	// Token: 0x04000204 RID: 516
	public static LayerMask FogOfWar;

	// Token: 0x04000205 RID: 517
	public static LayerMask VisibleFog;

	// Token: 0x04000206 RID: 518
	public static LayerMask Pickup;

	// Token: 0x04000207 RID: 519
	public static LayerMask PlayerPickupper;

	// Token: 0x04000208 RID: 520
	public static LayerMask Player;

	// Token: 0x04000209 RID: 521
	public static LayerMask PlayerProjectile;

	// Token: 0x0400020A RID: 522
	public static LayerMask PlayerProjectileMod;

	// Token: 0x0400020B RID: 523
	public static LayerMask Enemy;

	// Token: 0x0400020C RID: 524
	public static LayerMask EnemyProjectile;
}
