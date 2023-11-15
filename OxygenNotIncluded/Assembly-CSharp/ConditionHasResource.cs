using System;
using STRINGS;

// Token: 0x020009D5 RID: 2517
public class ConditionHasResource : ProcessCondition
{
	// Token: 0x06004B49 RID: 19273 RVA: 0x001A78DB File Offset: 0x001A5ADB
	public ConditionHasResource(Storage storage, SimHashes resource, float thresholdMass)
	{
		this.storage = storage;
		this.resource = resource;
		this.thresholdMass = thresholdMass;
	}

	// Token: 0x06004B4A RID: 19274 RVA: 0x001A78F8 File Offset: 0x001A5AF8
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.storage.GetAmountAvailable(this.resource.CreateTag()) < this.thresholdMass)
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B4B RID: 19275 RVA: 0x001A791C File Offset: 0x001A5B1C
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.READY, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
			else
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.WARNING, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
		}
		else
		{
			result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.FAILURE, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
		}
		return result;
	}

	// Token: 0x06004B4C RID: 19276 RVA: 0x001A79CC File Offset: 0x001A5BCC
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.READY, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
			else
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.WARNING, this.storage.GetProperName(), GameUtil.GetFormattedMass(this.thresholdMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
		}
		else
		{
			result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.FAILURE, this.storage.GetProperName(), GameUtil.GetFormattedMass(this.thresholdMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), ElementLoader.GetElement(this.resource.CreateTag()).name);
		}
		return result;
	}

	// Token: 0x06004B4D RID: 19277 RVA: 0x001A7AA4 File Offset: 0x001A5CA4
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003145 RID: 12613
	private Storage storage;

	// Token: 0x04003146 RID: 12614
	private SimHashes resource;

	// Token: 0x04003147 RID: 12615
	private float thresholdMass;
}
