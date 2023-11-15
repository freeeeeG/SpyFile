using System;

// Token: 0x0200051F RID: 1311
public class ToiletTracker : WorldTracker
{
	// Token: 0x06001F6F RID: 8047 RVA: 0x000A7F8B File Offset: 0x000A618B
	public ToiletTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F70 RID: 8048 RVA: 0x000A7F94 File Offset: 0x000A6194
	public override void UpdateData()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x000A7F9B File Offset: 0x000A619B
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
