using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E4C RID: 3660
	public class UpdateAnimation : CharacterOperation
	{
		// Token: 0x060048C3 RID: 18627 RVA: 0x000D4380 File Offset: 0x000D2580
		protected override void OnDestroy()
		{
			this._origin = null;
			this._clip = null;
			this._characterAnimation = null;
			base.OnDestroy();
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x000D43A0 File Offset: 0x000D25A0
		public override void Run(Character owner)
		{
			if (this._characterAnimation == null)
			{
				this._characterAnimation = owner.animationController.animations[0];
			}
			this.SetAnimation(this._clip);
			if (this._duration > 0f)
			{
				base.StartCoroutine(this.CExpire(owner.chronometer.master));
			}
		}

		// Token: 0x060048C5 RID: 18629 RVA: 0x000D4403 File Offset: 0x000D2603
		public override void Stop()
		{
			if (this._origin != null)
			{
				this.SetAnimation(this._origin);
			}
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x000D4420 File Offset: 0x000D2620
		private void SetAnimation(AnimationClip clip)
		{
			if (clip == null)
			{
				return;
			}
			UpdateAnimation.State state = this._state;
			if (state == UpdateAnimation.State.Idle)
			{
				this._characterAnimation.SetIdle(clip);
				return;
			}
			if (state != UpdateAnimation.State.Walk)
			{
				return;
			}
			this._characterAnimation.SetWalk(clip);
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x000D445F File Offset: 0x000D265F
		private IEnumerator CExpire(Chronometer chronometer)
		{
			yield return chronometer.WaitForSeconds(this._duration);
			this.Stop();
			yield break;
		}

		// Token: 0x040037CF RID: 14287
		[SerializeField]
		private UpdateAnimation.State _state;

		// Token: 0x040037D0 RID: 14288
		[SerializeField]
		private AnimationClip _origin;

		// Token: 0x040037D1 RID: 14289
		[SerializeField]
		private AnimationClip _clip;

		// Token: 0x040037D2 RID: 14290
		[SerializeField]
		private CharacterAnimation _characterAnimation;

		// Token: 0x040037D3 RID: 14291
		[SerializeField]
		private float _duration;

		// Token: 0x02000E4D RID: 3661
		private enum State
		{
			// Token: 0x040037D5 RID: 14293
			Idle,
			// Token: 0x040037D6 RID: 14294
			Walk
		}
	}
}
