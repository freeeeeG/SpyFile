using System;

// Token: 0x02000033 RID: 51
public class BreakIntensify : TimeBuff
{
	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000124 RID: 292 RVA: 0x00006701 File Offset: 0x00004901
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.BreakIntensify;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000125 RID: 293 RVA: 0x00006704 File Offset: 0x00004904
	public override bool IsStackable
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000126 RID: 294 RVA: 0x00006707 File Offset: 0x00004907
	public override bool IsTimeBase
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000127 RID: 295 RVA: 0x0000670A File Offset: 0x0000490A
	public override int MaxStacks
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000128 RID: 296 RVA: 0x0000670D File Offset: 0x0000490D
	public override float KeyValue
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000129 RID: 297 RVA: 0x00006714 File Offset: 0x00004914
	public override float BasicDuration
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x0600012A RID: 298 RVA: 0x0000671B File Offset: 0x0000491B
	public override void Affect(int stacks)
	{
		this.Target.DamageStrategy.ApplyBuffDmgIntensify(this.KeyValue * (float)stacks);
		this.Target.HealthBar.ShowIcon(2, true);
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00006748 File Offset: 0x00004948
	public override void End()
	{
		this.Target.DamageStrategy.ApplyBuffDmgIntensify(-this.KeyValue * (float)this.CurrentStack);
		this.Target.HealthBar.ShowIcon(2, false);
	}
}
