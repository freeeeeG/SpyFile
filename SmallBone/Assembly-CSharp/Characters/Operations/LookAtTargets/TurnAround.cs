using System;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000F08 RID: 3848
	public sealed class TurnAround : Target
	{
		// Token: 0x06004B36 RID: 19254 RVA: 0x000DD659 File Offset: 0x000DB859
		public override Character.LookingDirection GetDirectionFrom(Character character)
		{
			if (character.lookingDirection != Character.LookingDirection.Right)
			{
				return Character.LookingDirection.Right;
			}
			return Character.LookingDirection.Left;
		}
	}
}
