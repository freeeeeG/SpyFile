using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace GameResources
{
	// Token: 0x0200017E RID: 382
	public class GearResource : ScriptableObject
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x00017FB9 File Offset: 0x000161B9
		// (set) Token: 0x0600081A RID: 2074 RVA: 0x00017FC0 File Offset: 0x000161C0
		public static GearResource instance { get; private set; }

		// Token: 0x0600081B RID: 2075 RVA: 0x00017FC8 File Offset: 0x000161C8
		public Sprite GetGearThumbnail(string name)
		{
			Sprite result;
			this._gearThumbnailDictionary.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00017FE8 File Offset: 0x000161E8
		public Sprite GetWeaponHudMainIcon(string name)
		{
			Sprite result;
			this._weaponHudMainIconDictionary.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00018008 File Offset: 0x00016208
		public Sprite GetWeaponHudSubIcon(string name)
		{
			Sprite result;
			this._weaponHudSubIconDictionary.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00018028 File Offset: 0x00016228
		public Sprite GetQuintessenceHudIcon(string name)
		{
			Sprite result;
			this._quintessenceHudIconDictionary.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00018048 File Offset: 0x00016248
		public Sprite GetSkillIcon(string name)
		{
			Sprite result;
			this._skillIconDictionary.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00018068 File Offset: 0x00016268
		public Sprite GetItemBuffIcon(string name)
		{
			Sprite result;
			this._itemBuffIconDictionary.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x00018085 File Offset: 0x00016285
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x0001808D File Offset: 0x0001628D
		public ReadOnlyCollection<WeaponReference> weapons { get; private set; }

		// Token: 0x06000823 RID: 2083 RVA: 0x00018096 File Offset: 0x00016296
		public bool TryGetWeaponReferenceByName(string name, out WeaponReference reference)
		{
			return this._weaponDictionaryByName.TryGetValue(name, out reference);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x000180A5 File Offset: 0x000162A5
		public bool TryGetWeaponReferenceByGuid(string guid, out WeaponReference reference)
		{
			return this._weaponDictionaryByGuid.TryGetValue(guid, out reference);
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x000180B4 File Offset: 0x000162B4
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x000180BC File Offset: 0x000162BC
		public ReadOnlyCollection<ItemReference> items { get; private set; }

		// Token: 0x06000827 RID: 2087 RVA: 0x000180C5 File Offset: 0x000162C5
		public bool TryGetItemReferenceByName(string name, out ItemReference reference)
		{
			return this._itemDictionaryByName.TryGetValue(name, out reference);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x000180D4 File Offset: 0x000162D4
		public bool TryGetItemReferenceByGuid(string guid, out ItemReference reference)
		{
			return this._itemDictionaryByGuid.TryGetValue(guid, out reference);
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x000180E3 File Offset: 0x000162E3
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x000180EB File Offset: 0x000162EB
		public ReadOnlyCollection<EssenceReference> essences { get; private set; }

		// Token: 0x0600082B RID: 2091 RVA: 0x000180F4 File Offset: 0x000162F4
		public bool TryGetEssenceReferenceByName(string name, out EssenceReference reference)
		{
			return this._essenceDictionaryByName.TryGetValue(name, out reference);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00018103 File Offset: 0x00016303
		public bool TryGetEssenceReferenceByGuid(string guid, out EssenceReference reference)
		{
			return this._essenceDictionaryByGuid.TryGetValue(guid, out reference);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00018114 File Offset: 0x00016314
		public void Initialize()
		{
			GearResource.instance = this;
			base.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			this._gearThumbnailDictionary = this._gearThumbnails.ToDictionary((Sprite sprite) => sprite.name, StringComparer.OrdinalIgnoreCase);
			this._weaponHudMainIconDictionary = this._weaponHudMainIcons.ToDictionary((Sprite sprite) => sprite.name, StringComparer.OrdinalIgnoreCase);
			this._weaponHudSubIconDictionary = this._weaponHudSubIcons.ToDictionary((Sprite sprite) => sprite.name, StringComparer.OrdinalIgnoreCase);
			this._quintessenceHudIconDictionary = this._quintessenceHudIcons.ToDictionary((Sprite sprite) => sprite.name, StringComparer.OrdinalIgnoreCase);
			this._skillIconDictionary = this._skillIcons.ToDictionary((Sprite sprite) => sprite.name, StringComparer.OrdinalIgnoreCase);
			this._itemBuffIconDictionary = this._itemBuffIcons.ToDictionary((Sprite sprite) => sprite.name, StringComparer.OrdinalIgnoreCase);
			this.weapons = Array.AsReadOnly<WeaponReference>(this._weapons);
			this._weaponDictionaryByName = this.weapons.ToDictionary((WeaponReference weapon) => weapon.name, StringComparer.OrdinalIgnoreCase);
			this._weaponDictionaryByGuid = this.weapons.ToDictionary((WeaponReference weapon) => weapon.guid, StringComparer.OrdinalIgnoreCase);
			this.items = Array.AsReadOnly<ItemReference>(this._items);
			this._itemDictionaryByName = this.items.ToDictionary((ItemReference item) => item.name, StringComparer.OrdinalIgnoreCase);
			this._itemDictionaryByGuid = this.items.ToDictionary((ItemReference item) => item.guid, StringComparer.OrdinalIgnoreCase);
			this.essences = Array.AsReadOnly<EssenceReference>(this._essences);
			this._essenceDictionaryByName = this.essences.ToDictionary((EssenceReference quintessence) => quintessence.name, StringComparer.OrdinalIgnoreCase);
			this._essenceDictionaryByGuid = this.essences.ToDictionary((EssenceReference quintessence) => quintessence.guid, StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x04000664 RID: 1636
		[SerializeField]
		private Sprite[] _gearThumbnails;

		// Token: 0x04000665 RID: 1637
		private Dictionary<string, Sprite> _gearThumbnailDictionary;

		// Token: 0x04000666 RID: 1638
		[SerializeField]
		private Sprite[] _weaponHudMainIcons;

		// Token: 0x04000667 RID: 1639
		private Dictionary<string, Sprite> _weaponHudMainIconDictionary;

		// Token: 0x04000668 RID: 1640
		[SerializeField]
		private Sprite[] _weaponHudSubIcons;

		// Token: 0x04000669 RID: 1641
		private Dictionary<string, Sprite> _weaponHudSubIconDictionary;

		// Token: 0x0400066A RID: 1642
		[SerializeField]
		private Sprite[] _quintessenceHudIcons;

		// Token: 0x0400066B RID: 1643
		private Dictionary<string, Sprite> _quintessenceHudIconDictionary;

		// Token: 0x0400066C RID: 1644
		[SerializeField]
		private Sprite[] _skillIcons;

		// Token: 0x0400066D RID: 1645
		private Dictionary<string, Sprite> _skillIconDictionary;

		// Token: 0x0400066E RID: 1646
		[SerializeField]
		private Sprite[] _itemBuffIcons;

		// Token: 0x0400066F RID: 1647
		private Dictionary<string, Sprite> _itemBuffIconDictionary;

		// Token: 0x04000670 RID: 1648
		[SerializeField]
		private WeaponReference[] _weapons;

		// Token: 0x04000672 RID: 1650
		private Dictionary<string, WeaponReference> _weaponDictionaryByName;

		// Token: 0x04000673 RID: 1651
		private Dictionary<string, WeaponReference> _weaponDictionaryByGuid;

		// Token: 0x04000674 RID: 1652
		[SerializeField]
		private ItemReference[] _items;

		// Token: 0x04000676 RID: 1654
		private Dictionary<string, ItemReference> _itemDictionaryByName;

		// Token: 0x04000677 RID: 1655
		private Dictionary<string, ItemReference> _itemDictionaryByGuid;

		// Token: 0x04000678 RID: 1656
		[SerializeField]
		private EssenceReference[] _essences;

		// Token: 0x0400067A RID: 1658
		private Dictionary<string, EssenceReference> _essenceDictionaryByName;

		// Token: 0x0400067B RID: 1659
		private Dictionary<string, EssenceReference> _essenceDictionaryByGuid;
	}
}
