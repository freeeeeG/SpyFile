using System;
using KSerialization;

// Token: 0x020004B7 RID: 1207
[SerializationConfig(MemberSerialization.OptIn)]
public class GasSource : SubstanceSource
{
	// Token: 0x06001B68 RID: 7016 RVA: 0x00093362 File Offset: 0x00091562
	protected override CellOffset[] GetOffsetGroup()
	{
		return OffsetGroups.LiquidSource;
	}

	// Token: 0x06001B69 RID: 7017 RVA: 0x00093369 File Offset: 0x00091569
	protected override IChunkManager GetChunkManager()
	{
		return GasSourceManager.Instance;
	}

	// Token: 0x06001B6A RID: 7018 RVA: 0x00093370 File Offset: 0x00091570
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}
}
