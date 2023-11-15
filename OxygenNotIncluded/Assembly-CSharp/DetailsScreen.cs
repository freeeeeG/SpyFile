using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02000AF3 RID: 2803
public class DetailsScreen : KTabMenu
{
	// Token: 0x0600565A RID: 22106 RVA: 0x001F71CC File Offset: 0x001F53CC
	public static void DestroyInstance()
	{
		DetailsScreen.Instance = null;
	}

	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x0600565B RID: 22107 RVA: 0x001F71D4 File Offset: 0x001F53D4
	// (set) Token: 0x0600565C RID: 22108 RVA: 0x001F71DC File Offset: 0x001F53DC
	public GameObject target { get; private set; }

	// Token: 0x0600565D RID: 22109 RVA: 0x001F71E8 File Offset: 0x001F53E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.SortScreenOrder();
		base.ConsumeMouseScroll = true;
		global::Debug.Assert(DetailsScreen.Instance == null);
		DetailsScreen.Instance = this;
		this.DeactivateSideContent();
		this.Show(false);
		base.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnSelectObject));
	}

	// Token: 0x0600565E RID: 22110 RVA: 0x001F724D File Offset: 0x001F544D
	private void OnSelectObject(object data)
	{
		if (data == null)
		{
			this.previouslyActiveTab = -1;
		}
	}

	// Token: 0x0600565F RID: 22111 RVA: 0x001F725C File Offset: 0x001F545C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CodexEntryButton.onClick += this.OpenCodexEntry;
		this.ChangeOutfitButton.onClick += this.OnClickChangeOutfit;
		this.CloseButton.onClick += this.DeselectAndClose;
		this.TabTitle.OnNameChanged += this.OnNameChanged;
		this.TabTitle.OnStartedEditing += this.OnStartedEditing;
		this.sideScreen2.SetActive(false);
		base.Subscribe<DetailsScreen>(-1514841199, DetailsScreen.OnRefreshDataDelegate);
	}

	// Token: 0x06005660 RID: 22112 RVA: 0x001F72FF File Offset: 0x001F54FF
	private void OnStartedEditing()
	{
		base.isEditing = true;
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x06005661 RID: 22113 RVA: 0x001F7314 File Offset: 0x001F5514
	private void OnNameChanged(string newName)
	{
		base.isEditing = false;
		if (string.IsNullOrEmpty(newName))
		{
			return;
		}
		MinionIdentity component = this.target.GetComponent<MinionIdentity>();
		UserNameable component2 = this.target.GetComponent<UserNameable>();
		ClustercraftExteriorDoor component3 = this.target.GetComponent<ClustercraftExteriorDoor>();
		CommandModule component4 = this.target.GetComponent<CommandModule>();
		if (component != null)
		{
			component.SetName(newName);
		}
		else if (component4 != null)
		{
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(component4.GetComponent<LaunchConditionManager>()).SetRocketName(newName);
		}
		else if (component3 != null)
		{
			component3.GetTargetWorld().GetComponent<UserNameable>().SetName(newName);
		}
		else if (component2 != null)
		{
			component2.SetName(newName);
		}
		this.TabTitle.UpdateRenameTooltip(this.target);
	}

	// Token: 0x06005662 RID: 22114 RVA: 0x001F73D1 File Offset: 0x001F55D1
	protected override void OnDeactivate()
	{
		if (this.target != null && this.setRocketTitleHandle != -1)
		{
			this.target.Unsubscribe(this.setRocketTitleHandle);
		}
		this.setRocketTitleHandle = -1;
		this.DeactivateSideContent();
		base.OnDeactivate();
	}

	// Token: 0x06005663 RID: 22115 RVA: 0x001F740E File Offset: 0x001F560E
	protected override void OnShow(bool show)
	{
		if (!show)
		{
			this.DeactivateSideContent();
		}
		else
		{
			this.MaskSideContent(false);
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenHalfEffect);
		}
		base.OnShow(show);
	}

	// Token: 0x06005664 RID: 22116 RVA: 0x001F743E File Offset: 0x001F563E
	protected override void OnCmpDisable()
	{
		this.DeactivateSideContent();
		base.OnCmpDisable();
	}

	// Token: 0x06005665 RID: 22117 RVA: 0x001F744C File Offset: 0x001F564C
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!base.isEditing && this.target != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.DeselectAndClose();
		}
	}

	// Token: 0x06005666 RID: 22118 RVA: 0x001F7478 File Offset: 0x001F5678
	private static Component GetComponent(GameObject go, string name)
	{
		Type type = Type.GetType(name);
		Component component;
		if (type != null)
		{
			component = go.GetComponent(type);
		}
		else
		{
			component = go.GetComponent(name);
		}
		return component;
	}

	// Token: 0x06005667 RID: 22119 RVA: 0x001F74AC File Offset: 0x001F56AC
	private static bool IsExcludedPrefabTag(GameObject go, Tag[] excluded_tags)
	{
		if (excluded_tags == null || excluded_tags.Length == 0)
		{
			return false;
		}
		bool result = false;
		KPrefabID component = go.GetComponent<KPrefabID>();
		foreach (Tag b in excluded_tags)
		{
			if (component.PrefabTag == b)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005668 RID: 22120 RVA: 0x001F74F4 File Offset: 0x001F56F4
	private void UpdateCodexButton()
	{
		string selectedObjectCodexID = this.GetSelectedObjectCodexID();
		this.CodexEntryButton.isInteractable = (selectedObjectCodexID != "");
		this.CodexEntryButton.GetComponent<ToolTip>().SetSimpleTooltip(this.CodexEntryButton.isInteractable ? UI.TOOLTIPS.OPEN_CODEX_ENTRY : UI.TOOLTIPS.NO_CODEX_ENTRY);
	}

	// Token: 0x06005669 RID: 22121 RVA: 0x001F754C File Offset: 0x001F574C
	private void UpdateOutfitButton()
	{
		this.ChangeOutfitButton.gameObject.SetActive(this.target.GetComponent<MinionIdentity>());
	}

	// Token: 0x0600566A RID: 22122 RVA: 0x001F7570 File Offset: 0x001F5770
	public void OnRefreshData(object obj)
	{
		this.SetTitle(base.PreviousActiveTab);
		for (int i = 0; i < this.tabs.Count; i++)
		{
			if (this.tabs[i].gameObject.activeInHierarchy)
			{
				this.tabs[i].Trigger(-1514841199, obj);
			}
		}
	}

	// Token: 0x0600566B RID: 22123 RVA: 0x001F75D0 File Offset: 0x001F57D0
	public void Refresh(GameObject go)
	{
		if (this.screens == null)
		{
			return;
		}
		if (this.target != go && this.setRocketTitleHandle != -1)
		{
			this.target.Unsubscribe(this.setRocketTitleHandle);
			this.setRocketTitleHandle = -1;
		}
		this.target = go;
		this.sortedSideScreens.Clear();
		CellSelectionObject component = this.target.GetComponent<CellSelectionObject>();
		if (component)
		{
			component.OnObjectSelected(null);
		}
		if (!this.HasActivated)
		{
			if (this.screens != null)
			{
				for (int i = 0; i < this.screens.Length; i++)
				{
					GameObject gameObject = KScreenManager.Instance.InstantiateScreen(this.screens[i].screen.gameObject, this.body.gameObject).gameObject;
					this.screens[i].screen = gameObject.GetComponent<TargetScreen>();
					this.screens[i].tabIdx = base.AddTab(this.screens[i].icon, Strings.Get(this.screens[i].displayName), this.screens[i].screen, Strings.Get(this.screens[i].tooltip));
				}
			}
			base.onTabActivated += this.OnTabActivated;
			this.HasActivated = true;
		}
		int num = -1;
		int num2 = 0;
		for (int j = 0; j < this.screens.Length; j++)
		{
			bool flag = this.screens[j].screen.IsValidForTarget(go);
			bool flag2 = this.screens[j].hideWhenDead && base.gameObject.HasTag(GameTags.Dead);
			bool flag3 = flag && !flag2;
			base.SetTabEnabled(this.screens[j].tabIdx, flag3);
			if (flag3)
			{
				num2++;
				if (num == -1)
				{
					if (SimDebugView.Instance.GetMode() != OverlayModes.None.ID)
					{
						if (SimDebugView.Instance.GetMode() == this.screens[j].focusInViewMode)
						{
							num = j;
						}
					}
					else if (flag3 && this.previouslyActiveTab >= 0 && this.previouslyActiveTab < this.screens.Length && this.screens[j].name == this.screens[this.previouslyActiveTab].name)
					{
						num = this.screens[j].tabIdx;
					}
				}
			}
		}
		if (num != -1)
		{
			this.ActivateTab(num);
		}
		else
		{
			this.ActivateTab(0);
		}
		this.tabHeaderContainer.gameObject.SetActive(base.CountTabs() > 1);
		if (this.sideScreens != null && this.sideScreens.Count > 0)
		{
			bool flag4 = false;
			foreach (DetailsScreen.SideScreenRef sideScreenRef in this.sideScreens)
			{
				if (!sideScreenRef.screenPrefab.IsValidForTarget(this.target))
				{
					if (sideScreenRef.screenInstance != null && sideScreenRef.screenInstance.gameObject.activeSelf)
					{
						sideScreenRef.screenInstance.gameObject.SetActive(false);
					}
				}
				else
				{
					flag4 = true;
					if (sideScreenRef.screenInstance == null)
					{
						sideScreenRef.screenInstance = global::Util.KInstantiateUI<SideScreenContent>(sideScreenRef.screenPrefab.gameObject, this.sideScreenContentBody, false);
					}
					if (!this.sideScreen.activeSelf)
					{
						this.sideScreen.SetActive(true);
					}
					sideScreenRef.screenInstance.SetTarget(this.target);
					sideScreenRef.screenInstance.Show(true);
					int sideScreenSortOrder = sideScreenRef.screenInstance.GetSideScreenSortOrder();
					this.sortedSideScreens.Add(new KeyValuePair<GameObject, int>(sideScreenRef.screenInstance.gameObject, sideScreenSortOrder));
					if (this.currentSideScreen == null || !this.currentSideScreen.gameObject.activeSelf || sideScreenSortOrder > this.sortedSideScreens.Find((KeyValuePair<GameObject, int> match) => match.Key == this.currentSideScreen.gameObject).Value)
					{
						this.currentSideScreen = sideScreenRef.screenInstance;
					}
					this.RefreshTitle();
				}
			}
			if (!flag4)
			{
				this.sideScreen.SetActive(false);
			}
		}
		this.sortedSideScreens.Sort(delegate(KeyValuePair<GameObject, int> x, KeyValuePair<GameObject, int> y)
		{
			if (x.Value <= y.Value)
			{
				return 1;
			}
			return -1;
		});
		for (int k = 0; k < this.sortedSideScreens.Count; k++)
		{
			this.sortedSideScreens[k].Key.transform.SetSiblingIndex(k);
		}
	}

	// Token: 0x0600566C RID: 22124 RVA: 0x001F7AD8 File Offset: 0x001F5CD8
	public void RefreshTitle()
	{
		if (this.currentSideScreen)
		{
			this.sideScreenTitle.SetText(this.currentSideScreen.GetTitle());
		}
	}

	// Token: 0x0600566D RID: 22125 RVA: 0x001F7B00 File Offset: 0x001F5D00
	private void OnTabActivated(int newTab, int oldTab)
	{
		this.SetTitle(newTab);
		if (oldTab != -1)
		{
			this.screens[oldTab].screen.SetTarget(null);
		}
		if (newTab != -1)
		{
			this.screens[newTab].screen.SetTarget(this.target);
		}
	}

	// Token: 0x0600566E RID: 22126 RVA: 0x001F7B50 File Offset: 0x001F5D50
	public KScreen SetSecondarySideScreen(KScreen secondaryPrefab, string title)
	{
		this.ClearSecondarySideScreen();
		if (this.instantiatedSecondarySideScreens.ContainsKey(secondaryPrefab))
		{
			this.activeSideScreen2 = this.instantiatedSecondarySideScreens[secondaryPrefab];
			this.activeSideScreen2.gameObject.SetActive(true);
		}
		else
		{
			this.activeSideScreen2 = KScreenManager.Instance.InstantiateScreen(secondaryPrefab.gameObject, this.sideScreen2ContentBody);
			this.activeSideScreen2.Activate();
			this.instantiatedSecondarySideScreens.Add(secondaryPrefab, this.activeSideScreen2);
		}
		this.sideScreen2Title.text = title;
		this.sideScreen2.SetActive(true);
		return this.activeSideScreen2;
	}

	// Token: 0x0600566F RID: 22127 RVA: 0x001F7BED File Offset: 0x001F5DED
	public void ClearSecondarySideScreen()
	{
		if (this.activeSideScreen2 != null)
		{
			this.activeSideScreen2.gameObject.SetActive(false);
			this.activeSideScreen2 = null;
		}
		this.sideScreen2.SetActive(false);
	}

	// Token: 0x06005670 RID: 22128 RVA: 0x001F7C24 File Offset: 0x001F5E24
	public void DeactivateSideContent()
	{
		if (SideDetailsScreen.Instance != null && SideDetailsScreen.Instance.gameObject.activeInHierarchy)
		{
			SideDetailsScreen.Instance.Show(false);
		}
		if (this.sideScreens != null && this.sideScreens.Count > 0)
		{
			this.sideScreens.ForEach(delegate(DetailsScreen.SideScreenRef scn)
			{
				if (scn.screenInstance != null)
				{
					scn.screenInstance.ClearTarget();
					scn.screenInstance.Show(false);
				}
			});
		}
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenHalfEffect, STOP_MODE.ALLOWFADEOUT);
		this.sideScreen.SetActive(false);
	}

	// Token: 0x06005671 RID: 22129 RVA: 0x001F7CBC File Offset: 0x001F5EBC
	public void MaskSideContent(bool hide)
	{
		if (hide)
		{
			this.sideScreen.transform.localScale = Vector3.zero;
			return;
		}
		this.sideScreen.transform.localScale = Vector3.one;
	}

	// Token: 0x06005672 RID: 22130 RVA: 0x001F7CEC File Offset: 0x001F5EEC
	private string GetSelectedObjectCodexID()
	{
		string text = "";
		global::Debug.Assert(this.target != null, "Details Screen has no target");
		KSelectable component = this.target.GetComponent<KSelectable>();
		DebugUtil.AssertArgs(component != null, new object[]
		{
			"Details Screen target is not a KSelectable",
			this.target
		});
		CellSelectionObject component2 = component.GetComponent<CellSelectionObject>();
		BuildingUnderConstruction component3 = component.GetComponent<BuildingUnderConstruction>();
		CreatureBrain component4 = component.GetComponent<CreatureBrain>();
		PlantableSeed component5 = component.GetComponent<PlantableSeed>();
		BudUprootedMonitor component6 = component.GetComponent<BudUprootedMonitor>();
		if (component2 != null)
		{
			text = CodexCache.FormatLinkID(component2.element.id.ToString());
		}
		else if (component3 != null)
		{
			text = CodexCache.FormatLinkID(component3.Def.PrefabID);
		}
		else if (component4 != null)
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			text = text.Replace("BABY", "");
		}
		else if (component5 != null)
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
			text = text.Replace("SEED", "");
		}
		else if (component6 != null)
		{
			if (component6.parentObject.Get() != null)
			{
				text = CodexCache.FormatLinkID(component6.parentObject.Get().PrefabID().ToString());
			}
			else if (component6.GetComponent<TreeBud>() != null)
			{
				text = CodexCache.FormatLinkID(component6.GetComponent<TreeBud>().buddingTrunk.Get().PrefabID().ToString());
			}
		}
		else
		{
			text = CodexCache.FormatLinkID(component.PrefabID().ToString());
		}
		if (CodexCache.entries.ContainsKey(text) || CodexCache.FindSubEntry(text) != null)
		{
			return text;
		}
		return "";
	}

	// Token: 0x06005673 RID: 22131 RVA: 0x001F7EE4 File Offset: 0x001F60E4
	public void OpenCodexEntry()
	{
		string selectedObjectCodexID = this.GetSelectedObjectCodexID();
		if (selectedObjectCodexID != "")
		{
			ManagementMenu.Instance.OpenCodexToEntry(selectedObjectCodexID, null);
		}
	}

	// Token: 0x06005674 RID: 22132 RVA: 0x001F7F14 File Offset: 0x001F6114
	public void OnClickChangeOutfit()
	{
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
		MinionBrowserScreenConfig.MinionInstances(this.target).ApplyAndOpenScreen(delegate
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot, STOP_MODE.ALLOWFADEOUT);
		});
	}

	// Token: 0x06005675 RID: 22133 RVA: 0x001F7F70 File Offset: 0x001F6170
	public void DeselectAndClose()
	{
		if (base.gameObject.activeInHierarchy)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back", false));
		}
		if (this.GetActiveTab() != null)
		{
			this.GetActiveTab().SetTarget(null);
		}
		SelectTool.Instance.Select(null, false);
		ClusterMapSelectTool.Instance.Select(null, false);
		if (this.target == null)
		{
			return;
		}
		this.target = null;
		this.DeactivateSideContent();
		this.Show(false);
	}

	// Token: 0x06005676 RID: 22134 RVA: 0x001F7FEF File Offset: 0x001F61EF
	private void SortScreenOrder()
	{
		Array.Sort<DetailsScreen.Screens>(this.screens, (DetailsScreen.Screens x, DetailsScreen.Screens y) => x.displayOrderPriority.CompareTo(y.displayOrderPriority));
	}

	// Token: 0x06005677 RID: 22135 RVA: 0x001F801C File Offset: 0x001F621C
	public void UpdatePortrait(GameObject target)
	{
		KSelectable component = target.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		this.TabTitle.portrait.ClearPortrait();
		Building component2 = component.GetComponent<Building>();
		if (component2)
		{
			Sprite uisprite = component2.Def.GetUISprite("ui", false);
			if (uisprite != null)
			{
				this.TabTitle.portrait.SetPortrait(uisprite);
				return;
			}
		}
		if (target.GetComponent<MinionIdentity>())
		{
			this.TabTitle.SetPortrait(component.gameObject);
			return;
		}
		Edible component3 = target.GetComponent<Edible>();
		if (component3 != null)
		{
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(component3.GetComponent<KBatchedAnimController>().AnimFiles[0], "ui", false, "");
			this.TabTitle.portrait.SetPortrait(uispriteFromMultiObjectAnim);
			return;
		}
		PrimaryElement component4 = target.GetComponent<PrimaryElement>();
		if (component4 != null)
		{
			this.TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(ElementLoader.FindElementByHash(component4.ElementID).substance.anim, "ui", false, ""));
			return;
		}
		CellSelectionObject component5 = target.GetComponent<CellSelectionObject>();
		if (component5 != null)
		{
			string animName = component5.element.IsSolid ? "ui" : component5.element.substance.name;
			Sprite uispriteFromMultiObjectAnim2 = Def.GetUISpriteFromMultiObjectAnim(component5.element.substance.anim, animName, false, "");
			this.TabTitle.portrait.SetPortrait(uispriteFromMultiObjectAnim2);
			return;
		}
	}

	// Token: 0x06005678 RID: 22136 RVA: 0x001F81A0 File Offset: 0x001F63A0
	public bool CompareTargetWith(GameObject compare)
	{
		return this.target == compare;
	}

	// Token: 0x06005679 RID: 22137 RVA: 0x001F81B0 File Offset: 0x001F63B0
	public void SetTitle(int selectedTabIndex)
	{
		this.UpdateCodexButton();
		this.UpdateOutfitButton();
		if (this.TabTitle != null)
		{
			this.TabTitle.SetTitle(this.target.GetProperName());
			MinionIdentity minionIdentity = null;
			UserNameable x = null;
			ClustercraftExteriorDoor clustercraftExteriorDoor = null;
			CommandModule commandModule = null;
			if (this.target != null)
			{
				minionIdentity = this.target.gameObject.GetComponent<MinionIdentity>();
				x = this.target.gameObject.GetComponent<UserNameable>();
				clustercraftExteriorDoor = this.target.gameObject.GetComponent<ClustercraftExteriorDoor>();
				commandModule = this.target.gameObject.GetComponent<CommandModule>();
			}
			if (minionIdentity != null)
			{
				this.TabTitle.SetSubText(minionIdentity.GetComponent<MinionResume>().GetSkillsSubtitle(), "");
				this.TabTitle.SetUserEditable(true);
			}
			else if (x != null)
			{
				this.TabTitle.SetSubText("", "");
				this.TabTitle.SetUserEditable(true);
			}
			else if (commandModule != null)
			{
				this.TrySetRocketTitle(commandModule);
			}
			else if (clustercraftExteriorDoor != null)
			{
				this.TrySetRocketTitle(clustercraftExteriorDoor);
			}
			else
			{
				this.TabTitle.SetSubText("", "");
				this.TabTitle.SetUserEditable(false);
			}
			this.TabTitle.UpdateRenameTooltip(this.target);
		}
	}

	// Token: 0x0600567A RID: 22138 RVA: 0x001F8300 File Offset: 0x001F6500
	private void TrySetRocketTitle(ClustercraftExteriorDoor clusterCraftDoor)
	{
		if (clusterCraftDoor2.HasTargetWorld())
		{
			WorldContainer targetWorld = clusterCraftDoor2.GetTargetWorld();
			this.TabTitle.SetTitle(targetWorld.GetComponent<ClusterGridEntity>().Name);
			this.TabTitle.SetUserEditable(true);
			this.TabTitle.SetSubText(this.target.GetProperName(), "");
			this.setRocketTitleHandle = -1;
			return;
		}
		if (this.setRocketTitleHandle == -1)
		{
			this.setRocketTitleHandle = this.target.Subscribe(-71801987, delegate(object clusterCraftDoor)
			{
				this.OnRefreshData(null);
				this.target.Unsubscribe(this.setRocketTitleHandle);
				this.setRocketTitleHandle = -1;
			});
		}
	}

	// Token: 0x0600567B RID: 22139 RVA: 0x001F838C File Offset: 0x001F658C
	private void TrySetRocketTitle(CommandModule commandModule)
	{
		if (commandModule != null)
		{
			this.TabTitle.SetTitle(SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(commandModule.GetComponent<LaunchConditionManager>()).GetRocketName());
			this.TabTitle.SetUserEditable(true);
		}
		this.TabTitle.SetSubText(this.target.GetProperName(), "");
	}

	// Token: 0x0600567C RID: 22140 RVA: 0x001F83E9 File Offset: 0x001F65E9
	public void SetTitle(string title)
	{
		this.TabTitle.SetTitle(title);
	}

	// Token: 0x0600567D RID: 22141 RVA: 0x001F83F7 File Offset: 0x001F65F7
	public TargetScreen GetActiveTab()
	{
		if (this.previouslyActiveTab >= 0 && this.previouslyActiveTab < this.screens.Length)
		{
			return this.screens[this.previouslyActiveTab].screen;
		}
		return null;
	}

	// Token: 0x04003A1D RID: 14877
	public static DetailsScreen Instance;

	// Token: 0x04003A1E RID: 14878
	[SerializeField]
	private KButton CodexEntryButton;

	// Token: 0x04003A1F RID: 14879
	[SerializeField]
	private KButton ChangeOutfitButton;

	// Token: 0x04003A20 RID: 14880
	[Header("Panels")]
	public Transform UserMenuPanel;

	// Token: 0x04003A21 RID: 14881
	[Header("Name Editing (disabled)")]
	[SerializeField]
	private KButton CloseButton;

	// Token: 0x04003A22 RID: 14882
	[Header("Tabs")]
	[SerializeField]
	private EditableTitleBar TabTitle;

	// Token: 0x04003A23 RID: 14883
	[SerializeField]
	private DetailsScreen.Screens[] screens;

	// Token: 0x04003A24 RID: 14884
	[SerializeField]
	private GameObject tabHeaderContainer;

	// Token: 0x04003A25 RID: 14885
	[Header("Side Screens")]
	[SerializeField]
	private GameObject sideScreenContentBody;

	// Token: 0x04003A26 RID: 14886
	[SerializeField]
	private GameObject sideScreen;

	// Token: 0x04003A27 RID: 14887
	[SerializeField]
	private LocText sideScreenTitle;

	// Token: 0x04003A28 RID: 14888
	[SerializeField]
	private List<DetailsScreen.SideScreenRef> sideScreens;

	// Token: 0x04003A29 RID: 14889
	[Header("Secondary Side Screens")]
	[SerializeField]
	private GameObject sideScreen2ContentBody;

	// Token: 0x04003A2A RID: 14890
	[SerializeField]
	private GameObject sideScreen2;

	// Token: 0x04003A2B RID: 14891
	[SerializeField]
	private LocText sideScreen2Title;

	// Token: 0x04003A2C RID: 14892
	private KScreen activeSideScreen2;

	// Token: 0x04003A2E RID: 14894
	private bool HasActivated;

	// Token: 0x04003A2F RID: 14895
	private SideScreenContent currentSideScreen;

	// Token: 0x04003A30 RID: 14896
	private Dictionary<KScreen, KScreen> instantiatedSecondarySideScreens = new Dictionary<KScreen, KScreen>();

	// Token: 0x04003A31 RID: 14897
	private static readonly EventSystem.IntraObjectHandler<DetailsScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<DetailsScreen>(delegate(DetailsScreen component, object data)
	{
		component.OnRefreshData(data);
	});

	// Token: 0x04003A32 RID: 14898
	private List<KeyValuePair<GameObject, int>> sortedSideScreens = new List<KeyValuePair<GameObject, int>>();

	// Token: 0x04003A33 RID: 14899
	private int setRocketTitleHandle = -1;

	// Token: 0x02001A03 RID: 6659
	[Serializable]
	private struct Screens
	{
		// Token: 0x04007803 RID: 30723
		public string name;

		// Token: 0x04007804 RID: 30724
		public string displayName;

		// Token: 0x04007805 RID: 30725
		public string tooltip;

		// Token: 0x04007806 RID: 30726
		public Sprite icon;

		// Token: 0x04007807 RID: 30727
		public TargetScreen screen;

		// Token: 0x04007808 RID: 30728
		public int displayOrderPriority;

		// Token: 0x04007809 RID: 30729
		public bool hideWhenDead;

		// Token: 0x0400780A RID: 30730
		public HashedString focusInViewMode;

		// Token: 0x0400780B RID: 30731
		[HideInInspector]
		public int tabIdx;
	}

	// Token: 0x02001A04 RID: 6660
	[Serializable]
	public class SideScreenRef
	{
		// Token: 0x0400780C RID: 30732
		public string name;

		// Token: 0x0400780D RID: 30733
		public SideScreenContent screenPrefab;

		// Token: 0x0400780E RID: 30734
		public Vector2 offset;

		// Token: 0x0400780F RID: 30735
		[HideInInspector]
		public SideScreenContent screenInstance;
	}
}
