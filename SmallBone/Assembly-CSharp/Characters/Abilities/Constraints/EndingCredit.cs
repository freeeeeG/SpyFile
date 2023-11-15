using System;
using Scenes;

namespace Characters.Abilities.Constraints
{
	// Token: 0x02000C2D RID: 3117
	public class EndingCredit : Constraint
	{
		// Token: 0x0600400E RID: 16398 RVA: 0x000BA095 File Offset: 0x000B8295
		public override bool Pass()
		{
			return !Scene<GameBase>.instance.uiManager.endingCredit.gameObject.activeInHierarchy;
		}
	}
}
