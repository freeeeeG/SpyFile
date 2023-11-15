﻿using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei;
using Klei.AI;
using ProcGen;
using ProcGenGame;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000ABE RID: 2750
public static class CodexEntryGenerator
{
	// Token: 0x0600547E RID: 21630 RVA: 0x001E6D6C File Offset: 0x001E4F6C
	public static Dictionary<string, CodexEntry> GenerateBuildingEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (PlanScreen.PlanInfo category in TUNING.BUILDINGS.PLANORDER)
		{
			CodexEntryGenerator.GenerateEntriesForBuildingsInCategory(category, CodexEntryGenerator.categoryPrefx, ref dictionary);
		}
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			CodexEntryGenerator.GenerateDLC1RocketryEntries();
		}
		CodexEntryGenerator.PopulateCategoryEntries(dictionary);
		return dictionary;
	}

	// Token: 0x0600547F RID: 21631 RVA: 0x001E6DDC File Offset: 0x001E4FDC
	private static void GenerateEntriesForBuildingsInCategory(PlanScreen.PlanInfo category, string categoryPrefx, ref Dictionary<string, CodexEntry> categoryEntries)
	{
		string text = HashCache.Get().Get(category.category);
		string text2 = CodexCache.FormatLinkID(categoryPrefx + text);
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (KeyValuePair<string, string> keyValuePair in category.buildingAndSubcategoryData)
		{
			CodexEntry codexEntry = CodexEntryGenerator.GenerateSingleBuildingEntry(Assets.GetBuildingDef(keyValuePair.Key), text2);
			if (codexEntry != null)
			{
				dictionary.Add(codexEntry.id, codexEntry);
			}
		}
		if (dictionary.Count == 0)
		{
			return;
		}
		CategoryEntry categoryEntry = CodexEntryGenerator.GenerateCategoryEntry(CodexCache.FormatLinkID(text2), Strings.Get("STRINGS.UI.BUILDCATEGORIES." + text.ToUpper() + ".NAME"), dictionary, null, true, true, null);
		categoryEntry.parentId = "BUILDINGS";
		categoryEntry.category = "BUILDINGS";
		categoryEntry.icon = Assets.GetSprite(PlanScreen.IconNameMap[text]);
		categoryEntries.Add(text2, categoryEntry);
	}

	// Token: 0x06005480 RID: 21632 RVA: 0x001E6EF0 File Offset: 0x001E50F0
	private static CodexEntry GenerateSingleBuildingEntry(BuildingDef def, string categoryEntryID)
	{
		if (def.DebugOnly || def.Deprecated)
		{
			return null;
		}
		List<ContentContainer> list = new List<ContentContainer>();
		List<ICodexWidget> list2 = new List<ICodexWidget>();
		list2.Add(new CodexText(def.Name, CodexTextStyle.Title, null));
		Tech tech = Db.Get().Techs.TryGetTechForTechItem(def.PrefabID);
		if (tech != null)
		{
			list2.Add(new CodexLabelWithIcon(tech.Name, CodexTextStyle.Body, new global::Tuple<Sprite, Color>(Assets.GetSprite("research_type_alpha_icon"), Color.white)));
		}
		list2.Add(new CodexDividerLine());
		list.Add(new ContentContainer(list2, ContentContainer.ContentLayout.Vertical));
		CodexEntryGenerator.GenerateImageContainers(def.GetUISprite("ui", false), list);
		CodexEntryGenerator.GenerateBuildingDescriptionContainers(def, list);
		CodexEntryGenerator.GenerateFabricatorContainers(def.BuildingComplete, list);
		CodexEntryGenerator.GenerateReceptacleContainers(def.BuildingComplete, list);
		CodexEntryGenerator.GenerateConfigurableConsumerContainers(def.BuildingComplete, list);
		CodexEntry codexEntry = new CodexEntry(categoryEntryID, list, Strings.Get("STRINGS.BUILDINGS.PREFABS." + def.PrefabID.ToUpper() + ".NAME"));
		codexEntry.icon = def.GetUISprite("ui", false);
		codexEntry.parentId = categoryEntryID;
		CodexCache.AddEntry(def.PrefabID, codexEntry, null);
		return codexEntry;
	}

	// Token: 0x06005481 RID: 21633 RVA: 0x001E701C File Offset: 0x001E521C
	private static void GenerateDLC1RocketryEntries()
	{
		PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER.Find((PlanScreen.PlanInfo match) => match.category == new HashedString("Rocketry"));
		foreach (string prefab_id in SelectModuleSideScreen.moduleButtonSortOrder)
		{
			string str = HashCache.Get().Get(planInfo.category);
			string categoryEntryID = CodexCache.FormatLinkID(CodexEntryGenerator.categoryPrefx + str);
			BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
			CodexEntry codexEntry = CodexEntryGenerator.GenerateSingleBuildingEntry(buildingDef, categoryEntryID);
			List<ICodexWidget> list = new List<ICodexWidget>();
			list.Add(new CodexSpacer());
			list.Add(new CodexText(UI.CLUSTERMAP.ROCKETS.MODULE_STATS.NAME_HEADER, CodexTextStyle.Subtitle, null));
			list.Add(new CodexSpacer());
			list.Add(new CodexText(UI.CLUSTERMAP.ROCKETS.SPEED.TOOLTIP, CodexTextStyle.Body, null));
			RocketModuleCluster component = buildingDef.BuildingComplete.GetComponent<RocketModuleCluster>();
			float burden = component.performanceStats.Burden;
			float enginePower = component.performanceStats.EnginePower;
			RocketEngineCluster component2 = buildingDef.BuildingComplete.GetComponent<RocketEngineCluster>();
			if (component2 != null)
			{
				list.Add(new CodexText("    • " + UI.CLUSTERMAP.ROCKETS.MAX_HEIGHT.NAME_MAX_SUPPORTED + component2.maxHeight.ToString(), CodexTextStyle.Body, null));
			}
			list.Add(new CodexText("    • " + UI.CLUSTERMAP.ROCKETS.MAX_HEIGHT.NAME_RAW + buildingDef.HeightInCells.ToString(), CodexTextStyle.Body, null));
			if (burden != 0f)
			{
				list.Add(new CodexText("    • " + UI.CLUSTERMAP.ROCKETS.BURDEN_MODULE.NAME + burden.ToString(), CodexTextStyle.Body, null));
			}
			if (enginePower != 0f)
			{
				list.Add(new CodexText("    • " + UI.CLUSTERMAP.ROCKETS.POWER_MODULE.NAME + enginePower.ToString(), CodexTextStyle.Body, null));
			}
			ContentContainer container = new ContentContainer(list, ContentContainer.ContentLayout.Vertical);
			codexEntry.AddContentContainer(container);
		}
	}

	// Token: 0x06005482 RID: 21634 RVA: 0x001E7230 File Offset: 0x001E5430
	public static void GeneratePageNotFound()
	{
		CodexCache.AddEntry("PageNotFound", new CodexEntry("ROOT", new List<ContentContainer>
		{
			new ContentContainer
			{
				content = 
				{
					new CodexText(CODEX.PAGENOTFOUND.TITLE, CodexTextStyle.Title, null),
					new CodexText(CODEX.PAGENOTFOUND.SUBTITLE, CodexTextStyle.Subtitle, null),
					new CodexDividerLine(),
					new CodexImage(312, 312, Assets.GetSprite("outhouseMessage"))
				}
			}
		}, CODEX.PAGENOTFOUND.TITLE)
		{
			searchOnly = true
		}, null);
	}

	// Token: 0x06005483 RID: 21635 RVA: 0x001E72EC File Offset: 0x001E54EC
	public static Dictionary<string, CodexEntry> GenerateRoomsEntries()
	{
		Dictionary<string, CodexEntry> result = new Dictionary<string, CodexEntry>();
		RoomTypes roomTypesData = Db.Get().RoomTypes;
		string parentCategoryName = "ROOMS";
		Action<RoomTypeCategory> action = delegate(RoomTypeCategory roomCategory)
		{
			bool flag = false;
			List<ContentContainer> contentContainers = new List<ContentContainer>();
			CodexEntry codexEntry = new CodexEntry(parentCategoryName, contentContainers, roomCategory.Name);
			for (int i = 0; i < roomTypesData.Count; i++)
			{
				RoomType roomType = roomTypesData[i];
				if (roomType.category.Id == roomCategory.Id)
				{
					if (!flag)
					{
						flag = true;
						codexEntry.parentId = parentCategoryName;
						codexEntry.name = roomCategory.Name;
						CodexCache.AddEntry(parentCategoryName + roomCategory.Id, codexEntry, null);
						result.Add(parentCategoryName + roomType.category.Id, codexEntry);
						ContentContainer container = new ContentContainer(new List<ICodexWidget>
						{
							new CodexImage(312, 312, Assets.GetSprite(roomCategory.icon))
						}, ContentContainer.ContentLayout.Vertical);
						codexEntry.AddContentContainer(container);
					}
					List<ContentContainer> list = new List<ContentContainer>();
					CodexEntryGenerator.GenerateTitleContainers(roomType.Name, list);
					CodexEntryGenerator.GenerateRoomTypeDescriptionContainers(roomType, list);
					CodexEntryGenerator.GenerateRoomTypeDetailsContainers(roomType, list);
					SubEntry subEntry = new SubEntry(roomType.Id, parentCategoryName + roomType.category.Id, list, roomType.Name);
					subEntry.icon = Assets.GetSprite(roomCategory.icon);
					subEntry.iconColor = Color.white;
					codexEntry.subEntries.Add(subEntry);
				}
			}
		};
		action(Db.Get().RoomTypeCategories.Agricultural);
		action(Db.Get().RoomTypeCategories.Bathroom);
		action(Db.Get().RoomTypeCategories.Food);
		action(Db.Get().RoomTypeCategories.Hospital);
		action(Db.Get().RoomTypeCategories.Industrial);
		action(Db.Get().RoomTypeCategories.Park);
		action(Db.Get().RoomTypeCategories.Recreation);
		action(Db.Get().RoomTypeCategories.Sleep);
		action(Db.Get().RoomTypeCategories.Science);
		return result;
	}

	// Token: 0x06005484 RID: 21636 RVA: 0x001E73F4 File Offset: 0x001E55F4
	public static Dictionary<string, CodexEntry> GeneratePlantEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Harvestable>();
		prefabsWithComponent.AddRange(Assets.GetPrefabsWithComponent<WiltCondition>());
		foreach (GameObject gameObject in prefabsWithComponent)
		{
			if (!dictionary.ContainsKey(gameObject.PrefabID().ToString()) && !(gameObject.GetComponent<BudUprootedMonitor>() != null))
			{
				List<ContentContainer> list = new List<ContentContainer>();
				Sprite first = Def.GetUISprite(gameObject, "ui", false).first;
				CodexEntryGenerator.GenerateImageContainers(first, list);
				CodexEntryGenerator.GeneratePlantDescriptionContainers(gameObject, list);
				CodexEntry codexEntry = new CodexEntry("PLANTS", list, gameObject.GetProperName());
				codexEntry.parentId = "PLANTS";
				codexEntry.icon = first;
				CodexCache.AddEntry(gameObject.PrefabID().ToString(), codexEntry, null);
				dictionary.Add(gameObject.PrefabID().ToString(), codexEntry);
			}
		}
		return dictionary;
	}

	// Token: 0x06005485 RID: 21637 RVA: 0x001E7514 File Offset: 0x001E5714
	public static Dictionary<string, CodexEntry> GenerateFoodEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			GameObject prefab = Assets.GetPrefab(foodInfo.Id);
			if (!prefab.HasTag(GameTags.DeprecatedContent) && !prefab.HasTag(GameTags.IncubatableEgg))
			{
				List<ContentContainer> list = new List<ContentContainer>();
				CodexEntryGenerator.GenerateTitleContainers(foodInfo.Name, list);
				Sprite first = Def.GetUISprite(foodInfo.ConsumableId, "ui", false).first;
				CodexEntryGenerator.GenerateImageContainers(first, list);
				CodexEntryGenerator.GenerateFoodDescriptionContainers(foodInfo, list);
				CodexEntryGenerator.GenerateRecipeContainers(foodInfo.ConsumableId.ToTag(), list);
				CodexEntryGenerator.GenerateUsedInRecipeContainers(foodInfo.ConsumableId.ToTag(), list);
				CodexEntry codexEntry = new CodexEntry(CodexEntryGenerator.FOOD_CATEGORY_ID, list, foodInfo.Name);
				codexEntry.icon = first;
				codexEntry.parentId = CodexEntryGenerator.FOOD_CATEGORY_ID;
				CodexCache.AddEntry(foodInfo.Id, codexEntry, null);
				dictionary.Add(foodInfo.Id, codexEntry);
			}
		}
		CodexEntry codexEntry2 = CodexEntryGenerator.GenerateFoodEffectEntry();
		CodexCache.AddEntry(CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID, codexEntry2, null);
		dictionary.Add(CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID, codexEntry2);
		CodexEntry codexEntry3 = CodexEntryGenerator.GenerateTabelSaltEntry();
		CodexCache.AddEntry(CodexEntryGenerator.TABLE_SALT_ENTRY_ID, codexEntry3, null);
		dictionary.Add(CodexEntryGenerator.TABLE_SALT_ENTRY_ID, codexEntry3);
		return dictionary;
	}

	// Token: 0x06005486 RID: 21638 RVA: 0x001E768C File Offset: 0x001E588C
	private static CodexEntry GenerateFoodEffectEntry()
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		Dictionary<string, List<EdiblesManager.FoodInfo>> dictionary = new Dictionary<string, List<EdiblesManager.FoodInfo>>();
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			foreach (string key in foodInfo.Effects)
			{
				List<EdiblesManager.FoodInfo> list2;
				if (!dictionary.TryGetValue(key, out list2))
				{
					list2 = new List<EdiblesManager.FoodInfo>();
					dictionary[key] = list2;
				}
				list2.Add(foodInfo);
			}
		}
		foreach (KeyValuePair<string, List<EdiblesManager.FoodInfo>> self in dictionary)
		{
			string text;
			List<EdiblesManager.FoodInfo> list3;
			self.Deconstruct(out text, out list3);
			string text2 = text;
			List<EdiblesManager.FoodInfo> list4 = list3;
			Klei.AI.Modifier modifier = Db.Get().effects.Get(text2);
			string text3 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text2.ToUpper() + ".NAME");
			string text4 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text2.ToUpper() + ".DESCRIPTION");
			list.Add(new CodexText(text3, CodexTextStyle.Title, null));
			list.Add(new CodexText(text4, CodexTextStyle.Body, null));
			foreach (AttributeModifier attributeModifier in modifier.SelfModifiers)
			{
				string str = Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME");
				string tooltip = Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".DESC");
				list.Add(new CodexTextWithTooltip("    • " + str + ": " + attributeModifier.GetFormattedString(), tooltip, CodexTextStyle.Body));
			}
			list.Add(new CodexText(CODEX.HEADERS.FOODSWITHEFFECT + ": ", CodexTextStyle.Body, null));
			foreach (EdiblesManager.FoodInfo foodInfo2 in list4)
			{
				list.Add(new CodexTextWithTooltip("    • " + foodInfo2.Name, foodInfo2.Description, CodexTextStyle.Body));
			}
			list.Add(new CodexSpacer());
		}
		return new CodexEntry(CodexEntryGenerator.FOOD_CATEGORY_ID, new List<ContentContainer>
		{
			new ContentContainer(list, ContentContainer.ContentLayout.Vertical)
		}, CODEX.HEADERS.FOODEFFECTS)
		{
			parentId = CodexEntryGenerator.FOOD_CATEGORY_ID,
			icon = Assets.GetSprite("icon_category_food")
		};
	}

	// Token: 0x06005487 RID: 21639 RVA: 0x001E79C4 File Offset: 0x001E5BC4
	private static CodexEntry GenerateTabelSaltEntry()
	{
		LocString name = ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME;
		LocString desc = ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.DESC;
		Sprite sprite = Assets.GetSprite("ui_food_table_salt");
		List<ContentContainer> list = new List<ContentContainer>();
		CodexEntryGenerator.GenerateImageContainers(sprite, list);
		list.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(name, CodexTextStyle.Title, null),
			new CodexText(desc, CodexTextStyle.Body, null)
		}, ContentContainer.ContentLayout.Vertical));
		return new CodexEntry(CodexEntryGenerator.FOOD_CATEGORY_ID, list, name)
		{
			parentId = CodexEntryGenerator.FOOD_CATEGORY_ID,
			icon = sprite
		};
	}

	// Token: 0x06005488 RID: 21640 RVA: 0x001E7A54 File Offset: 0x001E5C54
	public static Dictionary<string, CodexEntry> GenerateMinionModifierEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (Effect effect in Db.Get().effects.resources)
		{
			if (effect.triggerFloatingText || !effect.showInUI)
			{
				string id = effect.Id;
				string text = "AVOID_COLLISIONS_" + id;
				StringEntry stringEntry;
				StringEntry stringEntry2;
				if (Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + id.ToUpper() + ".NAME", out stringEntry) && (Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + id.ToUpper() + ".DESCRIPTION", out stringEntry2) || Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + id.ToUpper() + ".TOOLTIP", out stringEntry2)))
				{
					string @string = stringEntry.String;
					string string2 = stringEntry2.String;
					List<ContentContainer> list = new List<ContentContainer>();
					ContentContainer contentContainer = new ContentContainer();
					List<ICodexWidget> content = contentContainer.content;
					content.Add(new CodexText(effect.Name, CodexTextStyle.Title, null));
					content.Add(new CodexText(effect.description, CodexTextStyle.Body, null));
					foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
					{
						string str = Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME");
						string tooltip = Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".DESC");
						content.Add(new CodexTextWithTooltip("    • " + str + ": " + attributeModifier.GetFormattedString(), tooltip, CodexTextStyle.Body));
					}
					content.Add(new CodexSpacer());
					list.Add(contentContainer);
					CodexEntry codexEntry = new CodexEntry(CodexEntryGenerator.MINION_MODIFIERS_CATEGORY_ID, list, effect.Name);
					codexEntry.icon = Assets.GetSprite(effect.customIcon);
					codexEntry.parentId = CodexEntryGenerator.MINION_MODIFIERS_CATEGORY_ID;
					CodexCache.AddEntry(text, codexEntry, null);
					dictionary.Add(text, codexEntry);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06005489 RID: 21641 RVA: 0x001E7CBC File Offset: 0x001E5EBC
	public static Dictionary<string, CodexEntry> GenerateTechEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (Tech tech in Db.Get().Techs.resources)
		{
			List<ContentContainer> list = new List<ContentContainer>();
			CodexEntryGenerator.GenerateTitleContainers(tech.Name, list);
			CodexEntryGenerator.GenerateTechDescriptionContainers(tech, list);
			CodexEntryGenerator.GeneratePrerequisiteTechContainers(tech, list);
			CodexEntryGenerator.GenerateUnlockContainers(tech, list);
			CodexEntry codexEntry = new CodexEntry("TECH", list, tech.Name);
			TechItem techItem = (tech.unlockedItems.Count != 0) ? tech.unlockedItems[0] : null;
			codexEntry.icon = ((techItem == null) ? null : techItem.getUISprite("ui", false));
			codexEntry.parentId = "TECH";
			CodexCache.AddEntry(tech.Id, codexEntry, null);
			dictionary.Add(tech.Id, codexEntry);
		}
		return dictionary;
	}

	// Token: 0x0600548A RID: 21642 RVA: 0x001E7DC0 File Offset: 0x001E5FC0
	public static Dictionary<string, CodexEntry> GenerateRoleEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (Skill skill in Db.Get().Skills.resources)
		{
			if (!skill.deprecated)
			{
				List<ContentContainer> list = new List<ContentContainer>();
				Sprite sprite = Assets.GetSprite(skill.hat);
				CodexEntryGenerator.GenerateTitleContainers(skill.Name, list);
				CodexEntryGenerator.GenerateImageContainers(sprite, list);
				CodexEntryGenerator.GenerateGenericDescriptionContainers(skill.description, list);
				CodexEntryGenerator.GenerateSkillRequirementsAndPerksContainers(skill, list);
				CodexEntryGenerator.GenerateRelatedSkillContainers(skill, list);
				CodexEntry codexEntry = new CodexEntry("ROLES", list, skill.Name);
				codexEntry.parentId = "ROLES";
				codexEntry.icon = sprite;
				CodexCache.AddEntry(skill.Id, codexEntry, null);
				dictionary.Add(skill.Id, codexEntry);
			}
		}
		return dictionary;
	}

	// Token: 0x0600548B RID: 21643 RVA: 0x001E7EBC File Offset: 0x001E60BC
	public static Dictionary<string, CodexEntry> GenerateGeyserEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Geyser>();
		if (prefabsWithComponent != null)
		{
			foreach (GameObject gameObject in prefabsWithComponent)
			{
				if (!gameObject.GetComponent<KPrefabID>().HasTag(GameTags.DeprecatedContent))
				{
					List<ContentContainer> list = new List<ContentContainer>();
					CodexEntryGenerator.GenerateTitleContainers(gameObject.GetProperName(), list);
					Sprite first = Def.GetUISprite(gameObject, "ui", false).first;
					CodexEntryGenerator.GenerateImageContainers(first, list);
					List<ICodexWidget> list2 = new List<ICodexWidget>();
					string text = gameObject.PrefabID().ToString().ToUpper();
					string text2 = "GENERICGEYSER_";
					if (text.StartsWith(text2))
					{
						text.Remove(0, text2.Length);
					}
					list2.Add(new CodexText(UI.CODEX.GEYSERS.DESC, CodexTextStyle.Body, null));
					ContentContainer item = new ContentContainer(list2, ContentContainer.ContentLayout.Vertical);
					list.Add(item);
					CodexEntry codexEntry = new CodexEntry("GEYSERS", list, gameObject.GetProperName());
					codexEntry.icon = first;
					codexEntry.parentId = "GEYSERS";
					codexEntry.id = gameObject.PrefabID().ToString();
					CodexCache.AddEntry(codexEntry.id, codexEntry, null);
					dictionary.Add(codexEntry.id, codexEntry);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x0600548C RID: 21644 RVA: 0x001E803C File Offset: 0x001E623C
	public static Dictionary<string, CodexEntry> GenerateEquipmentEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Equippable>();
		if (prefabsWithComponent != null)
		{
			foreach (GameObject gameObject in prefabsWithComponent)
			{
				bool flag = false;
				Equippable component = gameObject.GetComponent<Equippable>();
				if (component.def.AdditionalTags != null)
				{
					Tag[] additionalTags = component.def.AdditionalTags;
					for (int i = 0; i < additionalTags.Length; i++)
					{
						if (additionalTags[i] == GameTags.DeprecatedContent)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag && !component.hideInCodex)
				{
					List<ContentContainer> list = new List<ContentContainer>();
					CodexEntryGenerator.GenerateTitleContainers(gameObject.GetProperName(), list);
					Sprite first = Def.GetUISprite(gameObject, "ui", false).first;
					CodexEntryGenerator.GenerateImageContainers(first, list);
					List<ICodexWidget> list2 = new List<ICodexWidget>();
					string text = gameObject.PrefabID().ToString();
					list2.Add(new CodexText(Strings.Get("STRINGS.EQUIPMENT.PREFABS." + text.ToUpper() + ".DESC"), CodexTextStyle.Body, null));
					ContentContainer item = new ContentContainer(list2, ContentContainer.ContentLayout.Vertical);
					list.Add(item);
					CodexEntry codexEntry = new CodexEntry("EQUIPMENT", list, gameObject.GetProperName());
					codexEntry.icon = first;
					codexEntry.parentId = "EQUIPMENT";
					codexEntry.id = gameObject.PrefabID().ToString();
					CodexCache.AddEntry(codexEntry.id, codexEntry, null);
					dictionary.Add(codexEntry.id, codexEntry);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x0600548D RID: 21645 RVA: 0x001E81FC File Offset: 0x001E63FC
	public static Dictionary<string, CodexEntry> GenerateBiomeEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		ListPool<YamlIO.Error, WorldGen>.PooledList pooledList = ListPool<YamlIO.Error, WorldGen>.Allocate();
		Application.streamingAssetsPath + "/worldgen/worlds/";
		Application.streamingAssetsPath + "/worldgen/biomes/";
		Application.streamingAssetsPath + "/worldgen/subworlds/";
		WorldGen.LoadSettings(false);
		Dictionary<string, List<WeightedSubworldName>> dictionary2 = new Dictionary<string, List<WeightedSubworldName>>();
		foreach (KeyValuePair<string, ClusterLayout> keyValuePair in SettingsCache.clusterLayouts.clusterCache)
		{
			ClusterLayout value = keyValuePair.Value;
			string filePath = value.filePath;
			foreach (WorldPlacement worldPlacement in value.worldPlacements)
			{
				foreach (WeightedSubworldName weightedSubworldName in SettingsCache.worlds.GetWorldData(worldPlacement.world).subworldFiles)
				{
					string text = weightedSubworldName.name.Substring(weightedSubworldName.name.LastIndexOf("/"));
					string text2 = weightedSubworldName.name.Substring(0, weightedSubworldName.name.Length - text.Length);
					text2 = text2.Substring(text2.LastIndexOf("/") + 1);
					if (!(text2 == "subworlds"))
					{
						if (!dictionary2.ContainsKey(text2))
						{
							dictionary2.Add(text2, new List<WeightedSubworldName>
							{
								weightedSubworldName
							});
						}
						else
						{
							dictionary2[text2].Add(weightedSubworldName);
						}
					}
				}
			}
		}
		foreach (KeyValuePair<string, List<WeightedSubworldName>> keyValuePair2 in dictionary2)
		{
			string text3 = CodexCache.FormatLinkID(keyValuePair2.Key);
			global::Tuple<Sprite, Color> tuple = null;
			string text4 = Strings.Get("STRINGS.SUBWORLDS." + text3.ToUpper() + ".NAME");
			if (text4.Contains("MISSING"))
			{
				text4 = text3 + " (missing string key)";
			}
			List<ContentContainer> list = new List<ContentContainer>();
			CodexEntryGenerator.GenerateTitleContainers(text4, list);
			string text5 = "biomeIcon" + char.ToUpper(text3[0]).ToString() + text3.Substring(1).ToLower();
			Sprite sprite = Assets.GetSprite(text5);
			if (sprite != null)
			{
				tuple = new global::Tuple<Sprite, Color>(sprite, Color.white);
			}
			else
			{
				global::Debug.LogWarning("Missing codex biome icon: " + text5);
			}
			string text6 = Strings.Get("STRINGS.SUBWORLDS." + text3.ToUpper() + ".DESC");
			string text7 = Strings.Get("STRINGS.SUBWORLDS." + text3.ToUpper() + ".UTILITY");
			ContentContainer item = new ContentContainer(new List<ICodexWidget>
			{
				new CodexText(string.IsNullOrEmpty(text6) ? "Basic description of the biome." : text6, CodexTextStyle.Body, null),
				new CodexSpacer(),
				new CodexText(string.IsNullOrEmpty(text7) ? "Description of the biomes utility." : text7, CodexTextStyle.Body, null),
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical);
			list.Add(item);
			Dictionary<string, float> dictionary3 = new Dictionary<string, float>();
			ContentContainer item2 = new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(UI.CODEX.SUBWORLDS.ELEMENTS, CodexTextStyle.Subtitle, null),
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical);
			list.Add(item2);
			ContentContainer contentContainer = new ContentContainer();
			contentContainer.contentLayout = ContentContainer.ContentLayout.Vertical;
			contentContainer.content = new List<ICodexWidget>();
			list.Add(contentContainer);
			foreach (WeightedSubworldName weightedSubworldName2 in keyValuePair2.Value)
			{
				SubWorld subWorld = SettingsCache.subworlds[weightedSubworldName2.name];
				foreach (WeightedBiome weightedBiome in SettingsCache.subworlds[weightedSubworldName2.name].biomes)
				{
					foreach (ElementGradient elementGradient in SettingsCache.biomes.BiomeBackgroundElementBandConfigurations[weightedBiome.name])
					{
						if (dictionary3.ContainsKey(elementGradient.content))
						{
							dictionary3[elementGradient.content] = dictionary3[elementGradient.content] + elementGradient.bandSize;
						}
						else
						{
							if (ElementLoader.FindElementByName(elementGradient.content) == null)
							{
								global::Debug.LogError("Biome " + weightedBiome.name + " contains non-existent element " + elementGradient.content);
							}
							dictionary3.Add(elementGradient.content, elementGradient.bandSize);
						}
					}
				}
				foreach (Feature feature in subWorld.features)
				{
					foreach (KeyValuePair<string, ElementChoiceGroup<WeightedSimHash>> keyValuePair3 in SettingsCache.GetCachedFeature(feature.type).ElementChoiceGroups)
					{
						foreach (WeightedSimHash weightedSimHash in keyValuePair3.Value.choices)
						{
							if (dictionary3.ContainsKey(weightedSimHash.element))
							{
								dictionary3[weightedSimHash.element] = dictionary3[weightedSimHash.element] + 1f;
							}
							else
							{
								dictionary3.Add(weightedSimHash.element, 1f);
							}
						}
					}
				}
			}
			foreach (KeyValuePair<string, float> keyValuePair4 in dictionary3.OrderBy(delegate(KeyValuePair<string, float> pair)
			{
				KeyValuePair<string, float> keyValuePair5 = pair;
				return keyValuePair5.Value;
			}))
			{
				Element element = ElementLoader.FindElementByName(keyValuePair4.Key);
				if (tuple == null)
				{
					tuple = Def.GetUISprite(element.substance, "ui", false);
				}
				contentContainer.content.Add(new CodexIndentedLabelWithIcon(element.name, CodexTextStyle.Body, Def.GetUISprite(element.substance, "ui", false)));
			}
			List<Tag> list2 = new List<Tag>();
			ContentContainer item3 = new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(UI.CODEX.SUBWORLDS.PLANTS, CodexTextStyle.Subtitle, null),
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical);
			list.Add(item3);
			ContentContainer contentContainer2 = new ContentContainer();
			contentContainer2.contentLayout = ContentContainer.ContentLayout.Vertical;
			contentContainer2.content = new List<ICodexWidget>();
			list.Add(contentContainer2);
			foreach (WeightedSubworldName weightedSubworldName3 in keyValuePair2.Value)
			{
				foreach (WeightedBiome weightedBiome2 in SettingsCache.subworlds[weightedSubworldName3.name].biomes)
				{
					if (weightedBiome2.tags != null)
					{
						foreach (string s in weightedBiome2.tags)
						{
							if (!list2.Contains(s))
							{
								GameObject gameObject = Assets.TryGetPrefab(s);
								if (gameObject != null && (gameObject.GetComponent<Harvestable>() != null || gameObject.GetComponent<SeedProducer>() != null))
								{
									list2.Add(s);
									contentContainer2.content.Add(new CodexIndentedLabelWithIcon(gameObject.GetProperName(), CodexTextStyle.Body, Def.GetUISprite(gameObject, "ui", false)));
								}
							}
						}
					}
				}
				foreach (Feature feature2 in SettingsCache.subworlds[weightedSubworldName3.name].features)
				{
					foreach (MobReference mobReference in SettingsCache.GetCachedFeature(feature2.type).internalMobs)
					{
						Tag tag = mobReference.type.ToTag();
						if (!list2.Contains(tag))
						{
							GameObject gameObject2 = Assets.TryGetPrefab(tag);
							if (gameObject2 != null && (gameObject2.GetComponent<Harvestable>() != null || gameObject2.GetComponent<SeedProducer>() != null))
							{
								list2.Add(tag);
								contentContainer2.content.Add(new CodexIndentedLabelWithIcon(gameObject2.GetProperName(), CodexTextStyle.Body, Def.GetUISprite(gameObject2, "ui", false)));
							}
						}
					}
				}
			}
			if (list2.Count == 0)
			{
				contentContainer2.content.Add(new CodexIndentedLabelWithIcon(UI.CODEX.SUBWORLDS.NONE, CodexTextStyle.Body, new global::Tuple<Sprite, Color>(Assets.GetSprite("inspectorUI_cannot_build"), Color.red)));
			}
			List<Tag> list3 = new List<Tag>();
			ContentContainer item4 = new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(UI.CODEX.SUBWORLDS.CRITTERS, CodexTextStyle.Subtitle, null),
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical);
			list.Add(item4);
			ContentContainer contentContainer3 = new ContentContainer();
			contentContainer3.contentLayout = ContentContainer.ContentLayout.Vertical;
			contentContainer3.content = new List<ICodexWidget>();
			list.Add(contentContainer3);
			foreach (WeightedSubworldName weightedSubworldName4 in keyValuePair2.Value)
			{
				foreach (WeightedBiome weightedBiome3 in SettingsCache.subworlds[weightedSubworldName4.name].biomes)
				{
					if (weightedBiome3.tags != null)
					{
						foreach (string s2 in weightedBiome3.tags)
						{
							if (!list3.Contains(s2))
							{
								GameObject gameObject3 = Assets.TryGetPrefab(s2);
								if (gameObject3 != null && gameObject3.HasTag(GameTags.Creature))
								{
									list3.Add(s2);
									contentContainer3.content.Add(new CodexIndentedLabelWithIcon(gameObject3.GetProperName(), CodexTextStyle.Body, Def.GetUISprite(gameObject3, "ui", false)));
								}
							}
						}
					}
				}
				foreach (Feature feature3 in SettingsCache.subworlds[weightedSubworldName4.name].features)
				{
					foreach (MobReference mobReference2 in SettingsCache.GetCachedFeature(feature3.type).internalMobs)
					{
						Tag tag2 = mobReference2.type.ToTag();
						if (!list3.Contains(tag2))
						{
							GameObject gameObject4 = Assets.TryGetPrefab(tag2);
							if (gameObject4 != null && gameObject4.HasTag(GameTags.Creature))
							{
								list3.Add(tag2);
								contentContainer3.content.Add(new CodexIndentedLabelWithIcon(gameObject4.GetProperName(), CodexTextStyle.Body, Def.GetUISprite(gameObject4, "ui", false)));
							}
						}
					}
				}
			}
			if (list3.Count == 0)
			{
				contentContainer3.content.Add(new CodexIndentedLabelWithIcon(UI.CODEX.SUBWORLDS.NONE, CodexTextStyle.Body, new global::Tuple<Sprite, Color>(Assets.GetSprite("inspectorUI_cannot_build"), Color.red)));
			}
			string text8 = "BIOME" + text3;
			CodexEntry codexEntry = new CodexEntry("BIOMES", list, text8);
			codexEntry.name = text4;
			codexEntry.parentId = "BIOMES";
			codexEntry.icon = tuple.first;
			codexEntry.iconColor = tuple.second;
			CodexCache.AddEntry(text8, codexEntry, null);
			dictionary.Add(text8, codexEntry);
		}
		if (Application.isPlaying)
		{
			Global.Instance.modManager.HandleErrors(pooledList);
		}
		else
		{
			foreach (YamlIO.Error error in pooledList)
			{
				YamlIO.LogError(error, false);
			}
		}
		pooledList.Recycle();
		return dictionary;
	}

	// Token: 0x0600548E RID: 21646 RVA: 0x001E9108 File Offset: 0x001E7308
	public static Dictionary<string, CodexEntry> GenerateConstructionMaterialEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		Dictionary<Tag, List<BuildingDef>> dictionary2 = new Dictionary<Tag, List<BuildingDef>>();
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (!buildingDef.Deprecated)
			{
				foreach (string name in buildingDef.MaterialCategory)
				{
					Tag key = new Tag(name);
					if (!dictionary2.ContainsKey(key))
					{
						dictionary2.Add(key, new List<BuildingDef>());
					}
					dictionary2[key].Add(buildingDef);
				}
			}
		}
		foreach (Tag tag in dictionary2.Keys)
		{
			if (ElementLoader.GetElement(tag) == null)
			{
				string text = tag.ToString();
				string name2 = Strings.Get("STRINGS.MISC.TAGS." + text.ToUpper());
				List<ContentContainer> list = new List<ContentContainer>();
				CodexEntryGenerator.GenerateTitleContainers(name2, list);
				list.Add(new ContentContainer(new List<ICodexWidget>
				{
					new CodexSpacer(),
					new CodexText(Strings.Get("STRINGS.MISC.TAGS." + text.ToUpper() + "_DESC"), CodexTextStyle.Body, null),
					new CodexSpacer()
				}, ContentContainer.ContentLayout.Vertical));
				List<ICodexWidget> list2 = new List<ICodexWidget>();
				List<Tag> validMaterials = MaterialSelector.GetValidMaterials(tag, true);
				foreach (Tag tag2 in validMaterials)
				{
					list2.Add(new CodexIndentedLabelWithIcon(tag2.ProperName(), CodexTextStyle.Body, Def.GetUISprite(tag2, "ui", false)));
				}
				list.Add(new ContentContainer(list2, ContentContainer.ContentLayout.GridTwoColumn));
				list.Add(new ContentContainer(new List<ICodexWidget>
				{
					new CodexSpacer(),
					new CodexText(CODEX.HEADERS.MATERIALUSEDTOCONSTRUCT, CodexTextStyle.Title, null),
					new CodexDividerLine()
				}, ContentContainer.ContentLayout.Vertical));
				List<ICodexWidget> list3 = new List<ICodexWidget>();
				foreach (BuildingDef buildingDef2 in dictionary2[tag])
				{
					list3.Add(new CodexIndentedLabelWithIcon(buildingDef2.Name, CodexTextStyle.Body, Def.GetUISprite(buildingDef2.Tag, "ui", false)));
				}
				list.Add(new ContentContainer(list3, ContentContainer.ContentLayout.GridTwoColumn));
				CodexEntry codexEntry = new CodexEntry("BUILDINGMATERIALCLASSES", list, name2);
				codexEntry.parentId = codexEntry.category;
				CodexEntry codexEntry2 = codexEntry;
				Sprite icon;
				if ((icon = Assets.GetSprite("ui_" + tag.Name.ToLower())) == null)
				{
					icon = (((validMaterials.Count != 0) ? Def.GetUISprite(validMaterials[0], "ui", false).first : null) ?? Assets.GetSprite("ui_elements_classes"));
				}
				codexEntry2.icon = icon;
				if (tag == GameTags.BuildableAny)
				{
					codexEntry.icon = Assets.GetSprite("ui_elements_classes");
				}
				CodexCache.AddEntry(CodexCache.FormatLinkID(text), codexEntry, null);
				dictionary.Add(text, codexEntry);
			}
		}
		return dictionary;
	}

	// Token: 0x0600548F RID: 21647 RVA: 0x001E94C8 File Offset: 0x001E76C8
	public static Dictionary<string, CodexEntry> GenerateElementEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		Dictionary<string, CodexEntry> dictionary2 = new Dictionary<string, CodexEntry>();
		Dictionary<string, CodexEntry> dictionary3 = new Dictionary<string, CodexEntry>();
		Dictionary<string, CodexEntry> dictionary4 = new Dictionary<string, CodexEntry>();
		Dictionary<string, CodexEntry> dictionary5 = new Dictionary<string, CodexEntry>();
		string text = CodexCache.FormatLinkID("ELEMENTS");
		string text2 = CodexCache.FormatLinkID("ELEMENTS_SOLID");
		string text3 = CodexCache.FormatLinkID("ELEMENTS_LIQUID");
		string text4 = CodexCache.FormatLinkID("ELEMENTS_GAS");
		string text5 = CodexCache.FormatLinkID("ELEMENTS_OTHER");
		CodexCache.FormatLinkID("ELEMENTS_CLASSES");
		CodexEntryGenerator.CodexElementMap usedMap = new CodexEntryGenerator.CodexElementMap();
		CodexEntryGenerator.CodexElementMap madeMap = new CodexEntryGenerator.CodexElementMap();
		Tag waterTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		Tag dirtyWaterTag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		Action<GameObject, CodexEntryGenerator.CodexElementMap, CodexEntryGenerator.CodexElementMap> action = delegate(GameObject prefab, CodexEntryGenerator.CodexElementMap usedMap, CodexEntryGenerator.CodexElementMap made)
		{
			HashSet<ElementUsage> hashSet2 = new HashSet<ElementUsage>();
			HashSet<ElementUsage> hashSet3 = new HashSet<ElementUsage>();
			EnergyGenerator component = prefab.GetComponent<EnergyGenerator>();
			if (component)
			{
				IEnumerable<EnergyGenerator.InputItem> inputs = component.formula.inputs;
				foreach (EnergyGenerator.InputItem inputItem in (inputs ?? Enumerable.Empty<EnergyGenerator.InputItem>()))
				{
					hashSet2.Add(new ElementUsage(inputItem.tag, inputItem.consumptionRate, true));
				}
				IEnumerable<EnergyGenerator.OutputItem> outputs = component.formula.outputs;
				foreach (EnergyGenerator.OutputItem outputItem in (outputs ?? Enumerable.Empty<EnergyGenerator.OutputItem>()))
				{
					Tag tag2 = ElementLoader.FindElementByHash(outputItem.element).tag;
					hashSet3.Add(new ElementUsage(tag2, outputItem.creationRate, true));
				}
			}
			IEnumerable<ElementConverter> components = prefab.GetComponents<ElementConverter>();
			foreach (ElementConverter elementConverter in (components ?? Enumerable.Empty<ElementConverter>()))
			{
				IEnumerable<ElementConverter.ConsumedElement> consumedElements = elementConverter.consumedElements;
				foreach (ElementConverter.ConsumedElement consumedElement in (consumedElements ?? Enumerable.Empty<ElementConverter.ConsumedElement>()))
				{
					hashSet2.Add(new ElementUsage(consumedElement.Tag, consumedElement.MassConsumptionRate, true));
				}
				IEnumerable<ElementConverter.OutputElement> outputElements = elementConverter.outputElements;
				foreach (ElementConverter.OutputElement outputElement in (outputElements ?? Enumerable.Empty<ElementConverter.OutputElement>()))
				{
					Tag tag3 = ElementLoader.FindElementByHash(outputElement.elementHash).tag;
					hashSet3.Add(new ElementUsage(tag3, outputElement.massGenerationRate, true));
				}
			}
			IEnumerable<ElementConsumer> components2 = prefab.GetComponents<ElementConsumer>();
			foreach (ElementConsumer elementConsumer in (components2 ?? Enumerable.Empty<ElementConsumer>()))
			{
				if (!elementConsumer.storeOnConsume)
				{
					Tag tag4 = ElementLoader.FindElementByHash(elementConsumer.elementToConsume).tag;
					hashSet2.Add(new ElementUsage(tag4, elementConsumer.consumptionRate, true));
				}
			}
			IrrigationMonitor.Def def = prefab.GetDef<IrrigationMonitor.Def>();
			if (def != null)
			{
				foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in def.consumedElements)
				{
					hashSet2.Add(new ElementUsage(consumeInfo.tag, consumeInfo.massConsumptionRate, true));
				}
			}
			FertilizationMonitor.Def def2 = prefab.GetDef<FertilizationMonitor.Def>();
			if (def2 != null)
			{
				foreach (PlantElementAbsorber.ConsumeInfo consumeInfo2 in def2.consumedElements)
				{
					hashSet2.Add(new ElementUsage(consumeInfo2.tag, consumeInfo2.massConsumptionRate, true));
				}
			}
			FlushToilet component2 = prefab.GetComponent<FlushToilet>();
			if (component2)
			{
				hashSet2.Add(new ElementUsage(waterTag, component2.massConsumedPerUse, false));
				hashSet3.Add(new ElementUsage(dirtyWaterTag, component2.massEmittedPerUse, false));
			}
			HandSanitizer component3 = prefab.GetComponent<HandSanitizer>();
			if (component3)
			{
				Tag tag5 = ElementLoader.FindElementByHash(component3.consumedElement).tag;
				hashSet2.Add(new ElementUsage(tag5, component3.massConsumedPerUse, false));
				if (component3.outputElement != SimHashes.Vacuum)
				{
					Tag tag6 = ElementLoader.FindElementByHash(component3.outputElement).tag;
					hashSet3.Add(new ElementUsage(tag6, component3.massConsumedPerUse, false));
				}
			}
			if (prefab.IsPrefabID("Moo"))
			{
				hashSet2.Add(new ElementUsage("GasGrass", MooConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE, false));
				hashSet3.Add(new ElementUsage(ElementLoader.FindElementByHash(SimHashes.Milk).tag, MooTuning.MILK_PER_CYCLE, false));
			}
			CodexEntryGenerator.ConversionEntry conversionEntry2 = new CodexEntryGenerator.ConversionEntry();
			conversionEntry2.title = prefab.GetProperName();
			conversionEntry2.prefab = prefab;
			conversionEntry2.inSet = hashSet2;
			conversionEntry2.outSet = hashSet3;
			foreach (ElementUsage elementUsage in hashSet2)
			{
				usedMap.Add(elementUsage.tag, conversionEntry2);
			}
			foreach (ElementUsage elementUsage2 in hashSet3)
			{
				madeMap.Add(elementUsage2.tag, conversionEntry2);
			}
		};
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				if (buildingDef == null)
				{
					global::Debug.LogError("Building def for id " + keyValuePair.Key + " is null");
				}
				if (!buildingDef.Deprecated)
				{
					action(buildingDef.BuildingComplete, usedMap, madeMap);
				}
			}
		}
		HashSet<GameObject> hashSet = new HashSet<GameObject>(Assets.GetPrefabsWithComponent<Harvestable>());
		foreach (GameObject item in Assets.GetPrefabsWithComponent<WiltCondition>())
		{
			hashSet.Add(item);
		}
		foreach (GameObject gameObject in hashSet)
		{
			if (!(gameObject.GetComponent<BudUprootedMonitor>() != null))
			{
				action(gameObject, usedMap, madeMap);
			}
		}
		foreach (GameObject gameObject2 in Assets.GetPrefabsWithComponent<CreatureBrain>())
		{
			if (gameObject2.GetDef<BabyMonitor.Def>() == null)
			{
				action(gameObject2, usedMap, madeMap);
			}
		}
		foreach (KeyValuePair<Tag, Diet> keyValuePair2 in DietManager.CollectDiets(null))
		{
			GameObject gameObject3 = Assets.GetPrefab(keyValuePair2.Key).gameObject;
			if (gameObject3.GetDef<BabyMonitor.Def>() == null)
			{
				float num = 0f;
				foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(gameObject3.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
				{
					if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
					{
						num = attributeModifier.Value;
					}
				}
				foreach (Diet.Info info in keyValuePair2.Value.infos)
				{
					float num2 = -num / info.caloriesPerKg;
					float amount = num2 * info.producedConversionRate;
					foreach (Tag tag in info.consumedTags)
					{
						CodexEntryGenerator.ConversionEntry conversionEntry = new CodexEntryGenerator.ConversionEntry();
						conversionEntry.title = gameObject3.GetProperName();
						conversionEntry.prefab = gameObject3;
						conversionEntry.inSet = new HashSet<ElementUsage>();
						conversionEntry.inSet.Add(new ElementUsage(tag, num2, true));
						conversionEntry.outSet = new HashSet<ElementUsage>();
						conversionEntry.outSet.Add(new ElementUsage(info.producedElement, amount, true));
						usedMap.Add(tag, conversionEntry);
						madeMap.Add(info.producedElement, conversionEntry);
					}
				}
			}
		}
		int i;
		Action<Element, List<ContentContainer>> action2 = delegate(Element element, List<ContentContainer> containers)
		{
			List<ICodexWidget> list2 = new List<ICodexWidget>();
			List<ICodexWidget> list3 = new List<ICodexWidget>();
			if (element.highTempTransition != null)
			{
				list2.Add(new CodexTemperatureTransitionPanel(element, CodexTemperatureTransitionPanel.TransitionType.HEAT));
			}
			if (element.lowTempTransition != null)
			{
				list2.Add(new CodexTemperatureTransitionPanel(element, CodexTemperatureTransitionPanel.TransitionType.COOL));
			}
			foreach (Element element3 in ElementLoader.elements)
			{
				if (!element3.disabled)
				{
					if (element3.highTempTransition == element || ElementLoader.FindElementByHash(element3.highTempTransitionOreID) == element)
					{
						list3.Add(new CodexTemperatureTransitionPanel(element3, CodexTemperatureTransitionPanel.TransitionType.HEAT));
					}
					if (element3.lowTempTransition == element || ElementLoader.FindElementByHash(element3.lowTempTransitionOreID) == element)
					{
						list3.Add(new CodexTemperatureTransitionPanel(element3, CodexTemperatureTransitionPanel.TransitionType.COOL));
					}
				}
			}
			if (list2.Count > 0)
			{
				ContentContainer contentContainer = new ContentContainer(list2, ContentContainer.ContentLayout.Vertical);
				containers.Add(new ContentContainer(new List<ICodexWidget>
				{
					new CodexSpacer(),
					new CodexCollapsibleHeader(CODEX.HEADERS.ELEMENTTRANSITIONSTO, contentContainer)
				}, ContentContainer.ContentLayout.Vertical));
				containers.Add(contentContainer);
			}
			if (list3.Count > 0)
			{
				ContentContainer contentContainer2 = new ContentContainer(list3, ContentContainer.ContentLayout.Vertical);
				containers.Add(new ContentContainer(new List<ICodexWidget>
				{
					new CodexSpacer(),
					new CodexCollapsibleHeader(CODEX.HEADERS.ELEMENTTRANSITIONSFROM, contentContainer2)
				}, ContentContainer.ContentLayout.Vertical));
				containers.Add(contentContainer2);
			}
			List<ICodexWidget> list4 = new List<ICodexWidget>();
			List<ICodexWidget> list5 = new List<ICodexWidget>();
			Func<ComplexRecipe.RecipeElement, bool> <>9__2;
			Func<ComplexRecipe.RecipeElement, bool> <>9__3;
			foreach (ComplexRecipe complexRecipe in ComplexRecipeManager.Get().recipes)
			{
				IEnumerable<ComplexRecipe.RecipeElement> ingredients = complexRecipe.ingredients;
				Func<ComplexRecipe.RecipeElement, bool> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = ((ComplexRecipe.RecipeElement i) => i.material == element.tag));
				}
				if (ingredients.Any(predicate))
				{
					list4.Add(new CodexRecipePanel(complexRecipe, false));
				}
				IEnumerable<ComplexRecipe.RecipeElement> results = complexRecipe.results;
				Func<ComplexRecipe.RecipeElement, bool> predicate2;
				if ((predicate2 = <>9__3) == null)
				{
					predicate2 = (<>9__3 = ((ComplexRecipe.RecipeElement i) => i.material == element.tag));
				}
				if (results.Any(predicate2))
				{
					list5.Add(new CodexRecipePanel(complexRecipe, true));
				}
			}
			List<CodexEntryGenerator.ConversionEntry> list6;
			if (usedMap.map.TryGetValue(element.tag, out list6))
			{
				foreach (CodexEntryGenerator.ConversionEntry conversionEntry2 in list6)
				{
					list4.Add(new CodexConversionPanel(conversionEntry2.title, conversionEntry2.inSet.ToArray<ElementUsage>(), conversionEntry2.outSet.ToArray<ElementUsage>(), conversionEntry2.prefab));
				}
			}
			List<CodexEntryGenerator.ConversionEntry> list7;
			if (madeMap.map.TryGetValue(element.tag, out list7))
			{
				foreach (CodexEntryGenerator.ConversionEntry conversionEntry3 in list7)
				{
					list5.Add(new CodexConversionPanel(conversionEntry3.title, conversionEntry3.inSet.ToArray<ElementUsage>(), conversionEntry3.outSet.ToArray<ElementUsage>(), conversionEntry3.prefab));
				}
			}
			ContentContainer contentContainer3 = new ContentContainer(list4, ContentContainer.ContentLayout.Vertical);
			ContentContainer contentContainer4 = new ContentContainer(list5, ContentContainer.ContentLayout.Vertical);
			if (list4.Count > 0)
			{
				containers.Add(new ContentContainer(new List<ICodexWidget>
				{
					new CodexSpacer(),
					new CodexCollapsibleHeader(CODEX.HEADERS.ELEMENTCONSUMEDBY, contentContainer3)
				}, ContentContainer.ContentLayout.Vertical));
				containers.Add(contentContainer3);
			}
			if (list5.Count > 0)
			{
				containers.Add(new ContentContainer(new List<ICodexWidget>
				{
					new CodexSpacer(),
					new CodexCollapsibleHeader(CODEX.HEADERS.ELEMENTPRODUCEDBY, contentContainer4)
				}, ContentContainer.ContentLayout.Vertical));
				containers.Add(contentContainer4);
			}
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(element.FullDescription(true), CodexTextStyle.Body, null),
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical));
		};
		string text6;
		foreach (Element element2 in ElementLoader.elements)
		{
			if (!element2.disabled)
			{
				List<ContentContainer> list = new List<ContentContainer>();
				global::Tuple<Sprite, Color> tuple = Def.GetUISprite(element2, "ui", false);
				if (tuple.first == null)
				{
					if (element2.id == SimHashes.Void)
					{
						tuple = new global::Tuple<Sprite, Color>(Assets.GetSprite("ui_elements-void"), Color.white);
					}
					else if (element2.id == SimHashes.Vacuum)
					{
						tuple = new global::Tuple<Sprite, Color>(Assets.GetSprite("ui_elements-vacuum"), Color.white);
					}
				}
				CodexEntryGenerator.GenerateTitleContainers(element2.name, list);
				CodexEntryGenerator.GenerateImageContainers(new global::Tuple<Sprite, Color>[]
				{
					tuple
				}, list, ContentContainer.ContentLayout.Horizontal);
				action2(element2, list);
				text6 = element2.id.ToString();
				string text7;
				Dictionary<string, CodexEntry> dictionary6;
				if (element2.IsSolid)
				{
					text7 = text2;
					dictionary6 = dictionary2;
				}
				else if (element2.IsLiquid)
				{
					text7 = text3;
					dictionary6 = dictionary3;
				}
				else if (element2.IsGas)
				{
					text7 = text4;
					dictionary6 = dictionary4;
				}
				else
				{
					text7 = text5;
					dictionary6 = dictionary5;
				}
				CodexEntry codexEntry = new CodexEntry(text7, list, element2.name);
				codexEntry.parentId = text7;
				codexEntry.icon = tuple.first;
				codexEntry.iconColor = tuple.second;
				CodexCache.AddEntry(text6, codexEntry, null);
				dictionary6.Add(text6, codexEntry);
			}
		}
		text6 = text2;
		CodexEntry codexEntry2 = CodexEntryGenerator.GenerateCategoryEntry(text6, UI.CODEX.CATEGORYNAMES.ELEMENTSSOLID, dictionary2, Assets.GetSprite("ui_elements-solid"), true, true, null);
		codexEntry2.parentId = text;
		codexEntry2.category = text;
		dictionary.Add(text6, codexEntry2);
		text6 = text3;
		codexEntry2 = CodexEntryGenerator.GenerateCategoryEntry(text6, UI.CODEX.CATEGORYNAMES.ELEMENTSLIQUID, dictionary3, Assets.GetSprite("ui_elements-liquids"), true, true, null);
		codexEntry2.parentId = text;
		codexEntry2.category = text;
		dictionary.Add(text6, codexEntry2);
		text6 = text4;
		codexEntry2 = CodexEntryGenerator.GenerateCategoryEntry(text6, UI.CODEX.CATEGORYNAMES.ELEMENTSGAS, dictionary4, Assets.GetSprite("ui_elements-gases"), true, true, null);
		codexEntry2.parentId = text;
		codexEntry2.category = text;
		dictionary.Add(text6, codexEntry2);
		text6 = text5;
		codexEntry2 = CodexEntryGenerator.GenerateCategoryEntry(text6, UI.CODEX.CATEGORYNAMES.ELEMENTSOTHER, dictionary5, Assets.GetSprite("ui_elements-other"), true, true, null);
		codexEntry2.parentId = text;
		codexEntry2.category = text;
		dictionary.Add(text6, codexEntry2);
		CodexEntryGenerator.PopulateCategoryEntries(dictionary);
		return dictionary;
	}

	// Token: 0x06005490 RID: 21648 RVA: 0x001E9CAC File Offset: 0x001E7EAC
	public static Dictionary<string, CodexEntry> GenerateDiseaseEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (Disease disease in Db.Get().Diseases.resources)
		{
			if (!disease.Disabled)
			{
				List<ContentContainer> list = new List<ContentContainer>();
				CodexEntryGenerator.GenerateTitleContainers(disease.Name, list);
				CodexEntryGenerator.GenerateDiseaseDescriptionContainers(disease, list);
				CodexEntry codexEntry = new CodexEntry("DISEASE", list, disease.Name);
				codexEntry.parentId = "DISEASE";
				dictionary.Add(disease.Id, codexEntry);
				codexEntry.icon = Assets.GetSprite("overlay_disease");
				CodexCache.AddEntry(disease.Id, codexEntry, null);
			}
		}
		return dictionary;
	}

	// Token: 0x06005491 RID: 21649 RVA: 0x001E9D80 File Offset: 0x001E7F80
	public static CategoryEntry GenerateCategoryEntry(string id, string name, Dictionary<string, CodexEntry> entries, Sprite icon = null, bool largeFormat = true, bool sort = true, string overrideHeader = null)
	{
		List<ContentContainer> list = new List<ContentContainer>();
		CodexEntryGenerator.GenerateTitleContainers((overrideHeader == null) ? name : overrideHeader, list);
		List<CodexEntry> list2 = new List<CodexEntry>();
		foreach (KeyValuePair<string, CodexEntry> keyValuePair in entries)
		{
			list2.Add(keyValuePair.Value);
			if (icon == null)
			{
				icon = keyValuePair.Value.icon;
			}
		}
		CategoryEntry categoryEntry = new CategoryEntry("Root", list, name, list2, largeFormat, sort);
		categoryEntry.icon = icon;
		CodexCache.AddEntry(id, categoryEntry, null);
		return categoryEntry;
	}

	// Token: 0x06005492 RID: 21650 RVA: 0x001E9E2C File Offset: 0x001E802C
	public static Dictionary<string, CodexEntry> GenerateTutorialNotificationEntries()
	{
		CodexEntry codexEntry = new CodexEntry("MISCELLANEOUSTIPS", new List<ContentContainer>
		{
			new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical)
		}, Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES.MISCELLANEOUSTIPS"));
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		for (int i = 0; i < 20; i++)
		{
			TutorialMessage tutorialMessage = (TutorialMessage)Tutorial.Instance.TutorialMessage((Tutorial.TutorialMessages)i, false);
			if (tutorialMessage != null && DlcManager.IsDlcListValidForCurrentContent(tutorialMessage.DLCIDs))
			{
				if (!string.IsNullOrEmpty(tutorialMessage.videoClipId))
				{
					List<ContentContainer> list = new List<ContentContainer>();
					CodexEntryGenerator.GenerateTitleContainers(tutorialMessage.GetTitle(), list);
					CodexVideo codexVideo = new CodexVideo();
					codexVideo.videoName = tutorialMessage.videoClipId;
					codexVideo.overlayName = tutorialMessage.videoOverlayName;
					codexVideo.overlayTexts = new List<string>
					{
						tutorialMessage.videoTitleText,
						VIDEOS.TUTORIAL_HEADER
					};
					list.Add(new ContentContainer(new List<ICodexWidget>
					{
						codexVideo
					}, ContentContainer.ContentLayout.Vertical));
					list.Add(new ContentContainer(new List<ICodexWidget>
					{
						new CodexText(tutorialMessage.GetMessageBody(), CodexTextStyle.Body, tutorialMessage.GetTitle())
					}, ContentContainer.ContentLayout.Vertical));
					CodexEntry codexEntry2 = new CodexEntry("Videos", list, UI.FormatAsLink(tutorialMessage.GetTitle(), "videos_" + i.ToString()));
					codexEntry2.icon = Assets.GetSprite("codexVideo");
					CodexCache.AddEntry("videos_" + i.ToString(), codexEntry2, null);
					dictionary.Add(codexEntry2.id, codexEntry2);
				}
				else
				{
					List<ContentContainer> list2 = new List<ContentContainer>();
					CodexEntryGenerator.GenerateTitleContainers(tutorialMessage.GetTitle(), list2);
					list2.Add(new ContentContainer(new List<ICodexWidget>
					{
						new CodexText(tutorialMessage.GetMessageBody(), CodexTextStyle.Body, tutorialMessage.GetTitle())
					}, ContentContainer.ContentLayout.Vertical));
					list2.Add(new ContentContainer(new List<ICodexWidget>
					{
						new CodexSpacer(),
						new CodexSpacer()
					}, ContentContainer.ContentLayout.Vertical));
					SubEntry item = new SubEntry("MISCELLANEOUSTIPS" + i.ToString(), "MISCELLANEOUSTIPS", list2, tutorialMessage.GetTitle());
					codexEntry.subEntries.Add(item);
				}
			}
		}
		CodexCache.AddEntry("MISCELLANEOUSTIPS", codexEntry, null);
		return dictionary;
	}

	// Token: 0x06005493 RID: 21651 RVA: 0x001EA08C File Offset: 0x001E828C
	public static void PopulateCategoryEntries(Dictionary<string, CodexEntry> categoryEntries)
	{
		List<CategoryEntry> list = new List<CategoryEntry>();
		foreach (KeyValuePair<string, CodexEntry> keyValuePair in categoryEntries)
		{
			list.Add(keyValuePair.Value as CategoryEntry);
		}
		CodexEntryGenerator.PopulateCategoryEntries(list, null);
	}

	// Token: 0x06005494 RID: 21652 RVA: 0x001EA0F4 File Offset: 0x001E82F4
	public static void PopulateCategoryEntries(List<CategoryEntry> categoryEntries, Comparison<CodexEntry> comparison = null)
	{
		foreach (CategoryEntry categoryEntry in categoryEntries)
		{
			List<ContentContainer> contentContainers = categoryEntry.contentContainers;
			List<CodexEntry> list = new List<CodexEntry>();
			foreach (CodexEntry item in categoryEntry.entriesInCategory)
			{
				list.Add(item);
			}
			if (categoryEntry.sort)
			{
				if (comparison == null)
				{
					list.Sort((CodexEntry a, CodexEntry b) => UI.StripLinkFormatting(a.name).CompareTo(UI.StripLinkFormatting(b.name)));
				}
				else
				{
					list.Sort(comparison);
				}
			}
			if (categoryEntry.largeFormat)
			{
				ContentContainer contentContainer = new ContentContainer(new List<ICodexWidget>(), ContentContainer.ContentLayout.Grid);
				foreach (CodexEntry codexEntry in list)
				{
					contentContainer.content.Add(new CodexLabelWithLargeIcon(codexEntry.name, CodexTextStyle.BodyWhite, new global::Tuple<Sprite, Color>((codexEntry.icon != null) ? codexEntry.icon : Assets.GetSprite("unknown"), codexEntry.iconColor), codexEntry.id));
				}
				if (categoryEntry.showBeforeGeneratedCategoryLinks)
				{
					contentContainers.Add(contentContainer);
				}
				else
				{
					ContentContainer item2 = contentContainers[contentContainers.Count - 1];
					contentContainers.RemoveAt(contentContainers.Count - 1);
					contentContainers.Insert(0, item2);
					contentContainers.Insert(1, contentContainer);
					contentContainers.Insert(2, new ContentContainer(new List<ICodexWidget>
					{
						new CodexSpacer()
					}, ContentContainer.ContentLayout.Vertical));
				}
			}
			else
			{
				ContentContainer contentContainer2 = new ContentContainer(new List<ICodexWidget>(), ContentContainer.ContentLayout.Vertical);
				foreach (CodexEntry codexEntry2 in list)
				{
					if (codexEntry2.icon == null)
					{
						contentContainer2.content.Add(new CodexText(codexEntry2.name, CodexTextStyle.Body, null));
					}
					else
					{
						contentContainer2.content.Add(new CodexLabelWithIcon(codexEntry2.name, CodexTextStyle.Body, new global::Tuple<Sprite, Color>(codexEntry2.icon, codexEntry2.iconColor), 64, 48));
					}
				}
				if (categoryEntry.showBeforeGeneratedCategoryLinks)
				{
					contentContainers.Add(contentContainer2);
				}
				else
				{
					ContentContainer item3 = contentContainers[contentContainers.Count - 1];
					contentContainers.RemoveAt(contentContainers.Count - 1);
					contentContainers.Insert(0, item3);
					contentContainers.Insert(1, contentContainer2);
				}
			}
		}
	}

	// Token: 0x06005495 RID: 21653 RVA: 0x001EA3E8 File Offset: 0x001E85E8
	private static void GenerateTitleContainers(string name, List<ContentContainer> containers)
	{
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(name, CodexTextStyle.Title, null),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005496 RID: 21654 RVA: 0x001EA424 File Offset: 0x001E8624
	private static void GeneratePrerequisiteTechContainers(Tech tech, List<ContentContainer> containers)
	{
		if (tech.requiredTech == null || tech.requiredTech.Count == 0)
		{
			return;
		}
		List<ICodexWidget> list = new List<ICodexWidget>();
		list.Add(new CodexText(CODEX.HEADERS.PREREQUISITE_TECH, CodexTextStyle.Subtitle, null));
		list.Add(new CodexDividerLine());
		list.Add(new CodexSpacer());
		foreach (Tech tech2 in tech.requiredTech)
		{
			list.Add(new CodexText(tech2.Name, CodexTextStyle.Body, null));
		}
		list.Add(new CodexSpacer());
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005497 RID: 21655 RVA: 0x001EA4E4 File Offset: 0x001E86E4
	private static void GenerateSkillRequirementsAndPerksContainers(Skill skill, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		CodexText item = new CodexText(CODEX.HEADERS.ROLE_PERKS, CodexTextStyle.Subtitle, null);
		CodexText item2 = new CodexText(CODEX.HEADERS.ROLE_PERKS_DESC, CodexTextStyle.Body, null);
		list.Add(item);
		list.Add(new CodexDividerLine());
		list.Add(item2);
		list.Add(new CodexSpacer());
		foreach (SkillPerk skillPerk in skill.perks)
		{
			CodexText item3 = new CodexText(skillPerk.Name, CodexTextStyle.Body, null);
			list.Add(item3);
		}
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
		list.Add(new CodexSpacer());
	}

	// Token: 0x06005498 RID: 21656 RVA: 0x001EA5AC File Offset: 0x001E87AC
	private static void GenerateRelatedSkillContainers(Skill skill, List<ContentContainer> containers)
	{
		bool flag = false;
		List<ICodexWidget> list = new List<ICodexWidget>();
		CodexText item = new CodexText(CODEX.HEADERS.PREREQUISITE_ROLES, CodexTextStyle.Subtitle, null);
		list.Add(item);
		list.Add(new CodexDividerLine());
		list.Add(new CodexSpacer());
		foreach (string id in skill.priorSkills)
		{
			CodexText item2 = new CodexText(Db.Get().Skills.Get(id).Name, CodexTextStyle.Body, null);
			list.Add(item2);
			flag = true;
		}
		if (flag)
		{
			list.Add(new CodexSpacer());
			containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
		}
		bool flag2 = false;
		List<ICodexWidget> list2 = new List<ICodexWidget>();
		CodexText item3 = new CodexText(CODEX.HEADERS.UNLOCK_ROLES, CodexTextStyle.Subtitle, null);
		CodexText item4 = new CodexText(CODEX.HEADERS.UNLOCK_ROLES_DESC, CodexTextStyle.Body, null);
		list2.Add(item3);
		list2.Add(new CodexDividerLine());
		list2.Add(item4);
		list2.Add(new CodexSpacer());
		foreach (Skill skill2 in Db.Get().Skills.resources)
		{
			if (!skill2.deprecated)
			{
				using (List<string>.Enumerator enumerator = skill2.priorSkills.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == skill.Id)
						{
							CodexText item5 = new CodexText(skill2.Name, CodexTextStyle.Body, null);
							list2.Add(item5);
							flag2 = true;
						}
					}
				}
			}
		}
		if (flag2)
		{
			list2.Add(new CodexSpacer());
			containers.Add(new ContentContainer(list2, ContentContainer.ContentLayout.Vertical));
		}
	}

	// Token: 0x06005499 RID: 21657 RVA: 0x001EA7A0 File Offset: 0x001E89A0
	private static void GenerateUnlockContainers(Tech tech, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		CodexText item = new CodexText(CODEX.HEADERS.TECH_UNLOCKS, CodexTextStyle.Subtitle, null);
		list.Add(item);
		list.Add(new CodexDividerLine());
		list.Add(new CodexSpacer());
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
		foreach (TechItem techItem in tech.unlockedItems)
		{
			List<ICodexWidget> list2 = new List<ICodexWidget>();
			CodexImage item2 = new CodexImage(64, 64, techItem.getUISprite("ui", false));
			list2.Add(item2);
			CodexText item3 = new CodexText(techItem.Name, CodexTextStyle.Body, null);
			list2.Add(item3);
			containers.Add(new ContentContainer(list2, ContentContainer.ContentLayout.Horizontal));
		}
	}

	// Token: 0x0600549A RID: 21658 RVA: 0x001EA880 File Offset: 0x001E8A80
	private static void GenerateRecipeContainers(Tag prefabID, List<ContentContainer> containers)
	{
		Recipe recipe = null;
		foreach (Recipe recipe2 in RecipeManager.Get().recipes)
		{
			if (recipe2.Result == prefabID)
			{
				recipe = recipe2;
				break;
			}
		}
		if (recipe == null)
		{
			return;
		}
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(CODEX.HEADERS.RECIPE, CodexTextStyle.Subtitle, null),
			new CodexSpacer(),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical));
		Func<Recipe, List<ContentContainer>> func = delegate(Recipe rec)
		{
			List<ContentContainer> list = new List<ContentContainer>();
			foreach (Recipe.Ingredient ingredient in rec.Ingredients)
			{
				GameObject prefab = Assets.GetPrefab(ingredient.tag);
				if (prefab != null)
				{
					list.Add(new ContentContainer(new List<ICodexWidget>
					{
						new CodexImage(64, 64, Def.GetUISprite(prefab, "ui", false)),
						new CodexText(string.Format(UI.CODEX.RECIPE_ITEM, Assets.GetPrefab(ingredient.tag).GetProperName(), ingredient.amount, (ElementLoader.GetElement(ingredient.tag) == null) ? "" : UI.UNITSUFFIXES.MASS.KILOGRAM.text), CodexTextStyle.Body, null)
					}, ContentContainer.ContentLayout.Horizontal));
				}
			}
			return list;
		};
		containers.AddRange(func(recipe));
		GameObject gameObject = (recipe.fabricators == null) ? null : Assets.GetPrefab(recipe.fabricators[0]);
		if (gameObject != null)
		{
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexText(UI.CODEX.RECIPE_FABRICATOR_HEADER, CodexTextStyle.Subtitle, null),
				new CodexDividerLine()
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexImage(64, 64, Def.GetUISpriteFromMultiObjectAnim(gameObject.GetComponent<KBatchedAnimController>().AnimFiles[0], "ui", false, "")),
				new CodexText(string.Format(UI.CODEX.RECIPE_FABRICATOR, recipe.FabricationTime, gameObject.GetProperName()), CodexTextStyle.Body, null)
			}, ContentContainer.ContentLayout.Horizontal));
		}
	}

	// Token: 0x0600549B RID: 21659 RVA: 0x001EAA20 File Offset: 0x001E8C20
	private static void GenerateUsedInRecipeContainers(Tag prefabID, List<ContentContainer> containers)
	{
		List<Recipe> list = new List<Recipe>();
		foreach (Recipe recipe in RecipeManager.Get().recipes)
		{
			using (List<Recipe.Ingredient>.Enumerator enumerator2 = recipe.Ingredients.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.tag == prefabID)
					{
						list.Add(recipe);
					}
				}
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(CODEX.HEADERS.USED_IN_RECIPES, CodexTextStyle.Subtitle, null),
			new CodexSpacer(),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical));
		foreach (Recipe recipe2 in list)
		{
			GameObject prefab = Assets.GetPrefab(recipe2.Result);
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexImage(64, 64, Def.GetUISprite(prefab, "ui", false)),
				new CodexText(prefab.GetProperName(), CodexTextStyle.Body, null)
			}, ContentContainer.ContentLayout.Horizontal));
		}
	}

	// Token: 0x0600549C RID: 21660 RVA: 0x001EAB90 File Offset: 0x001E8D90
	private static void GenerateRoomTypeDetailsContainers(RoomType roomType, List<ContentContainer> containers)
	{
		ICodexWidget item = new CodexText(UI.CODEX.DETAILS, CodexTextStyle.Subtitle, null);
		ICodexWidget item2 = new CodexDividerLine();
		ContentContainer item3 = new ContentContainer(new List<ICodexWidget>
		{
			item,
			item2
		}, ContentContainer.ContentLayout.Vertical);
		containers.Add(item3);
		List<ICodexWidget> list = new List<ICodexWidget>();
		if (!string.IsNullOrEmpty(roomType.effect))
		{
			string roomEffectsString = roomType.GetRoomEffectsString();
			list.Add(new CodexText(roomEffectsString, CodexTextStyle.Body, null));
			list.Add(new CodexSpacer());
		}
		if (roomType.primary_constraint != null || roomType.additional_constraints != null)
		{
			list.Add(new CodexText(ROOMS.CRITERIA.HEADER, CodexTextStyle.Body, null));
			string text = "";
			if (roomType.primary_constraint != null)
			{
				text = text + "    • " + roomType.primary_constraint.name;
			}
			if (roomType.additional_constraints != null)
			{
				for (int i = 0; i < roomType.additional_constraints.Length; i++)
				{
					text = text + "\n    • " + roomType.additional_constraints[i].name;
				}
			}
			list.Add(new CodexText(text, CodexTextStyle.Body, null));
		}
		ContentContainer item4 = new ContentContainer(list, ContentContainer.ContentLayout.Vertical);
		containers.Add(item4);
	}

	// Token: 0x0600549D RID: 21661 RVA: 0x001EACC0 File Offset: 0x001E8EC0
	private static void GenerateRoomTypeDescriptionContainers(RoomType roomType, List<ContentContainer> containers)
	{
		ContentContainer item = new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(roomType.description, CodexTextStyle.Body, null),
			new CodexSpacer()
		}, ContentContainer.ContentLayout.Vertical);
		containers.Add(item);
	}

	// Token: 0x0600549E RID: 21662 RVA: 0x001EAD00 File Offset: 0x001E8F00
	private static void GeneratePlantDescriptionContainers(GameObject plant, List<ContentContainer> containers)
	{
		SeedProducer component = plant.GetComponent<SeedProducer>();
		if (component != null)
		{
			GameObject prefab = Assets.GetPrefab(component.seedInfo.seedId);
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexText(CODEX.HEADERS.GROWNFROMSEED, CodexTextStyle.Subtitle, null),
				new CodexDividerLine()
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexImage(48, 48, Def.GetUISprite(prefab, "ui", false)),
				new CodexText(prefab.GetProperName(), CodexTextStyle.Body, null)
			}, ContentContainer.ContentLayout.Horizontal));
		}
		List<ICodexWidget> list = new List<ICodexWidget>();
		list.Add(new CodexSpacer());
		list.Add(new CodexText(UI.CODEX.DETAILS, CodexTextStyle.Subtitle, null));
		list.Add(new CodexDividerLine());
		InfoDescription component2 = Assets.GetPrefab(plant.PrefabID()).GetComponent<InfoDescription>();
		if (component2 != null)
		{
			list.Add(new CodexText(component2.description, CodexTextStyle.Body, null));
		}
		string text = "";
		List<Descriptor> plantRequirementDescriptors = GameUtil.GetPlantRequirementDescriptors(plant);
		if (plantRequirementDescriptors.Count > 0)
		{
			text += plantRequirementDescriptors[0].text;
			for (int i = 1; i < plantRequirementDescriptors.Count; i++)
			{
				text = text + "\n    • " + plantRequirementDescriptors[i].text;
			}
			list.Add(new CodexText(text, CodexTextStyle.Body, null));
			list.Add(new CodexSpacer());
		}
		text = "";
		List<Descriptor> plantEffectDescriptors = GameUtil.GetPlantEffectDescriptors(plant);
		if (plantEffectDescriptors.Count > 0)
		{
			text += plantEffectDescriptors[0].text;
			for (int j = 1; j < plantEffectDescriptors.Count; j++)
			{
				text = text + "\n    • " + plantEffectDescriptors[j].text;
			}
			CodexText item = new CodexText(text, CodexTextStyle.Body, null);
			list.Add(item);
			list.Add(new CodexSpacer());
		}
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x0600549F RID: 21663 RVA: 0x001EAF06 File Offset: 0x001E9106
	private static ICodexWidget GetIconWidget(object entity)
	{
		return new CodexImage(32, 32, Def.GetUISprite(entity, "ui", false));
	}

	// Token: 0x060054A0 RID: 21664 RVA: 0x001EAF20 File Offset: 0x001E9120
	private static void GenerateDiseaseDescriptionContainers(Disease disease, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		list.Add(new CodexSpacer());
		StringEntry stringEntry = null;
		if (Strings.TryGet("STRINGS.DUPLICANTS.DISEASES." + disease.Id.ToUpper() + ".DESC", out stringEntry))
		{
			list.Add(new CodexText(stringEntry.String, CodexTextStyle.Body, null));
			list.Add(new CodexSpacer());
		}
		foreach (Descriptor descriptor in disease.GetQuantitativeDescriptors())
		{
			list.Add(new CodexText(descriptor.text, CodexTextStyle.Body, null));
		}
		list.Add(new CodexSpacer());
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x060054A1 RID: 21665 RVA: 0x001EAFEC File Offset: 0x001E91EC
	private static void GenerateFoodDescriptionContainers(EdiblesManager.FoodInfo food, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>
		{
			new CodexText(food.Description, CodexTextStyle.Body, null),
			new CodexSpacer(),
			new CodexText(string.Format(UI.CODEX.FOOD.QUALITY, GameUtil.GetFormattedFoodQuality(food.Quality)), CodexTextStyle.Body, null),
			new CodexText(string.Format(UI.CODEX.FOOD.CALORIES, GameUtil.GetFormattedCalories(food.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), CodexTextStyle.Body, null),
			new CodexSpacer(),
			new CodexText(food.CanRot ? string.Format(UI.CODEX.FOOD.SPOILPROPERTIES, GameUtil.GetFormattedTemperature(food.RotTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(food.PreserveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedCycles(food.SpoilTime, "F1", false)) : UI.CODEX.FOOD.NON_PERISHABLE.ToString(), CodexTextStyle.Body, null),
			new CodexSpacer()
		};
		if (food.Effects.Count > 0)
		{
			list.Add(new CodexText(CODEX.HEADERS.FOODEFFECTS + ":", CodexTextStyle.Body, null));
			foreach (string text in food.Effects)
			{
				Klei.AI.Modifier modifier = Db.Get().effects.Get(text);
				string text2 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
				string text3 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".DESCRIPTION");
				string text4 = "";
				foreach (AttributeModifier attributeModifier in modifier.SelfModifiers)
				{
					text4 = string.Concat(new string[]
					{
						text4,
						"\n    • ",
						Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME"),
						": ",
						attributeModifier.GetFormattedString()
					});
				}
				text3 += text4;
				text2 = UI.FormatAsLink(text2, CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID);
				list.Add(new CodexTextWithTooltip("    • " + text2, text3, CodexTextStyle.Body));
			}
			list.Add(new CodexSpacer());
		}
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x060054A2 RID: 21666 RVA: 0x001EB2A4 File Offset: 0x001E94A4
	private static void GenerateTechDescriptionContainers(Tech tech, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		CodexText item = new CodexText(Strings.Get("STRINGS.RESEARCH.TECHS." + tech.Id.ToUpper() + ".DESC"), CodexTextStyle.Body, null);
		list.Add(item);
		list.Add(new CodexSpacer());
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x060054A3 RID: 21667 RVA: 0x001EB304 File Offset: 0x001E9504
	private static void GenerateGenericDescriptionContainers(string description, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		CodexText item = new CodexText(description, CodexTextStyle.Body, null);
		list.Add(item);
		list.Add(new CodexSpacer());
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x060054A4 RID: 21668 RVA: 0x001EB340 File Offset: 0x001E9540
	private static void GenerateBuildingDescriptionContainers(BuildingDef def, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		list.Add(new CodexText(Strings.Get("STRINGS.BUILDINGS.PREFABS." + def.PrefabID.ToUpper() + ".EFFECT"), CodexTextStyle.Body, null));
		list.Add(new CodexSpacer());
		List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(def.BuildingComplete, false);
		List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(allDescriptors);
		if (requirementDescriptors.Count > 0)
		{
			list.Add(new CodexText(CODEX.HEADERS.BUILDINGREQUIREMENTS, CodexTextStyle.Subtitle, null));
			foreach (Descriptor descriptor in requirementDescriptors)
			{
				list.Add(new CodexTextWithTooltip("    " + descriptor.text, descriptor.tooltipText, CodexTextStyle.Body));
			}
			list.Add(new CodexSpacer());
		}
		if (def.MaterialCategory.Length != def.Mass.Length)
		{
			global::Debug.LogWarningFormat("{0} Required Materials({1}) and Masses({2}) mismatch!", new object[]
			{
				def.name,
				string.Join(", ", def.MaterialCategory),
				string.Join<float>(", ", def.Mass)
			});
		}
		if (def.MaterialCategory.Length + def.Mass.Length != 0)
		{
			list.Add(new CodexText(CODEX.HEADERS.BUILDINGCONSTRUCTIONPROPS, CodexTextStyle.Subtitle, null));
			list.Add(new CodexText("    " + string.Format(CODEX.FORMAT_STRINGS.BUILDING_SIZE, def.WidthInCells, def.HeightInCells), CodexTextStyle.Body, null));
			list.Add(new CodexText("    " + string.Format(CODEX.FORMAT_STRINGS.CONSTRUCTION_TIME, def.ConstructionTime), CodexTextStyle.Body, null));
			List<string> list2 = new List<string>();
			for (int i = 0; i < Math.Min(def.MaterialCategory.Length, def.Mass.Length); i++)
			{
				list2.Add(string.Format(CODEX.FORMAT_STRINGS.MATERIAL_MASS, UI.FormatAsLink(Strings.Get("STRINGS.MISC.TAGS." + def.MaterialCategory[i].ToUpper()), def.MaterialCategory[i]), GameUtil.GetFormattedMass(def.Mass[i], GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")));
			}
			list.Add(new CodexText("    " + CODEX.HEADERS.BUILDINGCONSTRUCTIONMATERIALS + string.Join(", ", list2), CodexTextStyle.Body, null));
			list.Add(new CodexSpacer());
		}
		List<Descriptor> effectDescriptors = GameUtil.GetEffectDescriptors(allDescriptors);
		if (effectDescriptors.Count > 0)
		{
			list.Add(new CodexText(CODEX.HEADERS.BUILDINGEFFECTS, CodexTextStyle.Subtitle, null));
			foreach (Descriptor descriptor2 in effectDescriptors)
			{
				list.Add(new CodexTextWithTooltip("    " + descriptor2.text, descriptor2.tooltipText, CodexTextStyle.Body));
			}
			list.Add(new CodexSpacer());
		}
		string[] roomClassForObject = CodexEntryGenerator.GetRoomClassForObject(def.BuildingComplete);
		if (roomClassForObject != null)
		{
			list.Add(new CodexText(CODEX.HEADERS.BUILDINGTYPE, CodexTextStyle.Subtitle, null));
			foreach (string str in roomClassForObject)
			{
				list.Add(new CodexText("    " + str, CodexTextStyle.Body, null));
			}
			list.Add(new CodexSpacer());
		}
		list.Add(new CodexText("<i>" + Strings.Get("STRINGS.BUILDINGS.PREFABS." + def.PrefabID.ToUpper() + ".DESC") + "</i>", CodexTextStyle.Body, null));
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x060054A5 RID: 21669 RVA: 0x001EB720 File Offset: 0x001E9920
	public static string[] GetRoomClassForObject(GameObject obj)
	{
		List<string> list = new List<string>();
		KPrefabID component = obj.GetComponent<KPrefabID>();
		if (component != null)
		{
			foreach (Tag key in component.Tags)
			{
				if (CodexEntryGenerator.room_constraint_to_building_label_dict.ContainsKey(key))
				{
					list.Add(CodexEntryGenerator.room_constraint_to_building_label_dict[key]);
				}
			}
		}
		if (list.Count <= 0)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x060054A6 RID: 21670 RVA: 0x001EB7B4 File Offset: 0x001E99B4
	public static void GenerateImageContainers(Sprite[] sprites, List<ContentContainer> containers, ContentContainer.ContentLayout layout)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		foreach (Sprite sprite in sprites)
		{
			if (!(sprite == null))
			{
				CodexImage item = new CodexImage(128, 128, sprite);
				list.Add(item);
			}
		}
		containers.Add(new ContentContainer(list, layout));
	}

	// Token: 0x060054A7 RID: 21671 RVA: 0x001EB80C File Offset: 0x001E9A0C
	public static void GenerateImageContainers(global::Tuple<Sprite, Color>[] sprites, List<ContentContainer> containers, ContentContainer.ContentLayout layout)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		foreach (global::Tuple<Sprite, Color> tuple in sprites)
		{
			if (tuple != null)
			{
				CodexImage item = new CodexImage(128, 128, tuple);
				list.Add(item);
			}
		}
		containers.Add(new ContentContainer(list, layout));
	}

	// Token: 0x060054A8 RID: 21672 RVA: 0x001EB860 File Offset: 0x001E9A60
	public static void GenerateImageContainers(Sprite sprite, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		CodexImage item = new CodexImage(128, 128, sprite);
		list.Add(item);
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x060054A9 RID: 21673 RVA: 0x001EB898 File Offset: 0x001E9A98
	public static void CreateUnlockablesContentContainer(SubEntry subentry)
	{
		subentry.lockedContentContainer = new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(CODEX.HEADERS.SECTION_UNLOCKABLES, CodexTextStyle.Subtitle, null),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical)
		{
			showBeforeGeneratedContent = false
		};
	}

	// Token: 0x060054AA RID: 21674 RVA: 0x001EB8E4 File Offset: 0x001E9AE4
	private static void GenerateFabricatorContainers(GameObject entity, List<ContentContainer> containers)
	{
		ComplexFabricator component = entity.GetComponent<ComplexFabricator>();
		if (component == null)
		{
			return;
		}
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexSpacer(),
			new CodexText(Strings.Get("STRINGS.CODEX.HEADERS.FABRICATIONS"), CodexTextStyle.Subtitle, null),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical));
		List<ICodexWidget> list = new List<ICodexWidget>();
		foreach (ComplexRecipe recipe in component.GetRecipes())
		{
			list.Add(new CodexRecipePanel(recipe, false));
		}
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x060054AB RID: 21675 RVA: 0x001EB988 File Offset: 0x001E9B88
	private static void GenerateConfigurableConsumerContainers(GameObject buildingComplete, List<ContentContainer> containers)
	{
		IConfigurableConsumer component = buildingComplete.GetComponent<IConfigurableConsumer>();
		if (component == null)
		{
			return;
		}
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexSpacer(),
			new CodexText(Strings.Get("STRINGS.CODEX.HEADERS.FABRICATIONS"), CodexTextStyle.Subtitle, null),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical));
		List<ICodexWidget> list = new List<ICodexWidget>();
		foreach (IConfigurableConsumerOption data in component.GetSettingOptions())
		{
			list.Add(new CodexConfigurableConsumerRecipePanel(data));
		}
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x060054AC RID: 21676 RVA: 0x001EBA24 File Offset: 0x001E9C24
	private static void GenerateReceptacleContainers(GameObject entity, List<ContentContainer> containers)
	{
		SingleEntityReceptacle plot = entity.GetComponent<SingleEntityReceptacle>();
		if (plot == null)
		{
			return;
		}
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(Strings.Get("STRINGS.CODEX.HEADERS.RECEPTACLE"), CodexTextStyle.Subtitle, null),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical));
		Predicate<GameObject> <>9__0;
		foreach (Tag tag in plot.possibleDepositObjectTags)
		{
			List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(tag);
			if (plot.rotatable == null)
			{
				List<GameObject> list = prefabsWithTag;
				Predicate<GameObject> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = delegate(GameObject go)
					{
						IReceptacleDirection component = go.GetComponent<IReceptacleDirection>();
						return component != null && component.Direction != plot.Direction;
					});
				}
				list.RemoveAll(match);
			}
			foreach (GameObject gameObject in prefabsWithTag)
			{
				containers.Add(new ContentContainer(new List<ICodexWidget>
				{
					new CodexImage(64, 64, Def.GetUISprite(gameObject, "ui", false).first),
					new CodexText(gameObject.GetProperName(), CodexTextStyle.Body, null)
				}, ContentContainer.ContentLayout.Horizontal));
			}
		}
	}

	// Token: 0x060054AD RID: 21677 RVA: 0x001EBB94 File Offset: 0x001E9D94
	// Note: this type is marked as 'beforefieldinit'.
	static CodexEntryGenerator()
	{
		Dictionary<Tag, string> dictionary = new Dictionary<Tag, string>();
		Tag industrialMachinery = RoomConstraints.ConstraintTags.IndustrialMachinery;
		dictionary[industrialMachinery] = CODEX.BUILDING_TYPE.INDUSTRIAL_MACHINERY;
		Tag recBuilding = RoomConstraints.ConstraintTags.RecBuilding;
		dictionary[recBuilding] = ROOMS.CRITERIA.REC_BUILDING.NAME;
		Tag clinic = RoomConstraints.ConstraintTags.Clinic;
		dictionary[clinic] = ROOMS.CRITERIA.CLINIC.NAME;
		Tag washStation = RoomConstraints.ConstraintTags.WashStation;
		dictionary[washStation] = ROOMS.CRITERIA.WASH_STATION.NAME;
		Tag advancedWashStation = RoomConstraints.ConstraintTags.AdvancedWashStation;
		dictionary[advancedWashStation] = ROOMS.CRITERIA.ADVANCED_WASH_STATION.NAME;
		Tag toiletType = RoomConstraints.ConstraintTags.ToiletType;
		dictionary[toiletType] = ROOMS.CRITERIA.TOILET.NAME;
		Tag flushToiletType = RoomConstraints.ConstraintTags.FlushToiletType;
		dictionary[flushToiletType] = ROOMS.CRITERIA.FLUSH_TOILET.NAME;
		Tag scienceBuilding = RoomConstraints.ConstraintTags.ScienceBuilding;
		dictionary[scienceBuilding] = ROOMS.CRITERIA.SCIENCE_BUILDING.NAME;
		Tag decoration = GameTags.Decoration;
		dictionary[decoration] = ROOMS.CRITERIA.DECOR_ITEM_CLASS;
		CodexEntryGenerator.room_constraint_to_building_label_dict = dictionary;
	}

	// Token: 0x04003870 RID: 14448
	private static string categoryPrefx = "BUILD_CATEGORY_";

	// Token: 0x04003871 RID: 14449
	public static readonly string FOOD_CATEGORY_ID = CodexCache.FormatLinkID("FOOD");

	// Token: 0x04003872 RID: 14450
	public static readonly string FOOD_EFFECTS_ENTRY_ID = CodexCache.FormatLinkID("id_food_effects");

	// Token: 0x04003873 RID: 14451
	public static readonly string TABLE_SALT_ENTRY_ID = CodexCache.FormatLinkID("id_table_salt");

	// Token: 0x04003874 RID: 14452
	public static readonly string MINION_MODIFIERS_CATEGORY_ID = CodexCache.FormatLinkID("MINION_MODIFIERS");

	// Token: 0x04003875 RID: 14453
	public static Dictionary<Tag, string> room_constraint_to_building_label_dict;

	// Token: 0x020019D2 RID: 6610
	private class ConversionEntry
	{
		// Token: 0x04007776 RID: 30582
		public string title;

		// Token: 0x04007777 RID: 30583
		public GameObject prefab;

		// Token: 0x04007778 RID: 30584
		public HashSet<ElementUsage> inSet = new HashSet<ElementUsage>();

		// Token: 0x04007779 RID: 30585
		public HashSet<ElementUsage> outSet = new HashSet<ElementUsage>();
	}

	// Token: 0x020019D3 RID: 6611
	private class CodexElementMap
	{
		// Token: 0x06009564 RID: 38244 RVA: 0x00339B48 File Offset: 0x00337D48
		public void Add(Tag t, CodexEntryGenerator.ConversionEntry ce)
		{
			List<CodexEntryGenerator.ConversionEntry> list;
			if (this.map.TryGetValue(t, out list))
			{
				list.Add(ce);
				return;
			}
			this.map[t] = new List<CodexEntryGenerator.ConversionEntry>
			{
				ce
			};
		}

		// Token: 0x0400777A RID: 30586
		public Dictionary<Tag, List<CodexEntryGenerator.ConversionEntry>> map = new Dictionary<Tag, List<CodexEntryGenerator.ConversionEntry>>();
	}
}
