using System;
using STRINGS;

// Token: 0x020009DE RID: 2526
public class InternalConstructionCompleteCondition : ProcessCondition
{
	// Token: 0x06004B76 RID: 19318 RVA: 0x001A80EF File Offset: 0x001A62EF
	public InternalConstructionCompleteCondition(BuildingInternalConstructor.Instance target)
	{
		this.target = target;
	}

	// Token: 0x06004B77 RID: 19319 RVA: 0x001A80FE File Offset: 0x001A62FE
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.target.IsRequestingConstruction() && !this.target.HasOutputInStorage())
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B78 RID: 19320 RVA: 0x001A811D File Offset: 0x001A631D
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.STATUS.FAILURE;
	}

	// Token: 0x06004B79 RID: 19321 RVA: 0x001A8134 File Offset: 0x001A6334
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.TOOLTIP.FAILURE;
	}

	// Token: 0x06004B7A RID: 19322 RVA: 0x001A814B File Offset: 0x001A634B
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003150 RID: 12624
	private BuildingInternalConstructor.Instance target;
}
