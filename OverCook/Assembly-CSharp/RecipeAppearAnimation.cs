using System;
using UnityEngine;

// Token: 0x02000A6D RID: 2669
public class RecipeAppearAnimation : WidgetAnimation
{
	// Token: 0x060034BF RID: 13503 RVA: 0x000F77CE File Offset: 0x000F5BCE
	public RecipeAppearAnimation(AnimationCurve curve)
	{
		this.m_curve = curve;
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x000F77DD File Offset: 0x000F5BDD
	public override void Advance(float _deltaTime)
	{
		this.m_time += _deltaTime;
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x000F77ED File Offset: 0x000F5BED
	public override bool IsFinished()
	{
		return this.m_time > 1f;
	}

	// Token: 0x060034C2 RID: 13506 RVA: 0x000F77FC File Offset: 0x000F5BFC
	public override Vector2 GetScaleModifier()
	{
		float num = this.m_curve.Evaluate(this.m_time % 1f);
		return new Vector2(num, num);
	}

	// Token: 0x04002A47 RID: 10823
	private float m_time;

	// Token: 0x04002A48 RID: 10824
	private AnimationCurve m_curve;
}
