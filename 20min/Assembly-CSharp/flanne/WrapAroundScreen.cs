using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000143 RID: 323
	public class WrapAroundScreen : MonoBehaviour
	{
		// Token: 0x0600086A RID: 2154 RVA: 0x00023A5D File Offset: 0x00021C5D
		private void Start()
		{
			this.mainCamera = Camera.main;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00023A6A File Offset: 0x00021C6A
		private void Update()
		{
			if (this._ctr < this.numWraps)
			{
				this.WarpAround();
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00023A80 File Offset: 0x00021C80
		private void OnDisable()
		{
			this._ctr = 0;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00023A8C File Offset: 0x00021C8C
		private void WarpAround()
		{
			Vector3 vector = this.mainCamera.WorldToViewportPoint(base.transform.position);
			bool flag = false;
			if ((double)vector.x < -0.1)
			{
				flag = true;
				vector.x = 1f;
			}
			else if ((double)vector.x > 1.1)
			{
				flag = true;
				vector.x = 0f;
			}
			if ((double)vector.y < -0.1)
			{
				flag = true;
				vector.y = 1f;
			}
			else if ((double)vector.y > 1.1)
			{
				flag = true;
				vector.y = 0f;
			}
			if (flag)
			{
				base.transform.position = this.mainCamera.ViewportToWorldPoint(vector);
				this._ctr++;
			}
		}

		// Token: 0x04000637 RID: 1591
		[SerializeField]
		private int numWraps = 2;

		// Token: 0x04000638 RID: 1592
		private Camera mainCamera;

		// Token: 0x04000639 RID: 1593
		private int _ctr;
	}
}
