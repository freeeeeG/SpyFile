using System;
using UnityEngine;

// Token: 0x020007FB RID: 2043
[AddComponentMenu("Scripts/Game/Ingredients/IngredientMeshVisibility")]
public class IngredientMeshVisibility : MeshVisibilityBase<IngredientMeshVisibility.VisState>
{
	// Token: 0x020007FC RID: 2044
	public enum VisState
	{
		// Token: 0x04001EDD RID: 7901
		Whole,
		// Token: 0x04001EDE RID: 7902
		Working
	}
}
