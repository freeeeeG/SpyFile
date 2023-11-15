using System;
using System.Collections.Generic;

// Token: 0x020008FB RID: 2299
public class QuestCriteria_LessOrEqual : QuestCriteria
{
	// Token: 0x060042B1 RID: 17073 RVA: 0x00175670 File Offset: 0x00173870
	public QuestCriteria_LessOrEqual(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x060042B2 RID: 17074 RVA: 0x0017567F File Offset: 0x0017387F
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current <= target;
	}
}
