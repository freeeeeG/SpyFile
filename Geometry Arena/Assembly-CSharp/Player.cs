using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public abstract class Player : MonoBehaviour
{
	// Token: 0x170000FD RID: 253
	// (get) Token: 0x060008EC RID: 2284 RVA: 0x00034467 File Offset: 0x00032667
	public int LifeMax
	{
		get
		{
			return this.unit.playerFactorTotal.lifeMaxPlayer;
		}
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x00034479 File Offset: 0x00032679
	protected virtual void Awake()
	{
		Player.inst = this;
		this.unit = base.gameObject.GetComponentInChildren<BasicUnit>();
		if (this.unit != null)
		{
			this.unit.objType = EnumObjType.PLAYER;
		}
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x000344AC File Offset: 0x000326AC
	protected void Start()
	{
		if (TempData.inst.currentSceneType != EnumSceneType.BATTLE)
		{
			if (TempData.inst.jobId == 9)
			{
				this.Init_SpecialSkillInit();
			}
			return;
		}
		this.SpriteTop();
		this.Energy_Init();
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x000051D0 File Offset: 0x000033D0
	protected virtual void Energy_Init()
	{
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x000051D0 File Offset: 0x000033D0
	protected virtual void Energy_FixedUpdate()
	{
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x000344DC File Offset: 0x000326DC
	public virtual void UpdateFactorTotal(bool ifTrue = false)
	{
		if (!ifTrue)
		{
			return;
		}
		this.unit.playerFactorTotal = this.unit.FactorBasic * Battle.inst.GetFactorMultis_Upgrates_CurBattle() * Player.inst.factorMultis_Skill;
		BattleManager.inst.FactorMultiBuffs(ref this.unit.playerFactorTotal);
		BuffManager.RefreshStateBuff_AboutStats();
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x0003453C File Offset: 0x0003273C
	public void InitPlayerFactor()
	{
		int jobId = TempData.inst.jobId;
		this.unit.FactorBasic = TempData.inst.playerPreview.TotalFactor;
		Color colorUnit = DataBase.Inst.Data_VarColors[TempData.inst.varColorId].ColorUnit;
		this.unit.mainColor = colorUnit;
		this.unit.DyeSprsWithColor(colorUnit);
		this.unit.inited = true;
		this.UpdateFactorTotal(true);
		this.unit.TransScale = Mathf.Sqrt(this.unit.playerFactorTotal.bodySize);
		this.unit.rb.useAutoMass = false;
		this.unit.rb.mass = this.unit.playerFactorTotal.bodySize;
		this.unit.life = (double)this.unit.playerFactorTotal.lifeMaxPlayer;
		this.shield = this.Get_MaxShields();
		this.Init_SpecialSkillInit();
		BuffManager.RefreshAllStateBuff();
		HealthPointControl.inst.UpdateHpUnits();
		Debug.Log("PlayerFactorInited");
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x000051D0 File Offset: 0x000033D0
	protected virtual void Init_SpecialSkillInit()
	{
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0003464C File Offset: 0x0003284C
	private void LookAtMouse_InUpdate()
	{
		if (TempData.inst.jobId == 2)
		{
			return;
		}
		if (!this.canAimAtMouse)
		{
			return;
		}
		float rotation = MyTool.MouseToPlayerAngle();
		this.unit.rb.SetRotation(rotation);
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x00034688 File Offset: 0x00032888
	protected void Update()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.LookAtMouse_InUpdate();
		if (TempData.inst.currentSceneType != EnumSceneType.BATTLE)
		{
			return;
		}
		if (!this.unit.inited)
		{
			return;
		}
		if (TimeManager.inst && TimeManager.inst.ifPause)
		{
			return;
		}
		this.DetectInput_Skill();
		if (TempData.inst.jobId == 10)
		{
			this.DetectMouse0_Shoot();
		}
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x000346F8 File Offset: 0x000328F8
	private void FixedUpdate()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		if (TempData.inst.currentSceneType != EnumSceneType.BATTLE)
		{
			if (TempData.inst.jobId != 2)
			{
				return;
			}
			this.SkillInFixedUpdate();
		}
		if (!this.unit.inited)
		{
			return;
		}
		if (TimeManager.inst && TimeManager.inst.ifPause)
		{
			return;
		}
		BulletsOptimization.Update_ActualFireSpeed();
		BulletsOptimization.Update_ActualBulletDamage();
		this.DetectKeyboard_Move();
		if ((this.IfMouseNotOnButton() || (TempData.inst.jobId == 4 && Icon_AutoFire.inst.open)) && TempData.inst.jobId != 10)
		{
			this.DetectMouse0_Shoot();
		}
		this.SkillInFixedUpdate();
		this.Energy_FixedUpdate();
		this.UpdateFactorTotal(true);
		this.unit.Antibug_MoveRestriction();
		this.FixedUpdate_SpecialUpgrade();
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x000347C5 File Offset: 0x000329C5
	private void FixedUpdate_SpecialUpgrade()
	{
		this.Immolation();
		this.Titan();
		this.Delivery();
		this.StayHere();
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x000347E0 File Offset: 0x000329E0
	private void Immolation()
	{
		if (BattleManager.inst.timeStage == EnumTimeStage.REST)
		{
			return;
		}
		int num = Battle.inst.specialEffect[76];
		if (num <= 0)
		{
			return;
		}
		float num2 = 3f / (float)num;
		this.special_ImmolationTime += Time.fixedDeltaTime;
		if (this.special_ImmolationTime >= num2)
		{
			int num3 = Mathf.FloorToInt(this.special_ImmolationTime / num2);
			for (int i = 0; i < num3; i++)
			{
				this.special_ImmolationTime -= num2;
				this.unit.HurtSelf(1, true);
			}
		}
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x00034868 File Offset: 0x00032A68
	private void Titan()
	{
		if (BattleManager.inst.timeStage == EnumTimeStage.REST)
		{
			return;
		}
		if (Battle.inst.specialEffect[86] <= 0)
		{
			return;
		}
		this.special_Titan += Time.fixedDeltaTime;
		if (this.special_Titan >= 1f)
		{
			int damage = Mathf.Max(1, Mathf.FloorToInt((float)this.LifeMax * 0.1f));
			this.unit.HurtSelf(damage, true);
			this.special_Titan -= 1f;
		}
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x000348EA File Offset: 0x00032AEA
	private void Delivery()
	{
		if (Battle.inst.specialEffect[85] <= 0)
		{
			return;
		}
		if (this.special_Delivery < 3f)
		{
			this.special_Delivery += Time.fixedDeltaTime;
		}
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0003491C File Offset: 0x00032B1C
	private void StayHere()
	{
		if (Battle.inst.specialEffect[88] <= 0)
		{
			return;
		}
		if (BattleManager.inst.timeStage == EnumTimeStage.REST)
		{
			return;
		}
		this.special_StayHere_Time += Time.deltaTime;
		if (this.special_StayHere_Time < 0.5f)
		{
			return;
		}
		if (this.special_StayHere_Vector2.magnitude == 0f)
		{
			this.special_StayHere_Vector2 = base.transform.position;
		}
		Vector2 a = base.transform.position;
		double magnitude = (double)(a - this.special_StayHere_Vector2).magnitude;
		this.special_StayHere_Vector2 = a;
		this.special_StayHere_Time -= 0.5f;
		if (magnitude >= 0.09)
		{
			int damage = Mathf.CeilToInt((float)this.unit.life * 0.1f);
			this.unit.HurtSelf(damage, true);
			return;
		}
		float num = (float)(this.unit.LifeMax - this.unit.life);
		if (num == 0f)
		{
			return;
		}
		int num2 = Mathf.CeilToInt(num * 0.1f);
		this.unit.HealHP((float)num2);
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x00034A40 File Offset: 0x00032C40
	protected bool IfMouseNotOnButton()
	{
		bool flag = BattleManager.inst.timeStage == EnumTimeStage.REST;
		return !BattleMapCanvas.inst.IfAnyWindowActive() && (!flag || !this.mouseOnButton);
	}

	// Token: 0x060008FD RID: 2301
	protected abstract void SkillInFixedUpdate();

	// Token: 0x060008FE RID: 2302 RVA: 0x00034A7C File Offset: 0x00032C7C
	private void DetectKeyboard_Move()
	{
		if (!this.canMove)
		{
			return;
		}
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		float axisRaw = Input.GetAxisRaw("Horizontal");
		float axisRaw2 = Input.GetAxisRaw("Vertical");
		Vector2 dir = new Vector2(axis, axis2);
		float num = 0.9f;
		if (Mathf.Abs(axisRaw) <= num && Mathf.Abs(axisRaw2) <= num)
		{
			if (this.ifMoving)
			{
				this.ifMoving = false;
				BuffManager.RefreshStateBuff_AboutMove(false);
			}
		}
		else if (!this.ifMoving)
		{
			this.ifMoving = true;
			BuffManager.RefreshStateBuff_AboutMove(true);
		}
		if (axis != 0f || axis2 != 0f)
		{
			this.unit.Move_WantoMoveOnce_InFixedUpdate(dir, this.unit.playerFactorTotal.moveSpd);
		}
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00034B38 File Offset: 0x00032D38
	protected virtual void DetectMouse0_Shoot()
	{
		if (TempData.inst.jobId == 11)
		{
			return;
		}
		if (!Icon_AutoFire.inst.open && BattleMapCanvas.inst != null && BattleMapCanvas.inst.IfAnyWindowActive())
		{
			return;
		}
		if (MyInput.KeyShootHold() && this.skill_CanShoot)
		{
			if (this.unit.Shoot_WantoShootOnce(MyTool.MouseToPlayerVec2()))
			{
				this.OnFire();
			}
			this.onShoot = true;
			return;
		}
		this.onShoot = false;
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x000051D0 File Offset: 0x000033D0
	protected virtual void OnFire()
	{
	}

	// Token: 0x06000901 RID: 2305
	protected abstract void DetectInput_Skill();

	// Token: 0x06000902 RID: 2306 RVA: 0x00034BB0 File Offset: 0x00032DB0
	private void SpriteTop()
	{
		foreach (SpriteRenderer spriteRenderer in this.unit.list_SprsToDye)
		{
			spriteRenderer.sortingLayerName = "Cover";
		}
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x00034C0C File Offset: 0x00032E0C
	public int Get_MaxShields()
	{
		int num = 0;
		for (int i = 57; i <= 60; i++)
		{
			if (Battle.inst.specialEffect[i] >= 1)
			{
				if (i == 57)
				{
					num += 2;
				}
				else
				{
					num++;
				}
			}
		}
		if (Battle.inst.specialEffect[68] >= 1)
		{
			num++;
		}
		return num;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x00034C5C File Offset: 0x00032E5C
	public void FixShield()
	{
		this.shield = Mathf.Clamp(this.shield, 0, this.Get_MaxShields());
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x00034C78 File Offset: 0x00032E78
	public void RestoreShield(int value)
	{
		if (value <= 0)
		{
			return;
		}
		if (Battle.inst.specialEffect[45] >= 1 && this.unit.life / (double)this.LifeMax <= 0.20000000298023224)
		{
			value *= 2;
		}
		this.shield += value;
		this.FixShield();
		HealthPointControl.inst.UpdateHpUnits();
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x00034CDB File Offset: 0x00032EDB
	public void RestoreShieldAll()
	{
		this.RestoreShield(this.Get_MaxShields());
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00034CE9 File Offset: 0x00032EE9
	public void RemoveShield(int value)
	{
		if (value <= 0)
		{
			Debug.LogError("Error_Remove" + value);
			return;
		}
		this.shield -= value;
		this.FixShield();
		HealthPointControl.inst.UpdateHpUnits();
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x00034D23 File Offset: 0x00032F23
	public void RemoveShieldAll()
	{
		this.RemoveShield(this.Get_MaxShields());
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x00034D34 File Offset: 0x00032F34
	private void OnCollisionEnter2D(Collision2D collision)
	{
		string tag = collision.gameObject.tag;
		if (tag == "Obstacle")
		{
			this.Special_OnHitWall();
			Vector2 vector = collision.contacts[0].normal.normalized * this.unit.rb.mass * GameParameters.Inst.RepulseForceByWall;
			if (!TempData.inst.diffiOptFlag[22])
			{
				vector = vector.normalized * (vector.magnitude + this.unit.playerFactorTotal.moveSpd * this.unit.rb.mass * 50f);
			}
			if (TempData.inst.diffiOptFlag[13])
			{
				this.unit.GetHurt(1.0, null, Vector2.zero, false, base.transform.position, true);
				vector *= 5f;
			}
			if (TempData.inst.diffiOptFlag[22])
			{
				vector *= 0.6f;
			}
			if (Battle.inst.specialEffect[72] >= 1)
			{
				vector *= 0.5f;
			}
			this.unit.rb.MyAddForce(this.unit.playerFactorTotal.bodySize, vector);
		}
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x00034E8C File Offset: 0x0003308C
	private void OnCollisionStay2D(Collision2D collision)
	{
		string tag = collision.gameObject.tag;
		if (tag == "Obstacle")
		{
			this.Special_OnHitWall();
			Vector2 vector = collision.contacts[0].normal.normalized * this.unit.rb.mass * GameParameters.Inst.RepulseForceByWall;
			if (Battle.inst.specialEffect[72] >= 1)
			{
				vector *= 0.5f;
			}
			this.unit.rb.MyAddForce(this.unit.playerFactorTotal.bodySize, vector);
		}
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x000051D0 File Offset: 0x000033D0
	protected virtual void Special_OnHitWall()
	{
	}

	// Token: 0x0400076B RID: 1899
	public static Player inst;

	// Token: 0x0400076C RID: 1900
	[HideInInspector]
	public BasicUnit unit;

	// Token: 0x0400076D RID: 1901
	public FactorMultis factorMultis_Skill = new FactorMultis();

	// Token: 0x0400076E RID: 1902
	[Header("Energy")]
	public float energy = 6f;

	// Token: 0x0400076F RID: 1903
	public float energyMax = 6f;

	// Token: 0x04000770 RID: 1904
	public float energyRecoverCD;

	// Token: 0x04000771 RID: 1905
	[Header("Shiled")]
	public int shield;

	// Token: 0x04000772 RID: 1906
	[Header("SpecialLogic")]
	public bool skill_CanShoot = true;

	// Token: 0x04000773 RID: 1907
	[SerializeField]
	protected bool onShoot;

	// Token: 0x04000774 RID: 1908
	[SerializeField]
	protected bool canAimAtMouse = true;

	// Token: 0x04000775 RID: 1909
	[SerializeField]
	protected bool canMove = true;

	// Token: 0x04000776 RID: 1910
	[SerializeField]
	public bool mouseOnButton;

	// Token: 0x04000777 RID: 1911
	[SerializeField]
	public bool ifMoving;

	// Token: 0x04000778 RID: 1912
	protected Skill theSkill;

	// Token: 0x04000779 RID: 1913
	protected int skillLevel;

	// Token: 0x0400077A RID: 1914
	[Header("SpecialUpgrade")]
	private float special_ImmolationTime;

	// Token: 0x0400077B RID: 1915
	private float special_Titan;

	// Token: 0x0400077C RID: 1916
	public float special_Delivery;

	// Token: 0x0400077D RID: 1917
	private Vector2 special_StayHere_Vector2 = Vector2.zero;

	// Token: 0x0400077E RID: 1918
	private float special_StayHere_Time;
}
