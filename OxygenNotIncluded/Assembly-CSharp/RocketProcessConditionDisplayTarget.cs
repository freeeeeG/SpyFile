using System;
using System.Collections.Generic;

// Token: 0x020009E1 RID: 2529
public class RocketProcessConditionDisplayTarget : KMonoBehaviour, IProcessConditionSet, ISim1000ms
{
	// Token: 0x06004B87 RID: 19335 RVA: 0x001A82D2 File Offset: 0x001A64D2
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		if (this.craftModuleInterface == null)
		{
			this.craftModuleInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		}
		return this.craftModuleInterface.GetConditionSet(conditionType);
	}

	// Token: 0x06004B88 RID: 19336 RVA: 0x001A8300 File Offset: 0x001A6500
	public void Sim1000ms(float dt)
	{
		bool flag = false;
		using (List<ProcessCondition>.Enumerator enumerator = this.GetConditionSet(ProcessCondition.ProcessConditionType.All).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
				{
					flag = true;
					if (this.statusHandle == Guid.Empty)
					{
						this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.RocketChecklistIncomplete, null);
						break;
					}
					break;
				}
			}
		}
		if (!flag && this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
		}
	}

	// Token: 0x04003156 RID: 12630
	private CraftModuleInterface craftModuleInterface;

	// Token: 0x04003157 RID: 12631
	private Guid statusHandle = Guid.Empty;
}
