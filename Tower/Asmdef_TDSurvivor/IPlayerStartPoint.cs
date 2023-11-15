using System;
using UnityEngine;

// Token: 0x0200009E RID: 158
public interface IPlayerStartPoint
{
	// Token: 0x0600033E RID: 830
	void RegisterToMapManager();

	// Token: 0x0600033F RID: 831
	void UnregisterToMapManager();

	// Token: 0x06000340 RID: 832
	Vector3 GetPosition();

	// Token: 0x06000341 RID: 833
	GameObject GetGameObject();
}
