using System;
using System.Runtime.CompilerServices;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200088D RID: 2189
public static class ClientGameSetup
{
	// Token: 0x06002A8D RID: 10893 RVA: 0x000C7512 File Offset: 0x000C5912
	public static void Initialise()
	{
		Mailbox client = Mailbox.Client;
		MessageType type = MessageType.GameSetup;
		if (ClientGameSetup.<>f__mg$cache0 == null)
		{
			ClientGameSetup.<>f__mg$cache0 = new OrderedMessageReceivedCallback(ClientGameSetup.OnGameSetupMessage);
		}
		client.RegisterForMessageType(type, ClientGameSetup.<>f__mg$cache0);
	}

	// Token: 0x06002A8E RID: 10894 RVA: 0x000C753D File Offset: 0x000C593D
	public static void Shutdown()
	{
		Mailbox client = Mailbox.Client;
		MessageType type = MessageType.GameSetup;
		if (ClientGameSetup.<>f__mg$cache1 == null)
		{
			ClientGameSetup.<>f__mg$cache1 = new OrderedMessageReceivedCallback(ClientGameSetup.OnGameSetupMessage);
		}
		client.UnregisterForMessageType(type, ClientGameSetup.<>f__mg$cache1);
	}

	// Token: 0x06002A8F RID: 10895 RVA: 0x000C7568 File Offset: 0x000C5968
	public static void OnGameSetupMessage(IOnlineMultiplayerSessionUserId sessionUser, Serialisable message)
	{
		ClientGameSetup.Mode = ((GameSetupMessage)message).m_Mode;
		RichPresenceManager.SetGameMode(ClientGameSetup.Mode);
	}

	// Token: 0x04002187 RID: 8583
	public static GameMode Mode = GameMode.COUNT;

	// Token: 0x04002188 RID: 8584
	public static string PrevScene = string.Empty;

	// Token: 0x04002189 RID: 8585
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cache0;

	// Token: 0x0400218A RID: 8586
	[CompilerGenerated]
	private static OrderedMessageReceivedCallback <>f__mg$cache1;
}
