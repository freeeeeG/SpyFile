using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B91 RID: 2961
public class MinionBrowserScreen : KMonoBehaviour
{
	// Token: 0x06005BFB RID: 23547 RVA: 0x0021AC80 File Offset: 0x00218E80
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.gridLayouter = new GridLayouter
		{
			minCellSize = 112f,
			maxCellSize = 144f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
	}

	// Token: 0x06005BFC RID: 23548 RVA: 0x0021ACE8 File Offset: 0x00218EE8
	protected override void OnCmpEnable()
	{
		if (this.isFirstDisplay)
		{
			this.isFirstDisplay = false;
			this.PopulateGallery();
			this.RefreshPreview();
			this.cycler.Initialize(this.CreateCycleOptions());
			this.editButton.onClick += delegate()
			{
				if (this.OnEditClickedFn != null)
				{
					this.OnEditClickedFn();
				}
			};
			this.changeOutfitButton.onClick += this.OnClickChangeOutfit;
		}
		else
		{
			this.RefreshGallery();
			this.RefreshPreview();
		}
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshGallery();
			this.RefreshPreview();
		});
	}

	// Token: 0x06005BFD RID: 23549 RVA: 0x0021AD74 File Offset: 0x00218F74
	private void Update()
	{
		this.gridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x06005BFE RID: 23550 RVA: 0x0021AD84 File Offset: 0x00218F84
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		if (this.Config.isValid)
		{
			this.Configure(this.Config);
			return;
		}
		this.Configure(MinionBrowserScreenConfig.Personalities(default(Option<Personality>)));
	}

	// Token: 0x1700068E RID: 1678
	// (get) Token: 0x06005BFF RID: 23551 RVA: 0x0021ADC6 File Offset: 0x00218FC6
	// (set) Token: 0x06005C00 RID: 23552 RVA: 0x0021ADCE File Offset: 0x00218FCE
	public MinionBrowserScreenConfig Config { get; private set; }

	// Token: 0x06005C01 RID: 23553 RVA: 0x0021ADD7 File Offset: 0x00218FD7
	public void Configure(MinionBrowserScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.PopulateGallery();
		this.RefreshPreview();
	}

	// Token: 0x06005C02 RID: 23554 RVA: 0x0021ADF5 File Offset: 0x00218FF5
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x06005C03 RID: 23555 RVA: 0x0021AE0C File Offset: 0x0021900C
	public void PopulateGallery()
	{
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		foreach (MinionBrowserScreen.GridItem item in this.Config.items)
		{
			this.<PopulateGallery>g__AddGridIcon|32_0(item);
		}
		this.RefreshGallery();
		this.SelectMinion(this.Config.defaultSelectedItem.Unwrap());
	}

	// Token: 0x06005C04 RID: 23556 RVA: 0x0021AE70 File Offset: 0x00219070
	private void SelectMinion(MinionBrowserScreen.GridItem item)
	{
		this.selectedGridItem = item;
		this.RefreshGallery();
		this.RefreshPreview();
		this.UIMinion.GetMinionVoice().PlaySoundUI("voice_land");
	}

	// Token: 0x06005C05 RID: 23557 RVA: 0x0021AEA8 File Offset: 0x002190A8
	public void RefreshPreview()
	{
		this.UIMinion.SetMinion(this.selectedGridItem.GetPersonality());
		this.UIMinion.ReactToPersonalityChange();
		this.detailsHeaderText.SetText(this.selectedGridItem.GetName());
		this.detailHeaderIcon.sprite = this.selectedGridItem.GetIcon();
		this.RefreshOutfitDescription();
		this.RefreshPreviewButtonsInteractable();
		this.SetDioramaBG();
	}

	// Token: 0x06005C06 RID: 23558 RVA: 0x0021AF14 File Offset: 0x00219114
	private void RefreshOutfitDescription()
	{
		if (this.RefreshOutfitDescriptionFn != null)
		{
			this.RefreshOutfitDescriptionFn();
		}
	}

	// Token: 0x06005C07 RID: 23559 RVA: 0x0021AF2C File Offset: 0x0021912C
	private void OnClickChangeOutfit()
	{
		if (this.selectedOutfitType.IsNone())
		{
			return;
		}
		OutfitBrowserScreenConfig.Minion(this.selectedOutfitType.Unwrap(), this.selectedGridItem).WithOutfit(this.selectedOutfit).ApplyAndOpenScreen();
	}

	// Token: 0x06005C08 RID: 23560 RVA: 0x0021AF74 File Offset: 0x00219174
	private void RefreshPreviewButtonsInteractable()
	{
		this.editButton.isInteractable = true;
		if (this.currentOutfitType == ClothingOutfitUtility.OutfitType.JoyResponse)
		{
			Option<string> joyResponseEditError = this.GetJoyResponseEditError();
			if (joyResponseEditError.IsSome())
			{
				this.editButton.isInteractable = false;
				this.editButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(joyResponseEditError.Unwrap());
				return;
			}
			this.editButton.isInteractable = true;
			this.editButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
		}
	}

	// Token: 0x06005C09 RID: 23561 RVA: 0x0021AFF0 File Offset: 0x002191F0
	private void SetDioramaBG()
	{
		this.dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.currentOutfitType);
	}

	// Token: 0x06005C0A RID: 23562 RVA: 0x0021B008 File Offset: 0x00219208
	private Option<string> GetJoyResponseEditError()
	{
		string joyTrait = this.selectedGridItem.GetPersonality().joyTrait;
		if (!(joyTrait == "BalloonArtist"))
		{
			return Option.Some<string>(UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_NO_FACADES_FOR_JOY_TRAIT.Replace("{JoyResponseType}", Db.Get().traits.Get(joyTrait).Name));
		}
		return Option.None;
	}

	// Token: 0x06005C0B RID: 23563 RVA: 0x0021B068 File Offset: 0x00219268
	public void SetEditingOutfitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		this.currentOutfitType = outfitType;
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_OUTFIT_ITEMS;
			this.changeOutfitButton.gameObject.SetActive(true);
			break;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_JOY_RESPONSE;
			this.changeOutfitButton.gameObject.SetActive(false);
			break;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_ATMO_SUIT_OUTFIT_ITEMS;
			this.changeOutfitButton.gameObject.SetActive(true);
			break;
		default:
			throw new NotImplementedException();
		}
		this.RefreshPreviewButtonsInteractable();
		this.OnEditClickedFn = delegate()
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				OutfitDesignerScreenConfig.Minion(this.selectedOutfit.IsSome() ? this.selectedOutfit.Unwrap() : ClothingOutfitTarget.ForNewTemplateOutfit(outfitType), this.selectedGridItem).ApplyAndOpenScreen();
				return;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
			{
				JoyResponseScreenConfig joyResponseScreenConfig = JoyResponseScreenConfig.From(this.selectedGridItem);
				joyResponseScreenConfig = joyResponseScreenConfig.WithInitialSelection(this.selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().AndThen<BalloonArtistFacadeResource>((string id) => Db.Get().Permits.BalloonArtistFacades.Get(id)));
				joyResponseScreenConfig.ApplyAndOpenScreen();
				return;
			}
			default:
				throw new NotImplementedException();
			}
		};
		this.RefreshOutfitDescriptionFn = delegate()
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				this.selectedOutfit = this.selectedGridItem.GetClothingOutfitTarget(outfitType);
				this.UIMinion.SetOutfit(outfitType, this.selectedOutfit);
				this.outfitDescriptionPanel.Refresh(this.selectedOutfit, outfitType, this.selectedGridItem.GetPersonality());
				return;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
			{
				this.selectedOutfit = this.selectedGridItem.GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType.Clothing);
				this.UIMinion.SetOutfit(ClothingOutfitUtility.OutfitType.Clothing, this.selectedOutfit);
				string text = this.selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().UnwrapOr(null, null);
				this.outfitDescriptionPanel.Refresh((text != null) ? Db.Get().Permits.Get(text) : null, outfitType, this.selectedGridItem.GetPersonality());
				return;
			}
			default:
				throw new NotImplementedException();
			}
		};
		this.RefreshOutfitDescription();
	}

	// Token: 0x06005C0C RID: 23564 RVA: 0x0021B160 File Offset: 0x00219360
	private MinionBrowserScreen.CyclerUI.OnSelectedFn[] CreateCycleOptions()
	{
		MinionBrowserScreen.CyclerUI.OnSelectedFn[] array = new MinionBrowserScreen.CyclerUI.OnSelectedFn[3];
		for (int i = 0; i < 3; i++)
		{
			ClothingOutfitUtility.OutfitType outfitType = (ClothingOutfitUtility.OutfitType)i;
			array[i] = delegate()
			{
				this.selectedOutfitType = Option.Some<ClothingOutfitUtility.OutfitType>(outfitType);
				this.cycler.SetLabel(outfitType.GetName());
				this.SetEditingOutfitType(outfitType);
				this.RefreshPreview();
			};
		}
		return array;
	}

	// Token: 0x06005C0D RID: 23565 RVA: 0x0021B1A4 File Offset: 0x002193A4
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06005C11 RID: 23569 RVA: 0x0021B1F0 File Offset: 0x002193F0
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIcon|32_0(MinionBrowserScreen.GridItem item)
	{
		GameObject gameObject = this.galleryGridItemPool.Borrow();
		gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = item.GetIcon();
		gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(item.GetName());
		MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
		MultiToggle toggle3 = toggle;
		toggle3.onEnter = (System.Action)Delegate.Combine(toggle3.onEnter, new System.Action(this.OnMouseOverToggle));
		MultiToggle toggle2 = toggle;
		toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
		{
			this.SelectMinion(item);
		}));
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			toggle.ChangeState((item == this.selectedGridItem) ? 1 : 0);
		}));
	}

	// Token: 0x04003DF1 RID: 15857
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04003DF2 RID: 15858
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x04003DF3 RID: 15859
	private GridLayouter gridLayouter;

	// Token: 0x04003DF4 RID: 15860
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	// Token: 0x04003DF5 RID: 15861
	[SerializeField]
	private UIMinion UIMinion;

	// Token: 0x04003DF6 RID: 15862
	[SerializeField]
	private LocText detailsHeaderText;

	// Token: 0x04003DF7 RID: 15863
	[SerializeField]
	private Image detailHeaderIcon;

	// Token: 0x04003DF8 RID: 15864
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x04003DF9 RID: 15865
	[SerializeField]
	private MinionBrowserScreen.CyclerUI cycler;

	// Token: 0x04003DFA RID: 15866
	[SerializeField]
	private KButton editButton;

	// Token: 0x04003DFB RID: 15867
	[SerializeField]
	private LocText editButtonText;

	// Token: 0x04003DFC RID: 15868
	[SerializeField]
	private KButton changeOutfitButton;

	// Token: 0x04003DFD RID: 15869
	private Option<ClothingOutfitUtility.OutfitType> selectedOutfitType;

	// Token: 0x04003DFE RID: 15870
	private Option<ClothingOutfitTarget> selectedOutfit;

	// Token: 0x04003DFF RID: 15871
	[Header("Diorama Backgrounds")]
	[SerializeField]
	private Image dioramaBGImage;

	// Token: 0x04003E00 RID: 15872
	private MinionBrowserScreen.GridItem selectedGridItem;

	// Token: 0x04003E01 RID: 15873
	private System.Action OnEditClickedFn;

	// Token: 0x04003E02 RID: 15874
	private bool isFirstDisplay = true;

	// Token: 0x04003E04 RID: 15876
	private bool postponeConfiguration = true;

	// Token: 0x04003E05 RID: 15877
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x04003E06 RID: 15878
	private System.Action RefreshGalleryFn;

	// Token: 0x04003E07 RID: 15879
	private System.Action RefreshOutfitDescriptionFn;

	// Token: 0x04003E08 RID: 15880
	private ClothingOutfitUtility.OutfitType currentOutfitType;

	// Token: 0x02001AB7 RID: 6839
	private enum MultiToggleState
	{
		// Token: 0x04007A36 RID: 31286
		Default,
		// Token: 0x04007A37 RID: 31287
		Selected,
		// Token: 0x04007A38 RID: 31288
		NonInteractable
	}

	// Token: 0x02001AB8 RID: 6840
	[Serializable]
	public class CyclerUI
	{
		// Token: 0x060097D4 RID: 38868 RVA: 0x0033FE78 File Offset: 0x0033E078
		public void Initialize(MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions)
		{
			this.cyclePrevButton.onClick += this.CyclePrev;
			this.cycleNextButton.onClick += this.CycleNext;
			this.SetCycleOptions(cycleOptions);
		}

		// Token: 0x060097D5 RID: 38869 RVA: 0x0033FEAF File Offset: 0x0033E0AF
		public void SetCycleOptions(MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions)
		{
			DebugUtil.Assert(cycleOptions != null);
			DebugUtil.Assert(cycleOptions.Length != 0);
			this.cycleOptions = cycleOptions;
			this.GoTo(0);
		}

		// Token: 0x060097D6 RID: 38870 RVA: 0x0033FED4 File Offset: 0x0033E0D4
		public void GoTo(int wrappingIndex)
		{
			if (this.cycleOptions == null || this.cycleOptions.Length == 0)
			{
				return;
			}
			while (wrappingIndex < 0)
			{
				wrappingIndex += this.cycleOptions.Length;
			}
			while (wrappingIndex >= this.cycleOptions.Length)
			{
				wrappingIndex -= this.cycleOptions.Length;
			}
			this.selectedIndex = wrappingIndex;
			this.cycleOptions[this.selectedIndex]();
		}

		// Token: 0x060097D7 RID: 38871 RVA: 0x0033FF35 File Offset: 0x0033E135
		public void CyclePrev()
		{
			this.GoTo(this.selectedIndex - 1);
		}

		// Token: 0x060097D8 RID: 38872 RVA: 0x0033FF45 File Offset: 0x0033E145
		public void CycleNext()
		{
			this.GoTo(this.selectedIndex + 1);
		}

		// Token: 0x060097D9 RID: 38873 RVA: 0x0033FF55 File Offset: 0x0033E155
		public void SetLabel(string text)
		{
			this.currentLabel.text = text;
		}

		// Token: 0x04007A39 RID: 31289
		[SerializeField]
		public KButton cyclePrevButton;

		// Token: 0x04007A3A RID: 31290
		[SerializeField]
		public KButton cycleNextButton;

		// Token: 0x04007A3B RID: 31291
		[SerializeField]
		public LocText currentLabel;

		// Token: 0x04007A3C RID: 31292
		[NonSerialized]
		private int selectedIndex = -1;

		// Token: 0x04007A3D RID: 31293
		[NonSerialized]
		private MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions;

		// Token: 0x02002232 RID: 8754
		// (Invoke) Token: 0x0600AD0F RID: 44303
		public delegate void OnSelectedFn();
	}

	// Token: 0x02001AB9 RID: 6841
	public abstract class GridItem : IEquatable<MinionBrowserScreen.GridItem>
	{
		// Token: 0x060097DB RID: 38875
		public abstract string GetName();

		// Token: 0x060097DC RID: 38876
		public abstract Sprite GetIcon();

		// Token: 0x060097DD RID: 38877
		public abstract string GetUniqueId();

		// Token: 0x060097DE RID: 38878
		public abstract Personality GetPersonality();

		// Token: 0x060097DF RID: 38879
		public abstract Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType);

		// Token: 0x060097E0 RID: 38880
		public abstract JoyResponseOutfitTarget GetJoyResponseOutfitTarget();

		// Token: 0x060097E1 RID: 38881 RVA: 0x0033FF74 File Offset: 0x0033E174
		public override bool Equals(object obj)
		{
			MinionBrowserScreen.GridItem gridItem = obj as MinionBrowserScreen.GridItem;
			return gridItem != null && this.Equals(gridItem);
		}

		// Token: 0x060097E2 RID: 38882 RVA: 0x0033FF94 File Offset: 0x0033E194
		public bool Equals(MinionBrowserScreen.GridItem other)
		{
			return this.GetHashCode() == other.GetHashCode();
		}

		// Token: 0x060097E3 RID: 38883 RVA: 0x0033FFA4 File Offset: 0x0033E1A4
		public override int GetHashCode()
		{
			return Hash.SDBMLower(this.GetUniqueId());
		}

		// Token: 0x060097E4 RID: 38884 RVA: 0x0033FFB1 File Offset: 0x0033E1B1
		public override string ToString()
		{
			return this.GetUniqueId();
		}

		// Token: 0x060097E5 RID: 38885 RVA: 0x0033FFBC File Offset: 0x0033E1BC
		public static MinionBrowserScreen.GridItem.MinionInstanceTarget Of(GameObject minionInstance)
		{
			MinionIdentity component = minionInstance.GetComponent<MinionIdentity>();
			return new MinionBrowserScreen.GridItem.MinionInstanceTarget
			{
				minionInstance = minionInstance,
				minionIdentity = component,
				personality = Db.Get().Personalities.Get(component.personalityResourceId)
			};
		}

		// Token: 0x060097E6 RID: 38886 RVA: 0x0033FFFE File Offset: 0x0033E1FE
		public static MinionBrowserScreen.GridItem.PersonalityTarget Of(Personality personality)
		{
			return new MinionBrowserScreen.GridItem.PersonalityTarget
			{
				personality = personality
			};
		}

		// Token: 0x02002233 RID: 8755
		public class MinionInstanceTarget : MinionBrowserScreen.GridItem
		{
			// Token: 0x0600AD12 RID: 44306 RVA: 0x00378FDC File Offset: 0x003771DC
			public override Sprite GetIcon()
			{
				return this.personality.GetMiniIcon();
			}

			// Token: 0x0600AD13 RID: 44307 RVA: 0x00378FE9 File Offset: 0x003771E9
			public override string GetName()
			{
				return this.minionIdentity.GetProperName();
			}

			// Token: 0x0600AD14 RID: 44308 RVA: 0x00378FF8 File Offset: 0x003771F8
			public override string GetUniqueId()
			{
				return "minion_instance_id::" + this.minionInstance.GetInstanceID().ToString();
			}

			// Token: 0x0600AD15 RID: 44309 RVA: 0x00379022 File Offset: 0x00377222
			public override Personality GetPersonality()
			{
				return this.personality;
			}

			// Token: 0x0600AD16 RID: 44310 RVA: 0x0037902A File Offset: 0x0037722A
			public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType)
			{
				return ClothingOutfitTarget.FromMinion(outfitType, this.minionInstance);
			}

			// Token: 0x0600AD17 RID: 44311 RVA: 0x0037903D File Offset: 0x0037723D
			public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
			{
				return JoyResponseOutfitTarget.FromMinion(this.minionInstance);
			}

			// Token: 0x040098E5 RID: 39141
			public GameObject minionInstance;

			// Token: 0x040098E6 RID: 39142
			public MinionIdentity minionIdentity;

			// Token: 0x040098E7 RID: 39143
			public Personality personality;
		}

		// Token: 0x02002234 RID: 8756
		public class PersonalityTarget : MinionBrowserScreen.GridItem
		{
			// Token: 0x0600AD19 RID: 44313 RVA: 0x00379052 File Offset: 0x00377252
			public override Sprite GetIcon()
			{
				return this.personality.GetMiniIcon();
			}

			// Token: 0x0600AD1A RID: 44314 RVA: 0x0037905F File Offset: 0x0037725F
			public override string GetName()
			{
				return this.personality.Name;
			}

			// Token: 0x0600AD1B RID: 44315 RVA: 0x0037906C File Offset: 0x0037726C
			public override string GetUniqueId()
			{
				return "personality::" + this.personality.nameStringKey;
			}

			// Token: 0x0600AD1C RID: 44316 RVA: 0x00379083 File Offset: 0x00377283
			public override Personality GetPersonality()
			{
				return this.personality;
			}

			// Token: 0x0600AD1D RID: 44317 RVA: 0x0037908B File Offset: 0x0037728B
			public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType)
			{
				return ClothingOutfitTarget.TryFromTemplateId(this.personality.GetSelectedTemplateOutfitId(outfitType));
			}

			// Token: 0x0600AD1E RID: 44318 RVA: 0x0037909E File Offset: 0x0037729E
			public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
			{
				return JoyResponseOutfitTarget.FromPersonality(this.personality);
			}

			// Token: 0x040098E8 RID: 39144
			public Personality personality;
		}
	}
}
