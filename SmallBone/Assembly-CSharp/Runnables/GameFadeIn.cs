using System;
using System.Collections;
using Scenes;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002F6 RID: 758
	public class GameFadeIn : CRunnable
	{
		// Token: 0x06000EEA RID: 3818 RVA: 0x0002E01A File Offset: 0x0002C21A
		public override IEnumerator CRun()
		{
			GameBase instance = Scene<GameBase>.instance;
			yield return instance.gameFadeInOut.CFadeIn(this._speed);
			yield break;
		}

		// Token: 0x04000C4B RID: 3147
		[SerializeField]
		private float _speed;
	}
}
