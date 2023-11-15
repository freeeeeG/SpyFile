using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Helios.GUI
{
	// Token: 0x020000DB RID: 219
	public class FakeLoadingBar : MonoBehaviour
	{
		// Token: 0x0600032B RID: 811 RVA: 0x0000E56D File Offset: 0x0000C76D
		private void OnEnable()
		{
			this._sdLoadingBar.value = 0f;
			this._txtLoadingPercent.text = "0";
			base.StartCoroutine(this.FakeLoading());
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000E59C File Offset: 0x0000C79C
		private IEnumerator FakeLoading()
		{
			yield return new WaitForSeconds(Random.Range(0.3f, 1f));
			float num = this._sdLoadingBar.value;
			num += (float)Random.Range(10, 21);
			if (num >= 100f)
			{
				num = 100f;
			}
			this._sdLoadingBar.value = num;
			this._txtLoadingPercent.text = string.Format("{0}%", num);
			if (num >= 100f)
			{
				yield return new WaitForSeconds(Random.Range(0.3f, 1f));
				SingletonPersistent<GameManager>.Instance.Back();
				SingletonPersistent<GameManager>.Instance.LoadPopup(this._objNextScreen);
				foreach (GameObject go in this._arrNextPopup)
				{
					SingletonPersistent<GameManager>.Instance.LoadPopup(go);
				}
				base.StopAllCoroutines();
			}
			else
			{
				base.StartCoroutine(this.FakeLoading());
			}
			yield break;
		}

		// Token: 0x0400030B RID: 779
		[SerializeField]
		private Slider _sdLoadingBar;

		// Token: 0x0400030C RID: 780
		[SerializeField]
		private GameObject _objNextScreen;

		// Token: 0x0400030D RID: 781
		[SerializeField]
		private TMP_Text _txtLoadingPercent;

		// Token: 0x0400030E RID: 782
		[SerializeField]
		private GameObject[] _arrNextPopup;
	}
}
