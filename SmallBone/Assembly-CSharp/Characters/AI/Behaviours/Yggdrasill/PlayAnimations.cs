using System;
using System.Collections;
using Characters.AI.YggdrasillElderEnt;
using UnityEngine;

namespace Characters.AI.Behaviours.Yggdrasill
{
	// Token: 0x020013D4 RID: 5076
	public sealed class PlayAnimations : Behaviour
	{
		// Token: 0x06006409 RID: 25609 RVA: 0x001224F9 File Offset: 0x001206F9
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (this._invulnerable)
			{
				controller.character.cinematic.Attach(this);
			}
			if (this._statusImmune)
			{
				controller.character.status.unstoppable.Attach(this);
			}
			if (this._tags == null || this._tags.Length == 0)
			{
				Debug.LogError("tag list error in PlayAnimations Behaviour");
				base.result = Behaviour.Result.Fail;
				yield break;
			}
			foreach (YggdrasillAnimation.Tag tag in this._tags)
			{
				yield return this._controller.CPlayAndWaitAnimation(tag);
			}
			YggdrasillAnimation.Tag[] array = null;
			if (this._invulnerable)
			{
				controller.character.cinematic.Detach(this);
			}
			if (this._statusImmune)
			{
				controller.character.status.unstoppable.Detach(this);
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x040050A5 RID: 20645
		[SerializeField]
		private YggdrasillAnimationController _controller;

		// Token: 0x040050A6 RID: 20646
		[SerializeField]
		private YggdrasillAnimation.Tag[] _tags;

		// Token: 0x040050A7 RID: 20647
		[SerializeField]
		private bool _invulnerable;

		// Token: 0x040050A8 RID: 20648
		[SerializeField]
		[Header("저지불가")]
		private bool _statusImmune;
	}
}
