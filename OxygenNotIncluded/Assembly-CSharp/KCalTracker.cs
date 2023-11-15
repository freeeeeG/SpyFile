using System;

// Token: 0x02000524 RID: 1316
public class KCalTracker : WorldTracker
{
	// Token: 0x06001F80 RID: 8064 RVA: 0x000A82C0 File Offset: 0x000A64C0
	public KCalTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F81 RID: 8065 RVA: 0x000A82C9 File Offset: 0x000A64C9
	public override void UpdateData()
	{
		base.AddPoint(RationTracker.Get().CountRations(null, ClusterManager.Instance.GetWorld(base.WorldID).worldInventory, true));
	}

	// Token: 0x06001F82 RID: 8066 RVA: 0x000A82F2 File Offset: 0x000A64F2
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedCalories(value, GameUtil.TimeSlice.None, true);
	}
}
