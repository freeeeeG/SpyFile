using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000677 RID: 1655
[ExecutionDependency(typeof(IFlowController))]
public class FlowbasedComponentActivation : MonoBehaviour
{
	// Token: 0x06001F9E RID: 8094 RVA: 0x0009A523 File Offset: 0x00098923
	private void OnValidate()
	{
		if (this.m_targetComponent != null)
		{
			this.m_targetComponent.enabled = this.m_activeOutOfRound;
		}
	}

	// Token: 0x06001F9F RID: 8095 RVA: 0x0009A547 File Offset: 0x00098947
	private void Awake()
	{
		this.m_targetComponent.enabled = this.m_activeOutOfRound;
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06001FA0 RID: 8096 RVA: 0x0009A574 File Offset: 0x00098974
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.Initialise();
		}
	}

	// Token: 0x06001FA1 RID: 8097 RVA: 0x0009A59C File Offset: 0x0009899C
	private void Initialise()
	{
		this.m_iFlowController = GameUtils.GetFlowController();
		if (this.m_iFlowController != null)
		{
			this.m_iFlowController.RoundActivatedCallback += this.OnBegun;
			this.m_iFlowController.RoundDeactivatedCallback += this.OnEnded;
		}
	}

	// Token: 0x06001FA2 RID: 8098 RVA: 0x0009A5F0 File Offset: 0x000989F0
	private void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		if (this.m_iFlowController != null)
		{
			this.m_iFlowController.RoundActivatedCallback -= this.OnBegun;
			this.m_iFlowController.RoundDeactivatedCallback -= this.OnEnded;
		}
	}

	// Token: 0x06001FA3 RID: 8099 RVA: 0x0009A64E File Offset: 0x00098A4E
	private void OnBegun()
	{
		this.m_targetComponent.enabled = this.m_activeInRound;
	}

	// Token: 0x06001FA4 RID: 8100 RVA: 0x0009A661 File Offset: 0x00098A61
	private void OnEnded()
	{
		this.m_targetComponent.enabled = this.m_activeOutOfRound;
	}

	// Token: 0x0400181B RID: 6171
	[SerializeField]
	private Behaviour m_targetComponent;

	// Token: 0x0400181C RID: 6172
	[SerializeField]
	private bool m_activeInRound = true;

	// Token: 0x0400181D RID: 6173
	[SerializeField]
	private bool m_activeOutOfRound;

	// Token: 0x0400181E RID: 6174
	private IFlowController m_iFlowController;
}
