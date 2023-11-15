using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200008C RID: 140
	[CreateAssetMenu(fileName = "SmallEnemySpeedModifier", menuName = "DifficultyMods/SmallEnemySpeedModifier")]
	public class SmallEnemySpeedModifier : DifficultyModifier
	{
		// Token: 0x06000534 RID: 1332 RVA: 0x000195E7 File Offset: 0x000177E7
		public override void ModifyHordeSpawner(HordeSpawner hordeSpawner)
		{
			hordeSpawner.speedMultiplier += this.additionalSpeedPercent;
		}

		// Token: 0x0400031C RID: 796
		[Range(0f, 1f)]
		[SerializeField]
		private float additionalSpeedPercent;
	}
}
