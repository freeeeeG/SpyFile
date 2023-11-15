using System;
using UnityEngine;

// Token: 0x0200061C RID: 1564
[AddComponentMenu("Scripts/Game/Environment/Utensils/UsableItem")]
public class ServerUsableItem : ServerInteractable, ICarryNotified
{
	// Token: 0x06001DA1 RID: 7585 RVA: 0x00090530 File Offset: 0x0008E930
	public override bool CanInteract(GameObject _interacter)
	{
		IPlayerCarrier playerCarrier = _interacter.RequireInterface<IPlayerCarrier>();
		GameObject x = playerCarrier.InspectCarriedItem();
		return base.CanInteract(_interacter) && x == base.gameObject;
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x00090566 File Offset: 0x0008E966
	public override bool InteractionIsSticky()
	{
		return base.InteractionIsSticky() || !this.UnderDirectControl();
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x0009057F File Offset: 0x0008E97F
	private bool UnderDirectControl()
	{
		return this.m_controls != null && this.m_controls.GetDirectlyUnderPlayerControl();
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x000905A0 File Offset: 0x0008E9A0
	public virtual void OnCarryBegun(ICarrier _carrier)
	{
		GameObject gameObject = (_carrier as MonoBehaviour).gameObject;
		this.m_controls = gameObject.RequestComponent<PlayerControls>();
	}

	// Token: 0x06001DA5 RID: 7589 RVA: 0x000905C5 File Offset: 0x0008E9C5
	public virtual void OnCarryEnded(ICarrier _carrier)
	{
		this.m_controls = null;
	}

	// Token: 0x040016E9 RID: 5865
	private PlayerControls m_controls;
}
