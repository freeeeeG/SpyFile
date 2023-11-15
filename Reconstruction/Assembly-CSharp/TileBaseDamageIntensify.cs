using System;

// Token: 0x02000031 RID: 49
public class TileBaseDamageIntensify : TileBuff
{
	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000113 RID: 275 RVA: 0x00006615 File Offset: 0x00004815
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.TileBaseDamageIntensify;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000114 RID: 276 RVA: 0x00006618 File Offset: 0x00004818
	public override bool IsStackable
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000115 RID: 277 RVA: 0x0000661B File Offset: 0x0000481B
	public override bool IsTimeBase
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000116 RID: 278 RVA: 0x0000661E File Offset: 0x0000481E
	public override float KeyValue
	{
		get
		{
			return 0.35f;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000117 RID: 279 RVA: 0x00006625 File Offset: 0x00004825
	public override float BasicDuration
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06000118 RID: 280 RVA: 0x0000662C File Offset: 0x0000482C
	public override void Affect(int stacks)
	{
		this.Target.DamageStrategy.ApplyBuffDmgIntensify(this.KeyValue * (float)stacks);
		this.Target.HealthBar.ShowIcon(0, true);
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00006659 File Offset: 0x00004859
	public override void End()
	{
		this.Target.DamageStrategy.ApplyBuffDmgIntensify(-this.KeyValue * (float)this.PrivateStacks);
		this.Target.HealthBar.ShowIcon(0, false);
	}
}
