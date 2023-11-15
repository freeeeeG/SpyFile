using System;
using System.Collections;
using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BB9 RID: 3001
	[Serializable]
	public sealed class LivingArmor : Ability
	{
		// Token: 0x140000AE RID: 174
		// (add) Token: 0x06003DD4 RID: 15828 RVA: 0x000B3A6C File Offset: 0x000B1C6C
		// (remove) Token: 0x06003DD5 RID: 15829 RVA: 0x000B3AA4 File Offset: 0x000B1CA4
		public event Action<Shield.Instance> onBroke;

		// Token: 0x140000AF RID: 175
		// (add) Token: 0x06003DD6 RID: 15830 RVA: 0x000B3ADC File Offset: 0x000B1CDC
		// (remove) Token: 0x06003DD7 RID: 15831 RVA: 0x000B3B14 File Offset: 0x000B1D14
		public event Action<Shield.Instance> onDetach;

		// Token: 0x06003DD8 RID: 15832 RVA: 0x000B3B49 File Offset: 0x000B1D49
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new LivingArmor.Instance(owner, this);
		}

		// Token: 0x04002FC9 RID: 12233
		[SerializeField]
		private EffectInfo _shieldEffectInfo = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x04002FCA RID: 12234
		[SerializeField]
		private EnumArray<Character.SizeForEffect, float> _effectSize;

		// Token: 0x04002FCB RID: 12235
		[SerializeField]
		private OperationInfos _operationInfos;

		// Token: 0x04002FCC RID: 12236
		[SerializeField]
		private OperationInfos _onBreakShield;

		// Token: 0x04002FCD RID: 12237
		[SerializeField]
		[Range(0f, 100f)]
		private float _shieldPercent;

		// Token: 0x04002FCE RID: 12238
		[SerializeField]
		private float _recoverWaitTime;

		// Token: 0x04002FCF RID: 12239
		[SerializeField]
		[Range(0f, 100f)]
		private float _recoverPercent;

		// Token: 0x04002FD0 RID: 12240
		[SerializeField]
		private float _recoverInterval;

		// Token: 0x02000BBA RID: 3002
		public sealed class Instance : AbilityInstance<LivingArmor>
		{
			// Token: 0x06003DDA RID: 15834 RVA: 0x000B3B6C File Offset: 0x000B1D6C
			public Instance(Character owner, LivingArmor ability) : base(owner, ability)
			{
			}

			// Token: 0x06003DDB RID: 15835 RVA: 0x000B3B78 File Offset: 0x000B1D78
			protected override void OnAttach()
			{
				this._shieldAmount = (float)this.owner.health.maximumHealth * this.ability._shieldPercent * 0.01f;
				this._shieldInstance = this.owner.health.shield.Add(this.ability, this._shieldAmount, new Action(this.OnShieldBroke));
				this.AttachShieldEffect();
				this.owner.health.onTookDamage += new TookDamageDelegate(this.HandleOnTookDamage);
				this._remainRecoverWaitTime = 2.1474836E+09f;
				this._remainRecoverInterval = this.ability._recoverInterval;
			}

			// Token: 0x06003DDC RID: 15836 RVA: 0x000B3C1F File Offset: 0x000B1E1F
			private IEnumerator CLoad()
			{
				yield return null;
				this.ability._operationInfos.Initialize();
				this.Activate();
				yield break;
			}

			// Token: 0x06003DDD RID: 15837 RVA: 0x000B3C30 File Offset: 0x000B1E30
			private void Activate()
			{
				if (this._active)
				{
					return;
				}
				this.ability._operationInfos.gameObject.SetActive(true);
				if (this.ability._operationInfos.gameObject.activeSelf)
				{
					this.ability._operationInfos.Run(this.owner);
				}
				this._active = true;
			}

			// Token: 0x06003DDE RID: 15838 RVA: 0x000B3C90 File Offset: 0x000B1E90
			private void Deactivate()
			{
				if (!this._active)
				{
					return;
				}
				this.ability._operationInfos.Stop();
				this._active = false;
			}

			// Token: 0x06003DDF RID: 15839 RVA: 0x000B3CB4 File Offset: 0x000B1EB4
			private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				Damage damage = tookDamage;
				if (damage.amount <= 0.0 || tookDamage.attackType == Damage.AttackType.None)
				{
					return;
				}
				this._remainRecoverWaitTime = this.ability._recoverWaitTime;
			}

			// Token: 0x06003DE0 RID: 15840 RVA: 0x000B3CF4 File Offset: 0x000B1EF4
			protected override void OnDetach()
			{
				this.owner.health.onTookDamage -= new TookDamageDelegate(this.HandleOnTookDamage);
				Action<Shield.Instance> onDetach = this.ability.onDetach;
				if (onDetach != null)
				{
					onDetach(this._shieldInstance);
				}
				if (this.owner.health.shield.Remove(this.ability))
				{
					this._shieldInstance = null;
				}
				if (this.ability._operationInfos != null)
				{
					this.Deactivate();
				}
			}

			// Token: 0x06003DE1 RID: 15841 RVA: 0x000B3D78 File Offset: 0x000B1F78
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainRecoverWaitTime -= deltaTime;
				this._remainRecoverInterval -= deltaTime;
				if (this._remainRecoverWaitTime <= 0f)
				{
					if (this._remainRecoverInterval > 0f)
					{
						return;
					}
					this._remainRecoverInterval = this.ability._recoverInterval;
					float num = this._shieldAmount * this.ability._recoverPercent * 0.01f;
					if (this._shieldInstance != null)
					{
						if (this._shieldInstance.amount >= (double)this._shieldAmount)
						{
							return;
						}
						if (this._loopEffectPoolInstance == null)
						{
							float extraScale = this.ability._effectSize[this.owner.sizeForEffect];
							this._loopEffectPoolInstance = ((this.ability._shieldEffectInfo != null) ? this.ability._shieldEffectInfo.Spawn(this.owner.transform.position, this.owner, 0f, extraScale) : null);
							if (this.ability._operationInfos != null)
							{
								this.owner.StartCoroutine(this.CLoad());
							}
						}
						this._shieldInstance.amount = (double)Mathf.Min((float)this._shieldInstance.amount + num, this._shieldAmount);
						this.owner.health.shield.AddOrUpdate(this.ability, (float)this._shieldInstance.amount, new Action(this.OnShieldBroke));
						return;
					}
					else
					{
						this.AttachShieldEffect();
						this._shieldInstance = this.owner.health.shield.Add(this.ability, num, new Action(this.OnShieldBroke));
					}
				}
			}

			// Token: 0x06003DE2 RID: 15842 RVA: 0x000B3F34 File Offset: 0x000B2134
			private void AttachShieldEffect()
			{
				float extraScale = this.ability._effectSize[this.owner.sizeForEffect];
				if (this._loopEffectPoolInstance != null)
				{
					this._loopEffectPoolInstance.Stop();
					this._loopEffectPoolInstance = null;
				}
				this._loopEffectPoolInstance = ((this.ability._shieldEffectInfo != null) ? this.ability._shieldEffectInfo.Spawn(this.owner.transform.position, this.owner, 0f, extraScale) : null);
				if (this.ability._operationInfos != null)
				{
					this.owner.StartCoroutine(this.CLoad());
				}
			}

			// Token: 0x06003DE3 RID: 15843 RVA: 0x000B3FE4 File Offset: 0x000B21E4
			private void OnShieldBroke()
			{
				if (this._shieldInstance == null)
				{
					return;
				}
				if (this._loopEffectPoolInstance != null)
				{
					this._loopEffectPoolInstance.Stop();
					this._loopEffectPoolInstance = null;
				}
				if (this.ability._operationInfos != null)
				{
					this.Deactivate();
				}
				this.owner.health.shield.Remove(this.ability);
				this._shieldInstance = null;
				Action<Shield.Instance> onBroke = this.ability.onBroke;
				if (onBroke != null)
				{
					onBroke(this._shieldInstance);
				}
				this.ability._onBreakShield.gameObject.SetActive(true);
				if (this.ability._onBreakShield.gameObject.activeSelf)
				{
					this.ability._onBreakShield.Run(this.owner);
				}
			}

			// Token: 0x04002FD1 RID: 12241
			private float _remainRecoverWaitTime;

			// Token: 0x04002FD2 RID: 12242
			private float _remainRecoverInterval;

			// Token: 0x04002FD3 RID: 12243
			private EffectPoolInstance _loopEffectPoolInstance;

			// Token: 0x04002FD4 RID: 12244
			private Shield.Instance _shieldInstance;

			// Token: 0x04002FD5 RID: 12245
			private float _shieldAmount;

			// Token: 0x04002FD6 RID: 12246
			private bool _active;
		}
	}
}
