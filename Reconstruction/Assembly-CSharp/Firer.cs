using System;
using UnityEngine;

// Token: 0x0200020D RID: 525
public class Firer : RecipeRefactor
{
	// Token: 0x06000D16 RID: 3350 RVA: 0x00021AE4 File Offset: 0x0001FCE4
	protected override void Shoot()
	{
		this.fireArea.Initialize(this, base.Target[0], null);
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x00021B12 File Offset: 0x0001FD12
	public override void AddTarget(TargetPoint target)
	{
		base.AddTarget(target);
		this.fireEffect.Play();
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x00021B26 File Offset: 0x0001FD26
	public override void RemoveTarget(TargetPoint target)
	{
		base.RemoveTarget(target);
		if (this.targetList.Count <= 0)
		{
			this.fireEffect.Stop();
		}
	}

	// Token: 0x04000659 RID: 1625
	[SerializeField]
	private FirerArea fireArea;

	// Token: 0x0400065A RID: 1626
	[SerializeField]
	private ParticleSystem fireEffect;
}
