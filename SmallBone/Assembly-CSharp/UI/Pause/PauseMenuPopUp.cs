using System;
using UnityEngine;

namespace UI.Pause
{
	// Token: 0x02000423 RID: 1059
	public class PauseMenuPopUp : PauseEvent
	{
		// Token: 0x0600140E RID: 5134 RVA: 0x0003D431 File Offset: 0x0003B631
		public override void Invoke()
		{
			this._panel.Open();
		}

		// Token: 0x04001106 RID: 4358
		[SerializeField]
		private Panel _panel;
	}
}
