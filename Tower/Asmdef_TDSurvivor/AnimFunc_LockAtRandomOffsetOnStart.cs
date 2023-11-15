using System;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class AnimFunc_LockAtRandomOffsetOnStart : StateMachineBehaviour
{
	// Token: 0x06000AEB RID: 2795 RVA: 0x000290A7 File Offset: 0x000272A7
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.isFirstEnter)
		{
			animator.Play(stateInfo.fullPathHash, layerIndex, Random.Range(0f, 1f));
			this.isFirstEnter = false;
			animator.speed = 0f;
		}
	}

	// Token: 0x04000854 RID: 2132
	private bool isFirstEnter = true;
}
