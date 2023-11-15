using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A53 RID: 2643
public class BuildMenuCategoriesScreen : KIconToggleMenu
{
	// Token: 0x06004FA4 RID: 20388 RVA: 0x001C24AB File Offset: 0x001C06AB
	public override float GetSortKey()
	{
		return 7f;
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x06004FA5 RID: 20389 RVA: 0x001C24B2 File Offset: 0x001C06B2
	public HashedString Category
	{
		get
		{
			return this.category;
		}
	}

	// Token: 0x06004FA6 RID: 20390 RVA: 0x001C24BA File Offset: 0x001C06BA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.onSelect += this.OnClickCategory;
	}

	// Token: 0x06004FA7 RID: 20391 RVA: 0x001C24D4 File Offset: 0x001C06D4
	public void Configure(HashedString category, int depth, object data, Dictionary<HashedString, List<BuildingDef>> categorized_building_map, Dictionary<HashedString, List<HashedString>> categorized_category_map, BuildMenuBuildingsScreen buildings_screen)
	{
		this.category = category;
		this.categorizedBuildingMap = categorized_building_map;
		this.categorizedCategoryMap = categorized_category_map;
		this.buildingsScreen = buildings_screen;
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		if (typeof(IList<BuildMenu.BuildingInfo>).IsAssignableFrom(data.GetType()))
		{
			this.buildingInfos = (IList<BuildMenu.BuildingInfo>)data;
		}
		else if (typeof(IList<BuildMenu.DisplayInfo>).IsAssignableFrom(data.GetType()))
		{
			this.subcategories = new List<HashedString>();
			foreach (BuildMenu.DisplayInfo displayInfo in ((IList<BuildMenu.DisplayInfo>)data))
			{
				string iconName = displayInfo.iconName;
				string text = HashCache.Get().Get(displayInfo.category).ToUpper();
				text = text.Replace(" ", "");
				KIconToggleMenu.ToggleInfo item = new KIconToggleMenu.ToggleInfo(Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + text + ".NAME"), iconName, new BuildMenuCategoriesScreen.UserData
				{
					category = displayInfo.category,
					depth = depth,
					requirementsState = PlanScreen.RequirementsState.Tech
				}, displayInfo.hotkey, Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + text + ".TOOLTIP"), "");
				list.Add(item);
				this.subcategories.Add(displayInfo.category);
			}
			base.Setup(list);
			this.toggles.ForEach(delegate(KToggle to)
			{
				foreach (ImageToggleState imageToggleState in to.GetComponents<ImageToggleState>())
				{
					if (imageToggleState.TargetImage.sprite != null && imageToggleState.TargetImage.name == "FG" && !imageToggleState.useSprites)
					{
						imageToggleState.SetSprites(Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"), imageToggleState.TargetImage.sprite, imageToggleState.TargetImage.sprite, Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"));
					}
				}
				to.GetComponent<KToggle>().soundPlayer.Enabled = false;
			});
		}
		this.UpdateBuildableStates(true);
	}

	// Token: 0x06004FA8 RID: 20392 RVA: 0x001C267C File Offset: 0x001C087C
	private void OnClickCategory(KIconToggleMenu.ToggleInfo toggle_info)
	{
		BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData)toggle_info.userData;
		PlanScreen.RequirementsState requirementsState = userData.requirementsState;
		if (requirementsState - PlanScreen.RequirementsState.Materials <= 1)
		{
			if (this.selectedCategory != userData.category)
			{
				this.selectedCategory = userData.category;
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
			}
			else
			{
				this.selectedCategory = HashedString.Invalid;
				this.ClearSelection();
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
			}
		}
		else
		{
			this.selectedCategory = HashedString.Invalid;
			this.ClearSelection();
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		toggle_info.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
		if (this.onCategoryClicked != null)
		{
			this.onCategoryClicked(this.selectedCategory, userData.depth);
		}
	}

	// Token: 0x06004FA9 RID: 20393 RVA: 0x001C2748 File Offset: 0x001C0948
	private void UpdateButtonStates()
	{
		if (this.toggleInfo != null && this.toggleInfo.Count > 0)
		{
			foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
			{
				BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData)toggleInfo.userData;
				HashedString x = userData.category;
				PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(x);
				bool flag = categoryRequirements == PlanScreen.RequirementsState.Tech;
				toggleInfo.toggle.gameObject.SetActive(!flag);
				if (categoryRequirements != PlanScreen.RequirementsState.Materials)
				{
					if (categoryRequirements == PlanScreen.RequirementsState.Complete)
					{
						ImageToggleState.State state = (!this.selectedCategory.IsValid || x != this.selectedCategory) ? ImageToggleState.State.Inactive : ImageToggleState.State.Active;
						if (userData.currentToggleState == null || userData.currentToggleState.GetValueOrDefault() != state)
						{
							userData.currentToggleState = new ImageToggleState.State?(state);
							this.SetImageToggleState(toggleInfo.toggle.gameObject, state);
						}
					}
				}
				else
				{
					toggleInfo.toggle.fgImage.SetAlpha(flag ? 0.2509804f : 1f);
					ImageToggleState.State state2 = (this.selectedCategory.IsValid && x == this.selectedCategory) ? ImageToggleState.State.DisabledActive : ImageToggleState.State.Disabled;
					if (userData.currentToggleState == null || userData.currentToggleState.GetValueOrDefault() != state2)
					{
						userData.currentToggleState = new ImageToggleState.State?(state2);
						this.SetImageToggleState(toggleInfo.toggle.gameObject, state2);
					}
				}
				toggleInfo.toggle.fgImage.transform.Find("ResearchIcon").gameObject.gameObject.SetActive(flag);
			}
		}
	}

	// Token: 0x06004FAA RID: 20394 RVA: 0x001C290C File Offset: 0x001C0B0C
	private void SetImageToggleState(GameObject target, ImageToggleState.State state)
	{
		ImageToggleState[] components = target.GetComponents<ImageToggleState>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].SetState(state);
		}
	}

	// Token: 0x06004FAB RID: 20395 RVA: 0x001C2938 File Offset: 0x001C0B38
	private PlanScreen.RequirementsState GetCategoryRequirements(HashedString category)
	{
		bool flag = true;
		bool flag2 = true;
		List<BuildingDef> list;
		if (this.categorizedBuildingMap.TryGetValue(category, out list))
		{
			if (list.Count <= 0)
			{
				goto IL_F3;
			}
			using (List<BuildingDef>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BuildingDef buildingDef = enumerator.Current;
					if (buildingDef.ShowInBuildMenu && buildingDef.IsAvailable())
					{
						PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(buildingDef);
						flag = (flag && requirementsState == PlanScreen.RequirementsState.Tech);
						flag2 = (flag2 && (requirementsState == PlanScreen.RequirementsState.Materials || requirementsState == PlanScreen.RequirementsState.Tech));
					}
				}
				goto IL_F3;
			}
		}
		List<HashedString> list2;
		if (this.categorizedCategoryMap.TryGetValue(category, out list2))
		{
			foreach (HashedString hashedString in list2)
			{
				PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(hashedString);
				flag = (flag && categoryRequirements == PlanScreen.RequirementsState.Tech);
				flag2 = (flag2 && (categoryRequirements == PlanScreen.RequirementsState.Materials || categoryRequirements == PlanScreen.RequirementsState.Tech));
			}
		}
		IL_F3:
		PlanScreen.RequirementsState result;
		if (flag)
		{
			result = PlanScreen.RequirementsState.Tech;
		}
		else if (flag2)
		{
			result = PlanScreen.RequirementsState.Materials;
		}
		else
		{
			result = PlanScreen.RequirementsState.Complete;
		}
		if (DebugHandler.InstantBuildMode)
		{
			result = PlanScreen.RequirementsState.Complete;
		}
		return result;
	}

	// Token: 0x06004FAC RID: 20396 RVA: 0x001C2A70 File Offset: 0x001C0C70
	public void UpdateNotifications(ICollection<HashedString> updated_categories)
	{
		if (this.toggleInfo == null)
		{
			return;
		}
		this.UpdateBuildableStates(false);
		foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
		{
			HashedString item = ((BuildMenuCategoriesScreen.UserData)toggleInfo.userData).category;
			if (updated_categories.Contains(item))
			{
				toggleInfo.toggle.gameObject.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
			}
		}
	}

	// Token: 0x06004FAD RID: 20397 RVA: 0x001C2AF8 File Offset: 0x001C0CF8
	public override void Close()
	{
		base.Close();
		this.selectedCategory = HashedString.Invalid;
		this.SetHasFocus(false);
		if (this.buildingInfos != null)
		{
			this.buildingsScreen.Close();
		}
	}

	// Token: 0x06004FAE RID: 20398 RVA: 0x001C2B25 File Offset: 0x001C0D25
	[ContextMenu("ForceUpdateBuildableStates")]
	private void ForceUpdateBuildableStates()
	{
		this.UpdateBuildableStates(true);
	}

	// Token: 0x06004FAF RID: 20399 RVA: 0x001C2B30 File Offset: 0x001C0D30
	public void UpdateBuildableStates(bool skip_flourish)
	{
		if (this.subcategories != null && this.subcategories.Count > 0)
		{
			this.UpdateButtonStates();
			using (IEnumerator<KIconToggleMenu.ToggleInfo> enumerator = this.toggleInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KIconToggleMenu.ToggleInfo toggleInfo = enumerator.Current;
					BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData)toggleInfo.userData;
					HashedString hashedString = userData.category;
					PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(hashedString);
					if (userData.requirementsState != categoryRequirements)
					{
						userData.requirementsState = categoryRequirements;
						toggleInfo.userData = userData;
						if (!skip_flourish)
						{
							toggleInfo.toggle.ActivateFlourish(false);
							string text = "NotificationPing";
							if (!toggleInfo.toggle.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag(text))
							{
								toggleInfo.toggle.gameObject.GetComponent<Animator>().Play(text);
								BuildMenu.Instance.PlayNewBuildingSounds();
							}
						}
					}
				}
				return;
			}
		}
		this.buildingsScreen.UpdateBuildableStates();
	}

	// Token: 0x06004FB0 RID: 20400 RVA: 0x001C2C34 File Offset: 0x001C0E34
	protected override void OnShow(bool show)
	{
		if (this.buildingInfos != null)
		{
			if (show)
			{
				this.buildingsScreen.Configure(this.category, this.buildingInfos);
				this.buildingsScreen.Show(true);
			}
			else
			{
				this.buildingsScreen.Close();
			}
		}
		base.OnShow(show);
	}

	// Token: 0x06004FB1 RID: 20401 RVA: 0x001C2C84 File Offset: 0x001C0E84
	public override void ClearSelection()
	{
		this.selectedCategory = HashedString.Invalid;
		base.ClearSelection();
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.isOn = false;
		}
	}

	// Token: 0x06004FB2 RID: 20402 RVA: 0x001C2CE8 File Offset: 0x001C0EE8
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.modalKeyInputBehaviour)
		{
			if (this.HasFocus)
			{
				if (e.TryConsume(global::Action.Escape))
				{
					Game.Instance.Trigger(288942073, null);
					return;
				}
				base.OnKeyDown(e);
				if (!e.Consumed)
				{
					global::Action action = e.GetAction();
					if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
					{
						e.TryConsume(action);
						return;
					}
				}
			}
		}
		else
		{
			base.OnKeyDown(e);
			if (e.Consumed)
			{
				this.UpdateButtonStates();
			}
		}
	}

	// Token: 0x06004FB3 RID: 20403 RVA: 0x001C2D58 File Offset: 0x001C0F58
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.modalKeyInputBehaviour)
		{
			if (this.HasFocus)
			{
				if (e.TryConsume(global::Action.Escape))
				{
					Game.Instance.Trigger(288942073, null);
					return;
				}
				base.OnKeyUp(e);
				if (!e.Consumed)
				{
					global::Action action = e.GetAction();
					if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
					{
						e.TryConsume(action);
						return;
					}
				}
			}
		}
		else
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06004FB4 RID: 20404 RVA: 0x001C2DBA File Offset: 0x001C0FBA
	public override void SetHasFocus(bool has_focus)
	{
		base.SetHasFocus(has_focus);
		if (this.focusIndicator != null)
		{
			this.focusIndicator.color = (has_focus ? this.focusedColour : this.unfocusedColour);
		}
	}

	// Token: 0x04003417 RID: 13335
	public Action<HashedString, int> onCategoryClicked;

	// Token: 0x04003418 RID: 13336
	[SerializeField]
	public bool modalKeyInputBehaviour;

	// Token: 0x04003419 RID: 13337
	[SerializeField]
	private Image focusIndicator;

	// Token: 0x0400341A RID: 13338
	[SerializeField]
	private Color32 focusedColour;

	// Token: 0x0400341B RID: 13339
	[SerializeField]
	private Color32 unfocusedColour;

	// Token: 0x0400341C RID: 13340
	private IList<HashedString> subcategories;

	// Token: 0x0400341D RID: 13341
	private Dictionary<HashedString, List<BuildingDef>> categorizedBuildingMap;

	// Token: 0x0400341E RID: 13342
	private Dictionary<HashedString, List<HashedString>> categorizedCategoryMap;

	// Token: 0x0400341F RID: 13343
	private BuildMenuBuildingsScreen buildingsScreen;

	// Token: 0x04003420 RID: 13344
	private HashedString category;

	// Token: 0x04003421 RID: 13345
	private IList<BuildMenu.BuildingInfo> buildingInfos;

	// Token: 0x04003422 RID: 13346
	private HashedString selectedCategory = HashedString.Invalid;

	// Token: 0x020018E5 RID: 6373
	private class UserData
	{
		// Token: 0x0400734C RID: 29516
		public HashedString category;

		// Token: 0x0400734D RID: 29517
		public int depth;

		// Token: 0x0400734E RID: 29518
		public PlanScreen.RequirementsState requirementsState;

		// Token: 0x0400734F RID: 29519
		public ImageToggleState.State? currentToggleState;
	}
}
