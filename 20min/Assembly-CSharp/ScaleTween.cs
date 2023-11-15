using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class ScaleTween : UITweener
{
	// Token: 0x060003FE RID: 1022 RVA: 0x00015555 File Offset: 0x00013755
	private void Awake()
	{
		base.transform.localScale = new Vector3(this.startScaleX, this.startScaleY, 1f);
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x00015578 File Offset: 0x00013778
	public override void Show()
	{
		LeanTween.scaleX(base.gameObject, 1f, this.duration).setEase(this.easeType).setIgnoreTimeScale(true);
		LeanTween.scaleY(base.gameObject, 1f, this.duration).setEase(this.easeType).setIgnoreTimeScale(true);
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x000155D5 File Offset: 0x000137D5
	public override void Hide()
	{
		LeanTween.scaleX(base.gameObject, this.startScaleX, this.duration).setIgnoreTimeScale(true);
		LeanTween.scaleY(base.gameObject, this.startScaleY, this.duration).setIgnoreTimeScale(true);
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x00015613 File Offset: 0x00013813
	public override void SetOff()
	{
		this.Hide();
	}

	// Token: 0x04000214 RID: 532
	[SerializeField]
	private LeanTweenType easeType = LeanTweenType.linear;

	// Token: 0x04000215 RID: 533
	[SerializeField]
	private float startScaleX = 1f;

	// Token: 0x04000216 RID: 534
	[SerializeField]
	private float startScaleY = 1f;
}
