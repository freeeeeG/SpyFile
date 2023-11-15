using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E11 RID: 3601
	public class DropGold : CharacterOperation
	{
		// Token: 0x060047E2 RID: 18402 RVA: 0x000D159B File Offset: 0x000CF79B
		public override void Run(Character owner)
		{
			Singleton<Service>.Instance.levelManager.DropGold(this._amount, this._count, this._dropPosition.position);
		}

		// Token: 0x0400370A RID: 14090
		[SerializeField]
		private Transform _dropPosition;

		// Token: 0x0400370B RID: 14091
		[SerializeField]
		private int _amount;

		// Token: 0x0400370C RID: 14092
		[SerializeField]
		private int _count;
	}
}
