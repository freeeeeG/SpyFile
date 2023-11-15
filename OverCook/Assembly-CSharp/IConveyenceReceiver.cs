using System;
using System.Collections;

// Token: 0x0200058C RID: 1420
public interface IConveyenceReceiver
{
	// Token: 0x06001AF1 RID: 6897
	void InformStartingConveyToMe();

	// Token: 0x06001AF2 RID: 6898
	IEnumerator ConveyToMe(ServerConveyorStation _priorConveyor, IAttachment _object);

	// Token: 0x06001AF3 RID: 6899
	void InformEndingConveyToMe();

	// Token: 0x06001AF4 RID: 6900
	bool IsReceiving();

	// Token: 0x06001AF5 RID: 6901
	bool CanConveyTo(IAttachment _itemToConvey);

	// Token: 0x06001AF6 RID: 6902
	void RegisterRefreshedConveyToCallback(CallbackVoid _callback);

	// Token: 0x06001AF7 RID: 6903
	void UnregisterRefreshedConveyToCallback(CallbackVoid _callback);

	// Token: 0x06001AF8 RID: 6904
	void RefreshConveyTo();

	// Token: 0x06001AF9 RID: 6905
	void RegisterAllowConveyToCallback(Generic<bool> _allowConveyCallback);

	// Token: 0x06001AFA RID: 6906
	void UnregisterAllowConveyToCallback(Generic<bool> _allowConveyCallback);
}
