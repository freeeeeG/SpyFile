using System;
using UI.SkulStories;
using UnityEngine;

namespace UI.Pause
{
	// Token: 0x0200042B RID: 1067
	public class StorySkip : PauseEvent
	{
		// Token: 0x06001453 RID: 5203 RVA: 0x0003E814 File Offset: 0x0003CA14
		public override void Invoke()
		{
			this._panel.Open();
		}

		// Token: 0x0400114B RID: 4427
		[SerializeField]
		private Confirm _panel;
	}
}
