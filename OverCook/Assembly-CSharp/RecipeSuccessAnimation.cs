using System;
using UnityEngine;

// Token: 0x02000A6F RID: 2671
public class RecipeSuccessAnimation : WidgetAnimation
{
	// Token: 0x060034CB RID: 13515 RVA: 0x000F7942 File Offset: 0x000F5D42
	public override void Advance(float _deltaTime)
	{
		this.m_time += _deltaTime;
	}

	// Token: 0x060034CC RID: 13516 RVA: 0x000F7952 File Offset: 0x000F5D52
	public override bool IsFinished()
	{
		return this.m_time > this.c_totalTime;
	}

	// Token: 0x060034CD RID: 13517 RVA: 0x000F7962 File Offset: 0x000F5D62
	private float SCurve(float _prop)
	{
		return 0.5f * (1f - Mathf.Cos(3.1415927f * Mathf.Clamp01(_prop)));
	}

	// Token: 0x060034CE RID: 13518 RVA: 0x000F7984 File Offset: 0x000F5D84
	public override Color GetColourModifier()
	{
		Color result = Color.Lerp(Color.white, this.m_color, Mathf.Sin(1.5707964f * Mathf.Clamp01(2f * this.m_time / this.c_totalTime)));
		result.a = Mathf.Lerp(result.a, 0f, this.SCurve(2f * this.m_time / this.c_totalTime - 1f));
		return result;
	}

	// Token: 0x04002A4F RID: 10831
	private Color m_color = Color.green;

	// Token: 0x04002A50 RID: 10832
	private float m_time;

	// Token: 0x04002A51 RID: 10833
	private readonly float c_totalTime = 0.5f;

	// Token: 0x04002A52 RID: 10834
	private readonly float c_oscillations = 10f;

	// Token: 0x04002A53 RID: 10835
	private readonly float c_amplitude = 3f;
}
