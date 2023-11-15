using System;
using System.Collections;
using GameResources;
using Scenes;
using UI;
using UnityEditor;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001E3 RID: 483
	public class OpenContentSelector : Sequence
	{
		// Token: 0x060009FF RID: 2559 RVA: 0x0001BE8B File Offset: 0x0001A08B
		public override IEnumerator CRun()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = this._portrait;
			npcConversation.name = Localization.GetLocalizedString(this._nameKey);
			npcConversation.body = Localization.GetLocalizedString(this._textKey);
			yield return npcConversation.CType();
			npcConversation.OpenContentSelector(Localization.GetLocalizedString(this._contentsLabelKey), delegate()
			{
				this._onContents.Run(null, null);
			}, Localization.GetLocalizedString(this._cancelLabelKey), delegate()
			{
				this._onClose.Run(null, null);
			});
			yield break;
		}

		// Token: 0x04000826 RID: 2086
		[SerializeField]
		[Header("Body")]
		private string _nameKey;

		// Token: 0x04000827 RID: 2087
		[SerializeField]
		private string _textKey;

		// Token: 0x04000828 RID: 2088
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x04000829 RID: 2089
		[SerializeField]
		[Header("Label")]
		private string _contentsLabelKey;

		// Token: 0x0400082A RID: 2090
		[SerializeField]
		private string _cancelLabelKey = "label/confirm/no";

		// Token: 0x0400082B RID: 2091
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ShotInfo))]
		private ShotInfo.Subcomponents _onContents;

		// Token: 0x0400082C RID: 2092
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ShotInfo))]
		private ShotInfo.Subcomponents _onClose;
	}
}
