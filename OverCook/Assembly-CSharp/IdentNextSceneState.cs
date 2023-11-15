using System;
using UnityEngine;

// Token: 0x020007E4 RID: 2020
public class IdentNextSceneState : StateMachineBehaviour
{
	// Token: 0x060026E0 RID: 9952 RVA: 0x000B8928 File Offset: 0x000B6D28
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		IdentScreenFlow identScreenFlow = UnityEngine.Object.FindObjectOfType<IdentScreenFlow>();
		identScreenFlow.ActivateNextScene();
	}
}
