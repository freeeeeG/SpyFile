using System;
using Scenes;

namespace Characters.Abilities.Constraints
{
	// Token: 0x02000C2F RID: 3119
	public class Story : Constraint
	{
		// Token: 0x06004012 RID: 16402 RVA: 0x000BA0CC File Offset: 0x000B82CC
		public override bool Pass()
		{
			return !Scene<GameBase>.instance.uiManager.narration.sceneVisible;
		}
	}
}
