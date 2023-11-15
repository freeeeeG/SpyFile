using System;
using UnityEngine;

// Token: 0x020007FA RID: 2042
public class ClientIngredientMeshVisibility : ClientMeshVisibilityBase<IngredientMeshVisibility.VisState>
{
	// Token: 0x06002726 RID: 10022 RVA: 0x000B91EE File Offset: 0x000B75EE
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		base.Setup(IngredientMeshVisibility.VisState.Whole);
	}

	// Token: 0x06002727 RID: 10023 RVA: 0x000B91FE File Offset: 0x000B75FE
	public void SetVisState(IngredientMeshVisibility.VisState _visState)
	{
		base.SetState(_visState);
	}
}
