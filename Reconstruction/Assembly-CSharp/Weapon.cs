using System;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public abstract class Weapon : ReusableObject, IDamage, IGameBehavior
{
	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06000593 RID: 1427 RVA: 0x0000F401 File Offset: 0x0000D601
	public string ExplosionSound
	{
		get
		{
			return "Sound_EnemyExplosion";
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06000594 RID: 1428 RVA: 0x0000F408 File Offset: 0x0000D608
	// (set) Token: 0x06000595 RID: 1429 RVA: 0x0000F410 File Offset: 0x0000D610
	public DamageStrategy DamageStrategy { get; set; }

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06000596 RID: 1430 RVA: 0x0000F419 File Offset: 0x0000D619
	// (set) Token: 0x06000597 RID: 1431 RVA: 0x0000F421 File Offset: 0x0000D621
	public Collider2D TargetCollider
	{
		get
		{
			return this.weaponCol;
		}
		set
		{
			this.weaponCol = value;
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06000598 RID: 1432 RVA: 0x0000F42A File Offset: 0x0000D62A
	// (set) Token: 0x06000599 RID: 1433 RVA: 0x0000F432 File Offset: 0x0000D632
	public SpriteRenderer gfxSprite
	{
		get
		{
			return this.m_SpriteRenderer;
		}
		set
		{
			this.m_SpriteRenderer = value;
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x0600059A RID: 1434 RVA: 0x0000F43B File Offset: 0x0000D63B
	// (set) Token: 0x0600059B RID: 1435 RVA: 0x0000F443 File Offset: 0x0000D643
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

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x0600059C RID: 1436 RVA: 0x0000F44C File Offset: 0x0000D64C
	public Knight Knight
	{
		get
		{
			return this.m_Knight;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x0600059D RID: 1437 RVA: 0x0000F454 File Offset: 0x0000D654
	public float MoveSpeed
	{
		get
		{
			if (!this.DamageStrategy.IsFrost)
			{
				return this.speed;
			}
			return 0f;
		}
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0000F470 File Offset: 0x0000D670
	public virtual void Initiate(Knight knight)
	{
		this.m_Knight = knight;
		this.DamageStrategy = new WeaponStrategy(this, this.Knight.DmgResist, this.frost);
		this.DamageStrategy.MaxHealth = knight.DamageStrategy.MaxHealth * this.healthPercent;
		this.HealthBar.FrostAmount = 0f;
		Singleton<GameManager>.Instance.nonEnemies.Add(this);
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0000F4DE File Offset: 0x0000D6DE
	public bool GameUpdate()
	{
		if (this.DamageStrategy.IsDie || this.Knight.DamageStrategy.IsDie)
		{
			this.OnDie();
			return false;
		}
		this.DamageStrategy.StrategyUpdate();
		this.MoveToKnight();
		return true;
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0000F51C File Offset: 0x0000D71C
	private void MoveToKnight()
	{
		float num = Vector2.Distance(base.transform.position, this.m_Knight.gfxSprite.transform.position);
		float num2 = 0f;
		if (num < 0.1f)
		{
			this.TriggerWeapon();
			Singleton<ObjectPool>.Instance.UnSpawn(this);
			Singleton<GameManager>.Instance.nonEnemies.Remove(this);
			return;
		}
		if (num < 1.5f)
		{
			num2 = 1f;
		}
		base.transform.position = Vector2.MoveTowards(base.transform.position, this.m_Knight.gfxSprite.transform.position, this.SpeedModify * (num2 + this.MoveSpeed) * Time.deltaTime);
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0000F5EB File Offset: 0x0000D7EB
	protected virtual void TriggerWeapon()
	{
		this.Knight.weaponInScene--;
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x0000F600 File Offset: 0x0000D800
	private void OnDie()
	{
		this.Knight.weaponInScene--;
		Singleton<ObjectPool>.Instance.Spawn(this.m_ExplosionPrefab).transform.position = base.transform.position;
		Singleton<Sound>.Instance.PlayEffect(this.ExplosionSound);
		Singleton<ObjectPool>.Instance.UnSpawn(this);
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0000F660 File Offset: 0x0000D860
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.DamageStrategy.UnFrost();
	}

	// Token: 0x04000263 RID: 611
	private const float speedUpDis = 1.5f;

	// Token: 0x04000264 RID: 612
	[SerializeField]
	private ReusableObject m_ExplosionPrefab;

	// Token: 0x04000265 RID: 613
	[SerializeField]
	private HealthBar_Sprie m_HealthBar;

	// Token: 0x04000266 RID: 614
	[SerializeField]
	private SpriteRenderer m_SpriteRenderer;

	// Token: 0x04000267 RID: 615
	[SerializeField]
	private Collider2D weaponCol;

	// Token: 0x04000268 RID: 616
	[SerializeField]
	private float healthPercent;

	// Token: 0x04000269 RID: 617
	[SerializeField]
	private float frost;

	// Token: 0x0400026A RID: 618
	[SerializeField]
	private float speed;

	// Token: 0x0400026C RID: 620
	private Knight m_Knight;

	// Token: 0x0400026D RID: 621
	public float SpeedModify = 1f;
}
