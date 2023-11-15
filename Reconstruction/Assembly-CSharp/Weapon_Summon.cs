using System;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class Weapon_Summon : Weapon
{
	// Token: 0x060005AD RID: 1453 RVA: 0x0000F888 File Offset: 0x0000DA88
	protected override void TriggerWeapon()
	{
		base.TriggerWeapon();
		EnemyType eType = this.summonTypes[Random.Range(0, this.summonTypes.Length)];
		Singleton<GameManager>.Instance.SpawnEnemy(eType, base.Knight.PointIndex, base.Knight.Intensify / 2f, base.Knight.DmgResist, BoardSystem.shortestPoints);
	}

	// Token: 0x04000271 RID: 625
	[SerializeField]
	private EnemyType[] summonTypes;
}
