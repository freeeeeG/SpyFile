using System;
using UnityEngine;

// Token: 0x020009ED RID: 2541
public interface ICarrier : ICarrierPlacement
{
	// Token: 0x0600319A RID: 12698
	void RegisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback);

	// Token: 0x0600319B RID: 12699
	void UnregisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback);

	// Token: 0x0600319C RID: 12700
	void CarryItem(GameObject _object);

	// Token: 0x0600319D RID: 12701
	GameObject AccessGameObject();
}
