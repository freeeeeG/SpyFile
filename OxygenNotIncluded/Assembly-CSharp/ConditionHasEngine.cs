using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009D2 RID: 2514
public class ConditionHasEngine : ProcessCondition
{
	// Token: 0x06004B39 RID: 19257 RVA: 0x001A72D6 File Offset: 0x001A54D6
	public ConditionHasEngine(ILaunchableRocket launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x06004B3A RID: 19258 RVA: 0x001A72E8 File Offset: 0x001A54E8
	public override ProcessCondition.Status EvaluateCondition()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchable.LaunchableGameObject.GetComponent<AttachableBuilding>()))
		{
			if (gameObject.GetComponent<RocketEngine>() != null || gameObject.GetComponent<RocketEngineCluster>())
			{
				return ProcessCondition.Status.Ready;
			}
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06004B3B RID: 19259 RVA: 0x001A7368 File Offset: 0x001A5568
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B3C RID: 19260 RVA: 0x001A73A8 File Offset: 0x001A55A8
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B3D RID: 19261 RVA: 0x001A73E8 File Offset: 0x001A55E8
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003142 RID: 12610
	private ILaunchableRocket launchable;
}
