using System;
using UnityEngine;

// Token: 0x020000CA RID: 202
public class Armor : MonoBehaviour, IDamage
{
	// Token: 0x17000246 RID: 582
	// (get) Token: 0x0600051C RID: 1308 RVA: 0x0000E1B4 File Offset: 0x0000C3B4
	public string ExplosionSound
	{
		get
		{
			return "Sound_EnemyExplosion";
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x0600051D RID: 1309 RVA: 0x0000E1BB File Offset: 0x0000C3BB
	// (set) Token: 0x0600051E RID: 1310 RVA: 0x0000E1C3 File Offset: 0x0000C3C3
	public HealthBar_Sprie HealthBar
	{
		get
		{
			return this.m_HealthBar;
		}
		set
		{
			this.m_HealthBar = value;
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x0600051F RID: 1311 RVA: 0x0000E1CC File Offset: 0x0000C3CC
	// (set) Token: 0x06000520 RID: 1312 RVA: 0x0000E1D4 File Offset: 0x0000C3D4
	public DamageStrategy DamageStrategy { get; set; }

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06000521 RID: 1313 RVA: 0x0000E1DD File Offset: 0x0000C3DD
	// (set) Token: 0x06000522 RID: 1314 RVA: 0x0000E1E5 File Offset: 0x0000C3E5
	public SpriteRenderer gfxSprite
	{
		get
		{
			return this.ArmorSprite;
		}
		set
		{
			this.ArmorSprite = value;
		}
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06000523 RID: 1315 RVA: 0x0000E1EE File Offset: 0x0000C3EE
	// (set) Token: 0x06000524 RID: 1316 RVA: 0x0000E1F6 File Offset: 0x0000C3F6
	public Collider2D TargetCollider
	{
		get
		{
			return this.ArmorCol;
		}
		set
		{
			this.ArmorCol = value;
		}
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06000525 RID: 1317 RVA: 0x0000E1FF File Offset: 0x0000C3FF
	// (set) Token: 0x06000526 RID: 1318 RVA: 0x0000E207 File Offset: 0x0000C407
	public Enemy EnemyParent { get; set; }

	// Token: 0x06000527 RID: 1319 RVA: 0x0000E210 File Offset: 0x0000C410
	public void Initialize(Enemy enemyParent, float maxHealth, ArmorHolder arHolder)
	{
		this.EnemyParent = enemyParent;
		this.armorHolder = arHolder;
		this.DamageStrategy = new ArmourStrategy(this, this.EnemyParent.DmgResist);
		this.DamageStrategy.MaxHealth = maxHealth;
		this.DamageStrategy.IsDie = false;
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0000E250 File Offset: 0x0000C450
	public virtual void DisArmor()
	{
		base.transform.localScale = Vector3.zero;
		Singleton<ObjectPool>.Instance.Spawn(this.ExplosionPrefab).transform.position = base.transform.position;
		Singleton<Sound>.Instance.PlayEffect(this.ExplosionSound);
		this.armorHolder.RemoveArmor(1);
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x0000E2AE File Offset: 0x0000C4AE
	public virtual void ReArmor()
	{
		base.transform.localScale = Vector3.one;
		this.DamageStrategy.CurrentHealth = this.DamageStrategy.MaxHealth;
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0000E2D8 File Offset: 0x0000C4D8
	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		this.bullet = collision.GetComponent<Bullet>();
		this.bullet.DealRealDamage(this.bullet.FinalDamage, this, base.transform.position, true, false);
		Singleton<GameManager>.Instance.nonEnemies.Remove(this.bullet);
		this.bullet.ReclaimBullet();
		ParticalControl particalControl = Singleton<ObjectPool>.Instance.Spawn(this.SputteringEffect) as ParticalControl;
		particalControl.transform.position = base.transform.position;
		particalControl.transform.localScale = Vector3.one * 0.3f;
		particalControl.PlayEffect();
	}

	// Token: 0x04000210 RID: 528
	[SerializeField]
	private ReusableObject ExplosionPrefab;

	// Token: 0x04000211 RID: 529
	[SerializeField]
	private ParticalControl SputteringEffect;

	// Token: 0x04000212 RID: 530
	[SerializeField]
	private SpriteRenderer ArmorSprite;

	// Token: 0x04000213 RID: 531
	[SerializeField]
	private Collider2D ArmorCol;

	// Token: 0x04000214 RID: 532
	[SerializeField]
	private HealthBar_Sprie m_HealthBar;

	// Token: 0x04000217 RID: 535
	private ArmorHolder armorHolder;

	// Token: 0x04000218 RID: 536
	private Bullet bullet;
}
