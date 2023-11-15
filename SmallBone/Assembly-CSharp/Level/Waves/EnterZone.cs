using System;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000550 RID: 1360
	public sealed class EnterZone : Leaf
	{
		// Token: 0x06001AF9 RID: 6905 RVA: 0x000541BB File Offset: 0x000523BB
		static EnterZone()
		{
			EnterZone._overlapper.contactFilter.SetLayerMask(512);
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x000541E1 File Offset: 0x000523E1
		protected override bool Check(EnemyWave wave)
		{
			return EnterZone._overlapper.OverlapCollider(this._zone).GetComponent<Target>() != null;
		}

		// Token: 0x04001736 RID: 5942
		[SerializeField]
		private Collider2D _zone;

		// Token: 0x04001737 RID: 5943
		private static readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);
	}
}
