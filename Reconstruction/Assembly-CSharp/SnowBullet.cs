using System;
using UnityEngine;

// Token: 0x0200021B RID: 539
public class SnowBullet : PenetrateBullet
{
	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06000D53 RID: 3411 RVA: 0x000226B2 File Offset: 0x000208B2
	// (set) Token: 0x06000D54 RID: 3412 RVA: 0x000226BA File Offset: 0x000208BA
	public bool TechSkillUnlocked { get; set; }

	// Token: 0x06000D55 RID: 3413 RVA: 0x000226C3 File Offset: 0x000208C3
	public override bool GameUpdate()
	{
		this.SizeChange();
		return base.DistanceCheck(this.TargetPos);
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x000226D7 File Offset: 0x000208D7
	private void SizeChange()
	{
		if (this.TechSkillUnlocked)
		{
			base.BulletDamageIntensify += 0.25f * Time.deltaTime;
		}
	}

	// Token: 0x04000674 RID: 1652
	private const float techSkillIncreaseRate = 0.25f;
}
