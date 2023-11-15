using System;
using System.Collections.Generic;
using System.Linq;
using Level;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002B9 RID: 697
	public class SpawnRandomWave : Runnable
	{
		// Token: 0x06000E2F RID: 3631 RVA: 0x0002CD00 File Offset: 0x0002AF00
		public override void Run()
		{
			IEnumerable<EnemyWave> enumerable = from wave in Map.Instance.waveContainer.enemyWaves
			where wave.state == Wave.State.Waiting
			select wave;
			if (enumerable.Count<EnemyWave>() == 0)
			{
				return;
			}
			enumerable.Random<EnemyWave>().Spawn(this._effectOnSpawned);
		}

		// Token: 0x04000BCD RID: 3021
		[SerializeField]
		private bool _effectOnSpawned;
	}
}
