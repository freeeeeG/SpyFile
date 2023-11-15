using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008F7 RID: 2295
public class QuestCriteria_Equals : QuestCriteria
{
	// Token: 0x060042A9 RID: 17065 RVA: 0x0017560B File Offset: 0x0017380B
	public QuestCriteria_Equals(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x060042AA RID: 17066 RVA: 0x0017561A File Offset: 0x0017381A
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return Mathf.Abs(target - current) <= Mathf.Epsilon;
	}
}
