using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009E2 RID: 2530
public class TransferCargoCompleteCondition : ProcessCondition
{
	// Token: 0x06004B8A RID: 19338 RVA: 0x001A83C7 File Offset: 0x001A65C7
	public TransferCargoCompleteCondition(GameObject target)
	{
		this.target = target;
	}

	// Token: 0x06004B8B RID: 19339 RVA: 0x001A83D8 File Offset: 0x001A65D8
	public override ProcessCondition.Status EvaluateCondition()
	{
		LaunchPad component = this.target.GetComponent<LaunchPad>();
		CraftModuleInterface craftModuleInterface;
		if (component == null)
		{
			craftModuleInterface = this.target.GetComponent<Clustercraft>().ModuleInterface;
		}
		else
		{
			RocketModuleCluster landedRocket = component.LandedRocket;
			if (landedRocket == null)
			{
				return ProcessCondition.Status.Ready;
			}
			craftModuleInterface = landedRocket.CraftInterface;
		}
		if (!craftModuleInterface.HasCargoModule)
		{
			return ProcessCondition.Status.Ready;
		}
		if (!this.target.HasTag(GameTags.TransferringCargoComplete))
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B8C RID: 19340 RVA: 0x001A8447 File Offset: 0x001A6647
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.STATUS.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.STATUS.WARNING;
	}

	// Token: 0x06004B8D RID: 19341 RVA: 0x001A8462 File Offset: 0x001A6662
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.TOOLTIP.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.TOOLTIP.WARNING;
	}

	// Token: 0x06004B8E RID: 19342 RVA: 0x001A847D File Offset: 0x001A667D
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003158 RID: 12632
	private GameObject target;
}
