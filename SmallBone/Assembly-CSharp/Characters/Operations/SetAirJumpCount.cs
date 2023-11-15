using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DFE RID: 3582
	public class SetAirJumpCount : CharacterOperation
	{
		// Token: 0x060047A5 RID: 18341 RVA: 0x000D0445 File Offset: 0x000CE645
		public override void Run(Character target)
		{
			target.movement.currentAirJumpCount = this._currentAirJumpCount;
		}

		// Token: 0x040036A7 RID: 13991
		[SerializeField]
		private int _currentAirJumpCount;
	}
}
