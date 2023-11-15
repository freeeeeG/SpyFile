using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200078D RID: 1933
public class ServerTutorialPopupController : ServerSynchroniserBase
{
	// Token: 0x0600255E RID: 9566 RVA: 0x000B0F24 File Offset: 0x000AF324
	public override EntityType GetEntityType()
	{
		return EntityType.TutorialPopup;
	}

	// Token: 0x0600255F RID: 9567 RVA: 0x000B0F28 File Offset: 0x000AF328
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_controller = (TutorialPopupController)synchronisedObject;
		this.m_controller.RegisterDismissCallback(new CallbackVoid(this.OnTutorialDismissed));
	}

	// Token: 0x06002560 RID: 9568 RVA: 0x000B0F54 File Offset: 0x000AF354
	private void OnTutorialDismissed()
	{
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x04001CF1 RID: 7409
	private TutorialPopupController m_controller;

	// Token: 0x04001CF2 RID: 7410
	private TutorialDismissMessage m_data = new TutorialDismissMessage();
}
