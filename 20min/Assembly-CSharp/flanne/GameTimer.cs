using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000B6 RID: 182
	public class GameTimer : MonoBehaviour
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0001B3AD File Offset: 0x000195AD
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x0001B3B5 File Offset: 0x000195B5
		public float timer { get; private set; }

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001B3BE File Offset: 0x000195BE
		private void Awake()
		{
			GameTimer.SharedInstance = this;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001B3C8 File Offset: 0x000195C8
		private void Update()
		{
			if (this._isPlaying)
			{
				this.timer += Time.deltaTime;
				if (this.timer >= this.timeLimit - 1f && !this.oneSecondWarningSent)
				{
					this.oneSecondWarningSent = true;
					this.PostNotification(GameTimer.OneSecondLeftNotification, null);
					this.timer = this.timeLimit - 1f;
				}
				if (this.timer >= this.timeLimit && !this.endless)
				{
					this.PostNotification(GameTimer.TimeLimitNotification, null);
					this.timer = this.timeLimit;
				}
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001B461 File Offset: 0x00019661
		public void Start()
		{
			this._isPlaying = true;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001B46A File Offset: 0x0001966A
		public void Stop()
		{
			this._isPlaying = false;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001B474 File Offset: 0x00019674
		public string TimeToString()
		{
			int num = Mathf.FloorToInt(this.timer / 60f);
			int num2 = Mathf.FloorToInt(this.timer % 60f);
			return num.ToString("00") + ":" + num2.ToString("00");
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001B4C8 File Offset: 0x000196C8
		public string TimeRemainingToString()
		{
			float num = this.timeLimit - this.timer;
			int num2 = Mathf.FloorToInt(num / 60f);
			int num3 = Mathf.FloorToInt(num % 60f);
			return num2.ToString("00") + ":" + num3.ToString("00");
		}

		// Token: 0x040003AD RID: 941
		public static GameTimer SharedInstance;

		// Token: 0x040003AE RID: 942
		public static string TimeLimitNotification = "GameTimer.TimeLimitNotification";

		// Token: 0x040003AF RID: 943
		public static string OneSecondLeftNotification = "GameTimer.OneSecondLeftNotification";

		// Token: 0x040003B0 RID: 944
		[NonSerialized]
		public float timeLimit = -1f;

		// Token: 0x040003B1 RID: 945
		[NonSerialized]
		public bool endless;

		// Token: 0x040003B2 RID: 946
		private bool oneSecondWarningSent;

		// Token: 0x040003B4 RID: 948
		private bool _isPlaying = true;
	}
}
