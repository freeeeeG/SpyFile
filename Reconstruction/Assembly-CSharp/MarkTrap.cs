using System;

// Token: 0x020001E5 RID: 485
public class MarkTrap : TrapContent
{
	// Token: 0x06000C79 RID: 3193 RVA: 0x00020868 File Offset: 0x0001EA68
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
		base.OnContentPass(enemy, content, index);
		BuffInfo buffInfo = new BuffInfo(EnemyBuffName.TileCountStun, 1 + enemy.DamageStrategy.TrapIntensify + (int)base.TrapIntensify, false);
		enemy.DamageStrategy.ApplyBuff(buffInfo);
		enemy.DamageStrategy.TrapIntensify = 0;
	}
}
