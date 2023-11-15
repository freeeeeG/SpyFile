using System;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class AnimFunc_SyncOnStart : StateMachineBehaviour
{
	// Token: 0x06000AED RID: 2797 RVA: 0x000290EF File Offset: 0x000272EF
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		bool flag = this.isFirstEnter;
	}

	// Token: 0x04000855 RID: 2133
	private bool isFirstEnter = true;
}
