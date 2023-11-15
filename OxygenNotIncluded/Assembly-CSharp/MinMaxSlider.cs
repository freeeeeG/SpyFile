using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C99 RID: 3225
[AddComponentMenu("KMonoBehaviour/scripts/MinMaxSlider")]
public class MinMaxSlider : KMonoBehaviour
{
	// Token: 0x170006FB RID: 1787
	// (get) Token: 0x060066B1 RID: 26289 RVA: 0x0026494D File Offset: 0x00262B4D
	// (set) Token: 0x060066B2 RID: 26290 RVA: 0x00264955 File Offset: 0x00262B55
	public MinMaxSlider.Mode mode { get; private set; }

	// Token: 0x060066B3 RID: 26291 RVA: 0x00264960 File Offset: 0x00262B60
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ToolTip component = base.transform.parent.gameObject.GetComponent<ToolTip>();
		if (component != null)
		{
			UnityEngine.Object.DestroyImmediate(this.toolTip);
			this.toolTip = component;
		}
		this.minSlider.value = this.currentMinValue;
		this.maxSlider.value = this.currentMaxValue;
		this.minSlider.interactable = this.interactable;
		this.maxSlider.interactable = this.interactable;
		this.minSlider.maxValue = this.maxLimit;
		this.maxSlider.maxValue = this.maxLimit;
		this.minSlider.minValue = this.minLimit;
		this.maxSlider.minValue = this.minLimit;
		this.minSlider.direction = (this.maxSlider.direction = this.direction);
		if (this.isOverPowered != null)
		{
			this.isOverPowered.enabled = false;
		}
		this.minSlider.gameObject.SetActive(false);
		if (this.mode != MinMaxSlider.Mode.Single)
		{
			this.minSlider.gameObject.SetActive(true);
		}
		if (this.extraSlider != null)
		{
			this.extraSlider.value = this.currentExtraValue;
			this.extraSlider.wholeNumbers = (this.minSlider.wholeNumbers = (this.maxSlider.wholeNumbers = this.wholeNumbers));
			this.extraSlider.direction = this.direction;
			this.extraSlider.interactable = this.interactable;
			this.extraSlider.maxValue = this.maxLimit;
			this.extraSlider.minValue = this.minLimit;
			this.extraSlider.gameObject.SetActive(false);
			if (this.mode == MinMaxSlider.Mode.Triple)
			{
				this.extraSlider.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x060066B4 RID: 26292 RVA: 0x00264B50 File Offset: 0x00262D50
	public void SetIcon(Image newIcon)
	{
		this.icon = newIcon;
		this.icon.gameObject.transform.SetParent(base.transform);
		this.icon.gameObject.transform.SetAsFirstSibling();
		this.icon.rectTransform().anchoredPosition = Vector2.zero;
	}

	// Token: 0x060066B5 RID: 26293 RVA: 0x00264BAC File Offset: 0x00262DAC
	public void SetMode(MinMaxSlider.Mode mode)
	{
		this.mode = mode;
		if (mode == MinMaxSlider.Mode.Single && this.extraSlider != null)
		{
			this.extraSlider.gameObject.SetActive(false);
			this.extraSlider.handleRect.gameObject.SetActive(false);
		}
	}

	// Token: 0x060066B6 RID: 26294 RVA: 0x00264BF8 File Offset: 0x00262DF8
	private void SetAnchor(RectTransform trans, Vector2 min, Vector2 max)
	{
		trans.anchorMin = min;
		trans.anchorMax = max;
	}

	// Token: 0x060066B7 RID: 26295 RVA: 0x00264C08 File Offset: 0x00262E08
	public void SetMinMaxValue(float currentMin, float currentMax, float min, float max)
	{
		this.minSlider.value = currentMin;
		this.currentMinValue = currentMin;
		this.maxSlider.value = currentMax;
		this.currentMaxValue = currentMax;
		this.minLimit = min;
		this.maxLimit = max;
		this.minSlider.minValue = this.minLimit;
		this.maxSlider.minValue = this.minLimit;
		this.minSlider.maxValue = this.maxLimit;
		this.maxSlider.maxValue = this.maxLimit;
		if (this.extraSlider != null)
		{
			this.extraSlider.minValue = this.minLimit;
			this.extraSlider.maxValue = this.maxLimit;
		}
	}

	// Token: 0x060066B8 RID: 26296 RVA: 0x00264CC2 File Offset: 0x00262EC2
	public void SetExtraValue(float current)
	{
		this.extraSlider.value = current;
		this.toolTip.toolTip = base.transform.parent.name + ": " + current.ToString("F2");
	}

	// Token: 0x060066B9 RID: 26297 RVA: 0x00264D04 File Offset: 0x00262F04
	public void SetMaxValue(float current, float max)
	{
		float num = current / max * 100f;
		if (this.isOverPowered != null)
		{
			this.isOverPowered.enabled = (num > 100f);
		}
		this.maxSlider.value = Mathf.Min(100f, num);
		if (this.toolTip != null)
		{
			this.toolTip.toolTip = string.Concat(new string[]
			{
				base.transform.parent.name,
				": ",
				current.ToString("F2"),
				"/",
				max.ToString("F2")
			});
		}
	}

	// Token: 0x060066BA RID: 26298 RVA: 0x00264DB8 File Offset: 0x00262FB8
	private void Update()
	{
		if (!this.interactable)
		{
			return;
		}
		this.minSlider.value = Mathf.Clamp(this.currentMinValue, this.minLimit, this.currentMinValue);
		this.maxSlider.value = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.currentMaxValue, Mathf.Max(this.minSlider.value, this.minLimit), this.maxLimit));
		if (this.direction == Slider.Direction.LeftToRight || this.direction == Slider.Direction.RightToLeft)
		{
			this.minRect.anchorMax = new Vector2(this.minSlider.value / this.maxLimit, this.minRect.anchorMax.y);
			this.maxRect.anchorMax = new Vector2(this.maxSlider.value / this.maxLimit, this.maxRect.anchorMax.y);
			this.maxRect.anchorMin = new Vector2(this.minSlider.value / this.maxLimit, this.maxRect.anchorMin.y);
			return;
		}
		this.minRect.anchorMax = new Vector2(this.minRect.anchorMin.x, this.minSlider.value / this.maxLimit);
		this.maxRect.anchorMin = new Vector2(this.maxRect.anchorMin.x, this.minSlider.value / this.maxLimit);
	}

	// Token: 0x060066BB RID: 26299 RVA: 0x00264F44 File Offset: 0x00263144
	public void OnMinValueChanged(float ignoreThis)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange)
		{
			this.currentMaxValue = Mathf.Min(Mathf.Max(this.minLimit, this.minSlider.value) + this.range, this.maxLimit);
			this.currentMinValue = Mathf.Max(this.minLimit, Mathf.Min(this.maxSlider.value, this.currentMaxValue - this.range));
		}
		else
		{
			this.currentMinValue = Mathf.Clamp(this.minSlider.value, this.minLimit, Mathf.Min(this.maxSlider.value, this.currentMaxValue));
		}
		if (this.onMinChange != null)
		{
			this.onMinChange(this);
		}
	}

	// Token: 0x060066BC RID: 26300 RVA: 0x00265008 File Offset: 0x00263208
	public void OnMaxValueChanged(float ignoreThis)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange)
		{
			this.currentMinValue = Mathf.Max(this.maxSlider.value - this.range, this.minLimit);
			this.currentMaxValue = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.maxSlider.value, Mathf.Max(this.currentMinValue + this.range, this.minLimit), this.maxLimit));
		}
		else
		{
			this.currentMaxValue = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.maxSlider.value, Mathf.Max(this.minSlider.value, this.minLimit), this.maxLimit));
		}
		if (this.onMaxChange != null)
		{
			this.onMaxChange(this);
		}
	}

	// Token: 0x060066BD RID: 26301 RVA: 0x002650E8 File Offset: 0x002632E8
	public void Lock(bool shouldLock)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockType == MinMaxSlider.LockingType.Drag)
		{
			this.lockRange = shouldLock;
			this.range = this.maxSlider.value - this.minSlider.value;
			this.mousePos = KInputManager.GetMousePos();
		}
	}

	// Token: 0x060066BE RID: 26302 RVA: 0x00265138 File Offset: 0x00263338
	public void ToggleLock()
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockType == MinMaxSlider.LockingType.Toggle)
		{
			this.lockRange = !this.lockRange;
			if (this.lockRange)
			{
				this.range = this.maxSlider.value - this.minSlider.value;
			}
		}
	}

	// Token: 0x060066BF RID: 26303 RVA: 0x0026518C File Offset: 0x0026338C
	public void OnDrag()
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange && this.lockType == MinMaxSlider.LockingType.Drag)
		{
			float num = KInputManager.GetMousePos().x - this.mousePos.x;
			if (this.direction == Slider.Direction.TopToBottom || this.direction == Slider.Direction.BottomToTop)
			{
				num = KInputManager.GetMousePos().y - this.mousePos.y;
			}
			this.currentMinValue = Mathf.Max(this.currentMinValue + num, this.minLimit);
			this.mousePos = KInputManager.GetMousePos();
		}
	}

	// Token: 0x040046DC RID: 18140
	public MinMaxSlider.LockingType lockType = MinMaxSlider.LockingType.Drag;

	// Token: 0x040046DE RID: 18142
	public bool lockRange;

	// Token: 0x040046DF RID: 18143
	public bool interactable = true;

	// Token: 0x040046E0 RID: 18144
	public float minLimit;

	// Token: 0x040046E1 RID: 18145
	public float maxLimit = 100f;

	// Token: 0x040046E2 RID: 18146
	public float range = 50f;

	// Token: 0x040046E3 RID: 18147
	public float barWidth = 10f;

	// Token: 0x040046E4 RID: 18148
	public float barHeight = 100f;

	// Token: 0x040046E5 RID: 18149
	public float currentMinValue = 10f;

	// Token: 0x040046E6 RID: 18150
	public float currentMaxValue = 90f;

	// Token: 0x040046E7 RID: 18151
	public float currentExtraValue = 50f;

	// Token: 0x040046E8 RID: 18152
	public Slider.Direction direction;

	// Token: 0x040046E9 RID: 18153
	public bool wholeNumbers = true;

	// Token: 0x040046EA RID: 18154
	public Action<MinMaxSlider> onMinChange;

	// Token: 0x040046EB RID: 18155
	public Action<MinMaxSlider> onMaxChange;

	// Token: 0x040046EC RID: 18156
	public Slider minSlider;

	// Token: 0x040046ED RID: 18157
	public Slider maxSlider;

	// Token: 0x040046EE RID: 18158
	public Slider extraSlider;

	// Token: 0x040046EF RID: 18159
	public RectTransform minRect;

	// Token: 0x040046F0 RID: 18160
	public RectTransform maxRect;

	// Token: 0x040046F1 RID: 18161
	public RectTransform bgFill;

	// Token: 0x040046F2 RID: 18162
	public RectTransform mgFill;

	// Token: 0x040046F3 RID: 18163
	public RectTransform fgFill;

	// Token: 0x040046F4 RID: 18164
	public Text title;

	// Token: 0x040046F5 RID: 18165
	[MyCmpGet]
	public ToolTip toolTip;

	// Token: 0x040046F6 RID: 18166
	public Image icon;

	// Token: 0x040046F7 RID: 18167
	public Image isOverPowered;

	// Token: 0x040046F8 RID: 18168
	private Vector3 mousePos;

	// Token: 0x02001BC3 RID: 7107
	public enum LockingType
	{
		// Token: 0x04007DD9 RID: 32217
		Toggle,
		// Token: 0x04007DDA RID: 32218
		Drag
	}

	// Token: 0x02001BC4 RID: 7108
	public enum Mode
	{
		// Token: 0x04007DDC RID: 32220
		Single,
		// Token: 0x04007DDD RID: 32221
		Double,
		// Token: 0x04007DDE RID: 32222
		Triple
	}
}
