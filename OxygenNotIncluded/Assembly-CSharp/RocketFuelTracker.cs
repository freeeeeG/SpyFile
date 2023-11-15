using System;

// Token: 0x02000527 RID: 1319
public class RocketFuelTracker : WorldTracker
{
	// Token: 0x06001F89 RID: 8073 RVA: 0x000A8425 File Offset: 0x000A6625
	public RocketFuelTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F8A RID: 8074 RVA: 0x000A8430 File Offset: 0x000A6630
	public override void UpdateData()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.WorldID).GetComponent<Clustercraft>();
		base.AddPoint((component != null) ? component.ModuleInterface.FuelRemaining : 0f);
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x000A8474 File Offset: 0x000A6674
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
