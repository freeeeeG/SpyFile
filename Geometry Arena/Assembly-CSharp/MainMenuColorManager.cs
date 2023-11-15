using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000018 RID: 24
public class MainMenuColorManager : MonoBehaviour
{
	// Token: 0x06000122 RID: 290 RVA: 0x00008879 File Offset: 0x00006A79
	public void Open()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00008887 File Offset: 0x00006A87
	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000124 RID: 292 RVA: 0x00008895 File Offset: 0x00006A95
	private void Start()
	{
		this.hue = this.hueStart;
	}

	// Token: 0x06000125 RID: 293 RVA: 0x000088A4 File Offset: 0x00006AA4
	private void Update()
	{
		this.hue += this.hueDeltaTime * Time.deltaTime;
		this.hue = this.StdHue(this.hue);
		Color color = Color.red.SetHue(this.hue).SetSaturation(this.sat).SetValue(this.val);
		foreach (MainMenuColorManager.ImageDyeSet imageDyeSet in this.imageDyeSets)
		{
			float value = this.StdHue(this.hue + imageDyeSet.hueDelta);
			imageDyeSet.img.color = color.SetHue(value);
		}
		foreach (MainMenuColorManager.TextDyeSet textDyeSet in this.textDyeSets)
		{
			float value2 = this.StdHue(this.hue + textDyeSet.hueDelta);
			textDyeSet.text.color = color.SetHue(value2);
		}
	}

	// Token: 0x06000126 RID: 294 RVA: 0x0000898B File Offset: 0x00006B8B
	private float StdHue(float hue)
	{
		if (hue > 1f)
		{
			hue -= 1f;
		}
		if (hue < 0f)
		{
			hue += 1f;
		}
		return hue;
	}

	// Token: 0x04000165 RID: 357
	public float hue;

	// Token: 0x04000166 RID: 358
	public float hueStart;

	// Token: 0x04000167 RID: 359
	public float hueDeltaTime = 0.15f;

	// Token: 0x04000168 RID: 360
	public float sat = 0.6f;

	// Token: 0x04000169 RID: 361
	public float val = 0.9f;

	// Token: 0x0400016A RID: 362
	[SerializeField]
	public MainMenuColorManager.ImageDyeSet[] imageDyeSets = new MainMenuColorManager.ImageDyeSet[0];

	// Token: 0x0400016B RID: 363
	[SerializeField]
	public MainMenuColorManager.TextDyeSet[] textDyeSets = new MainMenuColorManager.TextDyeSet[0];

	// Token: 0x02000135 RID: 309
	[Serializable]
	public class ImageDyeSet
	{
		// Token: 0x04000957 RID: 2391
		public Image img;

		// Token: 0x04000958 RID: 2392
		public float hueDelta;
	}

	// Token: 0x02000136 RID: 310
	[Serializable]
	public class TextDyeSet
	{
		// Token: 0x04000959 RID: 2393
		public Text text;

		// Token: 0x0400095A RID: 2394
		public float hueDelta;
	}
}
