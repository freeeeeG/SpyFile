using System;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CB5 RID: 3253
	[Serializable]
	public sealed class DarkPriestsRobesShield : Ability
	{
		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x06004211 RID: 16913 RVA: 0x000C0738 File Offset: 0x000BE938
		// (remove) Token: 0x06004212 RID: 16914 RVA: 0x000C0770 File Offset: 0x000BE970
		public event Action<Shield.Instance> onBroke;

		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x06004213 RID: 16915 RVA: 0x000C07A8 File Offset: 0x000BE9A8
		// (remove) Token: 0x06004214 RID: 16916 RVA: 0x000C07E0 File Offset: 0x000BE9E0
		public event Action<Shield.Instance> onDetach;

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06004215 RID: 16917 RVA: 0x000C0815 File Offset: 0x000BEA15
		// (set) Token: 0x06004216 RID: 16918 RVA: 0x000C081D File Offset: 0x000BEA1D
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

		// Token: 0x06004217 RID: 16919 RVA: 0x00089C49 File Offset: 0x00087E49
		public DarkPriestsRobesShield()
		{
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x000C0826 File Offset: 0x000BEA26
		public DarkPriestsRobesShield(float amount)
		{
			this._amount = amount;
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x000C0835 File Offset: 0x000BEA35
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new DarkPriestsRobesShield.Instance(owner, this);
		}

		// Token: 0x040032A2 RID: 12962
		[SerializeField]
		private Stat.Values _stats;

		// Token: 0x040032A3 RID: 12963
		[SerializeField]
		private float _amount;

		// Token: 0x040032A4 RID: 12964
		[SerializeField]
		private DarkPriestsRobesShield.HealthType _healthType;

		// Token: 0x02000CB6 RID: 3254
		public sealed class Instance : AbilityInstance<DarkPriestsRobesShield>
		{
			// Token: 0x0600421A RID: 16922 RVA: 0x000C083E File Offset: 0x000BEA3E
			public Instance(Character owner, DarkPriestsRobesShield ability) : base(owner, ability)
			{
			}

			// Token: 0x0600421B RID: 16923 RVA: 0x000C0848 File Offset: 0x000BEA48
			public override void Refresh()
			{
				base.Refresh();
				if (this._shieldInstance != null)
				{
					this._shieldInstance.amount = (double)this.ability._amount;
				}
			}

			// Token: 0x0600421C RID: 16924 RVA: 0x000C086F File Offset: 0x000BEA6F
			private void OnShieldBroke()
			{
				Action<Shield.Instance> onBroke = this.ability.onBroke;
				if (onBroke != null)
				{
					onBroke(this._shieldInstance);
				}
				this.owner.ability.Remove(this);
			}

			// Token: 0x0600421D RID: 16925 RVA: 0x000C08A0 File Offset: 0x000BEAA0
			protected override void OnAttach()
			{
				this.owner.stat.AttachOrUpdateValues(this.ability._stats);
				if (this.ability._healthType == DarkPriestsRobesShield.HealthType.Constant)
				{
					this._shieldInstance = this.owner.health.shield.Add(this.ability, this.ability._amount, new Action(this.OnShieldBroke));
					return;
				}
				this._shieldInstance = this.owner.health.shield.Add(this.ability, (float)(this.owner.health.maximumHealth * (double)this.ability._amount * 0.009999999776482582), new Action(this.OnShieldBroke));
			}

			// Token: 0x0600421E RID: 16926 RVA: 0x000C0964 File Offset: 0x000BEB64
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this.ability._stats);
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

			// Token: 0x040032A5 RID: 12965
			private Shield.Instance _shieldInstance;
		}

		// Token: 0x02000CB7 RID: 3255
		public enum HealthType
		{
			// Token: 0x040032A7 RID: 12967
			Constant,
			// Token: 0x040032A8 RID: 12968
			Percent
		}
	}
}
