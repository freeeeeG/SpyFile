using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006D6 RID: 1750
[Serializable]
public class RoundData : RoundDataBase
{
	// Token: 0x06002118 RID: 8472 RVA: 0x0009DD98 File Offset: 0x0009C198
	public override RoundInstanceDataBase InitialiseRound()
	{
		return new RoundData.RoundInstanceData
		{
			CumulativeFrequencies = new int[this.m_recipes.m_recipes.Length]
		};
	}

	// Token: 0x06002119 RID: 8473 RVA: 0x0009DDC4 File Offset: 0x0009C1C4
	public override RecipeList.Entry[] GetNextRecipe(RoundInstanceDataBase _data)
	{
		RoundData.RoundInstanceData data = _data as RoundData.RoundInstanceData;
		data.RecipeCount++;
		KeyValuePair<int, RecipeList.Entry> weightedRandomElement = this.m_recipes.m_recipes.GetWeightedRandomElement((int i, RecipeList.Entry e) => this.GetWeight(data, i));
		data.CumulativeFrequencies[weightedRandomElement.Key]++;
		return new RecipeList.Entry[]
		{
			weightedRandomElement.Value
		};
	}

	// Token: 0x0600211A RID: 8474 RVA: 0x0009DE48 File Offset: 0x0009C248
	private float GetWeight(RoundData.RoundInstanceData _instance, int _recipeIndex)
	{
		int num = _instance.CumulativeFrequencies.Collapse((int f, int total) => total + f);
		float num2 = (float)(num + 2) / (float)this.m_recipes.m_recipes.Length;
		return Mathf.Max(num2 - (float)_instance.CumulativeFrequencies[_recipeIndex], 0f);
	}

	// Token: 0x0600211B RID: 8475 RVA: 0x0009DEA8 File Offset: 0x0009C2A8
	private void DebugPrint(RoundData.RoundInstanceData _instance)
	{
		for (int i = 0; i < this.m_recipes.m_recipes.Length; i++)
		{
			RecipeList.Entry entry = this.m_recipes.m_recipes[i];
			float weight = this.GetWeight(_instance, i);
		}
	}

	// Token: 0x04001934 RID: 6452
	public RecipeList m_recipes;

	// Token: 0x04001935 RID: 6453
	public float m_roundTimer = 150f;

	// Token: 0x020006D7 RID: 1751
	protected class RoundInstanceData : RoundInstanceDataBase
	{
		// Token: 0x04001937 RID: 6455
		public int RecipeCount;

		// Token: 0x04001938 RID: 6456
		public int[] CumulativeFrequencies;
	}
}
