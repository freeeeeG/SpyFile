using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E36 RID: 3638
	public class SpawnGold : Operation
	{
		// Token: 0x06004883 RID: 18563 RVA: 0x000D2D90 File Offset: 0x000D0F90
		public override void Run()
		{
			Vector3 position = (this._point == null) ? base.transform.position : this._point.position;
			Singleton<Service>.Instance.levelManager.DropGold(this._gold, this._count, position);
		}

		// Token: 0x04003796 RID: 14230
		[SerializeField]
		private Transform _point;

		// Token: 0x04003797 RID: 14231
		[SerializeField]
		private int _gold;

		// Token: 0x04003798 RID: 14232
		[SerializeField]
		private int _count;
	}
}
