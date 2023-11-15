using System;
using Characters.Gear.Weapons;
using Characters.Player;
using Data;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CFD RID: 3325
	[Serializable]
	public sealed class StoneMask : Ability
	{
		// Token: 0x06004327 RID: 17191 RVA: 0x000C3D46 File Offset: 0x000C1F46
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StoneMask.Instance(owner, this);
		}

		// Token: 0x0400335F RID: 13151
		[SerializeField]
		private GameData.Currency.Type _type;

		// Token: 0x04003360 RID: 13152
		[SerializeField]
		private StoneMask.PercentByRarity _percentByRarity;

		// Token: 0x02000CFE RID: 3326
		public sealed class Instance : AbilityInstance<StoneMask>
		{
			// Token: 0x06004329 RID: 17193 RVA: 0x000C3D4F File Offset: 0x000C1F4F
			public Instance(Character owner, StoneMask ability) : base(owner, ability)
			{
				this._inventory = owner.playerComponents.inventory.weapon;
			}

			// Token: 0x0600432A RID: 17194 RVA: 0x000C3D6F File Offset: 0x000C1F6F
			protected override void OnAttach()
			{
				this._inventory.onChanged += this.UpdateMultiplier;
				this.UpdateMultiplier(null, null);
			}

			// Token: 0x0600432B RID: 17195 RVA: 0x000C3D90 File Offset: 0x000C1F90
			protected override void OnDetach()
			{
				this._inventory.onChanged -= this.UpdateMultiplier;
				GameData.Currency.currencies[this.ability._type].multiplier.Remove(this);
			}

			// Token: 0x0600432C RID: 17196 RVA: 0x000C3DCC File Offset: 0x000C1FCC
			private void UpdateMultiplier(Weapon old, Weapon @new)
			{
				GameData.Currency.currencies[this.ability._type].multiplier.Remove(this);
				Rarity rarity = this._inventory.weapons[0].rarity;
				float num = this.ability._percentByRarity[rarity];
				if (this._inventory.weapons[1] != null)
				{
					Rarity rarity2 = this._inventory.weapons[1].rarity;
					if (rarity2 > rarity)
					{
						num = this.ability._percentByRarity[rarity2];
					}
				}
				GameData.Currency.currencies[this.ability._type].multiplier.AddOrUpdate(this, (double)num);
			}

			// Token: 0x04003361 RID: 13153
			private WeaponInventory _inventory;
		}

		// Token: 0x02000CFF RID: 3327
		[Serializable]
		private class PercentByRarity : EnumArray<Rarity, float>
		{
		}
	}
}
