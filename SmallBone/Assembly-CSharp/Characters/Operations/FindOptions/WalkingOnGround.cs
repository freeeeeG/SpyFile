using System;
using Characters.Movements;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000EA2 RID: 3746
	[Serializable]
	public class WalkingOnGround : ICondition
	{
		// Token: 0x060049DF RID: 18911 RVA: 0x000D7AEA File Offset: 0x000D5CEA
		public bool Satisfied(Character character)
		{
			return character.movement.config.type == Movement.Config.Type.Walking;
		}
	}
}
