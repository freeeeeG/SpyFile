using System;
using System.Collections;
using UnityEngine;

namespace flanne.Core
{
	// Token: 0x02000201 RID: 513
	public class PauseController : MonoBehaviour
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x0002B2EA File Offset: 0x000294EA
		public static bool isPaused
		{
			get
			{
				return PauseController._isPaused.value;
			}
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0002B2F6 File Offset: 0x000294F6
		private void Awake()
		{
			PauseController.SharedInstance = this;
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002B2FE File Offset: 0x000294FE
		private void Start()
		{
			this.pauseCoroutine = null;
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0002B307 File Offset: 0x00029507
		public void Pause()
		{
			PauseController._isPaused.Flip();
			Time.timeScale = 0f;
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002B31D File Offset: 0x0002951D
		public void Pause(float duration)
		{
			if (this.pauseCoroutine != null)
			{
				PauseController._isPaused.UnFlip();
				base.StopCoroutine(this.pauseCoroutine);
			}
			this.pauseCoroutine = this.PauseCR(duration);
			base.StartCoroutine(this.pauseCoroutine);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002B357 File Offset: 0x00029557
		public void UnPause()
		{
			PauseController._isPaused.UnFlip();
			if (!PauseController._isPaused.value)
			{
				Time.timeScale = this._timeScale;
			}
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0002B37A File Offset: 0x0002957A
		public void SetTimeScale(float timeScale)
		{
			this._timeScale = timeScale;
			if (!PauseController.isPaused)
			{
				Time.timeScale = this._timeScale;
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0002B395 File Offset: 0x00029595
		private IEnumerator PauseCR(float duration)
		{
			this.Pause();
			yield return new WaitForSecondsRealtime(duration);
			this.UnPause();
			this.pauseCoroutine = null;
			yield break;
		}

		// Token: 0x040007EF RID: 2031
		public static PauseController SharedInstance;

		// Token: 0x040007F0 RID: 2032
		private IEnumerator pauseCoroutine;

		// Token: 0x040007F1 RID: 2033
		private static BoolToggle _isPaused = new BoolToggle(false);

		// Token: 0x040007F2 RID: 2034
		private float _timeScale = 1f;
	}
}
