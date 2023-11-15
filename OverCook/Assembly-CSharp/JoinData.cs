using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200086A RID: 2154
public class JoinData
{
	// Token: 0x040020D5 RID: 8405
	public User.MachineID machine;

	// Token: 0x040020D6 RID: 8406
	public UsersChangedMessage usersChanged = new UsersChangedMessage();

	// Token: 0x040020D7 RID: 8407
	public TimeSyncMessage timeSync = new TimeSyncMessage();

	// Token: 0x040020D8 RID: 8408
	public GameSetupMessage gameSetup = new GameSetupMessage();
}
