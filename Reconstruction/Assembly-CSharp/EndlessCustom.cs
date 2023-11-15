using System;
using UnityEngine;

// Token: 0x0200027C RID: 636
public class EndlessCustom : MonoBehaviour
{
	// Token: 0x06000FC9 RID: 4041 RVA: 0x0002A449 File Offset: 0x00028649
	public void Initialize()
	{
		this.m_BattleRecipe.Initialize();
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x0002A456 File Offset: 0x00028656
	public void EndlessModeStart()
	{
		this.m_BattleRecipe.UpdateRecipes();
		this.m_BattleRule.UpdateRules();
		Singleton<LevelManager>.Instance.StartNewGame(11);
	}

	// Token: 0x0400081D RID: 2077
	[SerializeField]
	private BattleRule m_BattleRule;

	// Token: 0x0400081E RID: 2078
	[SerializeField]
	private BattleRecipe m_BattleRecipe;
}
