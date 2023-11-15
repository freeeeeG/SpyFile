using System;
using Characters.Gear;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009E0 RID: 2528
	public class CurrentHeadTagAttacher : AbilityAttacher
	{
		// Token: 0x060035B9 RID: 13753 RVA: 0x0009F925 File Offset: 0x0009DB25
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x0009F934 File Offset: 0x0009DB34
		public override void StartAttach()
		{
			base.owner.playerComponents.inventory.weapon.onSwap += this.Check;
			base.owner.playerComponents.inventory.weapon.onChanged += this.OnWeaponChange;
			this.Check();
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x0009F993 File Offset: 0x0009DB93
		private void OnWeaponChange(Weapon old, Weapon @new)
		{
			this.Check();
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x0009F99C File Offset: 0x0009DB9C
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			base.owner.playerComponents.inventory.weapon.onSwap -= this.Check;
			base.owner.playerComponents.inventory.weapon.onChanged -= this.OnWeaponChange;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x0009FA20 File Offset: 0x0009DC20
		private void Check()
		{
			if (base.owner.playerComponents.inventory.weapon.polymorphOrCurrent.gearTag == this._tag)
			{
				this.Attach();
				return;
			}
			this.Detach();
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x0009FA56 File Offset: 0x0009DC56
		private void Attach()
		{
			if (this._attached)
			{
				return;
			}
			this._attached = true;
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x0009FA84 File Offset: 0x0009DC84
		private void Detach()
		{
			if (!this._attached)
			{
				return;
			}
			this._attached = false;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B3C RID: 11068
		[SerializeField]
		private Gear.Tag _tag;

		// Token: 0x04002B3D RID: 11069
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B3E RID: 11070
		private bool _attached;
	}
}
