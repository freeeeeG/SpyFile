using System;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000AD4 RID: 2772
	[Serializable]
	public sealed class Glutton : Ability
	{
		// Token: 0x060038DE RID: 14558 RVA: 0x000A7906 File Offset: 0x000A5B06
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Glutton.Instance(owner, this);
		}

		// Token: 0x04002D30 RID: 11568
		[SerializeField]
		private int _maxShieldAmount;

		// Token: 0x04002D31 RID: 11569
		[SerializeField]
		[Range(0f, 100f)]
		private int _shieldRatio;

		// Token: 0x02000AD5 RID: 2773
		public sealed class Instance : AbilityInstance<Glutton>
		{
			// Token: 0x060038E0 RID: 14560 RVA: 0x000A790F File Offset: 0x000A5B0F
			public Instance(Character owner, Glutton ability) : base(owner, ability)
			{
			}

			// Token: 0x060038E1 RID: 14561 RVA: 0x000A7919 File Offset: 0x000A5B19
			protected override void OnAttach()
			{
				this.owner.health.onHealed += this.HandleOnHealed;
			}

			// Token: 0x060038E2 RID: 14562 RVA: 0x000A7938 File Offset: 0x000A5B38
			private void HandleOnHealed(double healed, double overHealed)
			{
				float num = (float)(overHealed * (double)this.ability._shieldRatio * 0.009999999776482582);
				if (this._shieldInstance != null)
				{
					num = Mathf.Min((float)this._shieldInstance.amount + num, (float)this.ability._maxShieldAmount);
					this._shieldInstance.amount = (double)((int)num);
					return;
				}
				num = (float)((int)Mathf.Min(num, (float)this.ability._maxShieldAmount));
				this._shieldInstance = this.owner.health.shield.Add(this.ability, num, null);
			}

			// Token: 0x060038E3 RID: 14563 RVA: 0x000A79D0 File Offset: 0x000A5BD0
			protected override void OnDetach()
			{
				this.owner.health.onHealed -= this.HandleOnHealed;
				if (this.owner.health.shield.Remove(this.ability))
				{
					this._shieldInstance = null;
				}
			}

			// Token: 0x04002D32 RID: 11570
			private Shield.Instance _shieldInstance;
		}
	}
}
