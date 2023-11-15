using System;

// Token: 0x020001DE RID: 478
public class DamageMarkTrap : TrapContent
{
	// Token: 0x06000C69 RID: 3177 RVA: 0x0002053C File Offset: 0x0001E73C
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
		base.OnContentPass(enemy, content, index);
		BuffInfo buffInfo = new BuffInfo(EnemyBuffName.TileBaseDamageIntensify, 1 + (int)base.TrapIntensify + enemy.DamageStrategy.TrapIntensify, false);
		enemy.DamageStrategy.ApplyBuff(buffInfo);
	}
}
