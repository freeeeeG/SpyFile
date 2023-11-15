using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C05 RID: 3077
public class ClusterDestinationSideScreen : SideScreenContent
{
	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x06006167 RID: 24935 RVA: 0x0023F1FF File Offset: 0x0023D3FF
	// (set) Token: 0x06006168 RID: 24936 RVA: 0x0023F207 File Offset: 0x0023D407
	private ClusterDestinationSelector targetSelector { get; set; }

	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06006169 RID: 24937 RVA: 0x0023F210 File Offset: 0x0023D410
	// (set) Token: 0x0600616A RID: 24938 RVA: 0x0023F218 File Offset: 0x0023D418
	private RocketClusterDestinationSelector targetRocketSelector { get; set; }

	// Token: 0x0600616B RID: 24939 RVA: 0x0023F224 File Offset: 0x0023D424
	protected override void OnSpawn()
	{
		this.changeDestinationButton.onClick += this.OnClickChangeDestination;
		this.clearDestinationButton.onClick += this.OnClickClearDestination;
		this.launchPadDropDown.targetDropDownContainer = GameScreenManager.Instance.ssOverlayCanvas;
		this.launchPadDropDown.CustomizeEmptyRow(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE, null);
		this.repeatButton.onClick += this.OnRepeatClicked;
	}

	// Token: 0x0600616C RID: 24940 RVA: 0x0023F2A1 File Offset: 0x0023D4A1
	public override int GetSideScreenSortOrder()
	{
		return 300;
	}

	// Token: 0x0600616D RID: 24941 RVA: 0x0023F2A8 File Offset: 0x0023D4A8
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.Refresh(null);
			this.m_refreshHandle = this.targetSelector.Subscribe(543433792, delegate(object data)
			{
				this.Refresh(null);
			});
			return;
		}
		if (this.m_refreshHandle != -1)
		{
			this.targetSelector.Unsubscribe(this.m_refreshHandle);
			this.m_refreshHandle = -1;
			this.launchPadDropDown.Close();
		}
	}

	// Token: 0x0600616E RID: 24942 RVA: 0x0023F318 File Offset: 0x0023D518
	public override bool IsValidForTarget(GameObject target)
	{
		ClusterDestinationSelector component = target.GetComponent<ClusterDestinationSelector>();
		return (component != null && component.assignable) || (target.GetComponent<RocketModule>() != null && target.HasTag(GameTags.LaunchButtonRocketModule)) || (target.GetComponent<RocketControlStation>() != null && target.GetComponent<RocketControlStation>().GetMyWorld().GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Launching);
	}

	// Token: 0x0600616F RID: 24943 RVA: 0x0023F388 File Offset: 0x0023D588
	public override void SetTarget(GameObject target)
	{
		this.targetSelector = target.GetComponent<ClusterDestinationSelector>();
		if (this.targetSelector == null)
		{
			if (target.GetComponent<RocketModuleCluster>() != null)
			{
				this.targetSelector = target.GetComponent<RocketModuleCluster>().CraftInterface.GetClusterDestinationSelector();
			}
			else if (target.GetComponent<RocketControlStation>() != null)
			{
				this.targetSelector = target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface.GetClusterDestinationSelector();
			}
		}
		this.targetRocketSelector = (this.targetSelector as RocketClusterDestinationSelector);
	}

	// Token: 0x06006170 RID: 24944 RVA: 0x0023F410 File Offset: 0x0023D610
	private void Refresh(object data = null)
	{
		if (!this.targetSelector.IsAtDestination())
		{
			Sprite sprite;
			string str;
			string text;
			ClusterGrid.Instance.GetLocationDescription(this.targetSelector.GetDestination(), out sprite, out str, out text);
			this.destinationImage.sprite = sprite;
			this.destinationLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.TITLE + ": " + str;
			this.clearDestinationButton.isInteractable = true;
		}
		else
		{
			this.destinationImage.sprite = Assets.GetSprite("hex_unknown");
			this.destinationLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.TITLE + ": " + UI.SPACEDESTINATIONS.NONE.NAME;
			this.clearDestinationButton.isInteractable = false;
		}
		if (this.targetRocketSelector != null)
		{
			List<LaunchPad> launchPadsForDestination = LaunchPad.GetLaunchPadsForDestination(this.targetRocketSelector.GetDestination());
			this.launchPadDropDown.gameObject.SetActive(true);
			this.repeatButton.gameObject.SetActive(true);
			this.launchPadDropDown.Initialize(launchPadsForDestination, new Action<IListableOption, object>(this.OnLaunchPadEntryClick), new Func<IListableOption, IListableOption, object, int>(this.PadDropDownSort), new Action<DropDownEntry, object>(this.PadDropDownEntryRefreshAction), true, this.targetRocketSelector);
			if (!this.targetRocketSelector.IsAtDestination() && launchPadsForDestination.Count > 0)
			{
				this.launchPadDropDown.openButton.isInteractable = true;
				LaunchPad destinationPad = this.targetRocketSelector.GetDestinationPad();
				if (destinationPad != null)
				{
					this.launchPadDropDown.selectedLabel.text = destinationPad.GetProperName();
				}
				else
				{
					this.launchPadDropDown.selectedLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE;
				}
			}
			else
			{
				this.launchPadDropDown.selectedLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE;
				this.launchPadDropDown.openButton.isInteractable = false;
			}
			this.StyleRepeatButton();
		}
		else
		{
			this.launchPadDropDown.gameObject.SetActive(false);
			this.repeatButton.gameObject.SetActive(false);
		}
		this.StyleChangeDestinationButton();
	}

	// Token: 0x06006171 RID: 24945 RVA: 0x0023F617 File Offset: 0x0023D817
	private void OnClickChangeDestination()
	{
		if (this.targetSelector.assignable)
		{
			ClusterMapScreen.Instance.ShowInSelectDestinationMode(this.targetSelector);
		}
		this.StyleChangeDestinationButton();
	}

	// Token: 0x06006172 RID: 24946 RVA: 0x0023F63C File Offset: 0x0023D83C
	private void StyleChangeDestinationButton()
	{
	}

	// Token: 0x06006173 RID: 24947 RVA: 0x0023F63E File Offset: 0x0023D83E
	private void OnClickClearDestination()
	{
		this.targetSelector.SetDestination(this.targetSelector.GetMyWorldLocation());
	}

	// Token: 0x06006174 RID: 24948 RVA: 0x0023F658 File Offset: 0x0023D858
	private void OnLaunchPadEntryClick(IListableOption option, object data)
	{
		LaunchPad destinationPad = (LaunchPad)option;
		this.targetRocketSelector.SetDestinationPad(destinationPad);
	}

	// Token: 0x06006175 RID: 24949 RVA: 0x0023F678 File Offset: 0x0023D878
	private void PadDropDownEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		LaunchPad launchPad = (LaunchPad)entry.entryData;
		Clustercraft component = this.targetRocketSelector.GetComponent<Clustercraft>();
		if (!(launchPad != null))
		{
			entry.button.isInteractable = true;
			entry.image.sprite = Assets.GetBuildingDef("LaunchPad").GetUISprite("ui", false);
			entry.tooltip.SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_FIRST_AVAILABLE);
			return;
		}
		string simpleTooltip;
		if (component.CanLandAtPad(launchPad, out simpleTooltip) == Clustercraft.PadLandingStatus.CanNeverLand)
		{
			entry.button.isInteractable = false;
			entry.image.sprite = Assets.GetSprite("iconWarning");
			entry.tooltip.SetSimpleTooltip(simpleTooltip);
			return;
		}
		entry.button.isInteractable = true;
		entry.image.sprite = launchPad.GetComponent<Building>().Def.GetUISprite("ui", false);
		entry.tooltip.SetSimpleTooltip(string.Format(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_VALID_SITE, launchPad.GetProperName()));
	}

	// Token: 0x06006176 RID: 24950 RVA: 0x0023F777 File Offset: 0x0023D977
	private int PadDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return 0;
	}

	// Token: 0x06006177 RID: 24951 RVA: 0x0023F77A File Offset: 0x0023D97A
	private void OnRepeatClicked()
	{
		this.targetRocketSelector.Repeat = !this.targetRocketSelector.Repeat;
		this.StyleRepeatButton();
	}

	// Token: 0x06006178 RID: 24952 RVA: 0x0023F79B File Offset: 0x0023D99B
	private void StyleRepeatButton()
	{
		this.repeatButton.bgImage.colorStyleSetting = (this.targetRocketSelector.Repeat ? this.repeatOn : this.repeatOff);
		this.repeatButton.bgImage.ApplyColorStyleSetting();
	}

	// Token: 0x04004251 RID: 16977
	public Image destinationImage;

	// Token: 0x04004252 RID: 16978
	public LocText destinationLabel;

	// Token: 0x04004253 RID: 16979
	public KButton changeDestinationButton;

	// Token: 0x04004254 RID: 16980
	public KButton clearDestinationButton;

	// Token: 0x04004255 RID: 16981
	public DropDown launchPadDropDown;

	// Token: 0x04004256 RID: 16982
	public KButton repeatButton;

	// Token: 0x04004257 RID: 16983
	public ColorStyleSetting repeatOff;

	// Token: 0x04004258 RID: 16984
	public ColorStyleSetting repeatOn;

	// Token: 0x04004259 RID: 16985
	public ColorStyleSetting defaultButton;

	// Token: 0x0400425A RID: 16986
	public ColorStyleSetting highlightButton;

	// Token: 0x0400425D RID: 16989
	private int m_refreshHandle = -1;
}
