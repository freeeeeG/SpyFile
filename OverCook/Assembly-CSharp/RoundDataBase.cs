using System;

// Token: 0x020006D4 RID: 1748
public abstract class RoundDataBase
{
	// Token: 0x06002114 RID: 8468
	public abstract RoundInstanceDataBase InitialiseRound();

	// Token: 0x06002115 RID: 8469
	public abstract RecipeList.Entry[] GetNextRecipe(RoundInstanceDataBase _data);
}
