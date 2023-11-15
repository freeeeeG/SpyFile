using System;
using System.Collections;
using UnityEngine;

namespace Characters.Projectiles
{
	// Token: 0x0200075C RID: 1884
	[RequireComponent(typeof(PoolObject))]
	public class DeflectedProjectile : MonoBehaviour
	{
		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002696 RID: 9878 RVA: 0x00074932 File Offset: 0x00072B32
		public PoolObject reusable
		{
			get
			{
				return this._reusable;
			}
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x0007493C File Offset: 0x00072B3C
		public void Deflect(Vector2 direction, Sprite projectileSprite, Vector2 scale, float speed)
		{
			this._direction.x = direction.x * -1f;
			this._direction.y = UnityEngine.Random.Range(this._yAngleRange.x, this._yAngleRange.y);
			this._speed = ((speed < this._minimumSpeed) ? this._minimumSpeed : speed);
			this._renderer.sprite = projectileSprite;
			base.transform.localScale = scale;
			float z = Mathf.Atan2(this._direction.y, this._direction.x) * 57.29578f;
			base.transform.rotation = Quaternion.Euler(0f, 0f, z);
			base.StartCoroutine(this.CDespawn());
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x00074A07 File Offset: 0x00072C07
		private void Update()
		{
			base.transform.Translate(this._direction * this._speed * this._speedMultiplier, Space.World);
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x00074A36 File Offset: 0x00072C36
		private IEnumerator CDespawn()
		{
			yield return Chronometer.global.WaitForSeconds(this._duration);
			this._reusable.Despawn();
			yield break;
		}

		// Token: 0x04002112 RID: 8466
		[GetComponent]
		[SerializeField]
		private PoolObject _reusable;

		// Token: 0x04002113 RID: 8467
		[SerializeField]
		private SpriteRenderer _renderer;

		// Token: 0x04002114 RID: 8468
		[SerializeField]
		private float _duration;

		// Token: 0x04002115 RID: 8469
		[SerializeField]
		private float _minimumSpeed = 0.35f;

		// Token: 0x04002116 RID: 8470
		[SerializeField]
		private float _speedMultiplier = 1f;

		// Token: 0x04002117 RID: 8471
		private float _speed;

		// Token: 0x04002118 RID: 8472
		private Vector2 _direction;

		// Token: 0x04002119 RID: 8473
		[SerializeField]
		private Vector2 _yAngleRange = new Vector2(-0.1f, 0.1f);
	}
}
