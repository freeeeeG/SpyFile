using System;
using System.Collections;
using UnityEngine;

namespace flanne.AISpecials
{
	// Token: 0x02000267 RID: 615
	[CreateAssetMenu(fileName = "AIShootSpecial", menuName = "AISpecials/AIShootSpecial")]
	public class AIShootSpecial : AISpecial
	{
		// Token: 0x06000D41 RID: 3393 RVA: 0x00030374 File Offset: 0x0002E574
		public override void Use(AIComponent ai, Transform target)
		{
			Vector3 direction = target.position - ai.specialPoint.position;
			ai.StartCoroutine(this.ShootCR(direction, ai.specialPoint.transform));
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x000303B1 File Offset: 0x0002E5B1
		private IEnumerator ShootCR(Vector3 direction, Transform spawn)
		{
			int num;
			for (int i = 0; i < this.numRepeated; i = num + 1)
			{
				GameObject pooledObject = ObjectPooler.SharedInstance.GetPooledObject(this.projectileOPTag);
				pooledObject.SetActive(true);
				pooledObject.transform.position = spawn.position;
				pooledObject.GetComponent<MoveComponent2D>().vector = this.projectileSpeed * direction.normalized;
				SoundEffectSO soundEffectSO = this.soundFX;
				if (soundEffectSO != null)
				{
					soundEffectSO.Play(null);
				}
				yield return new WaitForSeconds(this.delayBetweenShots);
				num = i;
			}
			yield break;
		}

		// Token: 0x040009A1 RID: 2465
		[SerializeField]
		private string projectileOPTag;

		// Token: 0x040009A2 RID: 2466
		[SerializeField]
		private float projectileSpeed;

		// Token: 0x040009A3 RID: 2467
		[SerializeField]
		private int numRepeated = 1;

		// Token: 0x040009A4 RID: 2468
		[SerializeField]
		private float delayBetweenShots;

		// Token: 0x040009A5 RID: 2469
		[SerializeField]
		private SoundEffectSO soundFX;
	}
}
