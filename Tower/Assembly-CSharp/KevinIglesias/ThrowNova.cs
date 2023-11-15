using System;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000CC RID: 204
	public class ThrowNova : StateMachineBehaviour
	{
		// Token: 0x060002F5 RID: 757 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.cS == null)
			{
				this.cS = animator.GetComponent<CastSpells>();
			}
			if (this.cS != null)
			{
				this.cS.ThrowNova(this.spawnDelay);
			}
		}

		// Token: 0x04000284 RID: 644
		private CastSpells cS;

		// Token: 0x04000285 RID: 645
		public float spawnDelay;
	}
}
