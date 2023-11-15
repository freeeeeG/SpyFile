using System;

// Token: 0x02000123 RID: 291
public class AircraftStrategy : DamageStrategy
{
	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x0600078D RID: 1933 RVA: 0x00014595 File Offset: 0x00012795
	public override bool IsEnemy
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x0600078E RID: 1934 RVA: 0x00014598 File Offset: 0x00012798
	public override float PathProgress
	{
		get
		{
			return (float)(this.aircraft.boss.PointIndex / this.aircraft.boss.PathPoints.Count);
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x0600078F RID: 1935 RVA: 0x000145C1 File Offset: 0x000127C1
	public override int PathIndex
	{
		get
		{
			return this.aircraft.boss.PointIndex;
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06000790 RID: 1936 RVA: 0x000145D3 File Offset: 0x000127D3
	public override float DamageIntensify
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06000791 RID: 1937 RVA: 0x000145DA File Offset: 0x000127DA
	public override bool IsStun
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06000792 RID: 1938 RVA: 0x000145DD File Offset: 0x000127DD
	public override bool IsFrost
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06000793 RID: 1939 RVA: 0x000145E0 File Offset: 0x000127E0
	// (set) Token: 0x06000794 RID: 1940 RVA: 0x000145E8 File Offset: 0x000127E8
	public override bool IsDie
	{
		get
		{
			return base.IsDie;
		}
		set
		{
			base.IsDie = value;
			if (this.IsDie)
			{
				this.aircraft.boss.DamageStrategy.ApplyBuffDmgIntensify(this.DmgIntensifyWhenDie);
			}
		}
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x00014614 File Offset: 0x00012814
	public AircraftStrategy(IDamage damageTarget, float dmgIntenWhenDie, float dmgResist) : base(damageTarget)
	{
		this.HiddenResist = dmgResist;
		this.damageTarget = damageTarget;
		this.aircraft = (damageTarget as Aircraft);
		this.ModelTrans = this.aircraft.transform;
		this.DmgIntensifyWhenDie = dmgIntenWhenDie;
	}

	// Token: 0x040003B4 RID: 948
	private Aircraft aircraft;

	// Token: 0x040003B5 RID: 949
	public float DmgIntensifyWhenDie;
}
