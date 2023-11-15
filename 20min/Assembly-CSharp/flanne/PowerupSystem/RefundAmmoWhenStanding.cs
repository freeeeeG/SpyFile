using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.PowerupSystem
{
	// Token: 0x02000250 RID: 592
	public class RefundAmmoWhenStanding : MonoBehaviour
	{
		// Token: 0x06000CE4 RID: 3300 RVA: 0x0002F004 File Offset: 0x0002D204
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.myGun = componentInParent.gun;
			this.myGun.OnShoot.AddListener(new UnityAction(this.ChangeToRefundAmmo));
			this.ammo = base.transform.parent.GetComponentInChildren<Ammo>();
			this._lastFramePos = base.transform.position;
			this._thisFramePos = base.transform.position;
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0002F078 File Offset: 0x0002D278
		private void OnDestroy()
		{
			this.myGun.OnShoot.RemoveListener(new UnityAction(this.ChangeToRefundAmmo));
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0002F096 File Offset: 0x0002D296
		private void Update()
		{
			this._lastFramePos = this._thisFramePos;
			this._thisFramePos = base.transform.position;
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0002F0B5 File Offset: 0x0002D2B5
		private void ChangeToRefundAmmo()
		{
			if (this._lastFramePos == this._thisFramePos && Random.Range(0f, 1f) < this.chanceToRefund)
			{
				this.ammo.GainAmmo(1);
			}
		}

		// Token: 0x04000927 RID: 2343
		[Range(0f, 1f)]
		[SerializeField]
		private float chanceToRefund;

		// Token: 0x04000928 RID: 2344
		private Vector3 _lastFramePos;

		// Token: 0x04000929 RID: 2345
		private Vector3 _thisFramePos;

		// Token: 0x0400092A RID: 2346
		private Gun myGun;

		// Token: 0x0400092B RID: 2347
		private Ammo ammo;
	}
}
