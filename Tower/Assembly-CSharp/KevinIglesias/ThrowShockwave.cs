using System;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000CD RID: 205
	public class ThrowShockwave : StateMachineBehaviour
	{
		// Token: 0x060002F7 RID: 759 RVA: 0x0000D234 File Offset: 0x0000B434
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.cS == null)
			{
				this.cS = animator.GetComponent<CastSpells>();
			}
			if (this.cS != null)
			{
				this.cS.ThrowShockwave(this.castHand, this.spawnDelay);
			}
		}

		// Token: 0x04000286 RID: 646
		private CastSpells cS;

		// Token: 0x04000287 RID: 647
		public CastHand castHand;

		// Token: 0x04000288 RID: 648
		public float spawnDelay;
	}
}
