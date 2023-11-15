using System;
using GameResources;
using UnityEngine;

namespace CutScenes.Shots
{
	// Token: 0x020001CA RID: 458
	public class TalkerRandomInfo : TalkerInfo
	{
		// Token: 0x06000995 RID: 2453 RVA: 0x0001B320 File Offset: 0x00019520
		private void Awake()
		{
			this._seleted = UnityEngine.Random.Range(this._range.x, this._range.y);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0001B344 File Offset: 0x00019544
		public override string[] GetNextText()
		{
			string format = "{0}/{1}/{2}";
			object textKey = this._textKey;
			object arg = this._seleted;
			int currentIndex = this._currentIndex;
			this._currentIndex = currentIndex + 1;
			return Localization.GetLocalizedStringArray(string.Format(format, textKey, arg, currentIndex));
		}

		// Token: 0x040007DF RID: 2015
		[MinMaxSlider(0f, 10f)]
		private Vector2Int _range;

		// Token: 0x040007E0 RID: 2016
		private int _seleted;
	}
}
