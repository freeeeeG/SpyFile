using System;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Usables
{
	// Token: 0x02000751 RID: 1873
	public sealed class EffectZoneSizeByProjectileCount : MonoBehaviour
	{
		// Token: 0x0600261E RID: 9758 RVA: 0x00073148 File Offset: 0x00071348
		private void OnEnable()
		{
			int num = 0;
			foreach (Projectile projectile in this._projectilesToCount)
			{
				num += projectile.reusable.spawnedCount;
			}
			int num2 = Mathf.Clamp(num, 0, this._sizeRangeByProjectileCount.Length - 1);
			this._effectZone.sizeRange = this._sizeRangeByProjectileCount[num2];
		}

		// Token: 0x040020C9 RID: 8393
		[SerializeField]
		private EffectZone _effectZone;

		// Token: 0x040020CA RID: 8394
		[SerializeField]
		private Projectile[] _projectilesToCount;

		// Token: 0x040020CB RID: 8395
		[SerializeField]
		private Vector2[] _sizeRangeByProjectileCount;
	}
}
