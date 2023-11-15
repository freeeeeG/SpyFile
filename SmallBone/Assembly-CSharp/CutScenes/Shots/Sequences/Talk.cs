using System;
using System.Collections;
using GameResources;
using Scenes;
using UI;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001EF RID: 495
	public sealed class Talk : Sequence
	{
		// Token: 0x06000A33 RID: 2611 RVA: 0x0001C4C5 File Offset: 0x0001A6C5
		public override IEnumerator CRun()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = null;
			npcConversation.skippable = this._skippable;
			npcConversation.name = Localization.GetLocalizedString(this._nameKey);
			yield return npcConversation.CConversation(Localization.GetLocalizedStringArray(this._textKey));
			yield break;
		}

		// Token: 0x04000853 RID: 2131
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x04000854 RID: 2132
		[SerializeField]
		private string _nameKey;

		// Token: 0x04000855 RID: 2133
		[SerializeField]
		private string _textKey;

		// Token: 0x04000856 RID: 2134
		[SerializeField]
		private bool _skippable = true;
	}
}
