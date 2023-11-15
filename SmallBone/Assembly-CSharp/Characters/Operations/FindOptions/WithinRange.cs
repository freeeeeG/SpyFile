using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000EAC RID: 3756
	[Serializable]
	public class WithinRange : IScope
	{
		// Token: 0x060049F0 RID: 18928 RVA: 0x000D7D2E File Offset: 0x000D5F2E
		public List<Character> GetEnemyList()
		{
			WithinRange._overlapper.contactFilter.SetLayerMask(this._layerMask);
			return WithinRange._overlapper.OverlapCollider(this._range).GetComponents<Character>(true);
		}

		// Token: 0x04003929 RID: 14633
		[SerializeField]
		private Collider2D _range;

		// Token: 0x0400392A RID: 14634
		[SerializeField]
		private LayerMask _layerMask;

		// Token: 0x0400392B RID: 14635
		private static NonAllocOverlapper _overlapper = new NonAllocOverlapper(31);
	}
}
