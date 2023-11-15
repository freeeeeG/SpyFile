using System;
using UnityEngine;

// Token: 0x02000773 RID: 1907
public class DreamBubble : KMonoBehaviour
{
	// Token: 0x170003BF RID: 959
	// (get) Token: 0x060034B5 RID: 13493 RVA: 0x0011B092 File Offset: 0x00119292
	// (set) Token: 0x060034B4 RID: 13492 RVA: 0x0011B089 File Offset: 0x00119289
	public bool IsVisible { get; private set; }

	// Token: 0x060034B6 RID: 13494 RVA: 0x0011B09A File Offset: 0x0011929A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.dreamBackgroundComponent.SetSymbolVisiblity(this.snapToPivotSymbol, false);
		this.SetVisibility(false);
	}

	// Token: 0x060034B7 RID: 13495 RVA: 0x0011B0C0 File Offset: 0x001192C0
	public void Tick(float dt)
	{
		if (this._currentDream != null && this._currentDream.Icons.Length != 0)
		{
			float num = this._timePassedSinceDreamStarted / this._currentDream.secondPerImage;
			int num2 = Mathf.FloorToInt(num);
			float num3 = num - (float)num2;
			int num4 = (int)Mathf.Repeat((float)Mathf.FloorToInt(num), (float)this._currentDream.Icons.Length);
			if (this.dreamContentComponent.sprite != this._currentDream.Icons[num4])
			{
				this.dreamContentComponent.sprite = this._currentDream.Icons[num4];
			}
			this.dreamContentComponent.rectTransform.localScale = Vector3.one * num3;
			this._color.a = (Mathf.Sin(num3 * 6.2831855f - 1.5707964f) + 1f) * 0.5f;
			this.dreamContentComponent.color = this._color;
			this._timePassedSinceDreamStarted += dt;
		}
	}

	// Token: 0x060034B8 RID: 13496 RVA: 0x0011B1BC File Offset: 0x001193BC
	public void SetDream(Dream dream)
	{
		this._currentDream = dream;
		this.dreamBackgroundComponent.Stop();
		this.dreamBackgroundComponent.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(dream.BackgroundAnim)
		};
		this.dreamContentComponent.color = this._color;
		this.dreamContentComponent.enabled = (dream != null && dream.Icons != null && dream.Icons.Length != 0);
		this._timePassedSinceDreamStarted = 0f;
		this._color.a = 0f;
	}

	// Token: 0x060034B9 RID: 13497 RVA: 0x0011B250 File Offset: 0x00119450
	public void SetVisibility(bool visible)
	{
		this.IsVisible = visible;
		this.dreamBackgroundComponent.SetVisiblity(visible);
		this.dreamContentComponent.gameObject.SetActive(visible);
		if (visible)
		{
			if (this._currentDream != null)
			{
				this.dreamBackgroundComponent.Play("dream_loop", KAnim.PlayMode.Loop, 1f, 0f);
			}
			this.dreamBubbleBorderKanim.Play("dream_bubble_loop", KAnim.PlayMode.Loop, 1f, 0f);
			this.maskKanim.Play("dream_bubble_mask", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		this.dreamBackgroundComponent.Stop();
		this.maskKanim.Stop();
		this.dreamBubbleBorderKanim.Stop();
	}

	// Token: 0x060034BA RID: 13498 RVA: 0x0011B30E File Offset: 0x0011950E
	public void StopDreaming()
	{
		this._currentDream = null;
		this.SetVisibility(false);
	}

	// Token: 0x04001FD0 RID: 8144
	public KBatchedAnimController dreamBackgroundComponent;

	// Token: 0x04001FD1 RID: 8145
	public KBatchedAnimController maskKanim;

	// Token: 0x04001FD2 RID: 8146
	public KBatchedAnimController dreamBubbleBorderKanim;

	// Token: 0x04001FD3 RID: 8147
	public KImage dreamContentComponent;

	// Token: 0x04001FD4 RID: 8148
	private const string dreamBackgroundAnimationName = "dream_loop";

	// Token: 0x04001FD5 RID: 8149
	private const string dreamMaskAnimationName = "dream_bubble_mask";

	// Token: 0x04001FD6 RID: 8150
	private const string dreamBubbleBorderAnimationName = "dream_bubble_loop";

	// Token: 0x04001FD7 RID: 8151
	private HashedString snapToPivotSymbol = new HashedString("snapto_pivot");

	// Token: 0x04001FD9 RID: 8153
	private Dream _currentDream;

	// Token: 0x04001FDA RID: 8154
	private float _timePassedSinceDreamStarted;

	// Token: 0x04001FDB RID: 8155
	private Color _color = Color.white;

	// Token: 0x04001FDC RID: 8156
	private const float PI_2 = 6.2831855f;

	// Token: 0x04001FDD RID: 8157
	private const float HALF_PI = 1.5707964f;
}
