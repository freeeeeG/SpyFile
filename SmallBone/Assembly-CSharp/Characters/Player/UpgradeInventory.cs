using System;
using System.Collections.Generic;
using Characters.Gear.Upgrades;
using Platforms;
using UnityEngine;

namespace Characters.Player
{
	// Token: 0x020007FD RID: 2045
	public sealed class UpgradeInventory : MonoBehaviour
	{
		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06002996 RID: 10646 RVA: 0x0007F04C File Offset: 0x0007D24C
		// (remove) Token: 0x06002997 RID: 10647 RVA: 0x0007F084 File Offset: 0x0007D284
		public event Action onChanged;

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002998 RID: 10648 RVA: 0x0007F0B9 File Offset: 0x0007D2B9
		public List<UpgradeObject> upgrades { get; } = new List<UpgradeObject>
		{
			null,
			null,
			null,
			null
		};

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06002999 RID: 10649 RVA: 0x0007F0C1 File Offset: 0x0007D2C1
		public List<UpgradeObject> consumableUpgrades { get; } = new List<UpgradeObject>();

		// Token: 0x0600299A RID: 10650 RVA: 0x0007F0CC File Offset: 0x0007D2CC
		public int IndexOf(UpgradeObject upgrade)
		{
			for (int i = 0; i < this.upgrades.Count; i++)
			{
				if (!(this.upgrades[i] == null) && this.upgrades[i].reference.name.Equals(upgrade.reference.name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x0007F130 File Offset: 0x0007D330
		public int IndexOf(UpgradeResource.Reference reference)
		{
			for (int i = 0; i < this.upgrades.Count; i++)
			{
				if (!(this.upgrades[i] == null) && reference.name.Equals(this.upgrades[i].name, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x0007F18C File Offset: 0x0007D38C
		public void Trim()
		{
			int num = 0;
			for (int i = 0; i < this.upgrades.Count; i++)
			{
				if (this.upgrades[i] == null)
				{
					num++;
				}
				else
				{
					this.upgrades.Swap(i, i - num);
				}
			}
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x0007F1DC File Offset: 0x0007D3DC
		public bool TryEquip(UpgradeObject upgrade)
		{
			for (int i = 0; i < this.upgrades.Count; i++)
			{
				if (this.upgrades[i] == null)
				{
					this.EquipAt(upgrade, i);
					return true;
				}
				if (this.upgrades[i].name.Equals(upgrade.name, StringComparison.OrdinalIgnoreCase))
				{
					this.EquipAt(upgrade, i);
					return true;
				}
			}
			Debug.LogError("획득 실패!");
			return false;
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x0007F254 File Offset: 0x0007D454
		public bool TryEquip(UpgradeResource.Reference reference)
		{
			for (int i = 0; i < this.upgrades.Count; i++)
			{
				if (this.upgrades[i] == null)
				{
					this.EquipAt(reference, i);
					return true;
				}
				if (reference.name.Equals(this.upgrades[i].name, StringComparison.OrdinalIgnoreCase))
				{
					this.EquipAt(reference, i);
					return true;
				}
			}
			Debug.LogError("획득 실패!");
			return false;
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x0007F2CC File Offset: 0x0007D4CC
		public void RemoveAll()
		{
			for (int i = 0; i < this.upgrades.Count; i++)
			{
				this.Remove(i);
			}
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x0007F2F8 File Offset: 0x0007D4F8
		public bool Remove(int index)
		{
			UpgradeObject upgradeObject = this.upgrades[index];
			if (!upgradeObject)
			{
				return false;
			}
			UnityEngine.Object.Destroy(upgradeObject.gameObject);
			this.upgrades[index] = null;
			return true;
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x0007F338 File Offset: 0x0007D538
		public void EquipAt(UpgradeResource.Reference upgrade, int index)
		{
			this.Remove(index);
			UpgradeObject upgradeObject = upgrade.Instantiate();
			upgradeObject.transform.parent = this._character.@base;
			upgradeObject.transform.localPosition = Vector3.zero;
			upgradeObject.gameObject.name = upgrade.name;
			this.upgrades[index] = upgradeObject;
			Action action = this.onChanged;
			if (action != null)
			{
				action();
			}
			upgradeObject.Attach(this._character);
			this.CheckBestConditionAcheivement();
			this.CheckCursedOneAcheivement();
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x0007F3C4 File Offset: 0x0007D5C4
		public void EquipAt(UpgradeObject upgrade, int index)
		{
			this.Remove(index);
			UpgradeObject upgradeObject = UnityEngine.Object.Instantiate<UpgradeObject>(upgrade);
			upgradeObject.transform.parent = this._character.@base;
			upgradeObject.transform.localPosition = Vector3.zero;
			upgradeObject.gameObject.name = upgrade.gameObject.name;
			this.upgrades[index] = upgradeObject;
			Action action = this.onChanged;
			if (action != null)
			{
				action();
			}
			upgradeObject.Attach(this._character);
			this.CheckBestConditionAcheivement();
			this.CheckCursedOneAcheivement();
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x0007F454 File Offset: 0x0007D654
		public bool Has(UpgradeResource.Reference reference)
		{
			for (int i = 0; i < this.upgrades.Count; i++)
			{
				if (!(this.upgrades[i] == null) && this.upgrades[i].reference.name.Equals(reference.name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x0007F4B4 File Offset: 0x0007D6B4
		public bool Has(UpgradeObject upgrade)
		{
			for (int i = 0; i < this.upgrades.Count; i++)
			{
				if (!(this.upgrades[i] == null) && this.upgrades[i].reference.name.Equals(upgrade.reference.name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x0007F518 File Offset: 0x0007D718
		private void CheckBestConditionAcheivement()
		{
			int num = 0;
			while (num < this.upgrades.Count && !(this.upgrades[num] == null))
			{
				if (num == this.upgrades.Count - 1)
				{
					Achievement.Type.BestCondition.Set();
				}
				num++;
			}
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x0007F568 File Offset: 0x0007D768
		private void CheckCursedOneAcheivement()
		{
			int num = 0;
			while (num < this.upgrades.Count && !(this.upgrades[num] == null) && this.upgrades[num].type == UpgradeObject.Type.Cursed)
			{
				if (num == this.upgrades.Count - 1)
				{
					Achievement.Type.TheCursedOne.Set();
				}
				num++;
			}
		}

		// Token: 0x040023B0 RID: 9136
		public const int upgradeCount = 4;

		// Token: 0x040023B1 RID: 9137
		[SerializeField]
		[GetComponent]
		private Character _character;
	}
}
