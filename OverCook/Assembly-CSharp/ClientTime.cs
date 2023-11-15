using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200083C RID: 2108
public class ClientTime
{
	// Token: 0x060028AB RID: 10411 RVA: 0x000BF25B File Offset: 0x000BD65B
	public void Initialise()
	{
		ClientTime.m_MultiplayerController = GameUtils.RequireManager<MultiplayerController>();
		Mailbox.Client.RegisterForMessageType(MessageType.TimeSync, new OrderedMessageReceivedCallback(this.OnTimeSyncReceived));
	}

	// Token: 0x060028AC RID: 10412 RVA: 0x000BF27F File Offset: 0x000BD67F
	public void Shutdown()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.TimeSync, new OrderedMessageReceivedCallback(this.OnTimeSyncReceived));
	}

	// Token: 0x060028AD RID: 10413 RVA: 0x000BF299 File Offset: 0x000BD699
	public static float Time()
	{
		return ClientTime.m_fLocalRunningTime + Mathf.Lerp(ClientTime.m_fOldOffset, ClientTime.m_fCurrentOffset, (UnityEngine.Time.realtimeSinceStartup - ClientTime.m_fLastReceivedTime) / 3f);
	}

	// Token: 0x060028AE RID: 10414 RVA: 0x000BF2C1 File Offset: 0x000BD6C1
	public static float DeltaTime()
	{
		return ClientTime.m_fDelta;
	}

	// Token: 0x060028AF RID: 10415 RVA: 0x000BF2C8 File Offset: 0x000BD6C8
	public static void Update()
	{
		float realtimeSinceStartup = UnityEngine.Time.realtimeSinceStartup;
		ClientTime.m_fDelta = realtimeSinceStartup - ClientTime.m_fLocalTimeLastFrame;
		ClientTime.m_fLocalRunningTime += ClientTime.m_fDelta;
		ClientTime.m_fLocalTimeLastFrame = realtimeSinceStartup;
	}

	// Token: 0x060028B0 RID: 10416 RVA: 0x000BF300 File Offset: 0x000BD700
	private void OnTimeSyncReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		TimeSyncMessage timeSyncMessage = (TimeSyncMessage)message;
		float fLatency = ClientTime.m_MultiplayerController.GetClientConnectionStats(false).m_fLatency;
		ClientTime.m_fOldOffset = ClientTime.m_fCurrentOffset;
		ClientTime.m_fCurrentOffset = timeSyncMessage.fTime - ClientTime.m_fLocalRunningTime + fLatency;
		ClientTime.m_fLastReceivedTime = UnityEngine.Time.realtimeSinceStartup;
	}

	// Token: 0x04002017 RID: 8215
	private static float m_fDelta;

	// Token: 0x04002018 RID: 8216
	private static float m_fLocalRunningTime;

	// Token: 0x04002019 RID: 8217
	private static float m_fLocalTimeLastFrame;

	// Token: 0x0400201A RID: 8218
	private static MultiplayerController m_MultiplayerController;

	// Token: 0x0400201B RID: 8219
	private static float m_fLastReceivedTime;

	// Token: 0x0400201C RID: 8220
	private static float m_fCurrentOffset;

	// Token: 0x0400201D RID: 8221
	private static float m_fOldOffset;
}
