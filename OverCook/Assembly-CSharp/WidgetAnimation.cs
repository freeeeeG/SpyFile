using System;
using UnityEngine;

// Token: 0x02000A71 RID: 2673
public abstract class WidgetAnimation
{
	// Token: 0x060034D4 RID: 13524 RVA: 0x000F77B7 File Offset: 0x000F5BB7
	public virtual void Init(Animator _animator)
	{
	}

	// Token: 0x060034D5 RID: 13525
	public abstract void Advance(float _deltaTime);

	// Token: 0x060034D6 RID: 13526
	public abstract bool IsFinished();

	// Token: 0x060034D7 RID: 13527 RVA: 0x000F77B9 File Offset: 0x000F5BB9
	public virtual Color GetColourModifier()
	{
		return Color.white;
	}

	// Token: 0x060034D8 RID: 13528 RVA: 0x000F77C0 File Offset: 0x000F5BC0
	public virtual Vector2 GetPosModifier()
	{
		return Vector2.zero;
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x000F77C7 File Offset: 0x000F5BC7
	public virtual Vector2 GetScaleModifier()
	{
		return Vector2.one;
	}
}
