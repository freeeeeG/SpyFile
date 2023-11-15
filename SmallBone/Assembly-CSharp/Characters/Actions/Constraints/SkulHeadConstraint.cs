using System;

namespace Characters.Actions.Constraints
{
	// Token: 0x02000985 RID: 2437
	public class SkulHeadConstraint : Constraint
	{
		// Token: 0x06003446 RID: 13382 RVA: 0x0009ABE9 File Offset: 0x00098DE9
		public override bool Pass()
		{
			return SkulHeadToTeleport.instance != null && SkulHeadToTeleport.instance.gameObject.activeSelf;
		}
	}
}
