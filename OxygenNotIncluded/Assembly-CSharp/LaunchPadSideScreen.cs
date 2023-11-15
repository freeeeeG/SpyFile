using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C27 RID: 3111
public class LaunchPadSideScreen : SideScreenContent
{
	// Token: 0x06006262 RID: 25186 RVA: 0x00245364 File Offset: 0x00243564
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.startNewRocketbutton.onClick += this.ClickStartNewRocket;
		this.devAutoRocketButton.onClick += this.ClickAutoRocket;
	}

	// Token: 0x06006263 RID: 25187 RVA: 0x0024539A File Offset: 0x0024359A
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
		}
	}

	// Token: 0x06006264 RID: 25188 RVA: 0x002453B0 File Offset: 0x002435B0
	public override int GetSideScreenSortOrder()
	{
		return 100;
	}

	// Token: 0x06006265 RID: 25189 RVA: 0x002453B4 File Offset: 0x002435B4
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LaunchPad>() != null;
	}

	// Token: 0x06006266 RID: 25190 RVA: 0x002453C4 File Offset: 0x002435C4
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		if (this.refreshEventHandle != -1)
		{
			this.selectedPad.Unsubscribe(this.refreshEventHandle);
		}
		this.selectedPad = new_target.GetComponent<LaunchPad>();
		if (this.selectedPad == null)
		{
			global::Debug.LogError("The gameObject received does not contain a LaunchPad component");
			return;
		}
		this.refreshEventHandle = this.selectedPad.Subscribe(-887025858, new Action<object>(this.RefreshWaitingToLandList));
		this.RefreshRocketButton();
		this.RefreshWaitingToLandList(null);
	}

	// Token: 0x06006267 RID: 25191 RVA: 0x00245454 File Offset: 0x00243654
	private void RefreshWaitingToLandList(object data = null)
	{
		for (int i = this.waitingToLandRows.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.waitingToLandRows[i]);
		}
		this.waitingToLandRows.Clear();
		this.nothingWaitingRow.SetActive(true);
		AxialI myWorldLocation = this.selectedPad.GetMyWorldLocation();
		foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetEntitiesInRange(myWorldLocation, 1))
		{
			Clustercraft craft = clusterGridEntity as Clustercraft;
			if (!(craft == null) && craft.Status == Clustercraft.CraftStatus.InFlight && (!craft.IsFlightInProgress() || !(craft.Destination != myWorldLocation)))
			{
				GameObject gameObject = Util.KInstantiateUI(this.landableRocketRowPrefab, this.landableRowContainer, true);
				gameObject.GetComponentInChildren<LocText>().text = craft.Name;
				this.waitingToLandRows.Add(gameObject);
				KButton componentInChildren = gameObject.GetComponentInChildren<KButton>();
				componentInChildren.GetComponentInChildren<LocText>().SetText((craft.ModuleInterface.GetClusterDestinationSelector().GetDestinationPad() == this.selectedPad) ? UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.CANCEL_LAND_BUTTON : UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAND_BUTTON);
				string simpleTooltip;
				componentInChildren.isInteractable = (craft.CanLandAtPad(this.selectedPad, out simpleTooltip) != Clustercraft.PadLandingStatus.CanNeverLand);
				if (!componentInChildren.isInteractable)
				{
					componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
				}
				else
				{
					componentInChildren.GetComponent<ToolTip>().ClearMultiStringTooltip();
				}
				componentInChildren.onClick += delegate()
				{
					if (craft.ModuleInterface.GetClusterDestinationSelector().GetDestinationPad() == this.selectedPad)
					{
						craft.GetComponent<ClusterDestinationSelector>().SetDestination(craft.Location);
					}
					else
					{
						craft.LandAtPad(this.selectedPad);
					}
					this.RefreshWaitingToLandList(null);
				};
				this.nothingWaitingRow.SetActive(false);
			}
		}
	}

	// Token: 0x06006268 RID: 25192 RVA: 0x00245654 File Offset: 0x00243854
	private void ClickStartNewRocket()
	{
		((SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL)).SetLaunchPad(this.selectedPad);
	}

	// Token: 0x06006269 RID: 25193 RVA: 0x00245680 File Offset: 0x00243880
	private void RefreshRocketButton()
	{
		bool isOperational = this.selectedPad.GetComponent<Operational>().IsOperational;
		this.startNewRocketbutton.isInteractable = (this.selectedPad.LandedRocket == null && isOperational);
		if (!isOperational)
		{
			this.startNewRocketbutton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PAD_DISABLED);
		}
		else
		{
			this.startNewRocketbutton.GetComponent<ToolTip>().ClearMultiStringTooltip();
		}
		this.devAutoRocketButton.isInteractable = (this.selectedPad.LandedRocket == null);
		this.devAutoRocketButton.gameObject.SetActive(DebugHandler.InstantBuildMode);
	}

	// Token: 0x0600626A RID: 25194 RVA: 0x0024571C File Offset: 0x0024391C
	private void ClickAutoRocket()
	{
		AutoRocketUtility.StartAutoRocket(this.selectedPad);
	}

	// Token: 0x0400430D RID: 17165
	public GameObject content;

	// Token: 0x0400430E RID: 17166
	private LaunchPad selectedPad;

	// Token: 0x0400430F RID: 17167
	public LocText DescriptionText;

	// Token: 0x04004310 RID: 17168
	public GameObject landableRocketRowPrefab;

	// Token: 0x04004311 RID: 17169
	public GameObject newRocketPanel;

	// Token: 0x04004312 RID: 17170
	public KButton startNewRocketbutton;

	// Token: 0x04004313 RID: 17171
	public KButton devAutoRocketButton;

	// Token: 0x04004314 RID: 17172
	public GameObject landableRowContainer;

	// Token: 0x04004315 RID: 17173
	public GameObject nothingWaitingRow;

	// Token: 0x04004316 RID: 17174
	public KScreen changeModuleSideScreen;

	// Token: 0x04004317 RID: 17175
	private int refreshEventHandle = -1;

	// Token: 0x04004318 RID: 17176
	public List<GameObject> waitingToLandRows = new List<GameObject>();
}
