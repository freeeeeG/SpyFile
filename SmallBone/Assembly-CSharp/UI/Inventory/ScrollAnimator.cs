using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory
{
	// Token: 0x02000441 RID: 1089
	public class ScrollAnimator : MonoBehaviour
	{
		// Token: 0x060014B1 RID: 5297 RVA: 0x000405B9 File Offset: 0x0003E7B9
		private void OnEnable()
		{
			this.Appear();
			base.StartCoroutine(this.CBlockInput());
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x000405CE File Offset: 0x0003E7CE
		private void OnDisable()
		{
			EventSystem.current.currentInputModule.enabled = true;
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x000405E0 File Offset: 0x0003E7E0
		private IEnumerator CBlockInput()
		{
			EventSystem.current.currentInputModule.enabled = false;
			yield return new WaitForSecondsRealtime(0.3f);
			EventSystem.current.currentInputModule.enabled = true;
			yield break;
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x000405E8 File Offset: 0x0003E7E8
		public void Appear()
		{
			base.StartCoroutine(this.CAppear());
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x000405F7 File Offset: 0x0003E7F7
		private IEnumerator CAppear()
		{
			float t = 0f;
			Vector2 scrollSize = this._scroll.sizeDelta;
			Vector2 maskSize = this._mask.sizeDelta;
			scrollSize.x = this._foldedScrollWidth;
			maskSize.x = this._foldedMaskWidth;
			this._scroll.sizeDelta = scrollSize;
			this._mask.sizeDelta = maskSize;
			yield return new WaitForSecondsRealtime(0.1f);
			while (t < 1f)
			{
				scrollSize.x = EasingFunction.EaseOutCubic(this._foldedScrollWidth, this._scrollWidth, t);
				maskSize.x = EasingFunction.EaseOutCubic(this._foldedMaskWidth, this._maskWidth, t);
				this._scroll.sizeDelta = scrollSize;
				this._mask.sizeDelta = maskSize;
				yield return null;
				t += Time.unscaledDeltaTime * 2f;
			}
			scrollSize.x = this._scrollWidth;
			maskSize.x = this._maskWidth;
			this._scroll.sizeDelta = scrollSize;
			this._mask.sizeDelta = maskSize;
			yield break;
		}

		// Token: 0x040011CD RID: 4557
		[SerializeField]
		private RectTransform _scroll;

		// Token: 0x040011CE RID: 4558
		[SerializeField]
		private RectTransform _mask;

		// Token: 0x040011CF RID: 4559
		[SerializeField]
		private float _scrollWidth;

		// Token: 0x040011D0 RID: 4560
		[SerializeField]
		private float _foldedScrollWidth;

		// Token: 0x040011D1 RID: 4561
		[SerializeField]
		private float _maskWidth;

		// Token: 0x040011D2 RID: 4562
		[SerializeField]
		private float _foldedMaskWidth;
	}
}
