using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x020009D4 RID: 2516
public class ConditionHasNosecone : ProcessCondition
{
	// Token: 0x06004B44 RID: 19268 RVA: 0x001A77E3 File Offset: 0x001A59E3
	public ConditionHasNosecone(LaunchableRocketCluster launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x06004B45 RID: 19269 RVA: 0x001A77F4 File Offset: 0x001A59F4
	public override ProcessCondition.Status EvaluateCondition()
	{
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.launchable.parts.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().HasTag(GameTags.NoseRocketModule))
				{
					return ProcessCondition.Status.Ready;
				}
			}
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06004B46 RID: 19270 RVA: 0x001A7858 File Offset: 0x001A5A58
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B47 RID: 19271 RVA: 0x001A7898 File Offset: 0x001A5A98
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B48 RID: 19272 RVA: 0x001A78D8 File Offset: 0x001A5AD8
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003144 RID: 12612
	private LaunchableRocketCluster launchable;
}
