using System;
using System.Collections.Generic;

// Token: 0x02000525 RID: 1317
public class IdleTracker : WorldTracker
{
	// Token: 0x06001F83 RID: 8067 RVA: 0x000A82FC File Offset: 0x000A64FC
	public IdleTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F84 RID: 8068 RVA: 0x000A8308 File Offset: 0x000A6508
	public override void UpdateData()
	{
		int num = 0;
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false);
		for (int i = 0; i < worldItems.Count; i++)
		{
			if (worldItems[i].HasTag(GameTags.Idle))
			{
				num++;
			}
		}
		base.AddPoint((float)num);
	}

	// Token: 0x06001F85 RID: 8069 RVA: 0x000A8359 File Offset: 0x000A6559
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
