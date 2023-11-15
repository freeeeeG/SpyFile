using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200008A RID: 138
	[CreateAssetMenu(fileName = "SmallEnemyHPModifier", menuName = "DifficultyMods/SmallEnemyHPModifier")]
	public class SmallEnemyHPModifier : DifficultyModifier
	{
		// Token: 0x06000530 RID: 1328 RVA: 0x000195BD File Offset: 0x000177BD
		public override void ModifyHordeSpawner(HordeSpawner hordeSpawner)
		{
			hordeSpawner.healthMultiplier += this.additionalHPPercent;
		}

		// Token: 0x0400031A RID: 794
		[Range(0f, 1f)]
		[SerializeField]
		private float additionalHPPercent;
	}
}
