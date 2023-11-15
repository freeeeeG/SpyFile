using System;
using Scenes;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000323 RID: 803
	public sealed class GameFadeInOut : Runnable
	{
		// Token: 0x06000F74 RID: 3956 RVA: 0x0002F078 File Offset: 0x0002D278
		public override void Run()
		{
			GameBase instance = Scene<GameBase>.instance;
			if (this._fadeOut)
			{
				instance.gameFadeInOut.FadeOut(1f);
				return;
			}
			instance.gameFadeInOut.FadeIn(1f);
		}

		// Token: 0x04000CB7 RID: 3255
		[SerializeField]
		private bool _fadeOut;

		// Token: 0x04000CB8 RID: 3256
		[SerializeField]
		private float _speed;
	}
}
