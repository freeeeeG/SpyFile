using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001AB RID: 427
[RequireComponent(typeof(CanvasRenderer))]
public class UIOutline : MaskableGraphic
{
	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0002CA78 File Offset: 0x0002AC78
	public override Texture mainTexture
	{
		get
		{
			if (!(this.m_Texture == null))
			{
				return this.m_Texture;
			}
			return Graphic.s_WhiteTexture;
		}
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x0002CA94 File Offset: 0x0002AC94
	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		this.SetVerticesDirty();
		this.SetMaterialDirty();
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x0002CAA8 File Offset: 0x0002ACA8
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
		Rect rect = base.rectTransform.rect;
		float num = Mathf.Min(Mathf.Min(rect.width, rect.height) / 2f, this._cornerRadius);
		base.rectTransform.GetLocalCorners(this._corners);
		this._corners[0] += new Vector3(num, num, 0f);
		this._corners[1] += new Vector3(num, -num, 0f);
		this._corners[2] += new Vector3(-num, -num, 0f);
		this._corners[3] += new Vector3(-num, num, 0f);
		float num2 = this._corners[1].y - this._corners[0].y;
		float num3 = this._corners[2].x - this._corners[1].x;
		float[] array = new float[]
		{
			num2,
			num3,
			num2,
			num3
		};
		float num4 = 6.2831855f * Mathf.Lerp(num, num + this._outlineWidth, this._mappingBias);
		float num5 = num2 * 2f + num3 * 2f + num4;
		float num6 = num4 / 4f / (float)this._cornerSegments;
		UIVertex item = new UIVertex
		{
			color = this.color
		};
		this._verts.Clear();
		float num7 = 0f;
		for (int i = 0; i < 4; i++)
		{
			Vector3 a = this._corners[i];
			for (int j = 0; j < this._cornerSegments + 1; j++)
			{
				float num8 = (float)j / (float)this._cornerSegments * 3.1415927f / 2f + 1.5707964f - 3.1415927f * (float)i * 1.5f;
				Vector3 a2 = new Vector3(Mathf.Cos(-num8), Mathf.Sin(-num8), 0f);
				item.position = a + a2 * num;
				item.uv0 = new Vector2(num7, 0f);
				this._verts.Add(item);
				item.position = a + a2 * (num + this._outlineWidth);
				item.uv0 = new Vector2(num7, 1f);
				this._verts.Add(item);
				if (this._fillCenter)
				{
					item.position = rect.center;
					item.uv0 = new Vector2(num7, 0f);
					this._verts.Add(item);
				}
				if (j < this._cornerSegments)
				{
					num7 += num6 / num5;
				}
				else
				{
					num7 += array[i] / num5;
				}
			}
		}
		item = this._verts[0];
		item.uv0 = new Vector2(1f, 0f);
		this._verts.Add(item);
		item = this._verts[1];
		item.uv0 = new Vector2(1f, 1f);
		this._verts.Add(item);
		if (this._fillCenter)
		{
			item = this._verts[2];
			item.uv0 = new Vector2(1f, 1f);
			this._verts.Add(item);
		}
		foreach (UIVertex v in this._verts)
		{
			vh.AddVert(v);
		}
		if (this._fillCenter)
		{
			for (int k = 0; k < vh.currentVertCount - 3; k += 3)
			{
				vh.AddTriangle(k, k + 1, k + 4);
				vh.AddTriangle(k, k + 4, k + 3);
				vh.AddTriangle(k + 2, k, k + 3);
				vh.AddTriangle(k + 2, k + 3, k + 5);
			}
			return;
		}
		for (int l = 0; l < vh.currentVertCount - 2; l += 2)
		{
			vh.AddTriangle(l, l + 1, l + 3);
			vh.AddTriangle(l, l + 3, l + 2);
		}
	}

	// Token: 0x04000920 RID: 2336
	[SerializeField]
	private Texture m_Texture;

	// Token: 0x04000921 RID: 2337
	[SerializeField]
	[Range(0f, 500f)]
	private float _outlineWidth = 100f;

	// Token: 0x04000922 RID: 2338
	[SerializeField]
	[Range(0f, 500f)]
	private float _cornerRadius = 50f;

	// Token: 0x04000923 RID: 2339
	[SerializeField]
	[Range(1f, 20f)]
	private int _cornerSegments = 1;

	// Token: 0x04000924 RID: 2340
	[SerializeField]
	[Range(0f, 1f)]
	private float _mappingBias = 0.5f;

	// Token: 0x04000925 RID: 2341
	[SerializeField]
	private bool _fillCenter;

	// Token: 0x04000926 RID: 2342
	private Vector3[] _corners = new Vector3[4];

	// Token: 0x04000927 RID: 2343
	private List<UIVertex> _verts = new List<UIVertex>();
}
