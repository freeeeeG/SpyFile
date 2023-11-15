using System;
using System.Collections.Generic;
using Characters.Controllers;
using Characters.Gear;
using Characters.Gear.Quintessences;
using UnityEngine;

namespace Characters.Player
{
	// Token: 0x020007FC RID: 2044
	public class QuintessenceInventory : MonoBehaviour
	{
		// Token: 0x14000069 RID: 105
		// (add) Token: 0x06002987 RID: 10631 RVA: 0x0007EC74 File Offset: 0x0007CE74
		// (remove) Token: 0x06002988 RID: 10632 RVA: 0x0007ECAC File Offset: 0x0007CEAC
		public event Action onChanged;

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002989 RID: 10633 RVA: 0x0007ECE1 File Offset: 0x0007CEE1
		public List<Quintessence> items { get; } = new List<Quintessence>
		{
			null
		};

		// Token: 0x0600298A RID: 10634 RVA: 0x0007ECEC File Offset: 0x0007CEEC
		private void Trim()
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

		// Token: 0x0600298B RID: 10635 RVA: 0x0007ED3C File Offset: 0x0007CF3C
		public void UseAt(int index)
		{
			Quintessence quintessence = this.items[index];
			if (quintessence != null)
			{
				quintessence.Use();
			}
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x0007ED68 File Offset: 0x0007CF68
		public bool TryEquip(Quintessence item)
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

		// Token: 0x0600298D RID: 10637 RVA: 0x0007EDAC File Offset: 0x0007CFAC
		public void EquipAt(Quintessence item, int index)
		{
			Quintessence quintessence = this.items[index];
			if (quintessence != null)
			{
				this._character.stat.DetachValues(quintessence.stat);
				quintessence.state = Gear.State.Dropped;
			}
			item.transform.parent = this._character.@base;
			item.transform.localPosition = Vector3.zero;
			this._character.stat.AttachValues(item.stat);
			this.items[index] = item;
			Action action = this.onChanged;
			if (action != null)
			{
				action();
			}
			item.SetOwner(this._character);
			item.state = Gear.State.Equipped;
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x0007EE5C File Offset: 0x0007D05C
		public bool Drop(int index)
		{
			Quintessence quintessence = this.items[index];
			if (quintessence == null)
			{
				return false;
			}
			this._character.stat.DetachValues(quintessence.stat);
			quintessence.state = Gear.State.Dropped;
			this.items[index] = null;
			Action action = this.onChanged;
			if (action != null)
			{
				action();
			}
			return true;
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x0007EEC0 File Offset: 0x0007D0C0
		public bool Remove(int index)
		{
			Quintessence quintessence = this.items[index];
			if (!this.Drop(index))
			{
				return false;
			}
			quintessence.destructible = false;
			UnityEngine.Object.Destroy(quintessence.gameObject);
			return true;
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x0007EEF8 File Offset: 0x0007D0F8
		public void RemoveAll()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.Remove(i);
			}
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x0007EF24 File Offset: 0x0007D124
		public bool Discard(int index)
		{
			Quintessence quintessence = this.items[index];
			if (!this.Drop(index))
			{
				return false;
			}
			UnityEngine.Object.Destroy(quintessence.gameObject);
			return true;
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x0007EF58 File Offset: 0x0007D158
		public void Change(Quintessence old, Quintessence @new)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!(this.items[i] == null) && this.items[i].name.Equals(old.name, StringComparison.OrdinalIgnoreCase))
				{
					this.ChangeAt(@new, i);
				}
			}
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x0007EFB6 File Offset: 0x0007D1B6
		public void ChangeAt(Quintessence @new, int index)
		{
			this.Remove(index);
			this.EquipAt(@new, index);
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x0007EFC8 File Offset: 0x0007D1C8
		public int GetCountByRarity(Rarity rarity)
		{
			int num = 0;
			foreach (Quintessence quintessence in this.items)
			{
				if (!(quintessence == null) && quintessence.rarity == rarity)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x040023AC RID: 9132
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x040023AD RID: 9133
		[SerializeField]
		[GetComponent]
		private PlayerInput _input;
	}
}
