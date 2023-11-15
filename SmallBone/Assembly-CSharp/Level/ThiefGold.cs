using System;
using UnityEngine;

namespace Level
{
	// Token: 0x0200052E RID: 1326
	public class ThiefGold : MonoBehaviour
	{
		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06001A0F RID: 6671 RVA: 0x00051BE0 File Offset: 0x0004FDE0
		// (remove) Token: 0x06001A10 RID: 6672 RVA: 0x00051C14 File Offset: 0x0004FE14
		public static event ThiefGold.OnDespawn onDespawn;

		// Token: 0x06001A11 RID: 6673 RVA: 0x00051C47 File Offset: 0x0004FE47
		private void OnDisable()
		{
			ThiefGold.OnDespawn onDespawn = ThiefGold.onDespawn;
			if (onDespawn == null)
			{
				return;
			}
			onDespawn((double)this._goldParticle.currencyAmount, base.transform.position);
		}

		// Token: 0x040016CF RID: 5839
		[SerializeField]
		private CurrencyParticle _goldParticle;

		// Token: 0x0200052F RID: 1327
		// (Invoke) Token: 0x06001A14 RID: 6676
		public delegate void OnDespawn(double goldAmount, Vector3 position);
	}
}
