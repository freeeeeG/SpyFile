using System;
using Level;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000332 RID: 818
	public class SpawnWave : Runnable
	{
		// Token: 0x06000F98 RID: 3992 RVA: 0x0002F48F File Offset: 0x0002D68F
		public override void Run()
		{
			this._wave.Spawn(this._showEffect);
		}

		// Token: 0x04000CD4 RID: 3284
		[SerializeField]
		private EnemyWave _wave;

		// Token: 0x04000CD5 RID: 3285
		[SerializeField]
		private bool _showEffect;
	}
}
