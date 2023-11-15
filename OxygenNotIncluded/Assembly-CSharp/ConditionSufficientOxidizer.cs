using System;
using STRINGS;

// Token: 0x020009DD RID: 2525
public class ConditionSufficientOxidizer : ProcessCondition
{
	// Token: 0x06004B71 RID: 19313 RVA: 0x001A7FBA File Offset: 0x001A61BA
	public ConditionSufficientOxidizer(OxidizerTank oxidizerTank)
	{
		this.oxidizerTank = oxidizerTank;
	}

	// Token: 0x06004B72 RID: 19314 RVA: 0x001A7FCC File Offset: 0x001A61CC
	public override ProcessCondition.Status EvaluateCondition()
	{
		RocketModuleCluster component = this.oxidizerTank.GetComponent<RocketModuleCluster>();
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
				if (!component2.HasResourcesToMove(1, Clustercraft.CombustionResource.Oxidizer))
				{
					return ProcessCondition.Status.Failure;
				}
				return ProcessCondition.Status.Ready;
			}
			else
			{
				bool flag = component2.HasResourcesToMove(num * 2, Clustercraft.CombustionResource.Oxidizer);
				bool flag2 = component2.HasResourcesToMove(num, Clustercraft.CombustionResource.Oxidizer);
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

	// Token: 0x06004B73 RID: 19315 RVA: 0x001A806C File Offset: 0x001A626C
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B74 RID: 19316 RVA: 0x001A80AC File Offset: 0x001A62AC
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B75 RID: 19317 RVA: 0x001A80EC File Offset: 0x001A62EC
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400314F RID: 12623
	private OxidizerTank oxidizerTank;
}
