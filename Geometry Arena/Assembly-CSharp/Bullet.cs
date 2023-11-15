using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class Bullet : CommonObj
{
	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000849 RID: 2121 RVA: 0x0002F8E5 File Offset: 0x0002DAE5
	private float RangePercent
	{
		get
		{
			return 1f - this.rangeLeft / this.rangeTotal;
		}
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x0002F8FC File Offset: 0x0002DAFC
	protected virtual void UpdateProps()
	{
		this.bulletEffects.UpdateBulletEffects(this.RangePercent);
		this.bulletEffects.UpdateAbilityMods();
		this.TotalDamage = this.basicDamage * this.bulletEffects.ModDamage * this.special_CannonDamage * this.special_ReboundDamage * this.special_SplitDamage;
		this.TotalSpeed = Mathf.Min(300f, this.basicSpeed * this.bulletEffects.ModSpeed * this.special_ReboundSpeed * this.special_SplitSpeed);
		this.TotalSize = this.basicSize * this.bulletEffects.ModSize * this.splitSize * this.special_CannonSize;
		if (this.ifFrozen)
		{
			this.TotalSpeed = 0f;
		}
		if (this.bulletType == EnumBulletType.NORMAL || this.bulletType == EnumBulletType.MISSLE)
		{
			this.TotalDamage *= this.Upgrade_Suppressor();
		}
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x0002F9E0 File Offset: 0x0002DBE0
	public void UpdateDirection(Vector2 dir)
	{
		this.direction = dir.normalized;
		this.Velocity = this.direction * this.TotalSpeed;
		float z = MyTool.Vec2toAngle180(this.direction);
		base.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, z);
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x0002FA40 File Offset: 0x0002DC40
	public virtual void Awake()
	{
		this.objType = EnumObjType.BULLET;
		this.SetType();
		this.ifCrit = false;
		this.inited = false;
		this.rangeTotal = 5f;
		this.rangeLeft = 5f;
		this.special_CannonSize = 1f;
		this.special_CannonDamage = 1.0;
		this.special_ReboundDamage = 1.0;
		this.special_ReboundSpeed = 1f;
		this.special_SplitDamage = 1.0;
		this.special_SplitSpeed = 1f;
		this.splitSize = 1f;
		this.rb = base.GetComponentInChildren<Rigidbody2D>();
		EnumBulletType enumBulletType = this.bulletType;
		if (enumBulletType > EnumBulletType.GRENADE)
		{
			if (enumBulletType - EnumBulletType.MINE > 2)
			{
				return;
			}
			this.ifTrigBulletEffect = false;
			return;
		}
		else
		{
			this.ifTrigBulletEffect = true;
			if (this.bulletEffects == null)
			{
				this.bulletEffects = new BulletEffects(null);
				return;
			}
			this.bulletEffects.Init(false);
			return;
		}
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x0002FB28 File Offset: 0x0002DD28
	public virtual void AfterInit()
	{
		EnumBulletType enumBulletType = this.bulletType;
		if (enumBulletType > EnumBulletType.MISSLE)
		{
			if (enumBulletType - EnumBulletType.GRENADE > 3)
			{
			}
		}
		else
		{
			this.bloom.InitMat(this.spr_Body, ResourceLibrary.Inst.GlowIntensity_Bullet, true);
		}
		this.UpdateProps();
		this.hasDie = false;
		if (base.transform.parent == null)
		{
			base.transform.parent = ObjectPool.inst.obj_Container_Bullet.transform;
		}
		this.dirToBounce = Vector2.zero;
		if (this.bulletType != EnumBulletType.MINE && this.bulletType != EnumBulletType.LASER && Setting.Inst.Option_BulletParticle)
		{
			if (this.particle_Trail == null)
			{
				this.particle_Trail = ObjectPool.inst.GetPool_Particle_GetOrNew(ResourceLibrary.Inst.Prefab_Particle_BulletTrail);
			}
			this.particle_Trail.transform.parent = base.transform;
			this.particle_Trail.transform.localPosition = Vector2.zero;
			this.particle_Trail.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			this.particle_Trail.transform.localScale = Vector3.one;
			this.particle_Trail.Trail_Bullet_Init(this.shapeType, this.mainColor, base.TransScale);
		}
		this.lastSize = 0f;
		base.enabled = true;
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x0002FC90 File Offset: 0x0002DE90
	protected double Upgrade_Suppressor()
	{
		double num = 1.0;
		if (Battle.inst.specialEffect[79] >= 1)
		{
			double num2 = (double)DataBase.Inst.Data_Upgrades[255].buffFacs[0];
			for (int i = 9; i <= 23; i++)
			{
				if (i != 22 && Battle.inst.ForShow_UpgradeNum[i] >= 1)
				{
					num *= num2;
				}
			}
		}
		return num;
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x0002FCF8 File Offset: 0x0002DEF8
	protected void Update()
	{
		if (this.IfNeedReturn() || this.ifFrozen)
		{
			return;
		}
		this.UpdateProps();
		this.Velocity = this.direction * this.TotalSpeed;
		this.FixedUpdate_Scale();
		if (Setting.Inst.Option_BulletParticle && this.bulletType != EnumBulletType.MINE && this.particle_Trail != null)
		{
			this.particle_Trail.BulletTrail_UpdateSizeAndRotate(base.TransScale, base.transform.rotation.eulerAngles.z);
		}
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x0002FD88 File Offset: 0x0002DF88
	private void FixedUpdate()
	{
		if (this.IfNeedReturn())
		{
			return;
		}
		if (this.ifFrozen)
		{
			this.frozen_TimeLeft -= Time.fixedDeltaTime;
			if (this.frozen_TimeLeft > 0f)
			{
				return;
			}
			this.ifFrozen = false;
		}
		this.FixedUpdate_SpecialMove();
		if (this.bulletType == EnumBulletType.MINE || this.bulletType == EnumBulletType.SWORD)
		{
			return;
		}
		this.BulletSplit();
		this.SpecialInFixedUpdate();
		this.FixedUpdate_Move();
		this.FixedUpdate_Collider();
		this.AntiBug_WallDetect();
		this.Fade();
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x000051D0 File Offset: 0x000033D0
	protected virtual void SpecialInFixedUpdate()
	{
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0002FE0C File Offset: 0x0002E00C
	private bool IfNeedReturn()
	{
		if (TempData.inst.currentSceneType == EnumSceneType.BATTLE)
		{
			if (TimeManager.inst.ifPause)
			{
				return true;
			}
			if (this.source == null)
			{
				this.EndLife(true);
				return true;
			}
		}
		return !this.inited || this.hasDie;
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x0002FE60 File Offset: 0x0002E060
	protected virtual void FixedUpdate_Scale()
	{
		if (this.TotalSize == this.lastSize)
		{
			return;
		}
		this.lastSize = this.TotalSize;
		base.TransScale = Mathf.Sqrt(this.TotalSize);
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x0002FE90 File Offset: 0x0002E090
	protected virtual void FixedUpdate_Collider()
	{
		EnumBulletType enumBulletType = this.bulletType;
		if (enumBulletType > EnumBulletType.GRENADE && enumBulletType - EnumBulletType.MINE <= 2)
		{
			return;
		}
		if (this.transformBullet == null)
		{
			Debug.LogError("Error_TranformBullet==null!");
			return;
		}
		if (this.capsuleCollider2D == null)
		{
			Debug.LogError("Error_CapsuleCollider2D==null!");
			return;
		}
		float fixedDeltaTime = Time.fixedDeltaTime;
		float x = this.transformBullet.localScale.x;
		float x2 = Mathf.Max(x, this.TotalSpeed * fixedDeltaTime) / x;
		float x3 = 0f;
		this.capsuleCollider2D.size = new Vector2(x2, 1f);
		this.capsuleCollider2D.offset = new Vector2(x3, 0f);
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x000051D0 File Offset: 0x000033D0
	protected virtual void FixedUpdate_SpecialMove()
	{
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x0002FF40 File Offset: 0x0002E140
	protected virtual void FixedUpdate_Move()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		float num = Time.fixedUnscaledDeltaTime;
		num = Mathf.Min(num, FPSDetector.inst.curFixedTimeSet * 1.3f);
		base.transform.position += num * this.Velocity;
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0002FFA0 File Offset: 0x0002E1A0
	private bool GetBool_IfCanSplit()
	{
		return this.bulletType == EnumBulletType.NORMAL;
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0002FFBC File Offset: 0x0002E1BC
	private void BulletSplit()
	{
		if (this.GetBool_IfCanSplit())
		{
			if (this.bulletEffects.IfCanUseAndUse(0, this.RangePercent))
			{
				this.SubsplitOnce(this.Velocity.Rotate(-30f), 2);
				this.SubsplitOnce(this.Velocity.Rotate(30f), 2);
				this.EndLife(false);
				return;
			}
			if (this.bulletEffects.IfCanUseAndUse(1, this.RangePercent))
			{
				this.SubsplitOnce(this.Velocity.Rotate(-90f), 2);
				this.SubsplitOnce(this.Velocity.Rotate(90f), 2);
				this.EndLife(false);
				return;
			}
			if (this.bulletEffects.IfCanUseAndUse(2, this.RangePercent))
			{
				this.SubsplitOnce(this.Velocity.Rotate(-60f), 3);
				this.SubsplitOnce(this.Velocity.Rotate(60f), 3);
				this.SubsplitOnce(this.Velocity.Rotate(-180f), 3);
				this.EndLife(false);
				return;
			}
			if (this.bulletEffects.IfCanUseAndUse(3, this.RangePercent))
			{
				this.SubsplitOnce(this.Velocity.Rotate(-45f), 4);
				this.SubsplitOnce(this.Velocity.Rotate(45f), 4);
				this.SubsplitOnce(this.Velocity.Rotate(135f), 4);
				this.SubsplitOnce(this.Velocity.Rotate(-135f), 4);
				this.EndLife(false);
				return;
			}
			if (this.bulletEffects.IfCanUseAndUse(4, this.RangePercent))
			{
				this.SubsplitOnce(this.Velocity.Rotate(-90f), 4);
				this.SubsplitOnce(this.Velocity.Rotate(90f), 4);
				this.SubsplitOnce(this.Velocity.Rotate(0f), 4);
				this.SubsplitOnce(this.Velocity.Rotate(-180f), 4);
				this.EndLife(false);
				return;
			}
		}
		if (!this.ifTrigBulletEffect)
		{
			return;
		}
		if (this.bulletEffects.IfCanUseAndUse(6, this.RangePercent))
		{
			this.UpdateDirection(this.direction.Rotate((float)UnityEngine.Random.Range(0, 360)));
			return;
		}
		if (this.bulletEffects.IfCanUseAndUse(22, this.RangePercent))
		{
			this.ifFrozen = true;
			this.frozen_TimeLeft = DataBase.Inst.Data_Upgrades[22].buffFacs[0];
			return;
		}
		if (this.bulletEffects.IfCanUseAndUse(5, this.RangePercent))
		{
			this.UpdateDirection(this.direction.Rotate(180f));
			return;
		}
		if (!this.bulletEffects.IfCanUseAndUse(24, this.RangePercent))
		{
			return;
		}
		if (!base.enabled)
		{
			return;
		}
		Vector2 v = BattleManager.ChooseBattleItemGenePosInScene();
		base.transform.position = v;
		this.Special_Liangzi();
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x000051D0 File Offset: 0x000033D0
	protected virtual void Special_Liangzi()
	{
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00030294 File Offset: 0x0002E494
	private void Fade()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		float num = Time.fixedUnscaledDeltaTime;
		num = Mathf.Min(num, FPSDetector.inst.curFixedTimeSet * 1.3f);
		this.rangeLeft -= this.Velocity.magnitude * num;
		if (this.rangeLeft <= 0f)
		{
			this.EndLife(true);
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x000302FC File Offset: 0x0002E4FC
	public virtual void EndLife(bool ifNormal)
	{
		if (this.hasDie)
		{
			return;
		}
		this.hasDie = true;
		if (this.particle_Trail != null)
		{
			this.particle_Trail.WantoDestroy();
			this.particle_Trail = null;
		}
		if (ifNormal && Setting.Inst.Option_BulletParticle && (this.bulletType == EnumBulletType.MISSLE || this.bulletType == EnumBulletType.NORMAL))
		{
			Particle pool_Particle_GetOrNew = ObjectPool.inst.GetPool_Particle_GetOrNew(ResourceLibrary.Inst.Prefab_Particle_BulletBlast);
			pool_Particle_GetOrNew.transform.position = base.transform.position;
			pool_Particle_GetOrNew.Blast_Init(this.shapeType, this.mainColor, base.TransScale, true);
		}
		ObjectPool.inst.Bullet_GoPool(base.gameObject);
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x000303AC File Offset: 0x0002E5AC
	public virtual void BulletInit(EnumShapeType shape, Color cl, Vector2 direction, BasicUnit source)
	{
		this.source = source;
		Factor playerFactorTotal = source.playerFactorTotal;
		this.shapeType = shape;
		this.basicSize = playerFactorTotal.bulletSize / 10f * 2f;
		base.TransScale = MyTool.GetScaleWithSizeShape(shape, this.basicSize);
		this.basicDamage = source.Fac_ActualBulletDamage;
		this.rangeTotal = playerFactorTotal.bulletRng;
		this.rangeLeft = this.rangeTotal;
		this.basicSpeed = playerFactorTotal.bulletSpd;
		this.UpdateDirection(direction);
		this.repulse = source.Fac_ActualRepulse;
		if (MyTool.DecimalToBool(playerFactorTotal.critChc))
		{
			this.basicDamage *= (double)playerFactorTotal.critDmg;
			this.basicSize *= math.min(2f, playerFactorTotal.critDmg);
			this.basicSpeed *= Mathf.Min(2f, playerFactorTotal.critDmg);
			this.ifCrit = true;
		}
		this.mainColor = cl;
		if (this.ifCrit)
		{
			this.mainColor = this.mainColor.ApplyColorSet(ResourceLibrary.Inst.colorSet_BulletCrit);
		}
		else
		{
			this.mainColor = this.mainColor.ApplyColorSet(ResourceLibrary.Inst.colorSet_BulletNormal);
		}
		this.spr_Body.sprite = ResourceLibrary.Inst.GetSprite_Shape(this.shapeType, false);
		this.spr_Body.color = this.mainColor;
		this.SetType();
		this.AfterInit();
		if (this.bulletType != EnumBulletType.MISSLE)
		{
			this.upgrade_BounceWall = Battle.inst.ForShow_UpgradeNum[8];
			this.upgrade_BounceEnemy = Battle.inst.ForShow_UpgradeNum[7];
		}
		this.SpecialInit();
		this.inited = true;
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x0003055B File Offset: 0x0002E75B
	protected virtual void SetType()
	{
		this.bulletType = EnumBulletType.NORMAL;
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x000051D0 File Offset: 0x000033D0
	protected virtual void SpecialInit()
	{
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x00030564 File Offset: 0x0002E764
	public void MyColliderEnter2D(GameObject collision)
	{
		collision = collision.transform.root.gameObject;
		if (this.hasDie)
		{
			return;
		}
		bool flag = true;
		string tag = collision.gameObject.tag;
		if (tag == "Enemy")
		{
			if (this.bulletType == EnumBulletType.MINE)
			{
				this.EndLife(true);
				return;
			}
			Enemy component = collision.gameObject.GetComponent<Enemy>();
			this.Special_OnHitEnemy(component);
			SpecialEffects.BulletHitEnemy(this, component);
			BasicUnit component2 = collision.gameObject.GetComponent<BasicUnit>();
			if (component2.life <= 0.0)
			{
				return;
			}
			if (this.bulletType != EnumBulletType.MINE)
			{
				Vector2 hitPosition = Vector2.zero;
				if (this.bulletType == EnumBulletType.LASER || this.bulletType == EnumBulletType.SWORD)
				{
					hitPosition = component2.transform.position;
				}
				else
				{
					hitPosition = (base.transform.position + component2.transform.position) / 2f;
				}
				component2.GetHurt(this.TotalDamage, this.source, this.Velocity.normalized, this.ifCrit, hitPosition, true);
			}
			if (this.bulletType == EnumBulletType.LASER || this.bulletType == EnumBulletType.SWORD)
			{
				flag = false;
			}
			if (TempData.inst.jobId == 6 && (component2 == null || component2.life <= 0.0))
			{
				flag = false;
				this.Skill_Player6_Sizer();
				this.UpdateDirection(this.direction);
			}
			if (TempData.inst.jobId == 1 && this.bulletType == EnumBulletType.NORMAL && TempData.inst.GetBool_SkillModuleOpenFlag(3))
			{
				flag = false;
			}
			if (this.upgrade_BounceEnemy > 0 && this.bulletType != EnumBulletType.LASER && this.bulletType != EnumBulletType.SWORD)
			{
				this.upgrade_BounceEnemy--;
				flag = false;
				this.BounceFromEnemy(component);
				this.OnBounceEnemy();
			}
		}
		SoundEffects.Inst.hit.PlayRandom();
		if (flag)
		{
			this.EndLife(true);
		}
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x000051D0 File Offset: 0x000033D0
	protected virtual void Special_OnHitEnemy(Enemy enemy)
	{
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00030744 File Offset: 0x0002E944
	private void BounceFromEnemy(Enemy touchEnemy)
	{
		if (this.upgrade_BounceEnemy <= -1)
		{
			Debug.LogError("Error_无剩余敌人反弹次数！");
			return;
		}
		List<Enemy> listEnemies = BattleManager.inst.listEnemies;
		Enemy enemy = null;
		float num = 10000f;
		foreach (Enemy enemy2 in listEnemies)
		{
			if (!(enemy2 == touchEnemy))
			{
				float magnitude = (enemy2.transform.position - base.transform.position).magnitude;
				if (magnitude < num)
				{
					enemy = enemy2;
					num = magnitude;
				}
			}
		}
		if (enemy == null)
		{
			Vector2 dir = -this.direction;
			this.UpdateDirection(dir);
			return;
		}
		this.TargetTheEnemy(enemy);
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00030810 File Offset: 0x0002EA10
	protected virtual void TargetTheEnemy(Enemy enemy)
	{
		Vector2 dir = enemy.transform.position - base.transform.position;
		this.UpdateDirection(dir);
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00030845 File Offset: 0x0002EA45
	protected virtual void OnBounceWall()
	{
		this.OnBounceCommon();
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00030845 File Offset: 0x0002EA45
	protected virtual void OnBounceEnemy()
	{
		this.OnBounceCommon();
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00030850 File Offset: 0x0002EA50
	protected virtual void OnBounceCommon()
	{
		if (Battle.inst.specialEffect[66] >= 1)
		{
			Upgrade upgrade = DataBase.Inst.Data_Upgrades[260];
			this.special_ReboundDamage += (double)upgrade.buffFacs[1];
			this.special_ReboundSpeed += upgrade.buffFacs[3];
		}
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x000051D0 File Offset: 0x000033D0
	protected virtual void OnHitWall()
	{
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x000308AC File Offset: 0x0002EAAC
	private void SubsplitOnce(Vector2 dir, int splitNum)
	{
		Bullet bullet = ObjectPool.inst.GetPool_Bullet(base.gameObject.name);
		if (bullet == null)
		{
			bullet = Object.Instantiate<GameObject>(base.gameObject).GetComponent<Bullet>();
		}
		bullet.CopyFrom(this);
		bullet.bloom.CopyBlock(this.bloom.GetBlock());
		bullet.spr_Body.color = this.mainColor;
		if (Battle.inst.specialEffect[64] >= 1)
		{
			bullet.splitSize = 1f;
		}
		else
		{
			bullet.splitSize = this.splitSize * 1f / Mathf.Sqrt((float)splitNum);
		}
		if (Battle.inst.specialEffect[65] >= 1)
		{
			Upgrade upgrade = DataBase.Inst.Data_Upgrades[251];
			bullet.special_SplitDamage *= (double)upgrade.buffFacs[1];
			bullet.special_SplitSpeed *= upgrade.buffFacs[3];
		}
		dir.Normalize();
		bullet.UpdateDirection(dir);
		bullet.AfterInit();
		bullet.AfterSplit();
		if (base.GetComponent<SpecialBullet_Summon>() != null)
		{
			SpecialBullet_Summon component = base.GetComponent<SpecialBullet_Summon>();
			SpecialBullet_Summon component2 = bullet.GetComponent<SpecialBullet_Summon>();
			if (component.target != null)
			{
				component2.target = component.target;
			}
			component2.hasInitDirection = true;
		}
		bullet.inited = true;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x000051D0 File Offset: 0x000033D0
	protected virtual void AfterSplit()
	{
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x000309F8 File Offset: 0x0002EBF8
	public void CopyFrom(Bullet origin)
	{
		this.rangeLeft = origin.rangeLeft;
		this.rangeTotal = origin.rangeTotal;
		base.transform.position = origin.transform.position;
		base.name = origin.name;
		this.source = origin.source;
		this.basicDamage = origin.basicDamage;
		this.basicSpeed = origin.basicSpeed;
		this.basicSize = origin.basicSize;
		this.ifCrit = origin.ifCrit;
		this.mainColor = origin.mainColor;
		this.splitSize = origin.splitSize;
		this.upgrade_BounceEnemy = origin.upgrade_BounceEnemy;
		this.upgrade_BounceWall = origin.upgrade_BounceWall;
		this.special_CannonSize = origin.special_CannonSize;
		this.special_CannonDamage = origin.special_CannonDamage;
		this.special_ReboundDamage = origin.special_ReboundDamage;
		this.special_ReboundSpeed = origin.special_ReboundSpeed;
		this.special_SplitDamage = origin.special_SplitDamage;
		this.special_SplitSpeed = origin.special_SplitSpeed;
		if (this.bulletEffects == null)
		{
			this.bulletEffects = new BulletEffects(origin.bulletEffects);
			return;
		}
		this.bulletEffects.CloneFrom(origin.bulletEffects);
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x00030B20 File Offset: 0x0002ED20
	private void Skill_Player6_Sizer()
	{
		int num = 0;
		Skill skill = Skill.SkillCurrent(ref num);
		if (num <= 0)
		{
			Debug.LogError("skillLevel<=0!");
		}
		float[] facs = skill.facs;
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(1))
		{
			float a = this.special_CannonSize * facs[1];
			a = Mathf.Min(a, 729f);
			this.special_CannonSize = a;
		}
		double num2 = this.special_CannonDamage * (double)facs[3];
		this.special_CannonDamage = num2;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00030B88 File Offset: 0x0002ED88
	public virtual void AntiBug_WallDetect()
	{
		if (!this.antibug_DetectWall)
		{
			return;
		}
		if (this.hasDie)
		{
			return;
		}
		Vector2 vector = base.transform.position;
		float x = vector.x;
		float y = vector.y;
		float num = Mathf.Abs(x);
		float num2 = Mathf.Abs(y);
		float num3 = 22f * SceneObj.inst.SceneSize - base.TransScale / 2f;
		if (num > num3 || num2 > num3)
		{
			if (this.upgrade_BounceWall > 0)
			{
				bool flag = false;
				if (Mathf.Abs(x) - num3 > 0f && x * this.direction.x > 0f)
				{
					flag = true;
					Vector2 inNormal = new Vector2(1f, 0f);
					this.dirToBounce = Vector2.Reflect(this.direction, inNormal);
				}
				else if (Mathf.Abs(y) - num3 > 0f && y * this.direction.y > 0f)
				{
					flag = true;
					Vector2 inNormal2 = new Vector2(0f, 1f);
					this.dirToBounce = Vector2.Reflect(this.direction, inNormal2);
				}
				if (!flag)
				{
					return;
				}
				this.OnHitWall();
				this.upgrade_BounceWall--;
				this.UpdateDirection(this.dirToBounce);
				this.OnBounceWall();
				return;
			}
			else if (!this.GetBool_IfHeavyCannon())
			{
				this.EndLife(true);
			}
		}
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00030CD9 File Offset: 0x0002EED9
	private bool GetBool_IfHeavyCannon()
	{
		return TempData.inst.jobId == 6 && TempData.inst.GetBool_SkillModuleOpenFlag(3);
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x00030CFA File Offset: 0x0002EEFA
	public virtual float GetFloat_HitBackForce()
	{
		return this.TotalSpeed * this.TotalSize * this.repulse * 50f;
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00030D16 File Offset: 0x0002EF16
	public virtual Vector2 GetVector2_HitBackDirection(Transform transTarget)
	{
		return this.direction;
	}

	// Token: 0x040006CC RID: 1740
	public EnumBulletType bulletType;

	// Token: 0x040006CD RID: 1741
	[SerializeField]
	public SpriteRenderer spr_Body;

	// Token: 0x040006CE RID: 1742
	public Rigidbody2D rb;

	// Token: 0x040006CF RID: 1743
	[SerializeField]
	public BasicUnit source;

	// Token: 0x040006D0 RID: 1744
	[SerializeField]
	public Vector2 direction = Vector2.zero;

	// Token: 0x040006D1 RID: 1745
	[SerializeField]
	protected bool inited;

	// Token: 0x040006D2 RID: 1746
	public bool antibug_DetectWall = true;

	// Token: 0x040006D3 RID: 1747
	[Header("ParticelEffects")]
	[SerializeField]
	private Particle particle_Trail;

	// Token: 0x040006D4 RID: 1748
	[Header("Ability")]
	[SerializeField]
	protected double basicDamage = -1.0;

	// Token: 0x040006D5 RID: 1749
	[SerializeField]
	protected float basicSpeed = -1f;

	// Token: 0x040006D6 RID: 1750
	[SerializeField]
	protected float basicSize = -1f;

	// Token: 0x040006D7 RID: 1751
	[SerializeField]
	public float repulse;

	// Token: 0x040006D8 RID: 1752
	[SerializeField]
	protected bool hasDie;

	// Token: 0x040006D9 RID: 1753
	[SerializeField]
	public bool ifCrit;

	// Token: 0x040006DA RID: 1754
	public float splitSize = 1f;

	// Token: 0x040006DB RID: 1755
	[Header("TotalProps")]
	public double TotalDamage;

	// Token: 0x040006DC RID: 1756
	public float TotalSpeed;

	// Token: 0x040006DD RID: 1757
	public float TotalSize;

	// Token: 0x040006DE RID: 1758
	[Header("Range")]
	[SerializeField]
	protected float rangeTotal = 233f;

	// Token: 0x040006DF RID: 1759
	[SerializeField]
	protected float rangeLeft = 233f;

	// Token: 0x040006E0 RID: 1760
	[Header("BulletEffects")]
	[SerializeField]
	private BulletEffects bulletEffects;

	// Token: 0x040006E1 RID: 1761
	[Header("SpecialProps_Cannon")]
	[SerializeField]
	private float special_CannonSize = 1f;

	// Token: 0x040006E2 RID: 1762
	[SerializeField]
	private double special_CannonDamage = 1.0;

	// Token: 0x040006E3 RID: 1763
	[Header("SpecialProps_Split")]
	[SerializeField]
	private float special_SplitSpeed = 1f;

	// Token: 0x040006E4 RID: 1764
	[SerializeField]
	private double special_SplitDamage = 1.0;

	// Token: 0x040006E5 RID: 1765
	[Header("SpecialProps_Rebound")]
	[SerializeField]
	private float special_ReboundSpeed = 1f;

	// Token: 0x040006E6 RID: 1766
	[SerializeField]
	private double special_ReboundDamage = 1.0;

	// Token: 0x040006E7 RID: 1767
	[SerializeField]
	protected int upgrade_BounceEnemy;

	// Token: 0x040006E8 RID: 1768
	[SerializeField]
	protected int upgrade_BounceWall;

	// Token: 0x040006E9 RID: 1769
	[SerializeField]
	private Vector2 dirToBounce = Vector2.zero;

	// Token: 0x040006EA RID: 1770
	[Header("Frozen")]
	[SerializeField]
	private bool ifFrozen;

	// Token: 0x040006EB RID: 1771
	[SerializeField]
	private float frozen_TimeLeft;

	// Token: 0x040006EC RID: 1772
	[Header("Others")]
	public Vector2 Velocity = Vector2.zero;

	// Token: 0x040006ED RID: 1773
	[Header("Collider")]
	[SerializeField]
	private Transform transformBullet;

	// Token: 0x040006EE RID: 1774
	[SerializeField]
	private CapsuleCollider2D capsuleCollider2D;

	// Token: 0x040006EF RID: 1775
	[Header("Optimization")]
	private float lastSize;

	// Token: 0x040006F0 RID: 1776
	[SerializeField]
	protected BloomMaterialControl bloom;

	// Token: 0x040006F1 RID: 1777
	[SerializeField]
	private bool ifTrigBulletEffect;
}
