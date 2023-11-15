using System;
using Level;

namespace Runnables.Triggers
{
	// Token: 0x02000368 RID: 872
	public class StoppedEnemyContainer : Trigger
	{
		// Token: 0x0600101B RID: 4123 RVA: 0x00030058 File Offset: 0x0002E258
		protected override bool Check()
		{
			EnemyWaveContainer waveContainer = Map.Instance.waveContainer;
			return waveContainer.enemyWaves.Length == 0 || waveContainer.state != EnemyWaveContainer.State.Remain;
		}
	}
}
