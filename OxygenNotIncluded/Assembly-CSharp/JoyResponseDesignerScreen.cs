using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B28 RID: 2856
public class JoyResponseDesignerScreen : KMonoBehaviour
{
	// Token: 0x1700066F RID: 1647
	// (get) Token: 0x060057EA RID: 22506 RVA: 0x00203997 File Offset: 0x00201B97
	// (set) Token: 0x060057EB RID: 22507 RVA: 0x0020399F File Offset: 0x00201B9F
	public JoyResponseScreenConfig Config { get; private set; }

	// Token: 0x060057EC RID: 22508 RVA: 0x002039A8 File Offset: 0x00201BA8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(this.categoryRowPrefab.transform.parent == this.categoryListContent.transform);
		global::Debug.Assert(this.galleryItemPrefab.transform.parent == this.galleryGridContent.transform);
		this.categoryRowPrefab.SetActive(false);
		this.galleryItemPrefab.SetActive(false);
		this.galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.categoryRowPool = new UIPrefabLocalPool(this.categoryRowPrefab, this.categoryListContent.gameObject);
		this.galleryGridItemPool = new UIPrefabLocalPool(this.galleryItemPrefab, this.galleryGridContent.gameObject);
		JoyResponseDesignerScreen.JoyResponseCategory[] array = new JoyResponseDesignerScreen.JoyResponseCategory[1];
		int num = 0;
		JoyResponseDesignerScreen.JoyResponseCategory joyResponseCategory = new JoyResponseDesignerScreen.JoyResponseCategory();
		joyResponseCategory.displayName = UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSES.BALLOON_ARTIST;
		joyResponseCategory.icon = Assets.GetSprite("icon_inventory_balloonartist");
		JoyResponseDesignerScreen.GalleryItem[] items = (from r in Db.Get().Permits.BalloonArtistFacades.resources
		select JoyResponseDesignerScreen.GalleryItem.Of(r)).Prepend(JoyResponseDesignerScreen.GalleryItem.Of(Option.None)).ToArray<JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget>();
		joyResponseCategory.items = items;
		array[num] = joyResponseCategory;
		this.joyResponseCategories = array;
		this.dioramaVis.ConfigureSetup();
	}

	// Token: 0x060057ED RID: 22509 RVA: 0x00203B29 File Offset: 0x00201D29
	private void Update()
	{
		this.galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x060057EE RID: 22510 RVA: 0x00203B36 File Offset: 0x00201D36
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		if (this.Config.isValid)
		{
			this.Configure(this.Config);
			return;
		}
		throw new InvalidOperationException("Cannot open up JoyResponseDesignerScreen without a target personality or minion instance");
	}

	// Token: 0x060057EF RID: 22511 RVA: 0x00203B63 File Offset: 0x00201D63
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.Configure(this.Config);
		});
	}

	// Token: 0x060057F0 RID: 22512 RVA: 0x00203B84 File Offset: 0x00201D84
	public void Configure(JoyResponseScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.RegisterPreventScreenPop();
		this.primaryButton.ClearOnClick();
		TMP_Text componentInChildren = this.primaryButton.GetComponentInChildren<LocText>();
		LocString button_APPLY_TO_MINION = UI.JOY_RESPONSE_DESIGNER_SCREEN.BUTTON_APPLY_TO_MINION;
		string search = "{MinionName}";
		JoyResponseScreenConfig config2 = this.Config;
		componentInChildren.SetText(button_APPLY_TO_MINION.Replace(search, config2.target.GetMinionName()));
		this.primaryButton.onClick += delegate()
		{
			Option<PermitResource> permitResource = this.selectedGalleryItem.GetPermitResource();
			if (permitResource.IsSome())
			{
				string str = "Save selected balloon ";
				string name = this.selectedGalleryItem.GetName();
				string str2 = " for ";
				JoyResponseScreenConfig config3 = this.Config;
				global::Debug.Log(str + name + str2 + config3.target.GetMinionName());
				if (this.CanSaveSelection())
				{
					config3 = this.Config;
					config3.target.WriteFacadeId(permitResource.Unwrap().Id);
				}
			}
			else
			{
				string str3 = "Save selected balloon ";
				string name2 = this.selectedGalleryItem.GetName();
				string str4 = " for ";
				JoyResponseScreenConfig config3 = this.Config;
				global::Debug.Log(str3 + name2 + str4 + config3.target.GetMinionName());
				config3 = this.Config;
				config3.target.WriteFacadeId(Option.None);
			}
			LockerNavigator.Instance.PopScreen();
		};
		this.PopulateCategories();
		this.PopulateGallery();
		this.PopulatePreview();
		config2 = this.Config;
		if (config2.initalSelectedItem.IsSome())
		{
			config2 = this.Config;
			this.SelectGalleryItem(config2.initalSelectedItem.Unwrap());
		}
	}

	// Token: 0x060057F1 RID: 22513 RVA: 0x00203C3C File Offset: 0x00201E3C
	private bool CanSaveSelection()
	{
		return this.GetSaveSelectionError().IsNone();
	}

	// Token: 0x060057F2 RID: 22514 RVA: 0x00203C58 File Offset: 0x00201E58
	private Option<string> GetSaveSelectionError()
	{
		if (!this.selectedGalleryItem.IsUnlocked())
		{
			return Option.Some<string>(UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_PICK_JOY_RESPONSE_ERROR_LOCKED.Replace("{MinionName}", this.Config.target.GetMinionName()));
		}
		return Option.None;
	}

	// Token: 0x060057F3 RID: 22515 RVA: 0x00203CA4 File Offset: 0x00201EA4
	private void RefreshCategories()
	{
		if (this.RefreshCategoriesFn != null)
		{
			this.RefreshCategoriesFn();
		}
	}

	// Token: 0x060057F4 RID: 22516 RVA: 0x00203CBC File Offset: 0x00201EBC
	public void PopulateCategories()
	{
		this.RefreshCategoriesFn = null;
		this.categoryRowPool.ReturnAll();
		JoyResponseDesignerScreen.JoyResponseCategory[] array = this.joyResponseCategories;
		for (int i = 0; i < array.Length; i++)
		{
			JoyResponseDesignerScreen.<>c__DisplayClass28_0 CS$<>8__locals1 = new JoyResponseDesignerScreen.<>c__DisplayClass28_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.category = array[i];
			GameObject gameObject = this.categoryRowPool.Borrow();
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(CS$<>8__locals1.category.displayName);
			component.GetReference<Image>("Icon").sprite = CS$<>8__locals1.category.icon;
			MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
			MultiToggle toggle2 = toggle;
			toggle2.onEnter = (System.Action)Delegate.Combine(toggle2.onEnter, new System.Action(this.OnMouseOverToggle));
			toggle.onClick = delegate()
			{
				CS$<>8__locals1.<>4__this.SelectCategory(CS$<>8__locals1.category);
			};
			this.RefreshCategoriesFn = (System.Action)Delegate.Combine(this.RefreshCategoriesFn, new System.Action(delegate()
			{
				toggle.ChangeState((CS$<>8__locals1.category == CS$<>8__locals1.<>4__this.selectedCategoryOpt) ? 1 : 0);
			}));
			this.SetCatogoryClickUISound(CS$<>8__locals1.category, toggle);
		}
		this.SelectCategory(this.joyResponseCategories[0]);
	}

	// Token: 0x060057F5 RID: 22517 RVA: 0x00203E03 File Offset: 0x00202003
	public void SelectCategory(JoyResponseDesignerScreen.JoyResponseCategory category)
	{
		this.selectedCategoryOpt = category;
		this.galleryHeaderLabel.text = category.displayName;
		this.RefreshCategories();
		this.PopulateGallery();
		this.RefreshPreview();
	}

	// Token: 0x060057F6 RID: 22518 RVA: 0x00203E34 File Offset: 0x00202034
	private void SetCatogoryClickUISound(JoyResponseDesignerScreen.JoyResponseCategory category, MultiToggle toggle)
	{
	}

	// Token: 0x060057F7 RID: 22519 RVA: 0x00203E36 File Offset: 0x00202036
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x060057F8 RID: 22520 RVA: 0x00203E4C File Offset: 0x0020204C
	public void PopulateGallery()
	{
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		if (this.selectedCategoryOpt.IsNone())
		{
			return;
		}
		JoyResponseDesignerScreen.JoyResponseCategory joyResponseCategory = this.selectedCategoryOpt.Unwrap();
		foreach (JoyResponseDesignerScreen.GalleryItem item in joyResponseCategory.items)
		{
			this.<PopulateGallery>g__AddGridIcon|36_0(item);
		}
		this.SelectGalleryItem(joyResponseCategory.items[0]);
	}

	// Token: 0x060057F9 RID: 22521 RVA: 0x00203EB3 File Offset: 0x002020B3
	public void SelectGalleryItem(JoyResponseDesignerScreen.GalleryItem item)
	{
		this.selectedGalleryItem = item;
		this.RefreshGallery();
		this.RefreshPreview();
	}

	// Token: 0x060057FA RID: 22522 RVA: 0x00203EC8 File Offset: 0x002020C8
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x060057FB RID: 22523 RVA: 0x00203EDA File Offset: 0x002020DA
	public void RefreshPreview()
	{
		if (this.RefreshPreviewFn != null)
		{
			this.RefreshPreviewFn();
		}
	}

	// Token: 0x060057FC RID: 22524 RVA: 0x00203EEF File Offset: 0x002020EF
	public void PopulatePreview()
	{
		this.RefreshPreviewFn = (System.Action)Delegate.Combine(this.RefreshPreviewFn, new System.Action(delegate()
		{
			JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget balloonArtistFacadeTarget = this.selectedGalleryItem as JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget;
			if (balloonArtistFacadeTarget == null)
			{
				throw new NotImplementedException();
			}
			Option<PermitResource> permitResource = balloonArtistFacadeTarget.GetPermitResource();
			this.selectionHeaderLabel.SetText(balloonArtistFacadeTarget.GetName());
			KleiPermitDioramaVis_JoyResponseBalloon kleiPermitDioramaVis_JoyResponseBalloon = this.dioramaVis;
			JoyResponseScreenConfig config = this.Config;
			kleiPermitDioramaVis_JoyResponseBalloon.SetMinion(config.target.GetPersonality());
			this.dioramaVis.ConfigureWith(balloonArtistFacadeTarget.permit);
			OutfitDescriptionPanel outfitDescriptionPanel = this.outfitDescriptionPanel;
			PermitResource permitResource2 = permitResource.UnwrapOr(null, null);
			ClothingOutfitUtility.OutfitType outfitType = ClothingOutfitUtility.OutfitType.JoyResponse;
			config = this.Config;
			outfitDescriptionPanel.Refresh(permitResource2, outfitType, config.target.GetPersonality());
			Option<string> saveSelectionError = this.GetSaveSelectionError();
			if (saveSelectionError.IsSome())
			{
				this.primaryButton.isInteractable = false;
				this.primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(saveSelectionError.Unwrap());
				return;
			}
			this.primaryButton.isInteractable = true;
			this.primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
		}));
		this.RefreshPreview();
	}

	// Token: 0x060057FD RID: 22525 RVA: 0x00203F19 File Offset: 0x00202119
	private void RegisterPreventScreenPop()
	{
		this.UnregisterPreventScreenPop();
		this.preventScreenPopFn = delegate()
		{
			if (this.Config.target.ReadFacadeId() != this.selectedGalleryItem.GetPermitResource().AndThen<string>((PermitResource r) => r.Id))
			{
				this.RegisterPreventScreenPop();
				JoyResponseDesignerScreen.MakeSaveWarningPopup(this.Config.target, delegate
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

	// Token: 0x060057FE RID: 22526 RVA: 0x00203F48 File Offset: 0x00202148
	private void UnregisterPreventScreenPop()
	{
		if (this.preventScreenPopFn != null)
		{
			LockerNavigator.Instance.preventScreenPop.Remove(this.preventScreenPopFn);
			this.preventScreenPopFn = null;
		}
	}

	// Token: 0x060057FF RID: 22527 RVA: 0x00203F70 File Offset: 0x00202170
	public static void MakeSaveWarningPopup(JoyResponseOutfitTarget target, System.Action discardChangesFn)
	{
		Action<InfoDialogScreen> <>9__1;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.JOY_RESPONSE_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.HEADER.Replace("{MinionName}", target.GetMinionName())).AddPlainText(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BODY);
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

	// Token: 0x06005803 RID: 22531 RVA: 0x002040A0 File Offset: 0x002022A0
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIcon|36_0(JoyResponseDesignerScreen.GalleryItem item)
	{
		GameObject gameObject = this.galleryGridItemPool.Borrow();
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = item.GetIcon();
		component.GetReference<Image>("IsUnownedOverlay").gameObject.SetActive(!item.IsUnlocked());
		Option<PermitResource> permitResource = item.GetPermitResource();
		if (permitResource.IsSome())
		{
			KleiItemsUI.ConfigureTooltipOn(gameObject, KleiItemsUI.GetTooltipStringFor(permitResource.Unwrap()));
		}
		else
		{
			KleiItemsUI.ConfigureTooltipOn(gameObject, KleiItemsUI.GetNoneTooltipStringFor(PermitCategory.JoyResponse));
		}
		MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
		MultiToggle toggle3 = toggle;
		toggle3.onEnter = (System.Action)Delegate.Combine(toggle3.onEnter, new System.Action(this.OnMouseOverToggle));
		MultiToggle toggle2 = toggle;
		toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
		{
			this.SelectGalleryItem(item);
		}));
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			toggle.ChangeState((item == this.selectedGalleryItem) ? 1 : 0);
		}));
	}

	// Token: 0x04003B72 RID: 15218
	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	// Token: 0x04003B73 RID: 15219
	[SerializeField]
	private GameObject categoryRowPrefab;

	// Token: 0x04003B74 RID: 15220
	[Header("GalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	// Token: 0x04003B75 RID: 15221
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04003B76 RID: 15222
	[SerializeField]
	private GameObject galleryItemPrefab;

	// Token: 0x04003B77 RID: 15223
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	// Token: 0x04003B78 RID: 15224
	[SerializeField]
	private KleiPermitDioramaVis_JoyResponseBalloon dioramaVis;

	// Token: 0x04003B79 RID: 15225
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x04003B7A RID: 15226
	[SerializeField]
	private KButton primaryButton;

	// Token: 0x04003B7C RID: 15228
	public JoyResponseDesignerScreen.JoyResponseCategory[] joyResponseCategories;

	// Token: 0x04003B7D RID: 15229
	private bool postponeConfiguration = true;

	// Token: 0x04003B7E RID: 15230
	private Option<JoyResponseDesignerScreen.JoyResponseCategory> selectedCategoryOpt;

	// Token: 0x04003B7F RID: 15231
	private UIPrefabLocalPool categoryRowPool;

	// Token: 0x04003B80 RID: 15232
	private System.Action RefreshCategoriesFn;

	// Token: 0x04003B81 RID: 15233
	private JoyResponseDesignerScreen.GalleryItem selectedGalleryItem;

	// Token: 0x04003B82 RID: 15234
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x04003B83 RID: 15235
	private GridLayouter galleryGridLayouter;

	// Token: 0x04003B84 RID: 15236
	private System.Action RefreshGalleryFn;

	// Token: 0x04003B85 RID: 15237
	public System.Action RefreshPreviewFn;

	// Token: 0x04003B86 RID: 15238
	private Func<bool> preventScreenPopFn;

	// Token: 0x02001A28 RID: 6696
	public class JoyResponseCategory
	{
		// Token: 0x040078B0 RID: 30896
		public string displayName;

		// Token: 0x040078B1 RID: 30897
		public Sprite icon;

		// Token: 0x040078B2 RID: 30898
		public JoyResponseDesignerScreen.GalleryItem[] items;
	}

	// Token: 0x02001A29 RID: 6697
	private enum MultiToggleState
	{
		// Token: 0x040078B4 RID: 30900
		Default,
		// Token: 0x040078B5 RID: 30901
		Selected
	}

	// Token: 0x02001A2A RID: 6698
	public abstract class GalleryItem : IEquatable<JoyResponseDesignerScreen.GalleryItem>
	{
		// Token: 0x0600963C RID: 38460
		public abstract string GetName();

		// Token: 0x0600963D RID: 38461
		public abstract Sprite GetIcon();

		// Token: 0x0600963E RID: 38462
		public abstract string GetUniqueId();

		// Token: 0x0600963F RID: 38463
		public abstract bool IsUnlocked();

		// Token: 0x06009640 RID: 38464
		public abstract Option<PermitResource> GetPermitResource();

		// Token: 0x06009641 RID: 38465 RVA: 0x0033C214 File Offset: 0x0033A414
		public override bool Equals(object obj)
		{
			JoyResponseDesignerScreen.GalleryItem galleryItem = obj as JoyResponseDesignerScreen.GalleryItem;
			return galleryItem != null && this.Equals(galleryItem);
		}

		// Token: 0x06009642 RID: 38466 RVA: 0x0033C234 File Offset: 0x0033A434
		public bool Equals(JoyResponseDesignerScreen.GalleryItem other)
		{
			return this.GetHashCode() == other.GetHashCode();
		}

		// Token: 0x06009643 RID: 38467 RVA: 0x0033C244 File Offset: 0x0033A444
		public override int GetHashCode()
		{
			return Hash.SDBMLower(this.GetUniqueId());
		}

		// Token: 0x06009644 RID: 38468 RVA: 0x0033C251 File Offset: 0x0033A451
		public override string ToString()
		{
			return this.GetUniqueId();
		}

		// Token: 0x06009645 RID: 38469 RVA: 0x0033C259 File Offset: 0x0033A459
		public static JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget Of(Option<BalloonArtistFacadeResource> permit)
		{
			return new JoyResponseDesignerScreen.GalleryItem.BalloonArtistFacadeTarget
			{
				permit = permit
			};
		}

		// Token: 0x0200222E RID: 8750
		public class BalloonArtistFacadeTarget : JoyResponseDesignerScreen.GalleryItem
		{
			// Token: 0x0600ACFC RID: 44284 RVA: 0x00378E54 File Offset: 0x00377054
			public override Sprite GetIcon()
			{
				return this.permit.AndThen<Sprite>((BalloonArtistFacadeResource p) => p.GetPermitPresentationInfo().sprite).UnwrapOrElse(() => KleiItemsUI.GetNoneBalloonArtistIcon(), null);
			}

			// Token: 0x0600ACFD RID: 44285 RVA: 0x00378EB4 File Offset: 0x003770B4
			public override string GetName()
			{
				return this.permit.AndThen<string>((BalloonArtistFacadeResource p) => p.Name).UnwrapOrElse(() => KleiItemsUI.GetNoneClothingItemStrings(PermitCategory.JoyResponse).Item1, null);
			}

			// Token: 0x0600ACFE RID: 44286 RVA: 0x00378F14 File Offset: 0x00377114
			public override string GetUniqueId()
			{
				return "balloon_artist_facade::" + this.permit.AndThen<string>((BalloonArtistFacadeResource p) => p.Id).UnwrapOr("<none>", null);
			}

			// Token: 0x0600ACFF RID: 44287 RVA: 0x00378F63 File Offset: 0x00377163
			public override Option<PermitResource> GetPermitResource()
			{
				return this.permit.AndThen<PermitResource>((BalloonArtistFacadeResource p) => p);
			}

			// Token: 0x0600AD00 RID: 44288 RVA: 0x00378F90 File Offset: 0x00377190
			public override bool IsUnlocked()
			{
				return this.GetPermitResource().AndThen<bool>((PermitResource p) => p.IsUnlocked()).UnwrapOr(true, null);
			}

			// Token: 0x040098E4 RID: 39140
			public Option<BalloonArtistFacadeResource> permit;
		}
	}
}
