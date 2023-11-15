using System;
using UnityEngine;

namespace Level
{
	// Token: 0x0200047C RID: 1148
	public class EntSapling : MonoBehaviour
	{
		// Token: 0x060015E3 RID: 5603 RVA: 0x00044B92 File Offset: 0x00042D92
		public void RunIntro(bool activate)
		{
			this._introInvoker.SetActive(activate);
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x00044BA0 File Offset: 0x00042DA0
		public PoolObject Spawn(Vector3 position, bool intro)
		{
			PoolObject poolObject = this._pool.Spawn(position, Quaternion.identity, true);
			poolObject.GetComponent<EntSapling>().RunIntro(intro);
			return poolObject;
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x00044BC0 File Offset: 0x00042DC0
		public void Despawn()
		{
			this._introInvoker.SetActive(false);
			this._pool.Despawn();
		}

		// Token: 0x04001326 RID: 4902
		[SerializeField]
		private PoolObject _pool;

		// Token: 0x04001327 RID: 4903
		[SerializeField]
		private GameObject _introInvoker;
	}
}
