using System;

// Token: 0x0200002E RID: 46
public class LongMapBuff : TileBuff
{
	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000646A File Offset: 0x0000466A
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.LongMap;
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060000F5 RID: 245 RVA: 0x0000646E File Offset: 0x0000466E
	public override float KeyValue
	{
		get
		{
			return 0.01f;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060000F6 RID: 246 RVA: 0x00006475 File Offset: 0x00004675
	public override float KeyValue2
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x060000F7 RID: 247 RVA: 0x0000647C File Offset: 0x0000467C
	public override float BasicDuration
	{
		get
		{
			return 999f;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060000F8 RID: 248 RVA: 0x00006483 File Offset: 0x00004683
	public override bool IsTimeBase
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060000F9 RID: 249 RVA: 0x00006486 File Offset: 0x00004686
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060000FA RID: 250 RVA: 0x00006489 File Offset: 0x00004689
	private float maxValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00006490 File Offset: 0x00004690
	public override void Tick(float delta)
	{
		if (this.intensifiedValue < this.maxValue)
		{
			this.intensifiedValue += this.KeyValue;
			this.Target.DamageStrategy.ApplyBuffDmgIntensify(this.KeyValue);
		}
	}

	// Token: 0x060000FC RID: 252 RVA: 0x000064C9 File Offset: 0x000046C9
	public override void End()
	{
	}

	// Token: 0x060000FD RID: 253 RVA: 0x000064CB File Offset: 0x000046CB
	public override void Affect(int stacks)
	{
	}

	// Token: 0x04000112 RID: 274
	private float intensifiedValue;
}
