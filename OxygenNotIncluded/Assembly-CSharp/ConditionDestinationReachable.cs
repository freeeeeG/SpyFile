using System;
using STRINGS;

// Token: 0x020009CD RID: 2509
public class ConditionDestinationReachable : ProcessCondition
{
	// Token: 0x06004B17 RID: 19223 RVA: 0x001A6A87 File Offset: 0x001A4C87
	public ConditionDestinationReachable(RocketModule module)
	{
		this.module = module;
		this.craftRegisterType = module.GetComponent<ILaunchableRocket>().registerType;
	}

	// Token: 0x06004B18 RID: 19224 RVA: 0x001A6AA8 File Offset: 0x001A4CA8
	public override ProcessCondition.Status EvaluateCondition()
	{
		ProcessCondition.Status result = ProcessCondition.Status.Failure;
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				if (!this.module.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<RocketClusterDestinationSelector>().IsAtDestination())
				{
					result = ProcessCondition.Status.Ready;
				}
			}
		}
		else
		{
			int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
			SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
			if (spacecraftDestination != null && this.CanReachSpacecraftDestination(spacecraftDestination) && spacecraftDestination.GetDestinationType().visitable)
			{
				result = ProcessCondition.Status.Ready;
			}
		}
		return result;
	}

	// Token: 0x06004B19 RID: 19225 RVA: 0x001A6B2C File Offset: 0x001A4D2C
	public bool CanReachSpacecraftDestination(SpaceDestination destination)
	{
		Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		float rocketMaxDistance = this.module.GetComponent<CommandModule>().rocketStats.GetRocketMaxDistance();
		return (float)destination.OneBasedDistance * 10000f <= rocketMaxDistance;
	}

	// Token: 0x06004B1A RID: 19226 RVA: 0x001A6B70 File Offset: 0x001A4D70
	public SpaceDestination GetSpacecraftDestination()
	{
		Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
		return SpacecraftManager.instance.GetSpacecraftDestination(id);
	}

	// Token: 0x06004B1B RID: 19227 RVA: 0x001A6BB0 File Offset: 0x001A4DB0
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result = "";
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				result = UI.STARMAP.DESTINATIONSELECTION.REACHABLE;
			}
		}
		else if (status == ProcessCondition.Status.Ready && this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION.REACHABLE;
		}
		else if (this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION.UNREACHABLE;
		}
		else
		{
			result = UI.STARMAP.DESTINATIONSELECTION.NOTSELECTED;
		}
		return result;
	}

	// Token: 0x06004B1C RID: 19228 RVA: 0x001A6C1C File Offset: 0x001A4E1C
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result = "";
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				if (status == ProcessCondition.Status.Ready)
				{
					result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.REACHABLE;
				}
				else
				{
					result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.NOTSELECTED;
				}
			}
		}
		else if (status == ProcessCondition.Status.Ready && this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.REACHABLE;
		}
		else if (this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.UNREACHABLE;
		}
		else
		{
			result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.NOTSELECTED;
		}
		return result;
	}

	// Token: 0x06004B1D RID: 19229 RVA: 0x001A6C97 File Offset: 0x001A4E97
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003137 RID: 12599
	private LaunchableRocketRegisterType craftRegisterType;

	// Token: 0x04003138 RID: 12600
	private RocketModule module;
}
