using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;

// Token: 0x02000BEF RID: 3055
public class ShadowText : ShadowRect
{
	// Token: 0x06006092 RID: 24722 RVA: 0x0023B118 File Offset: 0x00239318
	protected override void MatchRect()
	{
		if (this.RectMain == null || this.RectShadow == null)
		{
			return;
		}
		if (this.shadowText == null)
		{
			this.shadowText = this.RectShadow.GetComponent<Text>();
		}
		if (this.mainText == null)
		{
			this.mainText = this.RectMain.GetComponent<Text>();
		}
		if (this.shadowText == null || this.mainText == null)
		{
			return;
		}
		if (this.shadowText.font != this.mainText.font)
		{
			this.shadowText.font = this.mainText.font;
		}
		if (this.shadowText.fontSize != this.mainText.fontSize)
		{
			this.shadowText.fontSize = this.mainText.fontSize;
		}
		if (this.shadowText.alignment != this.mainText.alignment)
		{
			this.shadowText.alignment = this.mainText.alignment;
		}
		if (this.shadowText.lineSpacing != this.mainText.lineSpacing)
		{
			this.shadowText.lineSpacing = this.mainText.lineSpacing;
		}
		string text = this.mainText.text;
		text = Regex.Replace(text, "\\</?color\\b.*?\\>", string.Empty);
		if (this.shadowText.text != text)
		{
			this.shadowText.text = text;
		}
		if (this.shadowText.color != this.shadowColor)
		{
			this.shadowText.color = this.shadowColor;
		}
		if (this.shadowText.horizontalOverflow != this.mainText.horizontalOverflow)
		{
			this.shadowText.horizontalOverflow = this.mainText.horizontalOverflow;
		}
		if (this.shadowText.verticalOverflow != this.mainText.verticalOverflow)
		{
			this.shadowText.verticalOverflow = this.mainText.verticalOverflow;
		}
		base.MatchRect();
	}

	// Token: 0x040041CA RID: 16842
	private Text shadowText;

	// Token: 0x040041CB RID: 16843
	private Text mainText;
}
