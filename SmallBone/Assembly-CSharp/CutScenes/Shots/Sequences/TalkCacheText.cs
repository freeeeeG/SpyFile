using System;
using System.Collections;
using GameResources;
using Runnables;
using Scenes;
using UI;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001F1 RID: 497
	public sealed class TalkCacheText : Sequence
	{
		// Token: 0x06000A3B RID: 2619 RVA: 0x0001C583 File Offset: 0x0001A783
		public override IEnumerator CRun()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = this._portrait;
			npcConversation.skippable = this._skippable;
			npcConversation.name = Localization.GetLocalizedString(this._nameCache.key);
			yield return npcConversation.CConversation(new string[]
			{
				Localization.GetLocalizedString(this._chatCache.key)
			});
			yield break;
		}

		// Token: 0x0400085A RID: 2138
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x0400085B RID: 2139
		[SerializeField]
		private TextKeyCache _nameCache;

		// Token: 0x0400085C RID: 2140
		[SerializeField]
		private TextKeyCache _chatCache;

		// Token: 0x0400085D RID: 2141
		[SerializeField]
		private bool _skippable = true;
	}
}
