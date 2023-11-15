using System;
using Characters;
using Scenes;
using UnityEngine;

namespace CutScenes.Shots.Events.Customs
{
	// Token: 0x02000220 RID: 544
	public class OpenVeteranAdventuererHealthBar : Event
	{
		// Token: 0x06000ABF RID: 2751 RVA: 0x0001D396 File Offset: 0x0001B596
		public override void Run()
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.OpenVeteranAdventurer(this._character, this._nameKey, this._titleKey);
		}

		// Token: 0x040008C5 RID: 2245
		[SerializeField]
		private Character _character;

		// Token: 0x040008C6 RID: 2246
		[SerializeField]
		private string _nameKey;

		// Token: 0x040008C7 RID: 2247
		[SerializeField]
		private string _titleKey;
	}
}
