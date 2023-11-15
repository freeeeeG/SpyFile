using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200007F RID: 127
	[CreateAssetMenu(fileName = "BossDamageModifier", menuName = "DifficultyMods/BossDamageModifier")]
	public class BossDamageModifier : DifficultyModifier
	{
		// Token: 0x06000516 RID: 1302 RVA: 0x00019451 File Offset: 0x00017651
		public override void ModifyBossSpawner(BossSpawner bossSpawner)
		{
			bossSpawner.enemyDamage += this.additionalDamage;
		}

		// Token: 0x0400030B RID: 779
		[SerializeField]
		private int additionalDamage;
	}
}
