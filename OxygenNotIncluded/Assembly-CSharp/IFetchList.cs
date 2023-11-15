using System;
using System.Collections.Generic;

// Token: 0x020007BF RID: 1983
public interface IFetchList
{
	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x060036EB RID: 14059
	Storage Destination { get; }

	// Token: 0x060036EC RID: 14060
	float GetMinimumAmount(Tag tag);

	// Token: 0x060036ED RID: 14061
	Dictionary<Tag, float> GetRemaining();

	// Token: 0x060036EE RID: 14062
	Dictionary<Tag, float> GetRemainingMinimum();
}
