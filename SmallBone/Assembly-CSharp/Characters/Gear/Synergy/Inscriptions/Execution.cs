using System;
using Characters.Abilities;
using FX;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000880 RID: 2176
	public sealed class Execution : InscriptionInstance
	{
		// Token: 0x06002DD5 RID: 11733 RVA: 0x0008AD88 File Offset: 0x00088F88
		protected override void Initialize()
		{
			this._debuff.Initialize();
			this._debuff.execution = this;
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x0008ADA1 File Offset: 0x00088FA1
		public override void Attach()
		{
			base.character.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.GiveDamageDelegate));
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x0008ADC4 File Offset: 0x00088FC4
		public override void Detach()
		{
			base.character.onGiveDamage.Remove(new GiveDamageDelegate(this.GiveDamageDelegate));
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x0008ADE4 File Offset: 0x00088FE4
		private bool GiveDamageDelegate(ITarget target, ref Damage damage)
		{
			if (this.keyword.step < 1)
			{
				return false;
			}
			if (target.character == null)
			{
				return false;
			}
			if (TargetLayer.IsPlayer(target.character.gameObject.layer))
			{
				return false;
			}
			if (target.character.ability.Contains(this._debuff))
			{
				return false;
			}
			target.character.ability.Add(this._debuff);
			return false;
		}

		// Token: 0x04002641 RID: 9793
		[SerializeField]
		private Execution.Debuff _debuff;

		// Token: 0x02000881 RID: 2177
		[Serializable]
		internal sealed class Debuff : Ability
		{
			// Token: 0x170009BD RID: 2493
			// (get) Token: 0x06002DDB RID: 11739 RVA: 0x0008AE5C File Offset: 0x0008905C
			// (set) Token: 0x06002DDC RID: 11740 RVA: 0x0008AE64 File Offset: 0x00089064
			public InscriptionInstance execution { get; set; }

			// Token: 0x06002DDE RID: 11742 RVA: 0x0008AE70 File Offset: 0x00089070
			public override IAbilityInstance CreateInstance(Character owner)
			{
				Execution.Debuff.Step0 step = new Execution.Debuff.Step0(owner, this, this._step0);
				Execution.Debuff.Step1 step2 = new Execution.Debuff.Step1(owner, this, this._step1);
				Execution.Debuff.Step2 step3 = new Execution.Debuff.Step2(owner, this, this._step2);
				return new Execution.Debuff.Instance(owner, this, step, step2, step3);
			}

			// Token: 0x04002643 RID: 9795
			[SerializeField]
			private Execution.Debuff.StateInfo _step0;

			// Token: 0x04002644 RID: 9796
			[SerializeField]
			private Execution.Debuff.StateInfo _step1;

			// Token: 0x04002645 RID: 9797
			[SerializeField]
			private Execution.Debuff.StateInfo _step2;

			// Token: 0x02000882 RID: 2178
			[Serializable]
			internal class StateInfo
			{
				// Token: 0x04002646 RID: 9798
				[Range(0f, 1f)]
				public float healthThreshold;

				// Token: 0x04002647 RID: 9799
				[Range(0f, 100f)]
				public float damageMultiplier;

				// Token: 0x04002648 RID: 9800
				[Tooltip("처형 조건이 충족될 때 지속적으로 표시될 이펙트")]
				[SerializeField]
				public EffectInfo markEffect = new EffectInfo
				{
					subordinated = true
				};

				// Token: 0x04002649 RID: 9801
				[Tooltip("처형 조건이 충족될 때 한 번 표시될 이펙트")]
				[SerializeField]
				public EffectInfo activatingEffect;
			}

			// Token: 0x02000883 RID: 2179
			internal abstract class State
			{
				// Token: 0x06002DE0 RID: 11744 RVA: 0x0008AECB File Offset: 0x000890CB
				public State(Character owner, Execution.Debuff ability, Execution.Debuff.StateInfo stateInfo)
				{
					this._owner = owner;
					this._ability = ability;
					this._stateInfo = stateInfo;
				}

				// Token: 0x06002DE1 RID: 11745 RVA: 0x0008AEE8 File Offset: 0x000890E8
				public virtual bool CheckEnter()
				{
					return this._owner.health.percent < (double)this._stateInfo.healthThreshold;
				}

				// Token: 0x06002DE2 RID: 11746
				public abstract void Enter();

				// Token: 0x06002DE3 RID: 11747
				public abstract void OnTakeDamage(ref Damage damage);

				// Token: 0x06002DE4 RID: 11748
				public abstract void Exit();

				// Token: 0x0400264A RID: 9802
				protected Character _owner;

				// Token: 0x0400264B RID: 9803
				protected Execution.Debuff _ability;

				// Token: 0x0400264C RID: 9804
				protected Execution.Debuff.StateInfo _stateInfo;
			}

			// Token: 0x02000884 RID: 2180
			[Serializable]
			private class Step0 : Execution.Debuff.State
			{
				// Token: 0x06002DE5 RID: 11749 RVA: 0x0008AF08 File Offset: 0x00089108
				public Step0(Character owner, Execution.Debuff ability, Execution.Debuff.StateInfo stateInfo) : base(owner, ability, stateInfo)
				{
				}

				// Token: 0x06002DE6 RID: 11750 RVA: 0x00002191 File Offset: 0x00000391
				public override void Enter()
				{
				}

				// Token: 0x06002DE7 RID: 11751 RVA: 0x00002191 File Offset: 0x00000391
				public override void Exit()
				{
				}

				// Token: 0x06002DE8 RID: 11752 RVA: 0x00002191 File Offset: 0x00000391
				public override void OnTakeDamage(ref Damage damage)
				{
				}
			}

			// Token: 0x02000885 RID: 2181
			[Serializable]
			private class Step1 : Execution.Debuff.State
			{
				// Token: 0x06002DE9 RID: 11753 RVA: 0x0008AF08 File Offset: 0x00089108
				public Step1(Character owner, Execution.Debuff ability, Execution.Debuff.StateInfo stateInfo) : base(owner, ability, stateInfo)
				{
				}

				// Token: 0x06002DEA RID: 11754 RVA: 0x0008AF14 File Offset: 0x00089114
				public override void Enter()
				{
					this._stateInfo.activatingEffect.Spawn(this._owner.transform.position, this._owner, 0f, 1f);
					this._markEffectInstance = this._stateInfo.markEffect.Spawn(this._owner.transform.position, this._owner, 0f, 1f);
				}

				// Token: 0x06002DEB RID: 11755 RVA: 0x0008AF88 File Offset: 0x00089188
				public override void Exit()
				{
					if (this._markEffectInstance != null)
					{
						this._markEffectInstance.Stop();
						this._markEffectInstance = null;
					}
				}

				// Token: 0x06002DEC RID: 11756 RVA: 0x0008AFAA File Offset: 0x000891AA
				public override void OnTakeDamage(ref Damage damage)
				{
					damage.percentMultiplier *= 1.0 + (double)this._stateInfo.damageMultiplier * 0.01;
				}

				// Token: 0x0400264D RID: 9805
				private EffectPoolInstance _markEffectInstance;
			}

			// Token: 0x02000886 RID: 2182
			[Serializable]
			private class Step2 : Execution.Debuff.State
			{
				// Token: 0x06002DED RID: 11757 RVA: 0x0008AF08 File Offset: 0x00089108
				public Step2(Character owner, Execution.Debuff ability, Execution.Debuff.StateInfo stateInfo) : base(owner, ability, stateInfo)
				{
				}

				// Token: 0x06002DEE RID: 11758 RVA: 0x0008AFD6 File Offset: 0x000891D6
				public override void Enter()
				{
					this._markEffectInstance = this._stateInfo.markEffect.Spawn(this._owner.transform.position, this._owner, 0f, 1f);
				}

				// Token: 0x06002DEF RID: 11759 RVA: 0x0008B00E File Offset: 0x0008920E
				public override void Exit()
				{
					if (this._markEffectInstance != null)
					{
						this._markEffectInstance.Stop();
						this._markEffectInstance = null;
					}
				}

				// Token: 0x06002DF0 RID: 11760 RVA: 0x0008B030 File Offset: 0x00089230
				public override void OnTakeDamage(ref Damage damage)
				{
					damage.percentMultiplier *= 1.0 + (double)this._stateInfo.damageMultiplier * 0.01;
					damage.SetGuaranteedCritical(int.MaxValue, true);
					damage.Evaluate(false);
				}

				// Token: 0x0400264E RID: 9806
				private EffectPoolInstance _markEffectInstance;
			}

			// Token: 0x02000887 RID: 2183
			public class Instance : AbilityInstance<Execution.Debuff>
			{
				// Token: 0x06002DF1 RID: 11761 RVA: 0x0008B06F File Offset: 0x0008926F
				public Instance(Character owner, Execution.Debuff ability, Execution.Debuff.State step0, Execution.Debuff.State step1, Execution.Debuff.State step2) : base(owner, ability)
				{
					this._step0 = step0;
					this._step1 = step1;
					this._step2 = step2;
				}

				// Token: 0x06002DF2 RID: 11762 RVA: 0x0008B090 File Offset: 0x00089290
				protected override void OnAttach()
				{
					this.owner.health.onTakeDamage.Add(int.MinValue, new TakeDamageDelegate(this.OnTakeDamage));
					this.owner.health.onChanged += this.OnHealthChanged;
					this._currentState = this._step0;
					this.OnHealthChanged();
				}

				// Token: 0x06002DF3 RID: 11763 RVA: 0x0008B0F4 File Offset: 0x000892F4
				protected override void OnDetach()
				{
					this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnTakeDamage));
					this.owner.health.onChanged -= this.OnHealthChanged;
					this._currentState.Exit();
				}

				// Token: 0x06002DF4 RID: 11764 RVA: 0x0008B14A File Offset: 0x0008934A
				private bool OnTakeDamage(ref Damage damage)
				{
					this._currentState.OnTakeDamage(ref damage);
					return false;
				}

				// Token: 0x06002DF5 RID: 11765 RVA: 0x0008B15C File Offset: 0x0008935C
				private void OnHealthChanged()
				{
					if (this._step2.CheckEnter() && this.ability.execution.keyword.isMaxStep)
					{
						if (!this._currentState.Equals(this._step2))
						{
							this._currentState.Exit();
							this._currentState = this._step2;
							this._currentState.Enter();
						}
						return;
					}
					if (this._step1.CheckEnter() && this.ability.execution.keyword.step >= 1)
					{
						if (!this._currentState.Equals(this._step1))
						{
							this._currentState.Exit();
							this._currentState = this._step1;
							this._currentState.Enter();
						}
						return;
					}
					if (this._step0.CheckEnter())
					{
						if (!this._currentState.Equals(this._step0))
						{
							this._currentState.Exit();
							this._currentState = this._step0;
							this._currentState.Enter();
						}
						return;
					}
				}

				// Token: 0x0400264F RID: 9807
				private Execution.Debuff.State _step0;

				// Token: 0x04002650 RID: 9808
				private Execution.Debuff.State _step1;

				// Token: 0x04002651 RID: 9809
				private Execution.Debuff.State _step2;

				// Token: 0x04002652 RID: 9810
				private Execution.Debuff.State _currentState;
			}
		}
	}
}
