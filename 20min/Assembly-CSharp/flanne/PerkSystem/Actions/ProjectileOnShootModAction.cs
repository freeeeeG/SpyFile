using System;
using flanne.PowerupSystem;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001DB RID: 475
	public class ProjectileOnShootModAction : Action
	{
		// Token: 0x06000A70 RID: 2672 RVA: 0x00028B66 File Offset: 0x00026D66
		public override void Activate(GameObject target)
		{
			ProjectileOnShoot component = GameObject.FindWithTag(this.tagName).GetComponent<ProjectileOnShoot>();
			component.numProjectiles += this.additionalProjectiles;
			component.periodicDamageFrequency /= this.periodicDamageFrequencyRateIncrease;
		}

		// Token: 0x04000779 RID: 1913
		[SerializeField]
		private string tagName;

		// Token: 0x0400077A RID: 1914
		[SerializeField]
		private int additionalProjectiles;

		// Token: 0x0400077B RID: 1915
		[SerializeField]
		private float periodicDamageFrequencyRateIncrease = 1f;
	}
}
