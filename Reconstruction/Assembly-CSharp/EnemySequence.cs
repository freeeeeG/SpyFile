using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
[Serializable]
public class EnemySequence
{
	// Token: 0x060005E6 RID: 1510 RVA: 0x000101F5 File Offset: 0x0000E3F5
	public EnemySequence()
	{
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x00010204 File Offset: 0x0000E404
	public EnemySequence(int wave, EnemyType type, int amount, float cooldown, float intensify, bool isBoss, float dmgResit)
	{
		this.Wave = wave;
		this.EnemyType = type;
		this.Amount = amount;
		this.CoolDown = cooldown;
		this.Intensify = intensify;
		this.SpawnTimer = 0f;
		this.IsBoss = isBoss;
		this.count = this.Amount;
		this.DmgResist = dmgResit;
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0001026A File Offset: 0x0000E46A
	public void Initialize()
	{
		this.count = this.Amount;
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x00010278 File Offset: 0x0000E478
	public bool Progress()
	{
		this.SpawnTimer += Time.deltaTime;
		if (this.count > 0)
		{
			if (this.first)
			{
				this.first = false;
				this.coolDown = 0f;
			}
			else
			{
				this.coolDown = this.CoolDown;
			}
			if (this.SpawnTimer >= this.coolDown)
			{
				this.SpawnTimer = 0f;
				this.count--;
				Singleton<GameManager>.Instance.SpawnEnemy(this.EnemyType, 0, this.Intensify, this.DmgResist, BoardSystem.shortestPoints);
			}
			return true;
		}
		this.first = true;
		return false;
	}

	// Token: 0x0400028B RID: 651
	public EnemyType EnemyType;

	// Token: 0x0400028C RID: 652
	public float Intensify;

	// Token: 0x0400028D RID: 653
	public int Amount;

	// Token: 0x0400028E RID: 654
	public float CoolDown;

	// Token: 0x0400028F RID: 655
	public bool IsBoss;

	// Token: 0x04000290 RID: 656
	public int Wave;

	// Token: 0x04000291 RID: 657
	private float SpawnTimer;

	// Token: 0x04000292 RID: 658
	private int count;

	// Token: 0x04000293 RID: 659
	public float DmgResist;

	// Token: 0x04000294 RID: 660
	public bool first = true;

	// Token: 0x04000295 RID: 661
	private float coolDown;
}
