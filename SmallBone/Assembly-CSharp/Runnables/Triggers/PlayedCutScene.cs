using System;
using CutScenes;
using Data;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x0200035E RID: 862
	public class PlayedCutScene : Trigger
	{
		// Token: 0x0600100E RID: 4110 RVA: 0x0002FEE0 File Offset: 0x0002E0E0
		protected override bool Check()
		{
			if (this._played != PlayedCutScene.Compare.Played)
			{
				return !GameData.Progress.cutscene.GetData(this._key);
			}
			return GameData.Progress.cutscene.GetData(this._key);
		}

		// Token: 0x04000D20 RID: 3360
		[SerializeField]
		private Key _key;

		// Token: 0x04000D21 RID: 3361
		[SerializeField]
		private PlayedCutScene.Compare _played;

		// Token: 0x0200035F RID: 863
		private enum Compare
		{
			// Token: 0x04000D23 RID: 3363
			Played,
			// Token: 0x04000D24 RID: 3364
			NotPlayed
		}
	}
}
