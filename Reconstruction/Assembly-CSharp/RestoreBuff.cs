using System;

// Token: 0x02000035 RID: 53
public class RestoreBuff : TileBuff
{
	// Token: 0x1700004C RID: 76
	// (get) Token: 0x06000137 RID: 311 RVA: 0x00006802 File Offset: 0x00004A02
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.RuleRestoreBuff;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06000138 RID: 312 RVA: 0x00006806 File Offset: 0x00004A06
	public override float KeyValue
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000139 RID: 313 RVA: 0x0000680D File Offset: 0x00004A0D
	public override bool IsTimeBase
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x0600013A RID: 314 RVA: 0x00006810 File Offset: 0x00004A10
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x0600013B RID: 315 RVA: 0x00006813 File Offset: 0x00004A13
	public override float BasicDuration
	{
		get
		{
			return 999f;
		}
	}

	// Token: 0x0600013C RID: 316 RVA: 0x0000681A File Offset: 0x00004A1A
	public override void Tick(float delta)
	{
		base.Tick(delta);
		if (this.Target.DamageStrategy.PathProgress > 0.5f)
		{
			this.End();
			base.IsFinished = true;
		}
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00006847 File Offset: 0x00004A47
	public override void Affect(int stacks)
	{
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000684C File Offset: 0x00004A4C
	public override void End()
	{
		this.Target.DamageStrategy.CurrentHealth += (this.Target.DamageStrategy.MaxHealth - this.Target.DamageStrategy.CurrentHealth) * this.KeyValue;
	}
}
