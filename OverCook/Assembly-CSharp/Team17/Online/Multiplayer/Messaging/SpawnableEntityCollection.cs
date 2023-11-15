using System;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000905 RID: 2309
	public class SpawnableEntityCollection : MonoBehaviour, INetworkEntitySpawner
	{
		// Token: 0x06002D1B RID: 11547 RVA: 0x000D4E98 File Offset: 0x000D3298
		public void RegisterSpawnable(GameObject _prefab, VoidGeneric<GameObject> _spawnCallback)
		{
			if (!this.m_spawnables.Contains(_prefab))
			{
				this.m_spawnables.Add(_prefab);
				List<VoidGeneric<GameObject>> list = new List<VoidGeneric<GameObject>>();
				list.Add(_spawnCallback);
				this.m_callbacks.Add(this.GetSpawnableID(_prefab), list);
			}
			else
			{
				List<VoidGeneric<GameObject>> list2 = this.m_callbacks[this.GetSpawnableID(_prefab)];
				if (!list2.Contains(_spawnCallback))
				{
					list2.Add(_spawnCallback);
				}
			}
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x000D4F10 File Offset: 0x000D3310
		public GameObject SpawnEntity(int _id, Vector3 _position, Quaternion _rotation, ref List<VoidGeneric<GameObject>> callbacks)
		{
			if (this.m_spawnables != null && this.m_spawnables.Count > 0)
			{
				GameObject source = this.m_spawnables[_id];
				GameObject gameObject = source.Instantiate(_position, _rotation);
				gameObject.transform.SetParent(null, true);
				callbacks = this.m_callbacks[_id];
				return gameObject;
			}
			return null;
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000D4F70 File Offset: 0x000D3370
		public int GetSpawnableID(GameObject _object)
		{
			int result = -1;
			if (this.m_spawnables != null && this.m_spawnables.Count > 0)
			{
				result = this.m_spawnables.IndexOf(_object);
			}
			return result;
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000D4FA9 File Offset: 0x000D33A9
		public GameObject AccessGameObject()
		{
			return base.gameObject;
		}

		// Token: 0x04002436 RID: 9270
		[SerializeField]
		[ReadOnly]
		private List<GameObject> m_spawnables = new List<GameObject>();

		// Token: 0x04002437 RID: 9271
		private Dictionary<int, List<VoidGeneric<GameObject>>> m_callbacks = new Dictionary<int, List<VoidGeneric<GameObject>>>();
	}
}
