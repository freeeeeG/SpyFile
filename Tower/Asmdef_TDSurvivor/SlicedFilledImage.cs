using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x020001A5 RID: 421
[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/Sliced Filled Image", 11)]
public class SlicedFilledImage : MaskableGraphic, ISerializationCallbackReceiver, ILayoutElement, ICanvasRaycastFilter
{
	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06000B36 RID: 2870 RVA: 0x0002B73B File Offset: 0x0002993B
	// (set) Token: 0x06000B37 RID: 2871 RVA: 0x0002B743 File Offset: 0x00029943
	public Sprite sprite
	{
		get
		{
			return this.m_Sprite;
		}
		set
		{
			if (SlicedFilledImage.SetPropertyUtility.SetClass<Sprite>(ref this.m_Sprite, value))
			{
				this.SetAllDirty();
				this.TrackImage();
			}
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06000B38 RID: 2872 RVA: 0x0002B75F File Offset: 0x0002995F
	// (set) Token: 0x06000B39 RID: 2873 RVA: 0x0002B767 File Offset: 0x00029967
	public SlicedFilledImage.FillDirection fillDirection
	{
		get
		{
			return this.m_FillDirection;
		}
		set
		{
			if (SlicedFilledImage.SetPropertyUtility.SetStruct<SlicedFilledImage.FillDirection>(ref this.m_FillDirection, value))
			{
				this.SetVerticesDirty();
			}
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000B3A RID: 2874 RVA: 0x0002B77D File Offset: 0x0002997D
	// (set) Token: 0x06000B3B RID: 2875 RVA: 0x0002B785 File Offset: 0x00029985
	public float fillAmount
	{
		get
		{
			return this.m_FillAmount;
		}
		set
		{
			if (SlicedFilledImage.SetPropertyUtility.SetStruct<float>(ref this.m_FillAmount, Mathf.Clamp01(value)))
			{
				this.SetVerticesDirty();
			}
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0002B7A0 File Offset: 0x000299A0
	// (set) Token: 0x06000B3D RID: 2877 RVA: 0x0002B7A8 File Offset: 0x000299A8
	public bool fillCenter
	{
		get
		{
			return this.m_FillCenter;
		}
		set
		{
			if (SlicedFilledImage.SetPropertyUtility.SetStruct<bool>(ref this.m_FillCenter, value))
			{
				this.SetVerticesDirty();
			}
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000B3E RID: 2878 RVA: 0x0002B7BE File Offset: 0x000299BE
	// (set) Token: 0x06000B3F RID: 2879 RVA: 0x0002B7C6 File Offset: 0x000299C6
	public float pixelsPerUnitMultiplier
	{
		get
		{
			return this.m_PixelsPerUnitMultiplier;
		}
		set
		{
			this.m_PixelsPerUnitMultiplier = Mathf.Max(0.01f, value);
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06000B40 RID: 2880 RVA: 0x0002B7DC File Offset: 0x000299DC
	public float pixelsPerUnit
	{
		get
		{
			float num = 100f;
			if (this.activeSprite)
			{
				num = this.activeSprite.pixelsPerUnit;
			}
			float num2 = 100f;
			if (base.canvas)
			{
				num2 = base.canvas.referencePixelsPerUnit;
			}
			return this.m_PixelsPerUnitMultiplier * num / num2;
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0002B831 File Offset: 0x00029A31
	// (set) Token: 0x06000B42 RID: 2882 RVA: 0x0002B839 File Offset: 0x00029A39
	public Sprite overrideSprite
	{
		get
		{
			return this.activeSprite;
		}
		set
		{
			if (SlicedFilledImage.SetPropertyUtility.SetClass<Sprite>(ref this.m_OverrideSprite, value))
			{
				this.SetAllDirty();
				this.TrackImage();
			}
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0002B855 File Offset: 0x00029A55
	private Sprite activeSprite
	{
		get
		{
			if (!(this.m_OverrideSprite != null))
			{
				return this.m_Sprite;
			}
			return this.m_OverrideSprite;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000B44 RID: 2884 RVA: 0x0002B874 File Offset: 0x00029A74
	public override Texture mainTexture
	{
		get
		{
			if (this.activeSprite != null)
			{
				return this.activeSprite.texture;
			}
			if (!(this.material != null) || !(this.material.mainTexture != null))
			{
				return Graphic.s_WhiteTexture;
			}
			return this.material.mainTexture;
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0002B8D0 File Offset: 0x00029AD0
	public bool hasBorder
	{
		get
		{
			return this.activeSprite != null && this.activeSprite.border.sqrMagnitude > 0f;
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000B46 RID: 2886 RVA: 0x0002B908 File Offset: 0x00029B08
	// (set) Token: 0x06000B47 RID: 2887 RVA: 0x0002B956 File Offset: 0x00029B56
	public override Material material
	{
		get
		{
			if (this.m_Material != null)
			{
				return this.m_Material;
			}
			if (this.activeSprite && this.activeSprite.associatedAlphaSplitTexture != null)
			{
				return Image.defaultETC1GraphicMaterial;
			}
			return this.defaultMaterial;
		}
		set
		{
			base.material = value;
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000B48 RID: 2888 RVA: 0x0002B95F File Offset: 0x00029B5F
	// (set) Token: 0x06000B49 RID: 2889 RVA: 0x0002B967 File Offset: 0x00029B67
	public float alphaHitTestMinimumThreshold { get; set; }

	// Token: 0x06000B4A RID: 2890 RVA: 0x0002B970 File Offset: 0x00029B70
	protected SlicedFilledImage()
	{
		base.useLegacyMeshGeneration = false;
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x0002B99C File Offset: 0x00029B9C
	protected override void OnEnable()
	{
		base.OnEnable();
		this.TrackImage();
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x0002B9AA File Offset: 0x00029BAA
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_Tracked)
		{
			this.UnTrackImage();
		}
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x0002B9C0 File Offset: 0x00029BC0
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		if (this.activeSprite == null)
		{
			base.OnPopulateMesh(vh);
			return;
		}
		this.GenerateSlicedFilledSprite(vh);
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x0002B9E0 File Offset: 0x00029BE0
	protected override void UpdateMaterial()
	{
		base.UpdateMaterial();
		if (this.activeSprite == null)
		{
			base.canvasRenderer.SetAlphaTexture(null);
			return;
		}
		Texture2D associatedAlphaSplitTexture = this.activeSprite.associatedAlphaSplitTexture;
		if (associatedAlphaSplitTexture != null)
		{
			base.canvasRenderer.SetAlphaTexture(associatedAlphaSplitTexture);
		}
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x0002BA30 File Offset: 0x00029C30
	private void GenerateSlicedFilledSprite(VertexHelper vh)
	{
		vh.Clear();
		if (this.m_FillAmount < 0.001f)
		{
			return;
		}
		Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
		Vector4 outerUV = DataUtility.GetOuterUV(this.activeSprite);
		Vector4 vector = DataUtility.GetPadding(this.activeSprite);
		if (!this.hasBorder)
		{
			Vector2 size = this.activeSprite.rect.size;
			int num = Mathf.RoundToInt(size.x);
			int num2 = Mathf.RoundToInt(size.y);
			Vector4 vertices = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * (vector.x / (float)num), pixelAdjustedRect.y + pixelAdjustedRect.height * (vector.y / (float)num2), pixelAdjustedRect.x + pixelAdjustedRect.width * (((float)num - vector.z) / (float)num), pixelAdjustedRect.y + pixelAdjustedRect.height * (((float)num2 - vector.w) / (float)num2));
			this.GenerateFilledSprite(vh, vertices, outerUV, this.m_FillAmount);
			return;
		}
		Vector4 innerUV = DataUtility.GetInnerUV(this.activeSprite);
		Vector4 adjustedBorders = this.GetAdjustedBorders(this.activeSprite.border / this.pixelsPerUnit, pixelAdjustedRect);
		vector /= this.pixelsPerUnit;
		SlicedFilledImage.s_SlicedVertices[0] = new Vector2(vector.x, vector.y);
		SlicedFilledImage.s_SlicedVertices[3] = new Vector2(pixelAdjustedRect.width - vector.z, pixelAdjustedRect.height - vector.w);
		SlicedFilledImage.s_SlicedVertices[1].x = adjustedBorders.x;
		SlicedFilledImage.s_SlicedVertices[1].y = adjustedBorders.y;
		SlicedFilledImage.s_SlicedVertices[2].x = pixelAdjustedRect.width - adjustedBorders.z;
		SlicedFilledImage.s_SlicedVertices[2].y = pixelAdjustedRect.height - adjustedBorders.w;
		for (int i = 0; i < 4; i++)
		{
			Vector2[] array = SlicedFilledImage.s_SlicedVertices;
			int num3 = i;
			array[num3].x = array[num3].x + pixelAdjustedRect.x;
			Vector2[] array2 = SlicedFilledImage.s_SlicedVertices;
			int num4 = i;
			array2[num4].y = array2[num4].y + pixelAdjustedRect.y;
		}
		SlicedFilledImage.s_SlicedUVs[0] = new Vector2(outerUV.x, outerUV.y);
		SlicedFilledImage.s_SlicedUVs[1] = new Vector2(innerUV.x, innerUV.y);
		SlicedFilledImage.s_SlicedUVs[2] = new Vector2(innerUV.z, innerUV.w);
		SlicedFilledImage.s_SlicedUVs[3] = new Vector2(outerUV.z, outerUV.w);
		float num5;
		float num7;
		if (this.m_FillDirection == SlicedFilledImage.FillDirection.Left || this.m_FillDirection == SlicedFilledImage.FillDirection.Right)
		{
			num5 = SlicedFilledImage.s_SlicedVertices[0].x;
			float num6 = SlicedFilledImage.s_SlicedVertices[3].x - SlicedFilledImage.s_SlicedVertices[0].x;
			num7 = ((num6 > 0f) ? (1f / num6) : 1f);
		}
		else
		{
			num5 = SlicedFilledImage.s_SlicedVertices[0].y;
			float num8 = SlicedFilledImage.s_SlicedVertices[3].y - SlicedFilledImage.s_SlicedVertices[0].y;
			num7 = ((num8 > 0f) ? (1f / num8) : 1f);
		}
		for (int j = 0; j < 3; j++)
		{
			int num9 = j + 1;
			for (int k = 0; k < 3; k++)
			{
				if (this.m_FillCenter || j != 1 || k != 1)
				{
					int num10 = k + 1;
					float num11;
					float num12;
					switch (this.m_FillDirection)
					{
					case SlicedFilledImage.FillDirection.Right:
						num11 = (SlicedFilledImage.s_SlicedVertices[j].x - num5) * num7;
						num12 = (SlicedFilledImage.s_SlicedVertices[num9].x - num5) * num7;
						break;
					case SlicedFilledImage.FillDirection.Left:
						num11 = 1f - (SlicedFilledImage.s_SlicedVertices[num9].x - num5) * num7;
						num12 = 1f - (SlicedFilledImage.s_SlicedVertices[j].x - num5) * num7;
						break;
					case SlicedFilledImage.FillDirection.Up:
						num11 = (SlicedFilledImage.s_SlicedVertices[k].y - num5) * num7;
						num12 = (SlicedFilledImage.s_SlicedVertices[num10].y - num5) * num7;
						break;
					case SlicedFilledImage.FillDirection.Down:
						num11 = 1f - (SlicedFilledImage.s_SlicedVertices[num10].y - num5) * num7;
						num12 = 1f - (SlicedFilledImage.s_SlicedVertices[k].y - num5) * num7;
						break;
					default:
						num12 = (num11 = 0f);
						break;
					}
					if (num11 < this.m_FillAmount)
					{
						Vector4 vertices2 = new Vector4(SlicedFilledImage.s_SlicedVertices[j].x, SlicedFilledImage.s_SlicedVertices[k].y, SlicedFilledImage.s_SlicedVertices[num9].x, SlicedFilledImage.s_SlicedVertices[num10].y);
						Vector4 uvs = new Vector4(SlicedFilledImage.s_SlicedUVs[j].x, SlicedFilledImage.s_SlicedUVs[k].y, SlicedFilledImage.s_SlicedUVs[num9].x, SlicedFilledImage.s_SlicedUVs[num10].y);
						float fillAmount = (this.m_FillAmount - num11) / (num12 - num11);
						this.GenerateFilledSprite(vh, vertices2, uvs, fillAmount);
					}
				}
			}
		}
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x0002BFB0 File Offset: 0x0002A1B0
	private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect)
	{
		Rect rect = base.rectTransform.rect;
		for (int i = 0; i <= 1; i++)
		{
			if (rect.size[i] != 0f)
			{
				float num = adjustedRect.size[i] / rect.size[i];
				ref Vector4 ptr = ref border;
				int index = i;
				ptr[index] *= num;
				ptr = ref border;
				index = i + 2;
				ptr[index] *= num;
			}
			float num2 = border[i] + border[i + 2];
			if (adjustedRect.size[i] < num2 && num2 != 0f)
			{
				float num = adjustedRect.size[i] / num2;
				ref Vector4 ptr = ref border;
				int index = i;
				ptr[index] *= num;
				ptr = ref border;
				index = i + 2;
				ptr[index] *= num;
			}
		}
		return border;
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x0002C0CC File Offset: 0x0002A2CC
	private void GenerateFilledSprite(VertexHelper vh, Vector4 vertices, Vector4 uvs, float fillAmount)
	{
		if (this.m_FillAmount < 0.001f)
		{
			return;
		}
		float num = uvs.x;
		float num2 = uvs.y;
		float num3 = uvs.z;
		float num4 = uvs.w;
		if (fillAmount < 1f)
		{
			if (this.m_FillDirection == SlicedFilledImage.FillDirection.Left || this.m_FillDirection == SlicedFilledImage.FillDirection.Right)
			{
				if (this.m_FillDirection == SlicedFilledImage.FillDirection.Left)
				{
					vertices.x = vertices.z - (vertices.z - vertices.x) * fillAmount;
					num = num3 - (num3 - num) * fillAmount;
				}
				else
				{
					vertices.z = vertices.x + (vertices.z - vertices.x) * fillAmount;
					num3 = num + (num3 - num) * fillAmount;
				}
			}
			else if (this.m_FillDirection == SlicedFilledImage.FillDirection.Down)
			{
				vertices.y = vertices.w - (vertices.w - vertices.y) * fillAmount;
				num2 = num4 - (num4 - num2) * fillAmount;
			}
			else
			{
				vertices.w = vertices.y + (vertices.w - vertices.y) * fillAmount;
				num4 = num2 + (num4 - num2) * fillAmount;
			}
		}
		SlicedFilledImage.s_Vertices[0] = new Vector3(vertices.x, vertices.y);
		SlicedFilledImage.s_Vertices[1] = new Vector3(vertices.x, vertices.w);
		SlicedFilledImage.s_Vertices[2] = new Vector3(vertices.z, vertices.w);
		SlicedFilledImage.s_Vertices[3] = new Vector3(vertices.z, vertices.y);
		SlicedFilledImage.s_UVs[0] = new Vector2(num, num2);
		SlicedFilledImage.s_UVs[1] = new Vector2(num, num4);
		SlicedFilledImage.s_UVs[2] = new Vector2(num3, num4);
		SlicedFilledImage.s_UVs[3] = new Vector2(num3, num2);
		int currentVertCount = vh.currentVertCount;
		for (int i = 0; i < 4; i++)
		{
			vh.AddVert(SlicedFilledImage.s_Vertices[i], this.color, SlicedFilledImage.s_UVs[i]);
		}
		vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
		vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000B52 RID: 2898 RVA: 0x0002C2F5 File Offset: 0x0002A4F5
	int ILayoutElement.layoutPriority
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000B53 RID: 2899 RVA: 0x0002C2F8 File Offset: 0x0002A4F8
	float ILayoutElement.minWidth
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000B54 RID: 2900 RVA: 0x0002C2FF File Offset: 0x0002A4FF
	float ILayoutElement.minHeight
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000B55 RID: 2901 RVA: 0x0002C306 File Offset: 0x0002A506
	float ILayoutElement.flexibleWidth
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x06000B56 RID: 2902 RVA: 0x0002C30D File Offset: 0x0002A50D
	float ILayoutElement.flexibleHeight
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06000B57 RID: 2903 RVA: 0x0002C314 File Offset: 0x0002A514
	float ILayoutElement.preferredWidth
	{
		get
		{
			if (this.activeSprite == null)
			{
				return 0f;
			}
			return DataUtility.GetMinSize(this.activeSprite).x / this.pixelsPerUnit;
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06000B58 RID: 2904 RVA: 0x0002C341 File Offset: 0x0002A541
	float ILayoutElement.preferredHeight
	{
		get
		{
			if (this.activeSprite == null)
			{
				return 0f;
			}
			return DataUtility.GetMinSize(this.activeSprite).y / this.pixelsPerUnit;
		}
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x0002C36E File Offset: 0x0002A56E
	void ILayoutElement.CalculateLayoutInputHorizontal()
	{
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x0002C370 File Offset: 0x0002A570
	void ILayoutElement.CalculateLayoutInputVertical()
	{
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x0002C374 File Offset: 0x0002A574
	bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		if (this.alphaHitTestMinimumThreshold <= 0f)
		{
			return true;
		}
		if (this.alphaHitTestMinimumThreshold > 1f)
		{
			return false;
		}
		if (this.activeSprite == null)
		{
			return true;
		}
		Vector2 vector;
		if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, screenPoint, eventCamera, out vector))
		{
			return false;
		}
		Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
		vector.x += base.rectTransform.pivot.x * pixelAdjustedRect.width;
		vector.y += base.rectTransform.pivot.y * pixelAdjustedRect.height;
		Rect rect = this.activeSprite.rect;
		Vector4 border = this.activeSprite.border;
		Vector4 adjustedBorders = this.GetAdjustedBorders(border / this.pixelsPerUnit, pixelAdjustedRect);
		for (int i = 0; i < 2; i++)
		{
			if (vector[i] > adjustedBorders[i])
			{
				if (pixelAdjustedRect.size[i] - vector[i] <= adjustedBorders[i + 2])
				{
					ref Vector2 ptr = ref vector;
					int index = i;
					ptr[index] -= pixelAdjustedRect.size[i] - rect.size[i];
				}
				else
				{
					float t = Mathf.InverseLerp(adjustedBorders[i], pixelAdjustedRect.size[i] - adjustedBorders[i + 2], vector[i]);
					vector[i] = Mathf.Lerp(border[i], rect.size[i] - border[i + 2], t);
				}
			}
		}
		Rect textureRect = this.activeSprite.textureRect;
		Vector2 vector2 = new Vector2(vector.x / textureRect.width, vector.y / textureRect.height);
		float num = Mathf.Lerp(textureRect.x, textureRect.xMax, vector2.x) / (float)this.activeSprite.texture.width;
		float num2 = Mathf.Lerp(textureRect.y, textureRect.yMax, vector2.y) / (float)this.activeSprite.texture.height;
		switch (this.m_FillDirection)
		{
		case SlicedFilledImage.FillDirection.Right:
			if (num > this.m_FillAmount)
			{
				return false;
			}
			break;
		case SlicedFilledImage.FillDirection.Left:
			if (1f - num > this.m_FillAmount)
			{
				return false;
			}
			break;
		case SlicedFilledImage.FillDirection.Up:
			if (num2 > this.m_FillAmount)
			{
				return false;
			}
			break;
		case SlicedFilledImage.FillDirection.Down:
			if (1f - num2 > this.m_FillAmount)
			{
				return false;
			}
			break;
		}
		bool result;
		try
		{
			result = (this.activeSprite.texture.GetPixelBilinear(num, num2).a >= this.alphaHitTestMinimumThreshold);
		}
		catch (UnityException ex)
		{
			Debug.LogError("Using alphaHitTestMinimumThreshold greater than 0 on Image whose sprite texture cannot be read. " + ex.Message + " Also make sure to disable sprite packing for this sprite.", this);
			result = true;
		}
		return result;
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x0002C68C File Offset: 0x0002A88C
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x0002C68E File Offset: 0x0002A88E
	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		this.m_FillAmount = Mathf.Clamp01(this.m_FillAmount);
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x0002C6A4 File Offset: 0x0002A8A4
	private void TrackImage()
	{
		if (this.activeSprite != null && this.activeSprite.texture == null)
		{
			if (!SlicedFilledImage.s_Initialized)
			{
				SpriteAtlasManager.atlasRegistered += SlicedFilledImage.RebuildImage;
				SlicedFilledImage.s_Initialized = true;
			}
			SlicedFilledImage.m_TrackedTexturelessImages.Add(this);
			this.m_Tracked = true;
		}
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x0002C702 File Offset: 0x0002A902
	private void UnTrackImage()
	{
		SlicedFilledImage.m_TrackedTexturelessImages.Remove(this);
		this.m_Tracked = false;
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x0002C718 File Offset: 0x0002A918
	private static void RebuildImage(SpriteAtlas spriteAtlas)
	{
		for (int i = SlicedFilledImage.m_TrackedTexturelessImages.Count - 1; i >= 0; i--)
		{
			SlicedFilledImage slicedFilledImage = SlicedFilledImage.m_TrackedTexturelessImages[i];
			if (spriteAtlas.CanBindTo(slicedFilledImage.activeSprite))
			{
				slicedFilledImage.SetAllDirty();
				SlicedFilledImage.m_TrackedTexturelessImages.RemoveAt(i);
			}
		}
	}

	// Token: 0x04000905 RID: 2309
	private static readonly Vector3[] s_Vertices = new Vector3[4];

	// Token: 0x04000906 RID: 2310
	private static readonly Vector2[] s_UVs = new Vector2[4];

	// Token: 0x04000907 RID: 2311
	private static readonly Vector2[] s_SlicedVertices = new Vector2[4];

	// Token: 0x04000908 RID: 2312
	private static readonly Vector2[] s_SlicedUVs = new Vector2[4];

	// Token: 0x04000909 RID: 2313
	[SerializeField]
	private Sprite m_Sprite;

	// Token: 0x0400090A RID: 2314
	[SerializeField]
	private SlicedFilledImage.FillDirection m_FillDirection;

	// Token: 0x0400090B RID: 2315
	[Range(0f, 1f)]
	[SerializeField]
	private float m_FillAmount = 1f;

	// Token: 0x0400090C RID: 2316
	[SerializeField]
	private bool m_FillCenter = true;

	// Token: 0x0400090D RID: 2317
	[SerializeField]
	private float m_PixelsPerUnitMultiplier = 1f;

	// Token: 0x0400090E RID: 2318
	[NonSerialized]
	private Sprite m_OverrideSprite;

	// Token: 0x04000910 RID: 2320
	private bool m_Tracked;

	// Token: 0x04000911 RID: 2321
	private static List<SlicedFilledImage> m_TrackedTexturelessImages = new List<SlicedFilledImage>();

	// Token: 0x04000912 RID: 2322
	private static bool s_Initialized;

	// Token: 0x020002BE RID: 702
	private static class SetPropertyUtility
	{
		// Token: 0x06000FA4 RID: 4004 RVA: 0x0003959B File Offset: 0x0003779B
		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x000395BC File Offset: 0x000377BC
		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}
	}

	// Token: 0x020002BF RID: 703
	public enum FillDirection
	{
		// Token: 0x04000CCC RID: 3276
		Right,
		// Token: 0x04000CCD RID: 3277
		Left,
		// Token: 0x04000CCE RID: 3278
		Up,
		// Token: 0x04000CCF RID: 3279
		Down
	}
}
