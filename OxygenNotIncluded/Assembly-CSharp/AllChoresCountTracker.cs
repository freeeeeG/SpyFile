using System;

// Token: 0x02000519 RID: 1305
public class AllChoresCountTracker : WorldTracker
{
	// Token: 0x06001F5D RID: 8029 RVA: 0x000A7AB6 File Offset: 0x000A5CB6
	public AllChoresCountTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F5E RID: 8030 RVA: 0x000A7AC0 File Offset: 0x000A5CC0
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			Tracker choreGroupTracker = TrackerTool.Instance.GetChoreGroupTracker(base.WorldID, Db.Get().ChoreGroups[i]);
			num += ((choreGroupTracker == null) ? 0f : choreGroupTracker.GetCurrentValue());
		}
		base.AddPoint(num);
	}

	// Token: 0x06001F5F RID: 8031 RVA: 0x000A7B28 File Offset: 0x000A5D28
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
