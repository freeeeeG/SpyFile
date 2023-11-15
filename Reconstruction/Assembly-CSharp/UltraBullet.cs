using System;
using UnityEngine;

// Token: 0x0200021F RID: 543
public class UltraBullet : GroundBullet
{
	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00022933 File Offset: 0x00020B33
	// (set) Token: 0x06000D61 RID: 3425 RVA: 0x0002293B File Offset: 0x00020B3B
	public float SplashIncreasePerSecond { get; set; }

	// Token: 0x06000D62 RID: 3426 RVA: 0x00022944 File Offset: 0x00020B44
	public override void TriggerDamage()
	{
		base.TriggerDamage();
		UltraLava ultraLava = Singleton<ObjectPool>.Instance.Spawn(this.UltraLava) as UltraLava;
		ultraLava.SplashRangeIncreasePerSecond = this.SplashIncreasePerSecond;
		ultraLava.transform.position = this.TargetPos;
		ultraLava.SetAtt(this);
		ultraLava.BulletLastTime = 5f;
	}

	// Token: 0x04000679 RID: 1657
	[SerializeField]
	private UltraLava UltraLava;
}
