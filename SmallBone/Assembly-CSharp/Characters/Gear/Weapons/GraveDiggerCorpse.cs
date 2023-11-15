using System;
using System.Collections;
using Characters.Abilities.Customs;
using Characters.Monsters;
using FX;
using Level;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Weapons
{
	// Token: 0x02000823 RID: 2083
	public class GraveDiggerCorpse : DestructibleObject
	{
		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06002B01 RID: 11009 RVA: 0x00084868 File Offset: 0x00082A68
		public Monster minion
		{
			get
			{
				return this._minion;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06002B02 RID: 11010 RVA: 0x00084870 File Offset: 0x00082A70
		public override Collider2D collider
		{
			get
			{
				return this._collider;
			}
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x00084878 File Offset: 0x00082A78
		public void SetPassive(GraveDiggerPassive graveDiggerPassive, Character owner)
		{
			this._graveDiggerPassive = graveDiggerPassive;
			this._syncWithOwner.Synchronize(this._minion.character, owner);
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x00084898 File Offset: 0x00082A98
		private void OnEnable()
		{
			this._remainHealth = this._health;
			this._spriteRenderer.color = this._endColor;
			this._remainLifetime = (float)this._lifetime;
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x000848C4 File Offset: 0x00082AC4
		private void Update()
		{
			this._remainLifetime -= Chronometer.global.deltaTime;
			if (this._remainLifetime <= 0f)
			{
				this._minion.Despawn();
			}
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x000848F8 File Offset: 0x00082AF8
		public override void Hit(Character from, ref Damage damage, Vector2 force)
		{
			this._remainHealth--;
			if (this._hitSound != null)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._hitSound, base.transform.position);
			}
			if (this._remainHealth <= 0)
			{
				this._graveDiggerPassive.HandleCorpseDie(base.transform.position);
				this._minion.character.health.Kill();
				return;
			}
			this._cEaseColorReference.Stop();
			this._cEaseColorReference = this.StartCoroutineWithReference(this.CEaseColor());
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x00084989 File Offset: 0x00082B89
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

		// Token: 0x0400249F RID: 9375
		[SerializeField]
		private Monster _minion;

		// Token: 0x040024A0 RID: 9376
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x040024A1 RID: 9377
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x040024A2 RID: 9378
		[Space]
		[SerializeField]
		private int _lifetime;

		// Token: 0x040024A3 RID: 9379
		[SerializeField]
		[Header("체력 (타격횟수)")]
		private int _health;

		// Token: 0x040024A4 RID: 9380
		[SerializeField]
		[Header("Hit Effect")]
		private SoundInfo _hitSound;

		// Token: 0x040024A5 RID: 9381
		[SerializeField]
		[Space]
		private Color _startColor;

		// Token: 0x040024A6 RID: 9382
		[SerializeField]
		private Color _endColor;

		// Token: 0x040024A7 RID: 9383
		[SerializeField]
		private Curve _hitColorCurve;

		// Token: 0x040024A8 RID: 9384
		private CoroutineReference _cEaseColorReference;

		// Token: 0x040024A9 RID: 9385
		[SerializeField]
		private CharacterSynchronization _syncWithOwner;

		// Token: 0x040024AA RID: 9386
		private float _remainLifetime;

		// Token: 0x040024AB RID: 9387
		private int _remainHealth;

		// Token: 0x040024AC RID: 9388
		private GraveDiggerPassive _graveDiggerPassive;
	}
}
