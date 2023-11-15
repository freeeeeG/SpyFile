using System;

// Token: 0x020000B3 RID: 179
public class FirerateBuff : TurretBuff
{
	// Token: 0x1700021F RID: 543
	// (get) Token: 0x0600046C RID: 1132 RVA: 0x0000C02F File Offset: 0x0000A22F
	public override TurretBuffName TBuffName
	{
		get
		{
			return TurretBuffName.FirerateBuff;
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x0600046D RID: 1133 RVA: 0x0000C032 File Offset: 0x0000A232
	public override int MaxStacks
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x0600046E RID: 1134 RVA: 0x0000C036 File Offset: 0x0000A236
	public override float KeyValue
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0000C03D File Offset: 0x0000A23D
	public override void Affect(int stacks)
	{
		this.TStrategy.TurnFireRateIntensify += this.KeyValue * (float)stacks;
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0000C05A File Offset: 0x0000A25A
	public override void End()
	{
		this.TStrategy.TurnFireRateIntensify -= this.KeyValue * (float)this.CurrentStack;
	}
}
