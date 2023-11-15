using System;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities
{
	// Token: 0x02000A14 RID: 2580
	[Serializable]
	public class StackableAdditionalHit : Ability
	{
		// Token: 0x060036B3 RID: 14003 RVA: 0x000A1C08 File Offset: 0x0009FE08
		protected override void Finalize()
		{
			try
			{
				if (this._hitEffectByStacks != null)
				{
					for (int i = 0; i < this._hitEffectByStacks.Length; i++)
					{
						if (this._hitEffectByStacks[i] != null)
						{
							this._hitEffectByStacks[i].Dispose();
							this._hitEffectByStacks[i] = null;
						}
					}
					this._hitEffectByStacks = null;
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x000A1C74 File Offset: 0x0009FE74
		public override void Initialize()
		{
			base.Initialize();
			this._targetOperationInfo.Initialize();
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x000A1C87 File Offset: 0x0009FE87
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StackableAdditionalHit.Instance(owner, this);
		}

		// Token: 0x04002BBE RID: 11198
		[SerializeField]
		private int _maxStack;

		// Token: 0x04002BBF RID: 11199
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04002BC0 RID: 11200
		[SerializeField]
		private int _applyCount;

		// Token: 0x04002BC1 RID: 11201
		[SerializeField]
		private float _damageMultiplierPerStack;

		// Token: 0x04002BC2 RID: 11202
		[SerializeField]
		private float _additionalDamageAmount;

		// Token: 0x04002BC3 RID: 11203
		[SerializeField]
		private bool _adaptiveForce;

		// Token: 0x04002BC4 RID: 11204
		[SerializeField]
		private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04002BC5 RID: 11205
		[SerializeField]
		private bool _needCritical;

		// Token: 0x04002BC6 RID: 11206
		[SerializeField]
		private MotionTypeBoolArray _attackTypes;

		// Token: 0x04002BC7 RID: 11207
		[SerializeField]
		private AttackTypeBoolArray _damageTypes;

		// Token: 0x04002BC8 RID: 11208
		[SerializeField]
		private PositionInfo _positionInfo;

		// Token: 0x04002BC9 RID: 11209
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04002BCA RID: 11210
		[SerializeField]
		[Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _targetOperationInfo;

		// Token: 0x04002BCB RID: 11211
		[SerializeField]
		private StackableAdditionalHit.HitEffectByStack[] _hitEffectByStacks;

		// Token: 0x02000A15 RID: 2581
		public class Instance : AbilityInstance<StackableAdditionalHit>
		{
			// Token: 0x060036B7 RID: 14007 RVA: 0x000A1CA4 File Offset: 0x0009FEA4
			internal Instance(Character owner, StackableAdditionalHit ability) : base(owner, ability)
			{
			}

			// Token: 0x060036B8 RID: 14008 RVA: 0x000A1CB0 File Offset: 0x0009FEB0
			protected override void OnAttach()
			{
				this._targetStacks = new Dictionary<Character, int>(128);
				this._remainCount = this.ability._applyCount;
				if (this.ability._applyCount == 0)
				{
					this._remainCount = int.MaxValue;
				}
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
				Singleton<Service>.Instance.levelManager.onMapLoaded += this.HandleOnMapLoaded;
			}

			// Token: 0x060036B9 RID: 14009 RVA: 0x000A1D38 File Offset: 0x0009FF38
			private void HandleOnMapLoaded()
			{
				this._targetStacks.Clear();
			}

			// Token: 0x060036BA RID: 14010 RVA: 0x000A1D45 File Offset: 0x0009FF45
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x060036BB RID: 14011 RVA: 0x000A1D5C File Offset: 0x0009FF5C
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
				Singleton<Service>.Instance.levelManager.onMapLoaded -= this.HandleOnMapLoaded;
			}

			// Token: 0x060036BC RID: 14012 RVA: 0x000A1DAC File Offset: 0x0009FFAC
			private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (target.character == null)
				{
					return;
				}
				if (this._remainCooldownTime > 0f || target.character.health.dead || !target.transform.gameObject.activeSelf || (this.ability._needCritical && !tookDamage.critical) || !this.ability._attackTypes[tookDamage.motionType] || !this.ability._damageTypes[tookDamage.attackType])
				{
					return;
				}
				if (this._targetStacks.ContainsKey(target.character))
				{
					int value = Mathf.Min(this.ability._maxStack, this._targetStacks[target.character] + 1);
					this._targetStacks.Clear();
					this._targetStacks.Add(target.character, value);
				}
				else
				{
					this._targetStacks.Clear();
					this._targetStacks.Add(target.character, 0);
				}
				if (this.ability._targetPoint != null)
				{
					Vector3 center = target.collider.bounds.center;
					Vector3 size = target.collider.bounds.size;
					size.x *= this.ability._positionInfo.pivotValue.x;
					size.y *= this.ability._positionInfo.pivotValue.y;
					Vector3 position = center + size;
					this.ability._targetPoint.position = position;
				}
				if (this.ability._adaptiveForce)
				{
					this.ability._additionalHit.ChangeAdaptiveDamageAttribute(this.owner);
				}
				float additionalDamageAmount = this.ability._additionalDamageAmount;
				Damage damage = this.owner.stat.GetDamage((double)additionalDamageAmount, MMMaths.RandomPointWithinBounds(target.collider.bounds), this.ability._additionalHit);
				damage.multiplier += (double)((float)this._targetStacks[target.character] * this.ability._damageMultiplierPerStack);
				int stack = this._targetStacks[target.character];
				foreach (StackableAdditionalHit.HitEffectByStack hitEffectByStack in this.ability._hitEffectByStacks)
				{
					if (hitEffectByStack.Contains(stack))
					{
						hitEffectByStack.Spawn(target.character);
					}
				}
				this.owner.StartCoroutine(this.ability._targetOperationInfo.CRun(this.owner, target.character));
				this.owner.Attack(target, ref damage);
				this._remainCooldownTime = this.ability._cooldownTime;
				this._remainCount--;
				if (this._remainCount == 0)
				{
					this.owner.ability.Remove(this);
				}
			}

			// Token: 0x04002BCC RID: 11212
			private float _remainCooldownTime;

			// Token: 0x04002BCD RID: 11213
			private int _remainCount;

			// Token: 0x04002BCE RID: 11214
			private Dictionary<Character, int> _targetStacks;
		}

		// Token: 0x02000A16 RID: 2582
		[Serializable]
		private class HitEffectByStack
		{
			// Token: 0x060036BD RID: 14013 RVA: 0x000A2093 File Offset: 0x000A0293
			public bool Contains(int stack)
			{
				return (float)stack >= this._stackRange.x && (float)stack <= this._stackRange.y;
			}

			// Token: 0x060036BE RID: 14014 RVA: 0x000A20B8 File Offset: 0x000A02B8
			public void Spawn(Character target)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._attackSoundInfo, target.transform.position);
				this._effectInfo.Spawn(MMMaths.RandomPointWithinBounds(target.collider.bounds), 0f, 1f);
			}

			// Token: 0x060036BF RID: 14015 RVA: 0x000A210C File Offset: 0x000A030C
			public void Dispose()
			{
				if (this._effectInfo != null)
				{
					this._effectInfo.Dispose();
					this._effectInfo = null;
				}
				if (this._attackSoundInfo != null)
				{
					this._attackSoundInfo.Dispose();
					this._attackSoundInfo = null;
				}
			}

			// Token: 0x04002BCF RID: 11215
			[SerializeField]
			private Vector2 _stackRange;

			// Token: 0x04002BD0 RID: 11216
			[SerializeField]
			private EffectInfo _effectInfo;

			// Token: 0x04002BD1 RID: 11217
			[SerializeField]
			private SoundInfo _attackSoundInfo;
		}
	}
}
