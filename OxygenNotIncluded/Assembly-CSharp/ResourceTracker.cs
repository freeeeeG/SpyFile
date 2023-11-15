using System;

// Token: 0x02000520 RID: 1312
public class ResourceTracker : WorldTracker
{
	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06001F72 RID: 8050 RVA: 0x000A7FA4 File Offset: 0x000A61A4
	// (set) Token: 0x06001F73 RID: 8051 RVA: 0x000A7FAC File Offset: 0x000A61AC
	public Tag tag { get; private set; }

	// Token: 0x06001F74 RID: 8052 RVA: 0x000A7FB5 File Offset: 0x000A61B5
	public ResourceTracker(int worldID, Tag materialCategoryTag) : base(worldID)
	{
		this.tag = materialCategoryTag;
	}

	// Token: 0x06001F75 RID: 8053 RVA: 0x000A7FC8 File Offset: 0x000A61C8
	public override void UpdateData()
	{
		if (ClusterManager.Instance.GetWorld(base.WorldID).worldInventory == null)
		{
			return;
		}
		base.AddPoint(ClusterManager.Instance.GetWorld(base.WorldID).worldInventory.GetAmount(this.tag, false));
	}

	// Token: 0x06001F76 RID: 8054 RVA: 0x000A801A File Offset: 0x000A621A
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
