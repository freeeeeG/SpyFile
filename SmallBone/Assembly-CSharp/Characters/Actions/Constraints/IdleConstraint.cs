using System;

namespace Characters.Actions.Constraints
{
	// Token: 0x0200097E RID: 2430
	public class IdleConstraint : Constraint
	{
		// Token: 0x06003432 RID: 13362 RVA: 0x0009A901 File Offset: 0x00098B01
		public override bool Pass()
		{
			return this._action.owner.runningMotion == null;
		}
	}
}
