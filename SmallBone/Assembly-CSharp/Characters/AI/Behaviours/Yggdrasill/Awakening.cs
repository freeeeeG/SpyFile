using System;
using System.Collections;
using Characters.AI.YggdrasillElderEnt;
using UnityEngine;

namespace Characters.AI.Behaviours.Yggdrasill
{
	// Token: 0x020013D2 RID: 5074
	public sealed class Awakening : Behaviour
	{
		// Token: 0x06006401 RID: 25601 RVA: 0x001223BC File Offset: 0x001205BC
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (this._tags == null || this._tags.Length == 0)
			{
				Debug.LogError("tag list error in PlayAnimations Behaviour");
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			controller.character.cinematic.Attach(this);
			controller.character.status.RemoveAllStatus();
			foreach (YggdrasillAnimation.Tag tag in this._tags)
			{
				yield return this._controller.CPlayAndWaitAnimation(tag);
			}
			YggdrasillAnimation.Tag[] array = null;
			controller.character.cinematic.Detach(this);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x0400509D RID: 20637
		[SerializeField]
		private YggdrasillAnimationController _controller;

		// Token: 0x0400509E RID: 20638
		[SerializeField]
		private YggdrasillAnimation.Tag[] _tags;
	}
}
