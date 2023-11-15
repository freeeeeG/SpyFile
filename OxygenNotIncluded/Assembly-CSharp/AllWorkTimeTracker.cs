using System;

// Token: 0x0200051B RID: 1307
public class AllWorkTimeTracker : WorldTracker
{
	// Token: 0x06001F63 RID: 8035 RVA: 0x000A7C8A File Offset: 0x000A5E8A
	public AllWorkTimeTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F64 RID: 8036 RVA: 0x000A7C94 File Offset: 0x000A5E94
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			num += TrackerTool.Instance.GetWorkTimeTracker(base.WorldID, Db.Get().ChoreGroups[i]).GetCurrentValue();
		}
		base.AddPoint(num);
	}

	// Token: 0x06001F65 RID: 8037 RVA: 0x000A7CF0 File Offset: 0x000A5EF0
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedPercent(value, GameUtil.TimeSlice.None).ToString();
	}
}
