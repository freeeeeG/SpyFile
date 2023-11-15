using System;
using UnityEngine;

// Token: 0x020008F6 RID: 2294
public interface Lerp
{
	// Token: 0x06002CAD RID: 11437
	void StartSynchronising(Component synchronisedObject);

	// Token: 0x06002CAE RID: 11438
	void Reset();

	// Token: 0x06002CAF RID: 11439
	void Reparented();

	// Token: 0x06002CB0 RID: 11440
	void ReceiveServerUpdate(Vector3 localPosition, Quaternion localRotation);

	// Token: 0x06002CB1 RID: 11441
	void ReceiveServerEvent(Vector3 localPosition, Quaternion localRotation);

	// Token: 0x06002CB2 RID: 11442
	void UpdateLerp(float delta);
}
