using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002A3 RID: 675
public class WaveInfoSetter : MonoBehaviour
{
	// Token: 0x06001082 RID: 4226 RVA: 0x0002D6F0 File Offset: 0x0002B8F0
	public void SetWaveInfo(List<EnemySequence> sequences, EnemyType nextBoss, int nextBossWave)
	{
		this.waveHolder.SetWaveInfo(sequences);
		this.bossHolder.SetBossInfo(nextBoss, nextBossWave);
	}

	// Token: 0x040008D3 RID: 2259
	[SerializeField]
	private WaveInfoHolder waveHolder;

	// Token: 0x040008D4 RID: 2260
	[SerializeField]
	private BossInfoHolder bossHolder;
}
