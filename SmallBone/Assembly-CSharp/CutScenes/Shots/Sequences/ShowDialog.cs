using System;
using System.Collections;
using System.Collections.Generic;
using GameResources;
using Scenes;
using UI;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001E7 RID: 487
	public sealed class ShowDialog : Sequence
	{
		// Token: 0x06000A11 RID: 2577 RVA: 0x0001C054 File Offset: 0x0001A254
		private void Start()
		{
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			this._startIndex = this._messageInfo.messages[0].startIndex;
			this._endIndex = this._messageInfo.messages[0].endIndex;
			this._npcConversation.portrait = this._portrait;
			this._npcConversation.skippable = true;
			this._texts = new List<string>();
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0001C0CE File Offset: 0x0001A2CE
		public override IEnumerator CRun()
		{
			TextMessageInfo messageInfo = this._messageInfo;
			string nameKey = messageInfo.nameKey;
			string messageKey = messageInfo.messageKey;
			this._npcConversation.name = Localization.GetLocalizedString(nameKey);
			while (this._startIndex <= this._endIndex)
			{
				this._texts.Add(Localization.GetLocalizedString(messageKey + this._startIndex.ToString()));
				this._startIndex++;
			}
			yield return this._npcConversation.CConversation(this._texts.ToArray());
			this._startIndex = this._messageInfo.messages[0].startIndex;
			yield break;
		}

		// Token: 0x04000835 RID: 2101
		[SerializeField]
		private TextMessageInfo _messageInfo;

		// Token: 0x04000836 RID: 2102
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x04000837 RID: 2103
		private int _startIndex;

		// Token: 0x04000838 RID: 2104
		private int _endIndex;

		// Token: 0x04000839 RID: 2105
		private NpcConversation _npcConversation;

		// Token: 0x0400083A RID: 2106
		private List<string> _texts;
	}
}
