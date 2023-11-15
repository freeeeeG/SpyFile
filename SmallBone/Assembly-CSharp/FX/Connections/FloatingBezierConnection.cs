using System;
using System.Collections;
using UnityEngine;

namespace FX.Connections
{
	// Token: 0x02000297 RID: 663
	[RequireComponent(typeof(BezierCurve))]
	public class FloatingBezierConnection : Connection
	{
		// Token: 0x06000CE5 RID: 3301 RVA: 0x00029E6E File Offset: 0x0002806E
		protected override void Show()
		{
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.CShow());
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00029E89 File Offset: 0x00028089
		protected override void Hide()
		{
			base.StopAllCoroutines();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00029E9D File Offset: 0x0002809D
		private IEnumerator CShow()
		{
			Vector3 startCurrent = base.startPosition;
			Vector3 endCurrent = base.endPosition;
			int middleCount = this._bezierCurve.count - 2;
			Vector2[] middleCurrents = new Vector2[middleCount];
			Vector2[] randomOffsets = new Vector2[middleCount];
			float floatingTime = 0f;
			for (int i = 0; i < middleCount; i++)
			{
				middleCurrents[i] = this.GetMiddlePosition(i);
				randomOffsets[i] = new Vector2(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f));
			}
			while (!this.lostConnection)
			{
				startCurrent = this.GetTrackingVector(startCurrent, base.startPosition, this._trackingSpeed);
				endCurrent = this.GetTrackingVector(endCurrent, base.endPosition, this._trackingSpeed);
				for (int j = 0; j < middleCount; j++)
				{
					Vector2 vector = this.GetMiddlePosition(j);
					vector.y += this._middleYOffset;
					vector += this.GetFloatingOffset(floatingTime, randomOffsets[j]);
					middleCurrents[j] = this.GetTrackingVector(middleCurrents[j], vector, this._middleTrackingSpeed);
				}
				floatingTime += this._floatingSpeed * 360f * Chronometer.global.deltaTime;
				this._bezierCurve.SetStart(startCurrent);
				this._bezierCurve.SetEnd(endCurrent);
				for (int k = 0; k < middleCount; k++)
				{
					this._bezierCurve.SetVector(k + 1, middleCurrents[k]);
				}
				this._bezierCurve.UpdateCurve();
				yield return null;
			}
			base.Disconnect();
			yield break;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x00029EAC File Offset: 0x000280AC
		private Vector2 GetTrackingVector(Vector2 current, Vector2 target, float speed)
		{
			Vector2 b = (target - current) * Mathf.Min(1f, Chronometer.global.deltaTime * 6f * speed);
			return current + b;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00029EEC File Offset: 0x000280EC
		private Vector2 GetMiddlePosition(int index)
		{
			index++;
			float t = (float)index / (float)(this._bezierCurve.count - 1);
			return Vector2.Lerp(base.startPosition, base.endPosition, t);
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00029F2C File Offset: 0x0002812C
		private Vector2 GetFloatingOffset(float floatingTime, Vector2 randomOffset)
		{
			Vector2 result;
			result.x = this._floatingRange * Mathf.Sin((floatingTime + randomOffset.x) * 0.017453292f);
			result.y = this._floatingRange * Mathf.Sin((floatingTime + randomOffset.y) * 0.017453292f);
			return result;
		}

		// Token: 0x04000B0A RID: 2826
		[SerializeField]
		[GetComponent]
		private BezierCurve _bezierCurve;

		// Token: 0x04000B0B RID: 2827
		[SerializeField]
		private float _middleYOffset = 5f;

		// Token: 0x04000B0C RID: 2828
		[SerializeField]
		private float _floatingRange = 5f;

		// Token: 0x04000B0D RID: 2829
		[SerializeField]
		private float _floatingSpeed = 0.2f;

		// Token: 0x04000B0E RID: 2830
		[SerializeField]
		private float _trackingSpeed = 1.3f;

		// Token: 0x04000B0F RID: 2831
		[SerializeField]
		private float _middleTrackingSpeed = 0.016f;

		// Token: 0x04000B10 RID: 2832
		private const float _speedCorrection = 6f;
	}
}
