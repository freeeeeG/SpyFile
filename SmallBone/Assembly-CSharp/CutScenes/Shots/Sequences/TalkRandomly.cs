using System;
using System.Collections;
using GameResources;
using Scenes;
using UI;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001F3 RID: 499
	public sealed class TalkRandomly : Sequence
	{
		// Token: 0x06000A43 RID: 2627 RVA: 0x0001C657 File Offset: 0x0001A857
		public override IEnumerator CRun()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = this._portrait;
			npcConversation.skippable = this._skippable;
			npcConversation.name = Localization.GetLocalizedString(this._nameKey);
			string[] texts = Localization.GetLocalizedStringArrays(this._textArrayKey).Random<string[]>();
			yield return npcConversation.CConversation(texts);
			yield break;
		}

		// Token: 0x04000861 RID: 2145
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x04000862 RID: 2146
		[SerializeField]
		private string _nameKey;

		// Token: 0x04000863 RID: 2147
		[SerializeField]
		private string _textArrayKey;

		// Token: 0x04000864 RID: 2148
		[SerializeField]
		private bool _skippable = true;
	}
}
