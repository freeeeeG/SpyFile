using System;
using UnityEngine;

// Token: 0x02000209 RID: 521
public class Chiller : RecipeRefactor
{
	// Token: 0x06000D0F RID: 3343 RVA: 0x00021A60 File Offset: 0x0001FC60
	protected override void Shoot()
	{
		this.fireArea.Initialize(this, base.Target[0], null);
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x00021A8E File Offset: 0x0001FC8E
	public override void AddTarget(TargetPoint target)
	{
		base.AddTarget(target);
		this.iceEffect.Play();
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x00021AA2 File Offset: 0x0001FCA2
	public override void RemoveTarget(TargetPoint target)
	{
		base.RemoveTarget(target);
		if (this.targetList.Count <= 0)
		{
			this.iceEffect.Stop();
		}
	}

	// Token: 0x04000657 RID: 1623
	[SerializeField]
	private FirerArea fireArea;

	// Token: 0x04000658 RID: 1624
	[SerializeField]
	private ParticleSystem iceEffect;
}
