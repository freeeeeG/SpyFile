using System;
using System.Collections;
using Scenes;
using UI;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001ED RID: 493
	public sealed class NextTalk : Sequence
	{
		// Token: 0x06000A2B RID: 2603 RVA: 0x0001C3FD File Offset: 0x0001A5FD
		public override IEnumerator CRun()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = this._talker.portrait;
			npcConversation.skippable = this._skippable;
			npcConversation.name = this._talker.name;
			yield return npcConversation.CConversation(this._talker.GetNextText());
			yield break;
		}

		// Token: 0x0400084E RID: 2126
		[SerializeField]
		private TalkerInfo _talker;

		// Token: 0x0400084F RID: 2127
		[SerializeField]
		private bool _skippable = true;
	}
}
