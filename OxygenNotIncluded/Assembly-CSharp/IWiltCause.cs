using System;

// Token: 0x02000740 RID: 1856
public interface IWiltCause
{
	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06003310 RID: 13072
	string WiltStateString { get; }

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x06003311 RID: 13073
	WiltCondition.Condition[] Conditions { get; }
}
