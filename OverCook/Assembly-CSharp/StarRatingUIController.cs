using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B51 RID: 2897
public abstract class StarRatingUIController : UIControllerBase
{
	// Token: 0x06003AD9 RID: 15065 RVA: 0x00112696 File Offset: 0x00110A96
	protected virtual void Awake()
	{
		this.m_animator = base.gameObject.RequireComponent<Animator>();
		if (this.m_buttonIcon != null)
		{
			this.m_buttonIcon.enabled = false;
		}
	}

	// Token: 0x06003ADA RID: 15066
	public abstract void SetScoreData(object _data);

	// Token: 0x06003ADB RID: 15067 RVA: 0x001126C6 File Offset: 0x00110AC6
	protected void SetScoreData(int _starRating)
	{
		this.m_animator.SetInteger(StarRatingUIController.m_iStarRating, _starRating);
		this.m_bScoreSet = true;
	}

	// Token: 0x06003ADC RID: 15068 RVA: 0x001126E0 File Offset: 0x00110AE0
	public void SetButtonActive()
	{
		if (this.m_buttonIcon != null)
		{
			this.m_buttonIcon.enabled = true;
		}
	}

	// Token: 0x06003ADD RID: 15069 RVA: 0x00112700 File Offset: 0x00110B00
	public bool HasAnimationSettled()
	{
		int integer = this.m_animator.GetInteger(StarRatingUIController.m_iStarRating);
		AnimatorStateInfo currentAnimatorStateInfo = this.m_animator.GetCurrentAnimatorStateInfo(1);
		return this.m_bScoreSet && currentAnimatorStateInfo.IsName("Star" + integer) && this.m_animator.GetBool(StarRatingUIController.m_iReady);
	}

	// Token: 0x06003ADE RID: 15070 RVA: 0x00112765 File Offset: 0x00110B65
	public virtual bool AllowedToSkip()
	{
		return true;
	}

	// Token: 0x06003ADF RID: 15071 RVA: 0x00112768 File Offset: 0x00110B68
	public virtual bool AllowedToRestart()
	{
		return true;
	}

	// Token: 0x04002FC0 RID: 12224
	[SerializeField]
	private Image m_buttonIcon;

	// Token: 0x04002FC1 RID: 12225
	private Animator m_animator;

	// Token: 0x04002FC2 RID: 12226
	private bool m_bScoreSet;

	// Token: 0x04002FC3 RID: 12227
	private static readonly int m_iStarRating = Animator.StringToHash("StarRating");

	// Token: 0x04002FC4 RID: 12228
	private static readonly int m_iReady = Animator.StringToHash("Ready");
}
