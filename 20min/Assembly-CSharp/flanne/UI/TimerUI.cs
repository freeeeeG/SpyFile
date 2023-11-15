using System;
using TMPro;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x02000213 RID: 531
	public class TimerUI : MonoBehaviour
	{
		// Token: 0x06000BF3 RID: 3059 RVA: 0x0002C5A1 File Offset: 0x0002A7A1
		private void Start()
		{
			this.gameTimer = GameTimer.SharedInstance;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0002C5AE File Offset: 0x0002A7AE
		private void Update()
		{
			if (this.gameTimer.endless)
			{
				this.TimerTMP.text = this.gameTimer.TimeToString();
				return;
			}
			this.TimerTMP.text = this.gameTimer.TimeRemainingToString();
		}

		// Token: 0x0400084F RID: 2127
		public TMP_Text TimerTMP;

		// Token: 0x04000850 RID: 2128
		private GameTimer gameTimer;
	}
}
