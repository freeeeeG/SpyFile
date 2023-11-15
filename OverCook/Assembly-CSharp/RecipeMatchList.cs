using System;
using UnityEngine;

// Token: 0x02000768 RID: 1896
[Serializable]
public class RecipeMatchList : ScriptableObject
{
	// Token: 0x04001BE5 RID: 7141
	public RecipeMatchList[] m_includeLists;

	// Token: 0x04001BE6 RID: 7142
	public OrderDefinitionNode[] m_recipes;

	// Token: 0x04001BE7 RID: 7143
	public CookingStepData[] m_cookingSteps;
}
