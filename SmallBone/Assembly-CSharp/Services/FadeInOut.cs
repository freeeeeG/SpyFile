using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Services
{
	// Token: 0x02000130 RID: 304
	public class FadeInOut : MonoBehaviour
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001126A File Offset: 0x0000F46A
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x00011272 File Offset: 0x0000F472
		public bool fading { get; private set; }

		// Token: 0x060005DC RID: 1500 RVA: 0x0001127B File Offset: 0x0000F47B
		private void SetFadeAlpha(float alpha)
		{
			this._color.a = alpha;
			this._fadeBackground.color = this._color;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001129A File Offset: 0x0000F49A
		public void SetFadeColor(Color color)
		{
			this._color = color;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000112A3 File Offset: 0x0000F4A3
		public void FadeIn()
		{
			base.StartCoroutine(this.CFadeIn());
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x000112B2 File Offset: 0x0000F4B2
		public IEnumerator CFadeIn()
		{
			this.fading = true;
			float t = 0f;
			this.SetFadeAlpha(1f);
			yield return null;
			while (t < 1f)
			{
				this.SetFadeAlpha(1f - t);
				yield return null;
				t += Time.unscaledDeltaTime * 2f;
			}
			this.SetFadeAlpha(0f);
			this.fading = false;
			yield break;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000112C1 File Offset: 0x0000F4C1
		public void FadeOut()
		{
			base.StartCoroutine(this.CFadeOut());
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000112D0 File Offset: 0x0000F4D0
		public IEnumerator CFadeOut()
		{
			this.fading = true;
			float t = 0f;
			this.SetFadeAlpha(0f);
			yield return null;
			while (t < 1f)
			{
				this.SetFadeAlpha(t);
				yield return null;
				t += Time.unscaledDeltaTime * 2f;
			}
			this.SetFadeAlpha(1f);
			yield break;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x000112DF File Offset: 0x0000F4DF
		public IEnumerator CShowLoadingScreen(LoadingScreen.LoadingScreenData loadingScreenData)
		{
			this._loadingScreen.gameObject.SetActive(true);
			yield return this._loadingScreen.CShow(loadingScreenData);
			yield break;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x000112F5 File Offset: 0x0000F4F5
		public IEnumerator CHideLoadingScreen()
		{
			yield return this._loadingScreen.CHide();
			this._loadingScreen.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00011304 File Offset: 0x0000F504
		public void ShowLoadingIcon()
		{
			this._loading.SetActive(true);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00011312 File Offset: 0x0000F512
		public void HideLoadingIcon()
		{
			this._loading.SetActive(false);
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00011320 File Offset: 0x0000F520
		public void FadeOutImmediately()
		{
			this.fading = true;
			this.SetFadeAlpha(1f);
		}

		// Token: 0x04000470 RID: 1136
		[SerializeField]
		private Image _fadeBackground;

		// Token: 0x04000471 RID: 1137
		[SerializeField]
		private LoadingScreen _loadingScreen;

		// Token: 0x04000472 RID: 1138
		[SerializeField]
		private GameObject _loading;

		// Token: 0x04000473 RID: 1139
		private Color _color = Color.black;
	}
}
