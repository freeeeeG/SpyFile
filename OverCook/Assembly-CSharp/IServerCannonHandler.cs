using System;
using UnityEngine;

// Token: 0x0200044C RID: 1100
public interface IServerCannonHandler
{
	// Token: 0x0600144E RID: 5198
	void Load(GameObject _obj);

	// Token: 0x0600144F RID: 5199
	void Unload(GameObject _obj);

	// Token: 0x06001450 RID: 5200
	void ExitCannonRoutine(GameObject _obj);

	// Token: 0x06001451 RID: 5201
	bool CanHandle(GameObject _obj);
}
