using System;
using System.Collections;
using Scenes;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002F8 RID: 760
	public class GameFadeOut : CRunnable
	{
		// Token: 0x06000EF2 RID: 3826 RVA: 0x0002E099 File Offset: 0x0002C299
		public override IEnumerator CRun()
		{
			GameBase instance = Scene<GameBase>.instance;
			instance.gameFadeInOut.Activate();
			yield return instance.gameFadeInOut.CFadeOut(this._speed);
			yield break;
		}

		// Token: 0x04000C4F RID: 3151
		[SerializeField]
		private float _speed;
	}
}
