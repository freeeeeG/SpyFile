using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A01 RID: 2561
public class PlayerAttachmentCarrier : MonoBehaviour, IParentable
{
	// Token: 0x0600322A RID: 12842 RVA: 0x000EB7CE File Offset: 0x000E9BCE
	private void Awake()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x0600322B RID: 12843 RVA: 0x000EB7E8 File Offset: 0x000E9BE8
	protected void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x0600322C RID: 12844 RVA: 0x000EB804 File Offset: 0x000E9C04
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.m_attachPoints[0] = base.transform.FindChildRecursive("Attachment").transform;
			this.m_attachPoints[1] = base.transform.FindChildRecursive("Attachment_Backpack").transform;
		}
	}

	// Token: 0x0600322D RID: 12845 RVA: 0x000EB860 File Offset: 0x000E9C60
	public Transform GetAttachPoint(GameObject gameObject)
	{
		if (gameObject != null)
		{
			IHandleAttachTarget handleAttachTarget = gameObject.RequestInterface<IHandleAttachTarget>();
			if (handleAttachTarget as MonoBehaviour != null)
			{
				return this.m_attachPoints[(int)handleAttachTarget.PlayerAttachTarget];
			}
		}
		return this.m_attachPoints[0];
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x000EB8A7 File Offset: 0x000E9CA7
	public Transform GetAttachPoint(PlayerAttachTarget playerAttachTarget)
	{
		return this.m_attachPoints[(int)playerAttachTarget];
	}

	// Token: 0x0600322F RID: 12847 RVA: 0x000EB8B1 File Offset: 0x000E9CB1
	public bool HasClientSidePrediction()
	{
		return false;
	}

	// Token: 0x04002867 RID: 10343
	private Transform[] m_attachPoints = new Transform[2];
}
