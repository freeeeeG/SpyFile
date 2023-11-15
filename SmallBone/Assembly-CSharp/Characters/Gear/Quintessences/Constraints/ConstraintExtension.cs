using System;

namespace Characters.Gear.Quintessences.Constraints
{
	// Token: 0x020008F6 RID: 2294
	public static class ConstraintExtension
	{
		// Token: 0x060030FE RID: 12542 RVA: 0x0009275C File Offset: 0x0009095C
		public static bool Pass(this Constraint[] constraints)
		{
			for (int i = 0; i < constraints.Length; i++)
			{
				if (!constraints[i].Pass())
				{
					return false;
				}
			}
			return true;
		}
	}
}
