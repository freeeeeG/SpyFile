using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x02000526 RID: 1318
public class RadiationTracker : WorldTracker
{
	// Token: 0x06001F86 RID: 8070 RVA: 0x000A8362 File Offset: 0x000A6562
	public RadiationTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x000A836C File Offset: 0x000A656C
	public override void UpdateData()
	{
		float num = 0f;
		List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(base.WorldID, false);
		if (worldItems.Count == 0)
		{
			base.AddPoint(0f);
			return;
		}
		foreach (MinionIdentity cmp in worldItems)
		{
			num += cmp.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).value;
		}
		float value = num / (float)worldItems.Count;
		base.AddPoint(value);
	}

	// Token: 0x06001F88 RID: 8072 RVA: 0x000A841C File Offset: 0x000A661C
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedRads(value, GameUtil.TimeSlice.None);
	}
}
