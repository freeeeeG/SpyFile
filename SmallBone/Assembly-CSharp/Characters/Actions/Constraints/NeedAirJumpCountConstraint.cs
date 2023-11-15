using System;

namespace Characters.Actions.Constraints
{
	// Token: 0x02000983 RID: 2435
	public class NeedAirJumpCountConstraint : Constraint
	{
		// Token: 0x06003440 RID: 13376 RVA: 0x0009AB54 File Offset: 0x00098D54
		public override bool Pass()
		{
			return !this._action.owner.movement.controller.isGrounded && this._action.owner.movement.currentAirJumpCount < this._action.owner.movement.airJumpCount.total;
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x0009ABB0 File Offset: 0x00098DB0
		public override void Consume()
		{
			this._action.owner.movement.currentAirJumpCount++;
		}
	}
}
