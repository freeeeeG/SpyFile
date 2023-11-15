using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class LightDrone : Drone
{
	// Token: 0x06000307 RID: 775 RVA: 0x00012C54 File Offset: 0x00010E54
	protected override void FixedUpdate()
	{
		if (this.cpu == null && !BattleManager.inst.GameOn)
		{
			base.DestoryThisDrone();
			return;
		}
		this.FixedUpdate_EnemyAndAim();
		this.FixedUpdate_Move();
		this.FixedUpdate_Collider();
	}

	// Token: 0x06000308 RID: 776 RVA: 0x00012C8C File Offset: 0x00010E8C
	public override void FixedUpdate_EnemyAndAim()
	{
		List<Enemy> listEnemies = BattleManager.inst.listEnemies;
		if (listEnemies.Count <= 0)
		{
			this.ifTracing = false;
			base.UpdateAngleWhenNoTarget();
			return;
		}
		this.ifTracing = true;
		float num = -1f;
		if (this.targetEnemy != null)
		{
			num = this.targetEnemy.transform.localScale.x;
		}
		foreach (Enemy enemy in listEnemies)
		{
			float x = enemy.transform.localScale.x;
			if (x >= num + 0.1f)
			{
				num = x;
				this.targetEnemy = enemy;
			}
		}
		this.aimDirection = this.targetEnemy.transform.position - base.transform.position;
		float z = MyTool.Vec2toAngle180(this.aimDirection);
		base.transform.eulerAngles = new Vector3(0f, 0f, z);
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00012DA4 File Offset: 0x00010FA4
	private void FixedUpdate_Collider()
	{
		this.colli_Cur += Time.fixedDeltaTime;
		if (this.colli_Cur >= this.colli_Max)
		{
			this.colli_Cur -= this.colli_Max;
			this.UpdateDamage();
			this.lightCollider.enabled = true;
			return;
		}
		this.lightCollider.enabled = false;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x00012E04 File Offset: 0x00011004
	private void UpdateDamage()
	{
		Factor playerFactorTotal = Player.inst.unit.playerFactorTotal;
		float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(6).facs;
		this.damage = math.pow(Player.inst.unit.playerFactorTotal.bulletDmg, 2.0) * (double)facs[7];
		this.ifCrit = MyTool.DecimalToBool(playerFactorTotal.critChc);
		if (this.ifCrit)
		{
			this.damage *= (double)playerFactorTotal.critDmg;
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(7))
		{
			float[] facs2 = SkillModule.GetSkillModule_CurrentJobWithEffectID(7).facs;
			this.damage *= (double)facs2[3];
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00012EB1 File Offset: 0x000110B1
	protected override void FixedUpdate_Move()
	{
		if (this.ifTracing)
		{
			this.targetPos = this.targetEnemy.transform.position;
		}
		base.FixedUpdate_Move();
	}

	// Token: 0x040002C1 RID: 705
	public bool ifTracing;

	// Token: 0x040002C2 RID: 706
	[SerializeField]
	private float colli_Max = 0.1f;

	// Token: 0x040002C3 RID: 707
	[SerializeField]
	private float colli_Cur;

	// Token: 0x040002C4 RID: 708
	[SerializeField]
	private Collider2D lightCollider;

	// Token: 0x040002C5 RID: 709
	[SerializeField]
	public double damage;

	// Token: 0x040002C6 RID: 710
	[SerializeField]
	public bool ifCrit;
}
