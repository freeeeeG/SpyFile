using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EB RID: 235
public abstract class Enemy : PathFollower, IDamage
{
	// Token: 0x1700026B RID: 619
	// (get) Token: 0x060005BB RID: 1467 RVA: 0x0000FA4C File Offset: 0x0000DC4C
	protected override bool IsPathfollower
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x060005BC RID: 1468 RVA: 0x0000FA4F File Offset: 0x0000DC4F
	public virtual string ExplosionSound
	{
		get
		{
			return "Sound_EnemyExplosion";
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x060005BD RID: 1469 RVA: 0x0000FA56 File Offset: 0x0000DC56
	// (set) Token: 0x060005BE RID: 1470 RVA: 0x0000FA5E File Offset: 0x0000DC5E
	public HealthBar_Sprie HealthBar { get; set; }

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x060005BF RID: 1471 RVA: 0x0000FA67 File Offset: 0x0000DC67
	// (set) Token: 0x060005C0 RID: 1472 RVA: 0x0000FA6F File Offset: 0x0000DC6F
	public DamageStrategy DamageStrategy { get; set; }

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0000FA78 File Offset: 0x0000DC78
	// (set) Token: 0x060005C2 RID: 1474 RVA: 0x0000FA80 File Offset: 0x0000DC80
	public SpriteRenderer gfxSprite
	{
		get
		{
			return this.enemySprite;
		}
		set
		{
			this.enemySprite = value;
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0000FA89 File Offset: 0x0000DC89
	// (set) Token: 0x060005C4 RID: 1476 RVA: 0x0000FA91 File Offset: 0x0000DC91
	public Collider2D TargetCollider
	{
		get
		{
			return this.enemyCol;
		}
		set
		{
			this.enemyCol = value;
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0000FA9A File Offset: 0x0000DC9A
	// (set) Token: 0x060005C6 RID: 1478 RVA: 0x0000FAA2 File Offset: 0x0000DCA2
	public BasicTile CurrentTile
	{
		get
		{
			return this.currentTile;
		}
		set
		{
			this.currentTile = value;
			this.currentTile.OnTilePass(this);
			this.Buffable.TileTick();
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0000FAC2 File Offset: 0x0000DCC2
	public bool IsEnemy
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0000FAC5 File Offset: 0x0000DCC5
	public virtual EnemyType EnemyType { get; }

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x060005C9 RID: 1481 RVA: 0x0000FACD File Offset: 0x0000DCCD
	// (set) Token: 0x060005CA RID: 1482 RVA: 0x0000FAD5 File Offset: 0x0000DCD5
	public List<TrapContent> PassedTraps
	{
		get
		{
			return this.passedTraps;
		}
		set
		{
			this.passedTraps = value;
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x060005CB RID: 1483 RVA: 0x0000FADE File Offset: 0x0000DCDE
	// (set) Token: 0x060005CC RID: 1484 RVA: 0x0000FAE6 File Offset: 0x0000DCE6
	public int ReachDamage { get; set; }

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x060005CD RID: 1485 RVA: 0x0000FAEF File Offset: 0x0000DCEF
	// (set) Token: 0x060005CE RID: 1486 RVA: 0x0000FAF7 File Offset: 0x0000DCF7
	public int AffectHealerCount
	{
		get
		{
			return this.affectHealerCount;
		}
		set
		{
			this.affectHealerCount = value;
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x060005CF RID: 1487 RVA: 0x0000FB00 File Offset: 0x0000DD00
	// (set) Token: 0x060005D0 RID: 1488 RVA: 0x0000FB1E File Offset: 0x0000DD1E
	public virtual float SpeedIntensify
	{
		get
		{
			return this.speedIntensify + ((this.AffectHealerCount > 0) ? 0.6f : 0f);
		}
		set
		{
			this.speedIntensify = value;
			this.ProgressFactor = this.Speed * base.Adjust;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0000FB3A File Offset: 0x0000DD3A
	public override float Speed
	{
		get
		{
			return (this.speed + this.SpeedIntensify) * GameRes.TurnSpeedAdjust * this.SpeedAdjust;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0000FB56 File Offset: 0x0000DD56
	// (set) Token: 0x060005D3 RID: 1491 RVA: 0x0000FB5E File Offset: 0x0000DD5E
	public float SpeedAdjust
	{
		get
		{
			return this.speedAdjust;
		}
		set
		{
			this.speedAdjust = value;
			this.ProgressFactor = this.Speed * base.Adjust;
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0000FB7A File Offset: 0x0000DD7A
	// (set) Token: 0x060005D5 RID: 1493 RVA: 0x0000FB95 File Offset: 0x0000DD95
	public override float ProgressFactor
	{
		get
		{
			if (!this.DamageStrategy.IsControlled)
			{
				return base.ProgressFactor;
			}
			return 0f;
		}
		set
		{
			base.ProgressFactor = value;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0000FB9E File Offset: 0x0000DD9E
	// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0000FBA6 File Offset: 0x0000DDA6
	public BuffableEnemy Buffable { get; set; }

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0000FBAF File Offset: 0x0000DDAF
	public virtual int MaxAmount
	{
		get
		{
			return 200;
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0000FBB6 File Offset: 0x0000DDB6
	// (set) Token: 0x060005DA RID: 1498 RVA: 0x0000FBBE File Offset: 0x0000DDBE
	public ArmorHolder HoldingArmor { get; set; }

	// Token: 0x060005DB RID: 1499 RVA: 0x0000FBC8 File Offset: 0x0000DDC8
	public virtual void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		this.m_Att = attribute;
		this.DmgResist = dmgResist;
		this.pathTiles = BoardSystem.shortestPath;
		base.PathOffset = pathOffset;
		this.Intensify = intensify;
		this.DamageStrategy.ResetStrategy(attribute, intensify, dmgResist);
		this.speed = attribute.Speed;
		this.ReachDamage = attribute.ReachDamage;
		foreach (BuffInfo buffInfo in EnemyBuffFactory.GlobalBuffs)
		{
			this.DamageStrategy.ApplyBuff(buffInfo);
		}
		this.SpawnOn(pathIndex, pathPoints);
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0000FC7C File Offset: 0x0000DE7C
	protected override void Awake()
	{
		base.Awake();
		this.SetStrategy();
		this.HealthBar = this.model.GetComponentInChildren<HealthBar_Sprie>();
		this.enemySprite = this.model.Find("GFX").GetComponent<SpriteRenderer>();
		this.Buffable = base.GetComponent<BuffableEnemy>();
		this.enemyCol = this.enemySprite.GetComponent<Collider2D>();
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0000FCEA File Offset: 0x0000DEEA
	protected virtual void SetStrategy()
	{
		this.DamageStrategy = new BasicEnemyStrategy(this);
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0000FCF8 File Offset: 0x0000DEF8
	public override bool GameUpdate()
	{
		if (this.isOutTroing)
		{
			return true;
		}
		this.OnEnemyUpdate();
		base.Progress += Time.deltaTime * this.ProgressFactor;
		if (base.Progress >= 0.5f && !this.trapTriggered)
		{
			this.trapTriggered = true;
			this.CurrentTile = this.pathTiles[this.PointIndex];
		}
		if (base.Progress >= 1f)
		{
			if (this.PointIndex == base.PathPoints.Count - 1)
			{
				this.isOutTroing = true;
				this.anim.SetTrigger("Exit");
				this.DamageStrategy.GFXFade(false);
				return true;
			}
			this.trapTriggered = false;
			base.Progress = 0f;
			this.PrepareNextState();
		}
		if (this.DirectionChange == DirectionChange.None)
		{
			base.transform.localPosition = Vector3.LerpUnclamped(this.positionFrom, this.positionTo, base.Progress);
		}
		else
		{
			float z = Mathf.LerpUnclamped(this.directionAngleFrom, this.directionAngleTo, base.Progress);
			base.transform.localRotation = Quaternion.Euler(0f, 0f, z);
		}
		return true;
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0000FE20 File Offset: 0x0000E020
	protected virtual void OnEnemyUpdate()
	{
		this.DamageStrategy.StrategyUpdate();
		this.Buffable.TimeTick();
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0000FE38 File Offset: 0x0000E038
	public virtual void OnDie()
	{
		Singleton<ObjectPool>.Instance.Spawn(this.ExplosionPrefab).transform.position = this.model.position;
		Singleton<Sound>.Instance.PlayEffect(this.ExplosionSound);
		Singleton<GameEvents>.Instance.EnemyDie(this);
		Singleton<ObjectPool>.Instance.UnSpawn(this);
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0000FE90 File Offset: 0x0000E090
	public virtual void EnemyExit()
	{
		if (!this.DamageStrategy.IsDie)
		{
			((BasicEnemyStrategy)this.DamageStrategy).UnFrost();
			Singleton<GameEvents>.Instance.EnemyReach(this);
			Singleton<ObjectPool>.Instance.UnSpawn(this);
		}
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0000FEC8 File Offset: 0x0000E0C8
	protected override void PrepareIntro()
	{
		base.PrepareIntro();
		this.CurrentTile = this.pathTiles[this.PointIndex];
		this.anim.Play("Default");
		this.anim.SetTrigger("Enter");
		this.gfxSprite.color = new Color(1f, 1f, 1f, 0f);
		this.DamageStrategy.GFXFade(true);
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0000FF44 File Offset: 0x0000E144
	public void Flash(int distance)
	{
		this.PointIndex -= distance;
		if (this.PointIndex < 0)
		{
			this.PointIndex = 0;
			base.Progress = 0f;
		}
		else if (this.PointIndex >= base.PathPoints.Count - 1)
		{
			this.PointIndex = base.PathPoints.Count - 1;
			base.Progress = 1f;
		}
		this.CurrentPoint = base.PathPoints[this.PointIndex];
		this.CurrentTile = this.pathTiles[this.PointIndex];
		this.trapTriggered = true;
		base.transform.localPosition = base.PathPoints[this.PointIndex].PathPos;
		base.PositionFrom = this.CurrentPoint.PathPos;
		base.PositionTo = this.CurrentPoint.ExitPoint;
		base.Direction = this.CurrentPoint.PathDirection;
		this.DirectionChange = DirectionChange.None;
		this.model.localPosition = new Vector3(base.PathOffset, 0f);
		base.DirectionAngleFrom = (base.DirectionAngleTo = base.Direction.GetAngle());
		base.transform.localRotation = this.CurrentPoint.PathDirection.GetRotation();
		base.Adjust = 2f;
		this.ProgressFactor = base.Adjust * this.Speed;
		this.anim.Play("Default");
		this.anim.SetTrigger("Enter");
		this.gfxSprite.color = new Color(1f, 1f, 1f, 0f);
		this.DamageStrategy.GFXFade(true);
		if (((BasicEnemyStrategy)this.DamageStrategy).m_FrostEffect != null)
		{
			((BasicEnemyStrategy)this.DamageStrategy).m_FrostEffect.transform.position = this.model.position;
		}
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0001014C File Offset: 0x0000E34C
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.model.localScale = Vector3.one;
		this.Buffable.RemoveAllBuffs();
		if (this.HoldingArmor != null)
		{
			Object.Destroy(this.HoldingArmor.gameObject);
		}
		this.PassedTraps.Clear();
		this.isOutTroing = false;
		this.SpeedIntensify = 0f;
		this.AffectHealerCount = 0;
		this.SpeedAdjust = 1f;
		Singleton<GameManager>.Instance.enemies.Remove(this);
	}

	// Token: 0x04000275 RID: 629
	[SerializeField]
	protected ReusableObject ExplosionPrefab;

	// Token: 0x04000276 RID: 630
	[Header("基本配置")]
	protected Animator anim;

	// Token: 0x04000278 RID: 632
	protected SpriteRenderer enemySprite;

	// Token: 0x04000279 RID: 633
	protected Collider2D enemyCol;

	// Token: 0x0400027A RID: 634
	protected EnemyAttribute m_Attribute;

	// Token: 0x0400027C RID: 636
	protected List<BasicTile> pathTiles;

	// Token: 0x0400027D RID: 637
	private BasicTile currentTile;

	// Token: 0x0400027E RID: 638
	public float Intensify;

	// Token: 0x0400027F RID: 639
	public float DmgResist;

	// Token: 0x04000280 RID: 640
	protected bool isOutTroing;

	// Token: 0x04000281 RID: 641
	protected bool trapTriggered;

	// Token: 0x04000283 RID: 643
	private List<TrapContent> passedTraps = new List<TrapContent>();

	// Token: 0x04000285 RID: 645
	private int affectHealerCount;

	// Token: 0x04000286 RID: 646
	private float speedIntensify;

	// Token: 0x04000287 RID: 647
	private float speedAdjust = 1f;

	// Token: 0x04000289 RID: 649
	protected EnemyAttribute m_Att;
}
