using System;
using STRINGS;

// Token: 0x020009D1 RID: 2513
public class ConditionHasControlStation : ProcessCondition
{
	// Token: 0x06004B34 RID: 19252 RVA: 0x001A7259 File Offset: 0x001A5459
	public ConditionHasControlStation(RocketModuleCluster module)
	{
		this.module = module;
	}

	// Token: 0x06004B35 RID: 19253 RVA: 0x001A7268 File Offset: 0x001A5468
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (Components.RocketControlStations.GetWorldItems(this.module.CraftInterface.GetComponent<WorldContainer>().id, false).Count <= 0)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B36 RID: 19254 RVA: 0x001A7295 File Offset: 0x001A5495
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.FAILURE;
	}

	// Token: 0x06004B37 RID: 19255 RVA: 0x001A72B0 File Offset: 0x001A54B0
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.FAILURE;
	}

	// Token: 0x06004B38 RID: 19256 RVA: 0x001A72CB File Offset: 0x001A54CB
	public override bool ShowInUI()
	{
		return this.EvaluateCondition() == ProcessCondition.Status.Failure;
	}

	// Token: 0x04003141 RID: 12609
	private RocketModuleCluster module;
}
