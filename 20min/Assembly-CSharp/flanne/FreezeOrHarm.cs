using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000098 RID: 152
	public class FreezeOrHarm : Harmful
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x0001A2EC File Offset: 0x000184EC
		private void Start()
		{
			this.freezeSys = FreezeSystem.SharedInstance;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001A2FC File Offset: 0x000184FC
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains(this.hitTag))
			{
				if (!this.freezeSys.IsFrozen(other.gameObject))
				{
					this.freezeSys.Freeze(other.gameObject);
					return;
				}
				Health component = other.gameObject.GetComponent<Health>();
				if (component != null)
				{
					component.TakeDamage(DamageType.None, this.damageAmount, 1f);
				}
			}
		}

		// Token: 0x0400035E RID: 862
		[SerializeField]
		private string hitTag;

		// Token: 0x0400035F RID: 863
		private FreezeSystem freezeSys;
	}
}
