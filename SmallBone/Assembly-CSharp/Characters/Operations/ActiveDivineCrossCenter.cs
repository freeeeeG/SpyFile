using System;
using Level.Traps;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DB9 RID: 3513
	public sealed class ActiveDivineCrossCenter : CharacterOperation
	{
		// Token: 0x060046B0 RID: 18096 RVA: 0x000CD29B File Offset: 0x000CB49B
		public override void Run(Character owner)
		{
			this._rotateLaser.Activate(owner);
		}

		// Token: 0x04003584 RID: 13700
		[SerializeField]
		private RotateLaser _rotateLaser;
	}
}
