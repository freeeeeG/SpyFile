using System;
using UnityEngine;

// Token: 0x02000A6E RID: 2670
public class RecipeFailureAnimation : WidgetAnimation
{
	// Token: 0x060034C4 RID: 13508 RVA: 0x000F785C File Offset: 0x000F5C5C
	public override void Init(Animator _animator)
	{
		_animator.SetTrigger(RecipeFailureAnimation.m_iFail);
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x000F7869 File Offset: 0x000F5C69
	public override void Advance(float _deltaTime)
	{
		this.m_time += _deltaTime;
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x000F7879 File Offset: 0x000F5C79
	public override bool IsFinished()
	{
		return this.m_time > this.c_totalTime;
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x000F7889 File Offset: 0x000F5C89
	public override Color GetColourModifier()
	{
		return Color.Lerp(Color.white, this.m_color, Mathf.Sin(3.1415927f * Mathf.Clamp01(this.m_time / this.c_totalTime)));
	}

	// Token: 0x060034C8 RID: 13512 RVA: 0x000F78B8 File Offset: 0x000F5CB8
	public override Vector2 GetPosModifier()
	{
		float x = this.c_amplitude * Mathf.Sin(this.c_oscillations * 2f * 3.1415927f * this.m_time / this.c_totalTime);
		return new Vector2(x, 0f);
	}

	// Token: 0x04002A49 RID: 10825
	private Color m_color = Color.red;

	// Token: 0x04002A4A RID: 10826
	private float m_time;

	// Token: 0x04002A4B RID: 10827
	private readonly float c_totalTime = 0.5f;

	// Token: 0x04002A4C RID: 10828
	private readonly float c_oscillations = 10f;

	// Token: 0x04002A4D RID: 10829
	private readonly float c_amplitude = 3f;

	// Token: 0x04002A4E RID: 10830
	private static int m_iFail = Animator.StringToHash("Fail");
}
