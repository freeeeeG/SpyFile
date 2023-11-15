using System;

// Token: 0x0200002B RID: 43
public class SlowBuff : TimeBuff
{
	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060000D9 RID: 217 RVA: 0x0000632C File Offset: 0x0000452C
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.SlowDown;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060000DA RID: 218 RVA: 0x0000632F File Offset: 0x0000452F
	public override bool IsStackable
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x060000DB RID: 219 RVA: 0x00006332 File Offset: 0x00004532
	public override bool IsTimeBase
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x060000DC RID: 220 RVA: 0x00006335 File Offset: 0x00004535
	public override float KeyValue
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x060000DD RID: 221 RVA: 0x0000633C File Offset: 0x0000453C
	public override int MaxStacks
	{
		get
		{
			return 6;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x060000DE RID: 222 RVA: 0x0000633F File Offset: 0x0000453F
	public override float BasicDuration
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00006346 File Offset: 0x00004546
	public override void Affect(int stacks)
	{
		this.Target.SpeedAdjust -= this.KeyValue * (float)stacks;
		this.Target.HealthBar.ShowIcon(1, true);
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00006375 File Offset: 0x00004575
	public override void End()
	{
		this.Target.SpeedAdjust += this.KeyValue * (float)this.CurrentStack;
		this.Target.HealthBar.ShowIcon(1, false);
	}
}
