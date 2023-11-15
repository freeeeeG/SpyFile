using System;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x0200020E RID: 526
	public class PlayAnimation : Event
	{
		// Token: 0x06000A89 RID: 2697 RVA: 0x0001CD76 File Offset: 0x0001AF76
		private void OnDestroy()
		{
			this._animator = null;
			this._animation = null;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0001CD86 File Offset: 0x0001AF86
		public override void Run()
		{
			this._animator.Play(this._animation.name);
		}

		// Token: 0x0400089F RID: 2207
		[SerializeField]
		private Animator _animator;

		// Token: 0x040008A0 RID: 2208
		[SerializeField]
		private AnimationClip _animation;
	}
}
