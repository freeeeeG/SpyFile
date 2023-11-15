using System;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E58 RID: 3672
	public class Jump : CharacterOperation
	{
		// Token: 0x060048F1 RID: 18673 RVA: 0x000D4C0F File Offset: 0x000D2E0F
		public override void Run(Character owner)
		{
			owner.movement.Jump(this._jumpHeight);
		}

		// Token: 0x0400380F RID: 14351
		[SerializeField]
		private float _jumpHeight = 3f;
	}
}
