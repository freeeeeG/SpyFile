using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C26 RID: 3110
public class LaunchButtonSideScreen : SideScreenContent
{
	// Token: 0x06006258 RID: 25176 RVA: 0x00244D33 File Offset: 0x00242F33
	protected override void OnSpawn()
	{
		this.Refresh();
		this.launchButton.onClick += this.TriggerLaunch;
	}

	// Token: 0x06006259 RID: 25177 RVA: 0x00244D52 File Offset: 0x00242F52
	public override int GetSideScreenSortOrder()
	{
		return -100;
	}

	// Token: 0x0600625A RID: 25178 RVA: 0x00244D56 File Offset: 0x00242F56
	public override bool IsValidForTarget(GameObject target)
	{
		return (target.GetComponent<RocketModule>() != null && target.HasTag(GameTags.LaunchButtonRocketModule)) || (target.GetComponent<LaunchPad>() && target.GetComponent<LaunchPad>().HasRocketWithCommandModule());
	}

	// Token: 0x0600625B RID: 25179 RVA: 0x00244D90 File Offset: 0x00242F90
	public override void SetTarget(GameObject target)
	{
		bool flag = this.rocketModule == null || this.rocketModule.gameObject != target;
		this.selectedPad = null;
		this.rocketModule = target.GetComponent<RocketModuleCluster>();
		if (this.rocketModule == null)
		{
			this.selectedPad = target.GetComponent<LaunchPad>();
			if (this.selectedPad != null)
			{
				foreach (Ref<RocketModuleCluster> @ref in this.selectedPad.LandedRocket.CraftInterface.ClusterModules)
				{
					if (@ref.Get().GetComponent<LaunchableRocketCluster>())
					{
						this.rocketModule = @ref.Get().GetComponent<RocketModuleCluster>();
						break;
					}
				}
			}
		}
		if (this.selectedPad == null)
		{
			CraftModuleInterface craftInterface = this.rocketModule.CraftInterface;
			this.selectedPad = craftInterface.CurrentPad;
		}
		if (flag)
		{
			this.acknowledgeWarnings = false;
		}
		this.rocketModule.CraftInterface.Subscribe<LaunchButtonSideScreen>(543433792, LaunchButtonSideScreen.RefreshDelegate);
		this.rocketModule.CraftInterface.Subscribe<LaunchButtonSideScreen>(1655598572, LaunchButtonSideScreen.RefreshDelegate);
		this.Refresh();
	}

	// Token: 0x0600625C RID: 25180 RVA: 0x00244ED4 File Offset: 0x002430D4
	public override void ClearTarget()
	{
		if (this.rocketModule != null)
		{
			this.rocketModule.CraftInterface.Unsubscribe<LaunchButtonSideScreen>(543433792, LaunchButtonSideScreen.RefreshDelegate, false);
			this.rocketModule.CraftInterface.Unsubscribe<LaunchButtonSideScreen>(1655598572, LaunchButtonSideScreen.RefreshDelegate, false);
			this.rocketModule = null;
		}
	}

	// Token: 0x0600625D RID: 25181 RVA: 0x00244F2C File Offset: 0x0024312C
	private void TriggerLaunch()
	{
		bool flag = !this.acknowledgeWarnings && this.rocketModule.CraftInterface.HasLaunchWarnings();
		bool flag2 = this.rocketModule.CraftInterface.IsLaunchRequested();
		if (flag)
		{
			this.acknowledgeWarnings = true;
		}
		else if (flag2)
		{
			this.rocketModule.CraftInterface.CancelLaunch();
			this.acknowledgeWarnings = false;
		}
		else
		{
			this.rocketModule.CraftInterface.TriggerLaunch(false);
		}
		this.Refresh();
	}

	// Token: 0x0600625E RID: 25182 RVA: 0x00244FA3 File Offset: 0x002431A3
	public void Update()
	{
		if (Time.unscaledTime > this.lastRefreshTime + 1f)
		{
			this.lastRefreshTime = Time.unscaledTime;
			this.Refresh();
		}
	}

	// Token: 0x0600625F RID: 25183 RVA: 0x00244FCC File Offset: 0x002431CC
	private void Refresh()
	{
		if (this.rocketModule == null || this.selectedPad == null)
		{
			return;
		}
		bool flag = !this.acknowledgeWarnings && this.rocketModule.CraftInterface.HasLaunchWarnings();
		bool flag2 = this.rocketModule.CraftInterface.IsLaunchRequested();
		bool flag3 = this.selectedPad.IsLogicInputConnected();
		bool flag4 = flag3 ? this.rocketModule.CraftInterface.CheckReadyForAutomatedLaunchCommand() : this.rocketModule.CraftInterface.CheckPreppedForLaunch();
		if (flag3)
		{
			this.launchButton.isInteractable = false;
			this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_AUTOMATION_CONTROLLED;
			this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_AUTOMATION_CONTROLLED_TOOLTIP;
		}
		else if (DebugHandler.InstantBuildMode || flag4)
		{
			this.launchButton.isInteractable = true;
			if (flag2)
			{
				this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_REQUESTED_BUTTON;
				this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_REQUESTED_BUTTON_TOOLTIP;
			}
			else if (flag)
			{
				this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_WARNINGS_BUTTON;
				this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_WARNINGS_BUTTON_TOOLTIP;
			}
			else
			{
				LocString loc_string = DebugHandler.InstantBuildMode ? UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_DEBUG : UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON;
				this.launchButton.GetComponentInChildren<LocText>().text = loc_string;
				this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_TOOLTIP;
			}
		}
		else
		{
			this.launchButton.isInteractable = false;
			this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON;
			this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_NOT_READY_TOOLTIP;
		}
		if (this.rocketModule.CraftInterface.GetInteriorWorld() == null)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
			return;
		}
		PassengerRocketModule component = this.rocketModule.GetComponent<PassengerRocketModule>();
		List<RocketControlStation> worldItems = Components.RocketControlStations.GetWorldItems(this.rocketModule.CraftInterface.GetInteriorWorld().id, false);
		RocketControlStationLaunchWorkable rocketControlStationLaunchWorkable = null;
		if (worldItems != null && worldItems.Count > 0)
		{
			rocketControlStationLaunchWorkable = worldItems[0].GetComponent<RocketControlStationLaunchWorkable>();
		}
		if (component == null || rocketControlStationLaunchWorkable == null)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
			return;
		}
		bool flag5 = component.CheckPassengersBoarded();
		bool flag6 = !component.CheckExtraPassengers();
		bool flag7 = rocketControlStationLaunchWorkable.worker != null;
		bool flag8 = this.rocketModule.CraftInterface.HasTag(GameTags.RocketNotOnGround);
		if (!flag4)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
			return;
		}
		if (!flag2)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.READY_FOR_LAUNCH;
			return;
		}
		if (!flag5)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.LOADING_CREW;
			return;
		}
		if (!flag6)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.UNLOADING_PASSENGERS;
			return;
		}
		if (!flag7)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.WAITING_FOR_PILOT;
			return;
		}
		if (!flag8)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.COUNTING_DOWN;
			return;
		}
		this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.TAKING_OFF;
	}

	// Token: 0x04004305 RID: 17157
	public KButton launchButton;

	// Token: 0x04004306 RID: 17158
	public LocText statusText;

	// Token: 0x04004307 RID: 17159
	private RocketModuleCluster rocketModule;

	// Token: 0x04004308 RID: 17160
	private LaunchPad selectedPad;

	// Token: 0x04004309 RID: 17161
	private bool acknowledgeWarnings;

	// Token: 0x0400430A RID: 17162
	private float lastRefreshTime;

	// Token: 0x0400430B RID: 17163
	private const float UPDATE_FREQUENCY = 1f;

	// Token: 0x0400430C RID: 17164
	private static readonly EventSystem.IntraObjectHandler<LaunchButtonSideScreen> RefreshDelegate = new EventSystem.IntraObjectHandler<LaunchButtonSideScreen>(delegate(LaunchButtonSideScreen cmp, object data)
	{
		cmp.Refresh();
	});
}
