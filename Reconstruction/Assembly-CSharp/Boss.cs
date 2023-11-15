using System;

// Token: 0x020000BC RID: 188
public class Boss : Enemy
{
	// Token: 0x1700023B RID: 571
	// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0000D2EA File Offset: 0x0000B4EA
	public override string ExplosionSound
	{
		get
		{
			return "Sound_BossExplosion";
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x060004D6 RID: 1238 RVA: 0x0000D2F1 File Offset: 0x0000B4F1
	protected virtual bool ShakeCam
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0000D2F4 File Offset: 0x0000B4F4
	public override int MaxAmount
	{
		get
		{
			return 20;
		}
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x0000D2F8 File Offset: 0x0000B4F8
	public override void OnDie()
	{
		base.OnDie();
		if (this.ShakeCam)
		{
			Singleton<GameManager>.Instance.ShakeCam();
		}
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x0000D312 File Offset: 0x0000B512
	protected void ShowBossText(float chance)
	{
		base.HealthBar.ShowBossTxt(this.m_Att, chance);
	}
}
