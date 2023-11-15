using System;
using Data;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000364 RID: 868
	public class PlayedTutorial : Trigger
	{
		// Token: 0x06001014 RID: 4116 RVA: 0x0002FF54 File Offset: 0x0002E154
		protected override bool Check()
		{
			if (this._played != PlayedTutorial.Compare.Played)
			{
				return !GameData.Generic.tutorial.isPlayed();
			}
			return GameData.Generic.tutorial.isPlayed();
		}

		// Token: 0x04000D2E RID: 3374
		[SerializeField]
		private PlayedTutorial.Compare _played;

		// Token: 0x02000365 RID: 869
		private enum Compare
		{
			// Token: 0x04000D30 RID: 3376
			Played,
			// Token: 0x04000D31 RID: 3377
			NotPlayed
		}
	}
}
