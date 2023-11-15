using System;

// Token: 0x020006C3 RID: 1731
public interface IRecipeListCache
{
	// Token: 0x060020BC RID: 8380
	OrderDefinitionNode[] GetCachedRecipeList();

	// Token: 0x060020BD RID: 8381
	AssembledDefinitionNode[] GetCachedAssembledRecipes();

	// Token: 0x060020BE RID: 8382
	CookingStepData[] GetCachedCookingSteps();
}
