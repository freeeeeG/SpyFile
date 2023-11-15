using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class SpecialBullet_Summon : Bullet
{
	// Token: 0x0600089E RID: 2206 RVA: 0x00031E5C File Offset: 0x0003005C
	protected override void SpecialInFixedUpdate()
	{
		if (!this.inited)
		{
			return;
		}
		if (this.target == null)
		{
			this.FindNewEnemyTarget();
		}
		if (this.target == null)
		{
			return;
		}
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(2))
		{
			this.RotateToTarget();
		}
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x00031EA8 File Offset: 0x000300A8
	private void RotateToTarget()
	{
		Vector2 dirTarget = this.target.transform.position - base.transform.position;
		float maxAngle = this.rotateAngleSpeedFac * this.TotalSpeed * Time.fixedDeltaTime;
		base.UpdateDirection(this.direction.DirectionApproach(dirTarget, maxAngle));
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00031F04 File Offset: 0x00030104
	private void FindNewEnemyTarget()
	{
		if (TempData.inst.GetBool_SkillModuleOpenFlag(3))
		{
			this.FindNearestEnemyTarget();
		}
		else
		{
			this.FindRandomEnemyTarget();
		}
		if (!this.hasInitDirection && this.target != null)
		{
			base.UpdateDirection((this.target.transform.position - base.transform.position).normalized);
			this.hasInitDirection = true;
		}
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x00031F7C File Offset: 0x0003017C
	private void FindRandomEnemyTarget()
	{
		if (BattleManager.inst.listEnemies.Count > 0)
		{
			Enemy random = BattleManager.inst.listEnemies.GetRandom<Enemy>();
			this.target = random.gameObject;
			return;
		}
		this.target = null;
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x00031FC0 File Offset: 0x000301C0
	private void FindNearestEnemyTarget()
	{
		float num = 1000f;
		if (BattleManager.inst.listEnemies.Count > 0)
		{
			using (List<Enemy>.Enumerator enumerator = BattleManager.inst.listEnemies.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Enemy enemy = enumerator.Current;
					float magnitude = (enemy.transform.position - base.transform.position).magnitude;
					if (magnitude < num)
					{
						num = magnitude;
						this.target = enemy.gameObject;
					}
				}
				return;
			}
		}
		this.target = null;
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x00032068 File Offset: 0x00030268
	public override void EndLife(bool ifNormal)
	{
		if (!this.hasDie)
		{
			base.EndLife(ifNormal);
		}
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0003207C File Offset: 0x0003027C
	public override void Awake()
	{
		this.hasDie = false;
		this.hasInitDirection = false;
		this.target = null;
		int num = 0;
		Skill skill = Skill.SkillCurrent(ref num);
		if (num <= 0)
		{
			Debug.LogError("skillLevel<=0!");
		}
		float[] facs = skill.facs;
		this.rotateAngleSpeedFac = facs[1] * Player.inst.unit.playerFactorTotal.accuracy;
		base.Awake();
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x000320DF File Offset: 0x000302DF
	protected override void TargetTheEnemy(Enemy enemy)
	{
		base.TargetTheEnemy(enemy);
		this.target = enemy.gameObject;
	}

	// Token: 0x04000719 RID: 1817
	[SerializeField]
	public GameObject target;

	// Token: 0x0400071A RID: 1818
	[SerializeField]
	private float rotateAngleSpeedFac = 100f;

	// Token: 0x0400071B RID: 1819
	[SerializeField]
	public bool hasInitDirection;
}
