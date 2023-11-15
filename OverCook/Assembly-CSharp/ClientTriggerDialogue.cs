using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A69 RID: 2665
public class ClientTriggerDialogue : ClientSynchroniserBase
{
	// Token: 0x060034A7 RID: 13479 RVA: 0x000F71AC File Offset: 0x000F55AC
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerDialogue;
	}

	// Token: 0x060034A8 RID: 13480 RVA: 0x000F71B0 File Offset: 0x000F55B0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerDialogue = (TriggerDialogue)synchronisedObject;
		this.m_dialogueController = base.gameObject.RequireComponent<ClientDialogueController>();
	}

	// Token: 0x060034A9 RID: 13481 RVA: 0x000F71D8 File Offset: 0x000F55D8
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		TriggerDialogueMessage triggerDialogueMessage = (TriggerDialogueMessage)serialisable;
		base.StartCoroutine(this.RunDialogueRoutine());
	}

	// Token: 0x060034AA RID: 13482 RVA: 0x000F71F9 File Offset: 0x000F55F9
	public bool IsSpeaking()
	{
		return this.m_isSpeaking;
	}

	// Token: 0x060034AB RID: 13483 RVA: 0x000F7204 File Offset: 0x000F5604
	private IEnumerator RunDialogueRoutine()
	{
		this.m_isSpeaking = true;
		this.SetControlsEnabled(this.m_triggerDialogue.m_canMoveDuringDialog);
		IEnumerator routine = null;
		if (this.m_triggerDialogue.m_followObject != null)
		{
			routine = this.m_dialogueController.StartDialogue(this.m_triggerDialogue.m_dialogue, this.m_triggerDialogue.m_followObject);
		}
		else
		{
			routine = this.m_dialogueController.StartDialogue(this.m_triggerDialogue.m_dialogue, this.m_triggerDialogue.m_anchor, this.m_triggerDialogue.m_pivot, this.m_triggerDialogue.m_rotation);
		}
		while (routine.MoveNext())
		{
			yield return null;
		}
		this.m_dialogueController.Shutdown(this.m_triggerDialogue.m_dialogue);
		this.SetControlsEnabled(true);
		this.m_isSpeaking = false;
		this.m_triggerDialogue.OnDialogueFinished();
		yield break;
	}

	// Token: 0x060034AC RID: 13484 RVA: 0x000F7220 File Offset: 0x000F5620
	private void SetControlsEnabled(bool _enabled)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < array.Length; i++)
		{
			PlayerControls playerControls = array[i].RequireComponent<PlayerControls>();
			if (_enabled)
			{
				Suppressor suppressor;
				if (this.m_controlsSuppressor.TryGetValue(playerControls, out suppressor))
				{
					suppressor.Release();
					this.m_controlsSuppressor.Remove(playerControls);
				}
			}
			else if (!this.m_controlsSuppressor.ContainsKey(playerControls))
			{
				this.m_controlsSuppressor.Add(playerControls, playerControls.Suppress(this));
			}
		}
	}

	// Token: 0x04002A30 RID: 10800
	private TriggerDialogue m_triggerDialogue;

	// Token: 0x04002A31 RID: 10801
	private Dictionary<PlayerControls, Suppressor> m_controlsSuppressor = new Dictionary<PlayerControls, Suppressor>();

	// Token: 0x04002A32 RID: 10802
	private ClientDialogueController m_dialogueController;

	// Token: 0x04002A33 RID: 10803
	private bool m_isSpeaking;
}
