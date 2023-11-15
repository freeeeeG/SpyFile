using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
[Serializable]
public class LTRect
{
	// Token: 0x06000323 RID: 803 RVA: 0x000134C8 File Offset: 0x000116C8
	public LTRect()
	{
		this.reset();
		this.rotateEnabled = (this.alphaEnabled = true);
		this._rect = new Rect(0f, 0f, 1f, 1f);
	}

	// Token: 0x06000324 RID: 804 RVA: 0x0001354C File Offset: 0x0001174C
	public LTRect(Rect rect)
	{
		this._rect = rect;
		this.reset();
	}

	// Token: 0x06000325 RID: 805 RVA: 0x000135A8 File Offset: 0x000117A8
	public LTRect(float x, float y, float width, float height)
	{
		this._rect = new Rect(x, y, width, height);
		this.alpha = 1f;
		this.rotation = 0f;
		this.rotateEnabled = (this.alphaEnabled = false);
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00013630 File Offset: 0x00011830
	public LTRect(float x, float y, float width, float height, float alpha)
	{
		this._rect = new Rect(x, y, width, height);
		this.alpha = alpha;
		this.rotation = 0f;
		this.rotateEnabled = (this.alphaEnabled = false);
	}

	// Token: 0x06000327 RID: 807 RVA: 0x000136B4 File Offset: 0x000118B4
	public LTRect(float x, float y, float width, float height, float alpha, float rotation)
	{
		this._rect = new Rect(x, y, width, height);
		this.alpha = alpha;
		this.rotation = rotation;
		this.rotateEnabled = (this.alphaEnabled = false);
		if (rotation != 0f)
		{
			this.rotateEnabled = true;
			this.resetForRotation();
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000328 RID: 808 RVA: 0x00013749 File Offset: 0x00011949
	public bool hasInitiliazed
	{
		get
		{
			return this._id != -1;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x06000329 RID: 809 RVA: 0x00013757 File Offset: 0x00011957
	public int id
	{
		get
		{
			return this._id | this.counter << 16;
		}
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00013769 File Offset: 0x00011969
	public void setId(int id, int counter)
	{
		this._id = id;
		this.counter = counter;
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0001377C File Offset: 0x0001197C
	public void reset()
	{
		this.alpha = 1f;
		this.rotation = 0f;
		this.rotateEnabled = (this.alphaEnabled = false);
		this.margin = Vector2.zero;
		this.sizeByHeight = false;
		this.useColor = false;
	}

	// Token: 0x0600032C RID: 812 RVA: 0x000137C8 File Offset: 0x000119C8
	public void resetForRotation()
	{
		Vector3 vector = new Vector3(GUI.matrix[0, 0], GUI.matrix[1, 1], GUI.matrix[2, 2]);
		if (this.pivot == Vector2.zero)
		{
			this.pivot = new Vector2((this._rect.x + this._rect.width * 0.5f) * vector.x + GUI.matrix[0, 3], (this._rect.y + this._rect.height * 0.5f) * vector.y + GUI.matrix[1, 3]);
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x0600032D RID: 813 RVA: 0x0001388E File Offset: 0x00011A8E
	// (set) Token: 0x0600032E RID: 814 RVA: 0x0001389B File Offset: 0x00011A9B
	public float x
	{
		get
		{
			return this._rect.x;
		}
		set
		{
			this._rect.x = value;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x0600032F RID: 815 RVA: 0x000138A9 File Offset: 0x00011AA9
	// (set) Token: 0x06000330 RID: 816 RVA: 0x000138B6 File Offset: 0x00011AB6
	public float y
	{
		get
		{
			return this._rect.y;
		}
		set
		{
			this._rect.y = value;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000331 RID: 817 RVA: 0x000138C4 File Offset: 0x00011AC4
	// (set) Token: 0x06000332 RID: 818 RVA: 0x000138D1 File Offset: 0x00011AD1
	public float width
	{
		get
		{
			return this._rect.width;
		}
		set
		{
			this._rect.width = value;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000333 RID: 819 RVA: 0x000138DF File Offset: 0x00011ADF
	// (set) Token: 0x06000334 RID: 820 RVA: 0x000138EC File Offset: 0x00011AEC
	public float height
	{
		get
		{
			return this._rect.height;
		}
		set
		{
			this._rect.height = value;
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000335 RID: 821 RVA: 0x000138FC File Offset: 0x00011AFC
	// (set) Token: 0x06000336 RID: 822 RVA: 0x00013A0D File Offset: 0x00011C0D
	public Rect rect
	{
		get
		{
			if (LTRect.colorTouched)
			{
				LTRect.colorTouched = false;
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
			}
			if (this.rotateEnabled)
			{
				if (this.rotateFinished)
				{
					this.rotateFinished = false;
					this.rotateEnabled = false;
					this.pivot = Vector2.zero;
				}
				else
				{
					GUIUtility.RotateAroundPivot(this.rotation, this.pivot);
				}
			}
			if (this.alphaEnabled)
			{
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alpha);
				LTRect.colorTouched = true;
			}
			if (this.fontScaleToFit)
			{
				if (this.useSimpleScale)
				{
					this.style.fontSize = (int)(this._rect.height * this.relativeRect.height);
				}
				else
				{
					this.style.fontSize = (int)this._rect.height;
				}
			}
			return this._rect;
		}
		set
		{
			this._rect = value;
		}
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00013A16 File Offset: 0x00011C16
	public LTRect setStyle(GUIStyle style)
	{
		this.style = style;
		return this;
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00013A20 File Offset: 0x00011C20
	public LTRect setFontScaleToFit(bool fontScaleToFit)
	{
		this.fontScaleToFit = fontScaleToFit;
		return this;
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00013A2A File Offset: 0x00011C2A
	public LTRect setColor(Color color)
	{
		this.color = color;
		this.useColor = true;
		return this;
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00013A3B File Offset: 0x00011C3B
	public LTRect setAlpha(float alpha)
	{
		this.alpha = alpha;
		return this;
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00013A45 File Offset: 0x00011C45
	public LTRect setLabel(string str)
	{
		this.labelStr = str;
		return this;
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00013A4F File Offset: 0x00011C4F
	public LTRect setUseSimpleScale(bool useSimpleScale, Rect relativeRect)
	{
		this.useSimpleScale = useSimpleScale;
		this.relativeRect = relativeRect;
		return this;
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00013A60 File Offset: 0x00011C60
	public LTRect setUseSimpleScale(bool useSimpleScale)
	{
		this.useSimpleScale = useSimpleScale;
		this.relativeRect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		return this;
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00013A8B File Offset: 0x00011C8B
	public LTRect setSizeByHeight(bool sizeByHeight)
	{
		this.sizeByHeight = sizeByHeight;
		return this;
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00013A98 File Offset: 0x00011C98
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"x:",
			this._rect.x,
			" y:",
			this._rect.y,
			" width:",
			this._rect.width,
			" height:",
			this._rect.height
		});
	}

	// Token: 0x040001A7 RID: 423
	public Rect _rect;

	// Token: 0x040001A8 RID: 424
	public float alpha = 1f;

	// Token: 0x040001A9 RID: 425
	public float rotation;

	// Token: 0x040001AA RID: 426
	public Vector2 pivot;

	// Token: 0x040001AB RID: 427
	public Vector2 margin;

	// Token: 0x040001AC RID: 428
	public Rect relativeRect = new Rect(0f, 0f, float.PositiveInfinity, float.PositiveInfinity);

	// Token: 0x040001AD RID: 429
	public bool rotateEnabled;

	// Token: 0x040001AE RID: 430
	[HideInInspector]
	public bool rotateFinished;

	// Token: 0x040001AF RID: 431
	public bool alphaEnabled;

	// Token: 0x040001B0 RID: 432
	public string labelStr;

	// Token: 0x040001B1 RID: 433
	public LTGUI.Element_Type type;

	// Token: 0x040001B2 RID: 434
	public GUIStyle style;

	// Token: 0x040001B3 RID: 435
	public bool useColor;

	// Token: 0x040001B4 RID: 436
	public Color color = Color.white;

	// Token: 0x040001B5 RID: 437
	public bool fontScaleToFit;

	// Token: 0x040001B6 RID: 438
	public bool useSimpleScale;

	// Token: 0x040001B7 RID: 439
	public bool sizeByHeight;

	// Token: 0x040001B8 RID: 440
	public Texture texture;

	// Token: 0x040001B9 RID: 441
	private int _id = -1;

	// Token: 0x040001BA RID: 442
	[HideInInspector]
	public int counter;

	// Token: 0x040001BB RID: 443
	public static bool colorTouched;
}
