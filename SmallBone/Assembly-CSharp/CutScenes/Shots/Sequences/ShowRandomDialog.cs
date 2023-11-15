using System;
using System.Collections;
using System.Collections.Generic;
using GameResources;
using Scenes;
using UI;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001EB RID: 491
	public class ShowRandomDialog : Sequence
	{
		// Token: 0x06000A22 RID: 2594 RVA: 0x0001C255 File Offset: 0x0001A455
		private void Start()
		{
			this._npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			this._npcConversation.portrait = this._portrait;
			this._npcConversation.skippable = true;
			this._texts = new List<string>();
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0001C294 File Offset: 0x0001A494
		public override IEnumerator CRun()
		{
			this._randomIndex = UnityEngine.Random.Range(0, this._messageInfo.Length);
			this._startIndex = this._messageInfo[this._randomIndex].messages[0].startIndex;
			this._endIndex = this._messageInfo[this._randomIndex].messages[0].endIndex;
			TextMessageInfo textMessageInfo = this._messageInfo[this._randomIndex];
			string nameKey = textMessageInfo.nameKey;
			string messageKey = textMessageInfo.messageKey;
			this._npcConversation.name = Localization.GetLocalizedString(nameKey);
			while (this._startIndex <= this._endIndex)
			{
				this._texts.Add(Localization.GetLocalizedString(messageKey + this._startIndex.ToString()));
				this._startIndex++;
			}
			yield return this._npcConversation.CConversation(this._texts.ToArray());
			this._texts.Clear();
			this._startIndex = this._messageInfo[this._randomIndex].messages[0].startIndex;
			yield break;
		}

		// Token: 0x04000844 RID: 2116
		[SerializeField]
		private TextMessageInfo[] _messageInfo;

		// Token: 0x04000845 RID: 2117
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x04000846 RID: 2118
		private NpcConversation _npcConversation;

		// Token: 0x04000847 RID: 2119
		private int _startIndex;

		// Token: 0x04000848 RID: 2120
		private int _endIndex;

		// Token: 0x04000849 RID: 2121
		private int _randomIndex;

		// Token: 0x0400084A RID: 2122
		private List<string> _texts;
	}
}
