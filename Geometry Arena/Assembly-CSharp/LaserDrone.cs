using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class LaserDrone : Drone
{
	// Token: 0x06000300 RID: 768 RVA: 0x000127C7 File Offset: 0x000109C7
	private void Awake()
	{
		this.colli_Max = 0.1f;
		this.colli_Cur = 0f;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Update()
	{
	}

	// Token: 0x06000302 RID: 770 RVA: 0x000127DF File Offset: 0x000109DF
	protected override void FixedUpdate()
	{
		if (this.cpu == null && !BattleManager.inst.GameOn)
		{
			base.DestoryThisDrone();
			return;
		}
		this.FixedUpdate_EnemyAndAim();
		this.FixedUpdate_Move();
		this.FixedUpdate_UpdateSmallLaser();
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00012814 File Offset: 0x00010A14
	public override void FixedUpdate_EnemyAndAim()
	{
		float num = Mathf.Sqrt(Player.inst.unit.playerFactorTotal.bulletRng);
		List<Enemy> listEnemies = BattleManager.inst.listEnemies;
		List<Enemy> list = new List<Enemy>();
		if (listEnemies.Count <= 0)
		{
			this.ifTracing = false;
			base.UpdateAngleWhenNoTarget();
			return;
		}
		this.ifTracing = true;
		if (this.targetEnemy == null)
		{
			foreach (Enemy enemy in listEnemies)
			{
				if ((enemy.transform.position - base.transform.position).magnitude < num * 3f)
				{
					list.Add(enemy);
				}
			}
			if (list.Count == 0)
			{
				foreach (Enemy enemy2 in listEnemies)
				{
					if ((enemy2.transform.position - base.transform.position).magnitude < num * 6f)
					{
						list.Add(enemy2);
					}
				}
				if (list.Count == 0)
				{
					foreach (Enemy enemy3 in listEnemies)
					{
						if ((enemy3.transform.position - base.transform.position).magnitude < num * 9f)
						{
							list.Add(enemy3);
						}
					}
				}
			}
			if (list.Count == 0)
			{
				this.targetEnemy = listEnemies.GetRandom<Enemy>();
			}
			else
			{
				this.targetEnemy = list.GetRandom<Enemy>();
			}
		}
		this.aimDirection = this.targetEnemy.transform.position - base.transform.position;
		float z = MyTool.Vec2toAngle180(this.aimDirection);
		base.transform.eulerAngles = new Vector3(0f, 0f, z);
	}

	// Token: 0x06000304 RID: 772 RVA: 0x00012A50 File Offset: 0x00010C50
	protected override void FixedUpdate_Move()
	{
		if (this.ifTracing)
		{
			this.targetPos = this.targetEnemy.transform.position;
		}
		Vector2 vector = this.targetPos - base.transform.position;
		float magnitude = vector.magnitude;
		if (this.ifTracing && magnitude <= this.targetEnemy.transform.localScale.x / 2f)
		{
			return;
		}
		float num = Time.fixedDeltaTime * this.moveSpeed;
		num = Mathf.Min(num, magnitude);
		Vector2 v = vector.normalized * num;
		base.transform.position += v;
	}

	// Token: 0x06000305 RID: 773 RVA: 0x00012B0C File Offset: 0x00010D0C
	private void FixedUpdate_UpdateSmallLaser()
	{
		if (this.smallLaser != null)
		{
			Object.Destroy(this.smallLaser.gameObject);
		}
		if (!this.ifTracing)
		{
			return;
		}
		float num = Mathf.Sqrt(Player.inst.unit.playerFactorTotal.bulletRng);
		float magnitude = (this.targetEnemy.transform.position - base.transform.position).magnitude;
		float num2 = this.targetEnemy.transform.localScale.x / 2f;
		if (magnitude >= (num + num2) * 1.1f)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.prefabSmallLaser, base.transform.position, Quaternion.Euler(new Vector3(0f, 0f, MyTool.Vec2toAngle180(this.aimDirection))));
		this.smallLaser = gameObject.GetComponent<SpecialBullet_SmallLaser>();
		bool ifColli = false;
		this.colli_Cur += Time.fixedDeltaTime;
		if (this.colli_Cur >= this.colli_Max)
		{
			this.colli_Cur -= this.colli_Max;
			ifColli = true;
		}
		this.smallLaser.LaserInit(this.sprBody.color, ifColli, this.aimDirection);
	}

	// Token: 0x040002BC RID: 700
	[SerializeField]
	public bool ifTracing;

	// Token: 0x040002BD RID: 701
	[SerializeField]
	private SpecialBullet_SmallLaser smallLaser;

	// Token: 0x040002BE RID: 702
	[SerializeField]
	private GameObject prefabSmallLaser;

	// Token: 0x040002BF RID: 703
	[SerializeField]
	private float colli_Max = 0.1f;

	// Token: 0x040002C0 RID: 704
	[SerializeField]
	private float colli_Cur;
}
