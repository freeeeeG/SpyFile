using System;

// Token: 0x02000038 RID: 56
public class RuleFastBuff : TileBuff
{
	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000151 RID: 337 RVA: 0x00006A18 File Offset: 0x00004C18
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.RuleFastBuff;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000152 RID: 338 RVA: 0x00006A1C File Offset: 0x00004C1C
	public override float BasicDuration
	{
		get
		{
			return 999f;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000153 RID: 339 RVA: 0x00006A23 File Offset: 0x00004C23
	public override float KeyValue
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000154 RID: 340 RVA: 0x00006A2A File Offset: 0x00004C2A
	public override bool IsTimeBase
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000155 RID: 341 RVA: 0x00006A2D File Offset: 0x00004C2D
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00006A30 File Offset: 0x00004C30
	public override void Affect(int stacks)
	{
		this.Target.SpeedIntensify += this.KeyValue;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00006A4A File Offset: 0x00004C4A
	public override void End()
	{
	}
}
