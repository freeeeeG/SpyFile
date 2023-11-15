using System;
using STRINGS;

// Token: 0x020009D7 RID: 2519
public class ConditionOnLaunchPad : ProcessCondition
{
	// Token: 0x06004B53 RID: 19283 RVA: 0x001A7B01 File Offset: 0x001A5D01
	public ConditionOnLaunchPad(CraftModuleInterface craftInterface)
	{
		this.craftInterface = craftInterface;
	}

	// Token: 0x06004B54 RID: 19284 RVA: 0x001A7B10 File Offset: 0x001A5D10
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (!(this.craftInterface.CurrentPad != null))
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B55 RID: 19285 RVA: 0x001A7B28 File Offset: 0x001A5D28
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B56 RID: 19286 RVA: 0x001A7B68 File Offset: 0x001A5D68
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B57 RID: 19287 RVA: 0x001A7BA8 File Offset: 0x001A5DA8
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003149 RID: 12617
	private CraftModuleInterface craftInterface;
}
