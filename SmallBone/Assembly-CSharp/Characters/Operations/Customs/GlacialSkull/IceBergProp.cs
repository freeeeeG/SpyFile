using System;
using System.Collections;
using Characters.Projectiles;
using FX;
using Level;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs.GlacialSkull
{
	// Token: 0x02001013 RID: 4115
	public class IceBergProp : DestructibleObject
	{
		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x06004F6E RID: 20334 RVA: 0x000EEF2C File Offset: 0x000ED12C
		// (remove) Token: 0x06004F6F RID: 20335 RVA: 0x000EEF64 File Offset: 0x000ED164
		public event IceBergProp.DidHitDelegate onDidHit;

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06004F70 RID: 20336 RVA: 0x000EEF99 File Offset: 0x000ED199
		public Key key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06004F71 RID: 20337 RVA: 0x000EEFA1 File Offset: 0x000ED1A1
		public override Collider2D collider
		{
			get
			{
				return this._collider;
			}
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x000EEFA9 File Offset: 0x000ED1A9
		private void Awake()
		{
			this.Initialize();
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x000EEFB1 File Offset: 0x000ED1B1
		private void OnEnable()
		{
			this._spriteRenderer.color = this._endColor;
		}

		// Token: 0x06004F74 RID: 20340 RVA: 0x000EEFC4 File Offset: 0x000ED1C4
		public void Initialize()
		{
			base.gameObject.SetActive(true);
			base.destroyed = false;
			this._targets = base.GetComponentsInChildren<Target>(true);
			this._collider = this._targets[0].collider;
			for (int i = 0; i < this._targets.Length; i++)
			{
				Target target = this._targets[i];
				target.collider.enabled = true;
				target.enabled = true;
			}
			if (this._animator != null)
			{
				this._animator.enabled = true;
				this._animator.Play(0, 0, 0f);
			}
			this._spriteRenderer.color = this._endColor;
		}

		// Token: 0x06004F75 RID: 20341 RVA: 0x000EF070 File Offset: 0x000ED270
		public override void Hit(Character owner, ref Damage damage, Vector2 force)
		{
			if (damage.motionType != Damage.MotionType.Basic)
			{
				return;
			}
			if (owner.type != Character.Type.Player)
			{
				return;
			}
			damage.Evaluate(false);
			IceBergProp.DidHitDelegate didHitDelegate = this.onDidHit;
			if (didHitDelegate != null)
			{
				didHitDelegate(owner, damage, force);
			}
			this.FireProjectile(owner, damage.hitPoint);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._hitSound, base.transform.position);
			this._cEaseColorReference.Stop();
			this._cEaseColorReference = this.StartCoroutineWithReference(this.CEaseColor());
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x000EF0F4 File Offset: 0x000ED2F4
		private void FireProjectile(Character owner, Vector2 firePosition)
		{
			int num = (owner.lookingDirection == Character.LookingDirection.Left) ? 180 : 0;
			firePosition += UnityEngine.Random.insideUnitCircle * this._positionCircleNoise;
			this._projectile.reusable.Spawn(firePosition, true).GetComponent<Projectile>().Fire(owner, this._projectileBaseDamage, (float)num + this._extraAngle.value, false, false, this._speedMultiplier.value, null, 0f);
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x000EF174 File Offset: 0x000ED374
		private IEnumerator CEaseColor()
		{
			float duration = this._hitColorCurve.duration;
			for (float time = 0f; time < duration; time += Chronometer.global.deltaTime)
			{
				this._spriteRenderer.color = Color.Lerp(this._startColor, this._endColor, this._hitColorCurve.Evaluate(time));
				yield return null;
			}
			this._spriteRenderer.color = this._endColor;
			yield break;
		}

		// Token: 0x04003FA0 RID: 16288
		[SerializeField]
		private Key _key = Key.SmallProp;

		// Token: 0x04003FA1 RID: 16289
		private Collider2D _collider;

		// Token: 0x04003FA2 RID: 16290
		[SerializeField]
		private Color _startColor;

		// Token: 0x04003FA3 RID: 16291
		[SerializeField]
		private Color _endColor;

		// Token: 0x04003FA4 RID: 16292
		[SerializeField]
		private Curve _hitColorCurve;

		// Token: 0x04003FA5 RID: 16293
		private CoroutineReference _cEaseColorReference;

		// Token: 0x04003FA6 RID: 16294
		[SerializeField]
		[GetComponent]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04003FA7 RID: 16295
		[GetComponent]
		[SerializeField]
		private Animator _animator;

		// Token: 0x04003FA8 RID: 16296
		[SerializeField]
		private SoundInfo _hitSound;

		// Token: 0x04003FA9 RID: 16297
		[SerializeField]
		[Header("Projectile")]
		private Projectile _projectile;

		// Token: 0x04003FAA RID: 16298
		[SerializeField]
		private float _projectileBaseDamage = 10f;

		// Token: 0x04003FAB RID: 16299
		[SerializeField]
		private CustomFloat _speedMultiplier = new CustomFloat(0.98f, 1.02f);

		// Token: 0x04003FAC RID: 16300
		[SerializeField]
		private CustomFloat _extraAngle = new CustomFloat(-2f, 2f);

		// Token: 0x04003FAD RID: 16301
		[SerializeField]
		private float _positionCircleNoise = 0.2f;

		// Token: 0x04003FAE RID: 16302
		private Target[] _targets;

		// Token: 0x02001014 RID: 4116
		// (Invoke) Token: 0x06004F7A RID: 20346
		public delegate void DidHitDelegate(Character owner, in Damage damage, Vector2 force);
	}
}
