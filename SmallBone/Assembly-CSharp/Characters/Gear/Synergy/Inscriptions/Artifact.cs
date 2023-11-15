using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.Abilities;
using Characters.Abilities.Constraints;
using Characters.Movements;
using Characters.Projectiles;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x0200086B RID: 2155
	public sealed class Artifact : SimpleStatBonusKeyword
	{
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06002D14 RID: 11540 RVA: 0x00089499 File Offset: 0x00087699
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06002D15 RID: 11541 RVA: 0x00088DBC File Offset: 0x00086FBC
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.PercentPoint;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x000894A1 File Offset: 0x000876A1
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.MagicAttackDamage;
			}
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000894A8 File Offset: 0x000876A8
		protected override void Initialize()
		{
			base.Initialize();
			this._buff.artifact = this;
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x000894BC File Offset: 0x000876BC
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
			if (this.keyword.step >= 2)
			{
				if (!base.character.ability.Contains(this._buff))
				{
					base.character.ability.Add(this._buff);
					return;
				}
			}
			else
			{
				base.character.ability.Remove(this._buff);
			}
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x00089524 File Offset: 0x00087724
		public override void Detach()
		{
			base.Detach();
			base.character.ability.Remove(this._buff);
		}

		// Token: 0x040025D2 RID: 9682
		private const float _checkInterval = 0.3f;

		// Token: 0x040025D3 RID: 9683
		[SerializeField]
		private double[] _statBonusByStep;

		// Token: 0x040025D4 RID: 9684
		[SerializeField]
		[Header("Buff")]
		private Artifact.Buff _buff;

		// Token: 0x0200086C RID: 2156
		[Serializable]
		private class Buff : IAbility, IAbilityInstance
		{
			// Token: 0x17000979 RID: 2425
			// (get) Token: 0x06002D1B RID: 11547 RVA: 0x00089543 File Offset: 0x00087743
			Character IAbilityInstance.owner
			{
				get
				{
					return this._owner;
				}
			}

			// Token: 0x1700097A RID: 2426
			// (get) Token: 0x06002D1C RID: 11548 RVA: 0x000716FD File Offset: 0x0006F8FD
			public IAbility ability
			{
				get
				{
					return this;
				}
			}

			// Token: 0x1700097B RID: 2427
			// (get) Token: 0x06002D1D RID: 11549 RVA: 0x0008954B File Offset: 0x0008774B
			// (set) Token: 0x06002D1E RID: 11550 RVA: 0x00089553 File Offset: 0x00087753
			public float remainTime { get; set; }

			// Token: 0x1700097C RID: 2428
			// (get) Token: 0x06002D1F RID: 11551 RVA: 0x000076D4 File Offset: 0x000058D4
			public bool attached
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700097D RID: 2429
			// (get) Token: 0x06002D20 RID: 11552 RVA: 0x0008955C File Offset: 0x0008775C
			public Sprite icon
			{
				get
				{
					if (this._remainCooldownTime <= 0f)
					{
						return null;
					}
					return this._icon;
				}
			}

			// Token: 0x1700097E RID: 2430
			// (get) Token: 0x06002D21 RID: 11553 RVA: 0x00089573 File Offset: 0x00087773
			public float iconFillAmount
			{
				get
				{
					return 1f - this._remainCooldownTime / this._interval;
				}
			}

			// Token: 0x1700097F RID: 2431
			// (get) Token: 0x06002D22 RID: 11554 RVA: 0x000076D4 File Offset: 0x000058D4
			public bool iconFillInversed
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000980 RID: 2432
			// (get) Token: 0x06002D23 RID: 11555 RVA: 0x000076D4 File Offset: 0x000058D4
			public bool iconFillFlipped
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000981 RID: 2433
			// (get) Token: 0x06002D24 RID: 11556 RVA: 0x00089588 File Offset: 0x00087788
			public int iconStacks
			{
				get
				{
					if (!this.artifact.keyword.isMaxStep)
					{
						return 0;
					}
					return this._fireCount;
				}
			}

			// Token: 0x17000982 RID: 2434
			// (get) Token: 0x06002D25 RID: 11557 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool expired
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000983 RID: 2435
			// (get) Token: 0x06002D26 RID: 11558 RVA: 0x000895A4 File Offset: 0x000877A4
			// (set) Token: 0x06002D27 RID: 11559 RVA: 0x000895AC File Offset: 0x000877AC
			public float duration { get; set; }

			// Token: 0x17000984 RID: 2436
			// (get) Token: 0x06002D28 RID: 11560 RVA: 0x00018EC5 File Offset: 0x000170C5
			public int iconPriority
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x17000985 RID: 2437
			// (get) Token: 0x06002D29 RID: 11561 RVA: 0x00018EC5 File Offset: 0x000170C5
			public bool removeOnSwapWeapon
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000986 RID: 2438
			// (get) Token: 0x06002D2A RID: 11562 RVA: 0x000895B5 File Offset: 0x000877B5
			// (set) Token: 0x06002D2B RID: 11563 RVA: 0x000895BD File Offset: 0x000877BD
			public Artifact artifact { get; set; }

			// Token: 0x06002D2C RID: 11564 RVA: 0x000895C6 File Offset: 0x000877C6
			public void Attach()
			{
				this._coroutineReference = this._owner.StartCoroutineWithReference(this.CStartAttackLoop());
			}

			// Token: 0x06002D2D RID: 11565 RVA: 0x000895DF File Offset: 0x000877DF
			public IAbilityInstance CreateInstance(Character owner)
			{
				this._owner = owner;
				this._overlapper = new NonAllocOverlapper(128);
				this._overlapper.contactFilter.SetLayerMask(1024);
				return this;
			}

			// Token: 0x06002D2E RID: 11566 RVA: 0x00089613 File Offset: 0x00087813
			public void Detach()
			{
				this._coroutineReference.Stop();
			}

			// Token: 0x06002D2F RID: 11567 RVA: 0x00002191 File Offset: 0x00000391
			public void Initialize()
			{
			}

			// Token: 0x06002D30 RID: 11568 RVA: 0x00002191 File Offset: 0x00000391
			public void UpdateTime(float deltaTime)
			{
			}

			// Token: 0x06002D31 RID: 11569 RVA: 0x00002191 File Offset: 0x00000391
			public void Refresh()
			{
			}

			// Token: 0x06002D32 RID: 11570 RVA: 0x00089620 File Offset: 0x00087820
			private IEnumerator CStartAttackLoop()
			{
				yield return null;
				for (;;)
				{
					this._remainCooldownTime = this._interval;
					while (this._remainCooldownTime > 0f)
					{
						if (this._constraints.Pass())
						{
							this._remainCooldownTime -= this._owner.chronometer.master.deltaTime;
						}
						yield return null;
					}
					if (this.artifact.keyword.isMaxStep)
					{
						this._fireCount++;
					}
					while (!this.Fire((this._fireCount == this._cycle && this.artifact.keyword.isMaxStep) ? this._projectileCount : 1))
					{
						yield return this._owner.chronometer.master.WaitForSeconds(0.3f);
					}
				}
				yield break;
			}

			// Token: 0x06002D33 RID: 11571 RVA: 0x00089630 File Offset: 0x00087830
			private bool Fire(int count)
			{
				using (new UsingCollider(this._range, true))
				{
					this._overlapper.OverlapCollider(this._range);
				}
				IEnumerable<Collider2D> source = this._overlapper.results.Where(delegate(Collider2D result)
				{
					Target component = result.GetComponent<Target>();
					if (component == null)
					{
						return false;
					}
					Character character = component.character;
					return !(character == null) && !character.health.dead && this._targetTypes[character.type];
				});
				if (source.Count<Collider2D>() == 0)
				{
					return false;
				}
				Collider2D[] array = source.ToArray<Collider2D>();
				array.PseudoShuffle<Collider2D>();
				this._owner.StartCoroutine(this.CFireWithDelay(count, array));
				return true;
			}

			// Token: 0x06002D34 RID: 11572 RVA: 0x000896C8 File Offset: 0x000878C8
			private IEnumerator CFireWithDelay(int count, Collider2D[] targets)
			{
				int remain = count;
				int index = 0;
				while (remain > 0)
				{
					int num = MMMaths.RandomBool() ? UnityEngine.Random.Range(45, 55) : UnityEngine.Random.Range(125, 135);
					Vector2 b = this.GetAdditionalVector(Vector2.right, (float)num) * this._distance;
					Character character = targets[index].GetComponent<Target>().character;
					Vector2 v = character.transform.position + b;
					Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
					if (character.movement.configs.Count > 0 && (character.movement.config.type == Movement.Config.Type.Static || character.movement.config.type == Movement.Config.Type.Flying || character.movement.config.type == Movement.Config.Type.AcceleratingFlying))
					{
						character.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
					}
					if (lastStandingCollider != null)
					{
						v = new Vector2(character.transform.position.x, lastStandingCollider.bounds.max.y) + b;
					}
					float direction = (float)(num + 180);
					Projectile projectile = this._projectile;
					if (this.artifact.keyword.isMaxStep && this._fireCount == this._cycle && remain == 1)
					{
						projectile = this._enhancedProjectile;
						this._fireCount = 0;
					}
					projectile.reusable.Spawn(v, true).GetComponent<Projectile>().Fire(this._owner, this._attackDamage.amount, direction, false, false, 1f, null, 0f);
					index = (index + 1) % targets.Length;
					int num2 = remain;
					remain = num2 - 1;
					yield return character.chronometer.master.WaitForSeconds(this._firingInterval.value);
				}
				yield break;
			}

			// Token: 0x06002D35 RID: 11573 RVA: 0x000896E8 File Offset: 0x000878E8
			private Vector2 GetAdditionalVector(Vector2 vec, float angle)
			{
				float f = Mathf.Atan2(vec.y, vec.x) + angle * 0.017453292f;
				return new Vector2(Mathf.Cos(f), Mathf.Sin(f));
			}

			// Token: 0x040025D5 RID: 9685
			[Constraint.SubcomponentAttribute]
			[SerializeField]
			private Constraint.Subcomponents _constraints;

			// Token: 0x040025D6 RID: 9686
			[SerializeField]
			[Header("4세트 효과")]
			private Sprite _icon;

			// Token: 0x040025D7 RID: 9687
			[SerializeField]
			private AttackDamage _attackDamage;

			// Token: 0x040025D8 RID: 9688
			[SerializeField]
			private CharacterTypeBoolArray _targetTypes;

			// Token: 0x040025D9 RID: 9689
			[SerializeField]
			private Collider2D _range;

			// Token: 0x040025DA RID: 9690
			[SerializeField]
			private float _interval;

			// Token: 0x040025DB RID: 9691
			[SerializeField]
			private Projectile _projectile;

			// Token: 0x040025DC RID: 9692
			[Header("6세트 효과")]
			[Tooltip("cycle번째 마다 강화된 추가공격")]
			[SerializeField]
			private int _projectileCount;

			// Token: 0x040025DD RID: 9693
			[SerializeField]
			private float _distance = 15f;

			// Token: 0x040025DE RID: 9694
			[SerializeField]
			private int _cycle;

			// Token: 0x040025DF RID: 9695
			[SerializeField]
			private CustomFloat _firingInterval;

			// Token: 0x040025E0 RID: 9696
			[SerializeField]
			private Projectile _enhancedProjectile;

			// Token: 0x040025E4 RID: 9700
			private NonAllocOverlapper _overlapper;

			// Token: 0x040025E5 RID: 9701
			private int _fireCount;

			// Token: 0x040025E6 RID: 9702
			private float _remainCooldownTime;

			// Token: 0x040025E7 RID: 9703
			private Character _owner;

			// Token: 0x040025E8 RID: 9704
			private CoroutineReference _coroutineReference;
		}
	}
}
