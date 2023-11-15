using System;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x0200023C RID: 572
	[RequireComponent(typeof(CanvasGroup))]
	public class AlphaTween : UITweener
	{
		// Token: 0x06000C92 RID: 3218 RVA: 0x0002DFCB File Offset: 0x0002C1CB
		private void Awake()
		{
			this.canvasGroup = base.GetComponent<CanvasGroup>();
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0002DFD9 File Offset: 0x0002C1D9
		public override void Show()
		{
			LeanTween.cancel(this._tweenID);
			this._tweenID = LeanTween.alphaCanvas(this.canvasGroup, 1f, this.duration).setIgnoreTimeScale(true).id;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0002E00D File Offset: 0x0002C20D
		public override void Hide()
		{
			LeanTween.cancel(this._tweenID);
			this._tweenID = LeanTween.alphaCanvas(this.canvasGroup, 0f, this.duration).setIgnoreTimeScale(true).id;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0002E041 File Offset: 0x0002C241
		public override void SetOff()
		{
			this.canvasGroup.alpha = 0f;
		}

		// Token: 0x040008CF RID: 2255
		private CanvasGroup canvasGroup;

		// Token: 0x040008D0 RID: 2256
		private int _tweenID;
	}
}
