using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A55 RID: 2645
public class ServerDialogueController : ServerSynchroniserBase
{
	// Token: 0x0600343E RID: 13374 RVA: 0x000F5660 File Offset: 0x000F3A60
	public override EntityType GetEntityType()
	{
		return EntityType.Dialogue;
	}

	// Token: 0x0600343F RID: 13375 RVA: 0x000F5664 File Offset: 0x000F3A64
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_dialogueController = (DialogueController)synchronisedObject;
		this.m_dialogueController.RegisterDialogueStateCallback(new GenericVoid<DialogueController.Dialogue, int>(this.OnDialogueStateChanged));
	}

	// Token: 0x06003440 RID: 13376 RVA: 0x000F5690 File Offset: 0x000F3A90
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.m_dialogueController.UnRegisterDialogueStateCallback(new GenericVoid<DialogueController.Dialogue, int>(this.OnDialogueStateChanged));
	}

	// Token: 0x06003441 RID: 13377 RVA: 0x000F56AF File Offset: 0x000F3AAF
	private void OnDialogueStateChanged(DialogueController.Dialogue _dialogue, int _state)
	{
		this.m_data.Initialise(_dialogue, _state);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x040029EE RID: 10734
	private DialogueController m_dialogueController;

	// Token: 0x040029EF RID: 10735
	private DialogueStateMessage m_data = new DialogueStateMessage();
}
