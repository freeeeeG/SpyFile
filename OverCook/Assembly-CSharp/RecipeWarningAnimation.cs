using System;
using UnityEngine;

// Token: 0x02000A70 RID: 2672
public class RecipeWarningAnimation : WidgetAnimation
{
	// Token: 0x060034CF RID: 13519 RVA: 0x000F79FD File Offset: 0x000F5DFD
	public RecipeWarningAnimation(AnimationCurve curve)
	{
		this.m_curve = curve;
	}

	// Token: 0x060034D0 RID: 13520 RVA: 0x000F7A0C File Offset: 0x000F5E0C
	public override void Advance(float _deltaTime)
	{
		this.m_time += _deltaTime;
	}

	// Token: 0x060034D1 RID: 13521 RVA: 0x000F7A1C File Offset: 0x000F5E1C
	public override bool IsFinished()
	{
		return this.m_time >= 1f;
	}

	// Token: 0x060034D2 RID: 13522 RVA: 0x000F7A30 File Offset: 0x000F5E30
	public override Vector2 GetPosModifier()
	{
		float x = this.m_curve.Evaluate(this.m_time % 1f);
		return new Vector2(x, 0f);
	}

	// Token: 0x04002A54 RID: 10836
	private float m_time;

	// Token: 0x04002A55 RID: 10837
	private AnimationCurve m_curve;
}
