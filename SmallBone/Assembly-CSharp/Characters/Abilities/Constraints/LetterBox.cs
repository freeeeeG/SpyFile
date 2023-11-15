using System;
using Scenes;

namespace Characters.Abilities.Constraints
{
	// Token: 0x02000C2E RID: 3118
	public class LetterBox : Constraint
	{
		// Token: 0x06004010 RID: 16400 RVA: 0x000BA0B3 File Offset: 0x000B82B3
		public override bool Pass()
		{
			return !Scene<GameBase>.instance.uiManager.letterBox.visible;
		}
	}
}
