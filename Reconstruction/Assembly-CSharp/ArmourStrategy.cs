using System;

// Token: 0x02000121 RID: 289
public class ArmourStrategy : DamageStrategy
{
	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06000775 RID: 1909 RVA: 0x000141BD File Offset: 0x000123BD
	public override bool IsEnemy
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06000776 RID: 1910 RVA: 0x000141C0 File Offset: 0x000123C0
	public override float PathProgress
	{
		get
		{
			return (float)(this.armor.EnemyParent.PointIndex / this.armor.EnemyParent.PathPoints.Count);
		}
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06000777 RID: 1911 RVA: 0x000141E9 File Offset: 0x000123E9
	public override int PathIndex
	{
		get
		{
			return this.armor.EnemyParent.PointIndex;
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06000778 RID: 1912 RVA: 0x000141FB File Offset: 0x000123FB
	public override float DamageIntensify
	{
		get
		{
			return this.armor.EnemyParent.DamageStrategy.DamageIntensify;
		}
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06000779 RID: 1913 RVA: 0x00014212 File Offset: 0x00012412
	public override bool IsStun
	{
		get
		{
			return this.armor.EnemyParent.DamageStrategy.IsStun;
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x0600077A RID: 1914 RVA: 0x00014229 File Offset: 0x00012429
	public override bool IsFrost
	{
		get
		{
			return this.armor.EnemyParent.DamageStrategy.IsFrost;
		}
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00014240 File Offset: 0x00012440
	public ArmourStrategy(IDamage damageTarget, float dmgResist) : base(damageTarget)
	{
		this.HiddenResist = dmgResist;
		this.damageTarget = damageTarget;
		this.armor = (damageTarget as Armor);
		this.ModelTrans = this.armor.transform;
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x0600077C RID: 1916 RVA: 0x00014274 File Offset: 0x00012474
	// (set) Token: 0x0600077D RID: 1917 RVA: 0x0001427C File Offset: 0x0001247C
	public override bool IsDie
	{
		get
		{
			return base.IsDie;
		}
		set
		{
			base.IsDie = value;
			if (value)
			{
				this.armor.DisArmor();
			}
		}
	}

	// Token: 0x040003AE RID: 942
	private Armor armor;
}
