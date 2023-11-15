using System;

// Token: 0x020001E8 RID: 488
public class StunTrap : TrapContent
{
	// Token: 0x06000C7E RID: 3198 RVA: 0x00020900 File Offset: 0x0001EB00
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
		base.OnContentPass(enemy, content, index);
		float num = (1f + (float)(enemy.PassedTraps.Count - 1) * 0.1f) * (1f + base.TrapIntensify + (float)enemy.DamageStrategy.TrapIntensify);
		enemy.DamageStrategy.StunTime += num;
		enemy.DamageStrategy.TrapIntensify = 0;
	}
}
