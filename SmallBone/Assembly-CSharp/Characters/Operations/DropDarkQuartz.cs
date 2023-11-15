using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E10 RID: 3600
	public class DropDarkQuartz : CharacterOperation
	{
		// Token: 0x060047E0 RID: 18400 RVA: 0x000D1555 File Offset: 0x000CF755
		public override void Run(Character owner)
		{
			Singleton<Service>.Instance.levelManager.DropDarkQuartz((int)this._amountRange.value, this._count, this._dropPosition.position);
		}

		// Token: 0x04003707 RID: 14087
		[SerializeField]
		private Transform _dropPosition;

		// Token: 0x04003708 RID: 14088
		[SerializeField]
		private int _count;

		// Token: 0x04003709 RID: 14089
		[SerializeField]
		private CustomFloat _amountRange = new CustomFloat(0f);
	}
}
