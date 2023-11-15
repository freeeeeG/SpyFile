using System;
using UnityEngine;

// Token: 0x020001DC RID: 476
public class BlinkTrap : TrapContent
{
	// Token: 0x06000C65 RID: 3173 RVA: 0x00020400 File Offset: 0x0001E600
	public override void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
		if (content == null)
		{
			content = this;
		}
		if (((TrapContent)content).BlinkedEnemy.Contains(enemy))
		{
			return;
		}
		base.OnContentPass(enemy, content, index);
		if (!this.BlinkedEnemy.Contains(enemy))
		{
			this.BlinkedEnemy.Add(enemy);
		}
		this.Distance = Mathf.RoundToInt(2f * (1f + base.TrapIntensify + (float)enemy.DamageStrategy.TrapIntensify));
		enemy.DamageStrategy.TrapIntensify = 0;
		enemy.Flash(this.Distance);
	}

	// Token: 0x0400062F RID: 1583
	private int Distance;
}
