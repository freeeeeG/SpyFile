using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200028E RID: 654
public class EnemyTips : IUserInterface
{
	// Token: 0x06001024 RID: 4132 RVA: 0x0002B3EC File Offset: 0x000295EC
	public void ReadSequenceInfo(List<EnemySequence> sequences)
	{
		for (int i = 0; i < 3; i++)
		{
			if (i >= sequences.Count)
			{
				this.enemyGrids[i].gameObject.SetActive(false);
			}
			else
			{
				this.enemyGrids[i].gameObject.SetActive(true);
				this.enemyGrids[i].SetEnemyInfo(Singleton<StaticData>.Instance.EnemyFactory.Get(sequences[i].EnemyType));
			}
		}
	}

	// Token: 0x04000866 RID: 2150
	[SerializeField]
	private EnemyGrid[] enemyGrids;
}
