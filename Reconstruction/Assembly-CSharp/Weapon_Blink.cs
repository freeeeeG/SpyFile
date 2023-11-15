using System;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class Weapon_Blink : Weapon
{
	// Token: 0x060005A7 RID: 1447 RVA: 0x0000F724 File Offset: 0x0000D924
	protected override void TriggerWeapon()
	{
		base.TriggerWeapon();
		int num = Mathf.Min(6, 1 + (GameRes.CurrentWave + 1) / 20);
		base.Knight.Flash(-num);
	}
}
