using System;
using System.Runtime.CompilerServices;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009F0 RID: 2544
	public class HeadCategoryAttacher : AbilityAttacher
	{
		// Token: 0x0600361B RID: 13851 RVA: 0x000A0736 File Offset: 0x0009E936
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x000A0743 File Offset: 0x0009E943
		public override void StartAttach()
		{
			base.owner.playerComponents.inventory.weapon.onChanged += this.Check;
			this.Check(null, null);
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x000A0774 File Offset: 0x0009E974
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			base.owner.playerComponents.inventory.weapon.onChanged -= this.Check;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x000A07D4 File Offset: 0x0009E9D4
		private void Check(Weapon old, Weapon @new)
		{
			HeadCategoryAttacher.<>c__DisplayClass7_0 CS$<>8__locals1;
			CS$<>8__locals1.categoryCounts = new EnumArray<Weapon.Category, int>();
			EnumArray<Weapon.Category, int> categoryCounts = CS$<>8__locals1.categoryCounts;
			Weapon.Category key = this._category1;
			int i = categoryCounts[key];
			categoryCounts[key] = i + 1;
			EnumArray<Weapon.Category, int> categoryCounts2 = CS$<>8__locals1.categoryCounts;
			key = this._category2;
			i = categoryCounts2[key];
			categoryCounts2[key] = i + 1;
			foreach (Weapon weapon in base.owner.playerComponents.inventory.weapon.weapons)
			{
				if (!(weapon == null))
				{
					EnumArray<Weapon.Category, int> categoryCounts3 = CS$<>8__locals1.categoryCounts;
					key = weapon.category;
					int num = categoryCounts3[key];
					categoryCounts3[key] = num - 1;
				}
			}
			if (HeadCategoryAttacher.<Check>g__CanAttach|7_0(ref CS$<>8__locals1))
			{
				this.Attach();
				return;
			}
			this.Detach();
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x000A0898 File Offset: 0x0009EA98
		private void Attach()
		{
			if (this._attached)
			{
				return;
			}
			this._attached = true;
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x000A08C6 File Offset: 0x0009EAC6
		private void Detach()
		{
			if (!this._attached)
			{
				return;
			}
			this._attached = false;
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x06003621 RID: 13857 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x000A08F4 File Offset: 0x0009EAF4
		[CompilerGenerated]
		internal static bool <Check>g__CanAttach|7_0(ref HeadCategoryAttacher.<>c__DisplayClass7_0 A_0)
		{
			for (int i = 0; i < A_0.categoryCounts.Keys.Count; i++)
			{
				if (A_0.categoryCounts.Array[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04002B6F RID: 11119
		[SerializeField]
		private Weapon.Category _category1;

		// Token: 0x04002B70 RID: 11120
		[SerializeField]
		private Weapon.Category _category2;

		// Token: 0x04002B71 RID: 11121
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x04002B72 RID: 11122
		private bool _attached;
	}
}
