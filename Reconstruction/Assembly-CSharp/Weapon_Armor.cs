using System;
using UnityEngine;

// Token: 0x020000E3 RID: 227
public class Weapon_Armor : Weapon
{
	// Token: 0x060005A5 RID: 1445 RVA: 0x0000F688 File Offset: 0x0000D888
	protected override void TriggerWeapon()
	{
		base.TriggerWeapon();
		if (base.Knight.HoldingArmor != null)
		{
			Object.Destroy(base.Knight.HoldingArmor.gameObject);
			base.Knight.HoldingArmor = null;
		}
		ArmorHolder armorHolder = Object.Instantiate<ArmorHolder>(this.armorHolderPrefab, base.Knight.gfxSprite.transform);
		armorHolder.Initialize(base.Knight, base.Knight.DamageStrategy.MaxHealth * this.armorIntensify);
		base.Knight.HoldingArmor = armorHolder;
	}

	// Token: 0x0400026E RID: 622
	[SerializeField]
	private ArmorHolder armorHolderPrefab;

	// Token: 0x0400026F RID: 623
	[SerializeField]
	private float armorIntensify;
}
