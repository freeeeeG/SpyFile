using System;

// Token: 0x0200002C RID: 44
public class DamageIntensifyBuff : TimeBuff
{
	// Token: 0x17000018 RID: 24
	// (get) Token: 0x060000E2 RID: 226 RVA: 0x000063B1 File Offset: 0x000045B1
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.DamageIntensify;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x060000E3 RID: 227 RVA: 0x000063B4 File Offset: 0x000045B4
	public override bool IsStackable
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x060000E4 RID: 228 RVA: 0x000063B7 File Offset: 0x000045B7
	public override bool IsTimeBase
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x060000E5 RID: 229 RVA: 0x000063BA File Offset: 0x000045BA
	public override int MaxStacks
	{
		get
		{
			return 100;
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x060000E6 RID: 230 RVA: 0x000063BE File Offset: 0x000045BE
	public override float KeyValue
	{
		get
		{
			return 0.01f;
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x060000E7 RID: 231 RVA: 0x000063C5 File Offset: 0x000045C5
	public override float BasicDuration
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x000063CC File Offset: 0x000045CC
	public override void Affect(int stacks)
	{
		this.Target.DamageStrategy.ApplyBuffDmgIntensify(this.KeyValue * (float)stacks);
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x000063E7 File Offset: 0x000045E7
	public override void End()
	{
		this.Target.DamageStrategy.ApplyBuffDmgIntensify(-this.KeyValue * (float)this.CurrentStack);
	}
}
