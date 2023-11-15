using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001AB RID: 427
	public class DealBulletDamageAction : Action
	{
		// Token: 0x060009F6 RID: 2550 RVA: 0x00027714 File Offset: 0x00025914
		public override void Activate(GameObject target)
		{
			Health component = target.GetComponent<Health>();
			PlayerController instance = PlayerController.Instance;
			int damage = Mathf.FloorToInt(this.damageMulti * instance.gun.damage);
			component.TakeDamage(this.damageType, damage, this.damageMulti);
		}

		// Token: 0x0400070B RID: 1803
		[SerializeField]
		private DamageType damageType;

		// Token: 0x0400070C RID: 1804
		[SerializeField]
		private float damageMulti = 1f;
	}
}
