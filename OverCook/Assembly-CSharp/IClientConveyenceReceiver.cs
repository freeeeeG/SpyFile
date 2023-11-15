using System;

// Token: 0x0200058D RID: 1421
public interface IClientConveyenceReceiver
{
	// Token: 0x06001AFB RID: 6907
	void InformStartingConveyToMe();

	// Token: 0x06001AFC RID: 6908
	void InformEndingConveyToMe();

	// Token: 0x06001AFD RID: 6909
	bool IsReceiving();

	// Token: 0x06001AFE RID: 6910
	void RegisterRefreshedConveyToCallback(CallbackVoid _callback);

	// Token: 0x06001AFF RID: 6911
	void UnregisterRefreshedConveyToCallback(CallbackVoid _callback);
}
