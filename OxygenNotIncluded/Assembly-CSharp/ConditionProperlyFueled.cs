using System;
using STRINGS;

// Token: 0x020009DA RID: 2522
public class ConditionProperlyFueled : ProcessCondition
{
	// Token: 0x06004B62 RID: 19298 RVA: 0x001A7D25 File Offset: 0x001A5F25
	public ConditionProperlyFueled(IFuelTank fuelTank)
	{
		this.fuelTank = fuelTank;
	}

	// Token: 0x06004B63 RID: 19299 RVA: 0x001A7D34 File Offset: 0x001A5F34
	public override ProcessCondition.Status EvaluateCondition()
	{
		RocketModuleCluster component = ((KMonoBehaviour)this.fuelTank).GetComponent<RocketModuleCluster>();
		if (component != null && component.CraftInterface != null)
		{
			Clustercraft component2 = component.CraftInterface.GetComponent<Clustercraft>();
			ClusterTraveler component3 = component.CraftInterface.GetComponent<ClusterTraveler>();
			if (component2 == null || component3 == null || component3.CurrentPath == null)
			{
				return ProcessCondition.Status.Failure;
			}
			int num = component3.RemainingTravelNodes();
			if (num == 0)
			{
				if (!component2.HasResourcesToMove(1, Clustercraft.CombustionResource.Fuel))
				{
					return ProcessCondition.Status.Failure;
				}
				return ProcessCondition.Status.Ready;
			}
			else
			{
				bool flag = component2.HasResourcesToMove(num * 2, Clustercraft.CombustionResource.Fuel);
				bool flag2 = component2.HasResourcesToMove(num, Clustercraft.CombustionResource.Fuel);
				if (flag)
				{
					return ProcessCondition.Status.Ready;
				}
				if (flag2)
				{
					return ProcessCondition.Status.Warning;
				}
			}
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06004B64 RID: 19300 RVA: 0x001A7DD8 File Offset: 0x001A5FD8
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B65 RID: 19301 RVA: 0x001A7E18 File Offset: 0x001A6018
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		Clustercraft component = ((KMonoBehaviour)this.fuelTank).GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				if (component.Destination == component.Location)
				{
					result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.READY_NO_DESTINATION;
				}
				else
				{
					result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.READY;
				}
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B66 RID: 19302 RVA: 0x001A7E93 File Offset: 0x001A6093
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400314C RID: 12620
	private IFuelTank fuelTank;
}
