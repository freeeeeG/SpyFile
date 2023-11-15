using System;
using Characters.Actions.Constraints;

// Token: 0x0200001D RID: 29
public static class ConstraintExtension
{
	// Token: 0x06000063 RID: 99 RVA: 0x00003914 File Offset: 0x00001B14
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

	// Token: 0x06000064 RID: 100 RVA: 0x0000393C File Offset: 0x00001B3C
	public static void Consume(this Constraint[] constraints)
	{
		for (int i = 0; i < constraints.Length; i++)
		{
			constraints[i].Consume();
		}
	}
}
