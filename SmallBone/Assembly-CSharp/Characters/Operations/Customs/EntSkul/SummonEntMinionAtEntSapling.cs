using System;
using Level;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Customs.EntSkul
{
	// Token: 0x02001018 RID: 4120
	public class SummonEntMinionAtEntSapling : CharacterOperation
	{
		// Token: 0x06004F8B RID: 20363 RVA: 0x000EF5FD File Offset: 0x000ED7FD
		static SummonEntMinionAtEntSapling()
		{
			SummonEntMinionAtEntSapling._overlapper.contactFilter.SetLayerMask(512);
		}

		// Token: 0x06004F8C RID: 20364 RVA: 0x000EF624 File Offset: 0x000ED824
		public override void Run(Character owner)
		{
			if (owner.playerComponents == null)
			{
				return;
			}
			foreach (SaplingTarget saplingTarget in SummonEntMinionAtEntSapling._overlapper.OverlapCollider(this._range).GetComponents<SaplingTarget>(true))
			{
				if (!saplingTarget.spawnable)
				{
					break;
				}
				saplingTarget.SummonEntMinion(owner, this._lifeTime);
			}
		}

		// Token: 0x04003FC1 RID: 16321
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04003FC2 RID: 16322
		[SerializeField]
		private float _lifeTime;

		// Token: 0x04003FC3 RID: 16323
		private static readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(32);
	}
}
