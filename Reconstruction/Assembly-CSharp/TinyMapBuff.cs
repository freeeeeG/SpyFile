using System;

// Token: 0x0200002D RID: 45
public class TinyMapBuff : TileBuff
{
	// Token: 0x1700001E RID: 30
	// (get) Token: 0x060000EB RID: 235 RVA: 0x00006410 File Offset: 0x00004610
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.TinyMap;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x060000EC RID: 236 RVA: 0x00006414 File Offset: 0x00004614
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x060000ED RID: 237 RVA: 0x0000641B File Offset: 0x0000461B
	public override float BasicDuration
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x060000EE RID: 238 RVA: 0x00006422 File Offset: 0x00004622
	public override bool IsTimeBase
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x060000EF RID: 239 RVA: 0x00006425 File Offset: 0x00004625
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00006428 File Offset: 0x00004628
	public override void Tick(float delta)
	{
		base.Tick(delta);
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00006431 File Offset: 0x00004631
	public override void Affect(int stacks)
	{
		this.Target.DamageStrategy.ApplyBuffDmgIntensify(this.KeyValue);
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00006449 File Offset: 0x00004649
	public override void End()
	{
		this.Target.DamageStrategy.ApplyBuffDmgIntensify(-this.KeyValue);
	}
}
