using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020004EE RID: 1262
[Serializable]
public abstract class LevelConfigBase : ScriptableObject
{
	// Token: 0x06001796 RID: 6038 RVA: 0x000783FB File Offset: 0x000767FB
	public virtual List<OrderDefinitionNode> GetAllRecipes()
	{
		return new List<OrderDefinitionNode>();
	}

	// Token: 0x0400113C RID: 4412
	public HazardInfo m_hazardInfo;

	// Token: 0x0400113D RID: 4413
	public bool m_disableDynamicParenting = true;

	// Token: 0x0400113E RID: 4414
	public bool m_gridSelection = true;

	// Token: 0x0400113F RID: 4415
	public bool m_showIconPrompts;

	// Token: 0x04001140 RID: 4416
	[FormerlySerializedAs("m_enableLevelBoundsCheck")]
	public bool m_enableRespawnBounds;

	// Token: 0x04001141 RID: 4417
	public LevelObjectiveBase[] m_objectives;

	// Token: 0x04001142 RID: 4418
	public RecipeMatchList m_recipeMatchingList;
}
