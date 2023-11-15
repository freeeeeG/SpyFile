using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A7F RID: 2687
[ExecuteInEditMode]
public abstract class BaseProgressBarUI : UISubElementContainer
{
	// Token: 0x0600351B RID: 13595 RVA: 0x000F7DAC File Offset: 0x000F61AC
	public void SetSprites(BaseProgressBarUI.ColourChangingImageConfig _background, BaseProgressBarUI.ColourChangingImageConfig _fill)
	{
		this.m_backgroundImageConfig = _background;
		this.m_fillImageConfig = _fill;
	}

	// Token: 0x0600351C RID: 13596 RVA: 0x000F7DBC File Offset: 0x000F61BC
	public void SetNotchs(Sprite[] _notchSprites, float[] _notchPositions, Color _color)
	{
		this.m_notchSprite = _notchSprites;
		this.m_notchPositions = _notchPositions;
		this.m_notchColor = _color;
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x0600351D RID: 13597 RVA: 0x000F7DD3 File Offset: 0x000F61D3
	// (set) Token: 0x0600351E RID: 13598 RVA: 0x000F7DDB File Offset: 0x000F61DB
	public float Value
	{
		get
		{
			return this.m_value;
		}
		set
		{
			this.SetValue(value);
		}
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x0600351F RID: 13599 RVA: 0x000F7DE4 File Offset: 0x000F61E4
	public Image FillImage
	{
		get
		{
			int num = 1;
			if (num < this.m_images.Length)
			{
				return this.m_images[num];
			}
			return null;
		}
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06003520 RID: 13600 RVA: 0x000F7E0C File Offset: 0x000F620C
	public Image CapImage
	{
		get
		{
			int num = 2;
			if (num < this.m_images.Length)
			{
				return this.m_images[num];
			}
			return null;
		}
	}

	// Token: 0x06003521 RID: 13601 RVA: 0x000F7E33 File Offset: 0x000F6233
	private void OnEnable()
	{
		this.UpdateColors();
		this.UpdateFill();
	}

	// Token: 0x06003522 RID: 13602 RVA: 0x000F7E41 File Offset: 0x000F6241
	private void Update()
	{
		this.UpdateColors();
	}

	// Token: 0x06003523 RID: 13603 RVA: 0x000F7E49 File Offset: 0x000F6249
	public void SetValue(float _value)
	{
		this.m_value = Mathf.Clamp01(_value);
		this.UpdateColors();
		this.UpdateFill();
	}

	// Token: 0x06003524 RID: 13604
	protected abstract void UpdateFill();

	// Token: 0x06003525 RID: 13605
	protected abstract Image CreateFillImage(GameObject _rect);

	// Token: 0x06003526 RID: 13606
	protected abstract void PositionNotch(Image _notch, float _position);

	// Token: 0x06003527 RID: 13607 RVA: 0x000F7E64 File Offset: 0x000F6264
	private void UpdateColors()
	{
		for (int i = 0; i < this.m_images.Length; i++)
		{
			if (this.m_images[i] != null)
			{
				this.m_images[i].color = this.GetSequenceColour(this.GetImageConfig((BaseProgressBarUI.ImageType)i).ColorSequence, this.m_value);
			}
		}
	}

	// Token: 0x06003528 RID: 13608 RVA: 0x000F7EC4 File Offset: 0x000F62C4
	private Color GetSequenceColour(BaseProgressBarUI.ColorPropCouple[] _colorSequence, float _propLeft)
	{
		BaseProgressBarUI.ColorPropCouple colorPropCouple = _colorSequence[0];
		BaseProgressBarUI.ColorPropCouple colorPropCouple2 = _colorSequence[0];
		float num = float.MaxValue;
		float num2 = float.MinValue;
		foreach (BaseProgressBarUI.ColorPropCouple colorPropCouple3 in _colorSequence)
		{
			if (colorPropCouple3.Prop >= _propLeft && colorPropCouple3.Prop < num)
			{
				num = colorPropCouple3.Prop;
				colorPropCouple = colorPropCouple3;
			}
			if (colorPropCouple3.Prop <= _propLeft && colorPropCouple3.Prop > num2)
			{
				num2 = colorPropCouple3.Prop;
				colorPropCouple2 = colorPropCouple3;
			}
		}
		if (num - num2 > 0.001f)
		{
			float t = MathUtils.ClampedRemap(_propLeft, num2, num, 0f, 1f);
			return Color.Lerp(colorPropCouple2.StageColor, colorPropCouple.StageColor, t);
		}
		return colorPropCouple2.StageColor;
	}

	// Token: 0x06003529 RID: 13609 RVA: 0x000F7F90 File Offset: 0x000F6390
	private void Awake()
	{
		this.m_images = new Image[3];
		for (int i = 0; i < this.m_images.Length; i++)
		{
			Transform transform = base.transform;
			string str = "ProgressBarUI_";
			BaseProgressBarUI.ImageType imageType = (BaseProgressBarUI.ImageType)i;
			Transform transform2 = transform.FindChildRecursive(str + imageType.ToString());
			if (transform2 != null)
			{
				if (transform2.Find("SubImage") != null)
				{
					transform2 = transform2.Find("SubImage");
				}
				this.m_images[i] = transform2.gameObject.RequireComponent<Image>();
			}
		}
	}

	// Token: 0x0600352A RID: 13610 RVA: 0x000F8028 File Offset: 0x000F6428
	protected override void OnRefreshSubObjectProperties(GameObject _container)
	{
		for (int i = 0; i < this.m_images.Length; i++)
		{
			if (this.m_images[i] != null)
			{
				Image image = this.m_images[i];
				BaseProgressBarUI.ColourChangingImageConfig imageConfig = this.GetImageConfig((BaseProgressBarUI.ImageType)i);
				Color sequenceColour = this.GetSequenceColour(imageConfig.ColorSequence, this.m_value);
				image.color = sequenceColour;
				image.sprite = imageConfig.SourceImage;
				image.type = imageConfig.imageType;
			}
		}
		this.UpdateFill();
	}

	// Token: 0x0600352B RID: 13611 RVA: 0x000F80AC File Offset: 0x000F64AC
	protected override void OnCreateSubObjects(GameObject _container)
	{
		this.m_images = new Image[3];
		for (int i = 0; i < 3; i++)
		{
			Image[] images = this.m_images;
			int num = i;
			BaseProgressBarUI.ImageType imageType = (BaseProgressBarUI.ImageType)i;
			string str = "ProgressBarUI_";
			BaseProgressBarUI.ImageType imageType2 = (BaseProgressBarUI.ImageType)i;
			images[num] = this.CreateImage(_container, imageType, str + imageType2.ToString());
		}
		if (this.m_notchPositions != null)
		{
			this.m_notches = new Image[this.m_notchPositions.Length];
			for (int j = 0; j < this.m_notches.Length; j++)
			{
				this.m_notches[j] = this.CreateNotchImage(_container, "Notch_" + j, j);
			}
		}
	}

	// Token: 0x0600352C RID: 13612 RVA: 0x000F8157 File Offset: 0x000F6557
	private BaseProgressBarUI.ColourChangingImageConfig GetImageConfig(BaseProgressBarUI.ImageType _image)
	{
		if (_image == BaseProgressBarUI.ImageType.Background)
		{
			return this.m_backgroundImageConfig;
		}
		if (_image == BaseProgressBarUI.ImageType.Filled)
		{
			return this.m_fillImageConfig;
		}
		if (_image != BaseProgressBarUI.ImageType.Cap)
		{
			return null;
		}
		return this.m_capImageConfig;
	}

	// Token: 0x0600352D RID: 13613 RVA: 0x000F8188 File Offset: 0x000F6588
	private Image CreateImage(GameObject _parent, BaseProgressBarUI.ImageType _imageType, string _name)
	{
		if (_imageType == BaseProgressBarUI.ImageType.Background)
		{
			GameObject gameObject = GameObjectUtils.CreateOnParent<Image>(_parent, _name);
			gameObject.hideFlags = HideFlags.NotEditable;
			return gameObject.GetComponent<Image>();
		}
		if (_imageType == BaseProgressBarUI.ImageType.Filled)
		{
			GameObject gameObject2 = GameObjectUtils.CreateOnParent<RectTransform>(_parent, _name);
			gameObject2.hideFlags = HideFlags.NotEditable;
			return this.CreateFillImage(gameObject2);
		}
		if (_imageType != BaseProgressBarUI.ImageType.Cap)
		{
			return null;
		}
		GameObject gameObject3 = GameObjectUtils.CreateOnParent<Image>(_parent, _name);
		gameObject3.hideFlags = HideFlags.NotEditable;
		return gameObject3.GetComponent<Image>();
	}

	// Token: 0x0600352E RID: 13614 RVA: 0x000F81F4 File Offset: 0x000F65F4
	private Image CreateNotchImage(GameObject _parent, string _name, int _index)
	{
		Image image = base.CreateImage(_parent, "Notch_" + _index);
		this.PositionNotch(image, this.m_notchPositions[_index]);
		if (this.m_notchSprite[_index] != null)
		{
			image.sprite = this.m_notchSprite[_index];
			image.color = this.m_notchColor;
		}
		return image;
	}

	// Token: 0x04002A96 RID: 10902
	[SerializeField]
	private BaseProgressBarUI.ColourChangingImageConfig m_fillImageConfig = new BaseProgressBarUI.ColourChangingImageConfig();

	// Token: 0x04002A97 RID: 10903
	[SerializeField]
	private BaseProgressBarUI.ColourChangingImageConfig m_capImageConfig = new BaseProgressBarUI.ColourChangingImageConfig();

	// Token: 0x04002A98 RID: 10904
	[SerializeField]
	private BaseProgressBarUI.ColourChangingImageConfig m_backgroundImageConfig = new BaseProgressBarUI.ColourChangingImageConfig();

	// Token: 0x04002A99 RID: 10905
	[SerializeField]
	private Sprite[] m_notchSprite;

	// Token: 0x04002A9A RID: 10906
	[SerializeField]
	private float[] m_notchPositions;

	// Token: 0x04002A9B RID: 10907
	[SerializeField]
	private Color m_notchColor;

	// Token: 0x04002A9C RID: 10908
	[SerializeField]
	[Range(0f, 1f)]
	private float m_value;

	// Token: 0x04002A9D RID: 10909
	protected Image[] m_images = new Image[3];

	// Token: 0x04002A9E RID: 10910
	protected Image[] m_notches = new Image[0];

	// Token: 0x02000A80 RID: 2688
	[Serializable]
	public class ColourChangingImageConfig
	{
		// Token: 0x04002A9F RID: 10911
		public BaseProgressBarUI.ColorPropCouple[] ColorSequence = new BaseProgressBarUI.ColorPropCouple[]
		{
			new BaseProgressBarUI.ColorPropCouple(Color.white, 0f)
		};

		// Token: 0x04002AA0 RID: 10912
		public Sprite SourceImage;

		// Token: 0x04002AA1 RID: 10913
		public Image.Type imageType;
	}

	// Token: 0x02000A81 RID: 2689
	[Serializable]
	public class ColorPropCouple
	{
		// Token: 0x06003530 RID: 13616 RVA: 0x000F827C File Offset: 0x000F667C
		public ColorPropCouple(Color _color, float _prop)
		{
			this.StageColor = _color;
			this.Prop = _prop;
		}

		// Token: 0x04002AA2 RID: 10914
		public Color StageColor;

		// Token: 0x04002AA3 RID: 10915
		public float Prop;
	}

	// Token: 0x02000A82 RID: 2690
	protected enum ImageType
	{
		// Token: 0x04002AA5 RID: 10917
		Background,
		// Token: 0x04002AA6 RID: 10918
		Filled,
		// Token: 0x04002AA7 RID: 10919
		Cap,
		// Token: 0x04002AA8 RID: 10920
		Length
	}
}
