using System;
using System.Collections;
using GameResources;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001E9 RID: 489
	public class ShowLine : Sequence
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x0001C1CA File Offset: 0x0001A3CA
		public override IEnumerator CRun()
		{
			string localizedString = Localization.GetLocalizedString(this._messageInfo.messageKey);
			yield return this._lineText.CDisplay(localizedString, this._time);
			yield break;
		}

		// Token: 0x0400083E RID: 2110
		[SerializeField]
		private TextMessageInfo _messageInfo;

		// Token: 0x0400083F RID: 2111
		[SerializeField]
		private LineText _lineText;

		// Token: 0x04000840 RID: 2112
		[SerializeField]
		private float _time;
	}
}
