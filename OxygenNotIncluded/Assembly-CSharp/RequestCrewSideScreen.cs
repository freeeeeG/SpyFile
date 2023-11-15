using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C3F RID: 3135
public class RequestCrewSideScreen : SideScreenContent
{
	// Token: 0x06006343 RID: 25411 RVA: 0x0024B02C File Offset: 0x0024922C
	protected override void OnSpawn()
	{
		this.changeCrewButton.onClick += this.OnChangeCrewButtonPressed;
		this.crewReleaseButton.onClick += this.CrewRelease;
		this.crewRequestButton.onClick += this.CrewRequest;
		this.toggleMap.Add(this.crewReleaseButton, PassengerRocketModule.RequestCrewState.Release);
		this.toggleMap.Add(this.crewRequestButton, PassengerRocketModule.RequestCrewState.Request);
		this.Refresh();
	}

	// Token: 0x06006344 RID: 25412 RVA: 0x0024B0A8 File Offset: 0x002492A8
	public override int GetSideScreenSortOrder()
	{
		return 100;
	}

	// Token: 0x06006345 RID: 25413 RVA: 0x0024B0AC File Offset: 0x002492AC
	public override bool IsValidForTarget(GameObject target)
	{
		PassengerRocketModule component = target.GetComponent<PassengerRocketModule>();
		RocketControlStation component2 = target.GetComponent<RocketControlStation>();
		if (component != null)
		{
			return component.GetMyWorld() != null;
		}
		if (component2 != null)
		{
			RocketControlStation.StatesInstance smi = component2.GetSMI<RocketControlStation.StatesInstance>();
			return !smi.sm.IsInFlight(smi) && !smi.sm.IsLaunching(smi);
		}
		return false;
	}

	// Token: 0x06006346 RID: 25414 RVA: 0x0024B10E File Offset: 0x0024930E
	public override void SetTarget(GameObject target)
	{
		if (target.GetComponent<RocketControlStation>() != null)
		{
			this.rocketModule = target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule();
		}
		else
		{
			this.rocketModule = target.GetComponent<PassengerRocketModule>();
		}
		this.Refresh();
	}

	// Token: 0x06006347 RID: 25415 RVA: 0x0024B14D File Offset: 0x0024934D
	private void Refresh()
	{
		this.RefreshRequestButtons();
	}

	// Token: 0x06006348 RID: 25416 RVA: 0x0024B155 File Offset: 0x00249355
	private void CrewRelease()
	{
		this.rocketModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Release);
		this.RefreshRequestButtons();
	}

	// Token: 0x06006349 RID: 25417 RVA: 0x0024B169 File Offset: 0x00249369
	private void CrewRequest()
	{
		this.rocketModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Request);
		this.RefreshRequestButtons();
	}

	// Token: 0x0600634A RID: 25418 RVA: 0x0024B180 File Offset: 0x00249380
	private void RefreshRequestButtons()
	{
		foreach (KeyValuePair<KToggle, PassengerRocketModule.RequestCrewState> keyValuePair in this.toggleMap)
		{
			this.RefreshRequestButton(keyValuePair.Key);
		}
	}

	// Token: 0x0600634B RID: 25419 RVA: 0x0024B1DC File Offset: 0x002493DC
	private void RefreshRequestButton(KToggle button)
	{
		ImageToggleState[] componentsInChildren;
		if (this.toggleMap[button] == this.rocketModule.PassengersRequested)
		{
			button.isOn = true;
			componentsInChildren = button.GetComponentsInChildren<ImageToggleState>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetActive();
			}
			button.GetComponent<ImageToggleStateThrobber>().enabled = false;
			return;
		}
		button.isOn = false;
		componentsInChildren = button.GetComponentsInChildren<ImageToggleState>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetInactive();
		}
		button.GetComponent<ImageToggleStateThrobber>().enabled = false;
	}

	// Token: 0x0600634C RID: 25420 RVA: 0x0024B264 File Offset: 0x00249464
	private void OnChangeCrewButtonPressed()
	{
		if (this.activeChangeCrewSideScreen == null)
		{
			this.activeChangeCrewSideScreen = (AssignmentGroupControllerSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeCrewSideScreenPrefab, UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TITLE);
			this.activeChangeCrewSideScreen.SetTarget(this.rocketModule.gameObject);
			return;
		}
		DetailsScreen.Instance.ClearSecondarySideScreen();
		this.activeChangeCrewSideScreen = null;
	}

	// Token: 0x0600634D RID: 25421 RVA: 0x0024B2CC File Offset: 0x002494CC
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
			this.activeChangeCrewSideScreen = null;
		}
	}

	// Token: 0x040043BE RID: 17342
	private PassengerRocketModule rocketModule;

	// Token: 0x040043BF RID: 17343
	public KToggle crewReleaseButton;

	// Token: 0x040043C0 RID: 17344
	public KToggle crewRequestButton;

	// Token: 0x040043C1 RID: 17345
	private Dictionary<KToggle, PassengerRocketModule.RequestCrewState> toggleMap = new Dictionary<KToggle, PassengerRocketModule.RequestCrewState>();

	// Token: 0x040043C2 RID: 17346
	public KButton changeCrewButton;

	// Token: 0x040043C3 RID: 17347
	public KScreen changeCrewSideScreenPrefab;

	// Token: 0x040043C4 RID: 17348
	private AssignmentGroupControllerSideScreen activeChangeCrewSideScreen;
}
