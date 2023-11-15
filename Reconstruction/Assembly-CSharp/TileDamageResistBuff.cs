using System;

// Token: 0x0200003A RID: 58
public class TileDamageResistBuff : TileBuff
{
	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000162 RID: 354 RVA: 0x00006B3B File Offset: 0x00004D3B
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.TileDamageResistBuff;
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000163 RID: 355 RVA: 0x00006B3F File Offset: 0x00004D3F
	public override float BasicDuration
	{
		get
		{
			return 50f;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06000164 RID: 356 RVA: 0x00006B46 File Offset: 0x00004D46
	public override float KeyValue
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x06000165 RID: 357 RVA: 0x00006B4D File Offset: 0x00004D4D
	public override float KeyValue2
	{
		get
		{
			return 0.05f;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000166 RID: 358 RVA: 0x00006B54 File Offset: 0x00004D54
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000167 RID: 359 RVA: 0x00006B57 File Offset: 0x00004D57
	public override bool IsTimeBase
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00006B5A File Offset: 0x00004D5A
	public override void Affect(int stacks)
	{
		this.intensifyValue = this.KeyValue;
		this.Target.DamageStrategy.ApplyBuffDmgIntensify(-this.KeyValue);
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00006B7F File Offset: 0x00004D7F
	public override void Tick(float delta)
	{
		if (this.intensifyValue > 0f)
		{
			this.intensifyValue -= this.KeyValue2;
			this.Target.DamageStrategy.ApplyBuffDmgIntensify(this.KeyValue2);
		}
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00006BB7 File Offset: 0x00004DB7
	public override void End()
	{
	}

	// Token: 0x0400011A RID: 282
	private float intensifyValue;
}
