using System;
using System.Collections;

// Token: 0x020004D9 RID: 1241
public interface IRespawnBehaviour
{
	// Token: 0x06001712 RID: 5906
	IEnumerator RespawnCoroutine(ServerRespawnCollider _collider);
}
