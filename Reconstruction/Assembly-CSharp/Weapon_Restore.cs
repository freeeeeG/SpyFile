using System;

// Token: 0x020000E6 RID: 230
public class Weapon_Restore : Weapon
{
	// Token: 0x060005AB RID: 1451 RVA: 0x0000F850 File Offset: 0x0000DA50
	protected override void TriggerWeapon()
	{
		base.TriggerWeapon();
		BuffInfo buffInfo = new BuffInfo(EnemyBuffName.SpeedUpBuff, 1, false);
		base.Knight.DamageStrategy.ApplyBuff(buffInfo);
	}
}
