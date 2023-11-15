using System;
using UnityEngine;

// Token: 0x020003C3 RID: 963
public class ClientHeldItemMeshVisibility : ClientMeshVisibilityBase<HeldItemMeshVisibility.VisState>, ICarryNotified
{
	// Token: 0x060011EB RID: 4587 RVA: 0x00065F77 File Offset: 0x00064377
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		base.Setup(HeldItemMeshVisibility.VisState.NotCarrying);
	}

	// Token: 0x060011EC RID: 4588 RVA: 0x00065F87 File Offset: 0x00064387
	public void SetVisState(HeldItemMeshVisibility.VisState _visState)
	{
		base.SetState(_visState);
	}

	// Token: 0x060011ED RID: 4589 RVA: 0x00065F90 File Offset: 0x00064390
	public void OnCarryBegun(ICarrier _carrier)
	{
		this.SetVisState(HeldItemMeshVisibility.VisState.Carrying);
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x00065F99 File Offset: 0x00064399
	public void OnCarryEnded(ICarrier _carrier)
	{
		this.SetVisState(HeldItemMeshVisibility.VisState.NotCarrying);
	}
}
