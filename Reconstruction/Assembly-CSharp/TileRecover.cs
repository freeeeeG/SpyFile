using System;

// Token: 0x0200003D RID: 61
public class TileRecover : TileBuff
{
	// Token: 0x17000074 RID: 116
	// (get) Token: 0x0600017D RID: 381 RVA: 0x00006CA5 File Offset: 0x00004EA5
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.TileRecover;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x0600017E RID: 382 RVA: 0x00006CA9 File Offset: 0x00004EA9
	public override float BasicDuration
	{
		get
		{
			return 999f;
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x0600017F RID: 383 RVA: 0x00006CB0 File Offset: 0x00004EB0
	public override float KeyValue
	{
		get
		{
			return 0.01f;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000180 RID: 384 RVA: 0x00006CB7 File Offset: 0x00004EB7
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000181 RID: 385 RVA: 0x00006CBA File Offset: 0x00004EBA
	public override bool IsTimeBase
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000182 RID: 386 RVA: 0x00006CBD File Offset: 0x00004EBD
	public override void Affect(int stacks)
	{
	}

	// Token: 0x06000183 RID: 387 RVA: 0x00006CBF File Offset: 0x00004EBF
	public override void Tick(float delta)
	{
		base.Tick(delta);
		this.Target.DamageStrategy.CurrentHealth += this.Target.DamageStrategy.MaxHealth * this.KeyValue;
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00006CF6 File Offset: 0x00004EF6
	public override void End()
	{
	}
}
