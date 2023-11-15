using System;
using UnityEngine;

// Token: 0x0200013C RID: 316
public interface ITriggerAnimation
{
	// Token: 0x060005B4 RID: 1460
	void OnNotifyAnimationTriggered(ITriggerAnimation _animator, AnimationClip _clip);

	// Token: 0x060005B5 RID: 1461
	void OnAnimationFinished(AnimationClip _clip);
}
