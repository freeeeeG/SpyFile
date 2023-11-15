using System;
using Characters.Operations;
using FX;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CA3 RID: 3235
	[Serializable]
	public sealed class ChosenHerosCirclet : Ability
	{
		// Token: 0x060041C3 RID: 16835 RVA: 0x000BF40C File Offset: 0x000BD60C
		public override void Initialize()
		{
			this._onFortitudeStart.Initialize();
			this._onFortitudeEnd.Initialize();
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x000BF424 File Offset: 0x000BD624
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ChosenHerosCirclet.Instance(owner, this);
		}

		// Token: 0x04003265 RID: 12901
		[Range(0f, 1f)]
		[SerializeField]
		private float _triggerHealthPercent;

		// Token: 0x04003266 RID: 12902
		[SerializeField]
		private float _fortitudeTime;

		// Token: 0x04003267 RID: 12903
		[SerializeField]
		private float _fortitudeCooldownTime;

		// Token: 0x04003268 RID: 12904
		[SerializeField]
		private float _recoverDamageUnit;

		// Token: 0x04003269 RID: 12905
		[Range(0f, 1f)]
		[SerializeField]
		private float _recoverMaxPercent;

		// Token: 0x0400326A RID: 12906
		[SerializeField]
		private Stat.Values _bonusStat;

		// Token: 0x0400326B RID: 12907
		[SerializeField]
		private EffectInfo _fortitudeLoopEffect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x0400326C RID: 12908
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onFortitudeStart;

		// Token: 0x0400326D RID: 12909
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onFortitudeEnd;

		// Token: 0x02000CA4 RID: 3236
		public class Instance : AbilityInstance<ChosenHerosCirclet>
		{
			// Token: 0x17000DCA RID: 3530
			// (get) Token: 0x060041C6 RID: 16838 RVA: 0x000BF447 File Offset: 0x000BD647
			public override Sprite icon
			{
				get
				{
					if (this._fortitude)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x17000DCB RID: 3531
			// (get) Token: 0x060041C7 RID: 16839 RVA: 0x000BF459 File Offset: 0x000BD659
			public override float iconFillAmount
			{
				get
				{
					return this._remainCooldownTime / this.ability._fortitudeCooldownTime;
				}
			}

			// Token: 0x060041C8 RID: 16840 RVA: 0x000BF46D File Offset: 0x000BD66D
			public Instance(Character owner, ChosenHerosCirclet ability) : base(owner, ability)
			{
			}

			// Token: 0x17000DCC RID: 3532
			// (get) Token: 0x060041C9 RID: 16841 RVA: 0x000BF477 File Offset: 0x000BD677
			public bool canUse
			{
				get
				{
					return !this._fortitude && this._remainCooldownTime <= 0f;
				}
			}

			// Token: 0x060041CA RID: 16842 RVA: 0x000BF493 File Offset: 0x000BD693
			protected override void OnAttach()
			{
				base.remainTime = 2.1474836E+09f;
				this.owner.health.onDie += this.HandleOnDie;
			}

			// Token: 0x060041CB RID: 16843 RVA: 0x000BF4BC File Offset: 0x000BD6BC
			private void HandleOnDie()
			{
				if (this.canUse)
				{
					this.owner.health.SetCurrentHealth(1.0);
					this.AttachBuff();
				}
			}

			// Token: 0x060041CC RID: 16844 RVA: 0x000BF4E8 File Offset: 0x000BD6E8
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainBuffTime -= deltaTime;
				this._remainCooldownTime -= deltaTime;
				if (this._fortitude)
				{
					if (this._remainBuffTime <= 0f)
					{
						this._remainCooldownTime = this.ability._fortitudeCooldownTime;
						this.DetachBuff();
						this.Heal();
						return;
					}
				}
				else if (this.owner.health.percent <= (double)this.ability._triggerHealthPercent && this._remainCooldownTime <= 0f)
				{
					this.AttachBuff();
				}
			}

			// Token: 0x060041CD RID: 16845 RVA: 0x000BF57C File Offset: 0x000BD77C
			private void AttachBuff()
			{
				this._remainBuffTime = this.ability._fortitudeTime;
				this._fortitude = true;
				this.owner.invulnerable.Attach(this);
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
				this.owner.stat.AttachValues(this.ability._bonusStat);
				this._cooldownReference.Stop();
				this._cooldownReference = this.owner.StartCoroutineWithReference(this.ability._onFortitudeStart.CRun(this.owner));
				this._loopEffect = this.ability._fortitudeLoopEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
			}

			// Token: 0x060041CE RID: 16846 RVA: 0x000BF65C File Offset: 0x000BD85C
			private void DetachBuff()
			{
				if (this._fortitude)
				{
					this.owner.StartCoroutine(this.ability._onFortitudeEnd.CRun(this.owner));
				}
				this._fortitude = false;
				this.owner.invulnerable.Detach(this);
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
				this.owner.stat.DetachValues(this.ability._bonusStat);
				if (this._loopEffect != null)
				{
					this._loopEffect.Stop();
					this._loopEffect = null;
				}
			}

			// Token: 0x060041CF RID: 16847 RVA: 0x000BF710 File Offset: 0x000BD910
			private void Heal()
			{
				float a = (float)this.owner.health.maximumHealth * this.ability._recoverMaxPercent;
				float b = (float)this.owner.health.maximumHealth * (float)this._totalAttackDamage / this.ability._recoverDamageUnit;
				float num = Mathf.Min(a, b);
				this.owner.health.Heal((double)num, true);
				this._totalAttackDamage = 0.0;
			}

			// Token: 0x060041D0 RID: 16848 RVA: 0x000BF78C File Offset: 0x000BD98C
			private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (gaveDamage.attribute != Damage.Attribute.Physical)
				{
					return;
				}
				double totalAttackDamage = this._totalAttackDamage;
				Damage damage = gaveDamage;
				this._totalAttackDamage = totalAttackDamage + damage.amount;
			}

			// Token: 0x060041D1 RID: 16849 RVA: 0x000BF7BD File Offset: 0x000BD9BD
			protected override void OnDetach()
			{
				this.owner.health.onDie -= this.HandleOnDie;
				this.DetachBuff();
			}

			// Token: 0x0400326E RID: 12910
			private float _remainCooldownTime;

			// Token: 0x0400326F RID: 12911
			private float _remainBuffTime;

			// Token: 0x04003270 RID: 12912
			private double _totalAttackDamage;

			// Token: 0x04003271 RID: 12913
			private bool _fortitude;

			// Token: 0x04003272 RID: 12914
			private CoroutineReference _cooldownReference;

			// Token: 0x04003273 RID: 12915
			private EffectPoolInstance _loopEffect;
		}
	}
}
