using System;

namespace Characters.Operations
{
	// Token: 0x02000E1E RID: 3614
	public class InverseLookingDirection : CharacterOperation
	{
		// Token: 0x06004823 RID: 18467 RVA: 0x000D1DD6 File Offset: 0x000CFFD6
		public override void Run(Character owner)
		{
			owner.ForceToLookAt((owner.lookingDirection == Character.LookingDirection.Right) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
		}
	}
}
