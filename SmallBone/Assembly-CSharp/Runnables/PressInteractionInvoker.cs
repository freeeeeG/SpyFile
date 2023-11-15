using System;
using Characters;
using UI;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002B8 RID: 696
	public class PressInteractionInvoker : InteractiveObject
	{
		// Token: 0x06000E2B RID: 3627 RVA: 0x0002CCB3 File Offset: 0x0002AEB3
		private void Start()
		{
			this._releaseButton.onPressed += this._cutScene.Run;
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00002191 File Offset: 0x00000391
		public override void InteractWith(Character character)
		{
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0002CCD2 File Offset: 0x0002AED2
		private void OnDestroy()
		{
			if (this._releaseButton == null)
			{
				return;
			}
			this._releaseButton.onPressed -= this._cutScene.Run;
		}

		// Token: 0x04000BCB RID: 3019
		[SerializeField]
		private PressingButton _releaseButton;

		// Token: 0x04000BCC RID: 3020
		[SerializeField]
		private Runnable _cutScene;
	}
}
