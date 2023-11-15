using System;

// Token: 0x020000B4 RID: 180
public class DamageBonusBuff : TurretBuff
{
	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06000472 RID: 1138 RVA: 0x0000C084 File Offset: 0x0000A284
	public override TurretBuffName TBuffName
	{
		get
		{
			return TurretBuffName.DamageBonusBuff;
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06000473 RID: 1139 RVA: 0x0000C087 File Offset: 0x0000A287
	public override int MaxStacks
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06000474 RID: 1140 RVA: 0x0000C08A File Offset: 0x0000A28A
	public override float KeyValue
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0000C091 File Offset: 0x0000A291
	public override void Affect(int stacks)
	{
		this.TStrategy.TurnFixDamageBonus += this.KeyValue * (float)stacks;
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0000C0AE File Offset: 0x0000A2AE
	public override void End()
	{
		this.TStrategy.TurnFixDamageBonus -= this.KeyValue * (float)this.CurrentStack;
	}
}
