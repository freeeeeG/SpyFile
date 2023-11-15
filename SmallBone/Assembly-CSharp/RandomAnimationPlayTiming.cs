using System;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class RandomAnimationPlayTiming : MonoBehaviour
{
	// Token: 0x0600020E RID: 526 RVA: 0x00009059 File Offset: 0x00007259
	private void Awake()
	{
		this._animator.Play(this._animation.name, 0, UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x0600020F RID: 527 RVA: 0x00009081 File Offset: 0x00007281
	private void OnDestroy()
	{
		this._animator = null;
		this._animation = null;
	}

	// Token: 0x040001C4 RID: 452
	[SerializeField]
	private Animator _animator;

	// Token: 0x040001C5 RID: 453
	[SerializeField]
	private AnimationClip _animation;
}
