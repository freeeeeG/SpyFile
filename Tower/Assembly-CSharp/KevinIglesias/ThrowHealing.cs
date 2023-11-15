using System;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000CB RID: 203
	public class ThrowHealing : StateMachineBehaviour
	{
		// Token: 0x060002F3 RID: 755 RVA: 0x0000D19C File Offset: 0x0000B39C
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.cS == null)
			{
				this.cS = animator.GetComponent<CastSpells>();
			}
			if (this.cS != null)
			{
				this.cS.ThrowHealing(this.castHand, this.spawnDelay);
			}
		}

		// Token: 0x04000281 RID: 641
		private CastSpells cS;

		// Token: 0x04000282 RID: 642
		public CastHand castHand;

		// Token: 0x04000283 RID: 643
		public float spawnDelay;
	}
}
