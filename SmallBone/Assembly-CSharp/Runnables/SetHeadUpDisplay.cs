using System;
using Scenes;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002E4 RID: 740
	public sealed class SetHeadUpDisplay : UICommands
	{
		// Token: 0x06000EB2 RID: 3762 RVA: 0x0002D9AD File Offset: 0x0002BBAD
		public override void Run()
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = this._visible;
		}

		// Token: 0x04000C2C RID: 3116
		[SerializeField]
		private bool _visible;
	}
}
