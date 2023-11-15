using System;
using System.Collections.Generic;

// Token: 0x02000529 RID: 1321
public class WorkingToiletTracker : WorldTracker
{
	// Token: 0x06001F8F RID: 8079 RVA: 0x000A84E4 File Offset: 0x000A66E4
	public WorkingToiletTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F90 RID: 8080 RVA: 0x000A84F0 File Offset: 0x000A66F0
	public override void UpdateData()
	{
		int num = 0;
		List<IUsable> worldItems = Components.Toilets.GetWorldItems(base.WorldID, false);
		for (int i = 0; i < worldItems.Count; i++)
		{
			if (worldItems[i].IsUsable())
			{
				num++;
			}
		}
		base.AddPoint((float)num);
	}

	// Token: 0x06001F91 RID: 8081 RVA: 0x000A853C File Offset: 0x000A673C
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
