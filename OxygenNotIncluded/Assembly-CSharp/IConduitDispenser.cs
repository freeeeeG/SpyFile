using System;

// Token: 0x020006F3 RID: 1779
public interface IConduitDispenser
{
	// Token: 0x17000369 RID: 873
	// (get) Token: 0x060030CA RID: 12490
	Storage Storage { get; }

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x060030CB RID: 12491
	ConduitType ConduitType { get; }
}
