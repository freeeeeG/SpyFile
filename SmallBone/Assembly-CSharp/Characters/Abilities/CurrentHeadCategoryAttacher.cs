using System;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009DF RID: 2527
	public class CurrentHeadCategoryAttacher : AbilityAttacher
	{
		// Token: 0x060035B0 RID: 13744 RVA: 0x0009F790 File Offset: 0x0009D990
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x0009F7A0 File Offset: 0x0009D9A0
		public override void StartAttach()
		{
			base.owner.playerComponents.inventory.weapon.onSwap += this.Check;
			base.owner.playerComponents.inventory.weapon.onChanged += this.OnWeaponChange;
			this.Check();
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x0009F7FF File Offset: 0x0009D9FF
		private void OnWeaponChange(Weapon old, Weapon @new)
		{
			this.Check();
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x0009F808 File Offset: 0x0009DA08
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			base.owner.playerComponents.inventory.weapon.onSwap -= this.Check;
			base.owner.playerComponents.inventory.weapon.onChanged -= this.OnWeaponChange;
			this._attached = false;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x0009F893 File Offset: 0x0009DA93
		private void Check()
		{
			if (base.owner.playerComponents.inventory.weapon.polymorphOrCurrent.category == this._category)
			{
				this.Attach();
				return;
			}
			this.Detach();
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x0009F8C9 File Offset: 0x0009DAC9
		private void Attach()
		{
			if (this._attached)
			{
				return;
			}
			this._attached = true;
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x0009F8F7 File Offset: 0x0009DAF7
		private void Detach()
		{
			if (!this._attached)
			{
				return;
			}
			this._attached = false;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B39 RID: 11065
		[SerializeField]
		private Weapon.Category _category;

		// Token: 0x04002B3A RID: 11066
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B3B RID: 11067
		private bool _attached;
	}
}
