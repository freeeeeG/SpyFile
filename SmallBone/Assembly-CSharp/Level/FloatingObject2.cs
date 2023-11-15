using System;
using System.Collections;
using UnityEngine;

namespace Level
{
	// Token: 0x02000489 RID: 1161
	public class FloatingObject2 : MonoBehaviour
	{
		// Token: 0x06001613 RID: 5651 RVA: 0x000452CA File Offset: 0x000434CA
		private void OnEnable()
		{
			if (this._startOnEnable)
			{
				base.StartCoroutine(this.CFloat());
			}
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x000452E1 File Offset: 0x000434E1
		public void Float()
		{
			base.StartCoroutine(this.CFloat());
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x000452F0 File Offset: 0x000434F0
		private IEnumerator CFloat()
		{
			float time = 0f;
			float amountStart = this._randomize ? UnityEngine.Random.Range(0f, 1f) : 0f;
			for (;;)
			{
				yield return null;
				float amount = amountStart + time * this._speed;
				this.UpdateTransform(amount, this._translateDegree.Evaluate(time));
				time += Chronometer.global.deltaTime;
			}
			yield break;
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00045300 File Offset: 0x00043500
		private void UpdateTransform(float amount, float degree)
		{
			float sinAmplitudeIn = this.GetSinAmplitudeIn(amount);
			float cosAmplitudeIn = this.GetCosAmplitudeIn(amount);
			float y = this._vertical * degree * sinAmplitudeIn;
			float z = this._rotation * degree * cosAmplitudeIn;
			base.transform.localPosition = new Vector3(0f, y, 0f);
			base.transform.localEulerAngles = new Vector3(0f, 0f, z);
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00044FEE File Offset: 0x000431EE
		private float GetSinAmplitudeIn(float point)
		{
			return Mathf.Sin(point * 360f * 0.017453292f);
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x00045369 File Offset: 0x00043569
		private float GetCosAmplitudeIn(float point)
		{
			return Mathf.Cos(point * 360f * 0.017453292f);
		}

		// Token: 0x04001358 RID: 4952
		[SerializeField]
		[Header("Initial Value")]
		private bool _startOnEnable = true;

		// Token: 0x04001359 RID: 4953
		[SerializeField]
		private float _vertical;

		// Token: 0x0400135A RID: 4954
		[SerializeField]
		private float _rotation;

		// Token: 0x0400135B RID: 4955
		[SerializeField]
		private Curve _translateDegree;

		// Token: 0x0400135C RID: 4956
		[SerializeField]
		private bool _randomize;

		// Token: 0x0400135D RID: 4957
		[SerializeField]
		private float _speed = 1f;
	}
}
