using System;
using System.Collections;
using System.Linq;
using Characters;
using FX;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level
{
	// Token: 0x0200050A RID: 1290
	public class Prop : DestructibleObject
	{
		// Token: 0x14000027 RID: 39
		// (add) Token: 0x0600196D RID: 6509 RVA: 0x0004FBD8 File Offset: 0x0004DDD8
		// (remove) Token: 0x0600196E RID: 6510 RVA: 0x0004FC10 File Offset: 0x0004DE10
		public event Prop.DidHitDelegate onDidHit;

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x0004FC45 File Offset: 0x0004DE45
		public Key key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001970 RID: 6512 RVA: 0x0004FC4D File Offset: 0x0004DE4D
		public int phase
		{
			get
			{
				return this._destructionPhase.current;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001971 RID: 6513 RVA: 0x0004FC5A File Offset: 0x0004DE5A
		public override Collider2D collider
		{
			get
			{
				return this._collider;
			}
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0004FC62 File Offset: 0x0004DE62
		private void Awake()
		{
			if (!this._countHealth)
			{
				this._health *= Singleton<Service>.Instance.levelManager.currentChapter.currentStage.healthMultiplier;
			}
			this.Initialize();
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x0004FC98 File Offset: 0x0004DE98
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
			this._destructionPhase.Initialize(this);
			this._spriteRenderer.sprite = this._destructionPhase.values[0].sprite;
			if (this._animator != null)
			{
				this._animator.enabled = true;
				this._animator.Play(0, 0, 0f);
			}
			this._spriteRenderer.color = this._endColor;
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0004FD6C File Offset: 0x0004DF6C
		public override void Hit(Character owner, ref Damage damage, Vector2 force)
		{
			damage.Evaluate(false);
			Prop.DestructionPhaseInfo.PhaseSprite phaseSprite = this._destructionPhase.TakeDamage(base.transform.position, owner, this._countHealth ? 1.0 : damage.amount, force);
			Prop.DidHitDelegate didHitDelegate = this.onDidHit;
			if (didHitDelegate != null)
			{
				didHitDelegate(owner, damage, force);
			}
			if (phaseSprite == null)
			{
				if (!base.destroyed)
				{
					base.destroyed = true;
					Action onDestroy = this._onDestroy;
					if (onDestroy != null)
					{
						onDestroy();
					}
					PersistentSingleton<SoundManager>.Instance.PlaySound(this._destroySound, base.transform.position);
					Key key = this.key;
					if (key != Key.SmallProp)
					{
						if (key == Key.LargeProp)
						{
							Settings.instance.largePropGoldPossibility.Drop(base.transform.position);
						}
					}
					else
					{
						Settings.instance.smallPropGoldPossibility.Drop(base.transform.position);
					}
					if (this._wreckage != null)
					{
						this.ChangeToWreck();
						return;
					}
					base.gameObject.SetActive(false);
					return;
				}
			}
			else
			{
				if (this._animator != null)
				{
					this._animator.runtimeAnimatorController = phaseSprite.animation;
				}
				this._spriteRenderer.sprite = phaseSprite.sprite;
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._hitSound, base.transform.position);
				this._cEaseColorReference.Stop();
				this._cEaseColorReference = this.StartCoroutineWithReference(this.CEaseColor());
			}
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x0004FEE4 File Offset: 0x0004E0E4
		private void ChangeToWreck()
		{
			for (int i = 0; i < this._targets.Length; i++)
			{
				Target target = this._targets[i];
				target.collider.enabled = false;
				target.enabled = false;
			}
			this._spriteRenderer.sprite = this._wreckage;
			if (this._animator != null)
			{
				this._animator.enabled = true;
				this._animator.Play(0, 0, 0f);
				this._animator.enabled = false;
			}
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x0004FF67 File Offset: 0x0004E167
		private IEnumerator CEaseColor()
		{
			float duration = this._hitColorCurve.duration;
			for (float time = 0f; time < duration; time += Chronometer.global.deltaTime)
			{
				this._spriteRenderer.color = Color.Lerp(this._startColor, this._endColor, this._hitColorCurve.Evaluate(time));
				yield return null;
			}
			this._spriteRenderer.color = this._endColor;
			if (this._wreckage == null && base.destroyed)
			{
				UnityEngine.Object.Destroy(this);
			}
			yield break;
		}

		// Token: 0x04001637 RID: 5687
		[SerializeField]
		private Key _key = Key.SmallProp;

		// Token: 0x04001638 RID: 5688
		[SerializeField]
		private bool _countHealth;

		// Token: 0x04001639 RID: 5689
		[SerializeField]
		private float _health;

		// Token: 0x0400163A RID: 5690
		private Collider2D _collider;

		// Token: 0x0400163B RID: 5691
		[SerializeField]
		private Color _startColor;

		// Token: 0x0400163C RID: 5692
		[SerializeField]
		private Color _endColor;

		// Token: 0x0400163D RID: 5693
		[SerializeField]
		private Curve _hitColorCurve;

		// Token: 0x0400163E RID: 5694
		private CoroutineReference _cEaseColorReference;

		// Token: 0x0400163F RID: 5695
		[SerializeField]
		[GetComponent]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04001640 RID: 5696
		[GetComponent]
		[SerializeField]
		private Animator _animator;

		// Token: 0x04001641 RID: 5697
		[SerializeField]
		private Prop.DestructionPhaseInfo _destructionPhase;

		// Token: 0x04001642 RID: 5698
		[SerializeField]
		private Sprite _wreckage;

		// Token: 0x04001643 RID: 5699
		[SerializeField]
		private SoundInfo _hitSound;

		// Token: 0x04001644 RID: 5700
		[SerializeField]
		private SoundInfo _destroySound;

		// Token: 0x04001645 RID: 5701
		private Target[] _targets;

		// Token: 0x0200050B RID: 1291
		[Serializable]
		public class DestructionPhaseInfo : ReorderableArray<Prop.DestructionPhaseInfo.PhaseSprite>
		{
			// Token: 0x17000521 RID: 1313
			// (get) Token: 0x06001978 RID: 6520 RVA: 0x0004FF86 File Offset: 0x0004E186
			// (set) Token: 0x06001979 RID: 6521 RVA: 0x0004FF8E File Offset: 0x0004E18E
			public int current { get; protected set; }

			// Token: 0x0600197A RID: 6522 RVA: 0x0004FF98 File Offset: 0x0004E198
			public void Initialize(Prop prop)
			{
				this._prop = prop;
				this._totalWeight = this.values.Sum((Prop.DestructionPhaseInfo.PhaseSprite v) => v.weight);
				this.current = 0;
				foreach (Prop.DestructionPhaseInfo.PhaseSprite phaseSprite in this.values)
				{
					phaseSprite.health = (double)(phaseSprite.weight / this._totalWeight * this._prop._health);
					if (phaseSprite.particleSpawnPoint == null)
					{
						phaseSprite.particleSpawnPoint = prop.transform;
					}
				}
			}

			// Token: 0x0600197B RID: 6523 RVA: 0x00050038 File Offset: 0x0004E238
			public Prop.DestructionPhaseInfo.PhaseSprite TakeDamage(Vector3 position, Character owner, double damage, Vector2 force)
			{
				while (damage > 0.0)
				{
					if (this.values.Length <= this.current)
					{
						return null;
					}
					Prop.DestructionPhaseInfo.PhaseSprite phaseSprite = this.values[this.current];
					if (phaseSprite.health > damage)
					{
						phaseSprite.health -= damage;
						break;
					}
					damage -= phaseSprite.health;
					if (phaseSprite.toDeactivate != null)
					{
						phaseSprite.toDeactivate.SetActive(false);
					}
					if (phaseSprite.particle != null)
					{
						phaseSprite.particle.Emit(phaseSprite.particleSpawnPoint.position, this._prop.collider.bounds, force, true);
					}
					int current = this.current;
					this.current = current + 1;
					if (this.values.Length <= this.current)
					{
						return null;
					}
				}
				return this.values[this.current];
			}

			// Token: 0x04001646 RID: 5702
			private Prop _prop;

			// Token: 0x04001647 RID: 5703
			private float _totalWeight;

			// Token: 0x0200050C RID: 1292
			[Serializable]
			public class PhaseSprite
			{
				// Token: 0x17000522 RID: 1314
				// (get) Token: 0x0600197D RID: 6525 RVA: 0x0005012C File Offset: 0x0004E32C
				public float weight
				{
					get
					{
						return this._weight;
					}
				}

				// Token: 0x17000523 RID: 1315
				// (get) Token: 0x0600197E RID: 6526 RVA: 0x00050134 File Offset: 0x0004E334
				public Sprite sprite
				{
					get
					{
						return this._sprite;
					}
				}

				// Token: 0x17000524 RID: 1316
				// (get) Token: 0x0600197F RID: 6527 RVA: 0x0005013C File Offset: 0x0004E33C
				public RuntimeAnimatorController animation
				{
					get
					{
						return this._animation;
					}
				}

				// Token: 0x17000525 RID: 1317
				// (get) Token: 0x06001980 RID: 6528 RVA: 0x00050144 File Offset: 0x0004E344
				public ParticleEffectInfo particle
				{
					get
					{
						return this._particle;
					}
				}

				// Token: 0x17000526 RID: 1318
				// (get) Token: 0x06001981 RID: 6529 RVA: 0x0005014C File Offset: 0x0004E34C
				public GameObject toDeactivate
				{
					get
					{
						return this._toDeactivate;
					}
				}

				// Token: 0x04001649 RID: 5705
				internal double health;

				// Token: 0x0400164A RID: 5706
				[SerializeField]
				private float _weight = 1f;

				// Token: 0x0400164B RID: 5707
				[SerializeField]
				private Sprite _sprite;

				// Token: 0x0400164C RID: 5708
				[SerializeField]
				private RuntimeAnimatorController _animation;

				// Token: 0x0400164D RID: 5709
				[SerializeField]
				private ParticleEffectInfo _particle;

				// Token: 0x0400164E RID: 5710
				[SerializeField]
				private GameObject _toDeactivate;

				// Token: 0x0400164F RID: 5711
				[SerializeField]
				[FormerlySerializedAs("_particleSpawnPoint")]
				public Transform particleSpawnPoint;
			}
		}

		// Token: 0x0200050E RID: 1294
		// (Invoke) Token: 0x06001987 RID: 6535
		public delegate void DidHitDelegate(Character owner, in Damage damage, Vector2 force);
	}
}
