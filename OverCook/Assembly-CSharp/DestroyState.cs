using System;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class DestroyState : StateMachineBehaviour
{
	// Token: 0x060004DE RID: 1246 RVA: 0x00028955 File Offset: 0x00026D55
	public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		_animator.gameObject.Destroy();
	}
}
