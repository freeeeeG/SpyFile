using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FX
{
	// Token: 0x02000260 RID: 608
	public class Vignette : MonoBehaviour
	{
		// Token: 0x06000BEF RID: 3055 RVA: 0x00020C14 File Offset: 0x0001EE14
		public void Initialize(Color startColor, Color endColor, Curve curve)
		{
			this._rectTransform.localScale = Vector3.one;
			this._rectTransform.localPosition = Vector3.zero;
			this._rectTransform.offsetMax = Vector2.zero;
			this._rectTransform.offsetMin = Vector2.zero;
			base.StartCoroutine(this.CFade(startColor, endColor, curve));
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00020C71 File Offset: 0x0001EE71
		private IEnumerator CFade(Color startColor, Color endColor, Curve curve)
		{
			float duration = curve.duration;
			for (float time = 0f; time < duration; time += Chronometer.global.deltaTime)
			{
				this._image.color = Color.Lerp(startColor, endColor, curve.Evaluate(time));
				yield return null;
			}
			this._poolObject.Despawn();
			yield break;
		}

		// Token: 0x040009F4 RID: 2548
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x040009F5 RID: 2549
		[SerializeField]
		private RectTransform _rectTransform;

		// Token: 0x040009F6 RID: 2550
		[SerializeField]
		private Image _image;
	}
}
