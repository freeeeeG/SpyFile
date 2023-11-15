using System;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000C7 RID: 199
	public class AddCastEffect : StateMachineBehaviour
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x0000CF96 File Offset: 0x0000B196
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.cS == null)
			{
				this.cS = animator.GetComponent<CastSpells>();
			}
			if (this.cS != null)
			{
				this.cS.SpawnEffect(this.castHand);
			}
		}

		// Token: 0x04000271 RID: 625
		private CastSpells cS;

		// Token: 0x04000272 RID: 626
		public CastHand castHand;
	}
}
