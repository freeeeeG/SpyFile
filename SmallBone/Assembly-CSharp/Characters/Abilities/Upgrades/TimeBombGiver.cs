using System;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000B13 RID: 2835
	[Serializable]
	public sealed class TimeBombGiver : Ability
	{
		// Token: 0x0600399F RID: 14751 RVA: 0x000AA04B File Offset: 0x000A824B
		public override void Initialize()
		{
			base.Initialize();
			this._timeBomb.Initialize();
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x000AA05E File Offset: 0x000A825E
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this._owner = owner;
			return new TimeBombGiver.Instance(owner, this);
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x000AA070 File Offset: 0x000A8270
		public void Attack(Character target)
		{
			if (this._targetPoint != null)
			{
				Vector3 center = target.collider.bounds.center;
				Vector3 size = target.collider.bounds.size;
				size.x *= this._positionInfo.pivotValue.x;
				size.y *= this._positionInfo.pivotValue.y;
				Vector3 position = center + size;
				this._targetPoint.position = position;
			}
			this._owner.StartCoroutine(this._bombOperations.CRun(this._owner));
		}

		// Token: 0x04002DB0 RID: 11696
		[SerializeField]
		private string _explodeAttackKey;

		// Token: 0x04002DB1 RID: 11697
		[SerializeField]
		private float _explodeTime;

		// Token: 0x04002DB2 RID: 11698
		[SerializeField]
		private TimeBombComponent _timeBomb;

		// Token: 0x04002DB3 RID: 11699
		[Header("Explode")]
		[SerializeField]
		private CustomFloat _damageAmount;

		// Token: 0x04002DB4 RID: 11700
		[SerializeField]
		private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04002DB5 RID: 11701
		[SerializeField]
		private PositionInfo _positionInfo;

		// Token: 0x04002DB6 RID: 11702
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04002DB7 RID: 11703
		[Subcomponent(typeof(TargetedOperationInfo))]
		[SerializeField]
		private TargetedOperationInfo.Subcomponents _targetOperationInfo;

		// Token: 0x04002DB8 RID: 11704
		[SerializeField]
		private EffectInfo _hitEffect;

		// Token: 0x04002DB9 RID: 11705
		[SerializeField]
		private SoundInfo _attackSoundInfo;

		// Token: 0x04002DBA RID: 11706
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _bombOperations;

		// Token: 0x04002DBB RID: 11707
		private Character _owner;

		// Token: 0x02000B14 RID: 2836
		public sealed class Instance : AbilityInstance<TimeBombGiver>
		{
			// Token: 0x17000BDF RID: 3039
			// (get) Token: 0x060039A3 RID: 14755 RVA: 0x000AA12C File Offset: 0x000A832C
			public override float iconFillAmount
			{
				get
				{
					return this._remainExplodeTime / this.ability._explodeTime;
				}
			}

			// Token: 0x060039A4 RID: 14756 RVA: 0x000AA140 File Offset: 0x000A8340
			public Instance(Character owner, TimeBombGiver ability) : base(owner, ability)
			{
			}

			// Token: 0x060039A5 RID: 14757 RVA: 0x000AA158 File Offset: 0x000A8358
			protected override void OnAttach()
			{
				this._bombs = new List<TimeBomb.Instance>(128);
				this._remainExplodeTime = this.ability._explodeTime;
				this.owner.onGiveDamage.Add(int.MaxValue, new GiveDamageDelegate(this.HandleOnGiveDamage));
				this._alpha = 0f;
			}

			// Token: 0x060039A6 RID: 14758 RVA: 0x000AA1B4 File Offset: 0x000A83B4
			private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
			{
				Character character = target.character;
				if (character == null)
				{
					return false;
				}
				if (character.type == Character.Type.Dummy || character.type == Character.Type.Trap || character.type == Character.Type.Player || character.type == Character.Type.PlayerMinion)
				{
					return false;
				}
				if (character.ability.GetInstance<TimeBomb>() != null)
				{
					return false;
				}
				if (damage.key.Equals(this.ability._explodeAttackKey, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				if (this._running)
				{
					return false;
				}
				TimeBomb.Instance item = (TimeBomb.Instance)character.ability.Add(this.ability._timeBomb.ability);
				this._bombs.Add(item);
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attackSoundInfo, target.transform.position);
				return false;
			}

			// Token: 0x060039A7 RID: 14759 RVA: 0x000AA27C File Offset: 0x000A847C
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainExplodeTime -= deltaTime;
				float num = this._acc;
				if ((double)this._remainExplodeTime <= 2.5)
				{
					num *= 20f;
				}
				this._speed += deltaTime * num;
				this._alpha += deltaTime * this._speed;
				for (int i = this._bombs.Count - 1; i >= 0; i--)
				{
					if (this._bombs[i] == null)
					{
						this._bombs.RemoveAt(i);
					}
				}
				foreach (TimeBomb.Instance instance in this._bombs)
				{
					instance.UpdateEffect(this._alpha % 2f);
				}
				if (this._remainExplodeTime <= 0f)
				{
					this._speed = 0f;
					this._alpha = 0f;
					this._remainExplodeTime = this.ability._explodeTime;
					this._running = true;
					foreach (TimeBomb.Instance instance2 in this._bombs)
					{
						if (instance2 != null && !(instance2.owner == null) && !instance2.owner.health.dead)
						{
							instance2.Explode();
						}
					}
					this._running = false;
					this._bombs.Clear();
				}
			}

			// Token: 0x060039A8 RID: 14760 RVA: 0x000AA41C File Offset: 0x000A861C
			protected override void OnDetach()
			{
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.HandleOnGiveDamage));
				foreach (TimeBomb.Instance instance in this._bombs)
				{
					if (instance != null && !(instance.owner == null) && !instance.owner.health.dead)
					{
						instance.owner.ability.Remove(instance);
					}
				}
				this._bombs = null;
			}

			// Token: 0x04002DBC RID: 11708
			private float _remainExplodeTime;

			// Token: 0x04002DBD RID: 11709
			private float _acc = 0.3f;

			// Token: 0x04002DBE RID: 11710
			private float _speed;

			// Token: 0x04002DBF RID: 11711
			private float _alpha;

			// Token: 0x04002DC0 RID: 11712
			private bool _running;

			// Token: 0x04002DC1 RID: 11713
			private List<TimeBomb.Instance> _bombs;
		}
	}
}
