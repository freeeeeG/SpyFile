using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C1 RID: 193
public abstract class Aircraft : ReusableObject, IDamage, IGameBehavior
{
	// Token: 0x1700023E RID: 574
	// (get) Token: 0x060004E6 RID: 1254 RVA: 0x0000D6CA File Offset: 0x0000B8CA
	public string ExplosionSound
	{
		get
		{
			return "Sound_EnemyExplosion";
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0000D6D1 File Offset: 0x0000B8D1
	public string ExplosionEffect
	{
		get
		{
			return "EnemyExplosionBlue";
		}
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x060004E8 RID: 1256 RVA: 0x0000D6D8 File Offset: 0x0000B8D8
	// (set) Token: 0x060004E9 RID: 1257 RVA: 0x0000D6E0 File Offset: 0x0000B8E0
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

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x060004EA RID: 1258 RVA: 0x0000D6E9 File Offset: 0x0000B8E9
	// (set) Token: 0x060004EB RID: 1259 RVA: 0x0000D6F1 File Offset: 0x0000B8F1
	public DamageStrategy DamageStrategy { get; set; }

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x060004EC RID: 1260 RVA: 0x0000D6FA File Offset: 0x0000B8FA
	// (set) Token: 0x060004ED RID: 1261 RVA: 0x0000D702 File Offset: 0x0000B902
	public SpriteRenderer gfxSprite
	{
		get
		{
			return this.aircraftSprite;
		}
		set
		{
			this.aircraftSprite = value;
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x060004EE RID: 1262 RVA: 0x0000D70B File Offset: 0x0000B90B
	// (set) Token: 0x060004EF RID: 1263 RVA: 0x0000D713 File Offset: 0x0000B913
	public Collider2D TargetCollider
	{
		get
		{
			return this.aircraftCol;
		}
		set
		{
			this.aircraftCol = value;
		}
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x0000D71C File Offset: 0x0000B91C
	public virtual void Initiate(AircraftCarrier boss, float maxHealth, float dmgIntenWhenDie, float dmgResist)
	{
		this.DamageStrategy = new AircraftStrategy(this, dmgIntenWhenDie, dmgResist);
		this.DamageStrategy.MaxHealth = maxHealth;
		this.DamageStrategy.IsDie = false;
		Singleton<GameManager>.Instance.nonEnemies.Add(this);
		boss.AddAircraft(this);
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x0000D75C File Offset: 0x0000B95C
	public virtual bool GameUpdate()
	{
		if (this.DamageStrategy.IsDie)
		{
			this.OnDie();
			Singleton<ObjectPool>.Instance.UnSpawn(this);
			return false;
		}
		return true;
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x0000D77F File Offset: 0x0000B97F
	private void OnDie()
	{
		Singleton<ObjectPool>.Instance.Spawn(this.ExplosionPrefab).transform.position = base.transform.position;
		Singleton<Sound>.Instance.PlayEffect(this.ExplosionSound);
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x0000D7B6 File Offset: 0x0000B9B6
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.fsm = null;
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x0000D7C8 File Offset: 0x0000B9C8
	public void PickRandomDes()
	{
		float x = Random.Range(this.boss.transform.position.x - this.maxDistanceToReturnToBoss, this.boss.transform.position.x + this.maxDistanceToReturnToBoss);
		float y = Random.Range(this.boss.transform.position.y - this.maxDistanceToReturnToBoss, this.boss.transform.position.y + this.maxDistanceToReturnToBoss);
		this.movingDirection = new Vector3(x, y) - base.transform.position;
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x0000D870 File Offset: 0x0000BA70
	public void MovingToTarget(Destination des)
	{
		switch (des)
		{
		case Destination.boss:
			this.movingDirection = this.boss.model.transform.position - base.transform.position;
			break;
		case Destination.target:
			this.movingDirection = this.targetTurret.transform.position - base.transform.position;
			break;
		}
		base.transform.Translate(Vector3.up * Time.deltaTime * this.movingSpeed);
		this.RotateTowards();
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x0000D910 File Offset: 0x0000BB10
	private void RotateTowards()
	{
		float angle = Mathf.Atan2(this.movingDirection.y, this.movingDirection.x) * 57.29578f - 90f;
		this.look_Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.look_Rotation, this.rotatingSpeed * Time.deltaTime);
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x0000D984 File Offset: 0x0000BB84
	public void Lure()
	{
		this.movingSpeed = this.originalMovingSpeed;
		this.rotatingSpeed = this.originalRotatingSpeed;
		if ((base.transform.position - this.targetTurret.transform.position).magnitude < this.minDistanceToLure)
		{
			this.movingDirection = this.targetTurret.transform.position - base.transform.position + new Vector3(0.5f, 0.5f);
			this.MovingToTarget(Destination.Random);
			return;
		}
		this.movingDirection = this.targetTurret.transform.position - base.transform.position;
		this.MovingToTarget(Destination.Random);
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x0000DA52 File Offset: 0x0000BC52
	public virtual void Attack()
	{
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x0000DA54 File Offset: 0x0000BC54
	public void ProtectMe()
	{
		this.fsm.PerformTransition(Transition.ProtectBoss);
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x0000DA64 File Offset: 0x0000BC64
	public void Protect()
	{
		this.movingSpeed = this.originalMovingSpeed;
		this.rotatingSpeed = this.originalRotatingSpeed;
		if ((base.transform.position - this.boss.transform.position).magnitude < this.minDistanceToLure)
		{
			this.movingDirection = this.boss.model.transform.position - base.transform.position + new Vector3(0.5f, 0.5f);
		}
		else
		{
			this.movingDirection = this.boss.model.transform.position - base.transform.position;
		}
		this.MovingToTarget(Destination.Random);
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0000DB38 File Offset: 0x0000BD38
	public void SearchTarget()
	{
		int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, this.exploreRange, this.attachedResult, LayerMask.GetMask(new string[]
		{
			StaticData.TurretMask
		}));
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				if (this.attachedResult[i].GetComponent<ConcreteContent>().Activated)
				{
					this.turrets.Add(this.attachedResult[i].GetComponent<ConcreteContent>());
				}
			}
			if (this.turrets.Count > 0)
			{
				int index = Random.Range(0, this.turrets.Count);
				this.targetTurret = this.turrets[index];
				this.turrets.Clear();
			}
		}
	}

	// Token: 0x040001EB RID: 491
	[SerializeField]
	private ReusableObject ExplosionPrefab;

	// Token: 0x040001EC RID: 492
	[SerializeField]
	private HealthBar_Sprie m_HealthBar;

	// Token: 0x040001ED RID: 493
	[SerializeField]
	private SpriteRenderer aircraftSprite;

	// Token: 0x040001EE RID: 494
	[SerializeField]
	private Collider2D aircraftCol;

	// Token: 0x040001F0 RID: 496
	[HideInInspector]
	public AircraftCarrier boss;

	// Token: 0x040001F1 RID: 497
	[HideInInspector]
	public ConcreteContent targetTurret;

	// Token: 0x040001F2 RID: 498
	private Quaternion look_Rotation;

	// Token: 0x040001F3 RID: 499
	protected float exploreRange = 10f;

	// Token: 0x040001F4 RID: 500
	protected Collider2D[] attachedResult = new Collider2D[10];

	// Token: 0x040001F5 RID: 501
	private List<ConcreteContent> turrets = new List<ConcreteContent>();

	// Token: 0x040001F6 RID: 502
	public readonly float minDistanceToLure = 0.1f;

	// Token: 0x040001F7 RID: 503
	public readonly float minDistanceToDealDamage = 0.75f;

	// Token: 0x040001F8 RID: 504
	[SerializeField]
	private float maxDistanceToReturnToBoss = 5f;

	// Token: 0x040001F9 RID: 505
	protected float movingSpeed = 3.5f;

	// Token: 0x040001FA RID: 506
	protected float rotatingSpeed = 2f;

	// Token: 0x040001FB RID: 507
	protected float originalMovingSpeed = 3.5f;

	// Token: 0x040001FC RID: 508
	protected float originalRotatingSpeed = 2f;

	// Token: 0x040001FD RID: 509
	protected Vector3 movingDirection;

	// Token: 0x040001FE RID: 510
	protected FSMSystem fsm;
}
