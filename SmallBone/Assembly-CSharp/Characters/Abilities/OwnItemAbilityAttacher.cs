using System;
using Characters.Gear.Items;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009F8 RID: 2552
	public class OwnItemAbilityAttacher : AbilityAttacher
	{
		// Token: 0x0600363E RID: 13886 RVA: 0x000A0CFF File Offset: 0x0009EEFF
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x000A0D0C File Offset: 0x0009EF0C
		public override void StartAttach()
		{
			base.owner.playerComponents.inventory.item.onChanged += this.OnItemInventoryChanged;
			this.OnItemInventoryChanged();
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x000A0D3A File Offset: 0x0009EF3A
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			base.owner.playerComponents.inventory.item.onChanged -= this.OnItemInventoryChanged;
			this.Detach();
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000A0D77 File Offset: 0x0009EF77
		private void OnItemInventoryChanged()
		{
			if (base.owner.playerComponents.inventory.item.Has(this._item))
			{
				this.Attach();
				return;
			}
			this.Detach();
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000A0DA8 File Offset: 0x0009EFA8
		private void Attach()
		{
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x000A0DC6 File Offset: 0x0009EFC6
		private void Detach()
		{
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B89 RID: 11145
		[SerializeField]
		private Item _item;

		// Token: 0x04002B8A RID: 11146
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;
	}
}
