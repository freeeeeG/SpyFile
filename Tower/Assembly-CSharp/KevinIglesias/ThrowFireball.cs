using System;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000CA RID: 202
	public class ThrowFireball : StateMachineBehaviour
	{
		// Token: 0x060002F1 RID: 753 RVA: 0x0000D148 File Offset: 0x0000B348
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.cS == null)
			{
				this.cS = animator.GetComponent<CastSpells>();
			}
			if (this.cS != null)
			{
				this.cS.ThrowFireball(this.castHand, this.spawnDelay);
			}
		}

		// Token: 0x0400027E RID: 638
		private CastSpells cS;

		// Token: 0x0400027F RID: 639
		public CastHand castHand;

		// Token: 0x04000280 RID: 640
		public float spawnDelay;
	}
}
