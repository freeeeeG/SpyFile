using System;
using System.Collections;
using UnityEngine;

namespace flanne.AISpecials
{
	// Token: 0x02000269 RID: 617
	[CreateAssetMenu(fileName = "AISpawnSpecial", menuName = "AISpecials/AISpawnSpecial")]
	public class AISpawnSpecial : AISpecial
	{
		// Token: 0x06000D47 RID: 3399 RVA: 0x00030414 File Offset: 0x0002E614
		public override void Use(AIComponent ai, Transform target)
		{
			Spawner componentInChildren = ai.GetComponentInChildren<Spawner>();
			ai.StartCoroutine(this.WaitToSpawn(this.numSpawns, componentInChildren));
			Animator animator = ai.animator;
			if (animator == null)
			{
				return;
			}
			animator.SetTrigger("Special");
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00030451 File Offset: 0x0002E651
		private IEnumerator WaitToSpawn(int numSpawns, Spawner spawner)
		{
			int num;
			for (int i = 0; i < numSpawns; i = num + 1)
			{
				yield return new WaitForSeconds(this.spawnWinduptime);
				SoundEffectSO soundEffectSO = this.soundFX;
				if (soundEffectSO != null)
				{
					soundEffectSO.Play(null);
				}
				if (spawner != null)
				{
					spawner.Spawn();
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x040009AA RID: 2474
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x040009AB RID: 2475
		[SerializeField]
		private float spawnWinduptime;

		// Token: 0x040009AC RID: 2476
		[SerializeField]
		private int numSpawns;
	}
}
