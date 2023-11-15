using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200040E RID: 1038
public class ClientWaterGunCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x060012C4 RID: 4804 RVA: 0x000693E4 File Offset: 0x000677E4
	public override void StartSynchronising(Component _synchronisedObject)
	{
		base.StartSynchronising(_synchronisedObject);
		this.m_cosmetics = (WaterGunCosmeticDecisions)_synchronisedObject;
		this.m_isHeldParam = Animator.StringToHash(this.m_cosmetics.m_isHeldParam);
		this.m_attachment = base.gameObject.RequireInterface<IClientAttachment>();
		this.m_attachment.RegisterAttachChangedCallback(new AttachChangedCallback(this.OnAttachmentChanged));
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x00069444 File Offset: 0x00067844
	private void OnAttachmentChanged(IParentable _parentable)
	{
		bool value = false;
		if (_parentable != null && _parentable is PlayerAttachmentCarrier)
		{
			value = true;
		}
		this.m_cosmetics.m_animator.SetBool(this.m_isHeldParam, value);
	}

	// Token: 0x060012C6 RID: 4806 RVA: 0x0006947D File Offset: 0x0006787D
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_attachment != null)
		{
			this.m_attachment.UnregisterAttachChangedCallback(new AttachChangedCallback(this.OnAttachmentChanged));
		}
	}

	// Token: 0x04000EC4 RID: 3780
	private int m_isHeldParam = -1;

	// Token: 0x04000EC5 RID: 3781
	private WaterGunCosmeticDecisions m_cosmetics;

	// Token: 0x04000EC6 RID: 3782
	private IClientAttachment m_attachment;
}
