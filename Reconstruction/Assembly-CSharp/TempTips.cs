using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200028F RID: 655
public class TempTips : IUserInterface
{
	// Token: 0x06001026 RID: 4134 RVA: 0x0002B468 File Offset: 0x00029668
	public override void Initialize()
	{
		base.Initialize();
		this.rect = base.GetComponent<RectTransform>();
		this.offsetDistance = this.boxFrame.rectTransform.sizeDelta.y - this.HorizontalText.rectTransform.sizeDelta.y / 2f;
		this.VerticalText.color = this.hideColor;
		this.HorizontalText.color = this.textColor;
	}

	// Token: 0x06001027 RID: 4135 RVA: 0x0002B4E0 File Offset: 0x000296E0
	private void AdjustDialogBoxSize()
	{
		if (this.VerticalText.rectTransform.sizeDelta.y > (float)this.rowHeight)
		{
			this.VerticalText.color = this.textColor;
			this.HorizontalText.color = this.hideColor;
			this.boxFrame.rectTransform.sizeDelta = new Vector2((float)this.minFrameWidth + this.VerticalText.rectTransform.sizeDelta.x / 2f, this.VerticalText.rectTransform.sizeDelta.y + this.offsetDistance);
			base.GetComponent<RectTransform>().sizeDelta = new Vector2((float)this.minFrameWidth + this.VerticalText.rectTransform.sizeDelta.x / 2f, this.VerticalText.rectTransform.sizeDelta.y + this.offsetDistance);
			return;
		}
		this.boxFrame.rectTransform.sizeDelta = new Vector2((float)this.minFrameWidth + this.HorizontalText.rectTransform.sizeDelta.x / 2f, this.HorizontalText.rectTransform.sizeDelta.y / 2f + this.offsetDistance);
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x0002B62E File Offset: 0x0002982E
	public void SendText(string input)
	{
		this.VerticalText.text = input;
		this.HorizontalText.text = input;
		base.StartCoroutine(this.AdjustSize());
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x0002B655 File Offset: 0x00029855
	private IEnumerator AdjustSize()
	{
		yield return new WaitForEndOfFrame();
		this.AdjustDialogBoxSize();
		yield break;
	}

	// Token: 0x04000867 RID: 2151
	public RectTransform rect;

	// Token: 0x04000868 RID: 2152
	public Image boxFrame;

	// Token: 0x04000869 RID: 2153
	public Text HorizontalText;

	// Token: 0x0400086A RID: 2154
	public Text VerticalText;

	// Token: 0x0400086B RID: 2155
	public int minFrameWidth = 100;

	// Token: 0x0400086C RID: 2156
	public int maxFrameWidth = 600;

	// Token: 0x0400086D RID: 2157
	public int rowHeight = 80;

	// Token: 0x0400086E RID: 2158
	public Color textColor = new Color(0f, 0f, 0f, 1f);

	// Token: 0x0400086F RID: 2159
	public Color hideColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04000870 RID: 2160
	private float offsetDistance;
}
