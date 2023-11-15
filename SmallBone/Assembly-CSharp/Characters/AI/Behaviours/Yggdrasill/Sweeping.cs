using System;
using System.Collections;
using Characters.AI.YggdrasillElderEnt;
using UnityEngine;

namespace Characters.AI.Behaviours.Yggdrasill
{
	// Token: 0x020013D6 RID: 5078
	public sealed class Sweeping : Behaviour
	{
		// Token: 0x06006411 RID: 25617 RVA: 0x00122677 File Offset: 0x00120877
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
			this._sweepHandcontroller.Select();
			if (this._sweepHandcontroller.left)
			{
				yield return this._animationController.CPlayAndWaitAnimation(this._left);
			}
			else
			{
				yield return this._animationController.CPlayAndWaitAnimation(this._right);
			}
			if (this._invulnerable)
			{
				controller.character.cinematic.Detach(this);
			}
			if (this._statusImmune)
			{
				controller.character.status.unstoppable.Detach(this);
			}
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x040050AF RID: 20655
		[SerializeField]
		private YggdrasillAnimationController _animationController;

		// Token: 0x040050B0 RID: 20656
		[SerializeField]
		private SweepHandController _sweepHandcontroller;

		// Token: 0x040050B1 RID: 20657
		[SerializeField]
		private YggdrasillAnimation.Tag _left;

		// Token: 0x040050B2 RID: 20658
		[SerializeField]
		private YggdrasillAnimation.Tag _right;

		// Token: 0x040050B3 RID: 20659
		[SerializeField]
		private bool _invulnerable;

		// Token: 0x040050B4 RID: 20660
		[SerializeField]
		[Header("저지불가")]
		private bool _statusImmune;
	}
}
