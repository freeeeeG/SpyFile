using System;
using UnityEngine;

// Token: 0x020008B8 RID: 2232
public interface IPolluter
{
	// Token: 0x0600408D RID: 16525
	int GetRadius();

	// Token: 0x0600408E RID: 16526
	int GetNoise();

	// Token: 0x0600408F RID: 16527
	GameObject GetGameObject();

	// Token: 0x06004090 RID: 16528
	void SetAttributes(Vector2 pos, int dB, GameObject go, string name = null);

	// Token: 0x06004091 RID: 16529
	string GetName();

	// Token: 0x06004092 RID: 16530
	Vector2 GetPosition();

	// Token: 0x06004093 RID: 16531
	void Clear();

	// Token: 0x06004094 RID: 16532
	void SetSplat(NoiseSplat splat);
}
