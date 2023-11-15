using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003E9 RID: 1001
public class ClientPressureSwitchCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x06001268 RID: 4712 RVA: 0x00067B70 File Offset: 0x00065F70
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_decisions = (PressureSwitchCosmeticDecisions)synchronisedObject;
		this.m_triggerZone = base.gameObject.RequireComponent<ClientTriggerZone>();
		this.m_normalY = this.m_decisions.m_buttonBit.transform.localPosition.y;
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x00067BC4 File Offset: 0x00065FC4
	private void Update()
	{
		if (this.m_triggerZone == null)
		{
			return;
		}
		if (this.m_triggerZone.IsOccupied())
		{
			this.m_decisions.m_buttonBit.material = this.m_decisions.m_occupiedMaterial;
			this.m_decisions.m_buttonBit.transform.localPosition = this.m_decisions.m_buttonBit.transform.localPosition.WithY(this.m_normalY + this.m_decisions.m_occupiedButtonVerticalOffset);
		}
		else
		{
			this.m_decisions.m_buttonBit.material = this.m_decisions.m_unoccuppiedMaterial;
			this.m_decisions.m_buttonBit.transform.localPosition = this.m_decisions.m_buttonBit.transform.localPosition.WithY(this.m_normalY);
		}
	}

	// Token: 0x04000E68 RID: 3688
	private PressureSwitchCosmeticDecisions m_decisions;

	// Token: 0x04000E69 RID: 3689
	private ClientTriggerZone m_triggerZone;

	// Token: 0x04000E6A RID: 3690
	private float m_normalY;
}
