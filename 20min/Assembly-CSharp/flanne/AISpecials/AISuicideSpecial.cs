using System;
using System.Collections;
using UnityEngine;

namespace flanne.AISpecials
{
	// Token: 0x0200026A RID: 618
	[CreateAssetMenu(fileName = "AISuicideSpecial", menuName = "AISpecials/AISuicideSpecial")]
	public class AISuicideSpecial : AISpecial
	{
		// Token: 0x06000D4A RID: 3402 RVA: 0x00030470 File Offset: 0x0002E670
		public override void Use(AIComponent ai, Transform target)
		{
			Health component = ai.GetComponent<Health>();
			if (component != null)
			{
				ai.StartCoroutine(this.WaitToSuicide(component));
				SoundEffectSO soundEffectSO = this.soundFX;
				if (soundEffectSO != null)
				{
					soundEffectSO.Play(null);
				}
				FlashSprite component2 = ai.GetComponent<FlashSprite>();
				if (component2 != null)
				{
					ai.StartCoroutine(this.FlashWarning(component2));
					return;
				}
			}
			else
			{
				Debug.LogWarning("AI is missing health component: " + ai.name);
			}
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x000304E2 File Offset: 0x0002E6E2
		private IEnumerator FlashWarning(FlashSprite flasher)
		{
			float flashTime = this.timeToActivate - 0.2f;
			for (float timer = 0f; timer < flashTime; timer += 0.1f)
			{
				flasher.Flash();
				yield return new WaitForSeconds(0.2f);
			}
			yield break;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x000304F8 File Offset: 0x0002E6F8
		private IEnumerator WaitToSuicide(Health health)
		{
			yield return new WaitForSeconds(this.timeToActivate);
			health.AutoKill(true);
			yield break;
		}

		// Token: 0x040009AD RID: 2477
		[SerializeField]
		private float timeToActivate;

		// Token: 0x040009AE RID: 2478
		[SerializeField]
		private SoundEffectSO soundFX;
	}
}
