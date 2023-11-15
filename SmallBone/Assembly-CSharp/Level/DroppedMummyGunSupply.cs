using System;
using System.Collections;
using UnityEngine;

namespace Level
{
	// Token: 0x020004C4 RID: 1220
	public class DroppedMummyGunSupply : MonoBehaviour
	{
		// Token: 0x060017A1 RID: 6049 RVA: 0x0004A242 File Offset: 0x00048442
		private void OnDisable()
		{
			if (this._gun == null)
			{
				return;
			}
			this._gun.onPickedUp -= this.OnGunPickedUp;
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x0004A26A File Offset: 0x0004846A
		public void Spawn(DroppedMummyGun droppedMummyGun, Vector3 position, float targetY)
		{
			DroppedMummyGunSupply component = this._poolObject.Spawn(position, true).GetComponent<DroppedMummyGunSupply>();
			component.Clear();
			component.Initialize(droppedMummyGun, targetY);
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x0004A28C File Offset: 0x0004848C
		private void Clear()
		{
			foreach (object obj in base.transform)
			{
				DroppedMummyGun component = ((Transform)obj).GetComponent<DroppedMummyGun>();
				if (component != null)
				{
					component.Despawn();
				}
			}
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x0004A2F4 File Offset: 0x000484F4
		private void Initialize(DroppedMummyGun droppedMummyGun, float targetY)
		{
			this._gun = droppedMummyGun;
			this._gun.onPickedUp -= this.OnGunPickedUp;
			this._gun.onPickedUp += this.OnGunPickedUp;
			this._gun.transform.SetParent(base.transform, false);
			this._gun.transform.localPosition = Vector3.zero;
			this._rigidbodyConstraints = this._gun.rigidbody.constraints;
			this._gun.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
			this._targetY = targetY;
			this._parachuteRenderer.enabled = true;
			base.StartCoroutine(this.CFall());
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x0004A3A9 File Offset: 0x000485A9
		private void OnGunPickedUp()
		{
			this._gun.onPickedUp -= this.OnGunPickedUp;
			this._gun.rigidbody.constraints = this._rigidbodyConstraints;
			this._poolObject.Despawn();
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x0004A3E3 File Offset: 0x000485E3
		private IEnumerator CFall()
		{
			do
			{
				yield return null;
				base.transform.Translate(0f, -this._fallSpeed * Chronometer.global.deltaTime, 0f);
			}
			while (base.transform.position.y >= this._targetY);
			Vector3 position = base.transform.position;
			position.y = this._targetY;
			base.transform.position = position;
			this._parachuteRenderer.enabled = false;
			yield break;
		}

		// Token: 0x040014A6 RID: 5286
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x040014A7 RID: 5287
		[SerializeField]
		private SpriteRenderer _parachuteRenderer;

		// Token: 0x040014A8 RID: 5288
		[SerializeField]
		private float _fallSpeed;

		// Token: 0x040014A9 RID: 5289
		private DroppedMummyGun _gun;

		// Token: 0x040014AA RID: 5290
		private float _targetY;

		// Token: 0x040014AB RID: 5291
		private RigidbodyConstraints2D _rigidbodyConstraints;
	}
}
