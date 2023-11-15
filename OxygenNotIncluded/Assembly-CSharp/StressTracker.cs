using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000523 RID: 1315
public class StressTracker : WorldTracker
{
	// Token: 0x06001F7D RID: 8061 RVA: 0x000A8222 File Offset: 0x000A6422
	public StressTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06001F7E RID: 8062 RVA: 0x000A822C File Offset: 0x000A642C
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			if (Components.LiveMinionIdentities[i].GetMyWorldId() == base.WorldID)
			{
				num = Mathf.Max(num, Components.LiveMinionIdentities[i].gameObject.GetAmounts().GetValue(Db.Get().Amounts.Stress.Id));
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x06001F7F RID: 8063 RVA: 0x000A82AD File Offset: 0x000A64AD
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
