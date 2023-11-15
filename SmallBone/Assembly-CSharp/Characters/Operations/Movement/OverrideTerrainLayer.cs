using System;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E66 RID: 3686
	public class OverrideTerrainLayer : CharacterOperation
	{
		// Token: 0x0600492A RID: 18730 RVA: 0x000D570B File Offset: 0x000D390B
		public override void Run(Character owner)
		{
			this._owner = owner;
			this._cached = this._owner.movement.controller.terrainMask;
			this._owner.movement.controller.terrainMask = this._terrainLayer;
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x000D574A File Offset: 0x000D394A
		private void Pop()
		{
			if (this._owner == null)
			{
				return;
			}
			this._owner.movement.controller.terrainMask = this._cached;
		}

		// Token: 0x0600492C RID: 18732 RVA: 0x000D5776 File Offset: 0x000D3976
		public override void Stop()
		{
			base.Stop();
			this.Pop();
		}

		// Token: 0x0400384B RID: 14411
		[SerializeField]
		private LayerMask _terrainLayer;

		// Token: 0x0400384C RID: 14412
		private LayerMask _cached;

		// Token: 0x0400384D RID: 14413
		private Character _owner;
	}
}
