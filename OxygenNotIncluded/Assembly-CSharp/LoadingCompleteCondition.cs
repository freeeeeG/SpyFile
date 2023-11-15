using System;
using STRINGS;

// Token: 0x020009DF RID: 2527
public class LoadingCompleteCondition : ProcessCondition
{
	// Token: 0x06004B7B RID: 19323 RVA: 0x001A814E File Offset: 0x001A634E
	public LoadingCompleteCondition(Storage target)
	{
		this.target = target;
		this.userControlledTarget = target.GetComponent<IUserControlledCapacity>();
	}

	// Token: 0x06004B7C RID: 19324 RVA: 0x001A8169 File Offset: 0x001A6369
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.userControlledTarget != null)
		{
			if (this.userControlledTarget.AmountStored < this.userControlledTarget.UserMaxCapacity)
			{
				return ProcessCondition.Status.Warning;
			}
			return ProcessCondition.Status.Ready;
		}
		else
		{
			if (!this.target.IsFull())
			{
				return ProcessCondition.Status.Warning;
			}
			return ProcessCondition.Status.Ready;
		}
	}

	// Token: 0x06004B7D RID: 19325 RVA: 0x001A819F File Offset: 0x001A639F
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.STATUS.WARNING;
	}

	// Token: 0x06004B7E RID: 19326 RVA: 0x001A81B6 File Offset: 0x001A63B6
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.TOOLTIP.WARNING;
	}

	// Token: 0x06004B7F RID: 19327 RVA: 0x001A81CD File Offset: 0x001A63CD
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003151 RID: 12625
	private Storage target;

	// Token: 0x04003152 RID: 12626
	private IUserControlledCapacity userControlledTarget;
}
