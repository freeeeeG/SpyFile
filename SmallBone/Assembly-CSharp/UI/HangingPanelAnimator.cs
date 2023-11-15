using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003A3 RID: 931
	public class HangingPanelAnimator : MonoBehaviour
	{
		// Token: 0x06001121 RID: 4385 RVA: 0x00032CA4 File Offset: 0x00030EA4
		private void Awake()
		{
			this._appearedPosition = this._container.transform.localPosition;
			this._disappearedPosition = this._appearedPosition;
			this._disappearedPosition.y = this._disappearedPosition.y + this._backgroundImage.rectTransform.sizeDelta.y * this._backgroundImage.rectTransform.localScale.y;
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x00032D12 File Offset: 0x00030F12
		private void OnEnable()
		{
			if (this._startOnEnable)
			{
				this.Appear();
			}
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00032D22 File Offset: 0x00030F22
		private IEnumerator CEasePosition(EasingFunction.Method method, Transform transform, Vector2 from, Vector2 to, float speed = 1f)
		{
			float t = 0f;
			EasingFunction.Function easingFunction = EasingFunction.GetEasingFunction(method);
			this._container.transform.localPosition = from;
			while (t < 1f)
			{
				Vector2 v;
				v.x = easingFunction(from.x, to.x, t);
				v.y = easingFunction(from.y, to.y, t);
				this._container.transform.localPosition = v;
				yield return null;
				t += Time.unscaledDeltaTime * speed;
			}
			this._container.transform.localPosition = to;
			yield break;
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00032D4F File Offset: 0x00030F4F
		public void Appear()
		{
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.CAppear());
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x00032D6A File Offset: 0x00030F6A
		public void Disappear()
		{
			base.StartCoroutine(this.CDisappear());
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00032D79 File Offset: 0x00030F79
		private IEnumerator CAppear()
		{
			this._container.transform.localPosition = this._disappearedPosition;
			while (LetterBox.instance.visible)
			{
				yield return null;
			}
			yield return this.CEasePosition(EasingFunction.Method.EaseOutBounce, this._container.transform, this._disappearedPosition, this._appearedPosition, 0.5f);
			yield break;
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00032D88 File Offset: 0x00030F88
		private IEnumerator CDisappear()
		{
			this._container.transform.localPosition = this._appearedPosition;
			while (LetterBox.instance.visible)
			{
				yield return null;
			}
			yield return this.CEasePosition(EasingFunction.Method.EaseOutQuad, this._container.transform, this._appearedPosition, this._disappearedPosition, 1f);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04000E1A RID: 3610
		[SerializeField]
		private Image _backgroundImage;

		// Token: 0x04000E1B RID: 3611
		[SerializeField]
		private GameObject _container;

		// Token: 0x04000E1C RID: 3612
		[SerializeField]
		private bool _startOnEnable;

		// Token: 0x04000E1D RID: 3613
		private Vector2 _appearedPosition;

		// Token: 0x04000E1E RID: 3614
		private Vector2 _disappearedPosition;
	}
}
