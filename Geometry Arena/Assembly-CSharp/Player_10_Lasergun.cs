using System;
using UnityEngine;

// Token: 0x02000056 RID: 86
public class Player_10_Lasergun : Player
{
	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06000354 RID: 852 RVA: 0x00014953 File Offset: 0x00012B53
	public static Skill SkillLaser
	{
		get
		{
			return DataBase.Inst.DataPlayerModels[10].skillLevels[0];
		}
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00014969 File Offset: 0x00012B69
	protected override void Awake()
	{
		base.Awake();
		Player_10_Lasergun.instLasergun = this;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x00014978 File Offset: 0x00012B78
	protected override void DetectMouse0_Shoot()
	{
		if (BulletsOptimization.ActualFireSpeed() <= 0f)
		{
			this.onShoot = false;
			return;
		}
		if (!Icon_AutoFire.inst.open && BattleMapCanvas.inst != null && BattleMapCanvas.inst.IfAnyWindowActive())
		{
			return;
		}
		if (MyInput.KeyShootHold() && this.skill_CanShoot)
		{
			this.Shoot_WantoShootOnce(MyTool.AngleToVec2(this.rotateAngle));
			this.onShoot = true;
			return;
		}
		this.onShoot = false;
	}

	// Token: 0x06000357 RID: 855 RVA: 0x000149F0 File Offset: 0x00012BF0
	public void Shoot_WantoShootOnce(Vector2 dir)
	{
		if (this.unit.fac_CurShootTime >= this.unit.Fac_MaxShootTime)
		{
			this.unit.fac_CurShootTime = this.unit.fac_CurShootTime % this.unit.Fac_MaxShootTime;
			this.ifLaserColli = true;
		}
		float num = this.unit.playerFactorTotal.bulletRng;
		if (this.ifOverload && TempData.inst.GetBool_SkillModuleOpenFlag(7))
		{
			SkillModule skillModule_CurrentJobWithEffectID = SkillModule.GetSkillModule_CurrentJobWithEffectID(7);
			num *= skillModule_CurrentJobWithEffectID.facs[2];
		}
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(8))
		{
			this.Shoot_WantoShootOnce_WithSplit(base.transform.position, dir, num, this.ifLaserColli);
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			this.Shoot_WantoShootOnce_WithSplit(base.transform.position, dir.Rotate((float)(i * 60)), num, this.ifLaserColli);
		}
	}

	// Token: 0x06000358 RID: 856 RVA: 0x00014AD8 File Offset: 0x00012CD8
	private void Shoot_WantoShootOnce_WithSplit(Vector2 posOrigin, Vector2 direction, float maxDist, bool ifColli)
	{
		int splitUpgradeCount = Battle.inst.GetSplitUpgradeCount();
		if (splitUpgradeCount == 0 || TempData.inst.GetBool_SkillModuleOpenFlag(5))
		{
			this.Laser_DetectAndGenerate(posOrigin, direction, maxDist, ifColli, 1f);
			return;
		}
		int num = Mathf.Pow(2f, (float)splitUpgradeCount).RoundToInt();
		float num2 = 30f;
		switch (splitUpgradeCount)
		{
		case 1:
			num2 = 10f;
			break;
		case 2:
			num2 = 20f;
			break;
		case 3:
			num2 = 30f;
			break;
		}
		for (int i = 0; i < num; i++)
		{
			float angelZ = -num2 + 2f * num2 * (float)i / (float)(num - 1);
			this.Laser_DetectAndGenerate(posOrigin, direction.Rotate(angelZ), maxDist, ifColli, 1f);
		}
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00014B8C File Offset: 0x00012D8C
	protected override void DetectInput_Skill()
	{
		this.ifOverload = MyInput.KeySkillHold();
		if (this.ifOverload && this.onShoot)
		{
			this.deltaTimeFromLastOverloadHurt += Time.deltaTime;
			float num = Player_10_Lasergun.SkillLaser.facs[7];
			int num2 = Player_10_Lasergun.SkillLaser.facs[8].RoundToInt();
			if (TempData.inst.GetBool_SkillModuleOpenFlag(7))
			{
				SkillModule skillModule_CurrentJobWithEffectID = SkillModule.GetSkillModule_CurrentJobWithEffectID(7);
				num = skillModule_CurrentJobWithEffectID.facs[3];
				num2 = skillModule_CurrentJobWithEffectID.facs[4].RoundToInt();
			}
			if (this.deltaTimeFromLastOverloadHurt >= num)
			{
				this.deltaTimeFromLastOverloadHurt = 0f;
				this.unit.GetHurt((double)num2, this.unit, Vector2.zero, false, base.transform.position, true);
			}
		}
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00014C54 File Offset: 0x00012E54
	private void Laser_DetectAndGenerate(Vector2 posOrigin, Vector2 direction, float maxDist, bool ifColli, float loopTime = 1f)
	{
		if (loopTime < 0f)
		{
			return;
		}
		if (maxDist <= 0f)
		{
			return;
		}
		Vector2 zero = Vector2.zero;
		Vector2 vector = this.MyRayCast(posOrigin, direction, ref zero);
		float magnitude = (vector - posOrigin).magnitude;
		if (magnitude == 0f)
		{
			Debug.LogError("Error_RayDist==0!");
			return;
		}
		if (magnitude >= maxDist)
		{
			this.Laser_InstantiateLaser(posOrigin, direction, maxDist, ifColli);
			return;
		}
		this.Laser_InstantiateLaser(posOrigin, direction, magnitude + 3f, ifColli);
		if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			Vector2 vector2 = MyTool.BounceMirror(direction, zero);
			Vector2 posOrigin2 = vector;
			Vector2 direction2 = vector2;
			float maxDist2 = maxDist - magnitude;
			float num = loopTime;
			loopTime = num - 1f;
			this.Laser_DetectAndGenerate(posOrigin2, direction2, maxDist2, ifColli, num);
		}
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00014CF8 File Offset: 0x00012EF8
	private Vector2 MyRayCast(Vector2 originPos, Vector2 inDirection, ref Vector2 mirrorNormal)
	{
		float num = SceneObj.inst.SceneSize * 22f;
		inDirection = inDirection.normalized;
		if (Mathf.Abs(inDirection.y) == 1f)
		{
			mirrorNormal = new Vector2(0f, 1f);
			return new Vector2(originPos.x, num * inDirection.y);
		}
		if (Mathf.Abs(inDirection.x) == 1f)
		{
			mirrorNormal = new Vector2(1f, 0f);
			return new Vector2(num * inDirection.x, originPos.y);
		}
		Vector2 vector = MyTool.Cross(originPos, inDirection, num, false);
		if (Mathf.Abs(vector.y) < num)
		{
			mirrorNormal = new Vector2(1f, 0f);
			return vector;
		}
		if (Mathf.Abs(vector.y) == num)
		{
			mirrorNormal = new Vector2(1f, 1f);
			return vector;
		}
		vector = MyTool.Cross(originPos, inDirection, num, true);
		mirrorNormal = new Vector2(0f, 1f);
		return vector;
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00014E0C File Offset: 0x0001300C
	private void Laser_InstantiateLaser(Vector2 posOrigin, Vector2 direction, float dist, bool ifColli)
	{
		GameObject prefab_Bullet = this.unit.prefab_Bullet;
		Bullet pool_Bullet = ObjectPool.inst.GetPool_Bullet(prefab_Bullet.name);
		SpecialBullet_Laser component;
		if (pool_Bullet == null)
		{
			component = Object.Instantiate<GameObject>(prefab_Bullet).GetComponent<SpecialBullet_Laser>();
		}
		else
		{
			component = pool_Bullet.GetComponent<SpecialBullet_Laser>();
		}
		GameObject gameObject = component.gameObject;
		gameObject.name = prefab_Bullet.name;
		gameObject.transform.position = posOrigin + direction.normalized * dist / 2f;
		component.LaserInit(this.unit.mainColor, dist, direction, this.unit, ifColli);
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00014EB0 File Offset: 0x000130B0
	protected override void SkillInFixedUpdate()
	{
		this.ifLaserColli = false;
		if (TempData.inst.GetBool_SkillModuleOpenFlag(8) && this.onShoot)
		{
			this.canAimAtMouse = false;
			float num = SkillModule.GetSkillModule_CurrentJobWithEffectID(8).facs[0];
			this.rotateAngle += num * Time.fixedDeltaTime;
			base.transform.localRotation = Quaternion.Euler(0f, 0f, this.rotateAngle);
			return;
		}
		this.canAimAtMouse = true;
		this.rotateAngle = MyTool.MouseToPlayerAngle();
	}

	// Token: 0x0600035E RID: 862 RVA: 0x00014F38 File Offset: 0x00013138
	public override void UpdateFactorTotal(bool ifTrue = true)
	{
		this.unit.playerFactorTotal = this.unit.FactorBasic * Battle.inst.GetFactorMultis_Upgrates_CurBattle();
		BattleManager.inst.FactorMultiBuffs(ref this.unit.playerFactorTotal);
		float bulletRng = this.unit.playerFactorTotal.bulletRng;
		int num = 1000;
		if (TempData.inst.GetBool_SkillModuleOpenFlag(8))
		{
			num = 150;
		}
		if (bulletRng > (float)num)
		{
			this.factorMultis_Skill.factorMultis[7] = (float)num / bulletRng;
			this.factorMultis_Skill.factorMultis[4] = bulletRng / (float)num;
		}
		else
		{
			this.factorMultis_Skill.factorMultis[7] = 1f;
			this.factorMultis_Skill.factorMultis[4] = 1f;
		}
		this.unit.playerFactorTotal *= this.factorMultis_Skill;
	}

	// Token: 0x040002F1 RID: 753
	public static Player_10_Lasergun instLasergun;

	// Token: 0x040002F2 RID: 754
	public bool ifOverload;

	// Token: 0x040002F3 RID: 755
	[SerializeField]
	private float deltaTimeFromLastOverloadHurt;

	// Token: 0x040002F4 RID: 756
	[SerializeField]
	private float rotateAngle;

	// Token: 0x040002F5 RID: 757
	[SerializeField]
	private bool ifLaserColli;
}
