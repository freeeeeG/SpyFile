using System;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x02000B3B RID: 2875
public static class KleiItemsUI
{
	// Token: 0x060058C5 RID: 22725 RVA: 0x00208385 File Offset: 0x00206585
	public static string WrapAsToolTipTitle(string text)
	{
		return "<b><style=\"KLink\">" + text + "</style></b>";
	}

	// Token: 0x060058C6 RID: 22726 RVA: 0x00208397 File Offset: 0x00206597
	public static string WrapWithColor(string text, Color color)
	{
		return string.Concat(new string[]
		{
			"<color=#",
			color.ToHexString(),
			">",
			text,
			"</color>"
		});
	}

	// Token: 0x060058C7 RID: 22727 RVA: 0x002083C9 File Offset: 0x002065C9
	public static Sprite GetNoneClothingItemIcon(PermitCategory category, Option<Personality> personality)
	{
		return KleiItemsUI.GetNoneIconForCategory(category, personality);
	}

	// Token: 0x060058C8 RID: 22728 RVA: 0x002083D2 File Offset: 0x002065D2
	public static Sprite GetNoneBalloonArtistIcon()
	{
		return KleiItemsUI.GetNoneIconForCategory(PermitCategory.JoyResponse, null);
	}

	// Token: 0x060058C9 RID: 22729 RVA: 0x002083E1 File Offset: 0x002065E1
	private static Sprite GetNoneIconForCategory(PermitCategory category, Option<Personality> personality)
	{
		return Assets.GetSprite(KleiItemsUI.<GetNoneIconForCategory>g__GetIconName|5_0(category, personality));
	}

	// Token: 0x060058CA RID: 22730 RVA: 0x002083F4 File Offset: 0x002065F4
	public static string GetNoneOutfitName(ClothingOutfitUtility.OutfitType outfitType)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return UI.OUTFIT_NAME.NONE;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			return UI.OUTFIT_NAME.NONE_JOY_RESPONSE;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			return UI.OUTFIT_NAME.NONE_ATMO_SUIT;
		default:
			DebugUtil.DevAssert(false, string.Format("Couldn't find \"no item\" string for outfit {0}", outfitType), null);
			return "-";
		}
	}

	// Token: 0x060058CB RID: 22731 RVA: 0x00208454 File Offset: 0x00206654
	[return: TupleElementNames(new string[]
	{
		"name",
		"desc"
	})]
	public static ValueTuple<string, string> GetNoneClothingItemStrings(PermitCategory category)
	{
		switch (category)
		{
		case PermitCategory.DupeTops:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.CLOTHING_TOPS.NAME, EQUIPMENT.PREFABS.CLOTHING_TOPS.DESC);
		case PermitCategory.DupeBottoms:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.NAME, EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.DESC);
		case PermitCategory.DupeGloves:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.CLOTHING_GLOVES.NAME, EQUIPMENT.PREFABS.CLOTHING_GLOVES.DESC);
		case PermitCategory.DupeShoes:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.CLOTHING_SHOES.NAME, EQUIPMENT.PREFABS.CLOTHING_SHOES.DESC);
		case PermitCategory.DupeHats:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.CLOTHING_HATS.NAME, EQUIPMENT.PREFABS.CLOTHING_HATS.DESC);
		case PermitCategory.DupeAccessories:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.CLOTHING_ACCESORIES.NAME, EQUIPMENT.PREFABS.CLOTHING_ACCESORIES.DESC);
		case PermitCategory.AtmoSuitHelmet:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.DESC);
		case PermitCategory.AtmoSuitBody:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.ATMO_SUIT_BODY.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BODY.DESC);
		case PermitCategory.AtmoSuitGloves:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.DESC);
		case PermitCategory.AtmoSuitBelt:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.ATMO_SUIT_BELT.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_BELT.DESC);
		case PermitCategory.AtmoSuitShoes:
			return new ValueTuple<string, string>(EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.NAME, EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.DESC);
		case PermitCategory.JoyResponse:
			return new ValueTuple<string, string>(UI.OUTFIT_DESCRIPTION.NO_JOY_RESPONSE_NAME, UI.OUTFIT_DESCRIPTION.NO_JOY_RESPONSE_DESC);
		}
		DebugUtil.DevAssert(false, string.Format("Couldn't find \"no item\" string for category {0}", category), null);
		return new ValueTuple<string, string>("-", "-");
	}

	// Token: 0x060058CC RID: 22732 RVA: 0x00208610 File Offset: 0x00206810
	public static void ConfigureTooltipOn(GameObject gameObject, Option<LocString> tooltipText = default(Option<LocString>))
	{
		KleiItemsUI.ConfigureTooltipOn(gameObject, tooltipText.HasValue ? Option.Some<string>(tooltipText.Value) : Option.None);
	}

	// Token: 0x060058CD RID: 22733 RVA: 0x00208640 File Offset: 0x00206840
	public static void ConfigureTooltipOn(GameObject gameObject, Option<string> tooltipText = default(Option<string>))
	{
		ToolTip toolTip = gameObject.GetComponent<ToolTip>();
		if (toolTip.IsNullOrDestroyed())
		{
			toolTip = gameObject.AddComponent<ToolTip>();
			toolTip.tooltipPivot = new Vector2(0.5f, 1f);
			if (gameObject.GetComponent<KButton>())
			{
				toolTip.tooltipPositionOffset = new Vector2(0f, 22f);
			}
			else
			{
				toolTip.tooltipPositionOffset = new Vector2(0f, 0f);
			}
			toolTip.parentPositionAnchor = new Vector2(0.5f, 0f);
			toolTip.toolTipPosition = ToolTip.TooltipPosition.Custom;
		}
		if (!tooltipText.HasValue)
		{
			toolTip.ClearMultiStringTooltip();
			return;
		}
		toolTip.SetSimpleTooltip(tooltipText.Value);
	}

	// Token: 0x060058CE RID: 22734 RVA: 0x002086EC File Offset: 0x002068EC
	public static string GetTooltipStringFor(PermitResource permit)
	{
		string text = KleiItemsUI.WrapAsToolTipTitle(permit.Name);
		if (!string.IsNullOrWhiteSpace(permit.Description))
		{
			text = text + "\n" + permit.Description;
		}
		string text2 = UI.KLEI_INVENTORY_SCREEN.ITEM_RARITY_DETAILS.Replace("{RarityName}", permit.Rarity.GetLocStringName());
		if (!string.IsNullOrWhiteSpace(text2))
		{
			text = text + "\n\n" + text2;
		}
		if (permit.IsOwnable() && PermitItems.GetOwnedCount(permit) <= 0)
		{
			text = text + "\n\n" + KleiItemsUI.WrapWithColor(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED);
		}
		return text;
	}

	// Token: 0x060058CF RID: 22735 RVA: 0x00208788 File Offset: 0x00206988
	public static string GetNoneTooltipStringFor(PermitCategory category)
	{
		ValueTuple<string, string> noneClothingItemStrings = KleiItemsUI.GetNoneClothingItemStrings(category);
		string item = noneClothingItemStrings.Item1;
		string item2 = noneClothingItemStrings.Item2;
		return KleiItemsUI.WrapAsToolTipTitle(item) + "\n" + item2;
	}

	// Token: 0x060058D0 RID: 22736 RVA: 0x002087B9 File Offset: 0x002069B9
	public static Color GetColor(string input)
	{
		if (input[0] == '#')
		{
			return Util.ColorFromHex(input.Substring(1));
		}
		return Util.ColorFromHex(input);
	}

	// Token: 0x060058D2 RID: 22738 RVA: 0x002087EC File Offset: 0x002069EC
	[CompilerGenerated]
	internal static string <GetNoneIconForCategory>g__GetIconName|5_0(PermitCategory category, Option<Personality> personality)
	{
		switch (category)
		{
		case PermitCategory.DupeTops:
			return KleiItemsUI.<GetNoneIconForCategory>g__GetTopDefaultIconNameForPersonality|5_1(personality);
		case PermitCategory.DupeBottoms:
			return KleiItemsUI.<GetNoneIconForCategory>g__GetBottomDefaultIconNameForPersonality|5_3(personality);
		case PermitCategory.DupeGloves:
			return KleiItemsUI.<GetNoneIconForCategory>g__GetGlovesDefaultIconNameForPersonality|5_2(personality);
		case PermitCategory.DupeShoes:
			return KleiItemsUI.<GetNoneIconForCategory>g__GetShoesDefaultIconNameForPersonality|5_4(personality);
		case PermitCategory.DupeHats:
			return "icon_inventory_hats";
		case PermitCategory.DupeAccessories:
			return "icon_inventory_accessories";
		case PermitCategory.AtmoSuitHelmet:
			return "icon_inventory_atmosuit_helmet";
		case PermitCategory.AtmoSuitBody:
			return "icon_inventory_atmosuit_body";
		case PermitCategory.AtmoSuitGloves:
			return "icon_inventory_atmosuit_gloves";
		case PermitCategory.AtmoSuitBelt:
			return "icon_inventory_atmosuit_belt";
		case PermitCategory.AtmoSuitShoes:
			return "icon_inventory_atmosuit_boots";
		case PermitCategory.Building:
			return "icon_inventory_buildings";
		case PermitCategory.Critter:
			return "icon_inventory_critters";
		case PermitCategory.Sweepy:
			return "icon_inventory_sweepys";
		case PermitCategory.Duplicant:
			return "icon_inventory_duplicants";
		case PermitCategory.Artwork:
			return "icon_inventory_artworks";
		case PermitCategory.JoyResponse:
			return "icon_inventory_joyresponses";
		default:
			return "NoTraits";
		}
	}

	// Token: 0x060058D3 RID: 22739 RVA: 0x002088B8 File Offset: 0x00206AB8
	[CompilerGenerated]
	internal static string <GetNoneIconForCategory>g__GetTopDefaultIconNameForPersonality|5_1(Option<Personality> personality)
	{
		if (personality.IsNone())
		{
			return "default_clothing_top_02";
		}
		string id = personality.Unwrap().Id;
		if (id != null)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(id);
			if (num <= 2055167374U)
			{
				if (num <= 928627962U)
				{
					if (num <= 436604749U)
					{
						if (num <= 106010801U)
						{
							if (num != 95707542U)
							{
								if (num != 106010801U)
								{
									goto IL_5EE;
								}
								if (!(id == "STINKY"))
								{
									goto IL_5EE;
								}
								goto IL_5E2;
							}
							else if (!(id == "BURT"))
							{
								goto IL_5EE;
							}
						}
						else if (num != 129004589U)
						{
							if (num != 371348305U)
							{
								if (num != 436604749U)
								{
									goto IL_5EE;
								}
								if (!(id == "RUBY"))
								{
									goto IL_5EE;
								}
							}
							else
							{
								if (!(id == "MIMA"))
								{
									goto IL_5EE;
								}
								goto IL_5DC;
							}
						}
						else
						{
							if (!(id == "TURNER"))
							{
								goto IL_5EE;
							}
							goto IL_5DC;
						}
					}
					else if (num <= 538377295U)
					{
						if (num != 475632249U)
						{
							if (num != 538377295U)
							{
								goto IL_5EE;
							}
							if (!(id == "LINDSAY"))
							{
								goto IL_5EE;
							}
							goto IL_5E2;
						}
						else
						{
							if (!(id == "MAX"))
							{
								goto IL_5EE;
							}
							goto IL_5DC;
						}
					}
					else if (num != 752457119U)
					{
						if (num != 915325154U)
						{
							if (num != 928627962U)
							{
								goto IL_5EE;
							}
							if (!(id == "MAE"))
							{
								goto IL_5EE;
							}
						}
						else
						{
							if (!(id == "MEEP"))
							{
								goto IL_5EE;
							}
							goto IL_5E2;
						}
					}
					else
					{
						if (!(id == "AMARI"))
						{
							goto IL_5EE;
						}
						goto IL_5E2;
					}
				}
				else if (num <= 1530553059U)
				{
					if (num <= 1261463929U)
					{
						if (num != 1108123711U)
						{
							if (num != 1261463929U)
							{
								goto IL_5EE;
							}
							if (!(id == "MARIE"))
							{
								goto IL_5EE;
							}
							goto IL_5E2;
						}
						else
						{
							if (!(id == "JEAN"))
							{
								goto IL_5EE;
							}
							goto IL_5E8;
						}
					}
					else if (num != 1440338656U)
					{
						if (num != 1476265495U)
						{
							if (num != 1530553059U)
							{
								goto IL_5EE;
							}
							if (!(id == "OTTO"))
							{
								goto IL_5EE;
							}
							goto IL_5E8;
						}
						else
						{
							if (!(id == "GOSSMANN"))
							{
								goto IL_5EE;
							}
							goto IL_5E2;
						}
					}
					else if (!(id == "NISBET"))
					{
						goto IL_5EE;
					}
				}
				else if (num <= 1652832477U)
				{
					if (num != 1569689369U)
					{
						if (num != 1652832477U)
						{
							goto IL_5EE;
						}
						if (!(id == "ABE"))
						{
							goto IL_5EE;
						}
						goto IL_5DC;
					}
					else
					{
						if (!(id == "BANHI"))
						{
							goto IL_5EE;
						}
						goto IL_5E8;
					}
				}
				else if (num != 1707726780U)
				{
					if (num != 2054509595U)
					{
						if (num != 2055167374U)
						{
							goto IL_5EE;
						}
						if (!(id == "CAMILLE"))
						{
							goto IL_5EE;
						}
						goto IL_5DC;
					}
					else if (!(id == "ADA"))
					{
						goto IL_5EE;
					}
				}
				else
				{
					if (!(id == "TRAVALDO"))
					{
						goto IL_5EE;
					}
					goto IL_5E2;
				}
			}
			else if (num <= 3154394837U)
			{
				if (num <= 2594838786U)
				{
					if (num <= 2193283315U)
					{
						if (num != 2185863441U)
						{
							if (num != 2193283315U)
							{
								goto IL_5EE;
							}
							if (!(id == "JOSHUA"))
							{
								goto IL_5EE;
							}
							goto IL_5DC;
						}
						else
						{
							if (!(id == "NIKOLA"))
							{
								goto IL_5EE;
							}
							goto IL_5E2;
						}
					}
					else if (num != 2379130810U)
					{
						if (num != 2446394380U)
						{
							if (num != 2594838786U)
							{
								goto IL_5EE;
							}
							if (!(id == "QUINN"))
							{
								goto IL_5EE;
							}
						}
						else if (!(id == "CATALINA"))
						{
							goto IL_5EE;
						}
					}
					else if (!(id == "REN"))
					{
						goto IL_5EE;
					}
				}
				else if (num <= 2852960560U)
				{
					if (num != 2787862260U)
					{
						if (num != 2852960560U)
						{
							goto IL_5EE;
						}
						if (!(id == "LEIRA"))
						{
							goto IL_5EE;
						}
						goto IL_5E2;
					}
					else
					{
						if (!(id == "BUBBLES"))
						{
							goto IL_5EE;
						}
						goto IL_5DC;
					}
				}
				else if (num != 2936110140U)
				{
					if (num != 3064712329U)
					{
						if (num != 3154394837U)
						{
							goto IL_5EE;
						}
						if (!(id == "HAROLD"))
						{
							goto IL_5EE;
						}
						goto IL_5E8;
					}
					else
					{
						if (!(id == "ARI"))
						{
							goto IL_5EE;
						}
						goto IL_5E2;
					}
				}
				else
				{
					if (!(id == "JORGE"))
					{
						goto IL_5EE;
					}
					return "default_clothing_top_jorge";
				}
			}
			else if (num <= 3856357889U)
			{
				if (num <= 3228940292U)
				{
					if (num != 3171906610U)
					{
						if (num != 3228940292U)
						{
							goto IL_5EE;
						}
						if (!(id == "STEVE"))
						{
							goto IL_5EE;
						}
						goto IL_5DC;
					}
					else
					{
						if (!(id == "NAILS"))
						{
							goto IL_5EE;
						}
						goto IL_5E8;
					}
				}
				else if (num != 3270619858U)
				{
					if (num != 3809262181U)
					{
						if (num != 3856357889U)
						{
							goto IL_5EE;
						}
						if (!(id == "FRANKIE"))
						{
							goto IL_5EE;
						}
						goto IL_5DC;
					}
					else if (!(id == "DEVON"))
					{
						goto IL_5EE;
					}
				}
				else
				{
					if (!(id == "ELLIE"))
					{
						goto IL_5EE;
					}
					goto IL_5DC;
				}
			}
			else if (num <= 4019714155U)
			{
				if (num != 3979082712U)
				{
					if (num != 4019714155U)
					{
						goto IL_5EE;
					}
					if (!(id == "ASHKAN"))
					{
						goto IL_5EE;
					}
					goto IL_5E8;
				}
				else
				{
					if (!(id == "LIAM"))
					{
						goto IL_5EE;
					}
					goto IL_5E8;
				}
			}
			else if (num != 4104112407U)
			{
				if (num != 4129415200U)
				{
					if (num != 4211154965U)
					{
						goto IL_5EE;
					}
					if (!(id == "PEI"))
					{
						goto IL_5EE;
					}
					goto IL_5E8;
				}
				else
				{
					if (!(id == "ROWAN"))
					{
						goto IL_5EE;
					}
					goto IL_5E8;
				}
			}
			else
			{
				if (!(id == "HASSAN"))
				{
					goto IL_5EE;
				}
				goto IL_5E8;
			}
			return "default_clothing_top_01";
			IL_5DC:
			return "default_clothing_top_02";
			IL_5E2:
			return "default_clothing_top_03";
			IL_5E8:
			return "default_clothing_top_04";
		}
		IL_5EE:
		return "NoTraits";
	}

	// Token: 0x060058D4 RID: 22740 RVA: 0x00208EB8 File Offset: 0x002070B8
	[CompilerGenerated]
	internal static string <GetNoneIconForCategory>g__GetGlovesDefaultIconNameForPersonality|5_2(Option<Personality> personality)
	{
		if (personality.IsSome() && personality.Unwrap().Id == "JORGE")
		{
			return "default_clothing_gloves_jorge";
		}
		return "default_clothing_gloves";
	}

	// Token: 0x060058D5 RID: 22741 RVA: 0x00208EE6 File Offset: 0x002070E6
	[CompilerGenerated]
	internal static string <GetNoneIconForCategory>g__GetBottomDefaultIconNameForPersonality|5_3(Option<Personality> personality)
	{
		if (personality.IsSome() && personality.Unwrap().Id == "JORGE")
		{
			return "default_clothing_pants_jorge";
		}
		return "default_clothing_pants";
	}

	// Token: 0x060058D6 RID: 22742 RVA: 0x00208F14 File Offset: 0x00207114
	[CompilerGenerated]
	internal static string <GetNoneIconForCategory>g__GetShoesDefaultIconNameForPersonality|5_4(Option<Personality> personality)
	{
		if (personality.IsSome() && personality.Unwrap().Id == "JORGE")
		{
			return "default_clothing_shoes_jorge";
		}
		return "default_clothing_shoes";
	}

	// Token: 0x04003C12 RID: 15378
	public static readonly Color TEXT_COLOR__PERMIT_NOT_OWNED = KleiItemsUI.GetColor("#DD992F");
}
