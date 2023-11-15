using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000A6D RID: 2669
public class ManagementMenu : KIconToggleMenu
{
	// Token: 0x060050EB RID: 20715 RVA: 0x001CB9E9 File Offset: 0x001C9BE9
	public static void DestroyInstance()
	{
		ManagementMenu.Instance = null;
	}

	// Token: 0x060050EC RID: 20716 RVA: 0x001CB9F1 File Offset: 0x001C9BF1
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x060050ED RID: 20717 RVA: 0x001CB9F8 File Offset: 0x001C9BF8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ManagementMenu.Instance = this;
		this.notificationDisplayer.onNotificationsChanged += this.OnNotificationsChanged;
		CodexCache.CodexCacheInit();
		ScheduledUIInstantiation component = GameScreenManager.Instance.GetComponent<ScheduledUIInstantiation>();
		this.starmapScreen = component.GetInstantiatedObject<StarmapScreen>();
		this.clusterMapScreen = component.GetInstantiatedObject<ClusterMapScreen>();
		this.skillsScreen = component.GetInstantiatedObject<SkillsScreen>();
		this.researchScreen = component.GetInstantiatedObject<ResearchScreen>();
		this.fullscreenUIs = new ManagementMenu.ManagementMenuToggleInfo[]
		{
			this.researchInfo,
			this.skillsInfo,
			this.starmapInfo,
			this.clusterMapInfo
		};
		base.Subscribe(Game.Instance.gameObject, 288942073, new Action<object>(this.OnUIClear));
		this.consumablesInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.CONSUMABLES, "OverviewUI_consumables_icon", null, global::Action.ManageConsumables, UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES, "");
		this.AddToggleTooltip(this.consumablesInfo, null);
		this.vitalsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.VITALS, "OverviewUI_vitals_icon", null, global::Action.ManageVitals, UI.TOOLTIPS.MANAGEMENTMENU_VITALS, "");
		this.AddToggleTooltip(this.vitalsInfo, null);
		this.researchInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.RESEARCH, "OverviewUI_research_nav_icon", null, global::Action.ManageResearch, UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, "");
		this.AddToggleTooltip(this.researchInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_RESEARCH);
		this.jobsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.JOBS, "OverviewUI_priority_icon", null, global::Action.ManagePriorities, UI.TOOLTIPS.MANAGEMENTMENU_JOBS, "");
		this.AddToggleTooltip(this.jobsInfo, null);
		this.skillsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.SKILLS, "OverviewUI_jobs_icon", null, global::Action.ManageSkills, UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, "");
		this.AddToggleTooltip(this.skillsInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_SKILL_STATION);
		this.starmapInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", null, global::Action.ManageStarmap, UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, "");
		this.AddToggleTooltip(this.starmapInfo, UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_TELESCOPE);
		this.clusterMapInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", null, global::Action.ManageStarmap, UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, "");
		this.AddToggleTooltip(this.clusterMapInfo, null);
		this.scheduleInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.SCHEDULE, "OverviewUI_schedule2_icon", null, global::Action.ManageSchedule, UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE, "");
		this.AddToggleTooltip(this.scheduleInfo, null);
		this.reportsInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.REPORT, "OverviewUI_reports_icon", null, global::Action.ManageReport, UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT, "");
		this.AddToggleTooltip(this.reportsInfo, null);
		this.reportsInfo.prefabOverride = this.smallPrefab;
		this.codexInfo = new ManagementMenu.ManagementMenuToggleInfo(UI.CODEX.MANAGEMENT_BUTTON, "OverviewUI_database_icon", null, global::Action.ManageDatabase, UI.TOOLTIPS.MANAGEMENTMENU_CODEX, "");
		this.AddToggleTooltip(this.codexInfo, null);
		this.codexInfo.prefabOverride = this.smallPrefab;
		this.ScreenInfoMatch.Add(this.consumablesInfo, new ManagementMenu.ScreenData
		{
			screen = this.consumablesScreen,
			tabIdx = 3,
			toggleInfo = this.consumablesInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.vitalsInfo, new ManagementMenu.ScreenData
		{
			screen = this.vitalsScreen,
			tabIdx = 2,
			toggleInfo = this.vitalsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.reportsInfo, new ManagementMenu.ScreenData
		{
			screen = this.reportsScreen,
			tabIdx = 4,
			toggleInfo = this.reportsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.jobsInfo, new ManagementMenu.ScreenData
		{
			screen = this.jobsScreen,
			tabIdx = 1,
			toggleInfo = this.jobsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.skillsInfo, new ManagementMenu.ScreenData
		{
			screen = this.skillsScreen,
			tabIdx = 0,
			toggleInfo = this.skillsInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.codexInfo, new ManagementMenu.ScreenData
		{
			screen = this.codexScreen,
			tabIdx = 6,
			toggleInfo = this.codexInfo,
			cancelHandler = null
		});
		this.ScreenInfoMatch.Add(this.scheduleInfo, new ManagementMenu.ScreenData
		{
			screen = this.scheduleScreen,
			tabIdx = 7,
			toggleInfo = this.scheduleInfo,
			cancelHandler = null
		});
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			this.ScreenInfoMatch.Add(this.clusterMapInfo, new ManagementMenu.ScreenData
			{
				screen = this.clusterMapScreen,
				tabIdx = 7,
				toggleInfo = this.clusterMapInfo,
				cancelHandler = new Func<bool>(this.clusterMapScreen.TryHandleCancel)
			});
		}
		else
		{
			this.ScreenInfoMatch.Add(this.starmapInfo, new ManagementMenu.ScreenData
			{
				screen = this.starmapScreen,
				tabIdx = 7,
				toggleInfo = this.starmapInfo,
				cancelHandler = null
			});
		}
		this.ScreenInfoMatch.Add(this.researchInfo, new ManagementMenu.ScreenData
		{
			screen = this.researchScreen,
			tabIdx = 5,
			toggleInfo = this.researchInfo,
			cancelHandler = null
		});
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		list.Add(this.vitalsInfo);
		list.Add(this.consumablesInfo);
		list.Add(this.scheduleInfo);
		list.Add(this.jobsInfo);
		list.Add(this.skillsInfo);
		list.Add(this.researchInfo);
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(this.clusterMapInfo);
		}
		else
		{
			list.Add(this.starmapInfo);
		}
		list.Add(this.reportsInfo);
		list.Add(this.codexInfo);
		base.Setup(list);
		base.onSelect += this.OnButtonClick;
		this.PauseMenuButton.onClick += this.OnPauseMenuClicked;
		this.PauseMenuButton.transform.SetAsLastSibling();
		this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, global::Action.Escape);
		KInputManager.InputChange.AddListener(new UnityAction(this.OnInputChanged));
		Components.ResearchCenters.OnAdd += new Action<IResearchCenter>(this.CheckResearch);
		Components.ResearchCenters.OnRemove += new Action<IResearchCenter>(this.CheckResearch);
		Components.RoleStations.OnAdd += new Action<RoleStation>(this.CheckSkills);
		Components.RoleStations.OnRemove += new Action<RoleStation>(this.CheckSkills);
		Game.Instance.Subscribe(-809948329, new Action<object>(this.CheckResearch));
		Game.Instance.Subscribe(-809948329, new Action<object>(this.CheckSkills));
		Game.Instance.Subscribe(445618876, new Action<object>(this.OnResolutionChanged));
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			Components.Telescopes.OnAdd += new Action<Telescope>(this.CheckStarmap);
			Components.Telescopes.OnRemove += new Action<Telescope>(this.CheckStarmap);
		}
		this.CheckResearch(null);
		this.CheckSkills(null);
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.CheckStarmap(null);
		}
		this.researchInfo.toggle.soundPlayer.AcceptClickCondition = (() => this.ResearchAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.soundPlayer.toggle_widget_sound_events[0].PlaySound = false;
			ktoggle.soundPlayer.toggle_widget_sound_events[1].PlaySound = false;
		}
		this.OnResolutionChanged(null);
	}

	// Token: 0x060050EE RID: 20718 RVA: 0x001CC230 File Offset: 0x001CA430
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.mutuallyExclusiveScreens.Add(AllResourcesScreen.Instance);
		this.mutuallyExclusiveScreens.Add(AllDiagnosticsScreen.Instance);
		this.OnNotificationsChanged();
	}

	// Token: 0x060050EF RID: 20719 RVA: 0x001CC25E File Offset: 0x001CA45E
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.OnInputChanged));
		base.OnForcedCleanUp();
	}

	// Token: 0x060050F0 RID: 20720 RVA: 0x001CC27C File Offset: 0x001CA47C
	private void OnInputChanged()
	{
		this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, global::Action.Escape);
		this.consumablesInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES, this.consumablesInfo.hotKey);
		this.vitalsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_VITALS, this.vitalsInfo.hotKey);
		this.researchInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, this.researchInfo.hotKey);
		this.jobsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_JOBS, this.jobsInfo.hotKey);
		this.skillsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, this.skillsInfo.hotKey);
		this.starmapInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, this.starmapInfo.hotKey);
		this.clusterMapInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, this.clusterMapInfo.hotKey);
		this.scheduleInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE, this.scheduleInfo.hotKey);
		this.reportsInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT, this.reportsInfo.hotKey);
		this.codexInfo.tooltip = GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.MANAGEMENTMENU_CODEX, this.codexInfo.hotKey);
	}

	// Token: 0x060050F1 RID: 20721 RVA: 0x001CC41C File Offset: 0x001CA61C
	private void OnResolutionChanged(object data = null)
	{
		bool flag = (float)Screen.width < 1300f;
		foreach (KToggle ktoggle in this.toggles)
		{
			HierarchyReferences component = ktoggle.GetComponent<HierarchyReferences>();
			if (!(component == null))
			{
				RectTransform reference = component.GetReference<RectTransform>("TextContainer");
				if (!(reference == null))
				{
					reference.gameObject.SetActive(!flag);
				}
			}
		}
	}

	// Token: 0x060050F2 RID: 20722 RVA: 0x001CC4A8 File Offset: 0x001CA6A8
	private void OnNotificationsChanged()
	{
		foreach (KeyValuePair<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData> keyValuePair in this.ScreenInfoMatch)
		{
			keyValuePair.Key.SetNotificationDisplay(false, false, null, this.noAlertColorStyle);
		}
	}

	// Token: 0x060050F3 RID: 20723 RVA: 0x001CC50C File Offset: 0x001CA70C
	private void AddToggleTooltip(ManagementMenu.ManagementMenuToggleInfo toggleInfo, string disabledTooltip = null)
	{
		toggleInfo.getTooltipText = delegate()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			if (disabledTooltip != null && !toggleInfo.toggle.interactable)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(disabledTooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				return list;
			}
			if (toggleInfo.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>(toggleInfo.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		};
	}

	// Token: 0x060050F4 RID: 20724 RVA: 0x001CC544 File Offset: 0x001CA744
	public bool IsFullscreenUIActive()
	{
		if (this.activeScreen == null)
		{
			return false;
		}
		foreach (ManagementMenu.ManagementMenuToggleInfo managementMenuToggleInfo in this.fullscreenUIs)
		{
			if (this.activeScreen.toggleInfo == managementMenuToggleInfo)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060050F5 RID: 20725 RVA: 0x001CC585 File Offset: 0x001CA785
	private void OnPauseMenuClicked()
	{
		PauseScreen.Instance.Show(true);
		this.PauseMenuButton.isOn = false;
	}

	// Token: 0x060050F6 RID: 20726 RVA: 0x001CC5A0 File Offset: 0x001CA7A0
	public void CheckResearch(object o)
	{
		if (this.researchInfo.toggle == null)
		{
			return;
		}
		bool flag = Components.ResearchCenters.Count <= 0 && !DebugHandler.InstantBuildMode;
		bool active = !flag && this.activeScreen != null && this.activeScreen.toggleInfo == this.researchInfo;
		this.ConfigureToggle(this.researchInfo.toggle, flag, active);
	}

	// Token: 0x060050F7 RID: 20727 RVA: 0x001CC610 File Offset: 0x001CA810
	public void CheckSkills(object o = null)
	{
		if (this.skillsInfo.toggle == null)
		{
			return;
		}
		bool disabled = Components.RoleStations.Count <= 0 && !DebugHandler.InstantBuildMode;
		bool active = this.activeScreen != null && this.activeScreen.toggleInfo == this.skillsInfo;
		this.ConfigureToggle(this.skillsInfo.toggle, disabled, active);
	}

	// Token: 0x060050F8 RID: 20728 RVA: 0x001CC67C File Offset: 0x001CA87C
	public void CheckStarmap(object o = null)
	{
		if (this.starmapInfo.toggle == null)
		{
			return;
		}
		bool disabled = Components.Telescopes.Count <= 0 && !DebugHandler.InstantBuildMode;
		bool active = this.activeScreen != null && this.activeScreen.toggleInfo == this.starmapInfo;
		this.ConfigureToggle(this.starmapInfo.toggle, disabled, active);
	}

	// Token: 0x060050F9 RID: 20729 RVA: 0x001CC6E8 File Offset: 0x001CA8E8
	private void ConfigureToggle(KToggle toggle, bool disabled, bool active)
	{
		toggle.interactable = !disabled;
		if (disabled)
		{
			toggle.GetComponentInChildren<ImageToggleState>().SetDisabled();
			return;
		}
		toggle.GetComponentInChildren<ImageToggleState>().SetActiveState(active);
	}

	// Token: 0x060050FA RID: 20730 RVA: 0x001CC70F File Offset: 0x001CA90F
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.activeScreen != null && e.TryConsume(global::Action.Escape))
		{
			this.ToggleIfCancelUnhandled(this.activeScreen);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x060050FB RID: 20731 RVA: 0x001CC73D File Offset: 0x001CA93D
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.activeScreen != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.ToggleIfCancelUnhandled(this.activeScreen);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x060050FC RID: 20732 RVA: 0x001CC770 File Offset: 0x001CA970
	private void ToggleIfCancelUnhandled(ManagementMenu.ScreenData screenData)
	{
		if (screenData.cancelHandler == null || !screenData.cancelHandler())
		{
			this.ToggleScreen(screenData);
		}
	}

	// Token: 0x060050FD RID: 20733 RVA: 0x001CC78E File Offset: 0x001CA98E
	private bool ResearchAvailable()
	{
		return Components.ResearchCenters.Count > 0 || DebugHandler.InstantBuildMode;
	}

	// Token: 0x060050FE RID: 20734 RVA: 0x001CC7A4 File Offset: 0x001CA9A4
	private bool SkillsAvailable()
	{
		return Components.RoleStations.Count > 0 || DebugHandler.InstantBuildMode;
	}

	// Token: 0x060050FF RID: 20735 RVA: 0x001CC7BA File Offset: 0x001CA9BA
	public static bool StarmapAvailable()
	{
		return Components.Telescopes.Count > 0 || DebugHandler.InstantBuildMode;
	}

	// Token: 0x06005100 RID: 20736 RVA: 0x001CC7D0 File Offset: 0x001CA9D0
	public void CloseAll()
	{
		if (this.activeScreen == null)
		{
			return;
		}
		if (this.activeScreen.toggleInfo != null)
		{
			this.ToggleScreen(this.activeScreen);
		}
		this.CloseActive();
		this.ClearSelection();
	}

	// Token: 0x06005101 RID: 20737 RVA: 0x001CC800 File Offset: 0x001CAA00
	private void OnUIClear(object data)
	{
		this.CloseAll();
	}

	// Token: 0x06005102 RID: 20738 RVA: 0x001CC808 File Offset: 0x001CAA08
	public void ToggleScreen(ManagementMenu.ScreenData screenData)
	{
		if (screenData == null)
		{
			return;
		}
		if (screenData.toggleInfo == this.researchInfo && !this.ResearchAvailable())
		{
			this.CheckResearch(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo == this.skillsInfo && !this.SkillsAvailable())
		{
			this.CheckSkills(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo == this.starmapInfo && !ManagementMenu.StarmapAvailable())
		{
			this.CheckStarmap(null);
			this.CloseActive();
			return;
		}
		if (screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().IsDisabled)
		{
			return;
		}
		if (this.activeScreen != null)
		{
			this.activeScreen.toggleInfo.toggle.isOn = false;
			this.activeScreen.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
		}
		if (this.activeScreen != screenData)
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			if (this.activeScreen != null)
			{
				this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenMigrated);
			screenData.toggleInfo.toggle.ActivateFlourish(true);
			screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetActive();
			this.CloseActive();
			this.activeScreen = screenData;
			if (!this.activeScreen.screen.IsActive())
			{
				this.activeScreen.screen.Activate();
			}
			this.activeScreen.screen.Show(true);
			foreach (ManagementMenuNotification managementMenuNotification in this.notificationDisplayer.GetNotificationsForAction(screenData.toggleInfo.hotKey))
			{
				if (managementMenuNotification.customClickCallback != null)
				{
					managementMenuNotification.customClickCallback(managementMenuNotification.customClickData);
					break;
				}
			}
			using (List<KScreen>.Enumerator enumerator2 = this.mutuallyExclusiveScreens.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KScreen kscreen = enumerator2.Current;
					kscreen.Show(false);
				}
				return;
			}
		}
		this.activeScreen.screen.Show(false);
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenMigrated, STOP_MODE.ALLOWFADEOUT);
		this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
		this.activeScreen = null;
		screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
	}

	// Token: 0x06005103 RID: 20739 RVA: 0x001CCAC0 File Offset: 0x001CACC0
	public void OnButtonClick(KIconToggleMenu.ToggleInfo toggle_info)
	{
		this.ToggleScreen(this.ScreenInfoMatch[(ManagementMenu.ManagementMenuToggleInfo)toggle_info]);
	}

	// Token: 0x06005104 RID: 20740 RVA: 0x001CCAD9 File Offset: 0x001CACD9
	private void CloseActive()
	{
		if (this.activeScreen != null)
		{
			this.activeScreen.toggleInfo.toggle.isOn = false;
			this.activeScreen.screen.Show(false);
			this.activeScreen = null;
		}
	}

	// Token: 0x06005105 RID: 20741 RVA: 0x001CCB14 File Offset: 0x001CAD14
	public void ToggleResearch()
	{
		if ((this.ResearchAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]) && this.researchInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		}
	}

	// Token: 0x06005106 RID: 20742 RVA: 0x001CCB69 File Offset: 0x001CAD69
	public void ToggleCodex()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.codexInfo]);
	}

	// Token: 0x06005107 RID: 20743 RVA: 0x001CCB88 File Offset: 0x001CAD88
	public void OpenCodexToEntry(string id, ContentContainer targetContainer = null)
	{
		if (!this.codexScreen.gameObject.activeInHierarchy)
		{
			this.ToggleCodex();
		}
		this.codexScreen.ChangeArticle(id, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		this.codexScreen.FocusContainer(targetContainer);
	}

	// Token: 0x06005108 RID: 20744 RVA: 0x001CCBD0 File Offset: 0x001CADD0
	public void ToggleSkills()
	{
		if ((this.SkillsAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]) && this.skillsInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
		}
	}

	// Token: 0x06005109 RID: 20745 RVA: 0x001CCC25 File Offset: 0x001CAE25
	public void ToggleStarmap()
	{
		if (this.starmapInfo != null)
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
		}
	}

	// Token: 0x0600510A RID: 20746 RVA: 0x001CCC4A File Offset: 0x001CAE4A
	public void ToggleClusterMap()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
	}

	// Token: 0x0600510B RID: 20747 RVA: 0x001CCC67 File Offset: 0x001CAE67
	public void TogglePriorities()
	{
		this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.jobsInfo]);
	}

	// Token: 0x0600510C RID: 20748 RVA: 0x001CCC84 File Offset: 0x001CAE84
	public void OpenReports(int day)
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo]);
		}
		ReportScreen.Instance.ShowReport(day);
	}

	// Token: 0x0600510D RID: 20749 RVA: 0x001CCCD4 File Offset: 0x001CAED4
	public void OpenResearch()
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
		}
	}

	// Token: 0x0600510E RID: 20750 RVA: 0x001CCD0E File Offset: 0x001CAF0E
	public void OpenStarmap()
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
		}
	}

	// Token: 0x0600510F RID: 20751 RVA: 0x001CCD48 File Offset: 0x001CAF48
	public void OpenClusterMap()
	{
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
		}
	}

	// Token: 0x06005110 RID: 20752 RVA: 0x001CCD82 File Offset: 0x001CAF82
	public void CloseClusterMap()
	{
		if (this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
		}
	}

	// Token: 0x06005111 RID: 20753 RVA: 0x001CCDBC File Offset: 0x001CAFBC
	public void OpenSkills(MinionIdentity minionIdentity)
	{
		this.skillsScreen.CurrentlySelectedMinion = minionIdentity;
		if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo])
		{
			this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
		}
	}

	// Token: 0x06005112 RID: 20754 RVA: 0x001CCE0D File Offset: 0x001CB00D
	public bool IsScreenOpen(KScreen screen)
	{
		return this.activeScreen != null && this.activeScreen.screen == screen;
	}

	// Token: 0x040034F4 RID: 13556
	private const float UI_WIDTH_COMPRESS_THRESHOLD = 1300f;

	// Token: 0x040034F5 RID: 13557
	[MyCmpReq]
	public ManagementMenuNotificationDisplayer notificationDisplayer;

	// Token: 0x040034F6 RID: 13558
	public static ManagementMenu Instance;

	// Token: 0x040034F7 RID: 13559
	[Header("Management Menu Specific")]
	[SerializeField]
	private KToggle smallPrefab;

	// Token: 0x040034F8 RID: 13560
	public KToggle PauseMenuButton;

	// Token: 0x040034F9 RID: 13561
	[Header("Top Right Screen References")]
	public JobsTableScreen jobsScreen;

	// Token: 0x040034FA RID: 13562
	public VitalsTableScreen vitalsScreen;

	// Token: 0x040034FB RID: 13563
	public ScheduleScreen scheduleScreen;

	// Token: 0x040034FC RID: 13564
	public ReportScreen reportsScreen;

	// Token: 0x040034FD RID: 13565
	public CodexScreen codexScreen;

	// Token: 0x040034FE RID: 13566
	public ConsumablesTableScreen consumablesScreen;

	// Token: 0x040034FF RID: 13567
	private StarmapScreen starmapScreen;

	// Token: 0x04003500 RID: 13568
	private ClusterMapScreen clusterMapScreen;

	// Token: 0x04003501 RID: 13569
	private SkillsScreen skillsScreen;

	// Token: 0x04003502 RID: 13570
	private ResearchScreen researchScreen;

	// Token: 0x04003503 RID: 13571
	[Header("Notification Styles")]
	public ColorStyleSetting noAlertColorStyle;

	// Token: 0x04003504 RID: 13572
	public List<ColorStyleSetting> alertColorStyle;

	// Token: 0x04003505 RID: 13573
	public List<TextStyleSetting> alertTextStyle;

	// Token: 0x04003506 RID: 13574
	private ManagementMenu.ManagementMenuToggleInfo jobsInfo;

	// Token: 0x04003507 RID: 13575
	private ManagementMenu.ManagementMenuToggleInfo consumablesInfo;

	// Token: 0x04003508 RID: 13576
	private ManagementMenu.ManagementMenuToggleInfo scheduleInfo;

	// Token: 0x04003509 RID: 13577
	private ManagementMenu.ManagementMenuToggleInfo vitalsInfo;

	// Token: 0x0400350A RID: 13578
	private ManagementMenu.ManagementMenuToggleInfo reportsInfo;

	// Token: 0x0400350B RID: 13579
	private ManagementMenu.ManagementMenuToggleInfo researchInfo;

	// Token: 0x0400350C RID: 13580
	private ManagementMenu.ManagementMenuToggleInfo codexInfo;

	// Token: 0x0400350D RID: 13581
	private ManagementMenu.ManagementMenuToggleInfo starmapInfo;

	// Token: 0x0400350E RID: 13582
	private ManagementMenu.ManagementMenuToggleInfo clusterMapInfo;

	// Token: 0x0400350F RID: 13583
	private ManagementMenu.ManagementMenuToggleInfo skillsInfo;

	// Token: 0x04003510 RID: 13584
	private ManagementMenu.ManagementMenuToggleInfo[] fullscreenUIs;

	// Token: 0x04003511 RID: 13585
	private Dictionary<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData> ScreenInfoMatch = new Dictionary<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData>();

	// Token: 0x04003512 RID: 13586
	private ManagementMenu.ScreenData activeScreen;

	// Token: 0x04003513 RID: 13587
	private KButton activeButton;

	// Token: 0x04003514 RID: 13588
	private string skillsTooltip;

	// Token: 0x04003515 RID: 13589
	private string skillsTooltipDisabled;

	// Token: 0x04003516 RID: 13590
	private string researchTooltip;

	// Token: 0x04003517 RID: 13591
	private string researchTooltipDisabled;

	// Token: 0x04003518 RID: 13592
	private string starmapTooltip;

	// Token: 0x04003519 RID: 13593
	private string starmapTooltipDisabled;

	// Token: 0x0400351A RID: 13594
	private string clusterMapTooltip;

	// Token: 0x0400351B RID: 13595
	private string clusterMapTooltipDisabled;

	// Token: 0x0400351C RID: 13596
	private List<KScreen> mutuallyExclusiveScreens = new List<KScreen>();

	// Token: 0x02001909 RID: 6409
	public class ScreenData
	{
		// Token: 0x040073EA RID: 29674
		public KScreen screen;

		// Token: 0x040073EB RID: 29675
		public ManagementMenu.ManagementMenuToggleInfo toggleInfo;

		// Token: 0x040073EC RID: 29676
		public Func<bool> cancelHandler;

		// Token: 0x040073ED RID: 29677
		public int tabIdx;
	}

	// Token: 0x0200190A RID: 6410
	public class ManagementMenuToggleInfo : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x06009393 RID: 37779 RVA: 0x0032FCD3 File Offset: 0x0032DED3
		public ManagementMenuToggleInfo(string text, string icon, object user_data = null, global::Action hotkey = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon, user_data, hotkey, tooltip, tooltip_header)
		{
			this.tooltip = GameUtil.ReplaceHotkeyString(this.tooltip, this.hotKey);
		}

		// Token: 0x06009394 RID: 37780 RVA: 0x0032FCFC File Offset: 0x0032DEFC
		public void SetNotificationDisplay(bool showAlertImage, bool showGlow, ColorStyleSetting buttonColorStyle, ColorStyleSetting alertColorStyle)
		{
			ImageToggleState component = this.toggle.GetComponent<ImageToggleState>();
			if (component != null)
			{
				if (buttonColorStyle != null)
				{
					component.SetColorStyle(buttonColorStyle);
				}
				else
				{
					component.SetColorStyle(this.originalButtonSetting);
				}
			}
			if (this.alertImage != null)
			{
				this.alertImage.gameObject.SetActive(showAlertImage);
				this.alertImage.SetColorStyle(alertColorStyle);
			}
			if (this.glowImage != null)
			{
				this.glowImage.gameObject.SetActive(showGlow);
				if (buttonColorStyle != null)
				{
					this.glowImage.SetColorStyle(buttonColorStyle);
				}
			}
		}

		// Token: 0x06009395 RID: 37781 RVA: 0x0032FD9C File Offset: 0x0032DF9C
		public override void SetToggle(KToggle toggle)
		{
			base.SetToggle(toggle);
			ImageToggleState component = toggle.GetComponent<ImageToggleState>();
			if (component != null)
			{
				this.originalButtonSetting = component.colorStyleSetting;
			}
			HierarchyReferences component2 = toggle.GetComponent<HierarchyReferences>();
			if (component2 != null)
			{
				this.alertImage = component2.GetReference<ImageToggleState>("AlertImage");
				this.glowImage = component2.GetReference<ImageToggleState>("GlowImage");
			}
		}

		// Token: 0x040073EE RID: 29678
		public ImageToggleState alertImage;

		// Token: 0x040073EF RID: 29679
		public ImageToggleState glowImage;

		// Token: 0x040073F0 RID: 29680
		private ColorStyleSetting originalButtonSetting;
	}
}
