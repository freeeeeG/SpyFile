using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000080 RID: 128
	[CreateAssetMenu(fileName = "BossHPModifier", menuName = "DifficultyMods/BossHPModifier")]
	public class BossHPModifier : DifficultyModifier
	{
		// Token: 0x06000518 RID: 1304 RVA: 0x00019466 File Offset: 0x00017666
		public override void ModifyBossSpawner(BossSpawner bossSpawner)
		{
			bossSpawner.healthMultiplier += this.additionalHPPercent;
		}

		// Token: 0x0400030C RID: 780
		[Range(0f, 1f)]
		[SerializeField]
		private float additionalHPPercent;
	}
}
