﻿using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AAD RID: 2733
public class BuildMenu : KScreen
{
	// Token: 0x06005371 RID: 21361 RVA: 0x001DF116 File Offset: 0x001DD316
	public override float GetSortKey()
	{
		return 6f;
	}

	// Token: 0x17000608 RID: 1544
	// (get) Token: 0x06005372 RID: 21362 RVA: 0x001DF11D File Offset: 0x001DD31D
	// (set) Token: 0x06005373 RID: 21363 RVA: 0x001DF124 File Offset: 0x001DD324
	public static BuildMenu Instance { get; private set; }

	// Token: 0x06005374 RID: 21364 RVA: 0x001DF12C File Offset: 0x001DD32C
	public static void DestroyInstance()
	{
		BuildMenu.Instance = null;
	}

	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x06005375 RID: 21365 RVA: 0x001DF134 File Offset: 0x001DD334
	public BuildingDef SelectedBuildingDef
	{
		get
		{
			return this.selectedBuilding;
		}
	}

	// Token: 0x06005376 RID: 21366 RVA: 0x001DF13C File Offset: 0x001DD33C
	private static HashedString CacheHashString(string str)
	{
		return HashCache.Get().Add(str);
	}

	// Token: 0x06005377 RID: 21367 RVA: 0x001DF149 File Offset: 0x001DD349
	public static bool UseHotkeyBuildMenu()
	{
		return KPlayerPrefs.GetInt("ENABLE_HOTKEY_BUILD_MENU") != 0;
	}

	// Token: 0x06005378 RID: 21368 RVA: 0x001DF158 File Offset: 0x001DD358
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		this.initTime = KTime.Instance.UnscaledGameTime;
		bool flag = BuildMenu.UseHotkeyBuildMenu();
		if (flag)
		{
			BuildMenu.Instance = this;
			this.productInfoScreen = global::Util.KInstantiateUI<ProductInfoScreen>(this.productInfoScreenPrefab, base.gameObject, true);
			this.productInfoScreen.rectTransform().pivot = new Vector2(0f, 0f);
			this.productInfoScreen.onElementsFullySelected = new System.Action(this.OnRecipeElementsFullySelected);
			this.productInfoScreen.Show(false);
			this.buildingsScreen = global::Util.KInstantiateUI<BuildMenuBuildingsScreen>(this.buildingsMenuPrefab.gameObject, base.gameObject, true);
			BuildMenuBuildingsScreen buildMenuBuildingsScreen = this.buildingsScreen;
			buildMenuBuildingsScreen.onBuildingSelected = (Action<BuildingDef>)Delegate.Combine(buildMenuBuildingsScreen.onBuildingSelected, new Action<BuildingDef>(this.OnBuildingSelected));
			this.buildingsScreen.Show(false);
			Game.Instance.Subscribe(288942073, new Action<object>(this.OnUIClear));
			Game.Instance.Subscribe(-1190690038, new Action<object>(this.OnBuildToolDeactivated));
			this.Initialize();
			this.rectTransform().anchoredPosition = Vector2.zero;
			return;
		}
		base.gameObject.SetActive(flag);
	}

	// Token: 0x06005379 RID: 21369 RVA: 0x001DF29C File Offset: 0x001DD49C
	private void Initialize()
	{
		foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> keyValuePair in this.submenus)
		{
			BuildMenuCategoriesScreen value = keyValuePair.Value;
			value.Close();
			UnityEngine.Object.DestroyImmediate(value.gameObject);
		}
		this.submenuStack.Clear();
		this.tagCategoryMap = new Dictionary<Tag, HashedString>();
		this.tagOrderMap = new Dictionary<Tag, int>();
		this.categorizedBuildingMap = new Dictionary<HashedString, List<BuildingDef>>();
		this.categorizedCategoryMap = new Dictionary<HashedString, List<HashedString>>();
		int num = 0;
		BuildMenu.DisplayInfo orderedBuildings = BuildMenu.OrderedBuildings;
		this.PopulateCategorizedMaps(orderedBuildings.category, 0, orderedBuildings.data, this.tagCategoryMap, this.tagOrderMap, ref num, this.categorizedBuildingMap, this.categorizedCategoryMap);
		BuildMenuCategoriesScreen buildMenuCategoriesScreen = this.submenus[BuildMenu.ROOT_HASHSTR];
		buildMenuCategoriesScreen.Show(true);
		buildMenuCategoriesScreen.modalKeyInputBehaviour = false;
		foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> keyValuePair2 in this.submenus)
		{
			HashedString key = keyValuePair2.Key;
			List<HashedString> list;
			if (!(key == BuildMenu.ROOT_HASHSTR) && this.categorizedCategoryMap.TryGetValue(key, out list))
			{
				Image component = keyValuePair2.Value.GetComponent<Image>();
				if (component != null)
				{
					component.enabled = (list.Count > 0);
				}
			}
		}
		this.PositionMenus();
	}

	// Token: 0x0600537A RID: 21370 RVA: 0x001DF41C File Offset: 0x001DD61C
	[ContextMenu("PositionMenus")]
	private void PositionMenus()
	{
		foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> keyValuePair in this.submenus)
		{
			HashedString key = keyValuePair.Key;
			BuildMenuCategoriesScreen value = keyValuePair.Value;
			LayoutGroup component = value.GetComponent<LayoutGroup>();
			Vector2 anchoredPosition;
			BuildMenu.PadInfo padInfo;
			if (key == BuildMenu.ROOT_HASHSTR)
			{
				anchoredPosition = this.rootMenuOffset;
				padInfo = this.rootMenuPadding;
				value.GetComponent<Image>().enabled = false;
			}
			else
			{
				anchoredPosition = this.nestedMenuOffset;
				padInfo = this.nestedMenuPadding;
			}
			value.rectTransform().anchoredPosition = anchoredPosition;
			component.padding.left = padInfo.left;
			component.padding.right = padInfo.right;
			component.padding.top = padInfo.top;
			component.padding.bottom = padInfo.bottom;
		}
		this.buildingsScreen.rectTransform().anchoredPosition = this.buildingsMenuOffset;
	}

	// Token: 0x0600537B RID: 21371 RVA: 0x001DF52C File Offset: 0x001DD72C
	public void Refresh()
	{
		foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> keyValuePair in this.submenus)
		{
			keyValuePair.Value.UpdateBuildableStates(true);
		}
	}

	// Token: 0x0600537C RID: 21372 RVA: 0x001DF588 File Offset: 0x001DD788
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
	}

	// Token: 0x0600537D RID: 21373 RVA: 0x001DF5AC File Offset: 0x001DD7AC
	protected override void OnCmpDisable()
	{
		Game.Instance.Unsubscribe(-107300940, new Action<object>(this.OnResearchComplete));
		base.OnCmpDisable();
	}

	// Token: 0x0600537E RID: 21374 RVA: 0x001DF5D0 File Offset: 0x001DD7D0
	private BuildMenuCategoriesScreen CreateCategorySubMenu(HashedString category, int depth, object data, Dictionary<HashedString, List<BuildingDef>> categorized_building_map, Dictionary<HashedString, List<HashedString>> categorized_category_map, Dictionary<Tag, HashedString> tag_category_map, BuildMenuBuildingsScreen buildings_screen)
	{
		BuildMenuCategoriesScreen buildMenuCategoriesScreen = global::Util.KInstantiateUI<BuildMenuCategoriesScreen>(this.categoriesMenuPrefab.gameObject, base.gameObject, true);
		buildMenuCategoriesScreen.Show(false);
		buildMenuCategoriesScreen.Configure(category, depth, data, this.categorizedBuildingMap, this.categorizedCategoryMap, this.buildingsScreen);
		buildMenuCategoriesScreen.onCategoryClicked = (Action<HashedString, int>)Delegate.Combine(buildMenuCategoriesScreen.onCategoryClicked, new Action<HashedString, int>(this.OnCategoryClicked));
		buildMenuCategoriesScreen.name = "BuildMenu_" + category.ToString();
		return buildMenuCategoriesScreen;
	}

	// Token: 0x0600537F RID: 21375 RVA: 0x001DF658 File Offset: 0x001DD858
	private void PopulateCategorizedMaps(HashedString category, int depth, object data, Dictionary<Tag, HashedString> category_map, Dictionary<Tag, int> order_map, ref int building_index, Dictionary<HashedString, List<BuildingDef>> categorized_building_map, Dictionary<HashedString, List<HashedString>> categorized_category_map)
	{
		Type type = data.GetType();
		if (type == typeof(BuildMenu.DisplayInfo))
		{
			BuildMenu.DisplayInfo displayInfo = (BuildMenu.DisplayInfo)data;
			List<HashedString> list;
			if (!categorized_category_map.TryGetValue(category, out list))
			{
				list = new List<HashedString>();
				categorized_category_map[category] = list;
			}
			list.Add(displayInfo.category);
			this.PopulateCategorizedMaps(displayInfo.category, depth + 1, displayInfo.data, category_map, order_map, ref building_index, categorized_building_map, categorized_category_map);
		}
		else
		{
			if (typeof(IList<BuildMenu.DisplayInfo>).IsAssignableFrom(type))
			{
				IEnumerable<BuildMenu.DisplayInfo> enumerable = (IList<BuildMenu.DisplayInfo>)data;
				List<HashedString> list2;
				if (!categorized_category_map.TryGetValue(category, out list2))
				{
					list2 = new List<HashedString>();
					categorized_category_map[category] = list2;
				}
				using (IEnumerator<BuildMenu.DisplayInfo> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BuildMenu.DisplayInfo displayInfo2 = enumerator.Current;
						list2.Add(displayInfo2.category);
						this.PopulateCategorizedMaps(displayInfo2.category, depth + 1, displayInfo2.data, category_map, order_map, ref building_index, categorized_building_map, categorized_category_map);
					}
					goto IL_195;
				}
			}
			foreach (BuildMenu.BuildingInfo buildingInfo in ((IList<BuildMenu.BuildingInfo>)data))
			{
				Tag key = new Tag(buildingInfo.id);
				category_map[key] = category;
				order_map[key] = building_index;
				building_index++;
				List<BuildingDef> list3;
				if (!categorized_building_map.TryGetValue(category, out list3))
				{
					list3 = new List<BuildingDef>();
					categorized_building_map[category] = list3;
				}
				BuildingDef buildingDef = Assets.GetBuildingDef(buildingInfo.id);
				buildingDef.HotKey = buildingInfo.hotkey;
				list3.Add(buildingDef);
			}
		}
		IL_195:
		this.submenus[category] = this.CreateCategorySubMenu(category, depth, data, this.categorizedBuildingMap, this.categorizedCategoryMap, this.tagCategoryMap, this.buildingsScreen);
	}

	// Token: 0x06005380 RID: 21376 RVA: 0x001DF844 File Offset: 0x001DDA44
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (this.mouseOver && base.ConsumeMouseScroll && !e.TryConsume(global::Action.ZoomIn))
		{
			e.TryConsume(global::Action.ZoomOut);
		}
		if (!e.Consumed && this.selectedCategory.IsValid && e.TryConsume(global::Action.Escape))
		{
			this.OnUIClear(null);
			return;
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06005381 RID: 21377 RVA: 0x001DF8B0 File Offset: 0x001DDAB0
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.selectedCategory.IsValid && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.OnUIClear(null);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06005382 RID: 21378 RVA: 0x001DF900 File Offset: 0x001DDB00
	private void OnUIClear(object data)
	{
		SelectTool.Instance.Activate();
		PlayerController.Instance.ActivateTool(SelectTool.Instance);
		SelectTool.Instance.Select(null, true);
		this.productInfoScreen.materialSelectionPanel.PriorityScreen.ResetPriority();
		this.CloseMenus();
	}

	// Token: 0x06005383 RID: 21379 RVA: 0x001DF94D File Offset: 0x001DDB4D
	private void OnBuildToolDeactivated(object data)
	{
		if (this.updating)
		{
			this.deactivateToolQueued = true;
			return;
		}
		this.CloseMenus();
		this.productInfoScreen.materialSelectionPanel.PriorityScreen.ResetPriority();
	}

	// Token: 0x06005384 RID: 21380 RVA: 0x001DF97C File Offset: 0x001DDB7C
	private void CloseMenus()
	{
		this.productInfoScreen.Close();
		while (this.submenuStack.Count > 0)
		{
			this.submenuStack.Pop().Close();
			this.productInfoScreen.Close();
		}
		this.selectedCategory = HashedString.Invalid;
		this.submenus[BuildMenu.ROOT_HASHSTR].ClearSelection();
	}

	// Token: 0x06005385 RID: 21381 RVA: 0x001DF9DF File Offset: 0x001DDBDF
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		if (this.timeSinceNotificationPing < 8f)
		{
			this.timeSinceNotificationPing += Time.unscaledDeltaTime;
		}
		if (this.timeSinceNotificationPing >= 0.5f)
		{
			this.notificationPingCount = 0;
		}
	}

	// Token: 0x06005386 RID: 21382 RVA: 0x001DFA1C File Offset: 0x001DDC1C
	public void PlayNewBuildingSounds()
	{
		if (KTime.Instance.UnscaledGameTime - this.initTime > 1.5f)
		{
			if (BuildMenu.Instance.timeSinceNotificationPing >= 8f)
			{
				string sound = GlobalAssets.GetSound("NewBuildable_Embellishment", false);
				if (sound != null)
				{
					SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition(), 1f, false));
				}
			}
			string sound2 = GlobalAssets.GetSound("NewBuildable", false);
			if (sound2 != null)
			{
				EventInstance instance = SoundEvent.BeginOneShot(sound2, SoundListenerController.Instance.transform.GetPosition(), 1f, false);
				instance.setParameterByName("playCount", (float)BuildMenu.Instance.notificationPingCount, false);
				SoundEvent.EndOneShot(instance);
			}
		}
		this.timeSinceNotificationPing = 0f;
		this.notificationPingCount++;
	}

	// Token: 0x06005387 RID: 21383 RVA: 0x001DFAEC File Offset: 0x001DDCEC
	public PlanScreen.RequirementsState BuildableState(BuildingDef def)
	{
		PlanScreen.RequirementsState result = PlanScreen.RequirementsState.Complete;
		if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive)
		{
			if (!Db.Get().TechItems.IsTechItemComplete(def.PrefabID))
			{
				result = PlanScreen.RequirementsState.Tech;
			}
			else if (!ProductInfoScreen.MaterialsMet(def.CraftRecipe))
			{
				result = PlanScreen.RequirementsState.Materials;
			}
		}
		return result;
	}

	// Token: 0x06005388 RID: 21384 RVA: 0x001DFB39 File Offset: 0x001DDD39
	private void CloseProductInfoScreen()
	{
		this.productInfoScreen.ClearProduct(true);
		this.productInfoScreen.Show(false);
	}

	// Token: 0x06005389 RID: 21385 RVA: 0x001DFB54 File Offset: 0x001DDD54
	private void Update()
	{
		if (this.deactivateToolQueued)
		{
			this.deactivateToolQueued = false;
			this.OnBuildToolDeactivated(null);
		}
		this.elapsedTime += Time.unscaledDeltaTime;
		if (this.elapsedTime <= this.updateInterval)
		{
			return;
		}
		this.elapsedTime = 0f;
		this.updating = true;
		if (this.productInfoScreen.gameObject.activeSelf)
		{
			this.productInfoScreen.materialSelectionPanel.UpdateResourceToggleValues();
		}
		foreach (KIconToggleMenu kiconToggleMenu in this.submenuStack)
		{
			if (kiconToggleMenu is BuildMenuCategoriesScreen)
			{
				(kiconToggleMenu as BuildMenuCategoriesScreen).UpdateBuildableStates(false);
			}
		}
		this.submenus[BuildMenu.ROOT_HASHSTR].UpdateBuildableStates(false);
		this.updating = false;
	}

	// Token: 0x0600538A RID: 21386 RVA: 0x001DFC3C File Offset: 0x001DDE3C
	private void OnRecipeElementsFullySelected()
	{
		if (this.selectedBuilding == null)
		{
			global::Debug.Log("No def!");
		}
		if (this.selectedBuilding.isKAnimTile && this.selectedBuilding.isUtility)
		{
			IList<Tag> getSelectedElementAsList = this.productInfoScreen.materialSelectionPanel.GetSelectedElementAsList;
			((this.selectedBuilding.BuildingComplete.GetComponent<Wire>() != null) ? WireBuildTool.Instance : UtilityBuildTool.Instance).Activate(this.selectedBuilding, getSelectedElementAsList);
			return;
		}
		BuildTool.Instance.Activate(this.selectedBuilding, this.productInfoScreen.materialSelectionPanel.GetSelectedElementAsList);
	}

	// Token: 0x0600538B RID: 21387 RVA: 0x001DFCE0 File Offset: 0x001DDEE0
	private void OnBuildingSelected(BuildingDef def)
	{
		if (this.selecting)
		{
			return;
		}
		this.selecting = true;
		this.selectedBuilding = def;
		this.buildingsScreen.SetHasFocus(false);
		foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> keyValuePair in this.submenus)
		{
			keyValuePair.Value.SetHasFocus(false);
		}
		ToolMenu.Instance.ClearSelection();
		if (def != null)
		{
			Vector2 anchoredPosition = this.productInfoScreen.rectTransform().anchoredPosition;
			RectTransform rectTransform = this.buildingsScreen.rectTransform();
			anchoredPosition.y = rectTransform.anchoredPosition.y;
			anchoredPosition.x = rectTransform.anchoredPosition.x + rectTransform.sizeDelta.x + 10f;
			this.productInfoScreen.rectTransform().anchoredPosition = anchoredPosition;
			this.productInfoScreen.ClearProduct(false);
			this.productInfoScreen.Show(true);
			this.productInfoScreen.ConfigureScreen(def);
		}
		else
		{
			this.productInfoScreen.Close();
		}
		this.selecting = false;
	}

	// Token: 0x0600538C RID: 21388 RVA: 0x001DFE10 File Offset: 0x001DE010
	private void OnCategoryClicked(HashedString new_category, int depth)
	{
		while (this.submenuStack.Count > depth)
		{
			KIconToggleMenu kiconToggleMenu = this.submenuStack.Pop();
			kiconToggleMenu.ClearSelection();
			kiconToggleMenu.Close();
		}
		this.productInfoScreen.Close();
		if (new_category != this.selectedCategory && new_category.IsValid)
		{
			foreach (KIconToggleMenu kiconToggleMenu2 in this.submenuStack)
			{
				if (kiconToggleMenu2 is BuildMenuCategoriesScreen)
				{
					(kiconToggleMenu2 as BuildMenuCategoriesScreen).SetHasFocus(false);
				}
			}
			this.selectedCategory = new_category;
			BuildMenuCategoriesScreen buildMenuCategoriesScreen;
			this.submenus.TryGetValue(new_category, out buildMenuCategoriesScreen);
			if (buildMenuCategoriesScreen != null)
			{
				buildMenuCategoriesScreen.Show(true);
				buildMenuCategoriesScreen.SetHasFocus(true);
				this.submenuStack.Push(buildMenuCategoriesScreen);
			}
		}
		else
		{
			this.selectedCategory = HashedString.Invalid;
		}
		foreach (KIconToggleMenu kiconToggleMenu3 in this.submenuStack)
		{
			if (kiconToggleMenu3 is BuildMenuCategoriesScreen)
			{
				(kiconToggleMenu3 as BuildMenuCategoriesScreen).UpdateBuildableStates(true);
			}
		}
		this.submenus[BuildMenu.ROOT_HASHSTR].UpdateBuildableStates(true);
	}

	// Token: 0x0600538D RID: 21389 RVA: 0x001DFF68 File Offset: 0x001DE168
	public void RefreshProductInfoScreen(BuildingDef def)
	{
		if (this.productInfoScreen.currentDef == def)
		{
			this.productInfoScreen.ClearProduct(false);
			this.productInfoScreen.Show(true);
			this.productInfoScreen.ConfigureScreen(def);
		}
	}

	// Token: 0x0600538E RID: 21390 RVA: 0x001DFFA4 File Offset: 0x001DE1A4
	private HashedString GetParentCategory(HashedString desired_category)
	{
		foreach (KeyValuePair<HashedString, List<HashedString>> keyValuePair in this.categorizedCategoryMap)
		{
			using (List<HashedString>.Enumerator enumerator2 = keyValuePair.Value.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == desired_category)
					{
						return keyValuePair.Key;
					}
				}
			}
		}
		return HashedString.Invalid;
	}

	// Token: 0x0600538F RID: 21391 RVA: 0x001E0044 File Offset: 0x001DE244
	private void AddParentCategories(HashedString child_category, ICollection<HashedString> categories)
	{
		for (;;)
		{
			HashedString parentCategory = this.GetParentCategory(child_category);
			if (parentCategory == HashedString.Invalid)
			{
				break;
			}
			categories.Add(parentCategory);
			child_category = parentCategory;
		}
	}

	// Token: 0x06005390 RID: 21392 RVA: 0x001E0074 File Offset: 0x001DE274
	private void OnResearchComplete(object data)
	{
		HashSet<HashedString> hashSet = new HashSet<HashedString>();
		Tech tech = (Tech)data;
		foreach (TechItem techItem in tech.unlockedItems)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
			if (buildingDef == null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("Tech '{0}' unlocked building '{1}' but no such building exists", tech.Name, techItem.Id)
				});
			}
			else
			{
				HashedString hashedString = this.tagCategoryMap[buildingDef.Tag];
				hashSet.Add(hashedString);
				this.AddParentCategories(hashedString, hashSet);
			}
		}
		this.UpdateNotifications(hashSet, BuildMenu.OrderedBuildings);
	}

	// Token: 0x06005391 RID: 21393 RVA: 0x001E0144 File Offset: 0x001DE344
	private void UpdateNotifications(ICollection<HashedString> updated_categories, object data)
	{
		foreach (KeyValuePair<HashedString, BuildMenuCategoriesScreen> keyValuePair in this.submenus)
		{
			keyValuePair.Value.UpdateNotifications(updated_categories);
		}
	}

	// Token: 0x06005392 RID: 21394 RVA: 0x001E01A0 File Offset: 0x001DE3A0
	public PrioritySetting GetBuildingPriority()
	{
		return this.productInfoScreen.materialSelectionPanel.PriorityScreen.GetLastSelectedPriority();
	}

	// Token: 0x040037BB RID: 14267
	public const string ENABLE_HOTKEY_BUILD_MENU_KEY = "ENABLE_HOTKEY_BUILD_MENU";

	// Token: 0x040037BC RID: 14268
	[SerializeField]
	private BuildMenuCategoriesScreen categoriesMenuPrefab;

	// Token: 0x040037BD RID: 14269
	[SerializeField]
	private BuildMenuBuildingsScreen buildingsMenuPrefab;

	// Token: 0x040037BE RID: 14270
	[SerializeField]
	private GameObject productInfoScreenPrefab;

	// Token: 0x040037BF RID: 14271
	private ProductInfoScreen productInfoScreen;

	// Token: 0x040037C0 RID: 14272
	private BuildMenuBuildingsScreen buildingsScreen;

	// Token: 0x040037C1 RID: 14273
	private BuildingDef selectedBuilding;

	// Token: 0x040037C2 RID: 14274
	private HashedString selectedCategory;

	// Token: 0x040037C3 RID: 14275
	private static readonly HashedString ROOT_HASHSTR = new HashedString("ROOT");

	// Token: 0x040037C4 RID: 14276
	private Dictionary<HashedString, BuildMenuCategoriesScreen> submenus = new Dictionary<HashedString, BuildMenuCategoriesScreen>();

	// Token: 0x040037C5 RID: 14277
	private Stack<KIconToggleMenu> submenuStack = new Stack<KIconToggleMenu>();

	// Token: 0x040037C6 RID: 14278
	private bool selecting;

	// Token: 0x040037C7 RID: 14279
	private bool updating;

	// Token: 0x040037C8 RID: 14280
	private bool deactivateToolQueued;

	// Token: 0x040037C9 RID: 14281
	[SerializeField]
	private Vector2 rootMenuOffset = Vector2.zero;

	// Token: 0x040037CA RID: 14282
	[SerializeField]
	private BuildMenu.PadInfo rootMenuPadding;

	// Token: 0x040037CB RID: 14283
	[SerializeField]
	private Vector2 nestedMenuOffset = Vector2.zero;

	// Token: 0x040037CC RID: 14284
	[SerializeField]
	private BuildMenu.PadInfo nestedMenuPadding;

	// Token: 0x040037CD RID: 14285
	[SerializeField]
	private Vector2 buildingsMenuOffset = Vector2.zero;

	// Token: 0x040037CE RID: 14286
	public static BuildMenu.DisplayInfo OrderedBuildings = new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("ROOT"), "icon_category_base", global::Action.NumActions, KKeyCode.None, new List<BuildMenu.DisplayInfo>
	{
		new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Base"), "icon_category_base", global::Action.Plan1, KKeyCode.None, new List<BuildMenu.DisplayInfo>
		{
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Tiles"), "icon_category_base", global::Action.BuildCategoryTiles, KKeyCode.T, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Tile", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("GasPermeableMembrane", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("MeshTile", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("InsulationTile", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("PlasticTile", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("MetalTile", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("GlassTile", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("BunkerTile", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("CarpetTile", global::Action.BuildMenuKeyL),
				new BuildMenu.BuildingInfo("ExteriorWall", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("ExobaseHeadquarters", global::Action.BuildMenuKeyH)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Ladders"), "icon_category_base", global::Action.BuildCategoryLadders, KKeyCode.A, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Ladder", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("LadderFast", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("FirePole", global::Action.BuildMenuKeyF)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Doors"), "icon_category_base", global::Action.BuildCategoryDoors, KKeyCode.D, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Door", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("ManualPressureDoor", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("PressureDoor", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("BunkerDoor", global::Action.BuildMenuKeyB)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Storage"), "icon_category_base", global::Action.BuildCategoryStorage, KKeyCode.S, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("StorageLocker", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("RationBox", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("Refrigerator", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("StorageLockerSmart", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("LiquidReservoir", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("GasReservoir", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("ObjectDispenser", global::Action.BuildMenuKeyO)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Research"), "icon_category_misc", global::Action.BuildCategoryResearch, KKeyCode.R, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("ResearchCenter", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("AdvancedResearchCenter", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("CosmicResearchCenter", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("DLC1CosmicResearchCenter", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("NuclearResearchCenter", global::Action.BuildMenuKeyN),
				new BuildMenu.BuildingInfo("Telescope", global::Action.BuildMenuKeyT)
			})
		}),
		new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Food And Agriculture"), "icon_category_food", global::Action.Plan2, KKeyCode.None, new List<BuildMenu.DisplayInfo>
		{
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Farming"), "icon_category_food", global::Action.BuildCategoryFarming, KKeyCode.F, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("PlanterBox", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("FarmTile", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("HydroponicFarm", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("Compost", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("FertilizerMaker", global::Action.BuildMenuKeyR)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Cooking"), "icon_category_food", global::Action.BuildCategoryCooking, KKeyCode.C, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("MicrobeMusher", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("CookingStation", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("SpiceGrinder", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("GourmetCookingStation", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("EggCracker", global::Action.BuildMenuKeyE)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Ranching"), "icon_category_food", global::Action.BuildCategoryRanching, KKeyCode.R, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("CreatureDeliveryPoint", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("FishDeliveryPoint", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("CreatureFeeder", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("FishFeeder", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("RanchStation", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("ShearingStation", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("EggIncubator", global::Action.BuildMenuKeyI),
				new BuildMenu.BuildingInfo("CreatureGroundTrap", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("WaterTrap", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("CreatureAirTrap", global::Action.BuildMenuKeyL)
			})
		}),
		new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Health And Happiness"), "icon_category_medical", global::Action.Plan3, KKeyCode.None, new List<BuildMenu.DisplayInfo>
		{
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Medical"), "icon_category_medical", global::Action.BuildCategoryMedical, KKeyCode.C, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Apothecary", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("DoctorStation", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("AdvancedDoctorStation", global::Action.BuildMenuKeyO),
				new BuildMenu.BuildingInfo("MedicalCot", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("MassageTable", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("Grave", global::Action.BuildMenuKeyR)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Hygiene"), "icon_category_medical", global::Action.BuildCategoryHygiene, KKeyCode.E, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Outhouse", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("FlushToilet", global::Action.BuildMenuKeyV),
				new BuildMenu.BuildingInfo(ShowerConfig.ID, global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("WashBasin", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("WashSink", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("HandSanitizer", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("DecontaminationShower", global::Action.BuildMenuKeyD)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Furniture"), "icon_category_furniture", global::Action.BuildCategoryFurniture, KKeyCode.F, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Bed", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("LuxuryBed", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo(LadderBedConfig.ID, global::Action.BuildMenuKeyL),
				new BuildMenu.BuildingInfo("DiningTable", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("FloorLamp", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("CeilingLight", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("SunLamp", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("RadiationLight", global::Action.BuildMenuKeyR)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Decor"), "icon_category_furniture", global::Action.BuildCategoryDecor, KKeyCode.D, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("FlowerVase", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("Canvas", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("CanvasWide", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("CanvasTall", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("Sculpture", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("IceSculpture", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("ItemPedestal", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("CrownMoulding", global::Action.BuildMenuKeyM),
				new BuildMenu.BuildingInfo("CornerMoulding", global::Action.BuildMenuKeyN)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Recreation"), "icon_category_medical", global::Action.BuildCategoryRecreation, KKeyCode.R, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("WaterCooler", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("ArcadeMachine", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("Phonobox", global::Action.BuildMenuKeyP),
				new BuildMenu.BuildingInfo("EspressoMachine", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("HotTub", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("MechanicalSurfboard", global::Action.BuildMenuKeyM),
				new BuildMenu.BuildingInfo("Sauna", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("BeachChair", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("Juicer", global::Action.BuildMenuKeyJ),
				new BuildMenu.BuildingInfo("SodaFountain", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("VerticalWindTunnel", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("ParkSign", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("Telephone", global::Action.BuildMenuKeyT)
			})
		}),
		new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Infrastructure"), "icon_category_utilities", global::Action.Plan4, KKeyCode.None, new List<BuildMenu.DisplayInfo>
		{
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Wires"), "icon_category_electrical", global::Action.BuildCategoryWires, KKeyCode.W, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Wire", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("WireBridge", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("HighWattageWire", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("WireBridgeHighWattage", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("WireRefined", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("WireRefinedBridge", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("WireRefinedHighWattage", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("WireRefinedBridgeHighWattage", global::Action.BuildMenuKeyA)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Generators"), "icon_category_electrical", global::Action.BuildCategoryGenerators, KKeyCode.G, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("ManualGenerator", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("Generator", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("WoodGasGenerator", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("NuclearReactor", global::Action.BuildMenuKeyN),
				new BuildMenu.BuildingInfo("HydrogenGenerator", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("MethaneGenerator", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("PetroleumGenerator", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("SteamTurbine", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("SteamTurbine2", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("SolarPanel", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("DevGenerator", global::Action.BuildMenuKeyX)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("PowerControl"), "icon_category_electrical", global::Action.BuildCategoryPowerControl, KKeyCode.R, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Battery", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("BatteryMedium", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("BatterySmart", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("PowerTransformerSmall", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("PowerTransformer", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo(SwitchConfig.ID, global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo(TemperatureControlledSwitchConfig.ID, global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo(PressureSwitchLiquidConfig.ID, global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo(PressureSwitchGasConfig.ID, global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo(LogicPowerRelayConfig.ID, global::Action.BuildMenuKeyX)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Pipes"), "icon_category_plumbing", global::Action.BuildCategoryPipes, KKeyCode.E, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("LiquidConduit", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("LiquidConduitBridge", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("InsulatedLiquidConduit", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("LiquidConduitRadiant", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("GasConduit", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("GasConduitBridge", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("InsulatedGasConduit", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("GasConduitRadiant", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("ContactConductivePipeBridge", global::Action.BuildMenuKeyA)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Plumbing Structures"), "icon_category_plumbing", global::Action.BuildCategoryPlumbingStructures, KKeyCode.B, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("LiquidPumpingStation", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("BottleEmptier", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("LiquidPump", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("LiquidMiniPump", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("LiquidValve", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("LiquidLogicValve", global::Action.BuildMenuKeyL),
				new BuildMenu.BuildingInfo("LiquidVent", global::Action.BuildMenuKeyV),
				new BuildMenu.BuildingInfo("LiquidFilter", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("LiquidConduitPreferentialFlow", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("LiquidConduitOverflow", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("LiquidLimitValve", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortLiquid", global::Action.BuildMenuKeyM),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortLiquidUnloader", global::Action.BuildMenuKeyU)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Ventilation Structures"), "icon_category_ventilation", global::Action.BuildCategoryVentilationStructures, KKeyCode.V, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("GasPump", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("GasMiniPump", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("GasValve", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("GasLogicValve", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("GasVent", global::Action.BuildMenuKeyV),
				new BuildMenu.BuildingInfo("GasVentHighPressure", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("GasFilter", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("GasBottler", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("BottleEmptierGas", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("GasConduitPreferentialFlow", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("GasConduitOverflow", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("GasLimitValve", global::Action.BuildMenuKeyL),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortGas", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortGasUnloader", global::Action.BuildMenuKeyU)
			})
		}),
		new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Industrial"), "icon_category_refinery", global::Action.Plan5, KKeyCode.None, new List<BuildMenu.DisplayInfo>
		{
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Oxygen"), "icon_category_oxygen", global::Action.BuildCategoryOxygen, KKeyCode.X, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("MineralDeoxidizer", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("SublimationStation", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("AlgaeHabitat", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("AirFilter", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("CO2Scrubber", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("Electrolyzer", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("RustDeoxidizer", global::Action.BuildMenuKeyF)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Utilities"), "icon_category_utilities", global::Action.BuildCategoryUtilities, KKeyCode.T, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("SpaceHeater", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("LiquidHeater", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("IceCooledFan", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("IceMachine", global::Action.BuildMenuKeyI),
				new BuildMenu.BuildingInfo("AirConditioner", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("LiquidConditioner", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("OreScrubber", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("ThermalBlock", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("HighEnergyParticleRedirector", global::Action.BuildMenuKeyP)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Refining"), "icon_category_refinery", global::Action.BuildCategoryRefining, KKeyCode.R, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("WaterPurifier", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("AlgaeDistillery", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("EthanolDistillery", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("RockCrusher", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("SludgePress", global::Action.BuildMenuKeyP),
				new BuildMenu.BuildingInfo("Kiln", global::Action.BuildMenuKeyZ),
				new BuildMenu.BuildingInfo("OilWellCap", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("OilRefinery", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("Polymerizer", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("MetalRefinery", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("GlassForge", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("OxyliteRefinery", global::Action.BuildMenuKeyO),
				new BuildMenu.BuildingInfo("SupermaterialRefinery", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("UraniumCentrifuge", global::Action.BuildMenuKeyU)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Equipment"), "icon_category_misc", global::Action.BuildCategoryEquipment, KKeyCode.S, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("RoleStation", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("FarmStation", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo("PowerControlStation", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("AstronautTrainingCenter", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("ResetSkillsStation", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("CraftingTable", global::Action.BuildMenuKeyZ),
				new BuildMenu.BuildingInfo("OxygenMaskMarker", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("OxygenMaskLocker", global::Action.BuildMenuKeyY),
				new BuildMenu.BuildingInfo("ClothingFabricator", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("SuitFabricator", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("SuitMarker", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("SuitLocker", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("JetSuitMarker", global::Action.BuildMenuKeyJ),
				new BuildMenu.BuildingInfo("JetSuitLocker", global::Action.BuildMenuKeyO),
				new BuildMenu.BuildingInfo("LeadSuitMarker", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("LeadSuitLocker", global::Action.BuildMenuKeyD)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Rocketry"), "icon_category_rocketry", global::Action.BuildCategoryRocketry, KKeyCode.C, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("Gantry", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("KeroseneEngine", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("SolidBooster", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("SteamEngine", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("LiquidFuelTank", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("CargoBay", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("GasCargoBay", global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo("LiquidCargoBay", global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo("SpecialCargoBay", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("SpecialCargoBayCluster", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("CommandModule", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("TouristModule", global::Action.BuildMenuKeyY),
				new BuildMenu.BuildingInfo("ResearchModule", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("HydrogenEngine", global::Action.BuildMenuKeyH),
				new BuildMenu.BuildingInfo("RailGun", global::Action.BuildMenuKeyP),
				new BuildMenu.BuildingInfo("LandingBeacon", global::Action.BuildMenuKeyL)
			})
		}),
		new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Logistics"), "icon_category_ventilation", global::Action.Plan6, KKeyCode.None, new List<BuildMenu.DisplayInfo>
		{
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("TravelTubes"), "icon_category_ventilation", global::Action.BuildCategoryTravelTubes, KKeyCode.T, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("TravelTube", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("TravelTubeEntrance", global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo("TravelTubeWallBridge", global::Action.BuildMenuKeyB)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("Conveyance"), "icon_category_ventilation", global::Action.BuildCategoryConveyance, KKeyCode.C, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("SolidTransferArm", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("SolidConduit", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo("SolidConduitInbox", global::Action.BuildMenuKeyI),
				new BuildMenu.BuildingInfo("SolidConduitOutbox", global::Action.BuildMenuKeyO),
				new BuildMenu.BuildingInfo("SolidVent", global::Action.BuildMenuKeyV),
				new BuildMenu.BuildingInfo("SolidLogicValve", global::Action.BuildMenuKeyL),
				new BuildMenu.BuildingInfo("SolidLimitValve", global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo("SolidConduitBridge", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("AutoMiner", global::Action.BuildMenuKeyM),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortSolid", global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo("ModularLaunchpadPortSolidUnloader", global::Action.BuildMenuKeyU)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("LogicWiring"), "icon_category_automation", global::Action.BuildCategoryLogicWiring, KKeyCode.W, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("LogicWire", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("LogicWireBridge", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("LogicRibbon", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("LogicRibbonBridge", global::Action.BuildMenuKeyV)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("LogicGates"), "icon_category_automation", global::Action.BuildCategoryLogicGates, KKeyCode.G, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo("LogicGateAND", global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo("LogicGateOR", global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("LogicGateXOR", global::Action.BuildMenuKeyX),
				new BuildMenu.BuildingInfo("LogicGateNOT", global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo("LogicGateBUFFER", global::Action.BuildMenuKeyB),
				new BuildMenu.BuildingInfo("LogicGateFILTER", global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo(LogicMemoryConfig.ID, global::Action.BuildMenuKeyV)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("LogicSwitches"), "icon_category_automation", global::Action.BuildCategoryLogicSwitches, KKeyCode.S, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo(LogicSwitchConfig.ID, global::Action.BuildMenuKeyS),
				new BuildMenu.BuildingInfo(LogicPressureSensorGasConfig.ID, global::Action.BuildMenuKeyA),
				new BuildMenu.BuildingInfo(LogicPressureSensorLiquidConfig.ID, global::Action.BuildMenuKeyQ),
				new BuildMenu.BuildingInfo(LogicTemperatureSensorConfig.ID, global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo(LogicTimeOfDaySensorConfig.ID, global::Action.BuildMenuKeyD),
				new BuildMenu.BuildingInfo(LogicTimerSensorConfig.ID, global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo(LogicCritterCountSensorConfig.ID, global::Action.BuildMenuKeyV),
				new BuildMenu.BuildingInfo(LogicDiseaseSensorConfig.ID, global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo(LogicElementSensorGasConfig.ID, global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo(LogicWattageSensorConfig.ID, global::Action.BuildMenuKeyP),
				new BuildMenu.BuildingInfo("FloorSwitch", global::Action.BuildMenuKeyW),
				new BuildMenu.BuildingInfo("Checkpoint", global::Action.BuildMenuKeyC),
				new BuildMenu.BuildingInfo(CometDetectorConfig.ID, global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo("LogicDuplicantSensor", global::Action.BuildMenuKeyF)
			}),
			new BuildMenu.DisplayInfo(BuildMenu.CacheHashString("ConduitSensors"), "icon_category_automation", global::Action.BuildCategoryLogicConduits, KKeyCode.X, new List<BuildMenu.BuildingInfo>
			{
				new BuildMenu.BuildingInfo(LiquidConduitTemperatureSensorConfig.ID, global::Action.BuildMenuKeyT),
				new BuildMenu.BuildingInfo(LiquidConduitDiseaseSensorConfig.ID, global::Action.BuildMenuKeyG),
				new BuildMenu.BuildingInfo(LiquidConduitElementSensorConfig.ID, global::Action.BuildMenuKeyE),
				new BuildMenu.BuildingInfo(GasConduitTemperatureSensorConfig.ID, global::Action.BuildMenuKeyR),
				new BuildMenu.BuildingInfo(GasConduitDiseaseSensorConfig.ID, global::Action.BuildMenuKeyF),
				new BuildMenu.BuildingInfo(GasConduitElementSensorConfig.ID, global::Action.BuildMenuKeyS)
			})
		})
	});

	// Token: 0x040037CF RID: 14287
	private Dictionary<HashedString, List<BuildingDef>> categorizedBuildingMap;

	// Token: 0x040037D0 RID: 14288
	private Dictionary<HashedString, List<HashedString>> categorizedCategoryMap;

	// Token: 0x040037D1 RID: 14289
	private Dictionary<Tag, HashedString> tagCategoryMap;

	// Token: 0x040037D2 RID: 14290
	private Dictionary<Tag, int> tagOrderMap;

	// Token: 0x040037D3 RID: 14291
	private const float NotificationPingExpire = 0.5f;

	// Token: 0x040037D4 RID: 14292
	private const float SpecialNotificationEmbellishDelay = 8f;

	// Token: 0x040037D5 RID: 14293
	private float timeSinceNotificationPing;

	// Token: 0x040037D6 RID: 14294
	private int notificationPingCount;

	// Token: 0x040037D7 RID: 14295
	private float initTime;

	// Token: 0x040037D8 RID: 14296
	private float updateInterval = 1f;

	// Token: 0x040037D9 RID: 14297
	private float elapsedTime;

	// Token: 0x020019C1 RID: 6593
	[Serializable]
	private struct PadInfo
	{
		// Token: 0x0400773E RID: 30526
		public int left;

		// Token: 0x0400773F RID: 30527
		public int right;

		// Token: 0x04007740 RID: 30528
		public int top;

		// Token: 0x04007741 RID: 30529
		public int bottom;
	}

	// Token: 0x020019C2 RID: 6594
	public struct BuildingInfo
	{
		// Token: 0x0600953A RID: 38202 RVA: 0x003395A2 File Offset: 0x003377A2
		public BuildingInfo(string id, global::Action hotkey)
		{
			this.id = id;
			this.hotkey = hotkey;
		}

		// Token: 0x04007742 RID: 30530
		public string id;

		// Token: 0x04007743 RID: 30531
		public global::Action hotkey;
	}

	// Token: 0x020019C3 RID: 6595
	public struct DisplayInfo
	{
		// Token: 0x0600953B RID: 38203 RVA: 0x003395B2 File Offset: 0x003377B2
		public DisplayInfo(HashedString category, string icon_name, global::Action hotkey, KKeyCode key_code, object data)
		{
			this.category = category;
			this.iconName = icon_name;
			this.hotkey = hotkey;
			this.keyCode = key_code;
			this.data = data;
		}

		// Token: 0x0600953C RID: 38204 RVA: 0x003395DC File Offset: 0x003377DC
		public BuildMenu.DisplayInfo GetInfo(HashedString category)
		{
			BuildMenu.DisplayInfo displayInfo = default(BuildMenu.DisplayInfo);
			if (this.data != null && typeof(IList<BuildMenu.DisplayInfo>).IsAssignableFrom(this.data.GetType()))
			{
				foreach (BuildMenu.DisplayInfo displayInfo2 in ((IList<BuildMenu.DisplayInfo>)this.data))
				{
					displayInfo = displayInfo2.GetInfo(category);
					if (displayInfo.category == category)
					{
						break;
					}
					if (displayInfo2.category == category)
					{
						displayInfo = displayInfo2;
						break;
					}
				}
			}
			return displayInfo;
		}

		// Token: 0x04007744 RID: 30532
		public HashedString category;

		// Token: 0x04007745 RID: 30533
		public string iconName;

		// Token: 0x04007746 RID: 30534
		public global::Action hotkey;

		// Token: 0x04007747 RID: 30535
		public KKeyCode keyCode;

		// Token: 0x04007748 RID: 30536
		public object data;
	}
}
