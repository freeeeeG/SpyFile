using System;
using Scenes;
using UnityEngine;

namespace Platforms
{
	// Token: 0x02000148 RID: 328
	public class PlatformPauseHandler : MonoBehaviour
	{
		// Token: 0x0600068B RID: 1675 RVA: 0x0001316F File Offset: 0x0001136F
		private void Start()
		{
			this._platformManager.onPause += this.OnPause;
			this._platformManager.onResume += this.OnResume;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001319F File Offset: 0x0001139F
		private void OnPause()
		{
			if (Scene<GameBase>.instance != null)
			{
				Scene<GameBase>.instance.uiManager.ShowPausePopup();
			}
			Chronometer.global.AttachTimeScale(this, 0f);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x000131CD File Offset: 0x000113CD
		private void OnResume()
		{
			Chronometer.global.DetachTimeScale(this);
		}

		// Token: 0x040004D2 RID: 1234
		[SerializeField]
		private PlatformManager _platformManager;
	}
}
