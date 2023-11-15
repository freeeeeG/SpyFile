using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006D1 RID: 1745
[Serializable]
public class DynamicRoundData : RoundData
{
	// Token: 0x0600210B RID: 8459 RVA: 0x0009DF30 File Offset: 0x0009C330
	public override RoundInstanceDataBase InitialiseRound()
	{
		DynamicRoundData.DynamicRoundInstanceData dynamicRoundInstanceData = new DynamicRoundData.DynamicRoundInstanceData();
		DynamicRoundData.Phase phase = this.Phases[0];
		dynamicRoundInstanceData.CumulativeFrequencies = new int[phase.Recipes.m_recipes.Length];
		return dynamicRoundInstanceData;
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x0009DF68 File Offset: 0x0009C368
	public virtual void MoveToNextPhase(RoundInstanceDataBase _data)
	{
		if (this.GetRemainingPhases(_data) > 0)
		{
			DynamicRoundData.DynamicRoundInstanceData dynamicRoundInstanceData = _data as DynamicRoundData.DynamicRoundInstanceData;
			dynamicRoundInstanceData.CurrentPhase++;
			DynamicRoundData.Phase phase = this.Phases[dynamicRoundInstanceData.CurrentPhase];
			dynamicRoundInstanceData.RecipeCount = 0;
			dynamicRoundInstanceData.CumulativeFrequencies = new int[phase.Recipes.m_recipes.Length];
		}
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x0009DFC4 File Offset: 0x0009C3C4
	public float GetCurrentPhaseDuration(RoundInstanceDataBase _data)
	{
		DynamicRoundData.DynamicRoundInstanceData dynamicRoundInstanceData = _data as DynamicRoundData.DynamicRoundInstanceData;
		return this.Phases[dynamicRoundInstanceData.CurrentPhase].Duration;
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x0009DFEC File Offset: 0x0009C3EC
	public int GetRemainingPhases(RoundInstanceDataBase _data)
	{
		DynamicRoundData.DynamicRoundInstanceData dynamicRoundInstanceData = _data as DynamicRoundData.DynamicRoundInstanceData;
		return this.Phases.Length - 1 - dynamicRoundInstanceData.CurrentPhase;
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x0009E014 File Offset: 0x0009C414
	public override RecipeList.Entry[] GetNextRecipe(RoundInstanceDataBase _data)
	{
		DynamicRoundData.DynamicRoundInstanceData instance = _data as DynamicRoundData.DynamicRoundInstanceData;
		DynamicRoundData.Phase phase = this.Phases[instance.CurrentPhase];
		instance.RecipeCount++;
		KeyValuePair<int, RecipeList.Entry> weightedRandomElement = phase.Recipes.m_recipes.GetWeightedRandomElement((int i, RecipeList.Entry e) => this.GetWeight(instance, i));
		instance.CumulativeFrequencies[weightedRandomElement.Key]++;
		return new RecipeList.Entry[]
		{
			weightedRandomElement.Value
		};
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x0009E0AC File Offset: 0x0009C4AC
	private float GetWeight(RoundData.RoundInstanceData _data, int _recipeIndex)
	{
		DynamicRoundData.DynamicRoundInstanceData dynamicRoundInstanceData = _data as DynamicRoundData.DynamicRoundInstanceData;
		DynamicRoundData.Phase phase = this.Phases[dynamicRoundInstanceData.CurrentPhase];
		int num = 0;
		for (int i = 0; i < _data.CumulativeFrequencies.Length; i++)
		{
			num += _data.CumulativeFrequencies[i];
		}
		float num2 = (float)(num + 2) / (float)phase.Recipes.m_recipes.Length;
		return Mathf.Max(num2 - (float)_data.CumulativeFrequencies[_recipeIndex], 0f);
	}

	// Token: 0x04001930 RID: 6448
	public DynamicRoundData.Phase[] Phases = new DynamicRoundData.Phase[0];

	// Token: 0x020006D2 RID: 1746
	[Serializable]
	public class Phase
	{
		// Token: 0x04001931 RID: 6449
		public RecipeList Recipes;

		// Token: 0x04001932 RID: 6450
		public float Duration;
	}

	// Token: 0x020006D3 RID: 1747
	protected class DynamicRoundInstanceData : RoundData.RoundInstanceData
	{
		// Token: 0x04001933 RID: 6451
		public int CurrentPhase;
	}
}
