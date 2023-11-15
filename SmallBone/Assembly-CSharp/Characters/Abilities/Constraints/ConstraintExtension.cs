using System;

namespace Characters.Abilities.Constraints
{
	// Token: 0x02000C2B RID: 3115
	public static class ConstraintExtension
	{
		// Token: 0x0600400B RID: 16395 RVA: 0x000BA04C File Offset: 0x000B824C
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
