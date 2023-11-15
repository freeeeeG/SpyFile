using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000D9 RID: 217
	public class CurseAndHarmOnContact : MonoBehaviour
	{
		// Token: 0x06000694 RID: 1684 RVA: 0x0001DBEA File Offset: 0x0001BDEA
		private void Start()
		{
			this.CS = CurseSystem.Instance;
			this.player = PlayerController.Instance;
			this.playerGun = this.player.gun;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001DC14 File Offset: 0x0001BE14
		private void OnCollisionEnter2D(Collision2D other)
		{
			this.CS.Curse(other.gameObject);
			Health component = other.gameObject.GetComponent<Health>();
			if (component != null)
			{
				component.TakeDamage(DamageType.Bullet, Mathf.FloorToInt(this.multiplier * this.playerGun.damage), 1f);
			}
		}

		// Token: 0x04000465 RID: 1125
		public float multiplier = 1f;

		// Token: 0x04000466 RID: 1126
		private CurseSystem CS;

		// Token: 0x04000467 RID: 1127
		private PlayerController player;

		// Token: 0x04000468 RID: 1128
		private Gun playerGun;
	}
}
