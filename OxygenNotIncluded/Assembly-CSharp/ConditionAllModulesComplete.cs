using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009CC RID: 2508
public class ConditionAllModulesComplete : ProcessCondition
{
	// Token: 0x06004B12 RID: 19218 RVA: 0x001A6983 File Offset: 0x001A4B83
	public ConditionAllModulesComplete(ILaunchableRocket launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x06004B13 RID: 19219 RVA: 0x001A6994 File Offset: 0x001A4B94
	public override ProcessCondition.Status EvaluateCondition()
	{
		using (List<GameObject>.Enumerator enumerator = AttachableBuilding.GetAttachedNetwork(this.launchable.LaunchableGameObject.GetComponent<AttachableBuilding>()).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetComponent<Constructable>() != null)
				{
					return ProcessCondition.Status.Failure;
				}
			}
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B14 RID: 19220 RVA: 0x001A6A04 File Offset: 0x001A4C04
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B15 RID: 19221 RVA: 0x001A6A44 File Offset: 0x001A4C44
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06004B16 RID: 19222 RVA: 0x001A6A84 File Offset: 0x001A4C84
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003136 RID: 12598
	private ILaunchableRocket launchable;
}
