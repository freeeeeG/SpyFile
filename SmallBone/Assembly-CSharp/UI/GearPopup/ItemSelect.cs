using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Characters;
using Characters.Controllers;
using Characters.Gear.Items;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using FX;
using InControl;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UserInput;

namespace UI.GearPopup
{
	// Token: 0x02000452 RID: 1106
	public class ItemSelect : Dialogue
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001510 RID: 5392 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool closeWithPauseKey
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x00042220 File Offset: 0x00040420
		private void Awake()
		{
			this._navigation.onItemSelected += this.OnItemSelected;
			Selectable[] componentsInChildren = base.GetComponentsInChildren<Selectable>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.AddComponent<PlaySoundOnSelected>().soundInfo = this._selectSound;
			}
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x00042274 File Offset: 0x00040474
		private new void Update()
		{
			base.Update();
			this.UpdateContainerSizeAndPosition(this._fieldGearContainer);
			this.UpdateContainerSizeAndPosition(this._inventoryGearContainer);
			this.FixFocus();
			if (KeyMapper.Map.Interaction.WasPressed || (KeyMapper.Map.SimplifiedLastInputType == BindingSourceType.KeyBindingSource && KeyMapper.Map.Submit.WasPressed))
			{
				this._itemInventory.Drop(this._navigation.selectedItemIndex);
				this._fieldItem.dropped.InteractWith(this._player);
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00042310 File Offset: 0x00040510
		private void FixFocus()
		{
			if (!base.focused)
			{
				return;
			}
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null && currentSelectedGameObject.GetComponent<ItemSelectElement>() != null)
			{
				return;
			}
			ItemSelectElement component = this._defaultFocus.GetComponent<ItemSelectElement>();
			EventSystem.current.SetSelectedGameObject(component.gameObject);
			Action onSelected = component.onSelected;
			if (onSelected == null)
			{
				return;
			}
			onSelected();
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x00042375 File Offset: 0x00040575
		protected override void OnEnable()
		{
			base.OnEnable();
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._openSound, Vector3.zero);
			PlayerInput.blocked.Attach(this);
			Chronometer.global.AttachTimeScale(this, 0f);
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x000423AE File Offset: 0x000405AE
		protected override void OnDisable()
		{
			base.OnDisable();
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._closeSound, Vector3.zero);
			PlayerInput.blocked.Detach(this);
			Chronometer.global.DetachTimeScale(this);
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x000423E4 File Offset: 0x000405E4
		private void OnItemSelected(int index)
		{
			Item item = this._itemInventory.items[index];
			this.SetInventoryGear(item);
			EnumArray<Inscription.Key, int> delta = new EnumArray<Inscription.Key, int>();
			EnumArray<Inscription.Key, int> enumArray = new EnumArray<Inscription.Key, int>();
			Inscription.Key key;
			int i;
			foreach (Item item2 in this._itemInventory.items)
			{
				if (!(item2 == null) && !(item2 == item))
				{
					EnumArray<Inscription.Key, int> enumArray2 = enumArray;
					key = item2.keyword1;
					i = enumArray2[key];
					enumArray2[key] = i + 1;
					EnumArray<Inscription.Key, int> enumArray3 = enumArray;
					key = item2.keyword2;
					i = enumArray3[key];
					enumArray3[key] = i + 1;
				}
			}
			EnumArray<Inscription.Key, int> enumArray4 = enumArray;
			key = this._fieldItem.keyword1;
			i = enumArray4[key];
			enumArray4[key] = i + 1;
			EnumArray<Inscription.Key, int> enumArray5 = enumArray;
			key = this._fieldItem.keyword2;
			i = enumArray5[key];
			enumArray5[key] = i + 1;
			foreach (Item item3 in this._itemInventory.items)
			{
				if (!(item3 == null) && !(item3 == item))
				{
					item3.EvaluateBonusKeyword(enumArray);
				}
			}
			this._fieldItem.EvaluateBonusKeyword(enumArray);
			foreach (Inscription.Key key2 in Inscription.keys)
			{
				delta[key2] = enumArray[key2] + this._synergy.inscriptions[key2].bonusCount - this._synergy.inscriptions[key2].count;
			}
			KeyValuePair<Inscription.Key, int>[] array = (from pair in enumArray.ToKeyValuePairs()
			where pair.Value > 0 || delta[pair.Key] != 0
			select pair).OrderByDescending(delegate(KeyValuePair<Inscription.Key, int> keywordCount)
			{
				ReadOnlyCollection<int> steps = this._synergy.inscriptions[keywordCount.Key].steps;
				if (keywordCount.Value == steps[steps.Count - 1])
				{
					return 2;
				}
				if (keywordCount.Value >= steps[1])
				{
					return 1;
				}
				return 0;
			}).ThenByDescending((KeyValuePair<Inscription.Key, int> keywordCount) => keywordCount.Value).ToArray<KeyValuePair<Inscription.Key, int>>();
			KeywordElement[] keywordElements = this._keywordElements;
			for (i = 0; i < keywordElements.Length; i++)
			{
				keywordElements[i].gameObject.SetActive(false);
			}
			int num = 0;
			if (array.Length <= 12)
			{
				this._moreinscriptionFrame.SetActive(false);
				num = 12;
			}
			else
			{
				this._moreinscriptionFrame.SetActive(true);
			}
			int num2 = Math.Min(array.Length, this._keywordElements.Length);
			for (int j = 0; j < num2; j++)
			{
				this._keywordElements[j + num].gameObject.SetActive(true);
				this._keywordElements[j + num].Set(array[j].Key, delta[array[j].Key]);
			}
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00042704 File Offset: 0x00040904
		private void UpdateContainerSizeAndPosition(RectTransform container)
		{
			Vector2 vector = container.sizeDelta / 2f;
			vector.x *= container.lossyScale.x;
			vector.y *= container.lossyScale.y;
			float num = this._canvas.sizeDelta.x * this._canvas.localScale.x;
			Vector3 position = container.position;
			position.x = Mathf.Clamp(position.x, vector.x, num - vector.x);
			container.position = position;
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x000427A0 File Offset: 0x000409A0
		public void Open(Item fieldItem)
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
			this._itemInventory = this._player.playerComponents.inventory.item;
			this._synergy = this._player.playerComponents.inventory.synergy;
			this._playerInput = this._player.GetComponent<PlayerInput>();
			this._fieldItem = fieldItem;
			this._fieldGearPopup.Set(fieldItem);
			base.Open();
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x00042822 File Offset: 0x00040A22
		public void SetInventoryGear(Item item)
		{
			this._inventoryGearPopup.Set(item);
		}

		// Token: 0x04001261 RID: 4705
		[SerializeField]
		private RectTransform _canvas;

		// Token: 0x04001262 RID: 4706
		[Space]
		[SerializeField]
		private GameObject _moreinscriptionFrame;

		// Token: 0x04001263 RID: 4707
		[SerializeField]
		private KeywordElement[] _keywordElements;

		// Token: 0x04001264 RID: 4708
		[SerializeField]
		[Space]
		private RectTransform _fieldGearContainer;

		// Token: 0x04001265 RID: 4709
		[SerializeField]
		private GearPopupForItemSelection _fieldGearPopup;

		// Token: 0x04001266 RID: 4710
		[SerializeField]
		[Space]
		private RectTransform _inventoryGearContainer;

		// Token: 0x04001267 RID: 4711
		[SerializeField]
		private GearPopupForItemSelection _inventoryGearPopup;

		// Token: 0x04001268 RID: 4712
		[Space]
		[SerializeField]
		private ItemSelectNavigation _navigation;

		// Token: 0x04001269 RID: 4713
		[SerializeField]
		[Header("Sound")]
		private SoundInfo _openSound;

		// Token: 0x0400126A RID: 4714
		[SerializeField]
		private SoundInfo _closeSound;

		// Token: 0x0400126B RID: 4715
		[SerializeField]
		private SoundInfo _selectSound;

		// Token: 0x0400126C RID: 4716
		private Item _fieldItem;

		// Token: 0x0400126D RID: 4717
		private Character _player;

		// Token: 0x0400126E RID: 4718
		private ItemInventory _itemInventory;

		// Token: 0x0400126F RID: 4719
		private Synergy _synergy;

		// Token: 0x04001270 RID: 4720
		private PlayerInput _playerInput;
	}
}
