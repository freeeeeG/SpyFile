using System;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class InvisibleBuff : TileBuff
{
	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000159 RID: 345 RVA: 0x00006A54 File Offset: 0x00004C54
	public override EnemyBuffName BuffName
	{
		get
		{
			return EnemyBuffName.Invisible;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x0600015A RID: 346 RVA: 0x00006A58 File Offset: 0x00004C58
	public override float BasicDuration
	{
		get
		{
			return 999f;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x0600015B RID: 347 RVA: 0x00006A5F File Offset: 0x00004C5F
	public override float KeyValue
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x0600015C RID: 348 RVA: 0x00006A66 File Offset: 0x00004C66
	public override bool IsTimeBase
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x0600015D RID: 349 RVA: 0x00006A69 File Offset: 0x00004C69
	public override bool IsStackable
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00006A6C File Offset: 0x00004C6C
	public override void Tick(float delta)
	{
		base.Tick(delta);
		this.timeCounter += delta;
		if (this.timeCounter > this.KeyValue)
		{
			int num = Physics2D.OverlapCircleNonAlloc(this.Target.transform.position, 3f, this.hitResult, StaticData.TurretLayerMask);
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				if (this.hitResult[i].GetComponent<RefactorTurret>())
				{
					flag = true;
					break;
				}
			}
			this.Target.DamageStrategy.InVisible = !flag;
			this.timeCounter = 0f;
		}
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00006B11 File Offset: 0x00004D11
	public override void Affect(int stacks)
	{
		this.Target.DamageStrategy.InVisible = true;
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00006B24 File Offset: 0x00004D24
	public override void End()
	{
	}

	// Token: 0x04000118 RID: 280
	private float timeCounter;

	// Token: 0x04000119 RID: 281
	private Collider2D[] hitResult = new Collider2D[10];
}
