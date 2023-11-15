using System;
using System.Collections.Generic;

// Token: 0x020009A4 RID: 2468
public class LaunchPadConditions : KMonoBehaviour, IProcessConditionSet
{
	// Token: 0x06004981 RID: 18817 RVA: 0x0019E63C File Offset: 0x0019C83C
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		if (conditionType != ProcessCondition.ProcessConditionType.RocketStorage)
		{
			return null;
		}
		return this.conditions;
	}

	// Token: 0x06004982 RID: 18818 RVA: 0x0019E64A File Offset: 0x0019C84A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.conditions = new List<ProcessCondition>();
		this.conditions.Add(new TransferCargoCompleteCondition(base.gameObject));
	}

	// Token: 0x04003052 RID: 12370
	private List<ProcessCondition> conditions;
}
