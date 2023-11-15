using System;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AB5 RID: 2741
	[Serializable]
	public class Shield : Ability
	{
		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06003871 RID: 14449 RVA: 0x000A6800 File Offset: 0x000A4A00
		// (remove) Token: 0x06003872 RID: 14450 RVA: 0x000A6838 File Offset: 0x000A4A38
		public event Action<Shield.Instance> onBroke;

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06003873 RID: 14451 RVA: 0x000A6870 File Offset: 0x000A4A70
		// (remove) Token: 0x06003874 RID: 14452 RVA: 0x000A68A8 File Offset: 0x000A4AA8
		public event Action<Shield.Instance> onDetach;

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06003875 RID: 14453 RVA: 0x000A68DD File Offset: 0x000A4ADD
		// (set) Token: 0x06003876 RID: 14454 RVA: 0x000A68E5 File Offset: 0x000A4AE5
		public float amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				this._amount = value;
			}
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x00089C49 File Offset: 0x00087E49
		public Shield()
		{
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x000A68EE File Offset: 0x000A4AEE
		public Shield(float amount)
		{
			this._amount = amount;
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x000A68FD File Offset: 0x000A4AFD
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Shield.Instance(owner, this);
		}

		// Token: 0x04002CF6 RID: 11510
		[SerializeField]
		private float _amount;

		// Token: 0x04002CF7 RID: 11511
		[SerializeField]
		private Shield.HealthType _healthType;

		// Token: 0x02000AB6 RID: 2742
		public class Instance : AbilityInstance<Shield>
		{
			// Token: 0x0600387A RID: 14458 RVA: 0x000A6906 File Offset: 0x000A4B06
			public Instance(Character owner, Shield ability) : base(owner, ability)
			{
			}

			// Token: 0x0600387B RID: 14459 RVA: 0x000A6910 File Offset: 0x000A4B10
			public override void Refresh()
			{
				base.Refresh();
				if (this._shieldInstance != null)
				{
					this._shieldInstance.amount = (double)this.ability._amount;
				}
			}

			// Token: 0x0600387C RID: 14460 RVA: 0x000A6937 File Offset: 0x000A4B37
			private void OnShieldBroke()
			{
				Action<Shield.Instance> onBroke = this.ability.onBroke;
				if (onBroke != null)
				{
					onBroke(this._shieldInstance);
				}
				this.owner.ability.Remove(this);
			}

			// Token: 0x0600387D RID: 14461 RVA: 0x000A6968 File Offset: 0x000A4B68
			protected override void OnAttach()
			{
				if (this.ability._healthType == Shield.HealthType.Constant)
				{
					this._shieldInstance = this.owner.health.shield.Add(this.ability, this.ability._amount, new Action(this.OnShieldBroke));
					return;
				}
				this._shieldInstance = this.owner.health.shield.Add(this.ability, (float)(this.owner.health.maximumHealth * (double)this.ability._amount * 0.009999999776482582), new Action(this.OnShieldBroke));
			}

			// Token: 0x0600387E RID: 14462 RVA: 0x000A6A10 File Offset: 0x000A4C10
			protected override void OnDetach()
			{
				Action<Shield.Instance> onDetach = this.ability.onDetach;
				if (onDetach != null)
				{
					onDetach(this._shieldInstance);
				}
				if (this.owner.health.shield.Remove(this.ability))
				{
					this._shieldInstance = null;
				}
			}

			// Token: 0x04002CF8 RID: 11512
			private Shield.Instance _shieldInstance;
		}

		// Token: 0x02000AB7 RID: 2743
		public enum HealthType
		{
			// Token: 0x04002CFA RID: 11514
			Constant,
			// Token: 0x04002CFB RID: 11515
			Percent
		}
	}
}
