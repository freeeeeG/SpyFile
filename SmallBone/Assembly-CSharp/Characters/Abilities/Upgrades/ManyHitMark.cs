using System;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000AE9 RID: 2793
	[Serializable]
	public sealed class ManyHitMark : Ability
	{
		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x0600391C RID: 14620 RVA: 0x000A8518 File Offset: 0x000A6718
		public HitInfo additionHitInfo
		{
			get
			{
				return this._additionalHit;
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x0600391D RID: 14621 RVA: 0x000A8520 File Offset: 0x000A6720
		public float additionalDamageAmount
		{
			get
			{
				return this._additionalDamageAmount;
			}
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x0600391E RID: 14622 RVA: 0x000A8528 File Offset: 0x000A6728
		public SoundInfo attackSoundInfo
		{
			get
			{
				return this._attackSoundInfo;
			}
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x000A8530 File Offset: 0x000A6730
		public override void Initialize()
		{
			base.Initialize();
			this._manyHitMarkTarget.Initialize();
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x000A8543 File Offset: 0x000A6743
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ManyHitMark.Instance(owner, this);
		}

		// Token: 0x04002D50 RID: 11600
		[Header("공격")]
		[SerializeField]
		private AttackTypeBoolArray _attackType;

		// Token: 0x04002D51 RID: 11601
		[SerializeField]
		private MotionTypeBoolArray _motionType;

		// Token: 0x04002D52 RID: 11602
		[SerializeField]
		private CharacterTypeBoolArray _targetType;

		// Token: 0x04002D53 RID: 11603
		[SerializeField]
		private float _additionalDamageAmount;

		// Token: 0x04002D54 RID: 11604
		[SerializeField]
		private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04002D55 RID: 11605
		[SerializeField]
		private PositionInfo _positionInfo;

		// Token: 0x04002D56 RID: 11606
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04002D57 RID: 11607
		[SerializeField]
		[Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _targetOperationInfo;

		// Token: 0x04002D58 RID: 11608
		[SerializeField]
		private SoundInfo _attackSoundInfo;

		// Token: 0x04002D59 RID: 11609
		[Header("타겟 효과")]
		[SerializeField]
		private float _cooldownTimePerTarget;

		// Token: 0x04002D5A RID: 11610
		[SerializeField]
		private ManyHitMark.ManyHitMarkTarget _manyHitMarkTarget;

		// Token: 0x02000AEA RID: 2794
		[Serializable]
		public sealed class ManyHitMarkTarget : Ability
		{
			// Token: 0x06003922 RID: 14626 RVA: 0x000A8560 File Offset: 0x000A6760
			public void SetAttacker(Character attacker, ManyHitMark manyHitMark)
			{
				this._attacker = attacker;
				this._manyHitMark = manyHitMark;
			}

			// Token: 0x06003923 RID: 14627 RVA: 0x000A8570 File Offset: 0x000A6770
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new ManyHitMark.ManyHitMarkTarget.Instance(owner, this);
			}

			// Token: 0x04002D5B RID: 11611
			[SerializeField]
			private int _maxStack;

			// Token: 0x04002D5C RID: 11612
			[SerializeField]
			private EffectInfo _stackEffect;

			// Token: 0x04002D5D RID: 11613
			[SerializeField]
			private EffectInfo _hitEffect;

			// Token: 0x04002D5E RID: 11614
			private Character _attacker;

			// Token: 0x04002D5F RID: 11615
			private ManyHitMark _manyHitMark;

			// Token: 0x02000AEB RID: 2795
			public sealed class Instance : AbilityInstance<ManyHitMark.ManyHitMarkTarget>
			{
				// Token: 0x06003925 RID: 14629 RVA: 0x000A8579 File Offset: 0x000A6779
				public Instance(Character owner, ManyHitMark.ManyHitMarkTarget ability) : base(owner, ability)
				{
					this._stackInfos = new ManyHitMark.ManyHitMarkTarget.Instance.StackInfo[3];
				}

				// Token: 0x06003926 RID: 14630 RVA: 0x000A858F File Offset: 0x000A678F
				protected override void OnAttach()
				{
					this._stack = -1;
					this.AddStack();
				}

				// Token: 0x06003927 RID: 14631 RVA: 0x000A859E File Offset: 0x000A679E
				public override void Refresh()
				{
					base.Refresh();
					this.AddStack();
				}

				// Token: 0x06003928 RID: 14632 RVA: 0x000A85AC File Offset: 0x000A67AC
				protected override void OnDetach()
				{
					for (int i = 0; i < this._stackInfos.Length; i++)
					{
						ManyHitMark.ManyHitMarkTarget.Instance.StackInfo stackInfo = this._stackInfos[i];
						if (stackInfo != null && stackInfo.stackEffectInstance != null)
						{
							stackInfo.stackEffectInstance.Stop();
							stackInfo.stackEffectInstance = null;
						}
						this._stackInfos[i] = null;
					}
					this._stackInfos = null;
				}

				// Token: 0x06003929 RID: 14633 RVA: 0x000A8608 File Offset: 0x000A6808
				private void AddStack()
				{
					this._stack++;
					Vector2 v = this.owner.collider.bounds.center + UnityEngine.Random.insideUnitCircle * this.owner.collider.bounds.extents * 0.7f;
					int num = UnityEngine.Random.Range(0, 360);
					EffectPoolInstance effectPoolInstance = this.ability._stackEffect.Spawn(v, this.owner, (float)num, 1f);
					effectPoolInstance.transform.SetParent(this.owner.attachWithFlip.transform);
					ManyHitMark.ManyHitMarkTarget.Instance.StackInfo stackInfo = new ManyHitMark.ManyHitMarkTarget.Instance.StackInfo
					{
						stackEffectInstance = effectPoolInstance,
						rotationZ = (float)num
					};
					this._stackInfos[this._stack] = stackInfo;
					if (this._stack >= this.ability._maxStack - 1)
					{
						this._stack = 0;
						this.Hit();
					}
				}

				// Token: 0x0600392A RID: 14634 RVA: 0x000A870C File Offset: 0x000A690C
				private void Hit()
				{
					for (int i = 0; i < this._stackInfos.Length; i++)
					{
						ManyHitMark.ManyHitMarkTarget.Instance.StackInfo stackInfo = this._stackInfos[i];
						if (stackInfo != null && stackInfo.stackEffectInstance != null)
						{
							this.ability._hitEffect.Spawn(stackInfo.stackEffectInstance.transform.position, this.owner, stackInfo.rotationZ, 1f);
							stackInfo.stackEffectInstance.Stop();
							stackInfo.stackEffectInstance = null;
						}
					}
					this.ability._manyHitMark.additionHitInfo.ChangeAdaptiveDamageAttribute(this.ability._attacker);
					Damage damage = this.ability._attacker.stat.GetDamage((double)this.ability._manyHitMark.additionalDamageAmount, MMMaths.RandomPointWithinBounds(this.owner.collider.bounds), this.ability._manyHitMark.additionHitInfo);
					PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._manyHitMark.attackSoundInfo, this.owner.transform.position);
					this.ability._attacker.StartCoroutine(this.ability._manyHitMark._targetOperationInfo.CRun(this.ability._attacker, this.owner));
					this.ability._attacker.Attack(this.owner, ref damage);
					this.owner.ability.Remove(this);
				}

				// Token: 0x04002D60 RID: 11616
				private int _stack;

				// Token: 0x04002D61 RID: 11617
				private ManyHitMark.ManyHitMarkTarget.Instance.StackInfo[] _stackInfos;

				// Token: 0x02000AEC RID: 2796
				internal class StackInfo
				{
					// Token: 0x04002D62 RID: 11618
					internal EffectPoolInstance stackEffectInstance;

					// Token: 0x04002D63 RID: 11619
					internal float rotationZ;
				}
			}
		}

		// Token: 0x02000AED RID: 2797
		public sealed class Instance : AbilityInstance<ManyHitMark>
		{
			// Token: 0x0600392C RID: 14636 RVA: 0x000A8882 File Offset: 0x000A6A82
			public Instance(Character owner, ManyHitMark ability) : base(owner, ability)
			{
				this._targets = new List<Character>(128);
				this._times = new List<float>(128);
			}

			// Token: 0x0600392D RID: 14637 RVA: 0x000A88AC File Offset: 0x000A6AAC
			protected override void OnAttach()
			{
				this.ability._manyHitMarkTarget.SetAttacker(this.owner, this.ability);
				this.owner.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
				Singleton<Service>.Instance.levelManager.onMapLoaded += this.HandleOnMapLoaded;
			}

			// Token: 0x0600392E RID: 14638 RVA: 0x000A8911 File Offset: 0x000A6B11
			private void HandleOnMapLoaded()
			{
				this._targets.Clear();
				this._targets.Clear();
			}

			// Token: 0x0600392F RID: 14639 RVA: 0x000A892C File Offset: 0x000A6B2C
			private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
			{
				if (damage.@null)
				{
					return false;
				}
				if (damage.amount == 0.0)
				{
					return false;
				}
				Character character = target.character;
				if (character == null)
				{
					return false;
				}
				if (!this.ability._attackType[damage.attackType])
				{
					return false;
				}
				if (!this.ability._motionType[damage.motionType])
				{
					return false;
				}
				if (!this.ability._targetType[character.type])
				{
					return false;
				}
				int num = this._targets.IndexOf(character);
				if (num == -1)
				{
					this._targets.Add(character);
					this._times.Add(Time.time);
				}
				else
				{
					if (Time.time - this._times[num] < this.ability._cooldownTimePerTarget)
					{
						return false;
					}
					this._times[num] = Time.time;
				}
				character.ability.Add(this.ability._manyHitMarkTarget);
				return false;
			}

			// Token: 0x06003930 RID: 14640 RVA: 0x000A8A30 File Offset: 0x000A6C30
			protected override void OnDetach()
			{
				this._targets.Clear();
				this._times.Clear();
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
				Singleton<Service>.Instance.levelManager.onMapLoaded -= this.HandleOnMapLoaded;
			}

			// Token: 0x04002D64 RID: 11620
			private List<Character> _targets;

			// Token: 0x04002D65 RID: 11621
			private List<float> _times;
		}
	}
}
