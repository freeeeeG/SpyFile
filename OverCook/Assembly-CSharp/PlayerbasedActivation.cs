using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000764 RID: 1892
public class PlayerbasedActivation : MonoBehaviour
{
	// Token: 0x06002469 RID: 9321 RVA: 0x000ACE16 File Offset: 0x000AB216
	private void Awake()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x000ACE30 File Offset: 0x000AB230
	private void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x0600246B RID: 9323 RVA: 0x000ACE4C File Offset: 0x000AB24C
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.UpdateActiveState();
		}
	}

	// Token: 0x0600246C RID: 9324 RVA: 0x000ACE74 File Offset: 0x000AB274
	private void UpdateActiveState()
	{
		bool flag = this.IsActiveForPlayerCount(ClientUserSystem.m_Users.Count);
		if (base.gameObject.activeSelf != flag)
		{
			base.gameObject.SetActive(flag);
		}
	}

	// Token: 0x0600246D RID: 9325 RVA: 0x000ACEB0 File Offset: 0x000AB2B0
	private bool IsActiveForPlayerCount(int _count)
	{
		int num = _count - 1;
		return num >= 0 && (this.m_activePlayerCount & 1 << num) > 0;
	}

	// Token: 0x04001BD8 RID: 7128
	[SerializeField]
	[Mask(typeof(PlayerbasedActivation.PlayerCount))]
	public int m_activePlayerCount = -1;

	// Token: 0x02000765 RID: 1893
	public enum PlayerCount
	{
		// Token: 0x04001BDA RID: 7130
		One,
		// Token: 0x04001BDB RID: 7131
		Two,
		// Token: 0x04001BDC RID: 7132
		Three,
		// Token: 0x04001BDD RID: 7133
		Four
	}
}
