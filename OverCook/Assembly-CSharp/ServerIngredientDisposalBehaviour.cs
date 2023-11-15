using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000480 RID: 1152
public class ServerIngredientDisposalBehaviour : ServerSynchroniserBase, IDisposalBehaviour
{
	// Token: 0x06001577 RID: 5495 RVA: 0x00074600 File Offset: 0x00072A00
	public void AddToDisposer(ICarrier _carrier, IDisposer _iDisposer)
	{
		GameObject gameObject = _carrier.TakeItem();
		_iDisposer.PassToDestroy(this.m_iAttachment);
		ServerMessenger.TriggerAudioMessage(GameOneShotAudioTag.TrashCan, base.gameObject.layer);
	}

	// Token: 0x06001578 RID: 5496 RVA: 0x00074633 File Offset: 0x00072A33
	public void AddToDisposer(IDisposer _iDisposer)
	{
		ServerMessenger.TriggerAudioMessage(GameOneShotAudioTag.TrashCan, base.gameObject.layer);
		_iDisposer.PassToDestroy(this.m_iAttachment);
	}

	// Token: 0x06001579 RID: 5497 RVA: 0x00074654 File Offset: 0x00072A54
	public bool WillBeDestroyed()
	{
		return true;
	}

	// Token: 0x0600157A RID: 5498 RVA: 0x00074657 File Offset: 0x00072A57
	private void Awake()
	{
		this.m_iAttachment = base.gameObject.RequestInterface<IAttachment>();
	}

	// Token: 0x0600157B RID: 5499 RVA: 0x0007466A File Offset: 0x00072A6A
	public void Destroying(IDisposer disposer)
	{
		if (!this.m_isDestroyed)
		{
			ServerMessenger.TriggerAudioMessage(GameOneShotAudioTag.TrashCan, base.gameObject.layer);
			this.m_isDestroyed = true;
		}
	}

	// Token: 0x04001062 RID: 4194
	private IAttachment m_iAttachment;

	// Token: 0x04001063 RID: 4195
	private bool m_isDestroyed;
}
