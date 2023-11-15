using System;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.GearPopup
{
	// Token: 0x02000456 RID: 1110
	public class ItemSelectNavigation : MonoBehaviour
	{
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06001524 RID: 5412 RVA: 0x000428FC File Offset: 0x00040AFC
		// (remove) Token: 0x06001525 RID: 5413 RVA: 0x00042934 File Offset: 0x00040B34
		public event Action<int> onItemSelected;

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x00042969 File Offset: 0x00040B69
		// (set) Token: 0x06001527 RID: 5415 RVA: 0x00042971 File Offset: 0x00040B71
		public int selectedItemIndex { get; private set; }

		// Token: 0x06001528 RID: 5416 RVA: 0x0004297C File Offset: 0x00040B7C
		private void OnEnable()
		{
			ItemInventory item = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.item;
			for (int i = 0; i < this._items.Length; i++)
			{
				ItemSelectElement itemSelectElement = this._items[i];
				itemSelectElement.SetIcon(item.items[i].icon);
				int cachedIndex = i;
				itemSelectElement.onSelected = delegate()
				{
					this.selectedItemIndex = cachedIndex;
					Action<int> action = this.onItemSelected;
					if (action == null)
					{
						return;
					}
					action(cachedIndex);
				};
			}
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x00042A00 File Offset: 0x00040C00
		private void Update()
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject == null)
			{
				return;
			}
			this._focus.rectTransform.position = currentSelectedGameObject.transform.position;
		}

		// Token: 0x04001278 RID: 4728
		[SerializeField]
		private Image _focus;

		// Token: 0x04001279 RID: 4729
		[SerializeField]
		private ItemSelectElement[] _items;
	}
}
