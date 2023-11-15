using System;
using STRINGS;

// Token: 0x020009D9 RID: 2521
public class ConditionPilotOnBoard : ProcessCondition
{
	// Token: 0x06004B5D RID: 19293 RVA: 0x001A7CCB File Offset: 0x001A5ECB
	public ConditionPilotOnBoard(PassengerRocketModule module)
	{
		this.module = module;
	}

	// Token: 0x06004B5E RID: 19294 RVA: 0x001A7CDA File Offset: 0x001A5EDA
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (!this.module.CheckPilotBoarded())
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B5F RID: 19295 RVA: 0x001A7CEC File Offset: 0x001A5EEC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.FAILURE;
	}

	// Token: 0x06004B60 RID: 19296 RVA: 0x001A7D07 File Offset: 0x001A5F07
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.FAILURE;
	}

	// Token: 0x06004B61 RID: 19297 RVA: 0x001A7D22 File Offset: 0x001A5F22
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400314B RID: 12619
	private PassengerRocketModule module;
}
