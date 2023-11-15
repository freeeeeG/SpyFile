using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005FE RID: 1534
public class ServerContentsDisposalBehaviour : ServerSynchroniserBase, IDisposalBehaviour
{
	// Token: 0x06001D27 RID: 7463 RVA: 0x0008F5FE File Offset: 0x0008D9FE
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_container = base.gameObject.RequireComponent<ServerIngredientContainer>();
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x0008F611 File Offset: 0x0008DA11
	public void AddToDisposer(ICarrier _carrier, IDisposer _iDisposer)
	{
		if (this.m_container.GetContentsCount() > 0)
		{
			ServerMessenger.TriggerAudioMessage(GameOneShotAudioTag.TrashCan, base.gameObject.layer);
			ServerMessenger.Achievement(_carrier.AccessGameObject(), 14, 1);
		}
		this.m_container.Empty();
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x0008F651 File Offset: 0x0008DA51
	public void AddToDisposer(IDisposer _iDisposer)
	{
		this.m_container.Empty();
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x0008F65E File Offset: 0x0008DA5E
	public bool WillBeDestroyed()
	{
		return false;
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x0008F661 File Offset: 0x0008DA61
	public void Destroying(IDisposer disposer)
	{
		if (this.m_container.GetContentsCount() > 0)
		{
			ServerMessenger.TriggerAudioMessage(GameOneShotAudioTag.TrashCan, base.gameObject.layer);
		}
	}

	// Token: 0x040016AE RID: 5806
	private ServerIngredientContainer m_container;
}
