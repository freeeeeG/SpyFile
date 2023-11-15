using System;
using Data;
using SkulStories;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000362 RID: 866
	public class PlayedSkulStory : Trigger
	{
		// Token: 0x06001012 RID: 4114 RVA: 0x0002FF26 File Offset: 0x0002E126
		protected override bool Check()
		{
			if (this._played != PlayedSkulStory.Compare.Played)
			{
				return !GameData.Progress.skulstory.GetData(this._key);
			}
			return GameData.Progress.skulstory.GetData(this._key);
		}

		// Token: 0x04000D29 RID: 3369
		[SerializeField]
		private Key _key;

		// Token: 0x04000D2A RID: 3370
		[SerializeField]
		private PlayedSkulStory.Compare _played;

		// Token: 0x02000363 RID: 867
		private enum Compare
		{
			// Token: 0x04000D2C RID: 3372
			Played,
			// Token: 0x04000D2D RID: 3373
			NotPlayed
		}
	}
}
