using System;
using System.Collections.Generic;

// Token: 0x020008F9 RID: 2297
public class QuestCriteria_LessThan : QuestCriteria
{
	// Token: 0x060042AD RID: 17069 RVA: 0x00175643 File Offset: 0x00173843
	public QuestCriteria_LessThan(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x060042AE RID: 17070 RVA: 0x00175652 File Offset: 0x00173852
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current < target;
	}
}
