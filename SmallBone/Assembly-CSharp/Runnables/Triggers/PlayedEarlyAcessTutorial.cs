using System;
using Data;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000360 RID: 864
	public class PlayedEarlyAcessTutorial : Trigger
	{
		// Token: 0x06001010 RID: 4112 RVA: 0x0002FF0E File Offset: 0x0002E10E
		protected override bool Check()
		{
			if (this._played != PlayedEarlyAcessTutorial.Compare.Played)
			{
				return !GameData.Generic.playedTutorialDuringEA;
			}
			return GameData.Generic.playedTutorialDuringEA;
		}

		// Token: 0x04000D25 RID: 3365
		[SerializeField]
		private PlayedEarlyAcessTutorial.Compare _played;

		// Token: 0x02000361 RID: 865
		private enum Compare
		{
			// Token: 0x04000D27 RID: 3367
			Played,
			// Token: 0x04000D28 RID: 3368
			NotPlayed
		}
	}
}
