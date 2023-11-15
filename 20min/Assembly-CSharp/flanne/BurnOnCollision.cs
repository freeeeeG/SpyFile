using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000D3 RID: 211
	public class BurnOnCollision : MonoBehaviour
	{
		// Token: 0x0600067E RID: 1662 RVA: 0x0001D7B2 File Offset: 0x0001B9B2
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains(this.hitTag))
			{
				BurnSystem.SharedInstance.Burn(other.gameObject, this.burnDamage);
			}
		}

		// Token: 0x0400044C RID: 1100
		public string hitTag;

		// Token: 0x0400044D RID: 1101
		public int burnDamage;
	}
}
