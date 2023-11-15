using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000124 RID: 292
	public class ProjectileSummon : Summon
	{
		// Token: 0x060007F9 RID: 2041 RVA: 0x00021E10 File Offset: 0x00020010
		protected override void Init()
		{
			SeekEnemy component = base.GetComponent<SeekEnemy>();
			if (component != null)
			{
				component.player = this.player.transform;
			}
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00021E3E File Offset: 0x0002003E
		private void OnEnable()
		{
			base.StartCoroutine(this.WaitToUnParentCR());
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00021E50 File Offset: 0x00020050
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.tag == "Enemy")
			{
				Health component = other.gameObject.GetComponent<Health>();
				if (component != null)
				{
					component.HPChange(Mathf.FloorToInt(-1f * base.summonDamageMod.Modify((float)this.baseDamage)));
				}
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00021EB8 File Offset: 0x000200B8
		private IEnumerator WaitToUnParentCR()
		{
			yield return null;
			base.transform.SetParent(null);
			yield break;
		}

		// Token: 0x040005D0 RID: 1488
		[SerializeField]
		private int baseDamage = 1;
	}
}
