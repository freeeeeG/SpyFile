using System;
using System.Collections.Generic;

// Token: 0x020008F8 RID: 2296
public class QuestCriteria_GreaterThan : QuestCriteria
{
	// Token: 0x060042AB RID: 17067 RVA: 0x0017562E File Offset: 0x0017382E
	public QuestCriteria_GreaterThan(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x060042AC RID: 17068 RVA: 0x0017563D File Offset: 0x0017383D
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current > target;
	}
}
