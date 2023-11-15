using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000D57 RID: 3415
	public class DupesVsSolidTransferArmFetch : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AFC RID: 27388 RVA: 0x0029AABE File Offset: 0x00298CBE
		public DupesVsSolidTransferArmFetch(float percentage, int numCycles)
		{
			this.percentage = percentage;
			this.numCycles = numCycles;
		}

		// Token: 0x06006AFD RID: 27389 RVA: 0x0029AAD4 File Offset: 0x00298CD4
		public override bool Success()
		{
			Dictionary<int, int> fetchDupeChoreDeliveries = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().fetchDupeChoreDeliveries;
			Dictionary<int, int> fetchAutomatedChoreDeliveries = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().fetchAutomatedChoreDeliveries;
			int num = 0;
			this.currentCycleCount = 0;
			for (int i = GameClock.Instance.GetCycle() - 1; i >= GameClock.Instance.GetCycle() - this.numCycles; i--)
			{
				if (fetchAutomatedChoreDeliveries.ContainsKey(i))
				{
					if (fetchDupeChoreDeliveries.ContainsKey(i) && (float)fetchDupeChoreDeliveries[i] >= (float)fetchAutomatedChoreDeliveries[i] * this.percentage)
					{
						break;
					}
					num++;
				}
				else if (fetchDupeChoreDeliveries.ContainsKey(i))
				{
					num = 0;
					break;
				}
			}
			this.currentCycleCount = Math.Max(this.currentCycleCount, num);
			return num >= this.numCycles;
		}

		// Token: 0x06006AFE RID: 27390 RVA: 0x0029AB8D File Offset: 0x00298D8D
		public void Deserialize(IReader reader)
		{
			this.numCycles = reader.ReadInt32();
			this.percentage = reader.ReadSingle();
		}

		// Token: 0x04004DC0 RID: 19904
		public float percentage;

		// Token: 0x04004DC1 RID: 19905
		public int numCycles;

		// Token: 0x04004DC2 RID: 19906
		public int currentCycleCount;

		// Token: 0x04004DC3 RID: 19907
		public bool armsOutPerformingDupesThisCycle;
	}
}
