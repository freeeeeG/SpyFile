using System;
using Characters;
using Scenes;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200033B RID: 827
	public sealed class OpenChapter4Phase1 : UICommands
	{
		// Token: 0x06000FB4 RID: 4020 RVA: 0x0002F70F File Offset: 0x0002D90F
		public override void Run()
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.OpenChapter4Phase1(this._left, this._right);
		}

		// Token: 0x04000CE8 RID: 3304
		[SerializeField]
		private Character _left;

		// Token: 0x04000CE9 RID: 3305
		[SerializeField]
		private Character _right;
	}
}
