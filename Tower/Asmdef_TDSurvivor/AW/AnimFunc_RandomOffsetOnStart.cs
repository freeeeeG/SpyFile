using System;
using UnityEngine;

namespace AW
{
	// Token: 0x020001C5 RID: 453
	public class AnimFunc_RandomOffsetOnStart : StateMachineBehaviour
	{
		// Token: 0x06000BE9 RID: 3049 RVA: 0x0002EB76 File Offset: 0x0002CD76
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.isFirstEnter)
			{
				animator.Play(stateInfo.fullPathHash, layerIndex, Random.Range(0f, 1f));
				this.isFirstEnter = false;
			}
		}

		// Token: 0x0400097F RID: 2431
		private bool isFirstEnter = true;
	}
}
