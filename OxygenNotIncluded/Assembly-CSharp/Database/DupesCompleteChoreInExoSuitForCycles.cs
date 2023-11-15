using System;
using System.Collections.Generic;
using System.Linq;

namespace Database
{
	// Token: 0x02000D58 RID: 3416
	public class DupesCompleteChoreInExoSuitForCycles : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AFF RID: 27391 RVA: 0x0029ABA7 File Offset: 0x00298DA7
		public DupesCompleteChoreInExoSuitForCycles(int numCycles)
		{
			this.numCycles = numCycles;
		}

		// Token: 0x06006B00 RID: 27392 RVA: 0x0029ABB8 File Offset: 0x00298DB8
		public override bool Success()
		{
			Dictionary<int, List<int>> dupesCompleteChoresInSuits = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().dupesCompleteChoresInSuits;
			Dictionary<int, float> dictionary = new Dictionary<int, float>();
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				dictionary.Add(minionIdentity.GetComponent<KPrefabID>().InstanceID, minionIdentity.arrivalTime);
			}
			int num = 0;
			int num2 = Math.Min(dupesCompleteChoresInSuits.Count, this.numCycles);
			for (int i = GameClock.Instance.GetCycle() - num2; i <= GameClock.Instance.GetCycle(); i++)
			{
				if (dupesCompleteChoresInSuits.ContainsKey(i))
				{
					List<int> list = dictionary.Keys.Except(dupesCompleteChoresInSuits[i]).ToList<int>();
					bool flag = true;
					foreach (int key in list)
					{
						if (dictionary[key] < (float)i)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						num++;
					}
					else if (i != GameClock.Instance.GetCycle())
					{
						num = 0;
					}
					this.currentCycleStreak = num;
					if (num >= this.numCycles)
					{
						this.currentCycleStreak = this.numCycles;
						return true;
					}
				}
				else
				{
					this.currentCycleStreak = Math.Max(this.currentCycleStreak, num);
					num = 0;
				}
			}
			return false;
		}

		// Token: 0x06006B01 RID: 27393 RVA: 0x0029AD38 File Offset: 0x00298F38
		public void Deserialize(IReader reader)
		{
			this.numCycles = reader.ReadInt32();
		}

		// Token: 0x06006B02 RID: 27394 RVA: 0x0029AD48 File Offset: 0x00298F48
		public int GetNumberOfDupesForCycle(int cycle)
		{
			int result = 0;
			Dictionary<int, List<int>> dupesCompleteChoresInSuits = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().dupesCompleteChoresInSuits;
			if (dupesCompleteChoresInSuits.ContainsKey(GameClock.Instance.GetCycle()))
			{
				result = dupesCompleteChoresInSuits[GameClock.Instance.GetCycle()].Count;
			}
			return result;
		}

		// Token: 0x04004DC4 RID: 19908
		public int currentCycleStreak;

		// Token: 0x04004DC5 RID: 19909
		public int numCycles;
	}
}
