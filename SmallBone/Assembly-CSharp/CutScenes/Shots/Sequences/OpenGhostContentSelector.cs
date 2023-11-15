using System;
using System.Collections;
using Data;
using GameResources;
using Level;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEditor;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001DE RID: 478
	public class OpenGhostContentSelector : Sequence
	{
		// Token: 0x060009E8 RID: 2536 RVA: 0x0001BBB2 File Offset: 0x00019DB2
		public override IEnumerator CRun()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = this._portrait;
			int hardmodeLevel = GameData.HardmodeProgress.hardmodeLevel;
			int num = Singleton<Service>.Instance.levelManager.currentChapter.type - Chapter.Type.HardmodeChapter1;
			npcConversation.name = Localization.GetLocalizedString(this._nameKey);
			npcConversation.body = Localization.GetLocalizedString(string.Format("{0}/{1}/{2}", this._textKey, hardmodeLevel, num));
			npcConversation.skippable = this._skippable;
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

		// Token: 0x04000810 RID: 2064
		[SerializeField]
		[Header("Body")]
		private string _nameKey;

		// Token: 0x04000811 RID: 2065
		[SerializeField]
		private string _textKey;

		// Token: 0x04000812 RID: 2066
		[SerializeField]
		private bool _skippable;

		// Token: 0x04000813 RID: 2067
		[SerializeField]
		private Sprite _portrait;

		// Token: 0x04000814 RID: 2068
		[SerializeField]
		[Header("Label")]
		private string _contentsLabelKey;

		// Token: 0x04000815 RID: 2069
		[SerializeField]
		private string _cancelLabelKey = "label/confirm/no";

		// Token: 0x04000816 RID: 2070
		[UnityEditor.Subcomponent(typeof(ShotInfo))]
		[SerializeField]
		private ShotInfo.Subcomponents _onContents;

		// Token: 0x04000817 RID: 2071
		[UnityEditor.Subcomponent(typeof(ShotInfo))]
		[SerializeField]
		private ShotInfo.Subcomponents _onClose;
	}
}
