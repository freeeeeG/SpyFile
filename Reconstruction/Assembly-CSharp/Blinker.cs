using System;
using System.Collections.Generic;

// Token: 0x020000CF RID: 207
public class Blinker : Boss
{
	// Token: 0x1700024F RID: 591
	// (get) Token: 0x0600053E RID: 1342 RVA: 0x0000E710 File Offset: 0x0000C910
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Blinker;
		}
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x0000E714 File Offset: 0x0000C914
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		this.blink = 3;
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x0000E72C File Offset: 0x0000C92C
	protected override void OnEnemyUpdate()
	{
		base.OnEnemyUpdate();
		if (base.DamageStrategy.CurrentHealth / base.DamageStrategy.MaxHealth < 0.75f && this.blink >= 3)
		{
			this.blink--;
			base.ShowBossText(0.5f);
			base.Flash(-this.flashDis);
			return;
		}
		if (base.DamageStrategy.CurrentHealth / base.DamageStrategy.MaxHealth < 0.5f && this.blink >= 2)
		{
			this.blink--;
			base.ShowBossText(0.5f);
			base.Flash(-this.flashDis);
			return;
		}
		if (base.DamageStrategy.CurrentHealth / base.DamageStrategy.MaxHealth < 0.25f && this.blink >= 1)
		{
			this.blink--;
			base.ShowBossText(0.5f);
			base.Flash(-this.flashDis);
		}
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0000E828 File Offset: 0x0000CA28
	public override void OnDie()
	{
		base.OnDie();
		Singleton<LevelManager>.Instance.SetAchievement("ACH_BAT");
	}

	// Token: 0x04000225 RID: 549
	private int flashDis = 3;

	// Token: 0x04000226 RID: 550
	private int blink;
}
