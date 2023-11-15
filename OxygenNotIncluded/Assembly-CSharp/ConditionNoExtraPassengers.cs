using System;
using STRINGS;

// Token: 0x020009D6 RID: 2518
public class ConditionNoExtraPassengers : ProcessCondition
{
	// Token: 0x06004B4E RID: 19278 RVA: 0x001A7AA7 File Offset: 0x001A5CA7
	public ConditionNoExtraPassengers(PassengerRocketModule module)
	{
		this.module = module;
	}

	// Token: 0x06004B4F RID: 19279 RVA: 0x001A7AB6 File Offset: 0x001A5CB6
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (!this.module.CheckExtraPassengers())
		{
			return ProcessCondition.Status.Ready;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06004B50 RID: 19280 RVA: 0x001A7AC8 File Offset: 0x001A5CC8
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.FAILURE;
	}

	// Token: 0x06004B51 RID: 19281 RVA: 0x001A7AE3 File Offset: 0x001A5CE3
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.TOOLTIP.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.TOOLTIP.FAILURE;
	}

	// Token: 0x06004B52 RID: 19282 RVA: 0x001A7AFE File Offset: 0x001A5CFE
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003148 RID: 12616
	private PassengerRocketModule module;
}
