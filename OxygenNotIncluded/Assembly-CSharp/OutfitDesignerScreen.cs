using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000BB6 RID: 2998
public class OutfitDesignerScreen : KMonoBehaviour
{
	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x06005D99 RID: 23961 RVA: 0x00223E65 File Offset: 0x00222065
	// (set) Token: 0x06005D9A RID: 23962 RVA: 0x00223E6D File Offset: 0x0022206D
	public OutfitDesignerScreenConfig Config { get; private set; }

	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x06005D9B RID: 23963 RVA: 0x00223E76 File Offset: 0x00222076
	// (set) Token: 0x06005D9C RID: 23964 RVA: 0x00223E7E File Offset: 0x0022207E
	public PermitResource SelectedPermit { get; private set; }

	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x06005D9D RID: 23965 RVA: 0x00223E87 File Offset: 0x00222087
	// (set) Token: 0x06005D9E RID: 23966 RVA: 0x00223E8F File Offset: 0x0022208F
	public PermitCategory SelectedCategory { get; private set; }

	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x06005D9F RID: 23967 RVA: 0x00223E98 File Offset: 0x00222098
	// (set) Token: 0x06005DA0 RID: 23968 RVA: 0x00223EA0 File Offset: 0x002220A0
	public OutfitDesignerScreen_OutfitState outfitState { get; private set; }

	// Token: 0x06005DA1 RID: 23969 RVA: 0x00223EAC File Offset: 0x002220AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(this.categoryRowPrefab.transform.parent == this.categoryListContent.transform);
		global::Debug.Assert(this.gridItemPrefab.transform.parent == this.galleryGridContent.transform);
		global::Debug.Assert(this.subcategoryUiPrefab.transform.parent == this.galleryGridContent.transform);
		this.categoryRowPrefab.SetActive(false);
		this.gridItemPrefab.SetActive(false);
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.galleryGridLayouter.overrideParentForSizeReference = this.galleryGridContent;
		this.categoryRowPool = new UIPrefabLocalPool(this.categoryRowPrefab, this.categoryListContent.gameObject);
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
		this.subcategoryUiPool = new UIPrefabLocalPool(this.subcategoryUiPrefab, this.galleryGridContent.gameObject);
		if (OutfitDesignerScreen.outfitTypeToCategoriesDict == null)
		{
			Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]> dictionary = new Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]>();
			dictionary[ClothingOutfitUtility.OutfitType.Clothing] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_CLOTHING;
			dictionary[ClothingOutfitUtility.OutfitType.AtmoSuit] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_ATMO_SUITS;
			OutfitDesignerScreen.outfitTypeToCategoriesDict = dictionary;
		}
		InventoryOrganization.Initialize();
	}

	// Token: 0x06005DA2 RID: 23970 RVA: 0x00224010 File Offset: 0x00222210
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x06005DA3 RID: 23971 RVA: 0x0022401D File Offset: 0x0022221D
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		this.minionOrMannequin.TrySpawn();
		if (!this.Config.isValid)
		{
			throw new NotSupportedException("Cannot open OutfitDesignerScreen without a config. Make sure to call Configure() before enabling the screen");
		}
		this.Configure(this.Config);
	}

	// Token: 0x06005DA4 RID: 23972 RVA: 0x00224056 File Offset: 0x00222256
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshCategories();
			this.RefreshGallery();
			this.RefreshOutfitState();
		});
	}

	// Token: 0x06005DA5 RID: 23973 RVA: 0x00224075 File Offset: 0x00222275
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.UnregisterPreventScreenPop();
	}

	// Token: 0x06005DA6 RID: 23974 RVA: 0x00224083 File Offset: 0x00222283
	private void UpdateSaveButtons()
	{
		if (this.updateSaveButtonsFn != null)
		{
			this.updateSaveButtonsFn();
		}
	}

	// Token: 0x06005DA7 RID: 23975 RVA: 0x00224098 File Offset: 0x00222298
	public void Configure(OutfitDesignerScreenConfig config)
	{
		this.Config = config;
		if (config.targetMinionInstance.HasValue)
		{
			this.outfitState = OutfitDesignerScreen_OutfitState.ForMinionInstance(this.Config.sourceTarget, config.targetMinionInstance.Value);
		}
		else
		{
			this.outfitState = OutfitDesignerScreen_OutfitState.ForTemplateOutfit(this.Config.sourceTarget);
		}
		if (this.postponeConfiguration)
		{
			return;
		}
		this.RegisterPreventScreenPop();
		this.minionOrMannequin.SetFrom(config.minionPersonality).SpawnedAvatar.GetComponent<WearableAccessorizer>();
		using (ListPool<ClothingItemResource, OutfitDesignerScreen>.PooledList pooledList = PoolsFor<OutfitDesignerScreen>.AllocateList<ClothingItemResource>())
		{
			this.outfitState.AddItemValuesTo(pooledList);
			this.minionOrMannequin.SetFrom(config.minionPersonality).SetOutfit(config.sourceTarget.OutfitType, pooledList);
		}
		this.PopulateCategories();
		this.SelectCategory(OutfitDesignerScreen.outfitTypeToCategoriesDict[this.outfitState.outfitType][0]);
		this.galleryGridLayouter.RequestGridResize();
		this.RefreshOutfitState();
		OutfitDesignerScreenConfig config2 = this.Config;
		if (config2.targetMinionInstance.HasValue)
		{
			this.updateSaveButtonsFn = null;
			this.primaryButton.ClearOnClick();
			TMP_Text componentInChildren = this.primaryButton.GetComponentInChildren<LocText>();
			LocString button_APPLY_TO_MINION = UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.BUTTON_APPLY_TO_MINION;
			string search = "{MinionName}";
			config2 = this.Config;
			componentInChildren.SetText(button_APPLY_TO_MINION.Replace(search, config2.targetMinionInstance.Value.GetProperName()));
			this.primaryButton.onClick += delegate()
			{
				OutfitDesignerScreenConfig config3 = this.Config;
				ClothingOutfitUtility.OutfitType outfitType = config3.sourceTarget.OutfitType;
				config3 = this.Config;
				ClothingOutfitTarget obj = ClothingOutfitTarget.FromMinion(outfitType, config3.targetMinionInstance.Value);
				config3 = this.Config;
				obj.WriteItems(config3.sourceTarget.OutfitType, this.outfitState.GetItems());
				if (this.Config.onWriteToOutfitTargetFn != null)
				{
					this.Config.onWriteToOutfitTargetFn(obj);
				}
				LockerNavigator.Instance.PopScreen();
			};
			this.secondaryButton.ClearOnClick();
			this.secondaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.BUTTON_APPLY_TO_TEMPLATE);
			this.secondaryButton.onClick += delegate()
			{
				OutfitDesignerScreen.MakeApplyToTemplatePopup(this.inputFieldPrefab, this.outfitState, this.Config.targetMinionInstance.Value, this.Config.outfitTemplate, this.Config.onWriteToOutfitTargetFn);
			};
			this.updateSaveButtonsFn = (System.Action)Delegate.Combine(this.updateSaveButtonsFn, new System.Action(delegate()
			{
				if (this.outfitState.DoesContainNonOwnedItems())
				{
					this.primaryButton.isInteractable = false;
					this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
					this.secondaryButton.isInteractable = false;
					this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
					return;
				}
				this.primaryButton.isInteractable = true;
				this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
				this.secondaryButton.isInteractable = true;
				this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
			}));
		}
		else
		{
			config2 = this.Config;
			if (!config2.outfitTemplate.HasValue)
			{
				throw new NotSupportedException();
			}
			this.updateSaveButtonsFn = null;
			this.primaryButton.ClearOnClick();
			this.primaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.BUTTON_SAVE);
			this.primaryButton.onClick += delegate()
			{
				this.outfitState.destinationTarget.WriteName(this.outfitState.name);
				this.outfitState.destinationTarget.WriteItems(this.outfitState.outfitType, this.outfitState.GetItems());
				OutfitDesignerScreenConfig config3 = this.Config;
				if (config3.minionPersonality.HasValue)
				{
					ClothingOutfits clothingOutfits = Db.Get().Permits.ClothingOutfits;
					config3 = this.Config;
					clothingOutfits.SetDuplicantPersonalityOutfit(config3.minionPersonality.Value.Id, this.outfitState.destinationTarget.OutfitId, this.outfitState.destinationTarget.OutfitType);
				}
				if (this.Config.onWriteToOutfitTargetFn != null)
				{
					this.Config.onWriteToOutfitTargetFn(this.outfitState.destinationTarget);
				}
				LockerNavigator.Instance.PopScreen();
			};
			this.secondaryButton.ClearOnClick();
			this.secondaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.BUTTON_COPY);
			this.secondaryButton.onClick += delegate()
			{
				OutfitDesignerScreen.MakeCopyPopup(this, this.inputFieldPrefab, this.outfitState, this.Config.outfitTemplate.Value, this.Config.minionPersonality, this.Config.onWriteToOutfitTargetFn);
			};
			this.updateSaveButtonsFn = (System.Action)Delegate.Combine(this.updateSaveButtonsFn, new System.Action(delegate()
			{
				if (!this.outfitState.destinationTarget.CanWriteItems)
				{
					this.primaryButton.isInteractable = false;
					this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_READONLY);
					if (this.outfitState.DoesContainNonOwnedItems())
					{
						this.secondaryButton.isInteractable = false;
						this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
						return;
					}
					this.secondaryButton.isInteractable = true;
					this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					return;
				}
				else
				{
					if (this.outfitState.DoesContainNonOwnedItems())
					{
						this.primaryButton.isInteractable = false;
						this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
						this.secondaryButton.isInteractable = false;
						this.secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
						return;
					}
					this.primaryButton.isInteractable = true;
					this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					this.secondaryButton.isInteractable = true;
					this.secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					return;
				}
			}));
		}
		this.UpdateSaveButtons();
	}

	// Token: 0x06005DA8 RID: 23976 RVA: 0x00224348 File Offset: 0x00222548
	private void RefreshOutfitState()
	{
		this.selectionHeaderLabel.text = this.outfitState.name;
		this.outfitDescriptionPanel.Refresh(this.outfitState, this.Config.minionPersonality);
		this.UpdateSaveButtons();
	}

	// Token: 0x06005DA9 RID: 23977 RVA: 0x00224382 File Offset: 0x00222582
	private void RefreshCategories()
	{
		if (this.RefreshCategoriesFn != null)
		{
			this.RefreshCategoriesFn();
		}
	}

	// Token: 0x06005DAA RID: 23978 RVA: 0x00224398 File Offset: 0x00222598
	public void PopulateCategories()
	{
		this.RefreshCategoriesFn = null;
		this.categoryRowPool.ReturnAll();
		PermitCategory[] array = OutfitDesignerScreen.outfitTypeToCategoriesDict[this.outfitState.outfitType];
		for (int i = 0; i < array.Length; i++)
		{
			OutfitDesignerScreen.<>c__DisplayClass47_0 CS$<>8__locals1 = new OutfitDesignerScreen.<>c__DisplayClass47_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.permitCategory = array[i];
			GameObject gameObject = this.categoryRowPool.Borrow();
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(PermitCategories.GetUppercaseDisplayName(CS$<>8__locals1.permitCategory));
			component.GetReference<Image>("Icon").sprite = Assets.GetSprite(PermitCategories.GetIconName(CS$<>8__locals1.permitCategory));
			MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
			MultiToggle toggle2 = toggle;
			toggle2.onEnter = (System.Action)Delegate.Combine(toggle2.onEnter, new System.Action(this.OnMouseOverToggle));
			toggle.onClick = delegate()
			{
				CS$<>8__locals1.<>4__this.SelectCategory(CS$<>8__locals1.permitCategory);
			};
			this.RefreshCategoriesFn = (System.Action)Delegate.Combine(this.RefreshCategoriesFn, new System.Action(delegate()
			{
				toggle.ChangeState((CS$<>8__locals1.permitCategory == CS$<>8__locals1.<>4__this.SelectedCategory) ? 1 : 0);
			}));
			this.SetCatogoryClickUISound(CS$<>8__locals1.permitCategory, toggle);
		}
	}

	// Token: 0x06005DAB RID: 23979 RVA: 0x002244EC File Offset: 0x002226EC
	public void SelectCategory(PermitCategory permitCategory)
	{
		this.SelectedCategory = permitCategory;
		this.galleryHeaderLabel.text = PermitCategories.GetDisplayName(permitCategory);
		this.RefreshCategories();
		this.PopulateGallery();
		Option<ClothingItemResource> itemForCategory = this.outfitState.GetItemForCategory(permitCategory);
		if (itemForCategory.HasValue)
		{
			this.SelectPermit(itemForCategory.Value);
			return;
		}
		this.SelectPermit(null);
	}

	// Token: 0x06005DAC RID: 23980 RVA: 0x00224548 File Offset: 0x00222748
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x06005DAD RID: 23981 RVA: 0x00224560 File Offset: 0x00222760
	public void PopulateGallery()
	{
		OutfitDesignerScreen.<>c__DisplayClass51_0 CS$<>8__locals1 = new OutfitDesignerScreen.<>c__DisplayClass51_0();
		CS$<>8__locals1.<>4__this = this;
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		this.subcategoryUiPool.ReturnAll();
		this.galleryGridLayouter.targetGridLayouts.Clear();
		this.galleryGridLayouter.OnSizeGridComplete = null;
		CS$<>8__locals1.onFirstDisplayCategoryDecided = new Promise<KleiInventoryUISubcategory>();
		CS$<>8__locals1.<PopulateGallery>g__AddGridIconForPermit|0(null);
		foreach (ClothingItemResource clothingItemResource in Db.Get().Permits.ClothingItems.resources)
		{
			if (clothingItemResource.Category == this.SelectedCategory && clothingItemResource.outfitType == this.Config.sourceTarget.OutfitType && !clothingItemResource.Id.StartsWith("visonly_"))
			{
				CS$<>8__locals1.<PopulateGallery>g__AddGridIconForPermit|0(clothingItemResource);
			}
		}
		foreach (GameObject gameObject3 in this.subcategoryUiPool.GetBorrowedObjects().StableSort(Comparer<GameObject>.Create(delegate(GameObject a, GameObject b)
		{
			KleiInventoryUISubcategory component = a.GetComponent<KleiInventoryUISubcategory>();
			KleiInventoryUISubcategory component2 = b.GetComponent<KleiInventoryUISubcategory>();
			int sortKey = InventoryOrganization.subcategoryIdToPresentationDataMap[component.subcategoryID].sortKey;
			int sortKey2 = InventoryOrganization.subcategoryIdToPresentationDataMap[component2.subcategoryID].sortKey;
			return sortKey.CompareTo(sortKey2);
		})))
		{
			gameObject3.transform.SetAsLastSibling();
		}
		GameObject gameObject2 = this.subcategoryUiPool.GetBorrowedObjects().FirstOrDefault((GameObject gameObject) => gameObject.GetComponent<KleiInventoryUISubcategory>().IsOpen);
		if (gameObject2 != null)
		{
			CS$<>8__locals1.onFirstDisplayCategoryDecided.Resolve(gameObject2.GetComponent<KleiInventoryUISubcategory>());
		}
		this.galleryGridLayouter.RequestGridResize();
		this.RefreshGallery();
	}

	// Token: 0x06005DAE RID: 23982 RVA: 0x00224724 File Offset: 0x00222924
	public void SelectPermit(PermitResource permit)
	{
		this.SelectedPermit = permit;
		this.RefreshGallery();
		this.UpdateSelectedItemDetails();
		this.UpdateSaveButtons();
	}

	// Token: 0x06005DAF RID: 23983 RVA: 0x00224740 File Offset: 0x00222940
	public void UpdateSelectedItemDetails()
	{
		Option<ClothingItemResource> item = Option.None;
		if (this.SelectedPermit != null)
		{
			ClothingItemResource clothingItemResource = this.SelectedPermit as ClothingItemResource;
			if (clothingItemResource != null)
			{
				item = clothingItemResource;
			}
		}
		this.outfitState.SetItemForCategory(this.SelectedCategory, item);
		this.minionOrMannequin.current.SetOutfit(this.outfitState);
		this.minionOrMannequin.current.ReactToClothingItemChange(this.SelectedCategory);
		this.outfitDescriptionPanel.Refresh(this.outfitState, this.Config.minionPersonality);
		this.dioramaBG.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.SelectedCategory);
	}

	// Token: 0x06005DB0 RID: 23984 RVA: 0x002247E6 File Offset: 0x002229E6
	private void RegisterPreventScreenPop()
	{
		this.UnregisterPreventScreenPop();
		this.preventScreenPopFn = delegate()
		{
			if (this.outfitState.IsDirty())
			{
				this.RegisterPreventScreenPop();
				OutfitDesignerScreen.MakeSaveWarningPopup(this.outfitState, delegate
				{
					this.UnregisterPreventScreenPop();
					LockerNavigator.Instance.PopScreen();
				});
				return true;
			}
			return false;
		};
		LockerNavigator.Instance.preventScreenPop.Add(this.preventScreenPopFn);
	}

	// Token: 0x06005DB1 RID: 23985 RVA: 0x00224815 File Offset: 0x00222A15
	private void UnregisterPreventScreenPop()
	{
		if (this.preventScreenPopFn != null)
		{
			LockerNavigator.Instance.preventScreenPop.Remove(this.preventScreenPopFn);
			this.preventScreenPopFn = null;
		}
	}

	// Token: 0x06005DB2 RID: 23986 RVA: 0x0022483C File Offset: 0x00222A3C
	public static void MakeSaveWarningPopup(OutfitDesignerScreen_OutfitState outfitState, System.Action discardChangesFn)
	{
		Action<InfoDialogScreen> <>9__1;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.HEADER.Replace("{OutfitName}", outfitState.name)).AddPlainText(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BODY);
			string text = UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_DISCARD;
			Action<InfoDialogScreen> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(InfoDialogScreen d)
				{
					d.Deactivate();
					discardChangesFn();
				});
			}
			infoDialogScreen.AddOption(text, action, true).AddOption(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_RETURN, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
		});
	}

	// Token: 0x06005DB3 RID: 23987 RVA: 0x00224874 File Offset: 0x00222A74
	public static void MakeApplyToTemplatePopup(KInputTextField inputFieldPrefab, OutfitDesignerScreen_OutfitState outfitState, GameObject targetMinionInstance, Option<ClothingOutfitTarget> existingOutfitTemplate, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		ClothingOutfitNameProposal proposal = default(ClothingOutfitNameProposal);
		Color errorTextColor = Util.ColorFromHex("F44A47");
		Color defaultTextColor = Util.ColorFromHex("FFFFFF");
		KInputTextField inputField;
		InfoScreenPlainText descLabel;
		KButton saveButton;
		LocText saveButtonText;
		LocText descLocText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.HEADER.Replace("{OutfitName}", outfitState.name)).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out descLabel).AddOption(true, out saveButton, out saveButtonText).AddDefaultCancel();
			descLocText = descLabel.gameObject.GetComponent<LocText>();
			descLocText.allowOverride = true;
			descLocText.alignment = TextAlignmentOptions.BottomLeft;
			descLocText.color = errorTextColor;
			descLocText.fontSize = 14f;
			descLabel.SetText("");
			inputField.onValueChanged.AddListener(new UnityAction<string>(base.<MakeApplyToTemplatePopup>g__Refresh|1));
			saveButton.onClick += delegate()
			{
				ClothingOutfitTarget clothingOutfitTarget = ClothingOutfitTarget.FromMinion(outfitState.outfitType, targetMinionInstance);
				ClothingOutfitNameProposal.Result result = proposal.result;
				ClothingOutfitTarget obj;
				if (result != ClothingOutfitNameProposal.Result.NewOutfit)
				{
					if (result != ClothingOutfitNameProposal.Result.SameOutfit)
					{
						throw new NotSupportedException(string.Format("Can't save outfit with name \"{0}\", failed with result: {1}", proposal.candidateName, proposal.result));
					}
					obj = existingOutfitTemplate.Value;
				}
				else
				{
					obj = ClothingOutfitTarget.ForNewTemplateOutfit(outfitState.outfitType, proposal.candidateName);
				}
				obj.WriteItems(outfitState.outfitType, outfitState.GetItems());
				clothingOutfitTarget.WriteItems(outfitState.outfitType, outfitState.GetItems());
				if (onWriteToOutfitTargetFn != null)
				{
					onWriteToOutfitTargetFn(obj);
				}
				dialog.Deactivate();
				LockerNavigator.Instance.PopScreen();
			};
			if (!existingOutfitTemplate.HasValue)
			{
				base.<MakeApplyToTemplatePopup>g__Refresh|1(outfitState.name);
				return;
			}
			if (existingOutfitTemplate.Value.CanWriteName && existingOutfitTemplate.Value.CanWriteItems)
			{
				base.<MakeApplyToTemplatePopup>g__Refresh|1(existingOutfitTemplate.Value.OutfitId);
				return;
			}
			base.<MakeApplyToTemplatePopup>g__Refresh|1(ClothingOutfitTarget.ForTemplateCopyOf(existingOutfitTemplate.Value).OutfitId);
		});
	}

	// Token: 0x06005DB4 RID: 23988 RVA: 0x002248F0 File Offset: 0x00222AF0
	public static void MakeCopyPopup(OutfitDesignerScreen screen, KInputTextField inputFieldPrefab, OutfitDesignerScreen_OutfitState outfitState, ClothingOutfitTarget outfitTemplate, Option<Personality> minionPersonality, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		ClothingOutfitNameProposal proposal = default(ClothingOutfitNameProposal);
		KInputTextField inputField;
		InfoScreenPlainText errorText;
		KButton okButton;
		LocText okButtonText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.COPY_POPUP.HEADER).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out errorText).AddOption(true, out okButton, out okButtonText).AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
			inputField.onValueChanged.AddListener(new UnityAction<string>(base.<MakeCopyPopup>g__Refresh|1));
			errorText.gameObject.SetActive(false);
			LocText component = errorText.gameObject.GetComponent<LocText>();
			component.allowOverride = true;
			component.alignment = TextAlignmentOptions.BottomLeft;
			component.color = Util.ColorFromHex("F44A47");
			component.fontSize = 14f;
			errorText.SetText("");
			okButtonText.text = UI.CONFIRMDIALOG.OK;
			okButton.onClick += delegate()
			{
				if (proposal.result == ClothingOutfitNameProposal.Result.NewOutfit)
				{
					ClothingOutfitTarget clothingOutfitTarget = ClothingOutfitTarget.ForNewTemplateOutfit(outfitTemplate.OutfitType, proposal.candidateName);
					clothingOutfitTarget.WriteItems(outfitState.outfitType, outfitState.GetItems());
					if (minionPersonality.HasValue)
					{
						Db.Get().Permits.ClothingOutfits.SetDuplicantPersonalityOutfit(minionPersonality.Value.Id, clothingOutfitTarget.OutfitId, clothingOutfitTarget.OutfitType);
					}
					if (onWriteToOutfitTargetFn != null)
					{
						onWriteToOutfitTargetFn(clothingOutfitTarget);
					}
					dialog.Deactivate();
					screen.Configure(screen.Config.WithOutfit(clothingOutfitTarget));
					return;
				}
				throw new NotSupportedException(string.Format("Can't save outfit with name \"{0}\", failed with result: {1}", proposal.candidateName, proposal.result));
			};
			base.<MakeCopyPopup>g__Refresh|1(ClothingOutfitTarget.ForTemplateCopyOf(outfitTemplate).OutfitId);
		});
	}

	// Token: 0x06005DB5 RID: 23989 RVA: 0x00224954 File Offset: 0x00222B54
	private void SetCatogoryClickUISound(PermitCategory category, MultiToggle toggle)
	{
		toggle.states[1].on_click_override_sound_path = category.ToString() + "_Click";
		toggle.states[0].on_click_override_sound_path = category.ToString() + "_Click";
	}

	// Token: 0x06005DB6 RID: 23990 RVA: 0x002249B4 File Offset: 0x00222BB4
	private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
	{
		if (permit == null)
		{
			toggle.states[1].on_click_override_sound_path = "HUD_Click";
			toggle.states[0].on_click_override_sound_path = "HUD_Click";
			return;
		}
		string clothingItemSoundName = OutfitDesignerScreen.GetClothingItemSoundName(permit);
		toggle.states[1].on_click_override_sound_path = clothingItemSoundName + "_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = clothingItemSoundName + "_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x06005DB7 RID: 23991 RVA: 0x00224ACC File Offset: 0x00222CCC
	public static string GetClothingItemSoundName(PermitResource permit)
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
		default:
			return "HUD";
		}
	}

	// Token: 0x06005DB8 RID: 23992 RVA: 0x00224B2D File Offset: 0x00222D2D
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x04003F09 RID: 16137
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x04003F0A RID: 16138
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x04003F0B RID: 16139
	private UIPrefabLocalPool categoryRowPool;

	// Token: 0x04003F0C RID: 16140
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x04003F0D RID: 16141
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04003F0E RID: 16142
	[SerializeField]
	private GameObject subcategoryUiPrefab;

	// Token: 0x04003F0F RID: 16143
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x04003F10 RID: 16144
	private UIPrefabLocalPool subcategoryUiPool;

	// Token: 0x04003F11 RID: 16145
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x04003F12 RID: 16146
	private GridLayouter galleryGridLayouter;

	// Token: 0x04003F13 RID: 16147
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x04003F14 RID: 16148
	[SerializeField]
	private UIMinionOrMannequin minionOrMannequin;

	// Token: 0x04003F15 RID: 16149
	[SerializeField]
	private Image dioramaBG;

	// Token: 0x04003F16 RID: 16150
	[SerializeField]
	private KButton primaryButton;

	// Token: 0x04003F17 RID: 16151
	[SerializeField]
	private KButton secondaryButton;

	// Token: 0x04003F18 RID: 16152
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x04003F19 RID: 16153
	[SerializeField]
	private KInputTextField inputFieldPrefab;

	// Token: 0x04003F1E RID: 16158
	public static Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]> outfitTypeToCategoriesDict;

	// Token: 0x04003F1F RID: 16159
	private bool postponeConfiguration = true;

	// Token: 0x04003F20 RID: 16160
	private System.Action updateSaveButtonsFn;

	// Token: 0x04003F21 RID: 16161
	private System.Action RefreshCategoriesFn;

	// Token: 0x04003F22 RID: 16162
	private System.Action RefreshGalleryFn;

	// Token: 0x04003F23 RID: 16163
	private Func<bool> preventScreenPopFn;

	// Token: 0x02001AEA RID: 6890
	private enum MultiToggleState
	{
		// Token: 0x04007AFC RID: 31484
		Default,
		// Token: 0x04007AFD RID: 31485
		Selected,
		// Token: 0x04007AFE RID: 31486
		NonInteractable
	}
}
