using System;

// Token: 0x0200002F RID: 47
public class LowStrikeBuff : TimeBuff
{
	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060000FF RID: 255 RVA: 0x000064D5 File Offset: 0x000046D5
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.RuleLowStrikeBuff;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000100 RID: 256 RVA: 0x000064D9 File Offset: 0x000046D9
	public override float BasicDuration
	{
		get
		{
			return 9999f;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000101 RID: 257 RVA: 0x000064E0 File Offset: 0x000046E0
	public override float KeyValue
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000102 RID: 258 RVA: 0x000064E7 File Offset: 0x000046E7
	public override float KeyValue2
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000103 RID: 259 RVA: 0x000064EE File Offset: 0x000046EE
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000104 RID: 260 RVA: 0x000064F1 File Offset: 0x000046F1
	public override bool IsTimeBase
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000105 RID: 261 RVA: 0x000064F4 File Offset: 0x000046F4
	public override void Affect(int stacks)
	{
	}

	// Token: 0x06000106 RID: 262 RVA: 0x000064F8 File Offset: 0x000046F8
	public override void Tick(float delta)
	{
		if (this.intensifyValue > 0f)
		{
			this.intensify = this.KeyValue2 * delta;
			this.Target.DamageStrategy.ApplyBuffDmgIntensify(this.intensify);
			this.intensifyValue -= this.intensify;
		}
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00006549 File Offset: 0x00004749
	public override void OnHit()
	{
		this.Target.DamageStrategy.ApplyBuffDmgIntensify(-this.KeyValue);
		this.intensifyValue += this.KeyValue;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00006575 File Offset: 0x00004775
	public override void End()
	{
	}

	// Token: 0x04000113 RID: 275
	private float intensifyValue;

	// Token: 0x04000114 RID: 276
	private float intensify;
}
