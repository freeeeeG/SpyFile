using System;

// Token: 0x020001E9 RID: 489
public class WeakTrap : TrapContent
{
	// Token: 0x06000C80 RID: 3200 RVA: 0x00020974 File Offset: 0x0001EB74
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
		base.OnContentPass(enemy, content, index);
		enemy.DamageStrategy.ApplyBuffDmgIntensify(0.05f * (1f + base.TrapIntensify + (float)enemy.DamageStrategy.TrapIntensify));
		enemy.DamageStrategy.TrapIntensify = 0;
	}
}
