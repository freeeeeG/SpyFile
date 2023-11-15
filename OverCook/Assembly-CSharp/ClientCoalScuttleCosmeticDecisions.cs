using System;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200039B RID: 923
public class ClientCoalScuttleCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x0600115E RID: 4446 RVA: 0x000639EC File Offset: 0x00061DEC
	public void OnPickupItem()
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_07_Coal_Collect, base.gameObject.layer);
	}
}
