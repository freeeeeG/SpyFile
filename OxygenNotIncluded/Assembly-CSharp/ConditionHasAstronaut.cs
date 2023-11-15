using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x020009CF RID: 2511
public class ConditionHasAstronaut : ProcessCondition
{
	// Token: 0x06004B2A RID: 19242 RVA: 0x001A70FF File Offset: 0x001A52FF
	public ConditionHasAstronaut(CommandModule module)
	{
		this.module = module;
	}

	// Token: 0x06004B2B RID: 19243 RVA: 0x001A7110 File Offset: 0x001A5310
	public override ProcessCondition.Status EvaluateCondition()
	{
		List<MinionStorage.Info> storedMinionInfo = this.module.GetComponent<MinionStorage>().GetStoredMinionInfo();
		if (storedMinionInfo.Count > 0 && storedMinionInfo[0].serializedMinion != null)
		{
			return ProcessCondition.Status.Ready;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06004B2C RID: 19244 RVA: 0x001A7148 File Offset: 0x001A5348
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUT_TITLE;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
	}

	// Token: 0x06004B2D RID: 19245 RVA: 0x001A7163 File Offset: 0x001A5363
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HASASTRONAUT;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
	}

	// Token: 0x06004B2E RID: 19246 RVA: 0x001A717E File Offset: 0x001A537E
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400313F RID: 12607
	private CommandModule module;
}
