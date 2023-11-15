using System;
using System.Collections;
using UnityEngine;

namespace flanne.AISpecials
{
	// Token: 0x02000262 RID: 610
	[CreateAssetMenu(fileName = "AILaserSpecial", menuName = "AISpecials/AILaserSpecial")]
	public class AILasersSpecial : AISpecial
	{
		// Token: 0x06000D32 RID: 3378 RVA: 0x00030174 File Offset: 0x0002E374
		public override void Use(AIComponent ai, Transform target)
		{
			GameObject windupObj = null;
			GameObject laserObj = null;
			for (int i = 0; i < ai.transform.childCount; i++)
			{
				Transform child = ai.transform.GetChild(i);
				if (child.tag == this.windupTag)
				{
					windupObj = child.gameObject;
				}
				if (child.tag == this.laserTag)
				{
					laserObj = child.gameObject;
				}
			}
			ai.StartCoroutine(this.LaserCR(windupObj, laserObj, ai));
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x000301EC File Offset: 0x0002E3EC
		private IEnumerator LaserCR(GameObject windupObj, GameObject laserObj, AIComponent ai)
		{
			windupObj.SetActive(true);
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
			laserObj.SetActive(true);
			Animator animator2 = ai.animator;
			if (animator2 != null)
			{
				animator2.SetTrigger("Special");
			}
			SoundEffectSO soundEffectSO2 = this.laserSFX;
			if (soundEffectSO2 != null)
			{
				soundEffectSO2.Play(null);
			}
			yield break;
		}

		// Token: 0x04000990 RID: 2448
		[SerializeField]
		private float windupTime;

		// Token: 0x04000991 RID: 2449
		[SerializeField]
		private string windupTag;

		// Token: 0x04000992 RID: 2450
		[SerializeField]
		private string laserTag;

		// Token: 0x04000993 RID: 2451
		[SerializeField]
		private SoundEffectSO laserSFX;

		// Token: 0x04000994 RID: 2452
		[SerializeField]
		private SoundEffectSO windupSFX;
	}
}
