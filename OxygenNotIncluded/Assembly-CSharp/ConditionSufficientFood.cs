using System;
using STRINGS;

// Token: 0x020009DC RID: 2524
public class ConditionSufficientFood : ProcessCondition
{
	// Token: 0x06004B6C RID: 19308 RVA: 0x001A7F4F File Offset: 0x001A614F
	public ConditionSufficientFood(CommandModule module)
	{
		this.module = module;
	}

	// Token: 0x06004B6D RID: 19309 RVA: 0x001A7F5E File Offset: 0x001A615E
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.storage.GetAmountAvailable(GameTags.Edible) <= 1f)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B6E RID: 19310 RVA: 0x001A7F81 File Offset: 0x001A6181
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASFOOD.NAME;
		}
		return UI.STARMAP.NOFOOD.NAME;
	}

	// Token: 0x06004B6F RID: 19311 RVA: 0x001A7F9C File Offset: 0x001A619C
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASFOOD.TOOLTIP;
		}
		return UI.STARMAP.NOFOOD.TOOLTIP;
	}

	// Token: 0x06004B70 RID: 19312 RVA: 0x001A7FB7 File Offset: 0x001A61B7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400314E RID: 12622
	private CommandModule module;
}
