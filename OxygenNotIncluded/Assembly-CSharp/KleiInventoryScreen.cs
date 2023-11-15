using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B32 RID: 2866
public class KleiInventoryScreen : KModalScreen
{
	// Token: 0x17000670 RID: 1648
	// (get) Token: 0x06005864 RID: 22628 RVA: 0x00205F8F File Offset: 0x0020418F
	// (set) Token: 0x06005865 RID: 22629 RVA: 0x00205F97 File Offset: 0x00204197
	private PermitResource SelectedPermit { get; set; }

	// Token: 0x17000671 RID: 1649
	// (get) Token: 0x06005866 RID: 22630 RVA: 0x00205FA0 File Offset: 0x002041A0
	// (set) Token: 0x06005867 RID: 22631 RVA: 0x00205FA8 File Offset: 0x002041A8
	private string SelectedCategoryId { get; set; }

	// Token: 0x06005868 RID: 22632 RVA: 0x00205FB4 File Offset: 0x002041B4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		base.ConsumeMouseScroll = true;
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = new List<GridLayoutGroup>()
		};
		this.galleryGridLayouter.overrideParentForSizeReference = this.galleryGridContent;
		InventoryOrganization.Initialize();
	}

	// Token: 0x06005869 RID: 22633 RVA: 0x00206027 File Offset: 0x00204227
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600586A RID: 22634 RVA: 0x00206049 File Offset: 0x00204249
	public override float GetSortKey()
	{
		return 20f;
	}

	// Token: 0x0600586B RID: 22635 RVA: 0x00206050 File Offset: 0x00204250
	protected override void OnActivate()
	{
		this.OnShow(true);
	}

	// Token: 0x0600586C RID: 22636 RVA: 0x00206059 File Offset: 0x00204259
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.InitConfig();
			this.ToggleDoublesOnly(0);
			this.ClearSearch();
		}
	}

	// Token: 0x0600586D RID: 22637 RVA: 0x00206078 File Offset: 0x00204278
	private void ToggleDoublesOnly(int newState)
	{
		this.showFilterState = newState;
		this.doublesOnlyToggle.ChangeState(this.showFilterState);
		this.doublesOnlyToggle.GetComponentInChildren<LocText>().text = this.showFilterState.ToString() + "+";
		string simpleTooltip = "";
		switch (this.showFilterState)
		{
		case 0:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_ALL_ITEMS;
			break;
		case 1:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_OWNED_ONLY;
			break;
		case 2:
			simpleTooltip = UI.KLEI_INVENTORY_SCREEN.TOOLTIP_VIEW_DOUBLES_ONLY;
			break;
		}
		ToolTip component = this.doublesOnlyToggle.GetComponent<ToolTip>();
		component.SetSimpleTooltip(simpleTooltip);
		component.refreshWhileHovering = true;
		component.forceRefresh = true;
		this.RefreshGallery();
	}

	// Token: 0x0600586E RID: 22638 RVA: 0x00206130 File Offset: 0x00204330
	private void InitConfig()
	{
		if (this.initConfigComplete)
		{
			return;
		}
		this.initConfigComplete = true;
		this.galleryGridLayouter.RequestGridResize();
		this.categoryListContent.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
		this.PopulateCategories();
		this.PopulateGallery();
		this.SelectCategory("BUILDINGS");
		this.searchField.onValueChanged.RemoveAllListeners();
		this.searchField.onValueChanged.AddListener(delegate(string value)
		{
			this.RefreshGallery();
		});
		this.clearSearchButton.ClearOnClick();
		this.clearSearchButton.onClick += this.ClearSearch;
		MultiToggle multiToggle = this.doublesOnlyToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			int newState = (this.showFilterState + 1) % 3;
			this.ToggleDoublesOnly(newState);
		}));
	}

	// Token: 0x0600586F RID: 22639 RVA: 0x00206203 File Offset: 0x00204403
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.ToggleDoublesOnly(0);
		this.ClearSearch();
		if (!this.initConfigComplete)
		{
			this.InitConfig();
		}
		this.RefreshUI();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshUI();
		});
	}

	// Token: 0x06005870 RID: 22640 RVA: 0x00206243 File Offset: 0x00204443
	private void ClearSearch()
	{
		this.searchField.text = "";
		this.searchField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.KLEI_INVENTORY_SCREEN.SEARCH_PLACEHOLDER;
		this.RefreshGallery();
	}

	// Token: 0x06005871 RID: 22641 RVA: 0x0020627A File Offset: 0x0020447A
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x06005872 RID: 22642 RVA: 0x00206288 File Offset: 0x00204488
	private void RefreshUI()
	{
		this.IS_ONLINE = ThreadedHttps<KleiAccount>.Instance.HasValidTicket();
		this.RefreshCategories();
		this.RefreshGallery();
		if (this.SelectedCategoryId.IsNullOrWhiteSpace())
		{
			this.SelectCategory("BUILDINGS");
		}
		this.RefreshDetails();
		this.RefreshBarterPanel();
	}

	// Token: 0x06005873 RID: 22643 RVA: 0x002062D5 File Offset: 0x002044D5
	private GameObject GetAvailableGridButton()
	{
		if (this.recycledGalleryGridButtons.Count == 0)
		{
			return Util.KInstantiateUI(this.gridItemPrefab, this.galleryGridContent.gameObject, true);
		}
		GameObject result = this.recycledGalleryGridButtons[0];
		this.recycledGalleryGridButtons.RemoveAt(0);
		return result;
	}

	// Token: 0x06005874 RID: 22644 RVA: 0x00206314 File Offset: 0x00204514
	private void RecycleGalleryGridButton(GameObject button)
	{
		button.GetComponent<MultiToggle>().onClick = null;
		this.recycledGalleryGridButtons.Add(button);
	}

	// Token: 0x06005875 RID: 22645 RVA: 0x00206330 File Offset: 0x00204530
	public void PopulateCategories()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.categoryToggles)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
		}
		this.categoryToggles.Clear();
		using (Dictionary<string, List<string>>.Enumerator enumerator2 = InventoryOrganization.categoryIdToSubcategoryIdsMap.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				string categoryId2;
				List<string> list;
				enumerator2.Current.Deconstruct(out categoryId2, out list);
				string categoryId = categoryId2;
				GameObject gameObject = Util.KInstantiateUI(this.categoryRowPrefab, this.categoryListContent.gameObject, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Label").SetText(InventoryOrganization.GetCategoryName(categoryId));
				component.GetReference<Image>("Icon").sprite = InventoryOrganization.categoryIdToIconMap[categoryId];
				MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
				MultiToggle multiToggle = component2;
				multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnMouseOverToggle));
				component2.onClick = delegate()
				{
					this.SelectCategory(categoryId);
				};
				this.categoryToggles.Add(categoryId, component2);
				this.SetCatogoryClickUISound(categoryId, component2);
			}
		}
	}

	// Token: 0x06005876 RID: 22646 RVA: 0x002064AC File Offset: 0x002046AC
	public void PopulateGallery()
	{
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			this.RecycleGalleryGridButton(keyValuePair.Value.gameObject);
		}
		this.galleryGridButtons.Clear();
		this.galleryGridLayouter.ImmediateSizeGridToScreenResolution();
		foreach (PermitResource permitResource in Db.Get().Permits.resources)
		{
			if (!permitResource.Id.StartsWith("visonly_"))
			{
				this.AddItemToGallery(permitResource);
			}
		}
		this.subcategories.Sort((KleiInventoryUISubcategory a, KleiInventoryUISubcategory b) => InventoryOrganization.subcategoryIdToPresentationDataMap[a.subcategoryID].sortKey.CompareTo(InventoryOrganization.subcategoryIdToPresentationDataMap[b.subcategoryID].sortKey));
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			kleiInventoryUISubcategory.gameObject.transform.SetAsLastSibling();
		}
		this.CollectSubcategoryGridLayouts();
		this.CloseSubcategory("UNCATEGORIZED");
	}

	// Token: 0x06005877 RID: 22647 RVA: 0x00206604 File Offset: 0x00204804
	private void CloseSubcategory(string subcategoryID)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = this.subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryID);
		if (kleiInventoryUISubcategory != null)
		{
			kleiInventoryUISubcategory.ToggleOpen(false);
		}
	}

	// Token: 0x06005878 RID: 22648 RVA: 0x00206648 File Offset: 0x00204848
	private void AddItemToSubcategoryUIContainer(GameObject itemButton, string subcategoryId)
	{
		KleiInventoryUISubcategory kleiInventoryUISubcategory = this.subcategories.Find((KleiInventoryUISubcategory match) => match.subcategoryID == subcategoryId);
		if (kleiInventoryUISubcategory == null)
		{
			kleiInventoryUISubcategory = Util.KInstantiateUI(this.subcategoryPrefab, this.galleryGridContent.gameObject, true).GetComponent<KleiInventoryUISubcategory>();
			kleiInventoryUISubcategory.subcategoryID = subcategoryId;
			this.subcategories.Add(kleiInventoryUISubcategory);
			kleiInventoryUISubcategory.SetIdentity(InventoryOrganization.GetSubcategoryName(subcategoryId), InventoryOrganization.subcategoryIdToPresentationDataMap[subcategoryId].icon);
		}
		itemButton.transform.SetParent(kleiInventoryUISubcategory.gridLayout.transform);
	}

	// Token: 0x06005879 RID: 22649 RVA: 0x002066F4 File Offset: 0x002048F4
	private void CollectSubcategoryGridLayouts()
	{
		this.galleryGridLayouter.OnSizeGridComplete = null;
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			this.galleryGridLayouter.targetGridLayouts.Add(kleiInventoryUISubcategory.gridLayout);
			GridLayouter gridLayouter = this.galleryGridLayouter;
			gridLayouter.OnSizeGridComplete = (System.Action)Delegate.Combine(gridLayouter.OnSizeGridComplete, new System.Action(kleiInventoryUISubcategory.RefreshDisplay));
		}
		this.galleryGridLayouter.RequestGridResize();
	}

	// Token: 0x0600587A RID: 22650 RVA: 0x00206794 File Offset: 0x00204994
	private void AddItemToGallery(PermitResource permit)
	{
		if (this.galleryGridButtons.ContainsKey(permit))
		{
			return;
		}
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		GameObject availableGridButton = this.GetAvailableGridButton();
		this.AddItemToSubcategoryUIContainer(availableGridButton, InventoryOrganization.GetPermitSubcategory(permit));
		HierarchyReferences component = availableGridButton.GetComponent<HierarchyReferences>();
		Image reference = component.GetReference<Image>("Icon");
		LocText reference2 = component.GetReference<LocText>("OwnedCountLabel");
		Image reference3 = component.GetReference<Image>("IsUnownedOverlay");
		MultiToggle component2 = availableGridButton.GetComponent<MultiToggle>();
		reference.sprite = permitPresentationInfo.sprite;
		if (permit.IsOwnable())
		{
			int ownedCount = PermitItems.GetOwnedCount(permit);
			reference2.text = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
			reference2.gameObject.SetActive(ownedCount > 0);
			reference3.gameObject.SetActive(ownedCount <= 0);
		}
		else
		{
			reference2.gameObject.SetActive(false);
			reference3.gameObject.SetActive(false);
		}
		MultiToggle multiToggle = component2;
		multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnMouseOverToggle));
		component2.onClick = delegate()
		{
			this.SelectItem(permit);
		};
		this.galleryGridButtons.Add(permit, component2);
		this.SetItemClickUISound(permit, component2);
		KleiItemsUI.ConfigureTooltipOn(availableGridButton, KleiItemsUI.GetTooltipStringFor(permit));
	}

	// Token: 0x0600587B RID: 22651 RVA: 0x00206912 File Offset: 0x00204B12
	public void SelectCategory(string categoryId)
	{
		if (InventoryOrganization.categoryIdToIsEmptyMap[categoryId])
		{
			return;
		}
		this.SelectedCategoryId = categoryId;
		this.galleryHeaderLabel.SetText(InventoryOrganization.GetCategoryName(categoryId));
		this.RefreshCategories();
		this.SelectDefaultCategoryItem();
	}

	// Token: 0x0600587C RID: 22652 RVA: 0x00206948 File Offset: 0x00204B48
	private void SelectDefaultCategoryItem()
	{
		foreach (KeyValuePair<PermitResource, MultiToggle> keyValuePair in this.galleryGridButtons)
		{
			if (InventoryOrganization.categoryIdToSubcategoryIdsMap[this.SelectedCategoryId].Contains(InventoryOrganization.GetPermitSubcategory(keyValuePair.Key)))
			{
				this.SelectItem(keyValuePair.Key);
				return;
			}
		}
		this.SelectItem(null);
	}

	// Token: 0x0600587D RID: 22653 RVA: 0x002069D0 File Offset: 0x00204BD0
	public void SelectItem(PermitResource permit)
	{
		this.SelectedPermit = permit;
		this.RefreshGallery();
		this.RefreshDetails();
		this.RefreshBarterPanel();
	}

	// Token: 0x0600587E RID: 22654 RVA: 0x002069EC File Offset: 0x00204BEC
	private void RefreshGallery()
	{
		string value = this.searchField.text.ToUpper();
		foreach (KeyValuePair<PermitResource, MultiToggle> self in this.galleryGridButtons)
		{
			PermitResource permitResource;
			MultiToggle multiToggle;
			self.Deconstruct(out permitResource, out multiToggle);
			PermitResource permitResource2 = permitResource;
			MultiToggle multiToggle2 = multiToggle;
			string permitSubcategory = InventoryOrganization.GetPermitSubcategory(permitResource2);
			bool flag = permitSubcategory == "UNCATEGORIZED" || InventoryOrganization.categoryIdToSubcategoryIdsMap[this.SelectedCategoryId].Contains(permitSubcategory);
			flag = (flag && (permitResource2.Name.ToUpper().Contains(value) || permitResource2.Id.ToUpper().Contains(value) || permitResource2.Description.ToUpper().Contains(value)));
			multiToggle2.ChangeState((permitResource2 == this.SelectedPermit) ? 1 : 0);
			HierarchyReferences component = multiToggle2.gameObject.GetComponent<HierarchyReferences>();
			LocText reference = component.GetReference<LocText>("OwnedCountLabel");
			Image reference2 = component.GetReference<Image>("IsUnownedOverlay");
			if (permitResource2.IsOwnable())
			{
				int ownedCount = PermitItems.GetOwnedCount(permitResource2);
				reference.text = UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT_ICON.Replace("{OwnedCount}", ownedCount.ToString());
				reference.gameObject.SetActive(ownedCount > 0);
				reference2.gameObject.SetActive(ownedCount <= 0);
				if (this.showFilterState == 2 && ownedCount < 2)
				{
					flag = false;
				}
				else if (this.showFilterState == 1 && ownedCount == 0)
				{
					flag = false;
				}
			}
			else
			{
				reference.gameObject.SetActive(false);
				reference2.gameObject.SetActive(false);
				if (this.showFilterState == 2)
				{
					flag = false;
				}
			}
			if (multiToggle2.gameObject.activeSelf != flag)
			{
				multiToggle2.gameObject.SetActive(flag);
			}
		}
		foreach (KleiInventoryUISubcategory kleiInventoryUISubcategory in this.subcategories)
		{
			kleiInventoryUISubcategory.RefreshDisplay();
		}
	}

	// Token: 0x0600587F RID: 22655 RVA: 0x00206C1C File Offset: 0x00204E1C
	private void RefreshCategories()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.categoryToggles)
		{
			keyValuePair.Value.ChangeState((keyValuePair.Key == this.SelectedCategoryId) ? 1 : 0);
			if (InventoryOrganization.categoryIdToIsEmptyMap[keyValuePair.Key])
			{
				keyValuePair.Value.ChangeState(2);
			}
			else
			{
				keyValuePair.Value.ChangeState((keyValuePair.Key == this.SelectedCategoryId) ? 1 : 0);
			}
		}
	}

	// Token: 0x06005880 RID: 22656 RVA: 0x00206CD4 File Offset: 0x00204ED4
	private void RefreshDetails()
	{
		PermitResource selectedPermit = this.SelectedPermit;
		PermitPresentationInfo permitPresentationInfo = selectedPermit.GetPermitPresentationInfo();
		this.permitVis.ConfigureWith(selectedPermit);
		this.selectionDetailsScrollRect.rectTransform().anchorMin = new Vector2(0f, 0f);
		this.selectionDetailsScrollRect.rectTransform().anchorMax = new Vector2(1f, 1f);
		this.selectionDetailsScrollRect.rectTransform().sizeDelta = new Vector2(-24f, 0f);
		this.selectionDetailsScrollRect.rectTransform().anchoredPosition = Vector2.zero;
		this.selectionDetailsScrollRect.content.rectTransform().sizeDelta = new Vector2(0f, this.selectionDetailsScrollRect.content.rectTransform().sizeDelta.y);
		this.selectionDetailsScrollRectScrollBarContainer.anchorMin = new Vector2(1f, 0f);
		this.selectionDetailsScrollRectScrollBarContainer.anchorMax = new Vector2(1f, 1f);
		this.selectionDetailsScrollRectScrollBarContainer.sizeDelta = new Vector2(24f, 0f);
		this.selectionDetailsScrollRectScrollBarContainer.anchoredPosition = Vector2.zero;
		this.selectionHeaderLabel.SetText(selectedPermit.Name);
		this.selectionNameLabel.SetText(selectedPermit.Name);
		this.selectionDescriptionLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(selectedPermit.Description));
		this.selectionDescriptionLabel.SetText(selectedPermit.Description);
		this.selectionFacadeForLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(permitPresentationInfo.facadeFor));
		this.selectionFacadeForLabel.SetText(permitPresentationInfo.facadeFor);
		string text = UI.KLEI_INVENTORY_SCREEN.ITEM_RARITY_DETAILS.Replace("{RarityName}", selectedPermit.Rarity.GetLocStringName());
		this.selectionRarityDetailsLabel.gameObject.SetActive(!string.IsNullOrWhiteSpace(text));
		this.selectionRarityDetailsLabel.SetText(text);
		this.selectionOwnedCount.gameObject.SetActive(true);
		if (!selectedPermit.IsOwnable())
		{
			this.selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE);
			return;
		}
		int ownedCount = PermitItems.GetOwnedCount(selectedPermit);
		if (ownedCount > 0)
		{
			this.selectionOwnedCount.SetText(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWNED_AMOUNT.Replace("{OwnedCount}", ownedCount.ToString()));
			return;
		}
		this.selectionOwnedCount.SetText(KleiItemsUI.WrapWithColor(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));
	}

	// Token: 0x06005881 RID: 22657 RVA: 0x00206F44 File Offset: 0x00205144
	private void RefreshBarterPanel()
	{
		this.barterBuyButton.ClearOnClick();
		this.barterSellButton.ClearOnClick();
		this.barterBuyButton.isInteractable = this.IS_ONLINE;
		this.barterSellButton.isInteractable = this.IS_ONLINE;
		HierarchyReferences component = this.barterBuyButton.GetComponent<HierarchyReferences>();
		HierarchyReferences component2 = this.barterSellButton.GetComponent<HierarchyReferences>();
		this.barterPanelBG.color = (this.IS_ONLINE ? Util.ColorFromHex("575D6F") : Util.ColorFromHex("6F6F6F"));
		this.filamentWalletSection.gameObject.SetActive(this.IS_ONLINE);
		this.barterOfflineLabel.gameObject.SetActive(!this.IS_ONLINE);
		ulong filamentAmount = KleiItems.GetFilamentAmount();
		this.filamentWalletSection.GetComponent<ToolTip>().SetSimpleTooltip((filamentAmount > 1UL) ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_PLURAL_TOOLTIP, filamentAmount) : string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.WALLET_TOOLTIP, filamentAmount));
		if (!this.IS_ONLINE)
		{
			component.GetReference<LocText>("CostLabel").SetText("");
			component2.GetReference<LocText>("CostLabel").SetText("");
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
			this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_ACTION_INVALID_OFFLINE);
			return;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(this.SelectedPermit.Id, out num, out num2);
		this.filamentWalletSection.GetComponentInChildren<LocText>().SetText(KleiItems.GetFilamentAmount().ToString());
		if (num == 0UL)
		{
			this.barterBuyButton.isInteractable = false;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNBUYABLE);
			component.GetReference<LocText>("CostLabel").SetText("");
		}
		else
		{
			bool flag = KleiItems.GetFilamentAmount() >= num;
			this.barterBuyButton.isInteractable = flag;
			this.barterBuyButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_ACTIVE, num.ToString()) : UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_BUY_CANT_AFFORD.text);
			component.GetReference<LocText>("CostLabel").SetText("-" + num.ToString());
			this.barterBuyButton.onClick += delegate()
			{
				GameObject gameObject = Util.KInstantiateUI(this.barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject, false);
				gameObject.rectTransform().sizeDelta = Vector2.zero;
				gameObject.GetComponent<BarterConfirmationScreen>().Present(this.SelectedPermit, true);
			};
		}
		if (num2 == 0UL)
		{
			this.barterSellButton.isInteractable = false;
			this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_UNSELLABLE);
			component2.GetReference<LocText>("CostLabel").SetText("");
			return;
		}
		bool flag2 = PermitItems.GetOwnedCount(this.SelectedPermit) > 0;
		this.barterSellButton.isInteractable = flag2;
		this.barterSellButton.GetComponent<ToolTip>().SetSimpleTooltip(flag2 ? string.Format(UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_SELL_ACTIVE, num2.ToString()) : UI.KLEI_INVENTORY_SCREEN.BARTERING.TOOLTIP_NONE_TO_SELL.text);
		component2.GetReference<LocText>("CostLabel").SetText(flag2 ? (UIConstants.ColorPrefixGreen + "+" + num2.ToString() + UIConstants.ColorSuffix) : ("+" + num2.ToString()));
		this.barterSellButton.onClick += delegate()
		{
			GameObject gameObject = Util.KInstantiateUI(this.barterConfirmationScreenPrefab, LockerNavigator.Instance.gameObject, false);
			gameObject.rectTransform().sizeDelta = Vector2.zero;
			gameObject.GetComponent<BarterConfirmationScreen>().Present(this.SelectedPermit, false);
		};
	}

	// Token: 0x06005882 RID: 22658 RVA: 0x00207294 File Offset: 0x00205494
	private void SetCatogoryClickUISound(string categoryID, MultiToggle toggle)
	{
		if (!this.categoryToggles.ContainsKey(categoryID))
		{
			toggle.states[1].on_click_override_sound_path = "";
			toggle.states[0].on_click_override_sound_path = "";
			return;
		}
		toggle.states[1].on_click_override_sound_path = "General_Category_Click";
		toggle.states[0].on_click_override_sound_path = "General_Category_Click";
	}

	// Token: 0x06005883 RID: 22659 RVA: 0x00207308 File Offset: 0x00205508
	private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
	{
		string facadeItemSoundName = KleiInventoryScreen.GetFacadeItemSoundName(permit);
		toggle.states[1].on_click_override_sound_path = facadeItemSoundName + "_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = facadeItemSoundName + "_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x06005884 RID: 22660 RVA: 0x002073F0 File Offset: 0x002055F0
	public static string GetFacadeItemSoundName(PermitResource permit)
	{
		if (permit == null)
		{
			return "HUD";
		}
		switch (permit.Category)
		{
		case PermitCategory.DupeTops:
			return "tops";
		case PermitCategory.DupeBottoms:
			return "bottoms";
		case PermitCategory.DupeGloves:
			return "gloves";
		case PermitCategory.DupeShoes:
			return "shoes";
		case PermitCategory.DupeHats:
			return "hats";
		case PermitCategory.AtmoSuitHelmet:
			return "atmosuit_helmet";
		case PermitCategory.AtmoSuitBody:
			return "tops";
		case PermitCategory.AtmoSuitGloves:
			return "gloves";
		case PermitCategory.AtmoSuitBelt:
			return "belt";
		case PermitCategory.AtmoSuitShoes:
			return "shoes";
		}
		if (permit.Category == PermitCategory.Building)
		{
			bool flag;
			BuildingDef buildingDef;
			KleiPermitVisUtil.GetBuildingDef(permit).Deconstruct(out flag, out buildingDef);
			bool flag2 = flag;
			BuildingDef buildingDef2 = buildingDef;
			if (!flag2)
			{
				return "HUD";
			}
			string prefabID = buildingDef2.PrefabID;
			if (prefabID != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(prefabID);
				if (num <= 2076384603U)
				{
					if (num <= 1633134164U)
					{
						if (num != 228062815U)
						{
							if (num != 595816591U)
							{
								if (num != 1633134164U)
								{
									goto IL_38E;
								}
								if (!(prefabID == "CeilingLight"))
								{
									goto IL_38E;
								}
								return "ceilingLight";
							}
							else if (!(prefabID == "FlowerVase"))
							{
								goto IL_38E;
							}
						}
						else
						{
							if (!(prefabID == "LuxuryBed"))
							{
								goto IL_38E;
							}
							string id = permit.Id;
							if (id != null)
							{
								if (id == "LuxuryBed_boat")
								{
									return "elegantbed_boat";
								}
								if (id == "LuxuryBed_bouncy")
								{
									return "elegantbed_bouncy";
								}
							}
							return "elegantbed";
						}
					}
					else if (num <= 1943253450U)
					{
						if (num != 1734850496U)
						{
							if (num != 1943253450U)
							{
								goto IL_38E;
							}
							if (!(prefabID == "WaterCooler"))
							{
								goto IL_38E;
							}
							return "watercooler";
						}
						else
						{
							if (!(prefabID == "RockCrusher"))
							{
								goto IL_38E;
							}
							return "rockrefinery";
						}
					}
					else if (num != 2028863301U)
					{
						if (num != 2076384603U)
						{
							goto IL_38E;
						}
						if (!(prefabID == "GasReservoir"))
						{
							goto IL_38E;
						}
						return "gasstorage";
					}
					else if (!(prefabID == "FlowerVaseHanging"))
					{
						goto IL_38E;
					}
				}
				else if (num <= 3048425356U)
				{
					if (num <= 2722382738U)
					{
						if (num != 2402859370U)
						{
							if (num != 2722382738U)
							{
								goto IL_38E;
							}
							if (!(prefabID == "PlanterBox"))
							{
								goto IL_38E;
							}
							return "planterbox";
						}
						else
						{
							if (!(prefabID == "StorageLocker"))
							{
								goto IL_38E;
							}
							return "storagelocker";
						}
					}
					else if (num != 2899744071U)
					{
						if (num != 3048425356U)
						{
							goto IL_38E;
						}
						if (!(prefabID == "Bed"))
						{
							goto IL_38E;
						}
						return "bed";
					}
					else
					{
						if (!(prefabID == "ExteriorWall"))
						{
							goto IL_38E;
						}
						return "wall";
					}
				}
				else if (num <= 3534553076U)
				{
					if (num != 3132083755U)
					{
						if (num != 3534553076U)
						{
							goto IL_38E;
						}
						if (!(prefabID == "MassageTable"))
						{
							goto IL_38E;
						}
						return "massagetable";
					}
					else if (!(prefabID == "FlowerVaseWall"))
					{
						goto IL_38E;
					}
				}
				else if (num != 3903452895U)
				{
					if (num != 3958671086U)
					{
						goto IL_38E;
					}
					if (!(prefabID == "FlowerVaseHangingFancy"))
					{
						goto IL_38E;
					}
				}
				else
				{
					if (!(prefabID == "EggCracker"))
					{
						goto IL_38E;
					}
					return "eggcracker";
				}
				return "flowervase";
			}
		}
		IL_38E:
		if (permit.Category == PermitCategory.Artwork)
		{
			bool flag;
			BuildingDef buildingDef;
			KleiPermitVisUtil.GetBuildingDef(permit).Deconstruct(out flag, out buildingDef);
			bool flag3 = flag;
			BuildingDef buildingDef3 = buildingDef;
			if (!flag3)
			{
				return "HUD";
			}
			ArtableStage artableStage = (ArtableStage)permit;
			if (KleiInventoryScreen.<GetFacadeItemSoundName>g__Has|70_0<Sculpture>(buildingDef3))
			{
				if (buildingDef3.PrefabID == "IceSculpture")
				{
					return "icesculpture";
				}
				return "sculpture";
			}
			else if (KleiInventoryScreen.<GetFacadeItemSoundName>g__Has|70_0<Painting>(buildingDef3))
			{
				return "painting";
			}
		}
		if (permit.Category == PermitCategory.JoyResponse && permit is BalloonArtistFacadeResource)
		{
			return "balloon";
		}
		return "HUD";
	}

	// Token: 0x06005885 RID: 22661 RVA: 0x0020780F File Offset: 0x00205A0F
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x0600588D RID: 22669 RVA: 0x00207901 File Offset: 0x00205B01
	[CompilerGenerated]
	internal static bool <GetFacadeItemSoundName>g__Has|70_0<T>(BuildingDef buildingDef) where T : Component
	{
		return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
	}

	// Token: 0x04003BC2 RID: 15298
	[Header("Header")]
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003BC3 RID: 15299
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x04003BC4 RID: 15300
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x04003BC5 RID: 15301
	private Dictionary<string, MultiToggle> categoryToggles = new Dictionary<string, MultiToggle>();

	// Token: 0x04003BC6 RID: 15302
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x04003BC7 RID: 15303
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04003BC8 RID: 15304
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x04003BC9 RID: 15305
	[SerializeField]
	private GameObject subcategoryPrefab;

	// Token: 0x04003BCA RID: 15306
	[SerializeField]
	private GameObject itemDummyPrefab;

	// Token: 0x04003BCB RID: 15307
	[Header("GalleryFilters")]
	[SerializeField]
	private KInputTextField searchField;

	// Token: 0x04003BCC RID: 15308
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x04003BCD RID: 15309
	[SerializeField]
	private MultiToggle doublesOnlyToggle;

	// Token: 0x04003BCE RID: 15310
	private int showFilterState;

	// Token: 0x04003BCF RID: 15311
	[Header("BarterSection")]
	[SerializeField]
	private Image barterPanelBG;

	// Token: 0x04003BD0 RID: 15312
	[SerializeField]
	private KButton barterBuyButton;

	// Token: 0x04003BD1 RID: 15313
	[SerializeField]
	private KButton barterSellButton;

	// Token: 0x04003BD2 RID: 15314
	[SerializeField]
	private GameObject barterConfirmationScreenPrefab;

	// Token: 0x04003BD3 RID: 15315
	[SerializeField]
	private GameObject filamentWalletSection;

	// Token: 0x04003BD4 RID: 15316
	[SerializeField]
	private GameObject barterOfflineLabel;

	// Token: 0x04003BD5 RID: 15317
	private Dictionary<PermitResource, MultiToggle> galleryGridButtons = new Dictionary<PermitResource, MultiToggle>();

	// Token: 0x04003BD6 RID: 15318
	private List<KleiInventoryUISubcategory> subcategories = new List<KleiInventoryUISubcategory>();

	// Token: 0x04003BD7 RID: 15319
	private List<GameObject> recycledGalleryGridButtons = new List<GameObject>();

	// Token: 0x04003BD8 RID: 15320
	private GridLayouter galleryGridLayouter;

	// Token: 0x04003BD9 RID: 15321
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x04003BDA RID: 15322
	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	// Token: 0x04003BDB RID: 15323
	[SerializeField]
	private KScrollRect selectionDetailsScrollRect;

	// Token: 0x04003BDC RID: 15324
	[SerializeField]
	private RectTransform selectionDetailsScrollRectScrollBarContainer;

	// Token: 0x04003BDD RID: 15325
	[SerializeField]
	private LocText selectionNameLabel;

	// Token: 0x04003BDE RID: 15326
	[SerializeField]
	private LocText selectionDescriptionLabel;

	// Token: 0x04003BDF RID: 15327
	[SerializeField]
	private LocText selectionFacadeForLabel;

	// Token: 0x04003BE0 RID: 15328
	[SerializeField]
	private LocText selectionRarityDetailsLabel;

	// Token: 0x04003BE1 RID: 15329
	[SerializeField]
	private LocText selectionOwnedCount;

	// Token: 0x04003BE3 RID: 15331
	private bool IS_ONLINE;

	// Token: 0x04003BE4 RID: 15332
	private bool initConfigComplete;

	// Token: 0x02001A41 RID: 6721
	private enum MultiToggleState
	{
		// Token: 0x04007907 RID: 30983
		Default,
		// Token: 0x04007908 RID: 30984
		Selected,
		// Token: 0x04007909 RID: 30985
		NonInteractable
	}
}
