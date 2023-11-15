using System;
using System.Collections;
using System.Collections.Generic;
using GameResources;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x0200010F RID: 271
	public class PlaceText : Sequence
	{
		// Token: 0x0600055C RID: 1372 RVA: 0x000106DB File Offset: 0x0000E8DB
		private void Start()
		{
			this._textList = new List<string>();
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000106E8 File Offset: 0x0000E8E8
		public override IEnumerator CRun()
		{
			foreach (TextLinkInfos.TextLink textInfo in this._textInfo.texts)
			{
				this.AddText(textInfo);
			}
			this._narration.CombineTexts(this._textList.ToArray());
			yield break;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x000106F8 File Offset: 0x0000E8F8
		private void AddText(TextLinkInfos.TextLink textInfo)
		{
			string localizedString = Localization.GetLocalizedString(textInfo.text);
			TextLinkInfos.TextLink.Position position = textInfo.position;
			if (position == TextLinkInfos.TextLink.Position.Normal)
			{
				this._textList.Add(localizedString);
				return;
			}
			if (position != TextLinkInfos.TextLink.Position.Below)
			{
				return;
			}
			this._textList.Add(Environment.NewLine + localizedString);
		}

		// Token: 0x04000418 RID: 1048
		[SerializeField]
		private TextLinkInfos _textInfo;

		// Token: 0x04000419 RID: 1049
		private List<string> _textList;
	}
}
