using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000E RID: 14
public class GeneralUISpace : MonoBehaviour
{
	// Token: 0x0600003D RID: 61 RVA: 0x00005204 File Offset: 0x00003404
	private void Start()
	{
		this.mainWindow.localScale = Vector3.zero;
		LeanTween.scale(this.mainWindow, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
		LeanTween.alphaCanvas(this.mainWindow.GetComponent<CanvasGroup>(), 0f, 1f).setDelay(2f).setLoopPingPong().setRepeat(2);
		this.mainParagraphText.anchoredPosition3D += new Vector3(0f, -10f, 0f);
		LeanTween.textAlpha(this.mainParagraphText, 0f, 0.6f).setFrom(0f).setDelay(0f);
		LeanTween.textAlpha(this.mainParagraphText, 1f, 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f);
		LeanTween.move(this.mainParagraphText, this.mainParagraphText.anchoredPosition3D + new Vector3(0f, 10f, 0f), 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f);
		LeanTween.textColor(this.mainTitleText, new Color(0.52156866f, 0.5686275f, 0.8745098f), 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f).setLoopPingPong().setRepeat(-1);
		LeanTween.textAlpha(this.mainButton2, 1f, 2f).setFrom(0f).setDelay(0f).setEase(LeanTweenType.easeOutQuad);
		LeanTween.alpha(this.mainButton2, 1f, 2f).setFrom(0f).setDelay(0f).setEase(LeanTweenType.easeOutQuad);
		LeanTween.size(this.mainButton1, this.mainButton1.sizeDelta * 1.1f, 0.5f).setDelay(3f).setEaseInOutCirc().setRepeat(6).setLoopPingPong();
		this.pauseWindow.anchoredPosition3D += new Vector3(0f, 200f, 0f);
		LeanTween.moveY(this.pauseWindow, this.pauseWindow.anchoredPosition3D.y + -200f, 0.6f).setEase(LeanTweenType.easeOutSine).setDelay(0.6f);
		RectTransform component = this.pauseWindow.Find("PauseText").GetComponent<RectTransform>();
		LeanTween.moveZ(component, component.anchoredPosition3D.z - 80f, 1.5f).setEase(LeanTweenType.punch).setDelay(2f);
		LeanTween.rotateAroundLocal(this.pauseRing1, Vector3.forward, 360f, 12f).setRepeat(-1);
		LeanTween.rotateAroundLocal(this.pauseRing2, Vector3.forward, -360f, 22f).setRepeat(-1);
		this.chatWindow.RotateAround(this.chatWindow.position, Vector3.up, -180f);
		LeanTween.rotateAround(this.chatWindow, Vector3.up, 180f, 2f).setEase(LeanTweenType.easeOutElastic).setDelay(1.2f);
		LeanTween.play(this.chatRect, this.chatSprites).setLoopPingPong();
		LeanTween.color(this.chatBar2, new Color(0.972549f, 0.2627451f, 0.42352942f, 0.5f), 1.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong().setDelay(1.2f);
		LeanTween.scale(this.chatBar2, new Vector2(1f, 0.7f), 1.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong();
		string origText = this.chatText.text;
		this.chatText.text = "";
		LeanTween.value(base.gameObject, 0f, (float)origText.Length, 6f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float val)
		{
			this.chatText.text = origText.Substring(0, Mathf.RoundToInt(val));
		}).setLoopClamp().setDelay(2f);
		LeanTween.alpha(this.rawImageRect, 0f, 1f).setLoopPingPong();
	}

	// Token: 0x0400003F RID: 63
	public RectTransform mainWindow;

	// Token: 0x04000040 RID: 64
	public RectTransform mainParagraphText;

	// Token: 0x04000041 RID: 65
	public RectTransform mainTitleText;

	// Token: 0x04000042 RID: 66
	public RectTransform mainButton1;

	// Token: 0x04000043 RID: 67
	public RectTransform mainButton2;

	// Token: 0x04000044 RID: 68
	public RectTransform pauseRing1;

	// Token: 0x04000045 RID: 69
	public RectTransform pauseRing2;

	// Token: 0x04000046 RID: 70
	public RectTransform pauseWindow;

	// Token: 0x04000047 RID: 71
	public RectTransform chatWindow;

	// Token: 0x04000048 RID: 72
	public RectTransform chatRect;

	// Token: 0x04000049 RID: 73
	public Sprite[] chatSprites;

	// Token: 0x0400004A RID: 74
	public RectTransform chatBar1;

	// Token: 0x0400004B RID: 75
	public RectTransform chatBar2;

	// Token: 0x0400004C RID: 76
	public Text chatText;

	// Token: 0x0400004D RID: 77
	public RectTransform rawImageRect;
}
