using System;
using Level;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000371 RID: 881
	public class WaveOnSpawn : Trigger
	{
		// Token: 0x0600103D RID: 4157 RVA: 0x0003046D File Offset: 0x0002E66D
		private void Start()
		{
			this._wave.onSpawn += delegate()
			{
				this._result = true;
			};
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x00030486 File Offset: 0x0002E686
		protected override bool Check()
		{
			return this._result;
		}

		// Token: 0x04000D47 RID: 3399
		[SerializeField]
		private EnemyWave _wave;

		// Token: 0x04000D48 RID: 3400
		private bool _result;
	}
}
