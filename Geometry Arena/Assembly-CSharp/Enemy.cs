using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public class Enemy : MonoBehaviour
{
	// Token: 0x170000FA RID: 250
	// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00032705 File Offset: 0x00030905
	// (set) Token: 0x060008B1 RID: 2225 RVA: 0x000326F6 File Offset: 0x000308F6
	public Vector2 Move_Direction
	{
		get
		{
			return this.move_CurrentDirection;
		}
		set
		{
			this.move_CurrentDirection = value.normalized;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0003270D File Offset: 0x0003090D
	public float Move_Speed
	{
		get
		{
			return this.unit.EnemyFactorTotal.moveSpd;
		}
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x00032720 File Offset: 0x00030920
	protected virtual void Awake()
	{
		if (!this.IfScene_Battle())
		{
			return;
		}
		this.fusionRank = 1;
		this.bossStage = 0;
		this.unit = base.gameObject.GetComponentInChildren<BasicUnit>();
		this.unit.SetCollis(false);
		if (this.modelID < 0)
		{
			Debug.LogError("modelID<0!");
		}
		else
		{
			this.InitEnemy(DataBase.Inst.Data_EnemyModels[this.modelID]);
		}
		this.Awake_SplitInit();
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00032794 File Offset: 0x00030994
	private void Start()
	{
		if (!this.IfScene_Battle())
		{
			return;
		}
		this.shoot_TimeLeft = 9f;
		int num = MyTool.DecimalToInt(this.shoot_TimeLeft * this.unit.EnemyFactorTotal.fireSpd);
		this.shoot_TimeLeft = (float)num / this.unit.EnemyFactorTotal.fireSpd * 1.05f;
		this.existTime = 0f;
		this.StartMove();
		if (!this.summoned && TempData.inst != null && Battle.inst != null)
		{
			this.unit.TransScale = 0.01f;
		}
		this.unit.objType = EnumObjType.ENEMY;
		BattleManager.inst.listEnemies.Add(this);
		this.fragMulti = 1f;
		this.starMulti = 1f;
		if (this.summoned)
		{
			this.fragMulti = 0f;
			this.starMulti = GameParameters.Inst.scoreSetting.Score_SummonPercent;
			BattleManager.inst.EnemyAmoutAddOne();
			this.debug_HasAmountAdd = true;
		}
		EnumRank rank = this.model.rank;
		if (rank != EnumRank.RARE)
		{
			if (rank == EnumRank.EPIC)
			{
				if (TempData.inst.diffiOptFlag[14])
				{
					this.fragMulti *= 3f;
					this.starMulti *= 3f;
				}
			}
		}
		else if (TempData.inst.diffiOptFlag[7])
		{
			this.fragMulti *= 2f;
			this.starMulti *= 2f;
		}
		this.unit.SetCollis(true);
		this.unit.inited = true;
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0001E1E3 File Offset: 0x0001C3E3
	private bool IfScene_Battle()
	{
		return TempData.inst.currentSceneType == EnumSceneType.BATTLE;
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00032930 File Offset: 0x00030B30
	private void StartMove()
	{
		switch (this.move_Type)
		{
		case EnumMoveType.UNINITED:
			Debug.LogError("MoveTypeUninited!");
			return;
		case EnumMoveType.TRACKING:
		case EnumMoveType.BOUNCE:
			break;
		case EnumMoveType.DASH:
			this.move_Dash_OnDashing = false;
			return;
		case EnumMoveType.JUMP:
			this.move_Jump_OnJumping = false;
			break;
		default:
			return;
		}
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x0003297C File Offset: 0x00030B7C
	private void Update()
	{
		if (!this.IfScene_Battle())
		{
			return;
		}
		if (TimeManager.inst.ifPause)
		{
			return;
		}
		if (Player.inst == null)
		{
			return;
		}
		this.DetectBossStage();
		this.existTime += Time.deltaTime * TimeManager.inst.currentEnemySpeed;
		this.Boss_Swallow_InUpdate();
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x000329D8 File Offset: 0x00030BD8
	private void LateUpdate()
	{
		if (!this.IfScene_Battle())
		{
			return;
		}
		if (Setting.Inst.Option_EnemyInfo)
		{
			if (this.infoDisplayText == null)
			{
				this.infoDisplayText = FloatDamageTextControl.inst.NewEnemyInfo(this);
				return;
			}
		}
		else if (this.infoDisplayText != null)
		{
			Object.Destroy(this.infoDisplayText.gameObject);
		}
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x00032A38 File Offset: 0x00030C38
	private void FixedUpdate()
	{
		if (!this.IfScene_Battle())
		{
			return;
		}
		if (TimeManager.inst.ifPause)
		{
			return;
		}
		if (Player.inst == null)
		{
			return;
		}
		this.FixedUpdateMove();
		this.FixedUpdateRotate();
		if (this.shoot_TimeLeft > 0f)
		{
			this.shoot_TimeLeft -= this.FixedDeltaTimeForEnemy();
		}
		this.FixedUpdateAutoShoot();
		this.lifeTime += this.FixedDeltaTimeForEnemy();
		this.unit.Antibug_MoveRestriction();
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x00032AB9 File Offset: 0x00030CB9
	private float DeltaTimeForEnemy()
	{
		return Time.deltaTime * TimeManager.inst.currentEnemySpeed;
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x00032ACB File Offset: 0x00030CCB
	private float FixedDeltaTimeForEnemy()
	{
		return Time.fixedDeltaTime * TimeManager.inst.currentEnemySpeed;
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x00032ADD File Offset: 0x00030CDD
	private void Move_InFixedUpdateForEnemy(Vector2 dir, float spd)
	{
		this.unit.Move_WantoMoveOnce_InFixedUpdate(dir, spd * TimeManager.inst.currentEnemySpeed);
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x00032AF8 File Offset: 0x00030CF8
	private void FixedUpdateMove()
	{
		if (this.swallowed_Boss != null)
		{
			return;
		}
		switch (this.move_Type)
		{
		case EnumMoveType.TRACKING:
			this.Move_Direction = this.Move_Direction.DirectionApproach(this.PlayerToMeVec2(), this.move_Tracking_AngelSpeed * this.FixedDeltaTimeForEnemy());
			this.LookAtDirection(this.Move_Direction);
			this.Move_InFixedUpdateForEnemy(this.Move_Direction, this.Move_Speed);
			if (this.PlayerToMeVec2().magnitude <= this.move_Tracking_DistTrig_Dist)
			{
				this.TrigControl(this.move_Tracking_DistTrig_Open, this.move_Tracking_DistTrig_TrigType);
				return;
			}
			break;
		case EnumMoveType.BOUNCE:
			this.LookAtDirection(this.Move_Direction);
			this.Move_InFixedUpdateForEnemy(this.Move_Direction, this.Move_Speed);
			return;
		case EnumMoveType.DASH:
			if (this.move_Dash_OnDashing)
			{
				this.LookAtDirection(this.Move_Direction);
				this.Move_InFixedUpdateForEnemy(this.Move_Direction, this.Move_Speed);
				return;
			}
			this.Move_TimeLeft += this.FixedDeltaTimeForEnemy();
			if (this.Move_TimeLeft >= this.move_Dash_IntervalTime)
			{
				this.move_Dash_OnDashing = true;
				this.Move_TimeLeft = 0f;
				this.Move_Direction = this.PlayerToMeVec2();
				this.TrigControl(this.move_Dash_StartTrig_Open, this.move_Dash_StartTrig_TrigType);
				return;
			}
			break;
		case EnumMoveType.JUMP:
			if (this.move_Jump_OnJumping)
			{
				this.LookAtDirection(this.Move_Direction);
				this.Move_InFixedUpdateForEnemy(this.Move_Direction, this.Move_Speed);
				this.Move_TimeLeft += this.FixedDeltaTimeForEnemy();
				if (this.Move_TimeLeft >= this.move_Jump_DurationTime)
				{
					this.move_Jump_OnJumping = false;
					this.Move_TimeLeft = 0f;
					this.TrigControl(this.move_Jump_EndTrig_Open, this.move_Jump_EndTrig_TrigType);
					return;
				}
			}
			else
			{
				this.Move_TimeLeft += this.FixedDeltaTimeForEnemy();
				if (this.Move_TimeLeft >= this.move_Jump_IntervalTime)
				{
					this.move_Jump_OnJumping = true;
					this.Move_TimeLeft = 0f;
					this.Move_Direction = this.PlayerToMeVec2().RandomDeltaEuler(-this.move_Jump_AngleDeltaRange, this.move_Jump_AngleDeltaRange);
					this.TrigControl(this.move_Jump_StartTrig_Open, this.move_Jump_StartTrig_TrigType);
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x00032D10 File Offset: 0x00030F10
	private void FixedUpdateRotate()
	{
		if (!this.rotate_Open)
		{
			return;
		}
		float smoothTime = 0.1f;
		switch (this.rotate_Type)
		{
		case EnumRotateType.AUTO:
			this.rotate_CurrenAngleSpeed = Mathf.SmoothDamp(this.rotate_CurrenAngleSpeed, this.rotate_AngleSpeed, ref this.rotate_RefAngleSpeed, smoothTime);
			break;
		case EnumRotateType.DASHONLY:
			if (this.move_Type != EnumMoveType.DASH)
			{
				Debug.LogError("Rotate_MoveTypeError!");
				return;
			}
			if (this.move_Dash_OnDashing)
			{
				this.rotate_CurrenAngleSpeed = Mathf.SmoothDamp(this.rotate_CurrenAngleSpeed, this.rotate_AngleSpeed, ref this.rotate_RefAngleSpeed, smoothTime);
			}
			else
			{
				this.rotate_CurrenAngleSpeed = Mathf.SmoothDamp(this.rotate_CurrenAngleSpeed, 0f, ref this.rotate_RefAngleSpeed, 0.1f);
			}
			break;
		case EnumRotateType.JUMPONLY:
			if (this.move_Type != EnumMoveType.JUMP)
			{
				Debug.LogError("Rotate_MoveTypeError!");
				return;
			}
			if (this.move_Jump_OnJumping)
			{
				this.rotate_CurrenAngleSpeed = Mathf.SmoothDamp(this.rotate_CurrenAngleSpeed, this.rotate_AngleSpeed, ref this.rotate_RefAngleSpeed, smoothTime);
			}
			else
			{
				this.rotate_CurrenAngleSpeed = Mathf.SmoothDamp(this.rotate_CurrenAngleSpeed, 0f, ref this.rotate_RefAngleSpeed, smoothTime);
			}
			break;
		}
		this.unit.rb.SetRotation(this.unit.rb.rotation + this.rotate_CurrenAngleSpeed * this.FixedDeltaTimeForEnemy());
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00032E58 File Offset: 0x00031058
	private void FixedUpdateAutoShoot()
	{
		if (!this.shoot_Open)
		{
			return;
		}
		if (!this.shoot_AutoShoot_Open)
		{
			return;
		}
		if (!MyTool.DecimalToBool(BattleManager.inst.MyTimeScale()))
		{
			return;
		}
		if (this.model.rank != EnumRank.EPIC && this.shoot_TimeLeft <= 0f)
		{
			return;
		}
		if (this.unit.fac_CurShootTime >= this.unit.Fac_MaxShootTime)
		{
			this.unit.prefab_Bullet = this.shoot_PrefabShootObject;
			this.unit.Shoot_WantoShootOnce(Vector2.zero);
			if (TempData.inst.diffiOptFlag[1])
			{
				base.StartCoroutine(this.ShootOnceAfter(0.09f));
			}
		}
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x00032F00 File Offset: 0x00031100
	public void Trig_ShootAtOnce()
	{
		if (!this.shoot_Open)
		{
			Debug.LogError("ShootClose!");
			return;
		}
		if (!MyTool.DecimalToBool(BattleManager.inst.MyTimeScale()))
		{
			return;
		}
		if (this.model.rank != EnumRank.EPIC && this.shoot_TimeLeft <= 0f)
		{
			return;
		}
		this.ShootOnce();
		if (TempData.inst.diffiOptFlag[1])
		{
			base.StartCoroutine(this.ShootOnceAfter(0.09f));
		}
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x00032F74 File Offset: 0x00031174
	private void ShootOnce()
	{
		this.unit.fac_CurShootTime = this.unit.Fac_MaxShootTime;
		this.unit.prefab_Bullet = this.shoot_PrefabShootObject;
		this.unit.Shoot_WantoShootOnce(Vector2.zero);
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00032FAE File Offset: 0x000311AE
	private IEnumerator ShootOnceAfter(float time)
	{
		yield return new WaitForSeconds(time);
		this.ShootOnce();
		yield break;
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x00032FC4 File Offset: 0x000311C4
	private void TrigControl(bool trigOpen, EnumTrigType type)
	{
		if (!trigOpen)
		{
			return;
		}
		switch (type)
		{
		case EnumTrigType.NONE:
			Debug.Log("EnumTrigType None!");
			return;
		case EnumTrigType.SHOOT:
			this.Trig_ShootAtOnce();
			return;
		case EnumTrigType.SPLIT:
			this.unit.Die(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x00032FFC File Offset: 0x000311FC
	private void Awake_SplitInit()
	{
		if (!this.split_Open)
		{
			return;
		}
		foreach (GameObject gameObject in this.split_ListChildObjects)
		{
			foreach (Enemy enemy in gameObject.GetComponentsInChildren<Enemy>())
			{
				enemy.summoned = true;
				enemy.enabled = false;
			}
			BasicUnit[] componentsInChildren2 = gameObject.GetComponentsInChildren<BasicUnit>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
			Rigidbody2D[] componentsInChildren3 = gameObject.GetComponentsInChildren<Rigidbody2D>();
			if (componentsInChildren3.Length != 0)
			{
				Debug.LogError(base.name + " RbLength>0 :" + componentsInChildren3.Length);
			}
			gameObject.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x000330E0 File Offset: 0x000312E0
	public void Trig_SplitAtOnce()
	{
		if (!this.split_Open)
		{
			return;
		}
		if (this.splited)
		{
			return;
		}
		if (!this.unit.inited)
		{
			Debug.LogError("Error:未初始化就触发了分裂？");
			return;
		}
		this.splited = true;
		foreach (GameObject gameObject in this.split_ListChildObjects)
		{
			if (gameObject == null)
			{
				Debug.LogWarning("Warning:Obj==null!");
			}
			else
			{
				BasicUnit component = gameObject.GetComponent<BasicUnit>();
				Enemy component2 = gameObject.GetComponent<Enemy>();
				float z = gameObject.transform.rotation.eulerAngles.z;
				Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
				if (rigidbody2D == null)
				{
					rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
				}
				else
				{
					Debug.LogWarning("警告:重复分裂");
				}
				rigidbody2D.drag = 5f;
				rigidbody2D.angularDrag = 10f;
				rigidbody2D.gravityScale = 0f;
				component.enabled = true;
				component2.enabled = true;
				component.Awake();
				component2.Awake();
				component2.UpdateFusion(this.fusionRank);
				component2.Move_Direction = MyTool.AngleToVec2(z);
				gameObject.transform.parent = null;
				Vector2 dir = MyTool.AngleToVec2(gameObject.transform.rotation.eulerAngles.z);
				component2.unit.rb.useAutoMass = false;
				component2.unit.rb.mass = 1f;
				component2.unit.PushByForce(dir, GameParameters.Inst.ForceEnemyShootOrigin);
			}
		}
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x00033298 File Offset: 0x00031498
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!this.unit.inited)
		{
			Debug.LogError("未初始化就触发碰撞");
			return;
		}
		if (this.unit.ifDie)
		{
			return;
		}
		Vector2 point = collision.contacts[0].point;
		Vector2 hitbackDirection = base.transform.position - point;
		string tag = collision.gameObject.tag;
		if (tag == "Player")
		{
			BasicUnit component = collision.gameObject.GetComponent<BasicUnit>();
			int num = TempData.inst.diffiOptFlag[8] ? 3 : 1;
			component.GetHurt((double)num, this.unit, -collision.contacts[0].normal, false, point, true);
			this.Move_HitPlayer(hitbackDirection, 1f);
			SpecialEffects.EnemyHitPlayer(component.GetComponent<Player>(), this);
			return;
		}
		if (tag == "PlayerShield")
		{
			Debug.LogError("Error_ShieldCollider");
			return;
		}
		if (!(tag == "Obstacle"))
		{
			return;
		}
		this.Move_HitWall(hitbackDirection, 1f);
		this.Special_ThornWall();
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x000333A4 File Offset: 0x000315A4
	private void OnCollisionStay2D(Collision2D collision)
	{
		string tag = collision.gameObject.tag;
		if (tag == "Player" || tag == "PlayerShield")
		{
			collision.gameObject.GetComponentInParent<Skill_Player8_Wave>();
			return;
		}
		if (!(tag == "Obstacle"))
		{
			return;
		}
		this.Move_HitWall(this.PlayerToMeVec2(), 1f);
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x00033404 File Offset: 0x00031604
	private void Special_ThornWall()
	{
		if (Battle.inst.specialEffect[78] <= 0)
		{
			return;
		}
		double dmgNum = (double)this.unit.lastSize * Player.inst.unit.playerFactorTotal.bulletDmg * (double)Player.inst.unit.playerFactorTotal.fireSpd;
		this.unit.GetHurt(dmgNum, Player.inst.unit, Vector2.zero, false, base.transform.position, true);
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x00033488 File Offset: 0x00031688
	private void Special_ThornBarrier()
	{
		if (TempData.inst.jobId != 0)
		{
			return;
		}
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(6))
		{
			return;
		}
		double bulletDmg = Player.inst.unit.playerFactorTotal.bulletDmg;
		double dmgNum = bulletDmg * bulletDmg;
		this.unit.GetHurt(dmgNum, Player.inst.unit, Vector2.zero, false, base.transform.position, true);
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x000334F4 File Offset: 0x000316F4
	private void OnTriggerEnter2D(Collider2D collision)
	{
		GameObject gameObject = collision.gameObject;
		string tag = gameObject.tag;
		if (!(tag == "Bullet"))
		{
			if (tag == "PlayerShield")
			{
				Vector2 hitbackDirection = base.transform.position - Player.inst.transform.position;
				this.Move_HitShield(hitbackDirection);
				this.Special_ThornBarrier();
				return;
			}
			if (tag == "SmallLaser")
			{
				SpecialBullet_SmallLaser componentInParent = gameObject.GetComponentInParent<SpecialBullet_SmallLaser>();
				this.unit.GetHurt(componentInParent.damage, Player.inst.unit, Vector2.zero, componentInParent.ifCrit, base.transform.position, true);
				return;
			}
			if (!(tag == "LightDrone"))
			{
				return;
			}
			LightDrone componentInParent2 = gameObject.GetComponentInParent<LightDrone>();
			this.unit.GetHurt(componentInParent2.damage, Player.inst.unit, Vector2.zero, componentInParent2.ifCrit, base.transform.position, true);
			return;
		}
		else
		{
			if (!gameObject.activeInHierarchy)
			{
				return;
			}
			Bullet componentInParent3 = gameObject.GetComponentInParent<Bullet>();
			if (componentInParent3 == null)
			{
				Debug.LogError("BulletNull!");
				return;
			}
			Vector2 vec2Normal = gameObject.transform.position - base.transform.position;
			if (componentInParent3.bulletType != EnumBulletType.MINE)
			{
				this.HitByBullet(collision.gameObject, vec2Normal);
			}
			componentInParent3.MyColliderEnter2D(base.gameObject);
			return;
		}
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00033670 File Offset: 0x00031870
	private void OnTriggerStay2D(Collider2D collision)
	{
		string tag = collision.gameObject.tag;
		if (tag == "PlayerShield")
		{
			Vector2 hitbackDirection = base.transform.position - Player.inst.transform.position;
			this.Move_HitShield(hitbackDirection);
		}
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x000336C2 File Offset: 0x000318C2
	private bool GetBool_IfBoss()
	{
		return this.model.rank == EnumRank.EPIC;
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x000336D2 File Offset: 0x000318D2
	private void Move_HitPlayer(Vector2 hitbackDirection, float multi)
	{
		if (TempData.GetBool_Stridebreaker())
		{
			multi *= 0.5f;
		}
		this.Move_HitSomething(hitbackDirection, multi);
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x000336EC File Offset: 0x000318EC
	private void Move_HitWall(Vector2 hitbackDirection, float multi)
	{
		this.Move_HitSomething(hitbackDirection, multi);
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x000336F6 File Offset: 0x000318F6
	private void Move_HitShield(Vector2 hitbackDirection)
	{
		if (this.GetBool_IfBoss() && TempData.GetBool_Stridebreaker())
		{
			return;
		}
		this.Move_HitSomething(hitbackDirection, 1f);
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x00033714 File Offset: 0x00031914
	public void BumperCar_HitEnemy(Enemy target)
	{
		if (this.GetBool_IfBoss())
		{
			return;
		}
		Vector2 hitBackDirection = base.transform.position - target.transform.position;
		if (hitBackDirection.magnitude == 0f)
		{
			hitBackDirection = UnityEngine.Random.insideUnitCircle;
		}
		this.Move_HitSomething(hitBackDirection, 1.8f);
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0003376C File Offset: 0x0003196C
	private void Move_HitSomething(Vector2 hitBackDirection, float multi)
	{
		switch (this.move_Type)
		{
		case EnumMoveType.BOUNCE:
		{
			float num = MyTool.Vec2toAngle180(hitBackDirection);
			float num2 = MyTool.Vec2toAngle180(this.Move_Direction);
			if (Mathf.Abs(num2 - num) > 90f)
			{
				float num3 = MyTool.BounceMirror(num2, num);
				num3 += (float)(UnityEngine.Random.Range(-1, 1) * 9);
				this.Move_Direction = MyTool.AngleToVec2(num3);
				hitBackDirection = this.Move_Direction;
			}
			else
			{
				this.Move_Direction = MyTool.AngleToVec2(num);
			}
			if (this.lifeTime >= this.move_Bounce_LifeTimeLimit)
			{
				this.TrigControl(this.move_Bounce_HitWallTrig_Open, this.move_Bounce_HitWallTrig_TrigType);
			}
			break;
		}
		case EnumMoveType.DASH:
			if (this.move_Dash_OnDashing)
			{
				this.move_Dash_OnDashing = false;
				this.TrigControl(this.move_Dash_HitWallTrig_Open, this.move_Dash_HitWallTrig_TrigType);
			}
			break;
		case EnumMoveType.JUMP:
			this.move_Jump_OnJumping = false;
			this.Move_TimeLeft = 0f;
			this.TrigControl(this.move_Jump_EndTrig_Open, this.move_Jump_EndTrig_TrigType);
			break;
		}
		Vector2 a = hitBackDirection.normalized * this.unit.rb.mass * GameParameters.Inst.RepulseForceByWall;
		this.unit.rb.MyAddForce(this.unit.lastSize, a * multi);
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x000338B4 File Offset: 0x00031AB4
	public void InitEnemy(EnemyModel enemyModel)
	{
		this.model = new EnemyModel(enemyModel);
		this.InitColor();
		float num = 0.95f + 0.05f * (float)this.fusionRank;
		float num2 = (float)this.fusionRank;
		float num3 = (float)(this.fusionRank - 1) * 0.6f + 1f;
		float num4 = 1f;
		if (this.model.rank == EnumRank.RARE && TempData.inst.diffiOptFlag[7])
		{
			num4 = 2f;
		}
		float num5 = 1f;
		if (this.model.rank == EnumRank.EPIC && TempData.inst.diffiOptFlag[14])
		{
			num5 = 3f;
		}
		this.unit.FactorBasic = new Factor(this.model.factorMultis, true);
		this.unit.FactorBasic.factor[0] = (double)this.model.factorMultis.mod_LifeMaxEnemy * GameParameters.Inst.DefaultFactorEnemy.lifeMaxEnemy * Battle.inst.factorBattleTotal.Enemy_ModLife;
		this.unit.FactorBasic *= Battle.inst.FacMulEnemy;
		ObscuredDouble[] factor = this.unit.FactorBasic.factor;
		int num6 = 8;
		factor[num6] *= (double)(num3 * num4);
		ObscuredDouble[] factor2 = this.unit.FactorBasic.factor;
		int num7 = 0;
		factor2[num7] *= (double)(num2 * num4 * num5);
		this.unit.life = this.unit.EnemyFactorTotal.lifeMaxEnemy;
		ObscuredDouble[] factor3 = this.unit.FactorBasic.factor;
		int num8 = 2;
		factor3[num8] *= (double)(Battle.inst.factorBattleTotal.Enemy_ModSpeed * num);
		ObscuredDouble[] factor4 = this.unit.FactorBasic.factor;
		int num9 = 3;
		factor4[num9] *= (double)(Battle.inst.FacMulEnemy.mod_MoveSpd * Battle.inst.factorBattleTotal.Enemy_ModSpeed * num * num4);
		GameParameters.EnemySetting enemySet = GameParameters.Inst.enemySet;
		float num10 = 1f;
		float num11 = this.model.factorMultis.mod_MoveSpd * Battle.inst.FacMulEnemy.mod_MoveSpd * Battle.inst.factorBattleTotal.Enemy_ModSpeed * num * num4;
		switch (this.move_Type)
		{
		case EnumMoveType.TRACKING:
			this.move_Tracking_AngelSpeed = num11 * enemySet.basicPara_Tracking_AngleSpeed;
			if (this.GetBool_IfSpecialist())
			{
				this.move_Tracking_AngelSpeed *= 3f;
			}
			num10 = enemySet.speedMod_Tracking;
			break;
		case EnumMoveType.BOUNCE:
			if (this.GetBool_IfSpecialist())
			{
				ObscuredDouble[] factor5 = this.unit.FactorBasic.factor;
				int num12 = 2;
				factor5[num12] *= 1.5;
			}
			num10 = enemySet.speedMod_Bounce;
			break;
		case EnumMoveType.DASH:
			this.move_Dash_IntervalTime = enemySet.basicPara_Dash_WaitTime / num11;
			if (this.GetBool_IfSpecialist())
			{
				this.move_Dash_IntervalTime /= 3f;
			}
			num10 = enemySet.speedMod_Dash;
			break;
		case EnumMoveType.JUMP:
			this.move_Jump_IntervalTime = enemySet.basicPara_Jump_IntervalTime / num11;
			this.move_Jump_DurationTime = enemySet.basicPara_Jump_DurationTime / num11;
			this.move_Jump_AngleDeltaRange = enemySet.jump_AngleDelta;
			if (this.GetBool_IfSpecialist())
			{
				this.move_Jump_IntervalTime /= 2f;
			}
			num10 = enemySet.speedMod_Jump;
			break;
		}
		ObscuredDouble[] factor6 = this.unit.FactorBasic.factor;
		int num13 = 2;
		factor6[num13] *= (double)num10;
		if (this.rotate_Open)
		{
			this.rotate_AngleSpeed = this.model.factorMultis.mod_MoveSpd * num10 * enemySet.basicPara_Rotate_AngleSpeed;
		}
		this.unit.SetDrags();
		this.bossStage = 0;
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x00033CCD File Offset: 0x00031ECD
	public void UpdateFusion(int newRank)
	{
		this.fusionRank = newRank;
		this.InitEnemy(DataBase.Inst.Data_EnemyModels[this.modelID]);
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x00033CF0 File Offset: 0x00031EF0
	private void InitColor()
	{
		Color color = Color.white;
		if (TempData.inst != null && TempData.inst.battle != null)
		{
			color = Battle.inst.levelColor;
		}
		else
		{
			Debug.LogWarning("Battle Null!");
		}
		ColorSet colorSet_Enemy = ResourceLibrary.Inst.colorSet_Enemy;
		this.unit.mainColor = color.ApplyColorSet(colorSet_Enemy);
		this.unit.DyeSprsWithColor(this.unit.mainColor);
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x00033D68 File Offset: 0x00031F68
	private void HitByBullet(GameObject objBullet, Vector2 vec2Normal)
	{
		if (this.GetBool_IfBoss() && TempData.GetBool_Stridebreaker())
		{
			return;
		}
		Bullet componentInParent = objBullet.GetComponentInParent<Bullet>();
		if (componentInParent == null)
		{
			Debug.LogError("BulletNull??: " + objBullet.name);
			return;
		}
		if (componentInParent.source == null)
		{
			Debug.LogError("BulletSourceNull!");
			return;
		}
		float float_HitBackForce = componentInParent.GetFloat_HitBackForce();
		if (float_HitBackForce == 0f)
		{
			return;
		}
		Vector2 a = componentInParent.GetVector2_HitBackDirection(base.transform);
		if (Battle.inst.specialEffect[16] >= 1)
		{
			a = -a;
		}
		this.unit.rb.MyAddForce(this.unit.lastSize, float_HitBackForce * a);
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x00033E1C File Offset: 0x0003201C
	private void MultiSpeed(float mul)
	{
		ObscuredDouble[] factor = this.unit.FactorBasic.factor;
		int num = 2;
		factor[num] *= (double)mul;
		ObscuredDouble[] factor2 = this.unit.FactorBasic.factor;
		int num2 = 3;
		factor2[num2] *= (double)mul;
		switch (this.move_Type)
		{
		case EnumMoveType.TRACKING:
			this.move_Tracking_AngelSpeed *= mul;
			return;
		case EnumMoveType.BOUNCE:
			break;
		case EnumMoveType.DASH:
			this.move_Dash_IntervalTime /= mul;
			return;
		case EnumMoveType.JUMP:
			this.move_Jump_IntervalTime /= mul;
			this.move_Jump_DurationTime /= mul;
			break;
		default:
			return;
		}
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x00033EDF File Offset: 0x000320DF
	private void MultiSize(float mul)
	{
		ObscuredDouble[] factor = this.unit.FactorBasic.factor;
		int num = 8;
		factor[num] *= (double)mul;
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x00033F10 File Offset: 0x00032110
	private void DetectBossStage()
	{
		if (this.model.rank != EnumRank.EPIC)
		{
			return;
		}
		double num = this.unit.life / this.unit.LifeMax;
		if (this.bossStage == 0)
		{
			this.bossStage = -1;
			this.MultiSpeed(0.72f);
			this.MultiSize(1f);
		}
		if (this.bossStage == -1 && num < 0.6000000238418579)
		{
			this.bossStage = 1;
			this.MultiSpeed(1.32f);
			this.MultiSize(1.44f);
		}
		if (this.bossStage == 1 && num < 0.30000001192092896)
		{
			this.bossStage = 2;
			this.MultiSpeed(1.32f);
			this.MultiSize(1.44f);
		}
		if (this.bossStage == 2 && num < 0.10000000149011612)
		{
			this.bossStage = 3;
			this.MultiSpeed(1.32f);
			this.MultiSize(1.44f);
		}
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00034000 File Offset: 0x00032200
	private void Boss_Swallow_InUpdate()
	{
		if (this.model.rank != EnumRank.EPIC)
		{
			return;
		}
		if (!TempData.inst.diffiOptFlag[26])
		{
			return;
		}
		this.swallowTime += (double)this.DeltaTimeForEnemy();
		double num = this.Boss_Swallow_GetSwallowTimeMax();
		if (this.swallowTime < num)
		{
			return;
		}
		List<Enemy> listEnemies = BattleManager.inst.listEnemies;
		List<Enemy> list = new List<Enemy>();
		for (int i = 0; i < listEnemies.Count; i++)
		{
			Enemy enemy = listEnemies[i];
			if (!(enemy == this))
			{
				list.Add(enemy);
			}
		}
		if (list.Count == 0)
		{
			Debug.LogWarning("没得吞");
			return;
		}
		this.swallowTime -= num;
		base.StartCoroutine(this.StartSwallow(list.GetRandom<Enemy>()));
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x000340C2 File Offset: 0x000322C2
	private double Boss_Swallow_GetSwallowTimeMax()
	{
		return math.max(this.unit.life / this.unit.LifeMax * 3.0, 0.3);
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x000340F3 File Offset: 0x000322F3
	private IEnumerator StartSwallow(Enemy target)
	{
		float spd = 18f * Battle.inst.factorBattleTotal.Enemy_ModSpeed;
		target.GetSwallowed(this);
		while (!(target == null))
		{
			GameObject gameObject = target.gameObject;
			if (gameObject == null)
			{
				yield break;
			}
			Vector2 vector = base.transform.position - gameObject.transform.position;
			float magnitude = vector.magnitude;
			if (magnitude < (base.transform.localScale.x + gameObject.transform.localScale.x) * 0.39f)
			{
				target.unit.Die(false);
				this.unit.life = math.min(this.unit.LifeMax, this.unit.life + target.unit.life);
				ObscuredDouble[] factor = this.unit.FactorBasic.factor;
				int num = 8;
				factor[num] += target.unit.FactorBasic.factor[8] * 0.6000000238418579;
				yield break;
			}
			Vector2 v = vector.normalized * spd * this.DeltaTimeForEnemy();
			if (v.magnitude > magnitude)
			{
				v = v.normalized * magnitude;
			}
			gameObject.transform.Translate(v, Space.World);
			yield return 0;
		}
		yield break;
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x00034109 File Offset: 0x00032309
	private void GetSwallowed(Enemy boss)
	{
		this.swallowed_Boss = boss;
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00034114 File Offset: 0x00032314
	protected void LookAtDirection(Vector2 direction)
	{
		if (this.rotate_Open)
		{
			return;
		}
		float z = MyTool.Vec2toAngle180(direction);
		base.transform.localRotation = Quaternion.Euler(0f, 0f, z);
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x0003414C File Offset: 0x0003234C
	public float PlayerToMeEuler()
	{
		Vector2 vector = Player.inst.gameObject.transform.position - base.gameObject.transform.position;
		return Mathf.Atan2(vector.y, vector.x) / 3.1415927f * 180f;
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x000341AA File Offset: 0x000323AA
	public float PlayerToMePI()
	{
		return this.PlayerToMeEuler() / 180f * 3.1415927f;
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x000341C0 File Offset: 0x000323C0
	public Vector2 PlayerToMeVec2()
	{
		if (Player.inst == null)
		{
			return Vector2.zero;
		}
		Vector2 a = Player.inst.gameObject.transform.position;
		if (TargetDrone.inst != null)
		{
			a = TargetDrone.inst.transform.position;
		}
		return a - base.gameObject.transform.position;
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x00034237 File Offset: 0x00032437
	private bool GetBool_IfSpecialist()
	{
		return TempData.inst.diffiOptFlag[27];
	}

	// Token: 0x04000725 RID: 1829
	public EnemyModel model;

	// Token: 0x04000726 RID: 1830
	public BasicUnit unit;

	// Token: 0x04000727 RID: 1831
	public bool debug_HasAmountAdd;

	// Token: 0x04000728 RID: 1832
	private EnemyInfoDisplayText infoDisplayText;

	// Token: 0x04000729 RID: 1833
	public bool summoned;

	// Token: 0x0400072A RID: 1834
	public bool splited;

	// Token: 0x0400072B RID: 1835
	public int modelID = -1;

	// Token: 0x0400072C RID: 1836
	private float lifeTime;

	// Token: 0x0400072D RID: 1837
	[SerializeField]
	private Vector2 move_CurrentDirection = Vector2.zero;

	// Token: 0x0400072E RID: 1838
	private float Move_TimeLeft;

	// Token: 0x0400072F RID: 1839
	[Header("MoveModule")]
	public EnumMoveType move_Type;

	// Token: 0x04000730 RID: 1840
	[SerializeField]
	[CustomLabel("转向角速度")]
	private float move_Tracking_AngelSpeed = 20f;

	// Token: 0x04000731 RID: 1841
	[SerializeField]
	[CustomLabel("是否开启距离触发")]
	private bool move_Tracking_DistTrig_Open;

	// Token: 0x04000732 RID: 1842
	[SerializeField]
	[CustomLabel("触发距离")]
	private float move_Tracking_DistTrig_Dist = 1f;

	// Token: 0x04000733 RID: 1843
	[SerializeField]
	[CustomLabel("触发类型")]
	private EnumTrigType move_Tracking_DistTrig_TrigType;

	// Token: 0x04000734 RID: 1844
	[SerializeField]
	[CustomLabel("是否开启撞墙触发")]
	private bool move_Bounce_HitWallTrig_Open;

	// Token: 0x04000735 RID: 1845
	[SerializeField]
	[CustomLabel("触发类型")]
	private EnumTrigType move_Bounce_HitWallTrig_TrigType;

	// Token: 0x04000736 RID: 1846
	private float move_Bounce_LifeTimeLimit = 0.1f;

	// Token: 0x04000737 RID: 1847
	[SerializeField]
	private bool move_Dash_OnDashing;

	// Token: 0x04000738 RID: 1848
	[SerializeField]
	private float move_Dash_IntervalTime = 3f;

	// Token: 0x04000739 RID: 1849
	[SerializeField]
	[CustomLabel("是否开启冲刺触发")]
	private bool move_Dash_StartTrig_Open;

	// Token: 0x0400073A RID: 1850
	[SerializeField]
	[CustomLabel("触发类型")]
	private EnumTrigType move_Dash_StartTrig_TrigType;

	// Token: 0x0400073B RID: 1851
	[SerializeField]
	[CustomLabel("是否开启撞墙触发")]
	private bool move_Dash_HitWallTrig_Open;

	// Token: 0x0400073C RID: 1852
	[SerializeField]
	[CustomLabel("触发类型")]
	private EnumTrigType move_Dash_HitWallTrig_TrigType;

	// Token: 0x0400073D RID: 1853
	[SerializeField]
	private bool move_Jump_OnJumping;

	// Token: 0x0400073E RID: 1854
	[SerializeField]
	private float move_Jump_IntervalTime = 1f;

	// Token: 0x0400073F RID: 1855
	[SerializeField]
	private float move_Jump_DurationTime = 1f;

	// Token: 0x04000740 RID: 1856
	[SerializeField]
	private float move_Jump_AngleDeltaRange = 30f;

	// Token: 0x04000741 RID: 1857
	[SerializeField]
	[CustomLabel("是否开启跳跃开始触发")]
	private bool move_Jump_StartTrig_Open;

	// Token: 0x04000742 RID: 1858
	[SerializeField]
	[CustomLabel("触发类型")]
	private EnumTrigType move_Jump_StartTrig_TrigType;

	// Token: 0x04000743 RID: 1859
	[SerializeField]
	[CustomLabel("是否开启跳跃结束触发")]
	private bool move_Jump_EndTrig_Open;

	// Token: 0x04000744 RID: 1860
	[SerializeField]
	[CustomLabel("触发类型")]
	private EnumTrigType move_Jump_EndTrig_TrigType;

	// Token: 0x04000745 RID: 1861
	[Header("ShootModule")]
	[SerializeField]
	public bool shoot_Open;

	// Token: 0x04000746 RID: 1862
	[SerializeField]
	public GameObject shoot_PrefabShootObject;

	// Token: 0x04000747 RID: 1863
	[SerializeField]
	private bool shoot_AutoShoot_Open;

	// Token: 0x04000748 RID: 1864
	public float shoot_TimeLeft = 9f;

	// Token: 0x04000749 RID: 1865
	[Header("RotateModule")]
	[SerializeField]
	private bool rotate_Open;

	// Token: 0x0400074A RID: 1866
	[SerializeField]
	private float rotate_AngleSpeed = 50f;

	// Token: 0x0400074B RID: 1867
	[SerializeField]
	private EnumRotateType rotate_Type;

	// Token: 0x0400074C RID: 1868
	private float rotate_CurrenAngleSpeed;

	// Token: 0x0400074D RID: 1869
	private float rotate_RefAngleSpeed;

	// Token: 0x0400074E RID: 1870
	[Header("SplitModule")]
	[SerializeField]
	private bool split_Open;

	// Token: 0x0400074F RID: 1871
	[SerializeField]
	private List<GameObject> split_ListChildObjects = new List<GameObject>();

	// Token: 0x04000750 RID: 1872
	[Header("Boss")]
	[SerializeField]
	private int bossStage;

	// Token: 0x04000751 RID: 1873
	[SerializeField]
	private double swallowTime;

	// Token: 0x04000752 RID: 1874
	[SerializeField]
	private Enemy swallowed_Boss;

	// Token: 0x04000753 RID: 1875
	[Header("Fusion")]
	[SerializeField]
	public int fusionRank = 1;

	// Token: 0x04000754 RID: 1876
	[SerializeField]
	public float existTime;

	// Token: 0x04000755 RID: 1877
	[SerializeField]
	public float fragMulti = 1f;

	// Token: 0x04000756 RID: 1878
	[SerializeField]
	public float starMulti = 1f;
}
