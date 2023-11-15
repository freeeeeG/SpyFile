using System;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class PlaytimeTimer : MonoBehaviour
{
	// Token: 0x0600020C RID: 524 RVA: 0x00008FD0 File Offset: 0x000071D0
	private void Update()
	{
		if (Singleton<Service>.Instance.fadeInOut.fading)
		{
			return;
		}
		if (Map.Instance != null && Map.Instance.pauseTimer)
		{
			return;
		}
		this._remainTime -= Time.deltaTime;
		if (this._remainTime < 0f)
		{
			this._remainTime += 1f;
			GameData.Progress.playTime++;
		}
	}

	// Token: 0x040001C3 RID: 451
	private float _remainTime = 1f;
}
