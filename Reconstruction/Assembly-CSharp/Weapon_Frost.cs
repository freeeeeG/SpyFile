using System;
using UnityEngine;

// Token: 0x020000E5 RID: 229
public class Weapon_Frost : Weapon
{
	// Token: 0x060005A9 RID: 1449 RVA: 0x0000F760 File Offset: 0x0000D960
	protected override void TriggerWeapon()
	{
		base.TriggerWeapon();
		int num = Mathf.Min(3, 1 + (GameRes.CurrentWave + 1) / 20);
		for (int i = 0; i < num; i++)
		{
			Vector2 targetPos = base.transform.position + Random.insideUnitCircle * Mathf.Min(8f, 1.5f + 1f * (float)((GameRes.CurrentWave + 1) / 20));
			Knight_FrostBullet knight_FrostBullet = Singleton<ObjectPool>.Instance.Spawn(this.frostBulletPrefab) as Knight_FrostBullet;
			knight_FrostBullet.transform.position = base.transform.position;
			float frostTime = Mathf.Min(10f, 1f + (float)((GameRes.CurrentWave + 1) / 20));
			float frostRange = Mathf.Min(8f, 1f + (float)((GameRes.CurrentWave + 1) / 20));
			knight_FrostBullet.SetBullet(targetPos, frostRange, frostTime);
		}
	}

	// Token: 0x04000270 RID: 624
	[SerializeField]
	private Knight_FrostBullet frostBulletPrefab;
}
