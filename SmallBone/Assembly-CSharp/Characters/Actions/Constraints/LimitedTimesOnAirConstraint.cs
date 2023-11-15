using System;
using Characters.Movements;
using Services;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x0200097F RID: 2431
	public class LimitedTimesOnAirConstraint : Constraint
	{
		// Token: 0x06003434 RID: 13364 RVA: 0x0009A91C File Offset: 0x00098B1C
		public override bool Pass()
		{
			return this._action.owner.movement.controller.isGrounded || (!this._action.owner.movement.controller.isGrounded && this._remain > 0);
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x0009A96E File Offset: 0x00098B6E
		public override void Consume()
		{
			if (!this._action.owner.movement.controller.isGrounded)
			{
				this._remain--;
			}
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x0009A99C File Offset: 0x00098B9C
		public override void Initilaize(Action action)
		{
			base.Initilaize(action);
			this._remain = this._maxTimes;
			this._action.owner.movement.onGrounded += this.OnGrounded;
			if (this._resetOnAirJump)
			{
				this._action.owner.movement.onJump += this.OnJump;
			}
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x0009AA08 File Offset: 0x00098C08
		protected override void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			if (this._action.owner == null)
			{
				return;
			}
			this._action.owner.movement.onGrounded -= this.OnGrounded;
			if (this._resetOnAirJump)
			{
				this._action.owner.movement.onJump -= this.OnJump;
			}
			this._action = null;
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x0009AA82 File Offset: 0x00098C82
		private void OnGrounded()
		{
			this._remain = this._maxTimes;
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x0009AA82 File Offset: 0x00098C82
		private void OnJump(Movement.JumpType jumpType, float jumpHeight)
		{
			this._remain = this._maxTimes;
		}

		// Token: 0x04002A46 RID: 10822
		[SerializeField]
		private int _maxTimes;

		// Token: 0x04002A47 RID: 10823
		[SerializeField]
		private bool _resetOnAirJump;

		// Token: 0x04002A48 RID: 10824
		private int _remain;
	}
}
