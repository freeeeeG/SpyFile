using System;
using Runnables;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x02000127 RID: 295
	public class Skip : Runnable
	{
		// Token: 0x060005BD RID: 1469 RVA: 0x0001106C File Offset: 0x0000F26C
		public override void Run()
		{
			this._onSkip.Run();
		}

		// Token: 0x0400045D RID: 1117
		[SerializeField]
		[Event.SubcomponentAttribute]
		private Event.Subcomponents _onSkip;
	}
}
