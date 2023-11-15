using System;
using Klei.AI;

// Token: 0x02000A93 RID: 2707
public interface IAmountDisplayer
{
	// Token: 0x060052D7 RID: 21207
	string GetValueString(Amount master, AmountInstance instance);

	// Token: 0x060052D8 RID: 21208
	string GetDescription(Amount master, AmountInstance instance);

	// Token: 0x060052D9 RID: 21209
	string GetTooltip(Amount master, AmountInstance instance);

	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x060052DA RID: 21210
	IAttributeFormatter Formatter { get; }
}
