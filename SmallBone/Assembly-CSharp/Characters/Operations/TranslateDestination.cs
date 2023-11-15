using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E0E RID: 3598
	public class TranslateDestination : CharacterOperation
	{
		// Token: 0x060047DC RID: 18396 RVA: 0x000D1325 File Offset: 0x000CF525
		public override void Run(Character owner)
		{
			this._target.position = this._destination.position;
		}

		// Token: 0x04003701 RID: 14081
		[SerializeField]
		private Transform _target;

		// Token: 0x04003702 RID: 14082
		[SerializeField]
		private Transform _destination;
	}
}
