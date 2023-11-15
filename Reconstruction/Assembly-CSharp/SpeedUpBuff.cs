using System;

// Token: 0x0200003B RID: 59
public class SpeedUpBuff : TimeBuff
{
	// Token: 0x1700006A RID: 106
	// (get) Token: 0x0600016C RID: 364 RVA: 0x00006BC1 File Offset: 0x00004DC1
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.SpeedUpBuff;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x0600016D RID: 365 RVA: 0x00006BC5 File Offset: 0x00004DC5
	public override float BasicDuration
	{
		get
		{
			return 6f;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x0600016E RID: 366 RVA: 0x00006BCC File Offset: 0x00004DCC
	public override float KeyValue
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x0600016F RID: 367 RVA: 0x00006BD3 File Offset: 0x00004DD3
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x06000170 RID: 368 RVA: 0x00006BD6 File Offset: 0x00004DD6
	public override bool IsTimeBase
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00006BD9 File Offset: 0x00004DD9
	public override void Affect(int stacks)
	{
		this.Target.SpeedIntensify += this.KeyValue;
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00006BF3 File Offset: 0x00004DF3
	public override void End()
	{
		this.Target.SpeedIntensify -= this.KeyValue;
	}
}
