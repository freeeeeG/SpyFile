using System;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class Fission : Solider
{
	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06000614 RID: 1556 RVA: 0x000107E5 File Offset: 0x0000E9E5
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Fission;
		}
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x000107E8 File Offset: 0x0000E9E8
	public override void OnDie()
	{
		if (!this.isOutTroing)
		{
			this.Divide();
		}
		base.OnDie();
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x00010800 File Offset: 0x0000EA00
	private void Divide()
	{
		for (int i = 0; i < 2; i++)
		{
			Singleton<GameManager>.Instance.SpawnEnemy(EnemyType.Fisson_Small, this.PointIndex, this.Intensify / 3f, this.DmgResist, BoardSystem.shortestPoints).Progress = Mathf.Clamp(base.Progress + Random.Range(-0.2f, 0.2f), 0f, 1f);
		}
	}
}
