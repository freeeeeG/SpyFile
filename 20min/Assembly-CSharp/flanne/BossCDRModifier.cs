using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200007E RID: 126
	[CreateAssetMenu(fileName = "BossCDRModifier", menuName = "DifficultyMods/BossCDRModifier")]
	public class BossCDRModifier : DifficultyModifier
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x00019434 File Offset: 0x00017634
		public override void ModifyBossSpawner(BossSpawner bossSpawner)
		{
			bossSpawner.cooldownRate += this.cooldownReduction;
		}

		// Token: 0x0400030A RID: 778
		[Range(0f, 1f)]
		[SerializeField]
		private float cooldownReduction;
	}
}
