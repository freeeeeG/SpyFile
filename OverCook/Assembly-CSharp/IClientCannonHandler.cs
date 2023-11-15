using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200044D RID: 1101
public interface IClientCannonHandler
{
	// Token: 0x06001452 RID: 5202
	void Load(GameObject _obj);

	// Token: 0x06001453 RID: 5203
	IEnumerator ExitCannonRoutine(GameObject _obj, Vector3 _exitPosition, Quaternion _exitRotation);

	// Token: 0x06001454 RID: 5204
	bool CanHandle(GameObject _obj);

	// Token: 0x06001455 RID: 5205
	void Launch(GameObject _obj);

	// Token: 0x06001456 RID: 5206
	void Land(GameObject _obj);
}
