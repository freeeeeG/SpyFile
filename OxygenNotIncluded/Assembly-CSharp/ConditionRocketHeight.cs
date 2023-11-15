using System;
using STRINGS;

// Token: 0x020009DB RID: 2523
public class ConditionRocketHeight : ProcessCondition
{
	// Token: 0x06004B67 RID: 19303 RVA: 0x001A7E96 File Offset: 0x001A6096
	public ConditionRocketHeight(RocketEngineCluster engine)
	{
		this.engine = engine;
	}

	// Token: 0x06004B68 RID: 19304 RVA: 0x001A7EA5 File Offset: 0x001A60A5
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.engine.maxHeight < this.engine.GetComponent<RocketModuleCluster>().CraftInterface.RocketHeight)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B69 RID: 19305 RVA: 0x001A7ECC File Offset: 0x001A60CC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B6A RID: 19306 RVA: 0x001A7F0C File Offset: 0x001A610C
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B6B RID: 19307 RVA: 0x001A7F4C File Offset: 0x001A614C
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400314D RID: 12621
	private RocketEngineCluster engine;
}
