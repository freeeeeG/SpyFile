using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200051C RID: 1308
public class WorkTimeTracker : WorldTracker
{
	// Token: 0x06001F66 RID: 8038 RVA: 0x000A7CFE File Offset: 0x000A5EFE
	public WorkTimeTracker(int worldID, ChoreGroup group) : base(worldID)
	{
		this.choreGroup = group;
	}

	// Token: 0x06001F67 RID: 8039 RVA: 0x000A7D10 File Offset: 0x000A5F10
	public override void UpdateData()
	{
		float num = 0f;
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false);
		Chore chore;
		Predicate<ChoreType> <>9__0;
		foreach (MinionIdentity minionIdentity in worldItems)
		{
			chore = minionIdentity.GetComponent<ChoreConsumer>().choreDriver.GetCurrentChore();
			if (chore != null)
			{
				List<ChoreType> choreTypes = this.choreGroup.choreTypes;
				Predicate<ChoreType> match2;
				if ((match2 = <>9__0) == null)
				{
					match2 = (<>9__0 = ((ChoreType match) => match == chore.choreType));
				}
				if (choreTypes.Find(match2) != null)
				{
					num += 1f;
				}
			}
		}
		base.AddPoint(num / (float)worldItems.Count * 100f);
	}

	// Token: 0x06001F68 RID: 8040 RVA: 0x000A7DE8 File Offset: 0x000A5FE8
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedPercent(Mathf.Round(value), GameUtil.TimeSlice.None).ToString();
	}

	// Token: 0x040011A1 RID: 4513
	public ChoreGroup choreGroup;
}
