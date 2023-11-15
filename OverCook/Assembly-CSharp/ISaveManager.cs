using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000723 RID: 1827
public interface ISaveManager
{
	// Token: 0x060022A4 RID: 8868
	MetaGameProgress GetMetaGameProgress();

	// Token: 0x060022A5 RID: 8869
	IEnumerator<SaveLoadResult?> LoadProfile(GamepadUser _user);

	// Token: 0x060022A6 RID: 8870
	IEnumerator<SaveLoadResult?> LoadSave(SaveMode _type, int _slot, int _dlcNumber);

	// Token: 0x060022A7 RID: 8871
	void UnloadProfile();

	// Token: 0x060022A8 RID: 8872
	void SaveMetaProgress(SaveSystemCallback _finished = null);

	// Token: 0x060022A9 RID: 8873
	IEnumerator SaveData(SaveMode _mode, int _slot, int _dlcNumber, SaveSystemCallback _finished = null);

	// Token: 0x060022AA RID: 8874
	IEnumerator HasMetaSaveFile(ReturnValue<SaveLoadResult> _result);

	// Token: 0x060022AB RID: 8875
	IEnumerator HasSaveFile(SaveMode _type, int _slot, int _dlcNumber, ReturnValue<SaveLoadResult> _result);

	// Token: 0x060022AC RID: 8876
	void DeleteSave(SaveMode _type, int _slot, int _dlcNumber, CallbackVoid _callback = null);

	// Token: 0x060022AD RID: 8877
	void DeleteMetaSave(CallbackVoid _callback = null);

	// Token: 0x060022AE RID: 8878
	void RegisterOnIdle(GenericVoid _callback);

	// Token: 0x060022AF RID: 8879
	void UnregisterOnIdle(GenericVoid _callback);

	// Token: 0x060022B0 RID: 8880
	void CreateMetaSession();

	// Token: 0x060022B1 RID: 8881
	void DestroyMetaSession();
}
