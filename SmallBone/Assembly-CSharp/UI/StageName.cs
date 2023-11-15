using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003D0 RID: 976
	public sealed class StageName : MonoBehaviour
	{
		// Token: 0x06001224 RID: 4644 RVA: 0x000357EC File Offset: 0x000339EC
		public void Show(string chapterName, string stageNumber, string stageName)
		{
			base.gameObject.SetActive(true);
			this._chapterName.text = chapterName;
			this._stageNumber.text = stageNumber;
			this._stageName.text = stageName;
			base.StartCoroutine(this.CShow());
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0003582B File Offset: 0x00033A2B
		private IEnumerator CShow()
		{
			this._animator.Appear();
			yield return new WaitForSecondsRealtime(4f);
			this._animator.Disappear();
			yield break;
		}

		// Token: 0x04000F0C RID: 3852
		[SerializeField]
		private TextMeshProUGUI _chapterName;

		// Token: 0x04000F0D RID: 3853
		[SerializeField]
		private TextMeshProUGUI _stageNumber;

		// Token: 0x04000F0E RID: 3854
		[SerializeField]
		private TextMeshProUGUI _stageName;

		// Token: 0x04000F0F RID: 3855
		[SerializeField]
		private Image _background;

		// Token: 0x04000F10 RID: 3856
		[SerializeField]
		private HangingPanelAnimator _animator;
	}
}
