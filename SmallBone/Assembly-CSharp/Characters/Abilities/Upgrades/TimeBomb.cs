using System;
using FX;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000B10 RID: 2832
	[Serializable]
	public sealed class TimeBomb : Ability
	{
		// Token: 0x06003997 RID: 14743 RVA: 0x000A9E3E File Offset: 0x000A803E
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new TimeBomb.Instance(owner, this);
		}

		// Token: 0x04002DAB RID: 11691
		[SerializeField]
		private EffectInfo _activeEffect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x04002DAC RID: 11692
		[SerializeField]
		private EffectInfo _deactiveEffect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x04002DAD RID: 11693
		[SerializeField]
		private TimeBombGiverComponent _giver;

		// Token: 0x02000B11 RID: 2833
		public sealed class Instance : AbilityInstance<TimeBomb>
		{
			// Token: 0x06003999 RID: 14745 RVA: 0x000A9E73 File Offset: 0x000A8073
			public Instance(Character owner, TimeBomb ability) : base(owner, ability)
			{
			}

			// Token: 0x0600399A RID: 14746 RVA: 0x000A9E80 File Offset: 0x000A8080
			protected override void OnAttach()
			{
				this._activeEffectInstance = this.ability._activeEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
				this._deactiveEffectInstance = this.ability._deactiveEffect.Spawn(this.owner.transform.position, this.owner, 0f, 1f);
			}

			// Token: 0x0600399B RID: 14747 RVA: 0x000A9EFC File Offset: 0x000A80FC
			protected override void OnDetach()
			{
				this._activeEffectInstance.renderer.color = new Color(255f, 255f, 255f, 255f);
				this._activeEffectInstance.Stop();
				this._deactiveEffectInstance.Stop();
				this._activeEffectInstance = null;
				this._deactiveEffectInstance = null;
			}

			// Token: 0x0600399C RID: 14748 RVA: 0x000A9F58 File Offset: 0x000A8158
			public void UpdateEffect(float alpha)
			{
				if (this._activeEffectInstance == null)
				{
					return;
				}
				Color color = this._activeEffectInstance.renderer.color;
				if (alpha >= 1f)
				{
					color.a = 2f - alpha;
				}
				else
				{
					color.a = alpha;
				}
				this._activeEffectInstance.renderer.color = color;
			}

			// Token: 0x0600399D RID: 14749 RVA: 0x000A9FB8 File Offset: 0x000A81B8
			public void Explode()
			{
				if (this._activeEffectInstance == null)
				{
					return;
				}
				this._activeEffectInstance.renderer.color = new Color(255f, 255f, 255f, 255f);
				this._activeEffectInstance.Stop();
				if (this._deactiveEffectInstance == null)
				{
					return;
				}
				this._deactiveEffectInstance.Stop();
				this.ability._giver.Attack(this.owner);
				base.remainTime = 0f;
			}

			// Token: 0x04002DAE RID: 11694
			private EffectPoolInstance _activeEffectInstance;

			// Token: 0x04002DAF RID: 11695
			private EffectPoolInstance _deactiveEffectInstance;
		}
	}
}
