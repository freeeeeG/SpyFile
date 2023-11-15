using System;
using Characters;
using Scenes;
using UI;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002E3 RID: 739
	public sealed class OpenUIHealthBar : UICommands
	{
		// Token: 0x06000EAF RID: 3759 RVA: 0x0002D986 File Offset: 0x0002BB86
		public override void Run()
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.Open(this._type, this._character);
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0002D94D File Offset: 0x0002BB4D
		private void OnDestroy()
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
		}

		// Token: 0x04000C2A RID: 3114
		[SerializeField]
		private Character _character;

		// Token: 0x04000C2B RID: 3115
		[SerializeField]
		private BossHealthbarController.Type _type;
	}
}
