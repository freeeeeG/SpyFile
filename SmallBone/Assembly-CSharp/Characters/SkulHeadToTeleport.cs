using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200071F RID: 1823
	public class SkulHeadToTeleport : MonoBehaviour
	{
		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060024F0 RID: 9456 RVA: 0x0006F02D File Offset: 0x0006D22D
		// (set) Token: 0x060024F1 RID: 9457 RVA: 0x0006F034 File Offset: 0x0006D234
		public static SkulHeadToTeleport instance { get; private set; }

		// Token: 0x060024F2 RID: 9458 RVA: 0x0006F03C File Offset: 0x0006D23C
		private void OnEnable()
		{
			SkulHeadToTeleport.instance = this;
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x0006F044 File Offset: 0x0006D244
		public void Despawn()
		{
			this._poolObject.Despawn();
		}

		// Token: 0x04001F56 RID: 8022
		[SerializeField]
		private PoolObject _poolObject;
	}
}
