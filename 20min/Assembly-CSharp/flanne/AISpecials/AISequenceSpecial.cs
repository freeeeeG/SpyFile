using System;
using System.Collections;
using UnityEngine;

namespace flanne.AISpecials
{
	// Token: 0x02000266 RID: 614
	[CreateAssetMenu(fileName = "AISequenceSpecial", menuName = "AISpecials/AISequenceSpecial")]
	public class AISequenceSpecial : AISpecial
	{
		// Token: 0x06000D3E RID: 3390 RVA: 0x00030345 File Offset: 0x0002E545
		public override void Use(AIComponent ai, Transform target)
		{
			ai.StartCoroutine(this.UseSpecialsCR(ai, target));
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00030356 File Offset: 0x0002E556
		private IEnumerator UseSpecialsCR(AIComponent ai, Transform target)
		{
			int num;
			for (int i = 0; i < this.specialSequence.Length; i = num + 1)
			{
				this.specialSequence[i].Use(ai, target);
				yield return new WaitForSeconds(this.delayBetweenSpecials);
				num = i;
			}
			yield break;
		}

		// Token: 0x0400099F RID: 2463
		[SerializeField]
		private AISpecial[] specialSequence;

		// Token: 0x040009A0 RID: 2464
		[SerializeField]
		private float delayBetweenSpecials;
	}
}
