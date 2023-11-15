using System;
using Scenes;

namespace Characters.Abilities.Constraints
{
	// Token: 0x02000C2C RID: 3116
	public class Dialogue : Constraint
	{
		// Token: 0x0600400C RID: 16396 RVA: 0x000BA074 File Offset: 0x000B8274
		public override bool Pass()
		{
			return !Scene<GameBase>.instance.uiManager.npcConversation.visible;
		}
	}
}
