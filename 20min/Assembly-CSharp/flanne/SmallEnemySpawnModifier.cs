using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200008B RID: 139
	[CreateAssetMenu(fileName = "SmallEnemySpawnModifier", menuName = "DifficultyMods/SmallEnemySpawnModifier")]
	public class SmallEnemySpawnModifier : DifficultyModifier
	{
		// Token: 0x06000532 RID: 1330 RVA: 0x000195D2 File Offset: 0x000177D2
		public override void ModifyHordeSpawner(HordeSpawner hordeSpawner)
		{
			hordeSpawner.spawnRateMulitplier += this.additionalSpawnRatePercent;
		}

		// Token: 0x0400031B RID: 795
		[Range(0f, 1f)]
		[SerializeField]
		private float additionalSpawnRatePercent;
	}
}
