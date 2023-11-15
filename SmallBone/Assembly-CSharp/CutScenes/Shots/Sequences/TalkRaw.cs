using System;
using System.Collections;
using Scenes;
using UI;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001F5 RID: 501
	public sealed class TalkRaw : Sequence
	{
		// Token: 0x06000A4B RID: 2635 RVA: 0x0001C71F File Offset: 0x0001A91F
		public override IEnumerator CRun()
		{
			NpcConversation npcConversation = Scene<GameBase>.instance.uiManager.npcConversation;
			npcConversation.portrait = null;
			npcConversation.skippable = this._skippable;
			npcConversation.name = this._info.name;
			yield return npcConversation.CConversation(this._info.messages);
			yield break;
		}

		// Token: 0x04000868 RID: 2152
		[SerializeField]
		private TalkRaw.Info _info;

		// Token: 0x04000869 RID: 2153
		[SerializeField]
		private bool _skippable = true;

		// Token: 0x020001F6 RID: 502
		[Serializable]
		private class Info
		{
			// Token: 0x0400086A RID: 2154
			public string name;

			// Token: 0x0400086B RID: 2155
			public string[] messages;
		}
	}
}
