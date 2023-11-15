using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BB4 RID: 2996
public class OutfitDescriptionPanel : KMonoBehaviour
{
	// Token: 0x06005D88 RID: 23944 RVA: 0x00223712 File Offset: 0x00221912
	public void Refresh(PermitResource permitResource, ClothingOutfitUtility.OutfitType outfitType, Option<Personality> personality)
	{
		if (permitResource != null)
		{
			this.Refresh(permitResource.Name, new string[]
			{
				permitResource.Id
			}, outfitType, personality);
			return;
		}
		this.Refresh(UI.OUTFIT_NAME.NONE, OutfitDescriptionPanel.NO_ITEMS, outfitType, personality);
	}

	// Token: 0x06005D89 RID: 23945 RVA: 0x0022374C File Offset: 0x0022194C
	public void Refresh(Option<ClothingOutfitTarget> outfit, ClothingOutfitUtility.OutfitType outfitType, Option<Personality> personality)
	{
		if (outfit.IsSome())
		{
			this.Refresh(outfit.Unwrap().ReadName(), outfit.Unwrap().ReadItems(), outfitType, personality);
			return;
		}
		this.Refresh(KleiItemsUI.GetNoneOutfitName(outfitType), OutfitDescriptionPanel.NO_ITEMS, outfitType, personality);
	}

	// Token: 0x06005D8A RID: 23946 RVA: 0x0022379C File Offset: 0x0022199C
	public void Refresh(OutfitDesignerScreen_OutfitState outfitState, Option<Personality> personality)
	{
		this.Refresh(outfitState.name, outfitState.GetItems(), outfitState.outfitType, personality);
	}

	// Token: 0x06005D8B RID: 23947 RVA: 0x002237B8 File Offset: 0x002219B8
	public void Refresh(string outfitName, string[] outfitItemIds, ClothingOutfitUtility.OutfitType outfitType, Option<Personality> personality)
	{
		this.ClearItemDescRows();
		using (DictionaryPool<PermitCategory, Option<PermitResource>, OutfitDescriptionPanel>.PooledDictionary pooledDictionary = PoolsFor<OutfitDescriptionPanel>.AllocateDict<PermitCategory, Option<PermitResource>>())
		{
			using (ListPool<PermitResource, OutfitDescriptionPanel>.PooledList pooledList = PoolsFor<OutfitDescriptionPanel>.AllocateList<PermitResource>())
			{
				switch (outfitType)
				{
				case ClothingOutfitUtility.OutfitType.Clothing:
					this.outfitNameLabel.SetText(outfitName);
					this.outfitDescriptionLabel.gameObject.SetActive(false);
					foreach (PermitCategory key in ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_CLOTHING)
					{
						pooledDictionary.Add(key, Option.None);
					}
					break;
				case ClothingOutfitUtility.OutfitType.JoyResponse:
					if (outfitItemIds != null && outfitItemIds.Length != 0)
					{
						if (Db.Get().Permits.BalloonArtistFacades.TryGet(outfitItemIds[0]) != null)
						{
							this.outfitDescriptionLabel.gameObject.SetActive(true);
							string text = DUPLICANTS.TRAITS.BALLOONARTIST.NAME;
							this.outfitNameLabel.SetText(text);
							this.outfitDescriptionLabel.SetText(outfitName);
						}
					}
					else
					{
						this.outfitNameLabel.SetText(outfitName);
						this.outfitDescriptionLabel.gameObject.SetActive(false);
					}
					pooledDictionary.Add(PermitCategory.JoyResponse, Option.None);
					break;
				case ClothingOutfitUtility.OutfitType.AtmoSuit:
					this.outfitNameLabel.SetText(outfitName);
					this.outfitDescriptionLabel.gameObject.SetActive(false);
					foreach (PermitCategory key2 in ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_ATMO_SUITS)
					{
						pooledDictionary.Add(key2, Option.None);
					}
					break;
				}
				foreach (string id in outfitItemIds)
				{
					PermitResource permitResource = Db.Get().Permits.Get(id);
					Option<PermitResource> option;
					if (pooledDictionary.TryGetValue(permitResource.Category, out option) && !option.HasValue)
					{
						pooledDictionary[permitResource.Category] = permitResource;
					}
					else
					{
						pooledList.Add(permitResource);
					}
				}
				foreach (KeyValuePair<PermitCategory, Option<PermitResource>> self in pooledDictionary)
				{
					PermitCategory permitCategory;
					Option<PermitResource> option2;
					self.Deconstruct(out permitCategory, out option2);
					PermitCategory category = permitCategory;
					Option<PermitResource> option3 = option2;
					if (option3.HasValue)
					{
						this.AddItemDescRow(option3.Value);
					}
					else
					{
						this.AddItemDescRow(KleiItemsUI.GetNoneClothingItemIcon(category, personality), KleiItemsUI.GetNoneClothingItemStrings(category).Item1, null, 1f);
					}
				}
				foreach (PermitResource permitResource2 in pooledList)
				{
					ClothingItemResource permit = (ClothingItemResource)permitResource2;
					this.AddItemDescRow(permit);
				}
			}
		}
		bool flag = ClothingOutfitTarget.DoesContainNonOwnedItems(outfitItemIds);
		this.usesUnownedItemsLabel.transform.SetAsLastSibling();
		if (!flag)
		{
			this.usesUnownedItemsLabel.gameObject.SetActive(false);
		}
		else
		{
			this.usesUnownedItemsLabel.SetText(KleiItemsUI.WrapWithColor(UI.OUTFIT_DESCRIPTION.CONTAINS_NON_OWNED_ITEMS, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED));
			this.usesUnownedItemsLabel.gameObject.SetActive(true);
		}
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.Refresh(outfitName, outfitItemIds, outfitType, personality);
		});
	}

	// Token: 0x06005D8C RID: 23948 RVA: 0x00223B80 File Offset: 0x00221D80
	private void ClearItemDescRows()
	{
		for (int i = 0; i < this.itemDescriptionRows.Count; i++)
		{
			UnityEngine.Object.Destroy(this.itemDescriptionRows[i]);
		}
		this.itemDescriptionRows.Clear();
	}

	// Token: 0x06005D8D RID: 23949 RVA: 0x00223BC0 File Offset: 0x00221DC0
	private void AddItemDescRow(PermitResource permit)
	{
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		bool flag = permit.IsUnlocked();
		string tooltip = flag ? null : UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE;
		this.AddItemDescRow(permitPresentationInfo.sprite, permit.Name, tooltip, flag ? 1f : 0.7f);
	}

	// Token: 0x06005D8E RID: 23950 RVA: 0x00223C10 File Offset: 0x00221E10
	private void AddItemDescRow(Sprite icon, string text, string tooltip = null, float alpha = 1f)
	{
		GameObject gameObject = Util.KInstantiateUI(this.itemDescriptionRowPrefab, this.itemDescriptionContainer, true);
		this.itemDescriptionRows.Add(gameObject);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = icon;
		component.GetReference<LocText>("Label").SetText(text);
		gameObject.AddOrGet<CanvasGroup>().alpha = alpha;
		gameObject.AddOrGet<NonDrawingGraphic>();
		if (tooltip != null)
		{
			gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(tooltip);
			return;
		}
		gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
	}

	// Token: 0x04003EFC RID: 16124
	[SerializeField]
	public LocText outfitNameLabel;

	// Token: 0x04003EFD RID: 16125
	[SerializeField]
	public LocText outfitDescriptionLabel;

	// Token: 0x04003EFE RID: 16126
	[SerializeField]
	private GameObject itemDescriptionRowPrefab;

	// Token: 0x04003EFF RID: 16127
	[SerializeField]
	private GameObject itemDescriptionContainer;

	// Token: 0x04003F00 RID: 16128
	[SerializeField]
	private LocText usesUnownedItemsLabel;

	// Token: 0x04003F01 RID: 16129
	private List<GameObject> itemDescriptionRows = new List<GameObject>();

	// Token: 0x04003F02 RID: 16130
	public static readonly string[] NO_ITEMS = new string[0];
}
