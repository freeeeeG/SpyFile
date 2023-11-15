using System;
using System.Collections;
using UnityEngine;

namespace flanne.AISpecials
{
	// Token: 0x02000264 RID: 612
	[CreateAssetMenu(fileName = "AIRandomAreaProjectileSpecial", menuName = "AISpecials/AIRandomAreaProjectileSpecial")]
	public class AIRandomAreaProjectileSpecial : AISpecial
	{
		// Token: 0x06000D37 RID: 3383 RVA: 0x0003022E File Offset: 0x0002E42E
		public override void Use(AIComponent ai, Transform target)
		{
			ai.StartCoroutine(this.ShootCR(ai.specialPoint.transform, target, ai));
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0003024A File Offset: 0x0002E44A
		private void InitProjectile(Vector2 spawnPos, Vector2 hitPos, float duration)
		{
			EnemyAreaProjectile component = Object.Instantiate<GameObject>(this.projectilePrefab.gameObject).GetComponent<EnemyAreaProjectile>();
			component.transform.position = spawnPos;
			component.TargetPos(hitPos, duration);
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00030279 File Offset: 0x0002E479
		private IEnumerator ShootCR(Transform spawn, Transform target, AIComponent ai)
		{
			Animator animator = ai.animator;
			if (animator != null)
			{
				animator.SetTrigger("Windup");
			}
			yield return new WaitForSeconds(this.windupTime);
			Animator animator2 = ai.animator;
			if (animator2 != null)
			{
				animator2.SetTrigger("Special");
			}
			Vector3 b = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0f);
			this.InitProjectile(spawn.position, target.position + b, 1f);
			for (int i = 0; i < this.numProjectiles; i++)
			{
				Vector2 normalized = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
				Vector2 hitPos = ai.transform.position + normalized * Random.Range(this.range.x, this.range.y);
				float duration = Random.Range(this.airTime.x, this.airTime.y);
				this.InitProjectile(spawn.transform.position, hitPos, duration);
			}
			yield return new WaitForSeconds(0.5f);
			yield break;
		}

		// Token: 0x04000996 RID: 2454
		[SerializeField]
		private EnemyAreaProjectile projectilePrefab;

		// Token: 0x04000997 RID: 2455
		[SerializeField]
		private float windupTime = 0.2f;

		// Token: 0x04000998 RID: 2456
		[SerializeField]
		private int numProjectiles = 1;

		// Token: 0x04000999 RID: 2457
		[SerializeField]
		private Vector2 range = new Vector2(50f, 50f);

		// Token: 0x0400099A RID: 2458
		[SerializeField]
		private Vector2 airTime = new Vector2(0f, 2f);
	}
}
