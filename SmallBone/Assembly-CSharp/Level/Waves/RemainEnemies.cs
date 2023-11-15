using System;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000554 RID: 1364
	public sealed class RemainEnemies : Leaf
	{
		// Token: 0x06001AFF RID: 6911 RVA: 0x00054253 File Offset: 0x00052453
		protected override bool Check(EnemyWave wave)
		{
			return this._wave.remains <= this._remains;
		}

		// Token: 0x0400173E RID: 5950
		[SerializeField]
		private EnemyWave _wave;

		// Token: 0x0400173F RID: 5951
		[SerializeField]
		private int _remains;
	}
}
