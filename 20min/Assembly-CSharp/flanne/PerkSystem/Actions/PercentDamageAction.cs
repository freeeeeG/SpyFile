using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001C7 RID: 455
	public class PercentDamageAction : Action
	{
		// Token: 0x06000A38 RID: 2616 RVA: 0x00027EF8 File Offset: 0x000260F8
		public override void Activate(GameObject target)
		{
			Health component = target.GetComponent<Health>();
			int damage;
			if (target.tag.Contains("Champion") || target.tag.Contains("Passive"))
			{
				damage = Mathf.FloorToInt((float)component.maxHP * this.championPercentDamage);
			}
			else
			{
				damage = Mathf.FloorToInt((float)component.maxHP * this.percentDamage);
			}
			component.TakeDamage(this.damageType, damage, 1f);
		}

		// Token: 0x04000731 RID: 1841
		[SerializeField]
		private DamageType damageType;

		// Token: 0x04000732 RID: 1842
		[Range(0f, 1f)]
		[SerializeField]
		private float percentDamage;

		// Token: 0x04000733 RID: 1843
		[Range(0f, 1f)]
		[SerializeField]
		private float championPercentDamage;
	}
}
