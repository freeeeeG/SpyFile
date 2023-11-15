using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E42 RID: 3650
	public class SwapWeapon : CharacterOperation
	{
		// Token: 0x060048A2 RID: 18594 RVA: 0x000D380B File Offset: 0x000D1A0B
		public override void Run(Character owner)
		{
			owner.playerComponents.inventory.weapon.NextWeapon(this._force);
		}

		// Token: 0x040037B7 RID: 14263
		[SerializeField]
		private bool _force = true;
	}
}
