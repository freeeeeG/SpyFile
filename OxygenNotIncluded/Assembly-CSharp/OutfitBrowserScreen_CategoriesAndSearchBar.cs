using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BB3 RID: 2995
[Serializable]
public class OutfitBrowserScreen_CategoriesAndSearchBar
{
	// Token: 0x06005D85 RID: 23941 RVA: 0x00223560 File Offset: 0x00221760
	public void InitializeWith(OutfitBrowserScreen outfitBrowserScreen)
	{
		this.outfitBrowserScreen = outfitBrowserScreen;
		this.clothingOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.selectOutfitType_Prefab.transform.parent.gameObject, true));
		this.clothingOutfitTypeButton.button.onClick += delegate()
		{
			this.SetOutfitType(ClothingOutfitUtility.OutfitType.Clothing);
		};
		this.clothingOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_equipment");
		KleiItemsUI.ConfigureTooltipOn(this.clothingOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_CLOTHING);
		this.atmosuitOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.selectOutfitType_Prefab.transform.parent.gameObject, true));
		this.atmosuitOutfitTypeButton.button.onClick += delegate()
		{
			this.SetOutfitType(ClothingOutfitUtility.OutfitType.AtmoSuit);
		};
		this.atmosuitOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_atmosuits");
		KleiItemsUI.ConfigureTooltipOn(this.atmosuitOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_ATMO_SUITS);
		this.searchTextField.onValueChanged.AddListener(delegate(string newFilter)
		{
			outfitBrowserScreen.state.Filter = newFilter;
		});
		this.searchTextField.transform.SetAsLastSibling();
		outfitBrowserScreen.state.OnCurrentOutfitTypeChanged += delegate()
		{
			if (outfitBrowserScreen.Config.onlyShowOutfitType.IsSome())
			{
				this.clothingOutfitTypeButton.root.gameObject.SetActive(false);
				this.atmosuitOutfitTypeButton.root.gameObject.SetActive(false);
				return;
			}
			this.clothingOutfitTypeButton.root.gameObject.SetActive(true);
			this.atmosuitOutfitTypeButton.root.gameObject.SetActive(true);
			this.clothingOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
			this.atmosuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
			ClothingOutfitUtility.OutfitType currentOutfitType = outfitBrowserScreen.state.CurrentOutfitType;
			if (currentOutfitType == ClothingOutfitUtility.OutfitType.Clothing)
			{
				this.clothingOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
				return;
			}
			if (currentOutfitType != ClothingOutfitUtility.OutfitType.AtmoSuit)
			{
				throw new NotImplementedException();
			}
			this.atmosuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
		};
	}

	// Token: 0x06005D86 RID: 23942 RVA: 0x002236F7 File Offset: 0x002218F7
	public void SetOutfitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		this.outfitBrowserScreen.state.CurrentOutfitType = outfitType;
	}

	// Token: 0x04003EF7 RID: 16119
	[NonSerialized]
	public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton clothingOutfitTypeButton;

	// Token: 0x04003EF8 RID: 16120
	[NonSerialized]
	public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton atmosuitOutfitTypeButton;

	// Token: 0x04003EF9 RID: 16121
	[NonSerialized]
	public OutfitBrowserScreen outfitBrowserScreen;

	// Token: 0x04003EFA RID: 16122
	public KButton selectOutfitType_Prefab;

	// Token: 0x04003EFB RID: 16123
	public KInputTextField searchTextField;

	// Token: 0x02001AE6 RID: 6886
	public enum SelectOutfitTypeButtonState
	{
		// Token: 0x04007AEC RID: 31468
		Disabled,
		// Token: 0x04007AED RID: 31469
		Unselected,
		// Token: 0x04007AEE RID: 31470
		Selected
	}

	// Token: 0x02001AE7 RID: 6887
	public readonly struct SelectOutfitTypeButton
	{
		// Token: 0x0600987F RID: 39039 RVA: 0x003417C8 File Offset: 0x0033F9C8
		public SelectOutfitTypeButton(OutfitBrowserScreen outfitBrowserScreen, GameObject rootGameObject)
		{
			this.outfitBrowserScreen = outfitBrowserScreen;
			this.root = rootGameObject.GetComponent<RectTransform>();
			this.button = rootGameObject.GetComponent<KButton>();
			this.buttonImage = rootGameObject.GetComponent<KImage>();
			this.icon = this.root.GetChild(0).GetComponent<Image>();
			global::Debug.Assert(this.root != null);
			global::Debug.Assert(this.button != null);
			global::Debug.Assert(this.buttonImage != null);
			global::Debug.Assert(this.icon != null);
		}

		// Token: 0x06009880 RID: 39040 RVA: 0x0034185C File Offset: 0x0033FA5C
		public void SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState state)
		{
			switch (state)
			{
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Disabled:
				this.button.isInteractable = false;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.notSelectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected:
				this.button.isInteractable = true;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.notSelectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected:
				this.button.isInteractable = true;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.selectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x04007AEF RID: 31471
		public readonly OutfitBrowserScreen outfitBrowserScreen;

		// Token: 0x04007AF0 RID: 31472
		public readonly RectTransform root;

		// Token: 0x04007AF1 RID: 31473
		public readonly KButton button;

		// Token: 0x04007AF2 RID: 31474
		public readonly KImage buttonImage;

		// Token: 0x04007AF3 RID: 31475
		public readonly Image icon;
	}
}
