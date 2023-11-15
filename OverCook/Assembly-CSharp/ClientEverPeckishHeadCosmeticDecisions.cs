using System;
using UnityEngine;

// Token: 0x02000A50 RID: 2640
public class ClientEverPeckishHeadCosmeticDecisions : ClientMeshVisibilityBase<EverPeckishHeadCosmeticDecisions.VisState>
{
	// Token: 0x0600342C RID: 13356 RVA: 0x000F5234 File Offset: 0x000F3634
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cosmeticDecision = (EverPeckishHeadCosmeticDecisions)synchronisedObject;
		base.Setup(this.m_cosmeticDecision.m_initialVisState);
	}

	// Token: 0x0600342D RID: 13357 RVA: 0x000F525A File Offset: 0x000F365A
	public void SetVisState(EverPeckishHeadCosmeticDecisions.VisState _visState)
	{
		base.SetState(_visState);
	}

	// Token: 0x040029DD RID: 10717
	private EverPeckishHeadCosmeticDecisions m_cosmeticDecision;
}
