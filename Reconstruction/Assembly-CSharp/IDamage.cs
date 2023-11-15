using System;
using UnityEngine;

// Token: 0x0200011E RID: 286
public interface IDamage
{
	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06000732 RID: 1842
	// (set) Token: 0x06000733 RID: 1843
	Collider2D TargetCollider { get; set; }

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06000734 RID: 1844
	// (set) Token: 0x06000735 RID: 1845
	SpriteRenderer gfxSprite { get; set; }

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06000736 RID: 1846
	// (set) Token: 0x06000737 RID: 1847
	DamageStrategy DamageStrategy { get; set; }

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06000738 RID: 1848
	// (set) Token: 0x06000739 RID: 1849
	HealthBar_Sprie HealthBar { get; set; }
}
