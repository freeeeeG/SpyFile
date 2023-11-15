using System;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000904 RID: 2308
	public interface INetworkEntitySpawner
	{
		// Token: 0x06002D17 RID: 11543
		GameObject SpawnEntity(int _id, Vector3 _position, Quaternion _rotation, ref List<VoidGeneric<GameObject>> callbacks);

		// Token: 0x06002D18 RID: 11544
		int GetSpawnableID(GameObject _object);

		// Token: 0x06002D19 RID: 11545
		GameObject AccessGameObject();
	}
}
