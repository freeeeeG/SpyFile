using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000087 RID: 135
	[CreateAssetMenu(fileName = "EnemyDamageModifierAfterTime", menuName = "DifficultyMods/EnemyDamageModifierAfterTime")]
	public class EnemyDamageModifierAfterTime : DifficultyModifier
	{
		// Token: 0x06000529 RID: 1321 RVA: 0x00019564 File Offset: 0x00017764
		public override void ModifyHordeSpawner(HordeSpawner hordeSpawner)
		{
			hordeSpawner.StartCoroutine(this.WaitToIncreaseDamage(hordeSpawner));
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00019574 File Offset: 0x00017774
		private IEnumerator WaitToIncreaseDamage(HordeSpawner hordeSpawner)
		{
			yield return null;
			float timeLimit = GameTimer.SharedInstance.timeLimit;
			yield return new WaitForSeconds(timeLimit - this.timeRemainingToActivate);
			hordeSpawner.enemyDamage += this.additionalDamage;
			yield break;
		}

		// Token: 0x04000316 RID: 790
		[SerializeField]
		private int additionalDamage;

		// Token: 0x04000317 RID: 791
		[SerializeField]
		private float timeRemainingToActivate;
	}
}
