using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000BB2 RID: 2994
public class OutfitBrowserScreen : KMonoBehaviour
{
	// Token: 0x06005D6C RID: 23916 RVA: 0x002227A0 File Offset: 0x002209A0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
		this.gridLayouter = new GridLayouter
		{
			minCellSize = 112f,
			maxCellSize = 144f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.categoriesAndSearchBar.InitializeWith(this);
		this.pickOutfitButton.onClick += this.OnClickPickOutfit;
		this.editOutfitButton.onClick += delegate()
		{
			if (this.state.SelectedOutfitOpt.IsNone())
			{
				return;
			}
			new OutfitDesignerScreenConfig(this.state.SelectedOutfitOpt.Unwrap(), this.Config.minionPersonality, this.Config.targetMinionInstance, new Action<ClothingOutfitTarget>(this.OnOutfitDesignerWritesToOutfitTarget)).ApplyAndOpenScreen();
		};
		this.renameOutfitButton.onClick += delegate()
		{
			ClothingOutfitTarget selectedOutfit = this.state.SelectedOutfitOpt.Unwrap();
			OutfitBrowserScreen.MakeRenamePopup(this.inputFieldPrefab, selectedOutfit, () => selectedOutfit.ReadName(), delegate(string new_name)
			{
				selectedOutfit.WriteName(new_name);
				this.Configure(this.Config.WithOutfit(selectedOutfit));
			});
		};
		this.deleteOutfitButton.onClick += delegate()
		{
			ClothingOutfitTarget selectedOutfit = this.state.SelectedOutfitOpt.Unwrap();
			OutfitBrowserScreen.MakeDeletePopup(selectedOutfit, delegate
			{
				selectedOutfit.Delete();
				this.Configure(this.Config.WithOutfit(Option.None));
			});
		};
	}

	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x06005D6D RID: 23917 RVA: 0x0022286E File Offset: 0x00220A6E
	// (set) Token: 0x06005D6E RID: 23918 RVA: 0x00222876 File Offset: 0x00220A76
	public OutfitBrowserScreenConfig Config { get; private set; }

	// Token: 0x06005D6F RID: 23919 RVA: 0x00222880 File Offset: 0x00220A80
	protected override void OnCmpEnable()
	{
		if (this.isFirstDisplay)
		{
			this.isFirstDisplay = false;
			this.dioramaMinionOrMannequin.TrySpawn();
			this.FirstTimeSetup();
			this.postponeConfiguration = false;
			this.Configure(this.Config);
		}
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshGallery();
			this.outfitDescriptionPanel.Refresh(this.state.SelectedOutfitOpt, ClothingOutfitUtility.OutfitType.Clothing, this.Config.minionPersonality);
		});
	}

	// Token: 0x06005D70 RID: 23920 RVA: 0x002228D8 File Offset: 0x00220AD8
	private void FirstTimeSetup()
	{
		this.state.OnCurrentOutfitTypeChanged += delegate()
		{
			this.PopulateGallery();
			OutfitBrowserScreenConfig config = this.Config;
			Option<ClothingOutfitTarget> selectedOutfitOpt;
			if (!config.minionPersonality.HasValue)
			{
				config = this.Config;
				if (!config.selectedTarget.HasValue)
				{
					selectedOutfitOpt = ClothingOutfitTarget.GetRandom(this.state.CurrentOutfitType);
					goto IL_4F;
				}
			}
			selectedOutfitOpt = this.Config.selectedTarget;
			IL_4F:
			if (selectedOutfitOpt.IsSome() && selectedOutfitOpt.Unwrap().DoesExist())
			{
				this.state.SelectedOutfitOpt = selectedOutfitOpt;
				return;
			}
			this.state.SelectedOutfitOpt = Option.None;
		};
		this.state.OnSelectedOutfitOptChanged += delegate()
		{
			if (this.state.SelectedOutfitOpt.IsSome())
			{
				this.selectionHeaderLabel.text = this.state.SelectedOutfitOpt.Unwrap().ReadName();
			}
			else
			{
				this.selectionHeaderLabel.text = UI.OUTFIT_NAME.NONE;
			}
			this.dioramaMinionOrMannequin.current.SetOutfit(this.state.CurrentOutfitType, this.state.SelectedOutfitOpt);
			this.dioramaMinionOrMannequin.current.ReactToFullOutfitChange();
			this.outfitDescriptionPanel.Refresh(this.state.SelectedOutfitOpt, this.state.CurrentOutfitType, this.Config.minionPersonality);
			this.dioramaBG.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.state.CurrentOutfitType);
			this.pickOutfitButton.gameObject.SetActive(this.Config.isPickingOutfitForDupe);
			OutfitBrowserScreenConfig config = this.Config;
			if (config.minionPersonality.IsSome())
			{
				this.pickOutfitButton.isInteractable = (!this.state.SelectedOutfitOpt.IsSome() || !this.state.SelectedOutfitOpt.Unwrap().DoesContainNonOwnedItems());
				GameObject gameObject = this.pickOutfitButton.gameObject;
				Option<string> tooltipText;
				if (!this.pickOutfitButton.isInteractable)
				{
					LocString tooltip_PICK_OUTFIT_ERROR_LOCKED = UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_PICK_OUTFIT_ERROR_LOCKED;
					string search = "{MinionName}";
					config = this.Config;
					tooltipText = Option.Some<string>(tooltip_PICK_OUTFIT_ERROR_LOCKED.Replace(search, config.GetMinionName()));
				}
				else
				{
					tooltipText = Option.None;
				}
				KleiItemsUI.ConfigureTooltipOn(gameObject, tooltipText);
			}
			this.editOutfitButton.isInteractable = this.state.SelectedOutfitOpt.IsSome();
			this.renameOutfitButton.isInteractable = (this.state.SelectedOutfitOpt.IsSome() && this.state.SelectedOutfitOpt.Unwrap().CanWriteName);
			KleiItemsUI.ConfigureTooltipOn(this.renameOutfitButton.gameObject, this.renameOutfitButton.isInteractable ? UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_RENAME_OUTFIT : UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_RENAME_OUTFIT_ERROR_READONLY);
			this.deleteOutfitButton.isInteractable = (this.state.SelectedOutfitOpt.IsSome() && this.state.SelectedOutfitOpt.Unwrap().CanDelete);
			KleiItemsUI.ConfigureTooltipOn(this.deleteOutfitButton.gameObject, this.deleteOutfitButton.isInteractable ? UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_DELETE_OUTFIT : UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_DELETE_OUTFIT_ERROR_READONLY);
			this.state.OnSelectedOutfitOptChanged += this.RefreshGallery;
			this.state.OnFilterChanged += this.RefreshGallery;
			this.state.OnCurrentOutfitTypeChanged += this.RefreshGallery;
			this.RefreshGallery();
		};
	}

	// Token: 0x06005D71 RID: 23921 RVA: 0x00222908 File Offset: 0x00220B08
	public void Configure(OutfitBrowserScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.dioramaMinionOrMannequin.SetFrom(config.minionPersonality);
		if (config.targetMinionInstance.HasValue)
		{
			this.galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.MINION_GALLERY_HEADER.Replace("{MinionName}", config.targetMinionInstance.Value.GetProperName());
		}
		else if (config.minionPersonality.HasValue)
		{
			this.galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.MINION_GALLERY_HEADER.Replace("{MinionName}", config.minionPersonality.Value.Name);
		}
		else
		{
			this.galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.GALLERY_HEADER;
		}
		this.state.CurrentOutfitType = config.onlyShowOutfitType.UnwrapOr(this.lastShownOutfitType.UnwrapOr(ClothingOutfitUtility.OutfitType.Clothing, null), null);
		if (base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06005D72 RID: 23922 RVA: 0x00222A0C File Offset: 0x00220C0C
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x06005D73 RID: 23923 RVA: 0x00222A24 File Offset: 0x00220C24
	private void PopulateGallery()
	{
		this.outfits.Clear();
		this.galleryGridItemPool.ReturnAll();
		this.RefreshGalleryFn = null;
		if (this.Config.isPickingOutfitForDupe)
		{
			this.<PopulateGallery>g__AddGridIconForTarget|35_0(Option.None);
		}
		OutfitBrowserScreenConfig config = this.Config;
		if (config.targetMinionInstance.HasValue)
		{
			ClothingOutfitUtility.OutfitType currentOutfitType = this.state.CurrentOutfitType;
			config = this.Config;
			this.<PopulateGallery>g__AddGridIconForTarget|35_0(ClothingOutfitTarget.FromMinion(currentOutfitType, config.targetMinionInstance.Value));
		}
		foreach (ClothingOutfitTarget value in from outfit in ClothingOutfitTarget.GetAllTemplates()
		where outfit.OutfitType == this.state.CurrentOutfitType
		select outfit)
		{
			this.<PopulateGallery>g__AddGridIconForTarget|35_0(value);
		}
		this.addButtonGridItem.transform.SetAsLastSibling();
		this.addButtonGridItem.SetActive(true);
		this.addButtonGridItem.GetComponent<MultiToggle>().onClick = delegate()
		{
			new OutfitDesignerScreenConfig(ClothingOutfitTarget.ForNewTemplateOutfit(this.state.CurrentOutfitType), this.Config.minionPersonality, this.Config.targetMinionInstance, new Action<ClothingOutfitTarget>(this.OnOutfitDesignerWritesToOutfitTarget)).ApplyAndOpenScreen();
		};
		this.RefreshGallery();
	}

	// Token: 0x06005D74 RID: 23924 RVA: 0x00222B44 File Offset: 0x00220D44
	private void OnOutfitDesignerWritesToOutfitTarget(ClothingOutfitTarget outfit)
	{
		this.Configure(this.Config.WithOutfit(outfit));
	}

	// Token: 0x06005D75 RID: 23925 RVA: 0x00222B6B File Offset: 0x00220D6B
	private void Update()
	{
		this.gridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x06005D76 RID: 23926 RVA: 0x00222B78 File Offset: 0x00220D78
	private void OnClickPickOutfit()
	{
		OutfitBrowserScreenConfig config = this.Config;
		if (config.targetMinionInstance.IsSome())
		{
			config = this.Config;
			WearableAccessorizer component = config.targetMinionInstance.Unwrap().GetComponent<WearableAccessorizer>();
			ClothingOutfitUtility.OutfitType currentOutfitType = this.state.CurrentOutfitType;
			Option<ClothingOutfitTarget> selectedOutfitOpt = this.state.SelectedOutfitOpt;
			component.AddCustomClothingOutfit(currentOutfitType, selectedOutfitOpt.AndThen<IEnumerable<ClothingItemResource>>((ClothingOutfitTarget outfit) => outfit.ReadItemValues()).UnwrapOr(ClothingOutfitTarget.NO_ITEM_VALUES, null));
		}
		else
		{
			config = this.Config;
			if (config.minionPersonality.IsSome())
			{
				ClothingOutfits clothingOutfits = Db.Get().Permits.ClothingOutfits;
				config = this.Config;
				string id = config.minionPersonality.Value.Id;
				Option<ClothingOutfitTarget> selectedOutfitOpt = this.state.SelectedOutfitOpt;
				clothingOutfits.SetDuplicantPersonalityOutfit(id, selectedOutfitOpt.AndThen<string>((ClothingOutfitTarget o) => o.OutfitId), this.state.CurrentOutfitType);
			}
		}
		LockerNavigator.Instance.PopScreen();
	}

	// Token: 0x06005D77 RID: 23927 RVA: 0x00222C90 File Offset: 0x00220E90
	public static void MakeDeletePopup(ClothingOutfitTarget sourceTarget, System.Action deleteFn)
	{
		Action<InfoDialogScreen> <>9__1;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.HEADER.Replace("{OutfitName}", sourceTarget.ReadName())).AddPlainText(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BODY.Replace("{OutfitName}", sourceTarget.ReadName()));
			string text = UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BUTTON_YES_DELETE;
			Action<InfoDialogScreen> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(InfoDialogScreen d)
				{
					deleteFn();
					d.Deactivate();
				});
			}
			infoDialogScreen.AddOption(text, action, true).AddOption(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BUTTON_DONT_DELETE, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
		});
	}

	// Token: 0x06005D78 RID: 23928 RVA: 0x00222CC8 File Offset: 0x00220EC8
	public static void MakeRenamePopup(KInputTextField inputFieldPrefab, ClothingOutfitTarget sourceTarget, Func<string> readName, Action<string> writeName)
	{
		KInputTextField inputField;
		InfoScreenPlainText errorText;
		KButton okButton;
		LocText okButtonText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_BROWSER_SCREEN.RENAME_POPUP.HEADER).AddUI<KInputTextField>(inputFieldPrefab, out inputField).AddSpacer(8f).AddUI<InfoScreenPlainText>(dialog.GetPlainTextPrefab(), out errorText).AddOption(true, out okButton, out okButtonText).AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, false);
			inputField.onValueChanged.AddListener(new UnityAction<string>(base.<MakeRenamePopup>g__Refresh|1));
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
				writeName(inputField.text);
				dialog.Deactivate();
			};
			base.<MakeRenamePopup>g__Refresh|1(readName());
		});
	}

	// Token: 0x06005D79 RID: 23929 RVA: 0x00222D10 File Offset: 0x00220F10
	private void SetButtonClickUISound(Option<ClothingOutfitTarget> target, MultiToggle toggle)
	{
		if (!target.HasValue)
		{
			toggle.states[1].on_click_override_sound_path = "HUD_Click";
			toggle.states[0].on_click_override_sound_path = "HUD_Click";
			return;
		}
		bool flag = !target.Value.DoesContainNonOwnedItems();
		toggle.states[1].on_click_override_sound_path = "ClothingItem_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (flag ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = "ClothingItem_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (flag ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	// Token: 0x06005D7A RID: 23930 RVA: 0x00222E22 File Offset: 0x00221022
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06005D82 RID: 23938 RVA: 0x0022331C File Offset: 0x0022151C
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIconForTarget|35_0(Option<ClothingOutfitTarget> target)
	{
		GameObject spawn = this.galleryGridItemPool.Borrow();
		GameObject gameObject = spawn.transform.GetChild(1).gameObject;
		GameObject isUnownedOverlayGO = spawn.transform.GetChild(2).gameObject;
		gameObject.SetActive(true);
		bool shouldShowOutfitWithDefaultItems = target.IsNone() || this.state.CurrentOutfitType == ClothingOutfitUtility.OutfitType.AtmoSuit;
		UIMannequin componentInChildren = gameObject.GetComponentInChildren<UIMannequin>();
		this.dioramaMinionOrMannequin.mannequin.shouldShowOutfitWithDefaultItems = shouldShowOutfitWithDefaultItems;
		componentInChildren.shouldShowOutfitWithDefaultItems = shouldShowOutfitWithDefaultItems;
		componentInChildren.personalityToUseForDefaultClothing = this.Config.minionPersonality;
		componentInChildren.SetOutfit(this.state.CurrentOutfitType, target);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		float x;
		float num;
		float num2;
		float y;
		switch (this.state.CurrentOutfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			x = 8f;
			num = 8f;
			num2 = 8f;
			y = 8f;
			break;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			throw new NotSupportedException();
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			x = 24f;
			num = 16f;
			num2 = 32f;
			y = 8f;
			break;
		default:
			throw new NotImplementedException();
		}
		component.offsetMin = new Vector2(x, y);
		component.offsetMax = new Vector2(-num, -num2);
		MultiToggle button = spawn.GetComponent<MultiToggle>();
		MultiToggle button2 = button;
		button2.onEnter = (System.Action)Delegate.Combine(button2.onEnter, new System.Action(this.OnMouseOverToggle));
		button.onClick = delegate()
		{
			this.state.SelectedOutfitOpt = target;
		};
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			button.ChangeState((target == this.state.SelectedOutfitOpt) ? 1 : 0);
			if (string.IsNullOrWhiteSpace(this.state.Filter) || target.IsNone())
			{
				spawn.SetActive(true);
			}
			else
			{
				spawn.SetActive(target.Unwrap().ReadName().ToLower().Contains(this.state.Filter.ToLower()));
			}
			if (!target.HasValue)
			{
				KleiItemsUI.ConfigureTooltipOn(spawn, KleiItemsUI.WrapAsToolTipTitle(KleiItemsUI.GetNoneOutfitName(this.state.CurrentOutfitType)));
				isUnownedOverlayGO.SetActive(false);
				return;
			}
			KleiItemsUI.ConfigureTooltipOn(spawn, KleiItemsUI.WrapAsToolTipTitle(target.Value.ReadName()));
			isUnownedOverlayGO.SetActive(target.Value.DoesContainNonOwnedItems());
		}));
		this.SetButtonClickUISound(target, button);
	}

	// Token: 0x04003EDE RID: 16094
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x04003EDF RID: 16095
	[SerializeField]
	private OutfitBrowserScreen_CategoriesAndSearchBar categoriesAndSearchBar;

	// Token: 0x04003EE0 RID: 16096
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04003EE1 RID: 16097
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x04003EE2 RID: 16098
	[SerializeField]
	private GameObject addButtonGridItem;

	// Token: 0x04003EE3 RID: 16099
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x04003EE4 RID: 16100
	private GridLayouter gridLayouter;

	// Token: 0x04003EE5 RID: 16101
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x04003EE6 RID: 16102
	[SerializeField]
	private UIMinionOrMannequin dioramaMinionOrMannequin;

	// Token: 0x04003EE7 RID: 16103
	[SerializeField]
	private Image dioramaBG;

	// Token: 0x04003EE8 RID: 16104
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x04003EE9 RID: 16105
	[SerializeField]
	private KButton pickOutfitButton;

	// Token: 0x04003EEA RID: 16106
	[SerializeField]
	private KButton editOutfitButton;

	// Token: 0x04003EEB RID: 16107
	[SerializeField]
	private KButton renameOutfitButton;

	// Token: 0x04003EEC RID: 16108
	[SerializeField]
	private KButton deleteOutfitButton;

	// Token: 0x04003EED RID: 16109
	[Header("Misc")]
	[SerializeField]
	private KInputTextField inputFieldPrefab;

	// Token: 0x04003EEE RID: 16110
	[SerializeField]
	public ColorStyleSetting selectedCategoryStyle;

	// Token: 0x04003EEF RID: 16111
	[SerializeField]
	public ColorStyleSetting notSelectedCategoryStyle;

	// Token: 0x04003EF0 RID: 16112
	public OutfitBrowserScreen.State state = new OutfitBrowserScreen.State();

	// Token: 0x04003EF1 RID: 16113
	public Option<ClothingOutfitUtility.OutfitType> lastShownOutfitType = Option.None;

	// Token: 0x04003EF2 RID: 16114
	private Dictionary<string, MultiToggle> outfits = new Dictionary<string, MultiToggle>();

	// Token: 0x04003EF4 RID: 16116
	private bool postponeConfiguration = true;

	// Token: 0x04003EF5 RID: 16117
	private bool isFirstDisplay = true;

	// Token: 0x04003EF6 RID: 16118
	private System.Action RefreshGalleryFn;

	// Token: 0x02001ADD RID: 6877
	public class State
	{
		// Token: 0x14000033 RID: 51
		// (add) Token: 0x0600985C RID: 39004 RVA: 0x00341098 File Offset: 0x0033F298
		// (remove) Token: 0x0600985D RID: 39005 RVA: 0x003410D0 File Offset: 0x0033F2D0
		public event System.Action OnSelectedOutfitOptChanged;

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x0600985E RID: 39006 RVA: 0x00341105 File Offset: 0x0033F305
		// (set) Token: 0x0600985F RID: 39007 RVA: 0x0034110D File Offset: 0x0033F30D
		public Option<ClothingOutfitTarget> SelectedOutfitOpt
		{
			get
			{
				return this.m_selectedOutfitOpt;
			}
			set
			{
				this.m_selectedOutfitOpt = value;
				if (this.OnSelectedOutfitOptChanged != null)
				{
					this.OnSelectedOutfitOptChanged();
				}
			}
		}

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06009860 RID: 39008 RVA: 0x0034112C File Offset: 0x0033F32C
		// (remove) Token: 0x06009861 RID: 39009 RVA: 0x00341164 File Offset: 0x0033F364
		public event System.Action OnCurrentOutfitTypeChanged;

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06009862 RID: 39010 RVA: 0x00341199 File Offset: 0x0033F399
		// (set) Token: 0x06009863 RID: 39011 RVA: 0x003411A1 File Offset: 0x0033F3A1
		public ClothingOutfitUtility.OutfitType CurrentOutfitType
		{
			get
			{
				return this.m_currentOutfitType;
			}
			set
			{
				this.m_currentOutfitType = value;
				if (this.OnCurrentOutfitTypeChanged != null)
				{
					this.OnCurrentOutfitTypeChanged();
				}
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06009864 RID: 39012 RVA: 0x003411C0 File Offset: 0x0033F3C0
		// (remove) Token: 0x06009865 RID: 39013 RVA: 0x003411F8 File Offset: 0x0033F3F8
		public event System.Action OnFilterChanged;

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06009866 RID: 39014 RVA: 0x0034122D File Offset: 0x0033F42D
		// (set) Token: 0x06009867 RID: 39015 RVA: 0x00341235 File Offset: 0x0033F435
		public string Filter
		{
			get
			{
				return this.m_filter;
			}
			set
			{
				this.m_filter = value;
				if (this.OnFilterChanged != null)
				{
					this.OnFilterChanged();
				}
			}
		}

		// Token: 0x04007AC6 RID: 31430
		private Option<ClothingOutfitTarget> m_selectedOutfitOpt;

		// Token: 0x04007AC7 RID: 31431
		private ClothingOutfitUtility.OutfitType m_currentOutfitType;

		// Token: 0x04007AC8 RID: 31432
		private string m_filter;
	}

	// Token: 0x02001ADE RID: 6878
	private enum MultiToggleState
	{
		// Token: 0x04007ACD RID: 31437
		Default,
		// Token: 0x04007ACE RID: 31438
		Selected,
		// Token: 0x04007ACF RID: 31439
		NonInteractable
	}
}
