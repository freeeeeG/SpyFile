using System;

// Token: 0x020000EC RID: 236
public interface IEventBase
{
	// Token: 0x17000069 RID: 105
	// (get) Token: 0x060005E8 RID: 1512
	int Count { get; }

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x060005E9 RID: 1513
	bool IsEmpty { get; }

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x060005EA RID: 1514
	uint SendEventCount { get; }

	// Token: 0x060005EB RID: 1515
	void Clear();
}
