using System;
using PhysicsUtils;
using UnityEngine;

namespace Level.ReactiveProps
{
	// Token: 0x0200056B RID: 1387
	public class EnterZoneFly : ReactiveProp
	{
		// Token: 0x06001B3F RID: 6975 RVA: 0x00054A9C File Offset: 0x00052C9C
		static EnterZoneFly()
		{
			EnterZoneFly._playerOverlapper.contactFilter.SetLayerMask(512);
			EnterZoneFly._enemyOverlapper = new NonAllocOverlapper(31);
			EnterZoneFly._enemyOverlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x00054AF3 File Offset: 0x00052CF3
		private void Update()
		{
			if (!this.CheckWithinSight() || this._flying)
			{
				return;
			}
			base.Activate();
			UnityEngine.Object.Destroy(this._collider);
			this._collider = null;
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x00054B20 File Offset: 0x00052D20
		private bool CheckWithinSight()
		{
			if (this._collider == null)
			{
				return false;
			}
			NonAllocOverlapper nonAllocOverlapper = EnterZoneFly._playerOverlapper.OverlapCollider(this._collider);
			NonAllocOverlapper nonAllocOverlapper2 = EnterZoneFly._enemyOverlapper.OverlapCollider(this._collider);
			return nonAllocOverlapper.results.Count > 0 || nonAllocOverlapper2.results.Count > 0;
		}

		// Token: 0x04001768 RID: 5992
		[SerializeField]
		[GetComponent]
		private Collider2D _collider;

		// Token: 0x04001769 RID: 5993
		private static readonly NonAllocOverlapper _playerOverlapper = new NonAllocOverlapper(15);

		// Token: 0x0400176A RID: 5994
		protected static readonly NonAllocOverlapper _enemyOverlapper;
	}
}
