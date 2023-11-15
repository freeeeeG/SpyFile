using System;
using System.Collections.Generic;

// Token: 0x020008FA RID: 2298
public class QuestCriteria_GreaterOrEqual : QuestCriteria
{
	// Token: 0x060042AF RID: 17071 RVA: 0x00175658 File Offset: 0x00173858
	public QuestCriteria_GreaterOrEqual(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x060042B0 RID: 17072 RVA: 0x00175667 File Offset: 0x00173867
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current >= target;
	}
}
