using System;
using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Hero.LightSwords
{
	// Token: 0x02001288 RID: 4744
	public class LightSwordStuck : MonoBehaviour
	{
		// Token: 0x06005E18 RID: 24088 RVA: 0x00114B59 File Offset: 0x00112D59
		private void Awake()
		{
			short currentSortingOrder = LightSwordStuck._currentSortingOrder;
			LightSwordStuck._currentSortingOrder = currentSortingOrder + 1;
			this._order = (int)currentSortingOrder;
			this._onStuck.Initialize();
		}

		// Token: 0x06005E19 RID: 24089 RVA: 0x00114B7C File Offset: 0x00112D7C
		public void OnStuck(Character owner, Vector2 position, float angle)
		{
			this.Hide();
			int num = this.Evaluate(angle);
			this._body = this._bodyContainer[num];
			this._trailEffectTransform.SetParent(this._trailEffectTransformContainer[num]);
			this._body.sortingOrder = this._order;
			base.transform.position = position;
			this._onStuck.gameObject.SetActive(true);
			this._onStuck.Run(owner);
			this.Show();
		}

		// Token: 0x06005E1A RID: 24090 RVA: 0x00114BFD File Offset: 0x00112DFD
		public void Despawn()
		{
			this.Hide();
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x00114C05 File Offset: 0x00112E05
		public void Sign()
		{
			base.StartCoroutine(this.CEaseColor());
		}

		// Token: 0x06005E1C RID: 24092 RVA: 0x00114C14 File Offset: 0x00112E14
		private int Evaluate(float angle)
		{
			angle -= 180f;
			int result = 0;
			float num = float.MaxValue;
			for (int i = 0; i < LightSwordStuck._points.Length; i++)
			{
				float num2 = Mathf.Abs(angle - (float)LightSwordStuck._points[i]);
				if (num2 < num)
				{
					result = i;
					num = num2;
				}
			}
			return result;
		}

		// Token: 0x06005E1D RID: 24093 RVA: 0x00114C5D File Offset: 0x00112E5D
		private void Show()
		{
			if (this._body != null)
			{
				this._body.gameObject.SetActive(true);
			}
		}

		// Token: 0x06005E1E RID: 24094 RVA: 0x00114C7E File Offset: 0x00112E7E
		private void Hide()
		{
			if (this._body != null)
			{
				this._body.color = this._startColor;
				this._body.gameObject.SetActive(false);
			}
		}

		// Token: 0x06005E1F RID: 24095 RVA: 0x00114CB0 File Offset: 0x00112EB0
		private IEnumerator CEaseColor()
		{
			float duration = this._hitColorCurve.duration;
			for (float time = 0f; time < duration; time += Chronometer.global.deltaTime)
			{
				this._body.color = Color.Lerp(this._startColor, this._endColor, this._hitColorCurve.Evaluate(time));
				yield return null;
			}
			this._body.color = this._endColor;
			yield break;
		}

		// Token: 0x04004B96 RID: 19350
		private static short _currentSortingOrder = 1;

		// Token: 0x04004B97 RID: 19351
		private static int[] _points = new int[]
		{
			45,
			75,
			90,
			105,
			135
		};

		// Token: 0x04004B98 RID: 19352
		[SerializeField]
		private SpriteRenderer[] _bodyContainer;

		// Token: 0x04004B99 RID: 19353
		[SerializeField]
		private Transform[] _trailEffectTransformContainer;

		// Token: 0x04004B9A RID: 19354
		[SerializeField]
		private Transform _trailEffectTransform;

		// Token: 0x04004B9B RID: 19355
		private SpriteRenderer _body;

		// Token: 0x04004B9C RID: 19356
		[SerializeField]
		private Color _startColor;

		// Token: 0x04004B9D RID: 19357
		[SerializeField]
		private Color _endColor;

		// Token: 0x04004B9E RID: 19358
		[SerializeField]
		private Curve _hitColorCurve;

		// Token: 0x04004B9F RID: 19359
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onStuck;

		// Token: 0x04004BA0 RID: 19360
		private int _order;
	}
}
