using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000B4 RID: 180
	public class TimeToLive : MonoBehaviour
	{
		// Token: 0x060005C6 RID: 1478 RVA: 0x0001B2E8 File Offset: 0x000194E8
		public void Refresh()
		{
			base.CancelInvoke();
			base.Invoke("Deactivate", this.lifetime);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001B301 File Offset: 0x00019501
		private void OnEnable()
		{
			base.Invoke("Deactivate", this.lifetime);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001B314 File Offset: 0x00019514
		private void Deactivate()
		{
			if (this.willDestroy)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001B336 File Offset: 0x00019536
		private void OnDisable()
		{
			base.CancelInvoke();
		}

		// Token: 0x040003A8 RID: 936
		[SerializeField]
		private float lifetime;

		// Token: 0x040003A9 RID: 937
		[SerializeField]
		private bool willDestroy;
	}
}
