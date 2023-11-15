using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Abilities.Spirits;
using Characters.Gear;
using Characters.Gear.Items;
using GameResources;
using UnityEngine;

namespace Characters.Player
{
	// Token: 0x020007EE RID: 2030
	public class ItemInventory : MonoBehaviour
	{
		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06002923 RID: 10531 RVA: 0x0007D938 File Offset: 0x0007BB38
		// (remove) Token: 0x06002924 RID: 10532 RVA: 0x0007D970 File Offset: 0x0007BB70
		public event Action onChanged;

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06002925 RID: 10533 RVA: 0x0007D9A5 File Offset: 0x0007BBA5
		public List<Item> items { get; } = new List<Item>
		{
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};

		// Token: 0x06002926 RID: 10534 RVA: 0x0007D9AD File Offset: 0x0007BBAD
		private void Awake()
		{
			this._spirits = new List<Spirit>(this._slots.Length);
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x0007D9C4 File Offset: 0x0007BBC4
		public int IndexOf(Item item)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] == item)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x0007DA00 File Offset: 0x0007BC00
		public void Trim()
		{
			int num = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] == null)
				{
					num++;
				}
				else
				{
					this.items.Swap(i, i - num);
				}
			}
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x0007DA50 File Offset: 0x0007BC50
		public bool TryEquip(Item item)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] == null)
				{
					this.EquipAt(item, i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x0007DA94 File Offset: 0x0007BC94
		public void RemoveAll()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.Remove(i);
			}
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x0007DAC0 File Offset: 0x0007BCC0
		public void Remove(Item item)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!(this.items[i] == null) && !(this.items[i] != item))
				{
					this.Remove(i);
				}
			}
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x0007DB14 File Offset: 0x0007BD14
		public bool Drop(int index)
		{
			Item item = this.items[index];
			if (item == null)
			{
				return false;
			}
			this._character.stat.DetachValues(item.stat);
			item.state = Gear.State.Dropped;
			this.items[index] = null;
			Action action = this.onChanged;
			if (action != null)
			{
				action();
			}
			return true;
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x0007DB78 File Offset: 0x0007BD78
		public bool Remove(int index)
		{
			Item item = this.items[index];
			if (!this.Drop(index))
			{
				return false;
			}
			item.destructible = false;
			UnityEngine.Object.Destroy(item.gameObject);
			return true;
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x0007DBB0 File Offset: 0x0007BDB0
		public bool Discard(Item item)
		{
			int num = this.IndexOf(item);
			return num != -1 && this.Discard(num);
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x0007DBD4 File Offset: 0x0007BDD4
		public bool Discard(int index)
		{
			Item item = this.items[index];
			if (!this.Drop(index))
			{
				return false;
			}
			UnityEngine.Object.Destroy(item.gameObject);
			return true;
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x0007DC08 File Offset: 0x0007BE08
		public void EquipAt(Item item, int index)
		{
			this.Drop(index);
			item.transform.parent = this._character.@base;
			item.transform.localPosition = Vector3.zero;
			this._character.stat.AttachValues(item.stat);
			this.items[index] = item;
			item.SetOwner(this._character);
			item.state = Gear.State.Equipped;
			Action action = this.onChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x0007DC8C File Offset: 0x0007BE8C
		public void Change(Item old, Item @new)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!(this.items[i] == null) && !(this.items[i] != old))
				{
					this.ChangeAt(@new, i);
				}
			}
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x0007DCDF File Offset: 0x0007BEDF
		public void ChangeAt(Item @new, int index)
		{
			this.Remove(index);
			this.EquipAt(@new, index);
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x0007DCF1 File Offset: 0x0007BEF1
		public void AttachSpirit(Spirit spirit)
		{
			this._spirits.Add(spirit);
			this.SortSpiritPositions();
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x0007DD05 File Offset: 0x0007BF05
		public void DetachSpirit(Spirit spirit)
		{
			this._spirits.Remove(spirit);
			this.SortSpiritPositions();
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x0007DD1C File Offset: 0x0007BF1C
		private void SortSpiritPositions()
		{
			for (int i = 0; i < this._spirits.Count; i++)
			{
				this._spirits[i].targetPosition = this._slots[i];
			}
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x0007DD58 File Offset: 0x0007BF58
		public int GetItemCountByRarity(Rarity rarity)
		{
			int num = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!(this.items[i] == null) && this.items[i].rarity == rarity)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x0007DDAC File Offset: 0x0007BFAC
		public int GetItemCountByTag(Gear.Tag tag)
		{
			int num = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!(this.items[i] == null) && this.items[i].gearTag.HasFlag(tag))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x0007DE10 File Offset: 0x0007C010
		public Item GetRandomItem(System.Random random)
		{
			int num = (from item in this.items
			where item != null
			select item).Count<Item>();
			if (num == 0)
			{
				return null;
			}
			int index = random.Next(0, num);
			return this.items[index];
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x0007DE68 File Offset: 0x0007C068
		public bool Has(Item item)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!(this.items[i] == null) && this.items[i].name.Equals(item.name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x0007DEC4 File Offset: 0x0007C0C4
		public bool Has(string guid)
		{
			ItemReference itemReference;
			if (!GearResource.instance.TryGetItemReferenceByGuid(guid, out itemReference))
			{
				return false;
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!(this.items[i] == null) && this.items[i].name.Equals(itemReference.name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x0007DF30 File Offset: 0x0007C130
		public bool HasGroup(Item item)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!(this.items[i] == null))
				{
					if (this.items[i].name.Equals(item.name, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
					foreach (string value in this.items[i].groupItemKeys)
					{
						if (item.name.Equals(value, StringComparison.OrdinalIgnoreCase))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0400237C RID: 9084
		public const int itemCount = 9;

		// Token: 0x0400237D RID: 9085
		[SerializeField]
		[GetComponent]
		private Character _character;

		// Token: 0x0400237E RID: 9086
		[SerializeField]
		private Transform[] _slots;

		// Token: 0x0400237F RID: 9087
		private List<Spirit> _spirits;
	}
}
