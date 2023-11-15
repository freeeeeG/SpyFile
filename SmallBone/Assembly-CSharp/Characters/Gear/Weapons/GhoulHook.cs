using System;
using System.Collections;
using Characters.Movements;
using FX;
using PhysicsUtils;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Weapons
{
	// Token: 0x0200081F RID: 2079
	public class GhoulHook : MonoBehaviour
	{
		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06002ADC RID: 10972 RVA: 0x00083CDC File Offset: 0x00081EDC
		// (remove) Token: 0x06002ADD RID: 10973 RVA: 0x00083D14 File Offset: 0x00081F14
		public event Action onTerrainHit;

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06002ADE RID: 10974 RVA: 0x00083D4C File Offset: 0x00081F4C
		// (remove) Token: 0x06002ADF RID: 10975 RVA: 0x00083D84 File Offset: 0x00081F84
		public event Action onExpired;

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06002AE0 RID: 10976 RVA: 0x00083DBC File Offset: 0x00081FBC
		// (remove) Token: 0x06002AE1 RID: 10977 RVA: 0x00083DF4 File Offset: 0x00081FF4
		public event Action onPullEnd;

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x06002AE2 RID: 10978 RVA: 0x00083E2C File Offset: 0x0008202C
		// (remove) Token: 0x06002AE3 RID: 10979 RVA: 0x00083E64 File Offset: 0x00082064
		public event Action onFlyEnd;

		// Token: 0x06002AE4 RID: 10980 RVA: 0x00083E9C File Offset: 0x0008209C
		private void Awake()
		{
			this._chain.transform.parent = null;
			this._head.transform.parent = null;
			this._chain.gameObject.SetActive(false);
			this._head.gameObject.SetActive(false);
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x00083EF0 File Offset: 0x000820F0
		private void LateUpdate()
		{
			if (!this._chain.gameObject.activeSelf)
			{
				return;
			}
			this._chain.transform.position = this._origin.position;
			float num = Vector2.Distance(this._chain.transform.position, this._head.transform.position);
			Vector2 size = this._pullCollider.size;
			size.x = num + 0.5f;
			this._pullCollider.size = size;
			Vector2 offset = this._pullCollider.offset;
			offset.x = num * 0.5f;
			if (this._weapon.owner.lookingDirection == Character.LookingDirection.Left)
			{
				offset.x *= -1f;
			}
			this._pullCollider.offset = offset;
			size = this._chain.size;
			size.x = num;
			this._chain.size = size;
			Vector3 right = this._head.transform.position - this._chain.transform.position;
			this._chain.transform.right = right;
			this._pullCollider.transform.right = right;
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x00084034 File Offset: 0x00082234
		private void OnDisable()
		{
			this._weapon.owner.movement.configs.Remove(this._flyMovmentConfig);
			this._chain.gameObject.SetActive(false);
			this._head.gameObject.SetActive(false);
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x00084084 File Offset: 0x00082284
		private void OnDestroy()
		{
			UnityEngine.Object.Destroy(this._chain.gameObject);
			UnityEngine.Object.Destroy(this._head.gameObject);
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x000840A6 File Offset: 0x000822A6
		public void Fire()
		{
			base.StopCoroutine("CFire");
			base.StartCoroutine("CFire");
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x000840BF File Offset: 0x000822BF
		private IEnumerator CFire()
		{
			this._origin = this._fireOrigin;
			this._chain.gameObject.SetActive(true);
			this._head.gameObject.SetActive(true);
			this._head.transform.position = this._origin.transform.position;
			Vector3 lossyScale = this._weapon.owner.transform.lossyScale;
			this._head.transform.localScale = base.transform.lossyScale;
			float traveled = 0f;
			while (traveled < this._distance)
			{
				yield return null;
				this._caster.contactFilter.SetLayerMask(Layers.terrainMaskForProjectile);
				float num = this._speed * this._weapon.owner.chronometer.animation.deltaTime;
				traveled += num;
				if (this._weapon.owner.lookingDirection == Character.LookingDirection.Left)
				{
					num *= -1f;
				}
				Vector2 right = Vector2.right;
				this._caster.RayCast(this._head.transform.position, right, num);
				if (this._caster.results.Count > 0 && (this._caster.results[0].collider.gameObject.layer != 19 || traveled > this._minDistanceForPlatform))
				{
					this._head.transform.position = this._caster.results[0].point;
					yield return this._weapon.owner.chronometer.animation.WaitForSeconds(this._flyDelay);
					Action action = this.onTerrainHit;
					if (action != null)
					{
						action();
					}
					yield break;
				}
				this._head.transform.Translate(right * num);
			}
			yield return this._weapon.owner.chronometer.animation.WaitForSeconds(this._pullDelay);
			Action action2 = this.onExpired;
			if (action2 != null)
			{
				action2();
			}
			yield break;
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x000840CE File Offset: 0x000822CE
		public void Pull()
		{
			base.StopCoroutine("CPull");
			base.StartCoroutine("CPull");
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x000840E7 File Offset: 0x000822E7
		private IEnumerator CPull()
		{
			this._origin = this._pullOrigin;
			Vector3 headPosition = this._head.transform.position;
			float time = 0f;
			while (time < 1f)
			{
				yield return null;
				time += this._weapon.owner.chronometer.animation.deltaTime * this._pullSpeed;
				this._head.transform.position = Vector2.LerpUnclamped(headPosition, this._origin.transform.position, time);
			}
			this._chain.gameObject.SetActive(false);
			this._head.gameObject.SetActive(false);
			Action action = this.onPullEnd;
			if (action != null)
			{
				action();
			}
			yield break;
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x000840F6 File Offset: 0x000822F6
		public void Fly()
		{
			base.StopCoroutine("CFly");
			base.StartCoroutine("CFly");
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x0008410F File Offset: 0x0008230F
		private IEnumerator CFly()
		{
			this._origin = this._flyOrigin;
			this._weapon.owner.movement.configs.Add(2147483646, this._flyMovmentConfig);
			float time = 0f;
			while (time < this._flyTimeout)
			{
				yield return new WaitForEndOfFrame();
				float deltaTime = this._weapon.owner.chronometer.animation.deltaTime;
				time += deltaTime;
				this._caster.contactFilter.SetLayerMask(Layers.terrainMask);
				float num = this._flySpeed * deltaTime;
				Vector3 vector = this._head.transform.position - this._weapon.hitbox.bounds.center;
				vector.Normalize();
				this._caster.ColliderCast(this._weapon.hitbox, vector, num);
				Vector3 vector2 = vector * num;
				Vector3 vector3 = this._weapon.owner.transform.position + vector2;
				if ((vector.x > 0f && vector3.x - this._head.transform.position.x > 0f) || (vector.x < 0f && vector3.x - this._head.transform.position.x < 0f) || this._caster.results.Count > 0)
				{
					this._head.transform.position = this._caster.results[0].point;
					PersistentSingleton<SoundManager>.Instance.PlaySound(this._flyEndSound, base.transform.position);
					this._weapon.owner.movement.configs.Remove(this._flyMovmentConfig);
					this._weapon.owner.movement.verticalVelocity = this._flyEndVerticalVelocity;
					this._chain.gameObject.SetActive(false);
					this._head.gameObject.SetActive(false);
					Action action = this.onFlyEnd;
					if (action != null)
					{
						action();
					}
					yield break;
				}
				this._weapon.owner.movement.force = vector2;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._flyEndSound, base.transform.position);
			this._weapon.owner.movement.configs.Remove(this._flyMovmentConfig);
			this._chain.gameObject.SetActive(false);
			this._head.gameObject.SetActive(false);
			Action action2 = this.onFlyEnd;
			if (action2 != null)
			{
				action2();
			}
			yield break;
		}

		// Token: 0x0400247E RID: 9342
		private NonAllocCaster _caster = new NonAllocCaster(1);

		// Token: 0x0400247F RID: 9343
		[SerializeField]
		private Weapon _weapon;

		// Token: 0x04002480 RID: 9344
		[Space]
		[SerializeField]
		private Transform _fireOrigin;

		// Token: 0x04002481 RID: 9345
		[SerializeField]
		private Transform _pullOrigin;

		// Token: 0x04002482 RID: 9346
		[SerializeField]
		private Transform _flyOrigin;

		// Token: 0x04002483 RID: 9347
		[SerializeField]
		[Space]
		private SpriteRenderer _chain;

		// Token: 0x04002484 RID: 9348
		[SerializeField]
		private SpriteRenderer _head;

		// Token: 0x04002485 RID: 9349
		[SerializeField]
		[Header("Fire")]
		private float _speed;

		// Token: 0x04002486 RID: 9350
		[SerializeField]
		private float _distance;

		// Token: 0x04002487 RID: 9351
		[SerializeField]
		private float _minDistanceForPlatform;

		// Token: 0x04002488 RID: 9352
		[Header("Pull")]
		[SerializeField]
		private float _pullDelay;

		// Token: 0x04002489 RID: 9353
		[SerializeField]
		private float _pullSpeed;

		// Token: 0x0400248A RID: 9354
		[Tooltip("Pull Collider의 너비와 각도는 체인에 맞게 자동으로 조정됩니다. 높이만 설정하세요.")]
		[SerializeField]
		private BoxCollider2D _pullCollider;

		// Token: 0x0400248B RID: 9355
		[SerializeField]
		[Header("Fly")]
		private float _flyDelay;

		// Token: 0x0400248C RID: 9356
		[SerializeField]
		private float _flySpeed;

		// Token: 0x0400248D RID: 9357
		[Tooltip("Fly 상태가 지속될 수 있는 최대 시간입니다. 이 시간이 넘어가면 도착여부에 관계없이 Fly가 끝납니다.")]
		[SerializeField]
		private float _flyTimeout;

		// Token: 0x0400248E RID: 9358
		[Tooltip("Fly가 끝날 때 Vertical Velocity를 몇으로 설정하지를 정합니다. Fly가 끝나면서 살짝 뛰어오르는 연출을 위해 사용합니다.")]
		[SerializeField]
		private float _flyEndVerticalVelocity;

		// Token: 0x0400248F RID: 9359
		[SerializeField]
		[Tooltip("Fly가 끝날 때 사운드를 재생합니다.")]
		private SoundInfo _flyEndSound;

		// Token: 0x04002490 RID: 9360
		[SerializeField]
		private Movement.Config _flyMovmentConfig;

		// Token: 0x04002491 RID: 9361
		private Transform _origin;
	}
}
