using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000730 RID: 1840
	public sealed class SpriteLaser : Laser
	{
		// Token: 0x0600256A RID: 9578 RVA: 0x00070A34 File Offset: 0x0006EC34
		public override void Activate(Vector2 direction)
		{
			this._direction = direction;
			this._directionDegree = Mathf.Atan2(this._direction.y, this._direction.x) * 57.29578f;
			this.UpdateLaser();
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x00070A84 File Offset: 0x0006EC84
		public override void Activate(float direction)
		{
			this._direction = Quaternion.Euler(0f, 0f, direction) * Vector2.right;
			this._directionDegree = direction;
			this.UpdateLaser();
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x00070AD4 File Offset: 0x0006ECD4
		public override void Deactivate()
		{
			base.gameObject.SetActive(false);
			this._fireEffect.gameObject.SetActive(false);
			if (this._hitEffect != null)
			{
				this._hitEffect.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x00070B12 File Offset: 0x0006ED12
		private void Update()
		{
			if (this._selfUpdate)
			{
				this.UpdateLaser();
			}
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x00070B24 File Offset: 0x0006ED24
		private void UpdateLaser()
		{
			RaycastHit2D hit = default(RaycastHit2D);
			float z = this._directionDegree;
			if (this._flipByLookinDirection && this._owner.lookingDirection == Character.LookingDirection.Left)
			{
				z = 180f - this._directionDegree;
			}
			if (TargetFinder.RayCast(this._originTransform.position, Quaternion.Euler(0f, 0f, z) * Vector2.right, this._maxLength, this._terrainLayer, ref hit))
			{
				this.UpdateTransform(hit);
				return;
			}
			this.UpdateToDefault();
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x00070BBC File Offset: 0x0006EDBC
		private void UpdateTransform(RaycastHit2D hit)
		{
			float num = Vector2.Distance(this._originTransform.position, hit.point);
			this._fireEffect.localRotation = Quaternion.Euler(0f, 0f, this._directionDegree);
			this._fireEffect.gameObject.SetActive(true);
			if (num < this._minSize)
			{
				num = this._minSize;
			}
			this._body.localScale = new Vector2(num, 1f);
			this._body.localRotation = Quaternion.Euler(0f, 0f, this._directionDegree);
			float z = Mathf.Atan2(hit.normal.y, hit.normal.x) * 57.29578f;
			Vector2 v = hit.point;
			if (num <= this._minSize)
			{
				int num2 = (this._flipByLookinDirection && this._owner.lookingDirection == Character.LookingDirection.Left) ? -1 : 1;
				Vector2 b = this._direction * num;
				b.x *= (float)num2;
				v = this._originTransform.position + b;
			}
			this._hitEffect.transform.SetPositionAndRotation(v, Quaternion.Euler(0f, 0f, z));
			this._hitEffect.transform.localScale = Vector2.one;
			this._hitEffect.gameObject.SetActive(true);
			base.transform.position = this._originTransform.position;
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x00070D4C File Offset: 0x0006EF4C
		private void UpdateToDefault()
		{
			Vector2 direction = this._direction;
			if (this._flipByLookinDirection && this._owner.lookingDirection == Character.LookingDirection.Left)
			{
				direction = new Vector2(-this._direction.x, this._direction.y);
			}
			this._fireEffect.localRotation = Quaternion.Euler(0f, 0f, this._directionDegree);
			this._fireEffect.gameObject.SetActive(true);
			this._body.localScale = new Vector2(this._maxLength, 1f);
			this._body.localRotation = Quaternion.Euler(0f, 0f, this._directionDegree);
			this._hitEffect.transform.SetPositionAndRotation(this._originTransform.position + direction * this._maxLength, Quaternion.identity);
			this._hitEffect.transform.localScale = Vector2.one;
			this._hitEffect.gameObject.SetActive(true);
			base.transform.position = this._originTransform.position;
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x00070E7C File Offset: 0x0006F07C
		private void OnDestroy()
		{
			if (this._hitEffect != null)
			{
				UnityEngine.Object.Destroy(this._hitEffect.gameObject);
			}
		}

		// Token: 0x04001FC4 RID: 8132
		[SerializeField]
		private Transform _fireEffect;

		// Token: 0x04001FC5 RID: 8133
		[SerializeField]
		private Transform _body;

		// Token: 0x04001FC6 RID: 8134
		[SerializeField]
		private Transform _hitEffect;

		// Token: 0x04001FC7 RID: 8135
		[SerializeField]
		private bool _flipByLookinDirection;

		// Token: 0x04001FC8 RID: 8136
		[SerializeField]
		private float _minSize;

		// Token: 0x04001FC9 RID: 8137
		[SerializeField]
		private bool _selfUpdate = true;

		// Token: 0x04001FCA RID: 8138
		private float _directionDegree;
	}
}
