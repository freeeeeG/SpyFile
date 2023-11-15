using System;
using System.Collections.Generic;

// Token: 0x02000C0C RID: 3084
public interface IProcessConditionSet
{
	// Token: 0x060061B6 RID: 25014
	List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType);
}
