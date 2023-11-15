using System;
using Scenes;

namespace Runnables
{
	// Token: 0x020002E2 RID: 738
	public sealed class CompleteUIConversation : UICommands
	{
		// Token: 0x06000EAD RID: 3757 RVA: 0x0002D970 File Offset: 0x0002BB70
		public override void Run()
		{
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
		}
	}
}
