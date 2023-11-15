using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class ConcreteContent : GameTileContent, IGameBehavior
{
	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0001EF6C File Offset: 0x0001D16C
	public override bool IsWalkable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x06000BEB RID: 3051 RVA: 0x0001EF6F File Offset: 0x0001D16F
	// (set) Token: 0x06000BEC RID: 3052 RVA: 0x0001EF77 File Offset: 0x0001D177
	public bool Dropped { get; set; }

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x06000BED RID: 3053 RVA: 0x0001EF80 File Offset: 0x0001D180
	// (set) Token: 0x06000BEE RID: 3054 RVA: 0x0001EF88 File Offset: 0x0001D188
	public List<TargetPoint> Target
	{
		get
		{
			return this.target;
		}
		set
		{
			this.target = value;
		}
	}

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x06000BEF RID: 3055 RVA: 0x0001EF91 File Offset: 0x0001D191
	// (set) Token: 0x06000BF0 RID: 3056 RVA: 0x0001EF99 File Offset: 0x0001D199
	public BuffableTurret Buffable { get; set; }

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x0001EFA2 File Offset: 0x0001D1A2
	public bool IsAttacking
	{
		get
		{
			return this.targetList.Count > 0 && this.Activated;
		}
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x0001EFBC File Offset: 0x0001D1BC
	protected virtual void Awake()
	{
		this.RangeHolder = Object.Instantiate<RangeHolder>(Singleton<StaticData>.Instance.RangeHolder, base.transform);
		this.rotTrans = base.transform.Find("RotPoint");
		this.shootPoint = this.rotTrans.Find("ShootPoint");
		this.CannonSprite = this.rotTrans.Find("Cannon").GetComponent<SpriteRenderer>();
		this.Buffable = base.GetComponent<BuffableTurret>();
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x0001F037 File Offset: 0x0001D237
	public virtual bool GameUpdate()
	{
		this.OnActivating();
		this.SkillUpdate();
		this.Buffable.TimeTick();
		return true;
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x0001F054 File Offset: 0x0001D254
	protected virtual void SkillUpdate()
	{
		foreach (TurretSkill turretSkill in this.Strategy.TurretSkills)
		{
			if (!turretSkill.IsFinish)
			{
				turretSkill.Tick(Time.deltaTime);
			}
		}
		foreach (GlobalSkill globalSkill in this.Strategy.GlobalSkills)
		{
			if (!globalSkill.IsFinish)
			{
				globalSkill.Tick(Time.deltaTime);
			}
		}
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x0001F10C File Offset: 0x0001D30C
	public void ShowRange(bool show)
	{
		this.isShowingRange = show;
		Singleton<GameManager>.Instance.ShowConcreteRange(this, show);
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x0001F121 File Offset: 0x0001D321
	public virtual void GenerateRange()
	{
		this.RangeHolder.SetRange();
		if (RangeContainer.ShowingConcrete == this)
		{
			this.ShowRange(this.isShowingRange);
		}
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x0001F147 File Offset: 0x0001D347
	protected virtual void OnActivating()
	{
		if (this.frostTime > 0f)
		{
			this.frostTime -= Time.deltaTime;
			if (this.frostTime <= 0f)
			{
				this.UnFrost();
			}
		}
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x0001F17C File Offset: 0x0001D37C
	public void UnFrost()
	{
		this.Activated = true;
		if (this.frostEffect != null)
		{
			this.frostEffect.Broke();
			this.frostEffect = null;
		}
		this.frostTime = 0f;
		foreach (TurretSkill turretSkill in this.Strategy.TurretSkills)
		{
			turretSkill.OnUnFrost();
		}
		foreach (GlobalSkill globalSkill in this.Strategy.GlobalSkills)
		{
			globalSkill.OnUnFrost();
		}
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x0001F248 File Offset: 0x0001D448
	public virtual void Frost(float time, FrostEffect effect = null)
	{
		if (this.Strategy == null)
		{
			Debug.LogWarning("Strategy=null问题出现");
			return;
		}
		this.Activated = false;
		this.frostTime = Mathf.Max(0.2f, time * (1f - this.Strategy.FinalFrostResist - GameRes.TurretFrostResist));
		if (this.frostEffect != null)
		{
			this.frostEffect.Broke();
		}
		this.frostEffect = effect;
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x0001F2B8 File Offset: 0x0001D4B8
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		if (this.Dropped)
		{
			StaticData.SetNodeWalkable(base.m_GameTile, false, true);
		}
		this.Target.Clear();
		this.targetList.Clear();
		this.Dropped = false;
		this.Strategy = null;
		this.frostTime = 0f;
		if (this.frostEffect != null)
		{
			this.frostEffect.Broke();
			this.frostEffect = null;
		}
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x0001F330 File Offset: 0x0001D530
	public virtual void TurnClear()
	{
		if (this.frostEffect != null)
		{
			this.frostEffect.Broke();
			this.frostTime = 0f;
			this.frostEffect = null;
			this.Activated = true;
		}
		this.Buffable.ClearBuffs();
		this.Target.Clear();
		this.targetList.Clear();
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x0001F390 File Offset: 0x0001D590
	public override void OnContentSelected(bool value)
	{
		base.OnContentSelected(value);
		this.ShowRange(value);
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x0001F3A0 File Offset: 0x0001D5A0
	public override void ContentLanded()
	{
		base.ContentLanded();
		Collider2D col = StaticData.RaycastCollider(base.transform.position, LayerMask.GetMask(new string[]
		{
			StaticData.ConcreteTileMask
		}));
		this.ContentLandedCheck(col);
		this.Dropped = true;
		StaticData.SetNodeWalkable(base.m_GameTile, false, false);
		Singleton<GameManager>.Instance.TriggerDetectSkills();
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x0001F408 File Offset: 0x0001D608
	protected override void ContentLandedCheck(Collider2D col)
	{
		base.ContentLandedCheck(col);
		if (col != null)
		{
			GameTile component = col.GetComponent<GameTile>();
			Singleton<ObjectPool>.Instance.UnSpawn(component);
		}
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x0001F437 File Offset: 0x0001D637
	public virtual void AddTarget(TargetPoint target)
	{
		if (target.gameObject.activeInHierarchy)
		{
			this.targetList.Add(target);
		}
		this.Strategy.EnterSkill(target.Enemy);
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x0001F464 File Offset: 0x0001D664
	public virtual void RemoveTarget(TargetPoint target)
	{
		if (this.targetList.Contains(target))
		{
			if (this.Target.Contains(target))
			{
				this.Target.Remove(target);
			}
			this.targetList.Remove(target);
			this.Strategy.ExitSkill(target.Enemy);
		}
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x0001F4B8 File Offset: 0x0001D6B8
	public virtual void SetGraphic()
	{
	}

	// Token: 0x040005F5 RID: 1525
	public StrategyBase Strategy;

	// Token: 0x040005F7 RID: 1527
	public bool Activated;

	// Token: 0x040005F8 RID: 1528
	private RangeHolder RangeHolder;

	// Token: 0x040005F9 RID: 1529
	private float frostTime;

	// Token: 0x040005FA RID: 1530
	private FrostEffect frostEffect;

	// Token: 0x040005FB RID: 1531
	private bool isShowingRange;

	// Token: 0x040005FC RID: 1532
	protected Transform rotTrans;

	// Token: 0x040005FD RID: 1533
	protected Transform shootPoint;

	// Token: 0x040005FE RID: 1534
	protected SpriteRenderer CannonSprite;

	// Token: 0x040005FF RID: 1535
	public List<TargetPoint> targetList = new List<TargetPoint>();

	// Token: 0x04000600 RID: 1536
	private List<TargetPoint> target = new List<TargetPoint>();
}
