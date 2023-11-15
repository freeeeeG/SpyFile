using System;
using GameModes;
using UnityEngine;

// Token: 0x020006CA RID: 1738
[Serializable]
public class BossRoundData : DynamicRoundData
{
	// Token: 0x060020FA RID: 8442 RVA: 0x0009E154 File Offset: 0x0009C554
	public override RoundInstanceDataBase InitialiseRound()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		this.m_gameModeKind = gameSession.GameModeKind;
		return new BossRoundData.BossRoundInstanceData();
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x0009E17C File Offset: 0x0009C57C
	public override RecipeList.Entry[] GetNextRecipe(RoundInstanceDataBase _data)
	{
		BossRoundData.BossRoundInstanceData bossRoundInstanceData = _data as BossRoundData.BossRoundInstanceData;
		DynamicRoundData.Phase phase = this.Phases[bossRoundInstanceData.CurrentPhase];
		if (bossRoundInstanceData.CurrentPhase == this.Phases.Length - 1 && (this.m_gameModeKind == Kind.Practice || this.m_gameModeKind == Kind.Survival))
		{
			return new RecipeList.Entry[]
			{
				phase.Recipes.m_recipes[UnityEngine.Random.Range(0, phase.Recipes.m_recipes.Length)]
			};
		}
		if (this.NoMoreRecipesToIssueInPhase(_data))
		{
			return new RecipeList.Entry[0];
		}
		RecipeList.Entry entry = phase.Recipes.m_recipes[bossRoundInstanceData.RecipeCount++];
		return new RecipeList.Entry[]
		{
			entry
		};
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x0009E230 File Offset: 0x0009C630
	public bool NoMoreRecipesToIssueInPhase(RoundInstanceDataBase _data)
	{
		BossRoundData.BossRoundInstanceData bossRoundInstanceData = _data as BossRoundData.BossRoundInstanceData;
		return bossRoundInstanceData.RecipeCount >= this.Phases[bossRoundInstanceData.CurrentPhase].Recipes.m_recipes.Length;
	}

	// Token: 0x04001923 RID: 6435
	private Kind m_gameModeKind;

	// Token: 0x020006CB RID: 1739
	protected class BossRoundInstanceData : DynamicRoundData.DynamicRoundInstanceData
	{
	}
}
