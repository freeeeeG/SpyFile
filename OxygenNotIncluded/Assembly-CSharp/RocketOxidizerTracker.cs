using System;

// Token: 0x02000528 RID: 1320
public class RocketOxidizerTracker : WorldTracker
{
	// Token: 0x06001F8C RID: 8076 RVA: 0x000A8484 File Offset: 0x000A6684
	public RocketOxidizerTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F8D RID: 8077 RVA: 0x000A8490 File Offset: 0x000A6690
	public override void UpdateData()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.WorldID).GetComponent<Clustercraft>();
		base.AddPoint((component != null) ? component.ModuleInterface.OxidizerPowerRemaining : 0f);
	}

	// Token: 0x06001F8E RID: 8078 RVA: 0x000A84D4 File Offset: 0x000A66D4
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
