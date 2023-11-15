using System;

// Token: 0x020000B5 RID: 181
public class BasicCritBuff : TurretBuff
{
	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06000478 RID: 1144 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
	public override TurretBuffName TBuffName
	{
		get
		{
			return TurretBuffName.BasicCritBuff;
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06000479 RID: 1145 RVA: 0x0000C0DB File Offset: 0x0000A2DB
	public override int MaxStacks
	{
		get
		{
			return 10;
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x0600047A RID: 1146 RVA: 0x0000C0DF File Offset: 0x0000A2DF
	public override float KeyValue
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0000C0E6 File Offset: 0x0000A2E6
	public override void Affect(int stacks)
	{
		this.TStrategy.TurnFixCriticalRate += this.KeyValue * (float)stacks;
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0000C103 File Offset: 0x0000A303
	public override void End()
	{
		this.TStrategy.TurnFixCriticalRate -= this.KeyValue * (float)this.CurrentStack;
	}
}
