using System;
using System.Linq;
using Characters.Gear.Upgrades;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace UI.Upgrades
{
	// Token: 0x020003F3 RID: 1011
	public sealed class UpgradedContainer : MonoBehaviour
	{
		// Token: 0x0600130C RID: 4876 RVA: 0x000391C8 File Offset: 0x000373C8
		public void Set(Panel panel)
		{
			UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
			for (int i = 0; i < this._elements.Length; i++)
			{
				this._elements[i].Set(panel, (upgrade.upgrades[i] == null) ? null : upgrade.upgrades[i].reference, false);
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x00039240 File Offset: 0x00037440
		public void Append(Panel panel)
		{
			UpgradeInventory upgrade = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade;
			int num = upgrade.upgrades.Count((UpgradeObject target) => target != null);
			for (int i = 0; i < this._elements.Length; i++)
			{
				this._elements[i].Set(panel, (upgrade.upgrades[i] == null) ? null : upgrade.upgrades[i].reference, i == num - 1);
			}
			panel.Focus(this._elements[num - 1].selectable);
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x000392FC File Offset: 0x000374FC
		public UpgradedElement GetFocusElementOnRemoved(int removedIndex)
		{
			if (Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.upgrade.upgrades[removedIndex] != null)
			{
				return this._elements[removedIndex];
			}
			if (removedIndex > 0)
			{
				return this._elements[removedIndex - 1];
			}
			return null;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00039354 File Offset: 0x00037554
		public UpgradedElement GetDefaultFocusElement()
		{
			foreach (UpgradedElement upgradedElement in this._elements)
			{
				if (!(upgradedElement == null) && upgradedElement.reference != null && upgradedElement.reference.type != UpgradeObject.Type.Cursed && upgradedElement.selectable)
				{
					return upgradedElement;
				}
			}
			return null;
		}

		// Token: 0x04001005 RID: 4101
		[SerializeField]
		private UpgradedElement[] _elements;
	}
}
