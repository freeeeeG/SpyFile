using System;
using Characters.Actions;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CBF RID: 3263
	[Serializable]
	public sealed class FrostGiantsLeather : Ability
	{
		// Token: 0x06004232 RID: 16946 RVA: 0x000C0C27 File Offset: 0x000BEE27
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new FrostGiantsLeather.Instance(owner, this);
		}

		// Token: 0x040032B4 RID: 12980
		[SerializeField]
		private float _buffTime;

		// Token: 0x040032B5 RID: 12981
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x040032B6 RID: 12982
		[SerializeField]
		private float _shieldTime;

		// Token: 0x040032B7 RID: 12983
		[SerializeField]
		private Stat.Values _statBonus;

		// Token: 0x040032B8 RID: 12984
		[SerializeField]
		private int _shieldAmount;

		// Token: 0x040032B9 RID: 12985
		[SerializeField]
		private int _maxShieldAmount;

		// Token: 0x040032BA RID: 12986
		[SerializeField]
		private EffectInfo _shieldEffect;

		// Token: 0x040032BB RID: 12987
		[SerializeField]
		private EffectInfo _buffAura;

		// Token: 0x040032BC RID: 12988
		[SerializeField]
		private SoundInfo _attachBuffSound;

		// Token: 0x02000CC0 RID: 3264
		public class Instance : AbilityInstance<FrostGiantsLeather>
		{
			// Token: 0x17000DD8 RID: 3544
			// (get) Token: 0x06004234 RID: 16948 RVA: 0x000C0C30 File Offset: 0x000BEE30
			public override float iconFillAmount
			{
				get
				{
					if (this._buffAttached)
					{
						return this._remainBuffTime / this.ability._buffTime;
					}
					return this._remainCooldownTime / this.ability._cooldownTime;
				}
			}

			// Token: 0x06004235 RID: 16949 RVA: 0x000C0C5F File Offset: 0x000BEE5F
			public Instance(Character owner, FrostGiantsLeather ability) : base(owner, ability)
			{
			}

			// Token: 0x06004236 RID: 16950 RVA: 0x000C0C69 File Offset: 0x000BEE69
			protected override void OnAttach()
			{
				this.owner.onStartAction += this.HandleOnStartAction;
			}

			// Token: 0x06004237 RID: 16951 RVA: 0x000C0C84 File Offset: 0x000BEE84
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (this._cooldownStart)
				{
					this._remainCooldownTime -= deltaTime;
				}
				this._remainBuffTime -= deltaTime;
				this._remainShieldTime -= deltaTime;
				if (this._remainShieldTime < 0f && this._shieldInstance != null)
				{
					this.BreakShield();
				}
				if (this._remainBuffTime < 0f && this._buffAttached)
				{
					this.DetachBuff();
				}
			}

			// Token: 0x06004238 RID: 16952 RVA: 0x000C0D00 File Offset: 0x000BEF00
			private void HandleOnStartAction(Characters.Actions.Action action)
			{
				if (action.type != Characters.Actions.Action.Type.Skill)
				{
					return;
				}
				if (this._remainCooldownTime > 0f)
				{
					return;
				}
				this._remainCooldownTime = this.ability._cooldownTime;
				this._cooldownStart = false;
				this.AttachBuff();
			}

			// Token: 0x06004239 RID: 16953 RVA: 0x000C0D38 File Offset: 0x000BEF38
			private void AttachBuff()
			{
				this.owner.stat.AttachValues(this.ability._statBonus);
				this.owner.status.onApplyFreeze += this.UpdateShieldAmount;
				this.owner.status.onRefreshFreeze += this.UpdateShieldAmount;
				this._buffAttached = true;
				base.iconFillInversed = true;
				base.iconFillFlipped = false;
				this._remainBuffTime = this.ability._buffTime;
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability._attachBuffSound, this.owner.transform.position);
				this._buffEffectInstance = ((this.ability._buffAura == null) ? null : this.ability._buffAura.Spawn(this.owner.transform.position, this.owner, 0f, 1f));
			}

			// Token: 0x0600423A RID: 16954 RVA: 0x000C0E2C File Offset: 0x000BF02C
			private void DetachBuff()
			{
				this.owner.stat.DetachValues(this.ability._statBonus);
				this.owner.status.onApplyFreeze -= this.UpdateShieldAmount;
				this.owner.status.onRefreshFreeze -= this.UpdateShieldAmount;
				this._buffAttached = false;
				this._cooldownStart = true;
				base.iconFillInversed = false;
				base.iconFillFlipped = true;
				if (this._buffEffectInstance != null)
				{
					this._buffEffectInstance.Stop();
					this._buffEffectInstance = null;
				}
			}

			// Token: 0x0600423B RID: 16955 RVA: 0x000C0EC8 File Offset: 0x000BF0C8
			private void UpdateShieldAmount(Character attacker, Character target)
			{
				this.UpdateShieldAmount();
			}

			// Token: 0x0600423C RID: 16956 RVA: 0x000C0ED0 File Offset: 0x000BF0D0
			private void UpdateShieldAmount()
			{
				if (this._shieldInstance != null)
				{
					int num = Mathf.Min((int)this._shieldInstance.amount + this.ability._shieldAmount, this.ability._maxShieldAmount);
					this._shieldInstance.amount = (double)num;
				}
				else
				{
					this._shieldInstance = this.owner.health.shield.Add(this, (float)this.ability._shieldAmount, new System.Action(this.BreakShield));
					EffectInfo shieldEffect = this.ability._shieldEffect;
					this._shieldEffectInstance = ((shieldEffect != null) ? shieldEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f) : null);
				}
				this._remainShieldTime = this.ability._shieldTime;
			}

			// Token: 0x0600423D RID: 16957 RVA: 0x000C0FA0 File Offset: 0x000BF1A0
			private void BreakShield()
			{
				if (this.owner.health.shield.Remove(this))
				{
					this._shieldInstance = null;
				}
				if (this._shieldEffectInstance != null)
				{
					this._shieldEffectInstance.Stop();
					this._shieldEffectInstance = null;
				}
			}

			// Token: 0x0600423E RID: 16958 RVA: 0x000C0FEC File Offset: 0x000BF1EC
			protected override void OnDetach()
			{
				this.owner.onStartAction -= this.HandleOnStartAction;
				this._remainBuffTime = this.ability._cooldownTime;
				this.DetachBuff();
				this.BreakShield();
			}

			// Token: 0x040032BD RID: 12989
			private Shield.Instance _shieldInstance;

			// Token: 0x040032BE RID: 12990
			private float _remainCooldownTime;

			// Token: 0x040032BF RID: 12991
			private float _remainBuffTime;

			// Token: 0x040032C0 RID: 12992
			private float _remainShieldTime;

			// Token: 0x040032C1 RID: 12993
			private bool _cooldownStart;

			// Token: 0x040032C2 RID: 12994
			private bool _buffAttached;

			// Token: 0x040032C3 RID: 12995
			private EffectPoolInstance _shieldEffectInstance;

			// Token: 0x040032C4 RID: 12996
			private EffectPoolInstance _buffEffectInstance;
		}
	}
}
