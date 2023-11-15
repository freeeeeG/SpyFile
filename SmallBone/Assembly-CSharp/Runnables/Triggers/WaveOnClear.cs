using System;
using Level;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000370 RID: 880
	public class WaveOnClear : Trigger
	{
		// Token: 0x06001039 RID: 4153 RVA: 0x00030443 File Offset: 0x0002E643
		private void Start()
		{
			this._wave.onClear += delegate()
			{
				this._result = true;
			};
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0003045C File Offset: 0x0002E65C
		protected override bool Check()
		{
			return this._result;
		}

		// Token: 0x04000D45 RID: 3397
		[SerializeField]
		private EnemyWave _wave;

		// Token: 0x04000D46 RID: 3398
		private bool _result;
	}
}
