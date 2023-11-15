using System;

// Token: 0x0200051D RID: 1309
public class CropTracker : WorldTracker
{
	// Token: 0x06001F69 RID: 8041 RVA: 0x000A7DFB File Offset: 0x000A5FFB
	public CropTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F6A RID: 8042 RVA: 0x000A7E04 File Offset: 0x000A6004
	public override void UpdateData()
	{
		float num = 0f;
		foreach (PlantablePlot plantablePlot in Components.PlantablePlots.GetItems(base.WorldID))
		{
			if (!(plantablePlot.plant == null) && plantablePlot.HasDepositTag(GameTags.CropSeed) && !plantablePlot.plant.HasTag(GameTags.Wilting))
			{
				num += 1f;
			}
		}
		base.AddPoint(num);
	}

	// Token: 0x06001F6B RID: 8043 RVA: 0x000A7E9C File Offset: 0x000A609C
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
