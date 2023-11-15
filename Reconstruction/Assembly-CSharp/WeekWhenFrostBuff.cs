using System;

// Token: 0x0200003C RID: 60
public class WeekWhenFrostBuff : TimeBuff
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000174 RID: 372 RVA: 0x00006C15 File Offset: 0x00004E15
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.WeekWhenFrost;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000175 RID: 373 RVA: 0x00006C19 File Offset: 0x00004E19
	public override float BasicDuration
	{
		get
		{
			return 9999f;
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000176 RID: 374 RVA: 0x00006C20 File Offset: 0x00004E20
	public override float KeyValue
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06000177 RID: 375 RVA: 0x00006C27 File Offset: 0x00004E27
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x06000178 RID: 376 RVA: 0x00006C2A File Offset: 0x00004E2A
	public override bool IsTimeBase
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000179 RID: 377 RVA: 0x00006C2D File Offset: 0x00004E2D
	public override void Affect(int stacks)
	{
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00006C30 File Offset: 0x00004E30
	public override void Tick(float delta)
	{
		if (this.Target.DamageStrategy.IsFrost)
		{
			if (!this.intensify)
			{
				this.Target.DamageStrategy.ApplyBuffDmgIntensify(this.KeyValue);
				this.intensify = true;
				return;
			}
		}
		else if (this.intensify)
		{
			this.Target.DamageStrategy.ApplyBuffDmgIntensify(-this.KeyValue);
			this.intensify = false;
		}
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00006C9B File Offset: 0x00004E9B
	public override void End()
	{
	}

	// Token: 0x0400011B RID: 283
	private bool intensify;
}
