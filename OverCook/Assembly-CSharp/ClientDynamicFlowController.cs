using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200066C RID: 1644
public class ClientDynamicFlowController : ClientCampaignFlowController
{
	// Token: 0x06001F5C RID: 8028 RVA: 0x00095744 File Offset: 0x00093B44
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_dynamicFlowController = (DynamicFlowController)synchronisedObject;
	}

	// Token: 0x06001F5D RID: 8029 RVA: 0x00095759 File Offset: 0x00093B59
	protected override void Awake()
	{
		base.Awake();
		Mailbox.Client.RegisterForMessageType(MessageType.DynamicLevel, new OrderedMessageReceivedCallback(this.OnDynamicLevelMessage));
	}

	// Token: 0x06001F5E RID: 8030 RVA: 0x00095779 File Offset: 0x00093B79
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Mailbox.Client.UnregisterForMessageType(MessageType.DynamicLevel, new OrderedMessageReceivedCallback(this.OnDynamicLevelMessage));
	}

	// Token: 0x06001F5F RID: 8031 RVA: 0x0009579C File Offset: 0x00093B9C
	protected void OnDynamicLevelMessage(IOnlineMultiplayerSessionUserId _sessionId, Serialisable _serialisable)
	{
		DynamicLevelMessage dynamicLevelMessage = (DynamicLevelMessage)_serialisable;
		IEnumerator item = this.BuildTransitionToPhaseRoutine(dynamicLevelMessage.m_phase);
		this.m_phaseQueue.Enqueue(item);
	}

	// Token: 0x06001F60 RID: 8032 RVA: 0x000957CC File Offset: 0x00093BCC
	protected override ClientOrderControllerBase BuildOrderController(RecipeFlowGUI _recipeUI)
	{
		ClientDynamicOrderController clientDynamicOrderController = new ClientDynamicOrderController(_recipeUI);
		clientDynamicOrderController.SetRoundTimer(base.RoundTimer);
		return clientDynamicOrderController;
	}

	// Token: 0x06001F61 RID: 8033 RVA: 0x000957F0 File Offset: 0x00093BF0
	protected override void OnUpdateInRound()
	{
		base.OnUpdateInRound();
		if (this.m_phaseQueue.Count > 0)
		{
			IEnumerator enumerator = this.m_phaseQueue.Peek();
			if (enumerator == null || !enumerator.MoveNext())
			{
				this.m_phaseQueue.Dequeue();
			}
		}
	}

	// Token: 0x06001F62 RID: 8034 RVA: 0x00095840 File Offset: 0x00093C40
	protected virtual IEnumerator BuildTransitionToPhaseRoutine(int _newPhase)
	{
		int index = _newPhase - 1;
		GameObject obj = this.m_dynamicFlowController.m_transitions[index];
		obj.SetActive(true);
		DynamicTransitionBase transition = obj.RequireComponent<DynamicTransitionBase>();
		transition.Setup(delegate
		{
		});
		IEnumerator routine = transition.Run();
		while (routine.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040017EB RID: 6123
	private DynamicFlowController m_dynamicFlowController;

	// Token: 0x040017EC RID: 6124
	private Queue<IEnumerator> m_phaseQueue = new Queue<IEnumerator>();
}
