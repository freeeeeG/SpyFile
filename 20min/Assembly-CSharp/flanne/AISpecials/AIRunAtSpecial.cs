using System;
using System.Collections;
using UnityEngine;

namespace flanne.AISpecials
{
	// Token: 0x02000265 RID: 613
	[CreateAssetMenu(fileName = "AIRunAtSpecial", menuName = "AISpecials/AIRunAtSpecial")]
	public class AIRunAtSpecial : AISpecial
	{
		// Token: 0x06000D3B RID: 3387 RVA: 0x000302F0 File Offset: 0x0002E4F0
		public override void Use(AIComponent ai, Transform target)
		{
			Vector2 direction = target.position - ai.transform.position;
			ai.StartCoroutine(this.RunAtCR(ai, direction));
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00030328 File Offset: 0x0002E528
		private IEnumerator RunAtCR(AIComponent ai, Vector2 direction)
		{
			Animator animator = ai.animator;
			if (animator != null)
			{
				animator.SetTrigger("Windup");
			}
			SoundEffectSO soundEffectSO = this.windupSFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			yield return new WaitForSeconds(this.windupTime);
			Animator animator2 = ai.animator;
			if (animator2 != null)
			{
				animator2.SetTrigger("Special");
			}
			ai.moveComponent.vector = direction.normalized * this.runSpeed;
			SoundEffectSO soundEffectSO2 = this.runningSFX;
			if (soundEffectSO2 != null)
			{
				soundEffectSO2.Play(null);
			}
			yield break;
		}

		// Token: 0x0400099B RID: 2459
		[SerializeField]
		private float runSpeed;

		// Token: 0x0400099C RID: 2460
		[SerializeField]
		private float windupTime;

		// Token: 0x0400099D RID: 2461
		[SerializeField]
		private SoundEffectSO runningSFX;

		// Token: 0x0400099E RID: 2462
		[SerializeField]
		private SoundEffectSO windupSFX;
	}
}
