using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000F1 RID: 241
	public class PeriodicallyBurnArea : MonoBehaviour
	{
		// Token: 0x060006F6 RID: 1782 RVA: 0x0001EB09 File Offset: 0x0001CD09
		private void Start()
		{
			this.burnSys = BurnSystem.SharedInstance;
			this._layer = 1 << TagLayerUtil.Enemy;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001EB2C File Offset: 0x0001CD2C
		private void Update()
		{
			this._timer += Time.deltaTime;
			if (this._timer > this.timePerTick)
			{
				this._timer -= this.timePerTick;
				int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, this.rangeRadius, this._colliders, this._layer);
				for (int i = 0; i < num; i++)
				{
					this.burnSys.Burn(this._colliders[i].gameObject, this.burnDamage);
				}
			}
		}

		// Token: 0x040004BC RID: 1212
		[SerializeField]
		private float timePerTick;

		// Token: 0x040004BD RID: 1213
		[SerializeField]
		private int burnDamage;

		// Token: 0x040004BE RID: 1214
		[SerializeField]
		private float rangeRadius = 1f;

		// Token: 0x040004BF RID: 1215
		private BurnSystem burnSys;

		// Token: 0x040004C0 RID: 1216
		private Collider2D[] _colliders = new Collider2D[2];

		// Token: 0x040004C1 RID: 1217
		private int _layer;

		// Token: 0x040004C2 RID: 1218
		private float _timer;
	}
}
