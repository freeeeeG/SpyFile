using System;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E71 RID: 3697
	public class StopMove : CharacterOperation
	{
		// Token: 0x06004956 RID: 18774 RVA: 0x000D61F5 File Offset: 0x000D43F5
		public override void Run(Character owner)
		{
			owner.movement.StopAllCoroutines();
		}
	}
}
