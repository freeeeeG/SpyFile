using System;
using UnityEngine;

// Token: 0x020009F3 RID: 2547
public interface IThrower
{
	// Token: 0x060031B4 RID: 12724
	void ThrowItem(GameObject _object, Vector2 _directionXZ);

	// Token: 0x060031B5 RID: 12725
	void OnFailedToThrowItem(GameObject _object);

	// Token: 0x060031B6 RID: 12726
	void RegisterThrowCallback(GenericVoid<GameObject> _callback);

	// Token: 0x060031B7 RID: 12727
	void UnregisterThrowCallback(GenericVoid<GameObject> _callback);
}
