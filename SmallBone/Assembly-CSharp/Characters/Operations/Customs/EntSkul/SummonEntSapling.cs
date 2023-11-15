using System;
using Level;
using UnityEngine;

namespace Characters.Operations.Customs.EntSkul
{
	// Token: 0x02001019 RID: 4121
	public class SummonEntSapling : CharacterOperation
	{
		// Token: 0x06004F8E RID: 20366 RVA: 0x000EF6A4 File Offset: 0x000ED8A4
		public override void Run(Character owner)
		{
			if (owner.playerComponents == null)
			{
				return;
			}
			Vector3 position = owner.transform.position;
			this._ent.Spawn(position, this._intro);
		}

		// Token: 0x04003FC4 RID: 16324
		[SerializeField]
		private bool _intro = true;

		// Token: 0x04003FC5 RID: 16325
		[SerializeField]
		private EntSapling _ent;

		// Token: 0x04003FC6 RID: 16326
		[SerializeField]
		private LayerMask _terrainLayer;

		// Token: 0x04003FC7 RID: 16327
		[SerializeField]
		private int _preloadCount = 5;

		// Token: 0x04003FC8 RID: 16328
		private const float _groundFindingRayDistance = 9f;
	}
}
