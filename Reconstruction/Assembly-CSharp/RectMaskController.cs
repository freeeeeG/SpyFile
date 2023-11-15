using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000260 RID: 608
public class RectMaskController : MonoBehaviour
{
	// Token: 0x06000F44 RID: 3908 RVA: 0x00028704 File Offset: 0x00026904
	private Vector2 WorldToCanvasPos(Canvas canvas, Vector3 world)
	{
		Vector2 result;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, world, canvas.GetComponent<Camera>(), out result);
		return result;
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00028731 File Offset: 0x00026931
	public void SetTarget(GameObject target, float delayTime)
	{
		if (target != null)
		{
			this.m_Target = target.GetComponent<RectTransform>();
			base.Invoke("RefreshMask", delayTime);
			return;
		}
		this.maskImg.gameObject.SetActive(false);
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x00028766 File Offset: 0x00026966
	private void Start()
	{
		this._material = this.maskImg.material;
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x0002877C File Offset: 0x0002697C
	private void RefreshMask()
	{
		this.maskImg.gameObject.SetActive(true);
		this.m_Target.GetWorldCorners(this._corners);
		this._targetOffsetX = Vector2.Distance(this.WorldToCanvasPos(this.canvas, this._corners[0]), this.WorldToCanvasPos(this.canvas, this._corners[3])) / 2f;
		this._targetOffsetY = Vector2.Distance(this.WorldToCanvasPos(this.canvas, this._corners[0]), this.WorldToCanvasPos(this.canvas, this._corners[1])) / 2f;
		float x = this._corners[0].x + (this._corners[3].x - this._corners[0].x) / 2f;
		float y = this._corners[0].y + (this._corners[1].y - this._corners[0].y) / 2f;
		Vector3 world = new Vector3(x, y, 0f);
		Vector2 vector = this.WorldToCanvasPos(this.canvas, world);
		Vector4 value = new Vector4(vector.x, vector.y, 0f, 0f);
		this._material.SetVector("_Center", value);
		RectTransform rectTransform = this.canvas.transform as RectTransform;
		if (rectTransform != null)
		{
			rectTransform.GetWorldCorners(this._corners);
			for (int i = 0; i < this._corners.Length; i++)
			{
				if (i % 2 == 0)
				{
					this._currentOffsetX = Mathf.Max(Vector3.Distance(this.WorldToCanvasPos(this.canvas, this._corners[i]), vector), this._currentOffsetX);
				}
				else
				{
					this._currentOffsetY = Mathf.Max(Vector3.Distance(this.WorldToCanvasPos(this.canvas, this._corners[i]), vector), this._currentOffsetY);
				}
			}
		}
		this._material.SetFloat("_SliderX", this._currentOffsetX);
		this._material.SetFloat("_SliderY", this._currentOffsetY);
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x000289E4 File Offset: 0x00026BE4
	private void Update()
	{
		float num = Mathf.SmoothDamp(this._currentOffsetX, this._targetOffsetX, ref this._shrinkVelocityX, this._shrinkTime);
		float num2 = Mathf.SmoothDamp(this._currentOffsetY, this._targetOffsetY, ref this._shrinkVelocityY, this._shrinkTime);
		if (!Mathf.Approximately(num, this._currentOffsetX))
		{
			this._currentOffsetX = num;
			this._material.SetFloat("_SliderX", this._currentOffsetX);
		}
		if (!Mathf.Approximately(num2, this._currentOffsetY))
		{
			this._currentOffsetY = num2;
			this._material.SetFloat("_SliderY", this._currentOffsetY);
		}
	}

	// Token: 0x040007A0 RID: 1952
	[SerializeField]
	private Canvas canvas;

	// Token: 0x040007A1 RID: 1953
	public RectTransform m_Target;

	// Token: 0x040007A2 RID: 1954
	private Vector3[] _corners = new Vector3[4];

	// Token: 0x040007A3 RID: 1955
	private Vector4 _center;

	// Token: 0x040007A4 RID: 1956
	private float _targetOffsetX;

	// Token: 0x040007A5 RID: 1957
	private float _targetOffsetY;

	// Token: 0x040007A6 RID: 1958
	[SerializeField]
	private Image maskImg;

	// Token: 0x040007A7 RID: 1959
	private Material _material;

	// Token: 0x040007A8 RID: 1960
	private float _currentOffsetX;

	// Token: 0x040007A9 RID: 1961
	private float _currentOffsetY;

	// Token: 0x040007AA RID: 1962
	private float _shrinkTime = 0.5f;

	// Token: 0x040007AB RID: 1963
	private float _shrinkVelocityX;

	// Token: 0x040007AC RID: 1964
	private float _shrinkVelocityY;
}
