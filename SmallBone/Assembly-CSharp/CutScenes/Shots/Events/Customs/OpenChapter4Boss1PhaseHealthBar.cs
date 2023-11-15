using System;
using Characters;
using Scenes;
using UnityEngine;

namespace CutScenes.Shots.Events.Customs
{
	// Token: 0x0200021D RID: 541
	public sealed class OpenChapter4Boss1PhaseHealthBar : Event
	{
		// Token: 0x06000AB1 RID: 2737 RVA: 0x0001D171 File Offset: 0x0001B371
		public override void Run()
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.OpenChapter4Phase1(this._left, this._right);
		}

		// Token: 0x040008BE RID: 2238
		[SerializeField]
		private Character _left;

		// Token: 0x040008BF RID: 2239
		[SerializeField]
		private Character _right;
	}
}
