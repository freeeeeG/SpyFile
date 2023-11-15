using System;
using Characters.Gear.Items;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000DA3 RID: 3491
	[Serializable]
	public class ElderEntsGratitude : Ability
	{
		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x06004643 RID: 17987 RVA: 0x000CB20D File Offset: 0x000C940D
		// (set) Token: 0x06004644 RID: 17988 RVA: 0x000CB215 File Offset: 0x000C9415
		public ElderEntsGratitudeComponent component { get; set; }

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x06004645 RID: 17989 RVA: 0x000CB21E File Offset: 0x000C941E
		public float amount
		{
			get
			{
				return this._amount;
			}
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x000CB226 File Offset: 0x000C9426
		public override void Initialize()
		{
			base.Initialize();
			this._operationsOnChange.Initialize();
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x000CB23C File Offset: 0x000C943C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this._instance = new ElderEntsGratitude.Instance(owner, this);
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x000CB259 File Offset: 0x000C9459
		public float GetShieldAmount()
		{
			if (this._instance == null)
			{
				return 0f;
			}
			return this._instance.GetShieldAmount();
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x000CB274 File Offset: 0x000C9474
		public void SetShieldAmount(float amount)
		{
			if (this._instance == null)
			{
				return;
			}
			this._instance.SetShieldAmount(amount);
		}

		// Token: 0x0400353C RID: 13628
		[SerializeField]
		private Item _elderEntsGratitudeItem;

		// Token: 0x0400353D RID: 13629
		[SerializeField]
		private Item _toChangeItemOnShieldBroken;

		// Token: 0x0400353E RID: 13630
		[SerializeField]
		[Space]
		private float _amount;

		// Token: 0x0400353F RID: 13631
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x04003540 RID: 13632
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operationsOnChange;

		// Token: 0x04003541 RID: 13633
		private ElderEntsGratitude.Instance _instance;

		// Token: 0x02000DA4 RID: 3492
		public class Instance : AbilityInstance<ElderEntsGratitude>
		{
			// Token: 0x17000EA2 RID: 3746
			// (get) Token: 0x0600464B RID: 17995 RVA: 0x000CB28B File Offset: 0x000C948B
			public override int iconStacks
			{
				get
				{
					return (int)this._shieldInstance.amount;
				}
			}

			// Token: 0x0600464C RID: 17996 RVA: 0x000CB299 File Offset: 0x000C9499
			public Instance(Character owner, ElderEntsGratitude ability) : base(owner, ability)
			{
			}

			// Token: 0x0600464D RID: 17997 RVA: 0x000CB2A3 File Offset: 0x000C94A3
			public override void Refresh()
			{
				base.Refresh();
				this._shieldInstance.amount = (double)this.ability.component.stack;
			}

			// Token: 0x0600464E RID: 17998 RVA: 0x000CB2C7 File Offset: 0x000C94C7
			private void OnShieldBroke()
			{
				this.ChangeItem();
				this.owner.ability.Remove(this);
			}

			// Token: 0x0600464F RID: 17999 RVA: 0x000CB2E4 File Offset: 0x000C94E4
			protected override void OnAttach()
			{
				this._shieldInstance = this.owner.health.shield.Add(this.ability, this.ability.component.savedShieldAmount, new Action(this.OnShieldBroke));
				this.owner.stat.AttachValues(this.ability._stat);
			}

			// Token: 0x06004650 RID: 18000 RVA: 0x000CB34C File Offset: 0x000C954C
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this.ability._stat);
				if (this._shieldInstance != null && this._shieldInstance.amount > 0.0)
				{
					this.ability.component.savedShieldAmount = (float)this._shieldInstance.amount;
				}
				if (this.owner.health.shield.Remove(this.ability))
				{
					this._shieldInstance = null;
				}
			}

			// Token: 0x06004651 RID: 18001 RVA: 0x000CB3D4 File Offset: 0x000C95D4
			private void ChangeItem()
			{
				if (this.owner.playerComponents == null)
				{
					return;
				}
				this.ability._operationsOnChange.Run(this.owner);
				this.owner.playerComponents.inventory.item.Change(this.ability._elderEntsGratitudeItem, this.ability._toChangeItemOnShieldBroken);
			}

			// Token: 0x06004652 RID: 18002 RVA: 0x000CB435 File Offset: 0x000C9635
			public float GetShieldAmount()
			{
				return (float)this._shieldInstance.amount;
			}

			// Token: 0x06004653 RID: 18003 RVA: 0x000CB443 File Offset: 0x000C9643
			public void SetShieldAmount(float amount)
			{
				this._shieldInstance.amount = (double)amount;
			}

			// Token: 0x04003543 RID: 13635
			private Shield.Instance _shieldInstance;
		}
	}
}
