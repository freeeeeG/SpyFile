using System;
using GameResources;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200032F RID: 815
	public sealed class ShowLineText : Runnable
	{
		// Token: 0x06000F92 RID: 3986 RVA: 0x0002F2EC File Offset: 0x0002D4EC
		public override void Run()
		{
			if (this._lineText == null)
			{
				this._lineText = base.GetComponentInChildren<LineText>();
				if (this._lineText == null)
				{
					return;
				}
			}
			if (!this._force && !this._lineText.finished)
			{
				return;
			}
			string[] localizedStringArray = Localization.GetLocalizedStringArray(this._textKey);
			if (localizedStringArray.Length == 0)
			{
				return;
			}
			string text = localizedStringArray.Random<string>();
			Vector3 position = this._spawnPosition.position;
			this._lineText.transform.position = position;
			this._lineText.Display(text, this._duration);
		}

		// Token: 0x04000CCC RID: 3276
		[SerializeField]
		private LineText _lineText;

		// Token: 0x04000CCD RID: 3277
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04000CCE RID: 3278
		[SerializeField]
		private string _textKey;

		// Token: 0x04000CCF RID: 3279
		[SerializeField]
		private float _duration = 2f;

		// Token: 0x04000CD0 RID: 3280
		[SerializeField]
		private bool _force;
	}
}
