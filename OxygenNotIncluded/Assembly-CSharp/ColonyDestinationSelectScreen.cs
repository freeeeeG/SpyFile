using System;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000AD9 RID: 2777
public class ColonyDestinationSelectScreen : NewGameFlowScreen
{
	// Token: 0x0600557F RID: 21887 RVA: 0x001F1A44 File Offset: 0x001EFC44
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.backButton.onClick += this.BackClicked;
		this.customizeButton.onClick += this.CustomizeClicked;
		this.launchButton.onClick += this.LaunchClicked;
		this.shuffleButton.onClick += this.ShuffleClicked;
		this.storyTraitShuffleButton.onClick += this.StoryTraitShuffleClicked;
		this.storyTraitShuffleButton.gameObject.SetActive(Db.Get().Stories.Count > 4);
		this.destinationMapPanel.OnAsteroidClicked += this.OnAsteroidClicked;
		KInputTextField kinputTextField = this.coordinate;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(this.CoordinateEditStarted));
		this.coordinate.onEndEdit.AddListener(new UnityAction<string>(this.CoordinateEditFinished));
		if (this.locationIcons != null)
		{
			bool cloudSavesAvailable = SaveLoader.GetCloudSavesAvailable();
			this.locationIcons.gameObject.SetActive(cloudSavesAvailable);
		}
		this.random = new KRandom();
	}

	// Token: 0x06005580 RID: 21888 RVA: 0x001F1B78 File Offset: 0x001EFD78
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshCloudSavePref();
		this.RefreshCloudLocalIcon();
		this.newGameSettings.Init();
		this.newGameSettings.SetCloseAction(new System.Action(this.CustomizeClose));
		this.destinationMapPanel.Init();
		CustomGameSettings.Instance.OnQualitySettingChanged += this.QualitySettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged += this.QualitySettingChanged;
		this.ShuffleClicked();
		this.RefreshMenuTabs();
		for (int i = 0; i < this.menuTabs.Length; i++)
		{
			int target = i;
			this.menuTabs[i].onClick = delegate()
			{
				this.selectedMenuTabIdx = target;
				this.RefreshMenuTabs();
			};
		}
		this.ResizeLayout();
		this.storyContentPanel.Init();
		this.storyContentPanel.SelectRandomStories(4, 4, true);
		this.storyContentPanel.SelectDefault();
		this.RefreshStoryLabel();
		this.RefreshRowsAndDescriptions();
	}

	// Token: 0x06005581 RID: 21889 RVA: 0x001F1C74 File Offset: 0x001EFE74
	private void ResizeLayout()
	{
		Vector2 sizeDelta = this.destinationProperties.clusterDetailsButton.rectTransform().sizeDelta;
		this.destinationProperties.clusterDetailsButton.rectTransform().sizeDelta = new Vector2(sizeDelta.x, (float)(DlcManager.FeatureClusterSpaceEnabled() ? 164 : 76));
		Vector2 sizeDelta2 = this.worldsScrollPanel.rectTransform().sizeDelta;
		Vector2 anchoredPosition = this.worldsScrollPanel.rectTransform().anchoredPosition;
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.worldsScrollPanel.rectTransform().anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + 88f);
		}
		float num = (float)(DlcManager.FeatureClusterSpaceEnabled() ? 436 : 524);
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
		num = Mathf.Min(num, this.destinationInfoPanel.sizeDelta.y - (float)(DlcManager.FeatureClusterSpaceEnabled() ? 164 : 76) - 22f);
		this.worldsScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
		this.storyScrollPanel.rectTransform().sizeDelta = new Vector2(sizeDelta2.x, num);
	}

	// Token: 0x06005582 RID: 21890 RVA: 0x001F1DA4 File Offset: 0x001EFFA4
	protected override void OnCleanUp()
	{
		CustomGameSettings.Instance.OnQualitySettingChanged -= this.QualitySettingChanged;
		CustomGameSettings.Instance.OnStorySettingChanged -= this.QualitySettingChanged;
		this.storyContentPanel.Cleanup();
		base.OnCleanUp();
	}

	// Token: 0x06005583 RID: 21891 RVA: 0x001F1DE4 File Offset: 0x001EFFE4
	private void RefreshCloudLocalIcon()
	{
		if (this.locationIcons == null)
		{
			return;
		}
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		HierarchyReferences component = this.locationIcons.GetComponent<HierarchyReferences>();
		LocText component2 = component.GetReference<RectTransform>("LocationText").GetComponent<LocText>();
		KButton component3 = component.GetReference<RectTransform>("CloudButton").GetComponent<KButton>();
		KButton component4 = component.GetReference<RectTransform>("LocalButton").GetComponent<KButton>();
		ToolTip component5 = component3.GetComponent<ToolTip>();
		ToolTip component6 = component4.GetComponent<ToolTip>();
		component5.toolTip = string.Format("{0}\n{1}", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA);
		component6.toolTip = string.Format("{0}\n{1}", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_LOCAL, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SAVETOCLOUD.TOOLTIP_EXTRA);
		bool flag = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
		component2.text = (flag ? UI.FRONTEND.LOADSCREEN.CLOUD_SAVE : UI.FRONTEND.LOADSCREEN.LOCAL_SAVE);
		component3.gameObject.SetActive(flag);
		component3.ClearOnClick();
		if (flag)
		{
			component3.onClick += delegate()
			{
				CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Disabled");
				this.RefreshCloudLocalIcon();
			};
		}
		component4.gameObject.SetActive(!flag);
		component4.ClearOnClick();
		if (!flag)
		{
			component4.onClick += delegate()
			{
				CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, "Enabled");
				this.RefreshCloudLocalIcon();
			};
		}
	}

	// Token: 0x06005584 RID: 21892 RVA: 0x001F1F18 File Offset: 0x001F0118
	private void RefreshCloudSavePref()
	{
		if (!SaveLoader.GetCloudSavesAvailable())
		{
			return;
		}
		string cloudSavesDefaultPref = SaveLoader.GetCloudSavesDefaultPref();
		CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, cloudSavesDefaultPref);
	}

	// Token: 0x06005585 RID: 21893 RVA: 0x001F1F43 File Offset: 0x001F0143
	private void BackClicked()
	{
		this.newGameSettings.Cancel();
		base.NavigateBackward();
	}

	// Token: 0x06005586 RID: 21894 RVA: 0x001F1F56 File Offset: 0x001F0156
	private void CustomizeClicked()
	{
		this.newGameSettings.Refresh();
		this.customSettings.SetActive(true);
	}

	// Token: 0x06005587 RID: 21895 RVA: 0x001F1F6F File Offset: 0x001F016F
	private void CustomizeClose()
	{
		this.customSettings.SetActive(false);
	}

	// Token: 0x06005588 RID: 21896 RVA: 0x001F1F7D File Offset: 0x001F017D
	private void LaunchClicked()
	{
		base.NavigateForward();
	}

	// Token: 0x06005589 RID: 21897 RVA: 0x001F1F88 File Offset: 0x001F0188
	private void RefreshMenuTabs()
	{
		for (int i = 0; i < this.menuTabs.Length; i++)
		{
			this.menuTabs[i].ChangeState((i == this.selectedMenuTabIdx) ? 1 : 0);
			this.menuTabs[i].GetComponentInChildren<LocText>().color = ((i == this.selectedMenuTabIdx) ? Color.white : Color.grey);
		}
		this.destinationInfoPanel.gameObject.SetActive(this.selectedMenuTabIdx == 0);
		this.storyInfoPanel.gameObject.SetActive(this.selectedMenuTabIdx == 1);
		this.destinationDetailsHeader.SetParent((this.selectedMenuTabIdx == 0) ? this.destinationDetailsParent_Asteroid : this.destinationDetailsParent_Story);
		this.destinationDetailsHeader.SetAsFirstSibling();
	}

	// Token: 0x0600558A RID: 21898 RVA: 0x001F2048 File Offset: 0x001F0248
	private void ShuffleClicked()
	{
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		int num = this.random.Next();
		if (currentClusterLayout != null && currentClusterLayout.fixedCoordinate != -1)
		{
			num = currentClusterLayout.fixedCoordinate;
		}
		this.newGameSettings.SetSetting(CustomGameSettingConfigs.WorldgenSeed, num.ToString(), true);
	}

	// Token: 0x0600558B RID: 21899 RVA: 0x001F2097 File Offset: 0x001F0297
	private void StoryTraitShuffleClicked()
	{
		this.storyContentPanel.SelectRandomStories(4, 4, false);
	}

	// Token: 0x0600558C RID: 21900 RVA: 0x001F20A8 File Offset: 0x001F02A8
	private void CoordinateChanged(string text)
	{
		string[] array = CustomGameSettings.ParseSettingCoordinate(text);
		if (array.Length != 4 && array.Length != 5)
		{
			return;
		}
		int num;
		if (!int.TryParse(array[2], out num))
		{
			return;
		}
		ClusterLayout clusterLayout = null;
		foreach (string name in SettingsCache.GetClusterNames())
		{
			ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(name);
			if (clusterData.coordinatePrefix == array[1])
			{
				clusterLayout = clusterData;
			}
		}
		if (clusterLayout != null)
		{
			this.newGameSettings.SetSetting(CustomGameSettingConfigs.ClusterLayout, clusterLayout.filePath, true);
		}
		this.newGameSettings.SetSetting(CustomGameSettingConfigs.WorldgenSeed, array[2], true);
		this.newGameSettings.ConsumeSettingsCode(array[3]);
		string code = (array.Length >= 5) ? array[4] : "0";
		this.newGameSettings.ConsumeStoryTraitsCode(code);
	}

	// Token: 0x0600558D RID: 21901 RVA: 0x001F2194 File Offset: 0x001F0394
	private void CoordinateEditStarted()
	{
		this.isEditingCoordinate = true;
	}

	// Token: 0x0600558E RID: 21902 RVA: 0x001F219D File Offset: 0x001F039D
	private void CoordinateEditFinished(string text)
	{
		this.CoordinateChanged(text);
		this.isEditingCoordinate = false;
		this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
	}

	// Token: 0x0600558F RID: 21903 RVA: 0x001F21C2 File Offset: 0x001F03C2
	private void QualitySettingChanged(SettingConfig config, SettingLevel level)
	{
		if (config == CustomGameSettingConfigs.SaveToCloud)
		{
			this.RefreshCloudLocalIcon();
		}
		if (!this.isEditingCoordinate)
		{
			this.coordinate.text = CustomGameSettings.Instance.GetSettingsCoordinate();
		}
		this.RefreshRowsAndDescriptions();
	}

	// Token: 0x06005590 RID: 21904 RVA: 0x001F21F8 File Offset: 0x001F03F8
	public void RefreshRowsAndDescriptions()
	{
		string setting = this.newGameSettings.GetSetting(CustomGameSettingConfigs.ClusterLayout);
		string setting2 = this.newGameSettings.GetSetting(CustomGameSettingConfigs.WorldgenSeed);
		this.destinationMapPanel.UpdateDisplayedClusters();
		int fixedCoordinate;
		int.TryParse(setting2, out fixedCoordinate);
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		if (currentClusterLayout.fixedCoordinate != -1)
		{
			this.newGameSettings.SetSetting(CustomGameSettingConfigs.WorldgenSeed, currentClusterLayout.fixedCoordinate.ToString(), false);
			fixedCoordinate = currentClusterLayout.fixedCoordinate;
			this.shuffleButton.isInteractable = false;
			this.shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP_DISABLED);
		}
		else
		{
			this.coordinate.interactable = true;
			this.shuffleButton.isInteractable = true;
			this.shuffleButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.COLONYDESTINATIONSCREEN.SHUFFLETOOLTIP);
		}
		ColonyDestinationAsteroidBeltData cluster;
		try
		{
			cluster = this.destinationMapPanel.SelectCluster(setting, fixedCoordinate);
		}
		catch
		{
			string defaultAsteroid = this.destinationMapPanel.GetDefaultAsteroid();
			this.newGameSettings.SetSetting(CustomGameSettingConfigs.ClusterLayout, defaultAsteroid, true);
			cluster = this.destinationMapPanel.SelectCluster(defaultAsteroid, fixedCoordinate);
		}
		if (DlcManager.IsContentActive("EXPANSION1_ID"))
		{
			this.destinationProperties.EnableClusterLocationLabels(true);
			this.destinationProperties.RefreshAsteroidLines(cluster, this.selectedLocationProperties, this.storyContentPanel.GetActiveStories());
			this.destinationProperties.EnableClusterDetails(true);
			this.destinationProperties.SetClusterDetailLabels(cluster);
			this.selectedLocationProperties.headerLabel.SetText(UI.FRONTEND.COLONYDESTINATIONSCREEN.SELECTED_CLUSTER_TRAITS_HEADER);
			this.destinationProperties.clusterDetailsButton.onClick = delegate()
			{
				this.destinationProperties.SelectWholeClusterDetails(cluster, this.selectedLocationProperties, this.storyContentPanel.GetActiveStories());
			};
		}
		else
		{
			this.destinationProperties.EnableClusterDetails(false);
			this.destinationProperties.EnableClusterLocationLabels(false);
			this.destinationProperties.SetParameterDescriptors(cluster.GetParamDescriptors());
			this.selectedLocationProperties.SetTraitDescriptors(cluster.GetTraitDescriptors(), this.storyContentPanel.GetActiveStories(), true);
		}
		this.RefreshStoryLabel();
	}

	// Token: 0x06005591 RID: 21905 RVA: 0x001F2420 File Offset: 0x001F0620
	public void RefreshStoryLabel()
	{
		this.storyTraitsDestinationDetailsLabel.SetText(this.storyContentPanel.GetTraitsString(false));
		this.storyTraitsDestinationDetailsLabel.GetComponent<ToolTip>().SetSimpleTooltip(this.storyContentPanel.GetTraitsString(true));
	}

	// Token: 0x06005592 RID: 21906 RVA: 0x001F2455 File Offset: 0x001F0655
	private void OnAsteroidClicked(ColonyDestinationAsteroidBeltData cluster)
	{
		this.newGameSettings.SetSetting(CustomGameSettingConfigs.ClusterLayout, cluster.beltPath, true);
		this.ShuffleClicked();
	}

	// Token: 0x06005593 RID: 21907 RVA: 0x001F2474 File Offset: 0x001F0674
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.isEditingCoordinate)
		{
			return;
		}
		if (!e.Consumed && e.TryConsume(global::Action.PanLeft))
		{
			this.destinationMapPanel.ScrollLeft();
		}
		else if (!e.Consumed && e.TryConsume(global::Action.PanRight))
		{
			this.destinationMapPanel.ScrollRight();
		}
		else if (this.customSettings.activeSelf && !e.Consumed && (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight)))
		{
			this.CustomizeClose();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04003901 RID: 14593
	[SerializeField]
	private GameObject destinationMap;

	// Token: 0x04003902 RID: 14594
	[SerializeField]
	private GameObject customSettings;

	// Token: 0x04003903 RID: 14595
	[SerializeField]
	private MultiToggle[] menuTabs;

	// Token: 0x04003904 RID: 14596
	private int selectedMenuTabIdx;

	// Token: 0x04003905 RID: 14597
	[SerializeField]
	private KButton backButton;

	// Token: 0x04003906 RID: 14598
	[SerializeField]
	private KButton customizeButton;

	// Token: 0x04003907 RID: 14599
	[SerializeField]
	private KButton launchButton;

	// Token: 0x04003908 RID: 14600
	[SerializeField]
	private KButton shuffleButton;

	// Token: 0x04003909 RID: 14601
	[SerializeField]
	private KButton storyTraitShuffleButton;

	// Token: 0x0400390A RID: 14602
	[SerializeField]
	private HierarchyReferences locationIcons;

	// Token: 0x0400390B RID: 14603
	[SerializeField]
	private RectTransform worldsScrollPanel;

	// Token: 0x0400390C RID: 14604
	[SerializeField]
	private RectTransform storyScrollPanel;

	// Token: 0x0400390D RID: 14605
	[SerializeField]
	private RectTransform destinationDetailsParent_Asteroid;

	// Token: 0x0400390E RID: 14606
	[SerializeField]
	private RectTransform destinationDetailsParent_Story;

	// Token: 0x0400390F RID: 14607
	[SerializeField]
	private LocText storyTraitsDestinationDetailsLabel;

	// Token: 0x04003910 RID: 14608
	private const int DESTINATION_HEADER_BUTTON_HEIGHT_CLUSTER = 164;

	// Token: 0x04003911 RID: 14609
	private const int DESTINATION_HEADER_BUTTON_HEIGHT_BASE = 76;

	// Token: 0x04003912 RID: 14610
	private const int WORLDS_SCROLL_PANEL_HEIGHT_CLUSTER = 436;

	// Token: 0x04003913 RID: 14611
	private const int WORLDS_SCROLL_PANEL_HEIGHT_BASE = 524;

	// Token: 0x04003914 RID: 14612
	[SerializeField]
	private AsteroidDescriptorPanel destinationProperties;

	// Token: 0x04003915 RID: 14613
	[SerializeField]
	private AsteroidDescriptorPanel selectedLocationProperties;

	// Token: 0x04003916 RID: 14614
	[SerializeField]
	private KInputTextField coordinate;

	// Token: 0x04003917 RID: 14615
	[SerializeField]
	private RectTransform destinationDetailsHeader;

	// Token: 0x04003918 RID: 14616
	[SerializeField]
	private RectTransform destinationInfoPanel;

	// Token: 0x04003919 RID: 14617
	[SerializeField]
	private RectTransform storyInfoPanel;

	// Token: 0x0400391A RID: 14618
	[MyCmpReq]
	public NewGameSettingsPanel newGameSettings;

	// Token: 0x0400391B RID: 14619
	[MyCmpReq]
	private DestinationSelectPanel destinationMapPanel;

	// Token: 0x0400391C RID: 14620
	[SerializeField]
	private StoryContentPanel storyContentPanel;

	// Token: 0x0400391D RID: 14621
	private KRandom random;

	// Token: 0x0400391E RID: 14622
	private bool isEditingCoordinate;
}
