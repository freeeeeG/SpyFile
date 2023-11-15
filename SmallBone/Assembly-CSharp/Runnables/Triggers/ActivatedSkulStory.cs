using System;
using Scenes;
using SkulStories;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000373 RID: 883
	public class ActivatedSkulStory : Trigger
	{
		// Token: 0x06001045 RID: 4165 RVA: 0x000304F3 File Offset: 0x0002E6F3
		private void Start()
		{
			this._narration = Scene<GameBase>.instance.uiManager.narration;
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0003050A File Offset: 0x0002E70A
		protected override bool Check()
		{
			return this._narration.sceneVisible == this._activated;
		}

		// Token: 0x04000D4B RID: 3403
		[SerializeField]
		private bool _activated;

		// Token: 0x04000D4C RID: 3404
		private Narration _narration;
	}
}
