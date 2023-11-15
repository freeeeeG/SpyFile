using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A68 RID: 2664
public class ServerTriggerDialogue : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x0600349F RID: 13471 RVA: 0x000F705E File Offset: 0x000F545E
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerDialogue;
	}

	// Token: 0x060034A0 RID: 13472 RVA: 0x000F7064 File Offset: 0x000F5464
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerDialogue = (TriggerDialogue)synchronisedObject;
		this.m_triggerDialogue.RegisterDialogueFinishedCallback(new GenericVoid<DialogueController.Dialogue>(this.OnDialogueFinished));
		this.m_iFlowController = GameUtils.GetFlowController();
		if (this.m_iFlowController != null)
		{
			this.m_iFlowController.RoundActivatedCallback += this.OnRoundBegun;
		}
	}

	// Token: 0x060034A1 RID: 13473 RVA: 0x000F70C8 File Offset: 0x000F54C8
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_iFlowController != null)
		{
			this.m_iFlowController.RoundActivatedCallback -= this.OnRoundBegun;
		}
	}

	// Token: 0x060034A2 RID: 13474 RVA: 0x000F70F2 File Offset: 0x000F54F2
	private void StartDialogue()
	{
		if (!this.m_isActive)
		{
			this.m_isActive = true;
			this.SendServerEvent(this.m_data);
		}
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x000F7112 File Offset: 0x000F5512
	public void OnDialogueFinished(DialogueController.Dialogue _dialogue)
	{
		if (_dialogue == this.m_triggerDialogue.m_dialogue)
		{
			this.m_isActive = false;
			base.gameObject.SendTrigger(this.m_triggerDialogue.m_onDialogueEndTrigger);
		}
	}

	// Token: 0x060034A4 RID: 13476 RVA: 0x000F7142 File Offset: 0x000F5542
	private void OnRoundBegun()
	{
		if (this.m_triggerDialogue != null && this.m_triggerDialogue.m_startOnAwake)
		{
			this.StartDialogue();
		}
	}

	// Token: 0x060034A5 RID: 13477 RVA: 0x000F716B File Offset: 0x000F556B
	public void OnTrigger(string _trigger)
	{
		if (_trigger == this.m_triggerDialogue.m_trigger && this.m_triggerDialogue.isActiveAndEnabled)
		{
			this.StartDialogue();
		}
	}

	// Token: 0x04002A2C RID: 10796
	private TriggerDialogue m_triggerDialogue;

	// Token: 0x04002A2D RID: 10797
	private TriggerDialogueMessage m_data = new TriggerDialogueMessage();

	// Token: 0x04002A2E RID: 10798
	private IFlowController m_iFlowController;

	// Token: 0x04002A2F RID: 10799
	private bool m_isActive;
}
