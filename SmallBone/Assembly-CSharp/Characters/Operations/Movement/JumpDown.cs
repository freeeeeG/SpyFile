using System;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E59 RID: 3673
	public class JumpDown : CharacterOperation
	{
		// Token: 0x060048F3 RID: 18675 RVA: 0x000D4C35 File Offset: 0x000D2E35
		public override void Run(Character owner)
		{
			owner.movement.JumpDown();
		}
	}
}
