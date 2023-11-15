using System;
using System.Collections;
using GameResources;
using Scenes;
using UI;
using UnityEditor;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001E1 RID: 481
	public class OpenChatSelector : Sequence
	{
		// Token: 0x060009F5 RID: 2549 RVA: 0x0001BD7E File Offset: 0x00019F7E
		public override IEnumerator CRun()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.OpenChatSelector(delegate
			{
				this._onChat.Run(null, null);
			}, delegate
			{
				this._onClose.Run(null, null);
			});
			npcConversation.portrait = this._portarit;
			npcConversation.name = Localization.GetLocalizedString(this._nameKey);
			if (this._randomText)
			{
				npcConversation.body = Localization.GetLocalizedStringArray(this._textKey).Random<string>();
			}
			else
			{
				npcConversation.body = Localization.GetLocalizedString(this._textKey);
			}
			yield return npcConversation.CType();
			yield break;
		}

		// Token: 0x0400081D RID: 2077
		[SerializeField]
		private Sprite _portarit;

		// Token: 0x0400081E RID: 2078
		[SerializeField]
		private string _nameKey;

		// Token: 0x0400081F RID: 2079
		[SerializeField]
		private bool _randomText;

		// Token: 0x04000820 RID: 2080
		[SerializeField]
		private string _textKey;

		// Token: 0x04000821 RID: 2081
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ShotInfo))]
		private ShotInfo.Subcomponents _onChat;

		// Token: 0x04000822 RID: 2082
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ShotInfo))]
		private ShotInfo.Subcomponents _onClose;
	}
}
