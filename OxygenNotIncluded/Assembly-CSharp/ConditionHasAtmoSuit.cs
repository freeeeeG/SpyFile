using System;
using STRINGS;

// Token: 0x020009D0 RID: 2512
public class ConditionHasAtmoSuit : ProcessCondition
{
	// Token: 0x06004B2F RID: 19247 RVA: 0x001A7184 File Offset: 0x001A5384
	public ConditionHasAtmoSuit(CommandModule module)
	{
		this.module = module;
		ManualDeliveryKG manualDeliveryKG = this.module.FindOrAdd<ManualDeliveryKG>();
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.SetStorage(module.storage);
		manualDeliveryKG.RequestedItemTag = GameTags.AtmoSuit;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.refillMass = 0.1f;
		manualDeliveryKG.capacity = 1f;
	}

	// Token: 0x06004B30 RID: 19248 RVA: 0x001A71FA File Offset: 0x001A53FA
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.storage.GetAmountAvailable(GameTags.AtmoSuit) < 1f)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B31 RID: 19249 RVA: 0x001A7220 File Offset: 0x001A5420
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASSUIT.NAME;
		}
		return UI.STARMAP.NOSUIT.NAME;
	}

	// Token: 0x06004B32 RID: 19250 RVA: 0x001A723B File Offset: 0x001A543B
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASSUIT.TOOLTIP;
		}
		return UI.STARMAP.NOSUIT.TOOLTIP;
	}

	// Token: 0x06004B33 RID: 19251 RVA: 0x001A7256 File Offset: 0x001A5456
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003140 RID: 12608
	private CommandModule module;
}
