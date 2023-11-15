using System;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x0200054E RID: 1358
	public class WaveState : Leaf
	{
		// Token: 0x06001AF6 RID: 6902 RVA: 0x0005419E File Offset: 0x0005239E
		protected override bool Check(EnemyWave wave)
		{
			return this._target.state == this._state;
		}

		// Token: 0x04001734 RID: 5940
		[SerializeField]
		private EnemyWave _target;

		// Token: 0x04001735 RID: 5941
		[SerializeField]
		private Wave.State _state;
	}
}
