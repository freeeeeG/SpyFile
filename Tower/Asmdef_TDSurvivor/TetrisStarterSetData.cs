using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000037 RID: 55
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/開局的Tetris組合設定 (TetrisStarterSetData)", order = 1)]
public class TetrisStarterSetData : ScriptableObject
{
	// Token: 0x06000113 RID: 275 RVA: 0x000052E4 File Offset: 0x000034E4
	public TetrisStarterSet GetWeightedRandomStarterSet(bool isTutorialFinished)
	{
		WeightedRandom<TetrisStarterSet> weightedRandom = new WeightedRandom<TetrisStarterSet>();
		foreach (TetrisStarterSet tetrisStarterSet in this.list_TetrisStarterSet)
		{
			if (isTutorialFinished || tetrisStarterSet.CanShowBeforeTutorial)
			{
				weightedRandom.AddItem(tetrisStarterSet, tetrisStarterSet.weight);
			}
		}
		return weightedRandom.GetRandomResult();
	}

	// Token: 0x040000C0 RID: 192
	[SerializeField]
	private List<TetrisStarterSet> list_TetrisStarterSet;
}
