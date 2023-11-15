using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
[RequireComponent(typeof(RectTransform))]
public class MoveTween : UITweener
{
	// Token: 0x060003F9 RID: 1017 RVA: 0x000153D0 File Offset: 0x000135D0
	private void Start()
	{
		switch (this.entranceFrom)
		{
		case MoveTween.Direction.Up:
			this.moveVector = new Vector3(0f, this.moveAmount, 0f);
			break;
		case MoveTween.Direction.Down:
			this.moveVector = new Vector3(0f, -1f * this.moveAmount, 0f);
			break;
		case MoveTween.Direction.Left:
			this.moveVector = new Vector3(-1f * this.moveAmount, 0f, 0f);
			break;
		case MoveTween.Direction.Right:
			this.moveVector = new Vector3(this.moveAmount, 0f, 0f);
			break;
		}
		this.rectTransform = base.GetComponent<RectTransform>();
		this.startPosition = this.rectTransform.anchoredPosition;
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0001549C File Offset: 0x0001369C
	public override void Show()
	{
		LeanTween.cancel(this._tweenID);
		this._tweenID = LeanTween.move(this.rectTransform, this.startPosition, this.duration).setEase(this.easeType).setIgnoreTimeScale(true).id;
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x000154DC File Offset: 0x000136DC
	public override void Hide()
	{
		LeanTween.cancel(this._tweenID);
		this._tweenID = LeanTween.move(this.rectTransform, this.startPosition + this.moveVector, this.duration).setIgnoreTimeScale(true).id;
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0001551C File Offset: 0x0001371C
	public override void SetOff()
	{
		LeanTween.move(this.rectTransform, this.startPosition + this.moveVector, 0f).setIgnoreTimeScale(true);
	}

	// Token: 0x0400020D RID: 525
	[SerializeField]
	private LeanTweenType easeType = LeanTweenType.linear;

	// Token: 0x0400020E RID: 526
	public MoveTween.Direction entranceFrom;

	// Token: 0x0400020F RID: 527
	public float moveAmount;

	// Token: 0x04000210 RID: 528
	private Vector3 moveVector;

	// Token: 0x04000211 RID: 529
	private Vector3 startPosition;

	// Token: 0x04000212 RID: 530
	private RectTransform rectTransform;

	// Token: 0x04000213 RID: 531
	private int _tweenID;

	// Token: 0x0200028A RID: 650
	public enum Direction
	{
		// Token: 0x04000A13 RID: 2579
		Up,
		// Token: 0x04000A14 RID: 2580
		Down,
		// Token: 0x04000A15 RID: 2581
		Left,
		// Token: 0x04000A16 RID: 2582
		Right
	}
}
