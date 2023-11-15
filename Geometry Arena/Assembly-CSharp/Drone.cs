using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class Drone : MonoBehaviour
{
	// Token: 0x060002F2 RID: 754 RVA: 0x00012428 File Offset: 0x00010628
	public virtual void FixedUpdate_EnemyAndAim()
	{
		if (this.type == Drone.EnumDroneType.GRENADE || this.type == Drone.EnumDroneType.ITEM)
		{
			this.UpdateAngleWhenNoTarget();
			return;
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(1) && this.type == Drone.EnumDroneType.MAIN)
		{
			this.targetEnemy = null;
			this.aimDirection = MyTool.MouseToPlayerVec2();
		}
		else
		{
			List<Enemy> listEnemies = BattleManager.inst.listEnemies;
			if (listEnemies.Count <= 0)
			{
				this.UpdateAngleWhenNoTarget();
				return;
			}
			if (this.targetEnemy != null && (this.targetEnemy.transform.position - base.transform.position).magnitude >= 0.75f * Player.inst.unit.playerFactorTotal.bulletRng)
			{
				this.targetEnemy = null;
			}
			if (this.targetEnemy == null)
			{
				this.targetEnemy = listEnemies.GetRandom<Enemy>();
			}
			this.aimDirection = this.targetEnemy.transform.position - base.transform.position;
		}
		float z = MyTool.Vec2toAngle180(this.aimDirection);
		base.transform.eulerAngles = new Vector3(0f, 0f, z);
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0001255C File Offset: 0x0001075C
	protected void UpdateAngleWhenNoTarget()
	{
		this.aimDirection = this.targetPos - base.transform.position;
		float z = MyTool.Vec2toAngle180(this.aimDirection);
		base.transform.eulerAngles = new Vector3(0f, 0f, z);
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x000125B1 File Offset: 0x000107B1
	private void Start()
	{
		this.bloom.InitMat(this.sprBody, ResourceLibrary.Inst.GlowIntensity_Unit, false);
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x000125CF File Offset: 0x000107CF
	protected virtual void FixedUpdate()
	{
		if (this.cpu == null && !BattleManager.inst.GameOn)
		{
			this.DestoryThisDrone();
			return;
		}
		this.FixedUpdate_EnemyAndAim();
		this.FixedUpdate_Move();
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x00012600 File Offset: 0x00010800
	public void UpdateTargetPos(Player_11_CPU cpu, int index, int totalNum, Vector2 centerPos, float angleDelta, float radius)
	{
		this.radius = radius;
		this.cpu = cpu;
		Vector2 a = MyTool.AngleToVec2(360f / (float)totalNum * (float)index + angleDelta) * radius;
		this.targetPos = a + centerPos;
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00012645 File Offset: 0x00010845
	public void SetMoveSpeed(float moveSpeed)
	{
		this.moveSpeed = moveSpeed + this.basicMoveSpeed;
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00012658 File Offset: 0x00010858
	public void SetBodySize(float size)
	{
		float num = Mathf.Sqrt(size);
		base.transform.localScale = new Vector3(num, num, 1f);
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00012684 File Offset: 0x00010884
	protected virtual void FixedUpdate_Move()
	{
		Vector2 vector = this.targetPos - base.transform.position;
		float magnitude = vector.magnitude;
		float num = Time.fixedDeltaTime * this.moveSpeed;
		num = Mathf.Min(num, magnitude);
		Vector2 v = vector.normalized * num;
		base.transform.position += v;
	}

	// Token: 0x060002FA RID: 762 RVA: 0x000126F4 File Offset: 0x000108F4
	public void DyeWithColor(Color mainColor)
	{
		this.sprBody.color = mainColor;
	}

	// Token: 0x060002FB RID: 763 RVA: 0x00012704 File Offset: 0x00010904
	public void DestoryThisDrone()
	{
		if (Setting.Inst.Option_BulletParticle)
		{
			Particle pool_Particle_GetOrNew = ObjectPool.inst.GetPool_Particle_GetOrNew(ResourceLibrary.Inst.Prefab_Particle_BulletBlast);
			pool_Particle_GetOrNew.transform.position = base.transform.position;
			pool_Particle_GetOrNew.Blast_Init(EnumShapeType.SQUARE, this.sprBody.color, base.transform.localScale.x, false);
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040002B1 RID: 689
	[SerializeField]
	protected Drone.EnumDroneType type;

	// Token: 0x040002B2 RID: 690
	[SerializeField]
	protected Player_11_CPU cpu;

	// Token: 0x040002B3 RID: 691
	[SerializeField]
	protected SpriteRenderer sprBody;

	// Token: 0x040002B4 RID: 692
	[SerializeField]
	public GunObj gun;

	// Token: 0x040002B5 RID: 693
	[SerializeField]
	protected Enemy targetEnemy;

	// Token: 0x040002B6 RID: 694
	[SerializeField]
	private float radius = 3f;

	// Token: 0x040002B7 RID: 695
	[SerializeField]
	protected Vector2 targetPos = Vector2.zero;

	// Token: 0x040002B8 RID: 696
	[SerializeField]
	private float basicMoveSpeed = 6f;

	// Token: 0x040002B9 RID: 697
	[SerializeField]
	protected float moveSpeed;

	// Token: 0x040002BA RID: 698
	[SerializeField]
	protected Vector2 aimDirection = Vector2.zero;

	// Token: 0x040002BB RID: 699
	[SerializeField]
	protected BloomMaterialControl bloom;

	// Token: 0x0200014C RID: 332
	protected enum EnumDroneType
	{
		// Token: 0x040009B0 RID: 2480
		MAIN,
		// Token: 0x040009B1 RID: 2481
		GRENADE,
		// Token: 0x040009B2 RID: 2482
		ITEM,
		// Token: 0x040009B3 RID: 2483
		LASER,
		// Token: 0x040009B4 RID: 2484
		TARGET,
		// Token: 0x040009B5 RID: 2485
		LIGHT
	}
}
