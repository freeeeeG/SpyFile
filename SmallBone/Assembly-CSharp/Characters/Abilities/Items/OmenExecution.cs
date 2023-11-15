using System;
using Characters.Actions;
using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CE6 RID: 3302
	[Serializable]
	public sealed class OmenExecution : Ability
	{
		// Token: 0x060042CD RID: 17101 RVA: 0x000C2702 File Offset: 0x000C0902
		public override void Initialize()
		{
			base.Initialize();
			this._onSummon.Initialize();
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x000C2715 File Offset: 0x000C0915
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OmenExecution.Instance(owner, this);
		}

		// Token: 0x04003315 RID: 13077
		[SerializeField]
		[Tooltip("혹시 단계별 이펙트를 바꾸고 싶을 경우 Duel 애니메이터의 Transition과 Parameter를 수정하면 됨")]
		[Header("연출")]
		private EffectInfo _effect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x04003316 RID: 13078
		[Header("필터")]
		[SerializeField]
		private CharacterTypeBoolArray _targetType;

		// Token: 0x04003317 RID: 13079
		[Header("설정")]
		[SerializeField]
		private int[] _damageStep;

		// Token: 0x04003318 RID: 13080
		[SerializeField]
		[Range(0f, 100f)]
		private int _enhancedDamageHealthPercent;

		// Token: 0x04003319 RID: 13081
		[Range(0f, 100f)]
		[SerializeField]
		private int _enhancedDamageBossHealthPercent;

		// Token: 0x0400331A RID: 13082
		[SerializeField]
		[Information("percentPoint", InformationAttribute.InformationType.Info, false)]
		private float _enhancedDamageMultiplier;

		// Token: 0x0400331B RID: 13083
		[Header("단두대")]
		[SerializeField]
		private Transform _summonPoint;

		// Token: 0x0400331C RID: 13084
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onSummon;

		// Token: 0x02000CE7 RID: 3303
		public sealed class Instance : AbilityInstance<OmenExecution>
		{
			// Token: 0x17000DE7 RID: 3559
			// (get) Token: 0x060042D0 RID: 17104 RVA: 0x000C2738 File Offset: 0x000C0938
			public override int iconStacks
			{
				get
				{
					return (int)this._totalDamage;
				}
			}

			// Token: 0x060042D1 RID: 17105 RVA: 0x000C2741 File Offset: 0x000C0941
			public Instance(Character owner, OmenExecution ability) : base(owner, ability)
			{
				this._effect = ability._effect;
			}

			// Token: 0x060042D2 RID: 17106 RVA: 0x000C2768 File Offset: 0x000C0968
			protected override void OnAttach()
			{
				this._effectInstance = ((this._effect == null) ? null : this._effect.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
				this.owner.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.OnGiveDamage));
				this.owner.onStartAction += this.HandleOnStartAction;
			}

			// Token: 0x060042D3 RID: 17107 RVA: 0x000C2810 File Offset: 0x000C0A10
			private void HandleOnStartAction(Characters.Actions.Action characterAction)
			{
				if (this._currentStep < this.ability._damageStep.Length - 1)
				{
					return;
				}
				if (characterAction.type != Characters.Actions.Action.Type.Skill)
				{
					return;
				}
				this._totalDamage = 0f;
				this.UpdateStep();
				if (this._effectInstance != null)
				{
					this.ability._summonPoint.transform.position = this._effectInstance.transform.position;
				}
				this.ability._onSummon.Run(this.owner);
				this._currentStep = 0;
				this._effectInstance.animator.Play(this.entryAnimation, 0, 0f);
				this._effectInstance.animator.SetInteger("Stacks", this._currentStep);
			}

			// Token: 0x060042D4 RID: 17108 RVA: 0x000C28D8 File Offset: 0x000C0AD8
			private bool OnGiveDamage(ITarget target, ref Damage damage)
			{
				if (!damage.key.Equals("OmenExecution", StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				Character character = target.character;
				if (character == null)
				{
					return false;
				}
				if ((character.type == Character.Type.Adventurer || character.type == Character.Type.Boss) && character.health.percent > (double)this.ability._enhancedDamageBossHealthPercent * 0.01)
				{
					return false;
				}
				if (character.health.percent > (double)this.ability._enhancedDamageHealthPercent * 0.01)
				{
					return false;
				}
				damage.multiplier += (double)this.ability._enhancedDamageMultiplier;
				return false;
			}

			// Token: 0x060042D5 RID: 17109 RVA: 0x000C2980 File Offset: 0x000C0B80
			private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				Character character = target.character;
				if (character == null)
				{
					return;
				}
				if (!this.ability._targetType[character.type])
				{
					return;
				}
				this._totalDamage += (float)damageDealt;
				this.UpdateStep();
			}

			// Token: 0x060042D6 RID: 17110 RVA: 0x000C29D0 File Offset: 0x000C0BD0
			private void UpdateStep()
			{
				if (this._currentStep >= this.ability._damageStep.Length - 1 && (float)this.ability._damageStep[this._currentStep] <= this._totalDamage)
				{
					return;
				}
				this._currentStep = 0;
				for (int i = this.ability._damageStep.Length - 1; i >= 0; i--)
				{
					if ((float)this.ability._damageStep[i] <= this._totalDamage)
					{
						this._currentStep = i;
						break;
					}
				}
				this._effectInstance.animator.SetInteger("Stacks", this._currentStep);
			}

			// Token: 0x060042D7 RID: 17111 RVA: 0x000C2A6C File Offset: 0x000C0C6C
			protected override void OnDetach()
			{
				if (this._effectInstance != null)
				{
					this._effectInstance.Stop();
					this._effectInstance = null;
				}
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
				this.owner.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
				this.owner.onStartAction -= this.HandleOnStartAction;
			}

			// Token: 0x0400331D RID: 13085
			private readonly int entryAnimation = Animator.StringToHash("OmenExecution_0");

			// Token: 0x0400331E RID: 13086
			private const string key = "OmenExecution";

			// Token: 0x0400331F RID: 13087
			private float _totalDamage;

			// Token: 0x04003320 RID: 13088
			private int _currentStep;

			// Token: 0x04003321 RID: 13089
			private readonly EffectInfo _effect;

			// Token: 0x04003322 RID: 13090
			private EffectPoolInstance _effectInstance;
		}
	}
}
