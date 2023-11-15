using System;

// Token: 0x020006D8 RID: 1752
[Serializable]
public class ScriptedRoundData : RoundData
{
	// Token: 0x0600211F RID: 8479 RVA: 0x0009E2A8 File Offset: 0x0009C6A8
	public override RecipeList.Entry[] GetNextRecipe(RoundInstanceDataBase _data)
	{
		RoundData.RoundInstanceData roundInstanceData = _data as RoundData.RoundInstanceData;
		if (roundInstanceData.RecipeCount < this.m_manualOrder.Length)
		{
			return new RecipeList.Entry[]
			{
				this.m_manualOrder[roundInstanceData.RecipeCount++]
			};
		}
		return base.GetNextRecipe(roundInstanceData);
	}

	// Token: 0x04001939 RID: 6457
	public RecipeList.Entry[] m_manualOrder;
}
