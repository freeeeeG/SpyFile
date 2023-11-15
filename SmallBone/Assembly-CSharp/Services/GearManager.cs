using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Gear.Weapons;
using Characters.Player;
using GameResources;
using Singletons;
using UnityEngine;

namespace Services
{
	// Token: 0x02000135 RID: 309
	public class GearManager : MonoBehaviour
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000600 RID: 1536 RVA: 0x000115F8 File Offset: 0x0000F7F8
		// (remove) Token: 0x06000601 RID: 1537 RVA: 0x00011630 File Offset: 0x0000F830
		public event Action onItemInstanceChanged;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000602 RID: 1538 RVA: 0x00011668 File Offset: 0x0000F868
		// (remove) Token: 0x06000603 RID: 1539 RVA: 0x000116A0 File Offset: 0x0000F8A0
		public event Action onEssenceInstanceChanged;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000604 RID: 1540 RVA: 0x000116D8 File Offset: 0x0000F8D8
		// (remove) Token: 0x06000605 RID: 1541 RVA: 0x00011710 File Offset: 0x0000F910
		public event Action onWeaponInstanceChanged;

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00011745 File Offset: 0x0000F945
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x0001174D File Offset: 0x0000F94D
		public bool initialized { get; private set; }

		// Token: 0x06000608 RID: 1544 RVA: 0x00011758 File Offset: 0x0000F958
		public void Initialize()
		{
			GearResource instance = GearResource.instance;
			ReadOnlyCollection<ItemReference> items = instance.items;
			ReadOnlyCollection<EssenceReference> essences = instance.essences;
			ReadOnlyCollection<WeaponReference> weapons = instance.weapons;
			foreach (IGrouping<Rarity, ItemReference> grouping in from item in items
			group item by item.rarity)
			{
				this._items[grouping.Key] = grouping.ToArray<ItemReference>();
			}
			foreach (IGrouping<Rarity, EssenceReference> grouping2 in from quintessence in essences
			group quintessence by quintessence.rarity)
			{
				this._quintessences[grouping2.Key] = grouping2.ToArray<EssenceReference>();
			}
			foreach (IGrouping<Rarity, WeaponReference> grouping3 in from weapon in weapons
			group weapon by weapon.rarity)
			{
				this._weapons[grouping3.Key] = grouping3.ToArray<WeaponReference>();
			}
			this.initialized = true;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000118E0 File Offset: 0x0000FAE0
		public void RegisterItemInstance(Gear item)
		{
			this._itemInstances.Add(item);
			Action action = this.onItemInstanceChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x000118FE File Offset: 0x0000FAFE
		public void UnregisterItemInstance(Gear item)
		{
			this._itemInstances.Remove(item);
			Action action = this.onItemInstanceChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001191D File Offset: 0x0000FB1D
		public void RegisterEssenceInstance(Gear essence)
		{
			this._essenceInstances.Add(essence);
			Action action = this.onEssenceInstanceChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001193B File Offset: 0x0000FB3B
		public void UnregisterEssenceInstance(Gear essence)
		{
			this._essenceInstances.Remove(essence);
			Action action = this.onEssenceInstanceChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001195A File Offset: 0x0000FB5A
		public void RegisterWeaponInstance(Gear weapon)
		{
			this._weaponInstances.Add(weapon);
			Action action = this.onWeaponInstanceChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00011978 File Offset: 0x0000FB78
		public void UnregisterWeaponInstance(Gear weapon)
		{
			this._weaponInstances.Remove(weapon);
			Action action = this.onWeaponInstanceChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00011997 File Offset: 0x0000FB97
		public void DestroyDroppedInstaces()
		{
			GearManager.<DestroyDroppedInstaces>g__DestroyGearInstances|29_0(this._itemInstances);
			GearManager.<DestroyDroppedInstaces>g__DestroyGearInstances|29_0(this._essenceInstances);
			GearManager.<DestroyDroppedInstaces>g__DestroyGearInstances|29_0(this._weaponInstances);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x000119BC File Offset: 0x0000FBBC
		private void UpdateLockedGearList()
		{
			GearResource instance = GearResource.instance;
			ReadOnlyCollection<ItemReference> items = instance.items;
			ReadOnlyCollection<EssenceReference> essences = instance.essences;
			ReadOnlyCollection<WeaponReference> weapons = instance.weapons;
			List<GearReference> list = new List<GearReference>(items.Count + essences.Count + weapons.Count);
			list.AddRange(items);
			list.AddRange(essences);
			list.AddRange(weapons);
			foreach (IGrouping<Rarity, GearReference> grouping in from item in list
			group item by item.rarity)
			{
				this._lockedGears[grouping.Key] = grouping.Where((GearReference gear) => gear.obtainable && !gear.unlocked).ToList<GearReference>();
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00011AA8 File Offset: 0x0000FCA8
		public GearReference GetGearToUnlock(System.Random random, Rarity rarity)
		{
			this.UpdateLockedGearList();
			List<GearReference> list3 = this._lockedGears[rarity];
			if ((from list in this._lockedGears
			select list.Count).Sum() == 0)
			{
				return null;
			}
			GearReference result;
			if (this.TryGetGearToUnlock(random, rarity, out result))
			{
				return result;
			}
			List<Rarity> list2 = EnumValues<Rarity>.Values.ToList<Rarity>();
			int num = list2.IndexOf(rarity);
			if (rarity == Rarity.Common)
			{
				for (int i = 1; i < list2.Count; i++)
				{
					int index = (num + i) % list2.Count;
					if (this.TryGetGearToUnlock(random, list2[index], out result))
					{
						return result;
					}
				}
			}
			else
			{
				for (int j = 1; j < list2.Count; j++)
				{
					int num2 = num - j;
					if (num2 < 0)
					{
						num2 += list2.Count;
					}
					if (this.TryGetGearToUnlock(random, list2[num2], out result))
					{
						return result;
					}
				}
			}
			return null;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00011B94 File Offset: 0x0000FD94
		private bool TryGetGearToUnlock(System.Random random, Rarity rarity, out GearReference gearReference)
		{
			gearReference = null;
			List<GearReference> list2 = this._lockedGears[rarity];
			if ((from list in this._lockedGears
			select list.Count).Sum() == 0)
			{
				return false;
			}
			if (list2.Count == 0)
			{
				return false;
			}
			gearReference = list2.Random(random);
			return true;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00011BF8 File Offset: 0x0000FDF8
		public GearReference GetGearToTake(Rarity rarity)
		{
			switch (EnumValues<Gear.Type>.Values.Random<Gear.Type>())
			{
			case Gear.Type.Weapon:
				return this.GetWeaponToTake(rarity);
			case Gear.Type.Item:
				return this.GetItemToTake(rarity);
			case Gear.Type.Quintessence:
				return this.GetQuintessenceToTake(rarity);
			default:
				return null;
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00011C3D File Offset: 0x0000FE3D
		public ItemReference GetItemToTake(Rarity rarity)
		{
			return this.GetItemToTake(MMMaths.random, rarity);
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00011C4C File Offset: 0x0000FE4C
		public ItemReference GetItemToTake(System.Random random, Rarity rarity)
		{
			if (Singleton<Service>.Instance.levelManager.player == null)
			{
				return (from item in this._items[rarity]
				where item.obtainable && item.unlocked
				select item).Random(random);
			}
			ItemInventory item2 = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
			IEnumerable<ItemReference> enumerable = this._items[rarity].Where(delegate(ItemReference item)
			{
				if (!item.obtainable)
				{
					return false;
				}
				if (!item.unlocked)
				{
					return false;
				}
				for (int i = 0; i < this._itemInstances.Count; i++)
				{
					Gear gear = this._itemInstances[i];
					if (item.name.Equals(gear.name))
					{
						return false;
					}
					foreach (string value in gear.groupItemKeys)
					{
						if (item.name.Equals(value, StringComparison.OrdinalIgnoreCase))
						{
							return false;
						}
					}
				}
				return true;
			});
			if (enumerable.Count<ItemReference>() == 0)
			{
				return this.GetItemToTake(random, (rarity == Rarity.Common) ? (rarity + 1) : (rarity - 1));
			}
			return enumerable.Random(random);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00011D08 File Offset: 0x0000FF08
		public ItemReference GetItemToTake(System.Random random, Gear.Tag tag, bool optainable = true)
		{
			GearResource instance = GearResource.instance;
			if (Singleton<Service>.Instance.levelManager.player == null)
			{
				return (from item in instance.items
				where item.obtainable && item.unlocked && item.gearTag.HasFlag(tag)
				select item).Random(random);
			}
			ItemInventory item2 = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
			IEnumerable<ItemReference> enumerable = instance.items.Where(delegate(ItemReference item)
			{
				if (optainable && !item.obtainable)
				{
					return false;
				}
				if (!item.unlocked)
				{
					return false;
				}
				if (!item.gearTag.HasFlag(tag))
				{
					return false;
				}
				for (int i = 0; i < this._itemInstances.Count; i++)
				{
					Gear gear = this._itemInstances[i];
					if (item.name.Equals(gear.name))
					{
						return false;
					}
					foreach (string value in gear.groupItemKeys)
					{
						if (item.name.Equals(value, StringComparison.OrdinalIgnoreCase))
						{
							return false;
						}
					}
				}
				return true;
			});
			if (enumerable.Count<ItemReference>() == 0)
			{
				return this.GetItemToTake(random, Rarity.Common);
			}
			return enumerable.Random(random);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00011DBC File Offset: 0x0000FFBC
		public ItemReference GetOmenItems(System.Random random)
		{
			GearResource instance = GearResource.instance;
			Gear.Tag tag = Gear.Tag.Omen;
			Gear.Tag except = Gear.Tag.UpgradedOmen;
			if (Singleton<Service>.Instance.levelManager.player == null)
			{
				return (from item in instance.items
				where item.obtainable && item.unlocked && item.gearTag.HasFlag(tag)
				select item).Random(random);
			}
			ItemInventory item2 = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
			IEnumerable<ItemReference> enumerable = instance.items.Where(delegate(ItemReference item)
			{
				if (!item.gearTag.HasFlag(tag))
				{
					return false;
				}
				if (item.gearTag.HasFlag(except))
				{
					return false;
				}
				for (int i = 0; i < this._itemInstances.Count; i++)
				{
					Gear gear = this._itemInstances[i];
					if (item.name.Equals(gear.name))
					{
						return false;
					}
					foreach (string value in gear.groupItemKeys)
					{
						if (item.name.Equals(value, StringComparison.OrdinalIgnoreCase))
						{
							return false;
						}
					}
				}
				return true;
			});
			if (enumerable.Count<ItemReference>() == 0)
			{
				return this.GetItemToTake(random, Rarity.Common);
			}
			return enumerable.Random(random);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00011E6F File Offset: 0x0001006F
		public EssenceReference GetQuintessenceToTake(Rarity rarity)
		{
			return this.GetQuintessenceToTake(MMMaths.random, rarity);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00011E80 File Offset: 0x00010080
		public EssenceReference GetQuintessenceToTake(System.Random random, Rarity rarity)
		{
			if (Singleton<Service>.Instance.levelManager.player == null)
			{
				return (from essence in this._quintessences[rarity]
				where essence.obtainable && essence.unlocked
				select essence).Random(random);
			}
			QuintessenceInventory quintessence = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.quintessence;
			IEnumerable<EssenceReference> enumerable = this._quintessences[rarity].Where(delegate(EssenceReference essence)
			{
				if (!essence.obtainable)
				{
					return false;
				}
				if (!essence.unlocked)
				{
					return false;
				}
				for (int i = 0; i < this._essenceInstances.Count; i++)
				{
					Gear gear = this._essenceInstances[i];
					if (essence.name.Equals(gear.name))
					{
						return false;
					}
					foreach (string value in gear.groupItemKeys)
					{
						if (essence.name.Equals(value, StringComparison.OrdinalIgnoreCase))
						{
							return false;
						}
					}
				}
				return true;
			});
			if (enumerable.Count<EssenceReference>() == 0)
			{
				return this.GetQuintessenceToTake(random, rarity - 1);
			}
			return enumerable.Random(random);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00011F34 File Offset: 0x00010134
		private string StripAwakeNumber(string name)
		{
			int num = name.IndexOf('_');
			if (num == -1)
			{
				return name;
			}
			return name.Substring(0, num);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00011F58 File Offset: 0x00010158
		public ICollection<GearReference> GetGearListByRarity(Gear.Type type, Rarity rarity)
		{
			switch (type)
			{
			case Gear.Type.Weapon:
				return this.GetWeaponListByRarity(rarity);
			case Gear.Type.Item:
				return this.GetItemListByRarity(rarity);
			case Gear.Type.Quintessence:
				return this.GetEssenceListByRarity(rarity);
			default:
				return null;
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00011F88 File Offset: 0x00010188
		public ICollection<GearReference> GetItemListByRarity(Rarity rarity)
		{
			ICollection<GearReference> collection = new List<GearReference>();
			foreach (ItemReference itemReference in this._items[rarity])
			{
				if (itemReference.obtainable && itemReference.unlocked)
				{
					collection.Add(itemReference);
				}
			}
			return collection;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00011FD4 File Offset: 0x000101D4
		public ICollection<GearReference> GetWeaponListByRarity(Rarity rarity)
		{
			ICollection<GearReference> collection = new List<GearReference>();
			foreach (WeaponReference weaponReference in this._weapons[rarity])
			{
				if (weaponReference.obtainable && weaponReference.unlocked)
				{
					collection.Add(weaponReference);
				}
			}
			return collection;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00012020 File Offset: 0x00010220
		public ICollection<GearReference> GetEssenceListByRarity(Rarity rarity)
		{
			ICollection<GearReference> collection = new List<GearReference>();
			foreach (EssenceReference essenceReference in this._quintessences[rarity])
			{
				if (essenceReference.obtainable && essenceReference.unlocked)
				{
					collection.Add(essenceReference);
				}
			}
			return collection;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001206C File Offset: 0x0001026C
		public WeaponReference GetWeaponByName(string name)
		{
			foreach (WeaponReference weaponReference in GearResource.instance.weapons)
			{
				if (weaponReference.name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return weaponReference;
				}
			}
			return null;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000120CC File Offset: 0x000102CC
		public WeaponReference GetWeaponToTake(Rarity rarity)
		{
			return this.GetWeaponToTake(MMMaths.random, rarity);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000120DC File Offset: 0x000102DC
		public WeaponReference GetWeaponToTake(System.Random random, Rarity rarity)
		{
			if (Singleton<Service>.Instance.levelManager.player == null)
			{
				return (from weapon in this._weapons[rarity]
				where weapon.obtainable && weapon.unlocked
				select weapon).Random(random);
			}
			WeaponInventory weapon2 = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon;
			IEnumerable<WeaponReference> enumerable = this._weapons[rarity].Where(delegate(WeaponReference weapon)
			{
				if (!weapon.obtainable)
				{
					return false;
				}
				if (!weapon.unlocked)
				{
					return false;
				}
				for (int i = 0; i < this._weaponInstances.Count; i++)
				{
					if (this.StripAwakeNumber(weapon.name).Equals(this.StripAwakeNumber(this._weaponInstances[i].name)))
					{
						return false;
					}
				}
				return true;
			});
			if (enumerable.Count<WeaponReference>() == 0)
			{
				return this.GetWeaponToTake(random, rarity - 1);
			}
			return enumerable.Random(random);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00012190 File Offset: 0x00010390
		public WeaponReference GetWeaponByCategory(System.Random random, Rarity rarity, Weapon.Category category)
		{
			GearManager.<>c__DisplayClass48_0 CS$<>8__locals1 = new GearManager.<>c__DisplayClass48_0();
			CS$<>8__locals1.category = category;
			CS$<>8__locals1.<>4__this = this;
			IEnumerable<WeaponReference> enumerable = this._weapons[rarity].Where(new Func<WeaponReference, bool>(CS$<>8__locals1.<GetWeaponByCategory>g__Pass|0));
			if (enumerable.Count<WeaponReference>() == 0)
			{
				return this.GetWeaponByCategory(random, rarity - 1, CS$<>8__locals1.category);
			}
			return enumerable.Random(random);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x000121F0 File Offset: 0x000103F0
		public ItemReference GetItemByKey(string key)
		{
			Func<ItemReference, bool> <>9__0;
			foreach (IEnumerable<ItemReference> source in this._items)
			{
				Func<ItemReference, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ItemReference item) => item.name.Equals(key, StringComparison.OrdinalIgnoreCase)));
				}
				IEnumerable<ItemReference> enumerable = source.Where(predicate);
				if (enumerable.Count<ItemReference>() != 0)
				{
					ItemReference itemReference = enumerable.Random<ItemReference>();
					if (itemReference != null)
					{
						return itemReference;
					}
				}
			}
			return null;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00012284 File Offset: 0x00010484
		public ItemReference GetItemByKeyword(System.Random random, Rarity rarity, Inscription.Key key)
		{
			IEnumerable<ItemReference> enumerable = this._items[rarity].Where(delegate(ItemReference item)
			{
				if (!item.obtainable)
				{
					return false;
				}
				if (!item.unlocked)
				{
					return false;
				}
				foreach (string value in this._keywordRandomizerItems)
				{
					if (item.name.Equals(value, StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
				for (int j = 0; j < this._itemInstances.Count; j++)
				{
					Gear gear = this._itemInstances[j];
					if (item.name.Equals(gear.name, StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
					foreach (string value2 in gear.groupItemKeys)
					{
						if (item.name.Equals(value2, StringComparison.OrdinalIgnoreCase))
						{
							return false;
						}
					}
				}
				return item.prefabKeyword1.Equals(key) || item.prefabKeyword2.Equals(key);
			});
			if (enumerable.Count<ItemReference>() == 0)
			{
				return null;
			}
			ItemReference itemReference = enumerable.Random(random);
			if (itemReference != null)
			{
				return itemReference;
			}
			return null;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000122DC File Offset: 0x000104DC
		public ICollection<Item> GetItemInstanceByKeyword(Inscription.Key key)
		{
			this._itemInstancesCached.Clear();
			foreach (Gear gear in this._itemInstances)
			{
				Item item = (Item)gear;
				if (item.keyword1 == key || item.keyword2 == key)
				{
					this._itemInstancesCached.Add(item);
				}
			}
			return this._itemInstancesCached;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0001235C File Offset: 0x0001055C
		public bool CanDrop(Inscription.Key key)
		{
			int num = 0;
			foreach (Gear gear in this._itemInstances)
			{
				Item item = (Item)gear;
				if (!(item == null) && item.obtainable && (item.keyword1 == key || item.keyword2 == key))
				{
					num++;
				}
			}
			foreach (ItemReference itemReference in GearResource.instance.items)
			{
				if (itemReference != null && itemReference.obtainable && (itemReference.prefabKeyword1 == key || itemReference.prefabKeyword2 == key))
				{
					num--;
				}
			}
			return num < 0;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000124D4 File Offset: 0x000106D4
		[CompilerGenerated]
		internal static void <DestroyDroppedInstaces>g__DestroyGearInstances|29_0(List<Gear> instances)
		{
			for (int i = instances.Count - 1; i >= 0; i--)
			{
				Gear gear = instances[i];
				if (gear.state == Gear.State.Dropped && gear.currencyByDiscard != 0)
				{
					UnityEngine.Object.Destroy(gear.gameObject);
				}
			}
		}

		// Token: 0x04000484 RID: 1156
		private readonly EnumArray<Rarity, ItemReference[]> _items = new EnumArray<Rarity, ItemReference[]>();

		// Token: 0x04000485 RID: 1157
		private readonly EnumArray<Rarity, EssenceReference[]> _quintessences = new EnumArray<Rarity, EssenceReference[]>();

		// Token: 0x04000486 RID: 1158
		private readonly EnumArray<Rarity, WeaponReference[]> _weapons = new EnumArray<Rarity, WeaponReference[]>();

		// Token: 0x04000487 RID: 1159
		private readonly EnumArray<Rarity, List<GearReference>> _lockedGears = new EnumArray<Rarity, List<GearReference>>();

		// Token: 0x04000488 RID: 1160
		private readonly List<Gear> _itemInstances = new List<Gear>();

		// Token: 0x04000489 RID: 1161
		private readonly List<Gear> _essenceInstances = new List<Gear>();

		// Token: 0x0400048A RID: 1162
		private readonly List<Gear> _weaponInstances = new List<Gear>();

		// Token: 0x0400048F RID: 1167
		private List<Item> _itemInstancesCached = new List<Item>();

		// Token: 0x04000490 RID: 1168
		private string[] _keywordRandomizerItems = new string[]
		{
			"PrincesBox",
			"PrincesBox_2",
			"CloneStamp",
			"CloneStamp_2"
		};
	}
}
