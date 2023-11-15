using System;
using System.Collections;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x02000154 RID: 340
	[RequireComponent(typeof(Collider2D))]
	public class ProjectileShieldRune : Rune
	{
		// Token: 0x060008C6 RID: 2246 RVA: 0x00024D03 File Offset: 0x00022F03
		protected override void Init()
		{
			this.shieldCollider = base.GetComponent<Collider2D>();
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00024D11 File Offset: 0x00022F11
		private void OnCollisionEnter2D(Collision2D collision)
		{
			this.sprite.enabled = false;
			this.shieldCollider.enabled = false;
			base.StartCoroutine(this.WaitForCooldownCR());
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00024D38 File Offset: 0x00022F38
		private IEnumerator WaitForCooldownCR()
		{
			yield return new WaitForSeconds(this.baseCD - this.cdrPerLevel * (float)this.level);
			this.sprite.enabled = true;
			this.shieldCollider.enabled = true;
			yield break;
		}

		// Token: 0x0400067E RID: 1662
		[SerializeField]
		private float baseCD = 10f;

		// Token: 0x0400067F RID: 1663
		[SerializeField]
		private float cdrPerLevel = 1f;

		// Token: 0x04000680 RID: 1664
		[SerializeField]
		private SpriteRenderer sprite;

		// Token: 0x04000681 RID: 1665
		private Collider2D shieldCollider;
	}
}
